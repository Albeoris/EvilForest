using EveilForest.CSharp;
using Memoria.EventEngine.EV;
using Xunit;
using static EveilForest.CSharp.Tests.SourceProjectTest;

namespace EveilForest.CSharp.Tests;

public sealed class SemanticCompilerTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-legacy-{Guid.NewGuid():N}");

    public SemanticCompilerTests()
    {
        Directory.CreateDirectory(_directory);
        WriteProject(_directory);
        File.WriteAllText(Path.Combine(_directory, "Support.cs"), """
            public sealed class VmServices
            {
                public object Wait(int frameDuration) => new object();
                public object DestroyEverything(int value) => new object();
            }
            public sealed class ServiceId
            {
                public static ServiceId System { get; } = new ServiceId();
                public static ServiceId Unknown { get; } = new ServiceId();
                public VmServices this[object ctx] => new VmServices();
            }
            """);
    }

    [Fact]
    public void CompileDirectory_WaitInstruction_ProducesExpectedInstruction()
    {
        const string source = """
            public sealed class TestObject
            {
                public System.Collections.Generic.IEnumerable<object> Init(object ctx)
                {
                    var sys = ServiceId.System[ctx];
                    yield return sys.Wait(frameDuration: 2); // WAIT(99)
                    yield break;
                }
            }
            """;
        File.WriteAllText(Path.Combine(_directory, "05_TestObject.cs"), source);

        foreach (string variant in BuildBoth())
        {
            EVObject result = Assert.Single(new CSharpEventCompiler().CompileDirectory(variant));
            Assert.Equal(5, result.Id);
            Assert.Equal(0, result.VariableCount);
            Assert.Equal(0, result.Flags);
            EVScript script = Assert.Single(result.Scripts);
            Assert.Equal(0, script.Id);
            Assert.Equal("WAIT(_frameDuration: 2)", script.Segment.EnumerateAllInstruction().First().ToString());
            Assert.Equal(Convert.FromHexString("22000204"), script.Instructions.Encode());
        }
    }

    [Fact]
    public void CompileDirectory_UnknownYieldInstruction_ThrowsInsteadOfWritingNop()
    {
        const string source = """
            public sealed class TestObject
            {
                public System.Collections.Generic.IEnumerable<object> Init(object ctx)
                {
                    var unknown = ServiceId.Unknown[ctx];
                    yield return unknown.DestroyEverything(value: 42); // WAIT(42)
                }
            }
            """;
        File.WriteAllText(Path.Combine(_directory, "01_TestObject.cs"), source);

        foreach (string variant in BuildBoth())
        {
            InvalidDataException error = Assert.Throws<InvalidDataException>(() => new CSharpEventCompiler().CompileDirectory(variant));
            Assert.Contains("Unsupported yield-return", error.ToString());
            Assert.Contains("unknown.DestroyEverything(value: 42)", error.ToString());
        }
    }

    [Fact]
    public void CompileDirectory_Assignment_IsParsedFromSyntaxTree()
    {
        const string source = """
            public sealed class TestObject
            {
                public byte Byte_0 { get; set; }
                public System.Collections.Generic.IEnumerable<object> Init()
                {
                    Byte_0 = 7;
                    yield break;
                }
            }
            """;
        File.WriteAllText(Path.Combine(_directory, "03_TestObject.cs"), source);

        foreach (string variant in BuildBoth())
        {
            EVObject result = Assert.Single(new CSharpEventCompiler().CompileDirectory(variant));
            Assert.Equal(1, result.VariableCount);
            EVScript script = Assert.Single(result.Scripts);
            Assert.Equal("Let(Byte_0 = 7)", script.Segment.EnumerateAllInstruction().First().ToString());
            Assert.Equal(Convert.FromHexString("05D6007D07002C7F04"), script.Instructions.Encode());
        }
    }

    private IEnumerable<string> BuildBoth()
    {
        foreach (string variant in new[] { _directory, CopyWithoutComments(_directory) })
        {
            RunDotnet(new[] { "build", ProjectFile(variant), "--nologo", "-v:q" });
            yield return variant;
        }
    }

    public void Dispose()
    {
        Directory.Delete(_directory, recursive: true);
        if (Directory.Exists(_directory + "-stripped"))
            Directory.Delete(_directory + "-stripped", recursive: true);
    }
}
