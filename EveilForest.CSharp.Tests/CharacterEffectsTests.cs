using System.Reflection;
using EveilForest.CSharp;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class CharacterEffectsTests
{
    [Fact]
    public void CharacterEffectsPreserveOperandsEvaluationAndEdits()
    {
        var storage = new EVVariable(EVStorageScope.Instance, EVStorageType.Byte, 0);
        var increment = new EVOperand([storage.Encode(), new EVExpressionToken((byte)Jsm.Excode.B_PRE_PLUS)]);
        var read = new EVOperand([storage.Encode()]);
        EVInstruction[] operations =
        [
            Effect(Immediate(0), Immediate(255), increment, increment, 0x0C),
            Effect(Immediate(4), Immediate(7), read, Immediate(0), 0x04),
            Effect(8, 3, 0, 0, 0x80),
            Effect(9, 11, 12, 13),
            Effect(10, 4096, -4096, 8192),
            Effect(11, 1, 2, 3),
            Effect(16, 38, 8, 914),
            Effect(Immediate(17), read, Immediate(4), Immediate(914), 0x02),
            Effect(18, 38, 2, 19),
            Effect(Immediate(19), Immediate(38), Immediate(2), increment, 0x08),
            Effect(read, Immediate(1), Immediate(2), Immediate(3), 0x01),
            new(0, Jsm.Opcode.Return),
        ];
        var header = new byte[128];
        new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
        for (int index = 0; index < operations.Length; index++)
            operations[index].Label = index;
        var slot = new EVContainerObject(0, 1, 0, 0)
        {
            BodyOrder = 0,
            Scripts = [new EVContainerScript(0, new EVInstructionBody(operations, operations.Length).Encode())]
        };
        var container = new EVContainer(header, [slot], []);
        string project = Path.Combine(Path.GetTempPath(), $"evilforest-character-effects-{Guid.NewGuid():N}");
        try
        {
            EventProjectExporter.Export(container, "CharacterEffectsTest", project);
            string file = Directory.GetFiles(project, "00_*.cs").Single();
            string source = File.ReadAllText(file);
            Assert.Contains("SetMirrorColor(red: 1, green: 2, blue: 3)", source);
            Assert.Contains("SetMirrorVisibility(enabled: ScriptFlag.FromScript(3)", source);
            Assert.Contains("RemoveFrameSound", source);
            Assert.Contains("SetAlternateFrameSound(animation: AnimationId.FromScript(", source);
            Assert.Contains("Actors.RunModelCode", source);
            Assert.Equal(container.Serialize(), EventProjectCompiler.Compile(project).Serialize());

            var host = new ModelEffectHost();
            var scheduler = new EventScheduler(host, new byte[16], new byte[80]);
            scheduler.RegisterProject(Assembly.Load(EventProjectCompiler.CompileManaged(project)));
            scheduler.StartScene();
            scheduler.Tick();
            Assert.Equal(new[] { 0, 255, 1, 2 }, host.Calls[0]);
            Assert.Equal(new[] { 4, 7, 2, 0 }, host.Calls[1]);
            Assert.Equal(new[] { 19, 38, 2, 3 }, host.Calls[9]);
            Assert.Equal(new[] { 3, 1, 2, 3 }, host.Calls[10]);

            File.WriteAllText(file, source.Replace("frame: 8", "frame: 9", StringComparison.Ordinal));
            byte[] edited = EventProjectCompiler.Compile(project).Serialize();
            operations[6].Operands[2] = new EVOperand(9);
            slot.Scripts[0].Body = new EVInstructionBody(operations, operations.Length).Encode();
            Assert.Equal(container.Serialize(), edited);
        }
        finally
        {
            if (Directory.Exists(project))
                Directory.Delete(project, recursive: true);
        }
    }

    private static EVOperand Immediate(int value) => new(value);

    private static EVInstruction Effect(int command, int first, int second, int third, byte mask = 0) =>
        Effect(Immediate(command), Immediate(first), Immediate(second), Immediate(third), mask);

    private static EVInstruction Effect(EVOperand command, EVOperand first, EVOperand second, EVOperand third, byte mask = 0) =>
        new(0, Jsm.Opcode.CHRFX) { ArgumentMask = mask, Operands = [command, first, second, third] };

    private sealed class ModelEffectHost : IEventHost
    {
        public List<int[]> Calls { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            if (opcode == Opcode.CHRFX)
                Calls.Add(operands.ToArray().Select(operand => (int)operand).ToArray());
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;
        public bool IsWindowOpen(int objectId, int windowId) => false;
    }
}
