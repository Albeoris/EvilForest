using System.Reflection;
using EveilForest.CSharp;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class SourceMemoryTests
{
    [Theory]
    [InlineData(EVStorageScope.Map)]
    [InlineData(EVStorageScope.Instance)]
    public void AFieldOffsetDrivesManagedAndBytecodeAccess(EVStorageScope scope)
    {
        var byteView = new EVVariable(scope, EVStorageType.Byte, 12);
        var bitView = new EVVariable(scope, EVStorageType.Bit, 97);
        var byteWrite = new EVInstruction(0, Jsm.Opcode.EXPR)
        {
            Expression = [byteView.Encode(), EVExpressionToken.Literal(2), new EVExpressionToken((byte)Jsm.Excode.Let)]
        };
        var bitWrite = new EVInstruction(1, Jsm.Opcode.EXPR)
        {
            Expression = [bitView.Encode(), EVExpressionToken.Literal(0), new EVExpressionToken((byte)Jsm.Excode.Let)]
        };
        var body = new EVInstructionBody([byteWrite, bitWrite, new EVInstruction(2, Jsm.Opcode.Return)], 3);
        var header = new byte[EVContainer.HeaderSize];
        new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
        var slot = new EVContainerObject(0, 32, 0, 0) { BodyOrder = 0, Scripts = [new EVContainerScript(0, body.Encode())] };
        var container = new EVContainer(header, [slot], []);
        string project = Path.Combine(Path.GetTempPath(), $"evilforest-map-layout-{Guid.NewGuid():N}");
        try
        {
            EventProjectExporter.Export(container, "EVT_MEMORY_LAYOUT", project);
            Assert.Equal(container.Serialize(), EventProjectCompiler.Compile(project).Serialize());
            bool map = scope == EVStorageScope.Map;
            string sourceFile = Path.Combine(project, map ? "Event.cs" : "00_Object_00.cs");
            string source = File.ReadAllText(sourceFile);
            string declaration = $"FieldOffset(12)]{Environment.NewLine}{new string(' ', map ? 8 : 12)}internal ByteCell";
            Assert.Contains(declaration, source);
            File.WriteAllText(sourceFile, source.Replace(declaration, declaration.Replace("(12)", "(20)", StringComparison.Ordinal), StringComparison.Ordinal));

            EVContainer rebuilt = EventProjectCompiler.Compile(project);
            EVInstructionBody rebuiltBody = EVInstructionBody.Decode(rebuilt.Objects[0].Scripts[0].Body);
            Assert.Equal(20, EVVariable.Decode(rebuiltBody.Instructions[0].Expression[0]).Address);
            Assert.Equal(97, EVVariable.Decode(rebuiltBody.Instructions[1].Expression[0]).Address);

            var assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
            Type location = assembly.GetType(map ? "Event_EVT_MEMORY_LAYOUT.Location" : "Event_EVT_MEMORY_LAYOUT.Object_00+LocalMemory")!;
            object instance = Activator.CreateInstance(location, nonPublic: true)!;
            var buffer = new byte[80];
            using (new global::EvilForest.EventSource.ScriptMemory([], map ? buffer : [], map ? [] : buffer).Enter())
            {
                location.GetProperty("Byte_12")!.SetValue(instance, 65);
                location.GetProperty("Bit_97")!.SetValue(instance, true);
                Assert.Equal(65, location.GetProperty("Byte_12")!.GetValue(instance));
            }
            Assert.Equal(2, buffer[12]);
            Assert.Equal(65, buffer[20]);
        }
        finally
        {
            if (Directory.Exists(project))
                Directory.Delete(project, recursive: true);
        }
    }

    [Theory]
    [InlineData("Map", 78, 79, 80)]
    [InlineData("Instance", 2, 3, 4)]
    public void DeclaredStorageMustFitAtTheEndOfItsScope(string scope, int exactAddress, int overflowAddress, int capacity)
    {
        string project = Path.Combine(Path.GetTempPath(), $"evilforest-capacity-{Guid.NewGuid():N}");
        try
        {
            var header = new byte[128];
            new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
            var slot = new EVContainerObject(0, 4, 0, 0) { BodyOrder = 0, Scripts = new[] { new EVContainerScript(0, new byte[] { 4 }) } };
            var original = new EVContainer(header, new[] { slot }, Array.Empty<byte>());
            EventProjectExporter.Export(original, "CapacityTest", project);
            string path = Directory.GetFiles(project, "00_*.cs").Single();
            string source = File.ReadAllText(path);
            int start = source.IndexOf('{');
            Assert.True(start >= 0);
            CompileAt(exactAddress);
            Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
            CompileAt(overflowAddress);
            var error = Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project));
            Assert.Contains($"NewField at {scope} address {overflowAddress} exceeds its {capacity}-byte allocation", error.Message);

            void CompileAt(int address)
            {
                string member = $$"""

                        private static class UserStorage
                        {
                            [StorageView(StorageScope.{{scope}}, StorageType.UInt16, {{address}})]
                            public static int NewField { get; set; }
                        }
                    """;
                File.WriteAllText(path, source.Insert(start + 1, member));
            }
        }
        finally
        {
            if (Directory.Exists(project))
                Directory.Delete(project, recursive: true);
        }
    }

    [Fact]
    public void NewInstanceFieldUsesSpaceLeftByPinnedViews()
    {
        string project = Path.Combine(Path.GetTempPath(), $"evilforest-new-field-{Guid.NewGuid():N}");
        try
        {
            var header = new byte[128];
            new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
            var slot = new EVContainerObject(0, 2, 0, 0) { BodyOrder = 0, Scripts = new[] { new EVContainerScript(0, new byte[] { 4 }) } };
            EventProjectExporter.Export(new EVContainer(header, new[] { slot }, Array.Empty<byte>()), "AllocationTest", project);
            string path = Directory.GetFiles(project, "00_*.cs").Single();
            string source = File.ReadAllText(path);
            int start = source.IndexOf('{');
            Assert.True(start >= 0);
            const string fields = """

                    private static class UserStorage
                    {
                        [StorageView(StorageScope.Instance, StorageType.Byte, 0)]
                        public static int ExistingField { get; set; }

                        [StorageField(StorageScope.Instance, StorageType.Byte)]
                        public static int Counter { get; set; }
                    }

                """;
            string edited = source.Insert(start + 1, fields).Replace("return;", $"UserStorage.Counter = 5;{Environment.NewLine}        return;", StringComparison.Ordinal);
            File.WriteAllText(path, edited);
            VerifyAddress(1);

            File.WriteAllText(path, edited.Replace("StorageType.Byte, 0", "StorageType.Byte, 1", StringComparison.Ordinal));
            VerifyAddress(0);

            var error = Assert.Throws<InvalidDataException>(() =>
            {
                File.WriteAllText(path, edited.Replace("StorageType.Byte, 0", "StorageType.UInt16, 0", StringComparison.Ordinal));
                EventProjectCompiler.Compile(project);
            });
            Assert.Contains("No 8-bit storage is available for Counter", error.Message);

            void VerifyAddress(int address)
            {
                EVContainer result = EventProjectCompiler.Compile(project);
                EVInstruction expression = EVInstructionBody.Decode(result.Objects[0].Scripts[0].Body).Instructions[0];
                EVVariable variable = EVVariable.Decode(expression.Expression[0]);
                Assert.Equal(EVStorageScope.Instance, variable.Scope);
                Assert.Equal(EVStorageType.Byte, variable.Type);
                Assert.Equal(address, variable.Address);

                var assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
                Type storage = assembly.GetTypes().Single(type => type.Name == "UserStorage");
                PropertyInfo counter = storage.GetProperty("Counter")!;
                var first = new byte[4];
                var second = new byte[4];
                byte[] map = new byte[80];
                using (new global::EvilForest.EventSource.ScriptMemory(new byte[16], map, first).Enter())
                    counter.SetValue(null, 12);
                using (new global::EvilForest.EventSource.ScriptMemory(new byte[16], map, second).Enter())
                    Assert.Equal(0, counter.GetValue(null));
                Assert.Equal(12, first[address]);
            }
        }
        finally
        {
            if (Directory.Exists(project))
                Directory.Delete(project, recursive: true);
        }
    }

    [Fact]
    public void LocationAllocationSkipsAnEngineReservedBit()
    {
        string project = Path.Combine(Path.GetTempPath(), $"evilforest-reserved-field-{Guid.NewGuid():N}");
        try
        {
            var header = new byte[128];
            new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
            var slot = new EVContainerObject(0, 0, 0, 0) { BodyOrder = 0, Scripts = new[] { new EVContainerScript(0, new byte[] { 4 }) } };
            EventProjectExporter.Export(new EVContainer(header, new[] { slot }, Array.Empty<byte>()), "AllocationTest", project);
            File.WriteAllText(Path.Combine(project, "NewLocation.cs"), """
                using EvilForest.EventSource;

                [assembly: MapStorageReservation(6, 1)]

                namespace Event_AllocationTest;

                internal static class NewLocation
                {
                    [StorageField(StorageScope.Map, StorageType.Byte)]
                    public static int Counter { get; set; }
                }
                """);
            string path = Directory.GetFiles(project, "00_*.cs").Single();
            string source = File.ReadAllText(path);
            File.WriteAllText(path, source.Replace("return;", $"NewLocation.Counter = 5;{Environment.NewLine}        return;", StringComparison.Ordinal));

            EVContainer result = EventProjectCompiler.Compile(project);
            EVInstruction instruction = EVInstructionBody.Decode(result.Objects[0].Scripts[0].Body).Instructions[0];
            EVVariable variable = EVVariable.Decode(instruction.Expression[0]);
            Assert.Equal(EVStorageScope.Map, variable.Scope);
            Assert.Equal(1, variable.Address);

            var assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
            byte[] location = new byte[80];
            using (new global::EvilForest.EventSource.ScriptMemory(new byte[16], location, new byte[4]).Enter())
                assembly.GetType("Event_AllocationTest.Object_00")!.GetMethod("OnInit")!.Invoke(null, null);
            Assert.Equal(5, location[variable.Address]);
        }
        finally
        {
            if (Directory.Exists(project))
                Directory.Delete(project, recursive: true);
        }
    }

    [Fact]
    public void AuthoredStorageOverlapsAndUsesIndependentInstanceBuffers()
    {
        string project = Path.Combine(Path.GetTempPath(), $"evilforest-memory-{Guid.NewGuid():N}");
        try
        {
            var header = new byte[128];
            new byte[] { 0x45, 0x56, 2 }.CopyTo(header, 0);
            EventProjectExporter.Export(new EVContainer(header, Array.Empty<EVContainerObject>(), Array.Empty<byte>()), "MemoryTest", project);
            const string probe = """
                using System;
                using EvilForest.EventSource;

                public static class MemoryProbe
                {
                    public static int[] ReadViews()
                    {
                        byte[] globals = new byte[16];
                        byte[] location = new byte[80];
                        var first = new ScriptMemory(globals, location, new byte[16]);
                        var second = new ScriptMemory(globals, location, new byte[16]);
                        using (first.Enter())
                        {
                            ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 1, 0x11234);
                            ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 2, 0xFE);
                            ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8, 1);
                            ScriptMemory.Write(StorageScope.Global, StorageType.UInt24, 5, 0x1845678);
                            ScriptMemory.Write(StorageScope.Map, StorageType.Byte, 1, 42);
                            ScriptMemory.Write(StorageScope.Instance, StorageType.Byte, 1, 7);
                        }
                        int[] result;
                        using (second.Enter())
                        {
                            result = new[] {
                                ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 1),
                                ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 1),
                                ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 5),
                                ScriptMemory.Read(StorageScope.Global, StorageType.UInt24, 5),
                                ScriptMemory.Read(StorageScope.Map, StorageType.Byte, 1),
                                ScriptMemory.Read(StorageScope.Instance, StorageType.Byte, 1), 0
                            };
                            ScriptMemory.Write(StorageScope.Instance, StorageType.Byte, 1, 9);
                        }
                        using (first.Enter())
                            result[6] = ScriptMemory.Read(StorageScope.Instance, StorageType.Byte, 1);
                        return result;
                    }
                }
                """;
            SyntaxTree[] trees = Directory.GetFiles(project, "*.cs").Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path))
                .Append(CSharpSyntaxTree.ParseText(probe)).ToArray();
            var compilation = CSharpCompilation.Create("EventStorageProbe", trees, SourceProjectTest.EventReferences(project),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using var assembly = new MemoryStream();
            var result = compilation.Emit(assembly);
            Assert.True(result.Success, string.Join(Environment.NewLine, result.Diagnostics));
            _ = typeof(global::EvilForest.EventSource.ScriptMemory).Assembly;
            Type type = Assembly.Load(assembly.ToArray()).GetType("MemoryProbe")!;
            Assert.Equal(new[] { unchecked((short)0xFE35), 0xFE35, -0x7BA988, -0x7BA988, 42, 0, 7 }, (int[])type.GetMethod("ReadViews")!.Invoke(null, null)!);
        }
        finally
        {
            if (Directory.Exists(project))
                Directory.Delete(project, recursive: true);
        }
    }
}
