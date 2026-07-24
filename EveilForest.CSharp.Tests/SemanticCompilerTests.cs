using System;
using System.IO;
using System.Linq;
using EveilForest.CSharp;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class SemanticCompilerTests : IDisposable
{
    private readonly string _directory = Path.Combine(
        Path.GetTempPath(), $"evilforest_compiler_{Guid.NewGuid():N}");

    public SemanticCompilerTests() => Directory.CreateDirectory(_directory);

    [Fact]
    public void CompileDirectory_WaitInstruction_ProducesExpectedInstruction()
    {
        const string source = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        var @sys = ServiceId.System[@ctx];
        yield return @sys.Wait(frameDuration: 2);
        yield break;
    }
}";
        File.WriteAllText(Path.Combine(_directory, "05_TestObject.cs"), source);

        EVObject result = Assert.Single(new CSharpEventCompiler().CompileDirectory(_directory));

        Assert.Equal(5, result.Id);
        Assert.Equal(0, result.VariableCount);
        Assert.Equal(0, result.Flags);
        EVScript script = Assert.Single(result.Scripts);
        Assert.Equal(0, script.Id);
        Assert.Equal("WAIT(_frameDuration: 2)",
            script.Segment.EnumerateAllInstruction().First().ToString());
    }

    [Fact]
    public void CompileDirectory_UnknownYieldInstruction_ThrowsInsteadOfWritingNop()
    {
        const string source = @"public sealed class TestObject
{
    public void Init()
    {
        yield return @unknown.DestroyEverything(value: 42);
    }
}";
        File.WriteAllText(Path.Combine(_directory, "01_TestObject.cs"), source);

        InvalidDataException error = Assert.Throws<InvalidDataException>(
            () => new CSharpEventCompiler().CompileDirectory(_directory));

        Assert.Contains("Unsupported yield-return", error.ToString());
        Assert.Contains("must never silently alter", error.ToString());
    }

    [Fact]
    public void CompileDirectory_Assignment_IsParsedFromSyntaxTree()
    {
        const string source = @"public sealed class TestObject
{
    public Byte Byte_0 { get; set; }
    public IEnumerable<IAwaitable> Init()
    {
        Byte_0 = 7;
        yield break;
    }
}";
        File.WriteAllText(Path.Combine(_directory, "03_TestObject.cs"), source);

        EVObject result = Assert.Single(new CSharpEventCompiler().CompileDirectory(_directory));
        Assert.Equal(1, result.VariableCount);
        Assert.Equal("Let(Byte_0 = 7)",
            Assert.Single(result.Scripts).Segment.EnumerateAllInstruction().First().ToString());
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }
}
