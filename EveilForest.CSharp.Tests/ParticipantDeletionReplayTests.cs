using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class ParticipantDeletionReplayTests : IDisposable
{
    private const int PhaseAddress = 24;
    private const int BarrierCountAddress = 26;
    private const int SignalByteAddress = 28;
    private const int SharedScriptUidOffset = 64;
    private const int SharedScriptEndFrame = 4;
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-participant-deletion-{Guid.NewGuid():N}");

    [Theory]
    [InlineData(255, true)]
    [InlineData(9, true)]
    [InlineData(250, true)]
    [InlineData(251, true)]
    [InlineData(8, false)]
    [InlineData(252, false)]
    public void DeleteDiscardsEveryActivationOfItsTargetAndOnlyStopsThatParticipant(int operand, bool deletesSelf)
    {
        var host = new RecordingHost();
        var scheduler = new EventScheduler(host, new byte[4096], new byte[80]);
        scheduler.ControlledActorId = 9;
        scheduler.SetPartyMember(0, 9);
        scheduler.SetPartyMember(1, 8);
        host.Frame = () => scheduler.Frame;
        scheduler.AddObject(7, 0, new Dictionary<int, Func<EventRoutine>> { [0] = WaitForDeleteCallback }, 0);
        scheduler.AddObject(9, 0, new Dictionary<int, Func<EventRoutine>> { [0] = EarlierWork, [1] = DeleteInNestedRole }, 0);
        scheduler.AddObject(8, 0, new Dictionary<int, Func<EventRoutine>> { [0] = OtherWork }, 0);
        scheduler.AddObject(10, 0, new Dictionary<int, Func<EventRoutine>> { [0] = NextParticipant }, 0);

        Tick(scheduler, 7);

        Command[] expected = deletesSelf
            ? [MarkAt(0, 9, 90), MarkAt(0, 8, 80), MarkAt(2, 9, 91), MarkAt(2, 10, 100), MarkAt(4, 8, 81)]
            : [MarkAt(0, 9, 90), MarkAt(0, 8, 80), MarkAt(2, 9, 91), MarkAt(2, 9, 92), MarkAt(2, 9, 93), MarkAt(2, 10, 100), MarkAt(3, 7, 70), MarkAt(6, 9, 94)];
        Assert.Equal(expected, host.Commands);
        Assert.Equal(!deletesSelf, scheduler.TryTrigger(9, 0, 1));
        Assert.Equal(deletesSelf, scheduler.TryTrigger(8, 0, 1));

        async EventRoutine WaitForDeleteCallback()
        {
            await Engine.Wait(1);
            await Scripts.RequestAndWait(2, 9, 1);
            Mark(70);
        }

        async EventRoutine EarlierWork()
        {
            Mark(90);
            await Engine.Wait(4);
            Mark(94);
        }

        async EventRoutine DeleteInNestedRole()
        {
            await Role();
            Mark(93);
            Scripts.ReturnFromScript();
        }

        async EventRoutine Role()
        {
            await Engine.Wait(1);
            Mark(91);
            Scripts.DeleteObject(operand);
            Mark(92);
        }

        async EventRoutine OtherWork()
        {
            Mark(80);
            await Engine.Wait(4);
            Mark(81);
        }

        async EventRoutine NextParticipant()
        {
            await Engine.Wait(2);
            Mark(100);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AnAwaitedRequestStaysSuspendedWhenItsTargetDisappearsAndItsUidIsReused(bool replaceLiveTarget)
    {
        var host = new RecordingHost();
        var scheduler = new EventScheduler(host, new byte[4096], new byte[80]);
        host.Frame = () => scheduler.Frame;
        scheduler.RegisterProject(typeof(ParticipantDeletionReplayTests).Assembly);
        scheduler.AddObject(7, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Caller }, 0);
        scheduler.AddObject(9, 0, new Dictionary<int, Func<EventRoutine>> { [1] = Target });

        scheduler.Tick();
        scheduler.CreateParticipant(60, 9);
        Tick(scheduler, 2);
        Assert.True(scheduler.TryTrigger(9, 1, 2));
        Tick(scheduler, 6);

        Assert.Equal(new[] { MarkAt(0, 7, 70), MarkAt(0, 9, 90), MarkAt(3, 9, 92) }, host.Commands);

        async EventRoutine Caller()
        {
            Mark(70);
            await Scripts.RequestAndWait(2, 9, 1);
            Mark(71);
        }

        EventRoutine Target() => replaceLiveTarget ? HoldTarget() : EventRoutine.Run(() =>
        {
            Mark(90);
            Scripts.DeleteObject(255);
        });

        async EventRoutine HoldTarget()
        {
            Mark(90);
            await Engine.Wait(3);
            Mark(93);
        }
    }

    [Fact]
    public void ARequestWhenReadyWaitsForAdmissionAndContinuesBeforeTheAcceptedTargetDeletesItself()
    {
        var host = new RecordingHost();
        var scheduler = new EventScheduler(host, new byte[4096], new byte[80]);
        host.Frame = () => scheduler.Frame;
        scheduler.AddObject(7, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Caller }, 0);
        scheduler.AddObject(9, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Occupied, [1] = Delete }, 0, initialLevel: 2);

        Tick(scheduler, 6);

        Assert.Equal(new[] { MarkAt(0, 7, 70), MarkAt(0, 9, 90), MarkAt(2, 9, 91), MarkAt(3, 7, 71), MarkAt(3, 9, 92) }, host.Commands);
        Assert.False(scheduler.TryTrigger(9, 1, 2));

        async EventRoutine Caller()
        {
            Mark(70);
            await Scripts.RequestWhenReady(2, 9, 1);
            Mark(71);
        }

        async EventRoutine Occupied()
        {
            Mark(90);
            await Engine.Wait(2);
            Mark(91);
        }

        EventRoutine Delete() => EventRoutine.Run(() =>
        {
            Mark(92);
            Scripts.DeleteObject(255);
        });
    }

    [Fact]
    public void DeletingTheControlledActorResolvesSelector250ToUidZeroEvenAfterReusingTheOldUid()
    {
        var host = new RecordingHost();
        var scheduler = new EventScheduler(host, new byte[4096], new byte[80]);
        host.Frame = () => scheduler.Frame;
        scheduler.RegisterProject(typeof(ParticipantDeletionReplayTests).Assembly);
        scheduler.ControlledActorId = 9;
        scheduler.AddObject(7, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Caller }, 0);
        scheduler.AddObject(8, 0, new Dictionary<int, Func<EventRoutine>>());
        scheduler.AddObject(9, 0, new Dictionary<int, Func<EventRoutine>> { [1] = () => EventRoutine.Run(() => Mark(90)) });
        scheduler.AddObject(0, 0, new Dictionary<int, Func<EventRoutine>> { [1] = () => EventRoutine.Run(() => Mark(100)) });

        scheduler.Tick();
        Assert.Equal(9, scheduler.ControlledActorId);
        scheduler.Tick();
        Assert.Equal(0, scheduler.ControlledActorId);
        scheduler.CreateParticipant(60, 9);
        Tick(scheduler, 2);
        Assert.Equal(new[] { MarkAt(0, 9, 90), MarkAt(3, 0, 100) }, host.Commands);
        Assert.True(scheduler.TryTrigger(9, 1, 2));
        scheduler.Tick();
        Assert.Equal(MarkAt(4, 9, 92), host.Commands[^1]);

        async EventRoutine Caller()
        {
            Scripts.DeleteObject(8);
            Scripts.RequestAsync(2, 250, 1);
            await Engine.Wait(1);
            Scripts.DeleteObject(9);
            await Engine.Wait(2);
            Scripts.RequestAsync(2, 250, 1);
        }
    }

    [ObjectSlot(60, 0, 0, 0, 60, 0, 0, "", "")]
    private static class Replacement
    {
        [Script(1, 0)]
        public static void OnRequest()
        {
            Mark(92);
        }
    }

    [Theory]
    [InlineData("EVT_CLEYRA3_TOWN", 13, 15, new int[] { 5, 6, 8 })]
    [InlineData("EVT_CLEYRA3_VISTA", 15, 16, new int[] { 15 })]
    [InlineData("EVT_CLEYRA3_WIND", 12, 5, new int[] { 4, 5, 6 })]
    public void OriginalCleyraActorsDieBeforeTheirPhysicallyAdjacentPhasesInBothManagedBuilds(string field, int phase, int adjacent, int[] actors)
    {
        EVContainer original = Fixture(field);
        string flat = Export(original, field, "flat", extract: false);
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(flat).Serialize());
        Assembly flatAssembly = Assembly.Load(EventProjectCompiler.CompileManaged(flat));
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "strict", extract: true));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assembly managedAssembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        string artifacts = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));
        Assembly sdkAssembly = Assembly.Load(File.ReadAllBytes(dll));

        foreach (int slot in actors)
        {
            EVInstruction[] nativeRole = ReadNativeRole(original, slot, phase, adjacent);
            var (commands, deletionFrame) = NativeTrace(nativeRole, slot);
            bool hasSharedScript = nativeRole.Any(instruction => instruction.Opcode == Jsm.Opcode.WAITSEQ);
            int expectedCount = field == "EVT_CLEYRA3_TOWN" && slot == 5 ? 3 : 0;
            foreach (Assembly assembly in new[] { flatAssembly, managedAssembly, sdkAssembly })
            {
                Replay replay = Trace(assembly, slot, phase, adjacent, deletionFrame, hasSharedScript);
                Assert.Equal(commands, replay.Commands);
                Assert.Equal(expectedCount, replay.Count);
                Assert.Equal(0, replay.Flags);
            }
        }
    }

    private static EVInstruction[] ReadNativeRole(EVContainer original, int slot, int phase, int adjacent)
    {
        var script = original.Objects[slot].Scripts.Single(entry => entry.Id == 1);
        IList<EVInstruction> instructions = EVInstructionBody.Decode(script.Body).Instructions;
        EVInstruction dispatch = instructions.First(instruction => instruction.Opcode is Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX);
        int start = Target(phase);
        int afterDelete = Target(adjacent);
        EVInstruction[] role = instructions.Where(instruction => instruction.Label >= start && instruction.Label < afterDelete).ToArray();
        EVInstruction delete = role[^1];
        Assert.Equal(Jsm.Opcode.DELETE, delete.Opcode);
        Assert.Equal(255, Assert.Single(delete.Operands).Value);
        Assert.Equal(afterDelete, delete.Label + delete.OriginalLength);
        return role;

        int Target(int key) => dispatch.Opcode == Jsm.Opcode.JMP_SWITCHEX
            ? dispatch.Targets[Array.IndexOf(dispatch.CaseValues, (ushort)key) + 1]
            : dispatch.Targets[key - dispatch.CaseValues[0] + 1];
    }

    private static (Command[] Commands, int DeletionFrame) NativeTrace(EVInstruction[] role, int slot)
    {
        int frame = 2;
        var expected = new List<Command>();
        foreach (EVInstruction instruction in role)
        {
            switch (instruction.Opcode)
            {
                case Jsm.Opcode.EXPR:
                case Jsm.Opcode.DELETE:
                    break;
                case Jsm.Opcode.WAIT:
                    frame += Assert.Single(instruction.Operands).Value;
                    break;
                case Jsm.Opcode.WAITSEQ:
                    frame = Math.Max(frame, SharedScriptEndFrame);
                    break;
                case Jsm.Opcode.CLRDIST:
                case Jsm.Opcode.MROT:
                case Jsm.Opcode.MOVE:
                case Jsm.Opcode.WAITANIM:
                    expected.Add(new Command(frame, slot, (Opcode)instruction.Opcode, string.Join(",", instruction.Operands.Select(operand => operand.Value))));
                    if (instruction.Opcode is Jsm.Opcode.MOVE or Jsm.Opcode.WAITANIM)
                    {
                        frame++;
                    }
                    break;
                default:
                    throw new InvalidDataException($"Unexpected opcode in deletion replay: {instruction.Opcode}.");
            }
        }
        return (expected.ToArray(), frame);
    }

    private static Replay Trace(Assembly assembly, int slot, int phase, int adjacent, int deletionFrame, bool hasSharedScript)
    {
        var (probe, _, _) = StartReplay();
        Tick(probe, deletionFrame - 2);
        Assert.True(probe.TryTrigger(slot, 0, 1));

        var (scheduler, host, location) = StartReplay();
        Tick(scheduler, deletionFrame - 1);
        Assert.False(scheduler.TryTrigger(slot, 0, 1));
        location[PhaseAddress] = (byte)adjacent;
        Tick(scheduler, 24);
        Assert.False(scheduler.TryTrigger(slot, 0, 1));
        return new Replay(host.Commands.ToArray(), location[BarrierCountAddress], location[SignalByteAddress]);

        (EventScheduler Scheduler, RecordingHost Host, byte[] Location) StartReplay()
        {
            var recording = new RecordingHost { YieldForActorActions = true };
            var map = new byte[80];
            var replay = new EventScheduler(recording, new byte[4096], map);
            recording.Frame = () => replay.Frame;
            replay.RegisterProject(assembly);
            if (hasSharedScript)
            {
                // Enter Vista's phase with a shared script already in flight.
                replay.AddObject(slot + SharedScriptUidOffset, 0, new Dictionary<int, Func<EventRoutine>> { [0] = EndSharedScript }, 0);
            }
            replay.CreateParticipant(slot);
            Tick(replay, 2);
            recording.Commands.Clear();
            map[PhaseAddress] = (byte)phase;
            return (replay, recording, map);
        }

        static async EventRoutine EndSharedScript()
        {
            await Engine.Wait(SharedScriptEndFrame);
            Scripts.DeleteObject(255);
        }
    }

    private string Export(EVContainer original, string field, string leaf, bool extract)
    {
        string project = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(original, field, project, useCutsceneNames: false, extractCutscenes: extract);
        return project;
    }

    private static EVContainer Fixture(string field)
    {
        using Stream input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", field + ".eb.bytes"));
        return EVContainer.Read(input);
    }

    private static void Tick(EventScheduler scheduler, int count)
    {
        for (int tick = 0; tick < count; tick++)
        {
            scheduler.Tick();
        }
    }

    private static void Mark(int value) => Messages.SetTextVariable(0, value);

    private static Command MarkAt(int frame, int uid, int value) => new(frame, uid, Opcode.MESVALUE, $"0,{value}");

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }

    private sealed record Command(int Frame, int Owner, Opcode Operation, string Operands);

    private sealed record Replay(Command[] Commands, byte Count, byte Flags);

    private sealed class RecordingHost : IEventHost
    {
        public Func<int> Frame { get; set; } = null!;
        public bool YieldForActorActions { get; init; }
        public List<Command> Commands { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Commands.Add(new Command(Frame(), objectId, opcode, string.Join(",", operands.ToArray().Select(operand => (int)operand))));
            return YieldForActorActions && opcode is Opcode.MOVE or Opcode.WAITANIM ? EventCommandFlow.Yield : EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }
}
