using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Memoria.EventEngine.EV;
using FF8.JSM;
using Xunit;
using Xunit.Abstractions;

namespace EveilForest.CSharp.Tests;

public sealed class MemoriaRuntimeTests(ITestOutputHelper output)
{
    internal static void VerifyCompiledTruthOperator(byte opcode, int[] inputs)
    {
        string? directory = Environment.GetEnvironmentVariable("EVILFOREST_MEMORIA_MANAGED");
        if (string.IsNullOrWhiteSpace(directory))
            return;
        new RuntimeProbe(Path.GetFullPath(directory)).CheckTruthOperator(opcode, inputs);
    }

    [Fact]
    [Trait("Category", "MemoriaRuntime")]
    public void InstalledMemoriaPreservesVariableViewsAndRequestTransitions()
    {
        string? directory = Environment.GetEnvironmentVariable("EVILFOREST_MEMORIA_MANAGED");
        if (string.IsNullOrWhiteSpace(directory))
        {
            output.WriteLine("Set EVILFOREST_MEMORIA_MANAGED to the installed Memoria Managed directory.");
            return;
        }

        var runtime = new RuntimeProbe(Path.GetFullPath(directory));
        runtime.CheckVariables();
        foreach (byte opcode in new byte[] { 0x10, 0x12, 0x14 })
        {
            byte[] original = { opcode, 0, 2, 2, 2, 0x22, 0, 1, 0x04 };
            byte[] rebuilt = EVInstructionBody.Decode(original).Encode();
            foreach (bool busy in new[] { false, true })
            {
                string[] trace = runtime.TraceRequest(original, busy);
                Assert.Equal(trace, runtime.TraceRequest(rebuilt, busy));
                if (busy && opcode == 0x10)
                    Assert.Equal(new[] { "busy", "caller continued" }, trace);
                if (busy && opcode != 0x10)
                    Assert.Equal("retry at 0", trace[1]);
                if (!busy || opcode != 0x10)
                {
                    Assert.Contains("accepted at target IP 6", trace);
                    Assert.Equal(opcode == 0x14 ? "target returned, caller resumed" : "target returned", trace[^1]);
                }
                output.WriteLine($"0x{opcode:X2}, busy={busy}: {string.Join(", ", trace)}");
            }
        }
    }

    [Fact]
    [Trait("Category", "MemoriaRuntime")]
    public void InstalledMemoriaReadsFieldFacingAndResolvesControlTransfers()
    {
        string? directory = Environment.GetEnvironmentVariable("EVILFOREST_MEMORIA_MANAGED");
        if (string.IsNullOrWhiteSpace(directory))
            return;

        var runtime = new RuntimeProbe(Path.GetFullPath(directory));
        runtime.CheckFieldFacingAndSelectors();
    }

    [Fact]
    [Trait("Category", "MemoriaRuntime")]
    public void InstalledMemoriaReadsTheWalkingSpeedAtItsObjectRelativeAddress()
    {
        string? directory = Environment.GetEnvironmentVariable("EVILFOREST_MEMORIA_MANAGED");
        if (string.IsNullOrWhiteSpace(directory))
            return;

        var runtime = new RuntimeProbe(Path.GetFullPath(directory));
        byte[] original = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", "EVT_KUINA_KM_SWP_2.eb.bytes"));
        using var input = new MemoryStream(original);
        EVContainer container = EVContainer.Read(input);
        Assert.Equal((30, 323), runtime.ReadWalkingSpeed(original));
        Assert.Equal((30, 323), runtime.ReadWalkingSpeed(container.Serialize()));

        EVContainerScript script = Assert.Single(container.Objects[18].Scripts.Where(script => script.Id == 19));
        script.Body = new byte[] { 0x22, 0, 1 }.Concat(script.Body).ToArray();
        Assert.Equal((30, 326), runtime.ReadWalkingSpeed(container.Serialize(), skip: 3));
    }

    [Fact]
    [Trait("Category", "MemoriaRuntime")]
    public void InstalledMemoriaReadsEditedCargoScaleAndQuadOperands()
    {
        string? directory = Environment.GetEnvironmentVariable("EVILFOREST_MEMORIA_MANAGED");
        if (string.IsNullOrWhiteSpace(directory))
            return;

        var runtime = new RuntimeProbe(Path.GetFullPath(directory));
        string path = Path.Combine(AppContext.BaseDirectory, "TestData", "EVT_ALEX1_TS_CARGO_0.eb.bytes");
        byte[] original = File.ReadAllBytes(path);
        EVEditDocument editor = EVEditDocument.Open(path);
        int scriptIndex = Array.FindIndex(editor.Objects[0].Scripts, script => script.Id == 10);
        EVInstructionBody script = editor.GetBody(0, scriptIndex);
        int scaleIndex = Enumerable.Range(0, script.Instructions.Count).Single(index =>
        {
            EVInstruction instruction = script.Instructions[index];
            return instruction.Opcode == Jsm.Opcode.SPS && instruction.Operands[1].Value == 145 && instruction.Operands[2].Value == 936;
        });
        int scaleOffset = script.Instructions[scaleIndex].Label;
        editor.SetImmediate(0, scriptIndex, scaleIndex, 2, 500);
        editor.SetImmediate(4, 0, 0, 3, 234);

        string outputPath = Path.Combine(Path.GetTempPath(), $"evilforest-runtime-{Guid.NewGuid():N}.eb.bytes");
        try
        {
            editor.Save(outputPath);
            byte[] edited = File.ReadAllBytes(outputPath);
            int[] scaleWidths = { 1, 1, 2, 2, 2 };
            Assert.Equal(new[] { 1, 145, 936, 0, 0 }, runtime.ReadImmediateOperands(original, 0, 10, scaleOffset, 0xB3, scaleWidths).Values);
            Assert.Equal(new[] { 1, 145, 500, 0, 0 }, runtime.ReadImmediateOperands(edited, 0, 10, scaleOffset, 0xB3, scaleWidths).Values);
            int[] vertexWidths = { 1, 2, 2, 2, 2, 2, 2, 2, 2 };
            Assert.Equal(new[] { 4, 0, 350, 210, 120, 300, 350, 210, 650 }, runtime.ReadImmediateOperands(original, 4, 0, 0, 0x29, vertexWidths).Values);
            Assert.Equal(new[] { 4, 0, 350, 234, 120, 300, 350, 210, 650 }, runtime.ReadImmediateOperands(edited, 4, 0, 0, 0x29, vertexWidths).Values);
        }
        finally
        {
            File.Delete(outputPath);
        }
    }

    private sealed class RuntimeProbe(string directory)
    {
        private const BindingFlags Members = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        private readonly Assembly _game = new GameLoadContext(directory).LoadFromAssemblyPath(Path.Combine(directory, "Assembly-CSharp.dll"));

        public void CheckTruthOperator(byte opcode, int[] inputs)
        {
            Type type = GameType("EBin");
            object interpreter = RuntimeHelpers.GetUninitializedObject(type);
            FieldInfo stackField = type.GetField("_calcStack", Members)!;
            object stack = Activator.CreateInstance(stackField.FieldType)!;
            stackField.SetValue(stackField.IsStatic ? null : interpreter, stack);
            object integer = Enum.Parse(type.GetNestedType("VariableSource", Members)!, "Int26");
            int prefix = (int)Call(interpreter, "encodeVarClass", integer)!;
            object operation = Enum.ToObject(type.GetNestedType("op_binary", Members)!, opcode);
            foreach (int value in inputs)
            {
                Call(stack, "emptyCalcStack");
                Call(stack, "push", (value & 0x03FFFFFF) | prefix);
                Call(stack, "push", prefix);
                Call(interpreter, "expr_jumpToSubCommand", operation);
                Assert.Equal(value == 0 ? 0 : 1, (int)Call(interpreter, "EvaluateValueExpression")!);
            }
        }

        public void CheckVariables()
        {
            Type type = GameType("EBin");
            Type variableType = type.GetNestedType("VariableType")!;
            object engine = RuntimeHelpers.GetUninitializedObject(type);
            byte[] storage = { 0, 0x34, 0x12, 0xA0, 0 };
            object Signed24() => Enum.Parse(variableType, "Int24");
            object Unsigned24() => Enum.Parse(variableType, "UInt24");
            object Bit() => Enum.Parse(variableType, "Bit");
            int Read(int address, object format) => (int)Call(engine, "GetVariableValueInternal", storage, address, format, 0)!;

            Assert.Equal(unchecked((int)0xFFA01234), Read(1, Signed24()));
            // Memoria's two 24-bit types both sign-extend on reads.
            Assert.Equal(Read(1, Signed24()), Read(1, Unsigned24()));
            Assert.Equal(1, Read(29, Bit()));
            Call(engine, "SetVariableValueInternal", storage, 29, Bit(), 0, 0);
            Assert.Equal(0, Read(29, Bit()));
            Assert.Equal(unchecked((int)0xFF801234), Read(1, Unsigned24()));
            Assert.Equal(0x1234, Read(1, Enum.Parse(variableType, "UInt16")));
        }

        public void CheckFieldFacingAndSelectors()
        {
            object engine = RuntimeHelpers.GetUninitializedObject(GameType("EventEngine"));
            object interpreter = RuntimeHelpers.GetUninitializedObject(GameType("EBin"));
            object context = Activator.CreateInstance(GameType("EventContext"))!;
            Type actorType = GameType("Actor");
            object actor = RuntimeHelpers.GetUninitializedObject(actorType);
            Type vector = actorType.GetField("rotAngle", Members)!.FieldType;
            Set(interpreter, "_eventEngine", engine);
            Set(engine, "_context", context);
            Set(engine, "gMode", 1);
            Set(engine, "sObjModIndexing", new List<int[]> { Enumerable.Range(0, 23).ToArray() });
            Set(context, "controlUID", (byte)17);
            Assert.Equal(17, Call(engine, "GetUIDFromLocalUID", 250, 0));
            Set(context, "controlUID", (byte)22);
            Assert.Equal(22, Call(engine, "GetUIDFromLocalUID", 250, 0));
            Assert.Equal(17, Call(engine, "GetUIDFromLocalUID", 17, 0));

            foreach (int turn in new[] { 0, 31, 32, 63, 64, 96, 128, 160, 192, 224, 255 })
            {
                Set(actor, "rotAngle", Activator.CreateInstance(vector, 0f, turn * 360f / 256f, 0f)!);
                Assert.Equal(turn, Call(interpreter, "getvobj", actor, 3));
            }
        }

        public string[] TraceRequest(byte[] code, bool busy)
        {
            var trace = new List<string>();
            Type objType = GameType("Obj");
            object engine = RuntimeHelpers.GetUninitializedObject(GameType("EventEngine"));
            object caller = Activator.CreateInstance(objType)!;
            object target = Activator.CreateInstance(objType)!;
            object context = Activator.CreateInstance(GameType("EventContext"))!;
            object entry = Activator.CreateInstance(GameType("ObjList"))!;
            Set(entry, "obj", caller);
            Set(context, "activeObj", entry);
            Set(engine, "_context", context);
            Set(engine, "_objPtrList", Array.CreateInstance(objType, 8));
            Set(engine, "nil", -1);
            Set(engine, "gExec", caller);
            Set(engine, "_lastIP", 0);
            Set(caller, "uid", (byte)1);
            Set(caller, "level", (byte)7);
            Set(caller, "currentByte", code);
            Set(caller, "ip", 0);
            Set(target, "buffer", new byte[128]);
            Set(target, "uid", (byte)2);
            Set(target, "sid", (byte)2);
            Set(target, "sofs", (byte)5);
            Set(target, "ip", 20);
            Set(target, "level", busy ? (byte)1 : (byte)7);
            Set(target, "ebData", Convert.FromHexString("00010200040004"));

            for (int attempt = 0; attempt < 2; attempt++)
            {
                byte opcode = (byte)Call(caller, "getByteIP", 0)!;
                int level = (byte)Call(caller, "getByteIP", 2)!;
                int uid = (byte)Call(caller, "getByteIP", 3)!;
                int tag = (byte)Call(caller, "getByteIP", 4)!;
                Set(caller, "ip", 5);
                Assert.Equal(Get<byte>(target, "uid"), uid);

                bool wait = opcode == 0x14;
                bool accepted = (bool)Call(engine, "Request", target, level, tag, wait)!;
                if (accepted)
                {
                    trace.Add($"accepted at target IP {Get<int>(target, "ip")}");
                    trace.Add(wait ? $"caller wait {Get<byte>(caller, "wait")}" : "caller continued");
                    Assert.Equal(wait ? 255 : 0, Get<byte>(caller, "wait"));
                    Call(engine, "Return", target);
                    Assert.Equal(0, Get<byte>(caller, "wait"));
                    Assert.Equal(20, Get<int>(target, "ip"));
                    trace.Add(wait ? "target returned, caller resumed" : "target returned");
                    break;
                }

                trace.Add("busy");
                if (opcode == 0x10)
                {
                    trace.Add("caller continued");
                    break;
                }

                Call(engine, "stay");
                trace.Add($"retry at {Get<int>(caller, "ip")}");
                Set(target, "level", (byte)7);
            }
            return trace.ToArray();
        }

        public (int Speed, int Ip) ReadWalkingSpeed(byte[] data, int skip = 0)
        {
            var result = ReadImmediateOperands(data, 18, 19, skip, 0x26, new[] { 1 });
            return (result.Values[0], result.Ip);
        }

        public (int[] Values, int Ip) ReadImmediateOperands(byte[] data, int objectId, int scriptId, int instructionOffset, byte opcode, int[] widths)
        {
            int slot = EVContainer.HeaderSize + objectId * EVContainer.ObjectEntrySize;
            int offset = EVContainer.HeaderSize + BitConverter.ToUInt16(data, slot);
            byte[] obj = data.AsSpan(offset, BitConverter.ToUInt16(data, slot + 2)).ToArray();
            object engine = RuntimeHelpers.GetUninitializedObject(GameType("EventEngine"));
            object actor = Activator.CreateInstance(GameType("Obj"))!;
            Set(actor, "currentByte", obj);
            int ip = (int)Call(engine, "GetIP", objectId, scriptId, obj)! + instructionOffset;
            Assert.Equal(opcode, obj[ip]);
            Assert.Equal(0, obj[ip + 1]);
            Set(actor, "ip", ip + 2);
            Set(engine, "gExec", actor);
            Set(engine, "gArgFlag", (int)obj[ip + 1]);

            int[] values = widths.Select(width => (int)Call(engine, $"getv{width}")!).ToArray();
            return (values, Get<int>(actor, "ip"));
        }

        private Type GameType(string name) => _game.GetType(name, throwOnError: true)!;

        private static object? Call(object instance, string name, params object[] args)
        {
            MethodInfo method = instance.GetType().GetMethods(Members).Single(candidate =>
                candidate.Name == name && candidate.GetParameters().Length == args.Length);
            return method.Invoke(instance, args);
        }

        private static T Get<T>(object instance, string name)
        {
            Type type = instance.GetType();
            return (T)(type.GetField(name, Members)?.GetValue(instance) ?? type.GetProperty(name, Members)!.GetValue(instance))!;
        }

        private static void Set(object instance, string name, object value)
        {
            Type type = instance.GetType();
            FieldInfo? field = type.GetField(name, Members);
            if (field != null)
                field.SetValue(instance, value);
            else
                type.GetProperty(name, Members)!.SetValue(instance, value);
        }

        private sealed class GameLoadContext(string directory) : AssemblyLoadContext()
        {
            protected override Assembly? Load(AssemblyName assemblyName)
            {
                if (assemblyName.Name is "mscorlib" or "System" or "System.Core")
                    return null;
                string path = Path.Combine(directory, assemblyName.Name + ".dll");
                return File.Exists(path) ? LoadFromAssemblyPath(path) : null;
            }
        }
    }
}

