using System.Buffers.Binary;
using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class FieldTransferArmEditTests : IDisposable
{
    private const int SelectorAddress = 239;
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-field-arm-").FullName;

    [Fact]
    public void RemovingTheFieldTransferRestoresTheNativeBreakInAnEditedArm()
    {
        EVInstruction[] instructions =
        [
            new(0, Jsm.Opcode.EXPR) { Expression = [new EVVariable(EVStorageScope.Global, EVStorageType.Int16, SelectorAddress).Encode()] },
            new(1, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [42, 5, 7], Targets = [8, 4, 2, 7] },
            new(2, Jsm.Opcode.MESVALUE) { Operands = [new(0), new(1)] },
            new(3, Jsm.Opcode.MAPJUMP) { Operands = [new(61000)] },
            new(4, Jsm.Opcode.MESVALUE) { Operands = [new(0), new(2)] },
            new(5, Jsm.Opcode.MAPJUMP) { Operands = [new(61001)] },
            new(6, Jsm.Opcode.JMP) { Targets = [8] },
            new(7, Jsm.Opcode.JMP) { Targets = [8] },
            new(8, Jsm.Opcode.MESVALUE) { Operands = [new(0), new(3)] },
            new(9, Jsm.Opcode.Return)
        ];
        byte[] body = new EVInstructionBody(instructions, instructions.Length).Encode();
        var slot = new EVContainerObject(0, 0, 0, 0)
        {
            BodyOrder = 0,
            Type = (byte)ParticipantKind.Thread,
            Scripts = [new EVContainerScript(0, body)]
        };
        var header = new byte[128];
        new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
        var original = new EVContainer(header, [slot], []);
        string project = Export(original, "source");
        string file = Directory.GetFiles(project, "00_*.cs").Single();
        string source = File.ReadAllText(file);
        Assert.Contains("TransferExitSections = \"0\"", source);
        Assert.DoesNotContain("goto ", source);
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, [1, 61000], transfer: true);

        const string transfer = "Field.ChangeMap(fieldId: 61000);";
        Assert.Contains(transfer, source);
        File.WriteAllText(file, source.Replace(transfer, "", StringComparison.Ordinal));
        EVContainer edited = EventProjectCompiler.Compile(project);
        IList<EVInstruction> result = EVInstructionBody.Decode(Assert.Single(edited.Objects[0].Scripts).Body).Instructions;
        EVInstruction dispatch = result[1];
        Assert.Equal(new ushort[] { 42, 5, 7 }, dispatch.CaseValues);
        Assert.Equal([Jsm.Opcode.MESVALUE, Jsm.Opcode.JMP], Arm(5));
        Assert.Equal([Jsm.Opcode.MESVALUE, Jsm.Opcode.MAPJUMP, Jsm.Opcode.JMP], Arm(42));
        string reopened = Export(edited, "reopened");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        ExpectPaths(reopened, [1, 3], transfer: false);

        Jsm.Opcode[] Arm(ushort selector)
        {
            int start = dispatch.Targets[Array.IndexOf(dispatch.CaseValues, selector) + 1];
            int end = dispatch.Targets.Where(target => target > start).Min();
            EVInstruction[] section = result.Where(instruction => instruction.Label >= start && instruction.Label < end).ToArray();
            Assert.Equal(dispatch.Targets[0], Assert.Single(section[^1].Targets));
            return section.Select(instruction => instruction.Opcode).ToArray();
        }
    }

    private string Export(EVContainer container, string leaf)
    {
        string project = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(container, "SwitchFixture", project, useCutsceneNames: false, extractCutscenes: false);
        return SourceProjectTest.CopyWithoutComments(project);
    }

    private static void ExpectPaths(string project, int[] values, bool transfer)
    {
        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Run(5, values, transfer);
        Run(42, [2, 61001], true);
        Run(7, [3], false);
        Run(0, [3], false);
        Run(-1, [3], false);
        Run(short.MinValue, [3], false);

        void Run(short selector, int[] expected, bool endsEvent)
        {
            var host = new RecordingHost();
            var globals = new byte[4096];
            BinaryPrimitives.WriteInt16LittleEndian(globals.AsSpan(SelectorAddress), selector);
            var scheduler = new EventScheduler(host, globals, new byte[80]);
            scheduler.RegisterProject(assembly);
            scheduler.StartScene();
            scheduler.Tick();
            Assert.Equal(expected, host.Values);
            Assert.Equal(endsEvent, scheduler.HasEnded);
        }
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);

    private sealed class RecordingHost : IEventHost
    {
        public List<int> Values { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Assert.True(opcode is Opcode.MESVALUE or Opcode.MAPJUMP);
            Values.Add((int)operands[^1]);
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => throw new InvalidOperationException();

        public bool IsWindowOpen(int objectId, int windowId) => throw new InvalidOperationException();
    }
}
