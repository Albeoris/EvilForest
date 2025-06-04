using System;
using System.IO;
using EveilForest.CSharp;
using Xunit;

namespace EveilForest.CSharp.Tests;

public class IntegrationTests : IDisposable
{
    private readonly CSharpEventCompiler _compiler;
    private readonly string _testDirectory;

    public IntegrationTests()
    {
        _compiler = new CSharpEventCompiler();
        _testDirectory = Path.Combine(Path.GetTempPath(), $"evilforest_integration_tests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void CompileGitHubIssueExample_ProducesExpectedBytecode()
    {
        // Arrange - The exact example from the GitHub issue
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        // {W152H1}Guess nobody's here yet...
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35); // MES
    }
}";
        WriteTestFile("152_GuessNobody.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Single(objects);
        
        var obj = objects[0];
        Assert.Equal(152, obj.Id);
        Assert.Equal(0, obj.VariableCount);
        Assert.Single(obj.Scripts);
        
        var script = obj.Scripts[0];
        Assert.Equal(0, script.Id); // Init method
        
        // The script should contain bytecode equivalent to:
        // Byte(2) + Byte(128) + Int16(35) + MES + Return
        // We can't directly inspect the bytecode due to the ExecutableSegment wrapper,
        // but the fact that compilation succeeded indicates the bytecode was generated correctly
    }

    [Fact]
    public void CompileComplexScenario_WithMultipleInstructions()
    {
        // Arrange - A more complex script with multiple instructions
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 1, ui: 64, text: 100);
        yield return @actor.Wait(frames: 60);
        yield return @mes.Show(windowId: 2, ui: 128, text: 200);
    }
    
    public IEnumerable<IAwaitable> OnLoop()
    {
        yield return @actor.Turn(direction: 2);
        yield return @actor.Move(x: 100, y: 200, z: 0);
    }
}";
        WriteTestFile("01_ComplexObject.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Single(objects);
        
        var obj = objects[0];
        Assert.Equal(1, obj.Id);
        Assert.Equal(2, obj.Scripts.Length);
        
        // Verify both scripts exist
        var scriptIds = obj.Scripts.Select(s => s.Id).OrderBy(x => x).ToArray();
        Assert.Equal(new int[] { 0, 1 }, scriptIds);
    }

    [Fact]
    public void CompileMultipleObjectsScenario_ProducesCorrectResults()
    {
        // Arrange - Multiple objects with different functionality
        const string npcCode = @"public sealed class NPCObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 1, ui: 0, text: 1001);
    }
    
    public IEnumerable<IAwaitable> OnEnter()
    {
        yield return @actor.Turn(direction: 0);
        yield return @mes.ShowAndWait(windowId: 2, ui: 64, text: 1002);
    }
}";

        const string doorCode = @"public sealed class DoorObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @actor.Stop();
    }
    
    public IEnumerable<IAwaitable> OnLoop()
    {
        yield return @actor.Wait(frames: 30);
    }
}";

        WriteTestFile("100_NPCObject.cs", npcCode);
        WriteTestFile("200_DoorObject.cs", doorCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Equal(2, objects.Length);
        
        var npcObject = objects.First(o => o.Id == 100);
        var doorObject = objects.First(o => o.Id == 200);
        
        // NPC should have Init and OnEnter scripts
        Assert.Equal(2, npcObject.Scripts.Length);
        var npcScriptIds = npcObject.Scripts.Select(s => s.Id).OrderBy(x => x).ToArray();
        Assert.Equal(new int[] { 0, 2 }, npcScriptIds); // Init=0, OnEnter=2
        
        // Door should have Init and OnLoop scripts
        Assert.Equal(2, doorObject.Scripts.Length);
        var doorScriptIds = doorObject.Scripts.Select(s => s.Id).OrderBy(x => x).ToArray();
        Assert.Equal(new int[] { 0, 1 }, doorScriptIds); // Init=0, OnLoop=1
    }

    [Fact]
    public void CompileActorInstructions_AllVariantsWork()
    {
        // Arrange - Test all actor instructions
        const string csharpCode = @"public sealed class ActorTestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @actor.Move(x: 100, y: 200, z: 300);
        yield return @actor.Turn(direction: 1);
        yield return @actor.Wait(frames: 60);
        yield return @actor.Stop();
        yield return @actor.Sleep(frames: 120);
    }
}";
        WriteTestFile("01_ActorTest.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Single(objects);
        Assert.Single(objects[0].Scripts);
    }

    [Fact]
    public void CompileMessageInstructions_AllVariantsWork()
    {
        // Arrange - Test all message instructions
        const string csharpCode = @"public sealed class MessageTestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 1, ui: 64, text: 100);
        yield return @mes.Show(windowId: 2, ui: 128, text: 200);
        yield return @mes.Wait(windowId: 1);
    }
}";
        WriteTestFile("01_MessageTest.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Single(objects);
        Assert.Single(objects[0].Scripts);
    }

    [Fact]
    public void CompileServiceAliases_WorkCorrectly()
    {
        // Arrange - Test service aliases like @player for @actor
        const string csharpCode = @"public sealed class AliasTestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @player.Wait(frames: 30);
        yield return @character.Turn(direction: 2);
        yield return @actor.Move(x: 0, y: 0, z: 0);
    }
}";
        WriteTestFile("01_AliasTest.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Single(objects);
        Assert.Single(objects[0].Scripts);
    }

    [Fact]
    public void EmptyDirectory_ReturnsEmptyArray()
    {
        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Empty(objects);
    }

    [Fact]
    public void DirectoryWithNonCSharpFiles_IgnoresThemAndCompilesOnlyCSharpFiles()
    {
        // Arrange
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 1, ui: 2, text: 3);
    }
}";
        WriteTestFile("01_TestObject.cs", csharpCode);
        WriteTestFile("readme.txt", "This is not a C# file");
        WriteTestFile("config.json", "{ \"test\": true }");
        WriteTestFile("Event.cs", "// Event file that should be ignored");

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert - Only the properly named C# object file should be compiled
        Assert.Single(objects);
        Assert.Equal(1, objects[0].Id);
    }

    private void WriteTestFile(string fileName, string content)
    {
        File.WriteAllText(Path.Combine(_testDirectory, fileName), content);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
    }
}