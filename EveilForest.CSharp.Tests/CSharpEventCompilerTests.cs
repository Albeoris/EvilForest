using System;
using System.IO;
using EveilForest.CSharp;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public class CSharpEventCompilerTests : IDisposable
{
    private readonly CSharpEventCompiler _compiler;
    private readonly string _testDirectory;

    public CSharpEventCompilerTests()
    {
        _compiler = new CSharpEventCompiler();
        _testDirectory = Path.Combine(Path.GetTempPath(), $"evilforest_tests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void CompileDirectory_ValidCSharpCode_CompilesToBytecode()
    {
        // Arrange
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
}";
        WriteTestFile("152_TestObject.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Single(objects);
        
        var obj = objects[0];
        Assert.Equal(152, obj.Id);
        Assert.Equal(0, obj.VariableCount);
        Assert.Single(obj.Scripts);
        
        var script = obj.Scripts[0];
        Assert.Equal(0, script.Id); // Init method maps to script ID 0
    }

    [Fact]
    public void CompileDirectory_MultipleScripts_CompilesAllScripts()
    {
        // Arrange
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
    
    public IEnumerable<IAwaitable> OnLoop()
    {
        yield return @actor.Wait(frames: 60);
    }
}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Single(objects);
        
        var obj = objects[0];
        Assert.Equal(1, obj.Id);
        Assert.Equal(2, obj.Scripts.Length);
        
        // Check script IDs
        var scriptIds = obj.Scripts.Select(s => s.Id).OrderBy(x => x).ToArray();
        Assert.Equal(new int[] { 0, 1 }, scriptIds); // Init=0, OnLoop=1
    }

    [Fact]
    public void CompileDirectory_InvalidObjectFileName_IgnoresFile()
    {
        // Arrange
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
}";
        WriteTestFile("InvalidFileName.cs", csharpCode); // Missing numeric prefix - should be ignored

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert - File should be ignored, not compiled
        Assert.Empty(objects);
    }

    [Fact]
    public void CompileDirectory_EmptyDirectory_ReturnsEmptyArray()
    {
        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Empty(objects);
    }

    [Fact]
    public void CompileDirectory_MultipleObjects_CompilesAll()
    {
        // Arrange
        const string object1Code = @"public sealed class TestObject1
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 1, ui: 64, text: 10);
    }
}";
        const string object2Code = @"public sealed class TestObject2
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @actor.Wait(frames: 30);
    }
}";
        WriteTestFile("01_TestObject1.cs", object1Code);
        WriteTestFile("02_TestObject2.cs", object2Code);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Equal(2, objects.Length);
        
        var objectIds = objects.Select(o => o.Id).OrderBy(x => x).ToArray();
        Assert.Equal(new int[] { 1, 2 }, objectIds);
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