using System.Buffers.Binary;
using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class NestedMapSwitchReplayTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-map-switch-{Guid.NewGuid():N}");

    [Theory]
    [InlineData("EVT_CLEYRA2_CATHE_2", 5, 5, 482, Opcode.MES, 246)]
    [InlineData("EVT_CLEYRA2_INN", 19, 8, 483, null, 0)]
    [InlineData("EVT_CLEYRA2_TOWN", 6, 5, 483, Opcode.POS, -1137)]
    [InlineData("EVT_CLEYRA2_VISTA", 16, 5, 484, null, 0)]
    [InlineData("EVT_CLEYRA2_WATER", 18, 1, 484, null, 0)]
    public void EachNativeDestinationEndsTheFieldAndUnmatchedSelectorsKeepRunning(string field, int slot, byte phase, int phaseMarkerAddress, Opcode? continuation, int operand)
    {
        using Stream input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", field + ".eb.bytes"));
        EVContainer original = EVContainer.Read(input);
        var body = EVInstructionBody.Decode(original.Objects[slot].Scripts.Single(script => script.Id == 1).Body).Instructions;
        EVInstruction dispatch = Assert.Single(body.Where(instruction => instruction.Opcode == Jsm.Opcode.JMP_SWITCH && instruction.CaseValues[0] == 1050 && instruction.Targets.Length == 12));
        string project = Path.Combine(_directory, field);
        EventProjectExporter.Export(original, field, project);
        string source = SourceProjectTest.CopyWithoutComments(project);
        string artifacts = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(source), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));

        foreach (Assembly assembly in new[] { Assembly.Load(File.ReadAllBytes(dll)), Assembly.Load(EventProjectCompiler.CompileManaged(source)) })
        {
            for (int index = 0; index < dispatch.Targets.Length - 1; index++)
            {
                int start = dispatch.Targets[index + 1];
                int end = dispatch.Targets.Where(target => target > start).DefaultIfEmpty(int.MaxValue).Min();
                EVInstruction[] arm = body.Where(instruction => instruction.Label >= start && instruction.Label < end).ToArray();
                int preload = Assert.Single(arm.Where(instruction => instruction.Opcode == Jsm.Opcode.HINT)).Operands[1].Value;
                int destination = Assert.Single(arm.Where(instruction => instruction.Opcode == Jsm.Opcode.MAPJUMP)).Operands[0].Value;
                var (scheduler, host, _) = Replay(assembly, (short)(dispatch.CaseValues[0] + index));
                Assert.True(scheduler.HasEnded);
                Assert.Equal(new[] { (Opcode.HINT, preload), (Opcode.MAPJUMP, destination) }, host.Transfers);
                Assert.Equal(Opcode.MAPJUMP, host.Commands[^1].Opcode);
                int frame = scheduler.Frame;
                scheduler.Tick();
                Assert.Equal(frame, scheduler.Frame);
            }

            foreach (short selector in new short[] { -1, 1061 })
            {
                var (scheduler, host, globals) = Replay(assembly, selector);
                Assert.False(scheduler.HasEnded);
                Assert.Empty(host.Transfers);
                Assert.NotEqual(0, globals[phaseMarkerAddress]);
                if (continuation.HasValue)
                {
                    Assert.Contains((continuation.Value, operand), host.Commands);
                }
            }
        }

        (EventScheduler, RecordingHost, byte[]) Replay(Assembly assembly, short selector)
        {
            var host = new RecordingHost();
            var globals = new byte[4096];
            var location = new byte[80];
            var scheduler = new EventScheduler(host, globals, location);
            scheduler.RegisterProject(assembly);
            scheduler.CreateParticipant(slot);
            scheduler.Tick();
            scheduler.Tick();
            host.Commands.Clear();
            BinaryPrimitives.WriteInt16LittleEndian(globals.AsSpan(239), selector);
            location[24] = phase;
            for (int frame = 0; frame < 256 && !scheduler.HasEnded; frame++)
            {
                scheduler.Tick();
            }
            return (scheduler, host, globals);
        }
    }

    private sealed class RecordingHost : IEventHost
    {
        public List<(Opcode Opcode, int Operand)> Commands { get; } = new();
        public IEnumerable<(Opcode, int)> Transfers => Commands.Where(command => command.Opcode is Opcode.HINT or Opcode.MAPJUMP);

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Commands.Add((opcode, operands.IsEmpty ? 0 : (int)operands[^1]));
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;
        public bool IsWindowOpen(int objectId, int windowId) => false;
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }
}
