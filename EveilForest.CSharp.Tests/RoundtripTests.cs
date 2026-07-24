using System;
using System.IO;
using System.Linq;
using EveilForest.CSharp;
using Memoria.EventEngine.EV;
using FF8.JSM;
using FF8.JSM.Format;
using Xunit;
using Xunit.Abstractions;

namespace EveilForest.CSharp.Tests;

public sealed class RoundtripTests : IDisposable
{
    private readonly string _tempDir;
    private readonly ITestOutputHelper _out;

    public RoundtripTests(ITestOutputHelper output)
    {
        _out     = output;
        _tempDir = Path.Combine(Path.GetTempPath(), $"evilforest_rt_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        if (Environment.GetEnvironmentVariable("EVILFOREST_KEEP_TEST_OUTPUT") == "1")
        {
            _out.WriteLine($"Kept generated files in: {_tempDir}");
            return;
        }

        if (Directory.Exists(_tempDir))
            try { Directory.Delete(_tempDir, recursive: true); } catch { }
    }

    [Fact]
    public void EVT_ALEX1_TS_CARGO_0_CorpusCompilesAndWritesValidFile()
    {
        string ebPath = Path.Combine(
            Path.GetDirectoryName(typeof(RoundtripTests).Assembly.Location)!,
            "TestData", "EVT_ALEX1_TS_CARGO_0.eb.bytes");

        Assert.True(File.Exists(ebPath), $"Test data not found: {ebPath}");

        EVObject[] original = EVFileReader.Read(ebPath);
        _out.WriteLine($"Loaded {original.Length} objects");

        DecompileToCSharp(original, _tempDir);
        _out.WriteLine($"Generated sources: {_tempDir}");
        var files = Directory.GetFiles(_tempDir, "*.cs");
        _out.WriteLine($"Generated {files.Length} C# files");

        var compiler   = new CSharpEventCompiler();
        EVObject[] recompiled = compiler.CompileDirectory(_tempDir);
        _out.WriteLine($"Compiled back {recompiled.Length} objects");

        string recompiledPath = Path.Combine(_tempDir, "recompiled.eb.bytes");
        EVFileWriter.Write(recompiledPath, recompiled);

        AssertSameStructure(original, recompiled);
        // Prove that the writer produced a structurally valid file, not merely a
        // collection of in-memory segments that happens to compare successfully.
        AssertSameStructure(original, EVFileReader.Read(recompiledPath));
    }

    private static void DecompileToCSharp(EVObject[] objects, string dir)
    {
        var ctx = DummyFormatterContext.Instance;
        var svc = Memoria.EventEngine.Execution.StatelessServices.Instance;
        foreach (var obj in objects)
        {
            var sw = new ScriptWriter();
            string typeName = obj.GetObjectName(ctx);
            obj.FormatType(sw, typeName, ctx, svc);
            File.WriteAllText(Path.Combine(dir, $"{obj.Id:D2}_{typeName}.cs"), sw.Release());
        }
    }

    private void AssertSameStructure(EVObject[] expected, EVObject[] actual)
    {
        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            var e = expected[i]; var a = actual[i];
            Assert.Equal(e.Id,            a.Id);
            Assert.Equal(e.VariableCount, a.VariableCount);
            Assert.Equal(e.Flags,         a.Flags);
            Assert.Equal(e.Scripts.Length, a.Scripts.Length);
            for (int j = 0; j < e.Scripts.Length; j++)
            {
                Assert.Equal(e.Scripts[j].Id, a.Scripts[j].Id);
                var ei = e.Scripts[j].Segment.EnumerateAllInstruction().ToArray();
                var ai = a.Scripts[j].Segment.EnumerateAllInstruction().ToArray();
                Assert.Equal(ei.Length == 0, ai.Length == 0);
            }
        }
        _out.WriteLine("Corpus compile/write/read validation PASSED");
    }
}
