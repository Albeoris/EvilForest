using System;
using System.IO;
using System.Linq;
using EveilForest.CSharp;
using Memoria.EventEngine.EV;
using Xunit;
using FF8.JSM.Format;
using Memoria.EventEngine.Execution;
using EvilForest.Resources;
using System.Collections.Generic;
using System.Reflection;

namespace EveilForest.CSharp.Tests;

public class RoundtripTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly CSharpEventCompiler _compiler;

    public RoundtripTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), $"evilforest_roundtrip_tests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
        _compiler = new CSharpEventCompiler();
    }

    [Fact]
    public void EVT_ALEX1_TS_CARGO_0_RoundtripTest()
    {
        // Arrange - Get the test data file path
        string testDataFile = Path.Combine(Path.GetDirectoryName(typeof(RoundtripTests).Assembly.Location)!, 
            "TestData", "EVT_ALEX1_TS_CARGO_0.eb.bytes");
        
        Assert.True(File.Exists(testDataFile), $"Test data file not found: {testDataFile}");

        // Step 1: Read original .eb.bytes file and convert to C# files
        EVObject[] originalObjects = ConvertEbBytesToCSharpFiles(testDataFile);
        
        // Debug: Print first few instructions
        if (originalObjects.Length > 0 && originalObjects[0].Scripts.Length > 0)
        {
            var instructions = originalObjects[0].Scripts[0].Segment.EnumerateAllInstruction().ToArray();
            Console.WriteLine($"\nOriginal first script has {instructions.Length} instructions:");
            for (int i = 0; i < Math.Min(5, instructions.Length); i++)
            {
                var instr = instructions[i];
                Console.WriteLine($"[{i}] = {{{instr.GetType().Name}}} {instr}");
            }
        }
        
        // Verify files were generated
        var generatedFiles = Directory.GetFiles(_testDirectory, "*.cs");
        Assert.Equal(originalObjects.Length, generatedFiles.Length);
        
        // Step 2: Compile the C# files back to EVObjects
        EVObject[] recompiledObjects = _compiler.CompileDirectory(_testDirectory);
        
        // Debug: Print recompiled instructions
        if (recompiledObjects.Length > 0 && recompiledObjects[0].Scripts.Length > 0)
        {
            var instructions = recompiledObjects[0].Scripts[0].Segment.EnumerateAllInstruction().ToArray();
            Console.WriteLine($"\nRecompiled first script has {instructions.Length} instructions:");
            for (int i = 0; i < Math.Min(5, instructions.Length); i++)
            {
                var instr = instructions[i];
                Console.WriteLine($"[{i}] = {{{instr.GetType().Name}}} {instr}");
            }
        }
        
        // Step 3: Write recompiled objects to a new .eb.bytes file
        string recompiledFile = Path.Combine(_testDirectory, "recompiled.eb.bytes");
        WriteEVObjectsToFile(recompiledObjects, recompiledFile);
        
        // Step 4: Compare the results
        AssertObjectsAreEquivalent(originalObjects, recompiledObjects);
    }

    private EVObject[] ConvertEbBytesToCSharpFiles(string ebBytesPath)
    {
        // Read the original file
        EVObject[] objects = EVFileReader.Read(ebBytesPath);

        // Create a dummy formatter context for converting to C# files
        var formatterContext = new TestFormatterContext();
        var scriptWriter = new ScriptWriter();

        // Convert each object to C# files similar to Program.Main
        for (int i = 0; i < objects.Length; i++)
        {
            EVObject obj = objects[i];
            string typeName = obj.GetObjectName(formatterContext);
            string fileName = Path.Combine(_testDirectory, $"{obj.Id:D2}_{typeName}.cs");
            
            obj.FormatType(scriptWriter, typeName, formatterContext, StatelessServices.Instance);
            string result = scriptWriter.Release();

            File.WriteAllText(fileName, result);
        }

        return objects;
    }

    private void WriteEVObjectsToFile(EVObject[] objects, string outputPath)
    {
        EVFileWriter.Write(outputPath, objects);
    }

    private void AssertObjectsAreEquivalent(EVObject[] expected, EVObject[] actual)
    {
        Assert.Equal(expected.Length, actual.Length);

        for (int i = 0; i < expected.Length; i++)
        {
            var expectedObj = expected[i];
            var actualObj = actual[i];

            Assert.Equal(expectedObj.Id, actualObj.Id);
            Assert.Equal(expectedObj.VariableCount, actualObj.VariableCount);
            Assert.Equal(expectedObj.Flags, actualObj.Flags);
            Assert.Equal(expectedObj.Scripts.Length, actualObj.Scripts.Length);

            for (int j = 0; j < expectedObj.Scripts.Length; j++)
            {
                var expectedScript = expectedObj.Scripts[j];
                var actualScript = actualObj.Scripts[j];

                Assert.Equal(expectedScript.Id, actualScript.Id);
                
                // Compare script content/bytecode
                CompareScriptBytecode(expectedScript, actualScript, expectedObj.Id, j);
            }
        }
    }

    private void CompareScriptBytecode(EVScript expected, EVScript actual, int objectId, int scriptIndex)
    {
        // Get instructions from both scripts to compare
        var expectedInstructions = expected.Segment.EnumerateAllInstruction().ToArray();
        var actualInstructions = actual.Segment.EnumerateAllInstruction().ToArray();
        
        // Both scripts should have the same number of instructions
        Assert.Equal(expectedInstructions.Length, actualInstructions.Length);
        
        // If both are empty, that's fine
        if (expectedInstructions.Length == 0 && actualInstructions.Length == 0)
        {
            return;
        }
        
        // Compare each instruction
        for (int i = 0; i < expectedInstructions.Length; i++)
        {
            var expectedInstr = expectedInstructions[i];
            var actualInstr = actualInstructions[i];
            
            // Instructions should be of the same type and have the same opcode
            Assert.Equal(expectedInstr.GetType(), actualInstr.GetType());
            
            // For more detailed comparison, we can compare the instruction's string representation
            // This will catch differences in arguments and instruction details
            Assert.Equal(expectedInstr.ToString(), actualInstr.ToString());
        }
    }

    private byte[] GetScriptBytecode(EVScript script)
    {
        // This method is deprecated in favor of instruction-level comparison
        // Keeping it for potential future use
        return new byte[0];
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
    }

    // Simple test formatter context for converting EVObjects to C#
    private sealed class TestFormatterContext : IScriptFormatterContext
    {
        public DBEvent Event { get; } = CreateTestEvent();

        private static DBEvent CreateTestEvent()
        {
            // Create a minimal test event
            return new DBEvent
            {
                FileName = "TestEvent"
            };
        }

        public string GetScriptName(DBScriptName.Id id)
        {
            return id switch
            {
                (DBScriptName.Id)0 => "Init",
                (DBScriptName.Id)1 => "OnLoop", 
                (DBScriptName.Id)2 => "OnEnter",
                (DBScriptName.Id)3 => "OnExit",
                _ => $"Script_{(int)id:D2}"
            };
        }

        public string GetMessage(DBFieldMessage.Id messageIndex)
        {
            return $"Message_{(int)messageIndex}";
        }

        public DBModel GetModel(DBModel.Id modelId)
        {
            return new DBModel { FileName = $"Model_{(int)modelId}", DisplayName = $"Model_{(int)modelId}" };
        }

        public DBAnimation GetAnimation(DBAnimation.Id animationId)
        {
            return new DBAnimation { FileName = $"Animation_{(int)animationId}" };
        }

        public DBMusic GetMusic(DBMusic.Id musicId)
        {
            return new DBMusic { FileName = $"Music_{(int)musicId}", DisplayName = $"Music_{(int)musicId}" };
        }

        public DBSong GetSong(DBSong.Id songId)
        {
            return new DBSong { FileName = $"Song_{(int)songId}", DisplayName = $"Song_{(int)songId}" };
        }

        public DBSfx GetSfx(DBSfx.Id sfxId)
        {
            return new DBSfx { FileName = $"Sfx_{(int)sfxId}", DisplayName = $"Sfx_{(int)sfxId}" };
        }
    }
}