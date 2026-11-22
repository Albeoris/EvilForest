using EvilForest.EventSource;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class EventSchedulerTests
{
    [Fact]
    public void WaitZeroContinuesAndPositiveWaitsFollowObjectVisitOrder()
    {
        var host = new RecordingHost();
        var scheduler = MakeScheduler(host);
        scheduler.AddObject(1, 0, new Dictionary<int, Func<EventRoutine>> { [0] = First }, 0);
        scheduler.AddObject(2, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Second }, 0);

        Tick(scheduler, 4);

        Assert.Equal(new[] { "0.1.10", "0.1.11", "0.2.20", "1.1.12", "1.2.21", "2.2.22", "3.1.13", "3.2.23" }, host.Trace);

        static async EventRoutine First()
        {
            Mark(10);
            await Engine.Wait(0);
            Mark(11);
            await Engine.Wait(1);
            Mark(12);
            await Engine.Wait(2);
            Mark(13);
        }

        static async EventRoutine Second()
        {
            for (int index = 20; index <= 23; index++)
            {
                Mark(index);
                await Engine.YieldFrame();
            }
        }
    }

    [Theory]
    [InlineData(-1, 2)]
    [InlineData(2, 4)]
    public void AMessagePollAdvancesToTheNextObjectBeforeResuming(int openThrough, int resumeFrame)
    {
        var host = new RecordingHost { OpenThrough = openThrough };
        var scheduler = MakeScheduler(host);
        scheduler.AddObject(1, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Handler }, 0);

        Tick(scheduler, 6);

        Assert.Equal(new[] { "0.1.10", $"{resumeFrame}.1.11" }, host.Trace);

        static async EventRoutine Handler()
        {
            Mark(10);
            await Messages.ShowAndWait(1, 0, 34);
            Mark(11);
        }
    }

    [Fact]
    public void RequestsRetryAtTheCallSiteAndRepliesCanPreemptTheWaitingCaller()
    {
        var host = new RecordingHost();
        var scheduler = MakeScheduler(host);
        scheduler.AddObject(1, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Caller, [2] = Reply }, 0, initialLevel: 5);
        scheduler.AddObject(2, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Busy, [1] = Target }, 0, initialLevel: 2);

        Tick(scheduler, 9);

        Assert.Equal(new[] { "0.1.10", "0.1.11", "0.2.20", "1.2.21", "2.2.30", "3.1.40", "4.1.41", "4.2.31", "5.1.12" }, host.Trace);

        static async EventRoutine Caller()
        {
            Mark(10);
            Scripts.RequestAsync(3, 2, 1);
            Mark(11);
            await Scripts.RequestAndWait(3, 2, 1);
            Mark(12);
        }

        static async EventRoutine Busy()
        {
            Mark(20);
            await Engine.Wait(1);
            Mark(21);
        }

        static async EventRoutine Target()
        {
            Mark(30);
            await Scripts.ReplyAndWait(1, 2);
            Mark(31);
        }

        static async EventRoutine Reply()
        {
            Mark(40);
            await Engine.Wait(1);
            Mark(41);
        }
    }

    [Fact]
    public void ControlledActorRequestsObserveControlChangesDuringAdmissionAndWaitForTheAcceptedActor()
    {
        var host = new RecordingHost();
        var scheduler = MakeScheduler(host);
        TestControlledActor controller = EventSceneContext.BindControlled(new TestControlledActor(new TestSceneContext()));
        var scripts = new ScriptService();
        scheduler.ControlledActorId = 17;
        scheduler.AddObject(8, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Caller }, 0);
        scheduler.AddObject(17, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Busy, [12] = Reward }, 0, initialLevel: 1);
        scheduler.AddObject(22, 0, new Dictionary<int, Func<EventRoutine>> { [12] = Reward });

        scheduler.Tick();
        scheduler.ControlledActorId = 22;
        scheduler.Tick();
        scheduler.ControlledActorId = 17;
        Tick(scheduler, 3);

        Assert.Equal(new[] { "0.8.10", "1.22.12", "2.22.13", "3.8.11" }, host.Trace);

        async EventRoutine Caller()
        {
            Mark(10);
            await scripts.RequestAndWait(controller.Reward, level: 2);
            Mark(11);
        }

        static async EventRoutine Busy()
        {
            await Engine.Wait(5);
        }

        static async EventRoutine Reward()
        {
            Mark(12);
            await Engine.Wait(1);
            Mark(13);
        }
    }

    private sealed class TestSceneContext : EventSceneContext;

    private sealed class TestControlledActor(TestSceneContext context) : EventActor(context)
    {
        [Script(12, 0)]
        public EventRoutine Reward() => throw new InvalidOperationException("A request must defer the handler.");
    }

    [Theory]
    [InlineData(0, FacingSector.Sector0)]
    [InlineData(31, FacingSector.Sector0)]
    [InlineData(32, FacingSector.Sector1)]
    [InlineData(63, FacingSector.Sector1)]
    [InlineData(64, FacingSector.Sector2)]
    [InlineData(96, FacingSector.Sector3)]
    [InlineData(128, FacingSector.Sector4)]
    [InlineData(160, FacingSector.Sector5)]
    [InlineData(192, FacingSector.Sector6)]
    [InlineData(224, FacingSector.Sector7)]
    [InlineData(255, FacingSector.Sector7)]
    public void FacingSectorsUseTheOriginalTurnOrdinals(int rawTurn, FacingSector expected)
    {
        var host = new FacingHost(rawTurn);
        var scheduler = new EventScheduler(host, new byte[16], new byte[80]) { ControlledActorId = 17 };
        scheduler.AddObject(8, 0, new Dictionary<int, Func<EventRoutine>> { [0] = () => EventRoutine.Run(Caller) }, 0);
        scheduler.Tick();
        Assert.Equal(17, host.Uid);

        void Caller()
        {
            var actor = new ControlledActorTarget();
            Assert.Equal(rawTurn, actor.Facing.RawTurn);
            Assert.Equal(expected, actor.Facing.Sector8);
        }
    }

    private sealed class FacingHost(int rawTurn) : IEventHost
    {
        public int Uid { get; private set; }
        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands) => EventCommandFlow.Continue;

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands)
        {
            Assert.Equal(Excode.B_OBJSPECA, opcode);
            Uid = (int)operands[0];
            Assert.Equal(3, (int)operands[1]);
            return rawTurn;
        }

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }

    [Fact]
    public void AnAwaitedSelfRequestPreemptsTheCallerWithinTheSameVisit()
    {
        var host = new RecordingHost();
        var scheduler = MakeScheduler(host);
        scheduler.AddObject(8, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Caller, [12] = Reward }, 0);

        Tick(scheduler, 3);

        Assert.Equal(new[] { "0.8.10", "0.8.12", "1.8.13", "2.8.11" }, host.Trace);

        static async EventRoutine Caller()
        {
            Mark(10);
            await Scripts.RequestWhenReady(2, 255, 12);
            Mark(11);
        }

        static async EventRoutine Reward()
        {
            Mark(12);
            await Engine.Wait(1);
            Mark(13);
        }
    }

    [Fact]
    public void AnAcceptedRequestWithoutAnOptionalHandlerDoesNotSuspend()
    {
        var host = new RecordingHost();
        var scheduler = MakeScheduler(host);
        scheduler.AddObject(8, 0, new Dictionary<int, Func<EventRoutine>> { [0] = Caller }, 0);
        scheduler.AddObject(17, 0, new Dictionary<int, Func<EventRoutine>>());
        scheduler.ControlledActorId = 17;

        scheduler.Tick();

        Assert.Equal(new[] { "0.8.10", "0.8.11" }, host.Trace);

        static async EventRoutine Caller()
        {
            Mark(10);
            await Scripts.RequestAndWait(2, 250, 12);
            Mark(11);
        }
    }

    [Fact]
    public void RegisteredDefinitionsUsePerUidStorageAndEnterLoopAfterInit()
    {
        var host = new RecordingHost();
        var scheduler = MakeScheduler(host);
        scheduler.RegisterProject(typeof(TestSceneRoot).Assembly);
        scheduler.StartScene();

        Tick(scheduler, 6);

        Assert.Equal(new[] { "0.0.1", "1.1.0", "1.10.0", "2.1.3", "2.10.3", "3.0.2", "4.10.0", "5.10.3" }, host.Trace);
    }

    [ObjectSlot(0, 0, 0, 0, 0, 0, 0, "", "")]
    private static class TestSceneRoot
    {
        private static readonly ParticipantDefinition ChildDefinition = new(typeof(TestSceneChild), ParticipantKind.Thread);

        [Script(0, 0)]
        public static void Init()
        {
            Scripts.StartThread(ChildDefinition);
            Scripts.StartThread(ChildDefinition, new InstanceUid(10));
            Mark(1);
        }

        [Script(1, 1)]
        public static async EventRoutine Loop()
        {
            await Engine.Wait(2);
            Scripts.StartThread(ChildDefinition, new InstanceUid(10));
            Mark(2);
        }
    }

    [ObjectSlot(1, 1, 0, 0, 1, 0, 0, "", "")]
    private static class TestSceneChild
    {
        [Script(0, 0)]
        public static void Init()
        {
            Mark(ScriptMemory.Read(StorageScope.Instance, StorageType.Byte, 0));
            ScriptMemory.Write(StorageScope.Instance, StorageType.Byte, 0, 3);
        }

        [Script(1, 1)]
        public static void Loop()
        {
            Mark(ScriptMemory.Read(StorageScope.Instance, StorageType.Byte, 0));
            ScriptMemory.Write(StorageScope.Instance, StorageType.Byte, 0, 9);
        }
    }

    [Fact]
    public void SharedScriptActsForItsOwnerAndUsesItsOwnInstanceStorage()
    {
        var host = new RecordingHost();
        var scheduler = MakeScheduler(host);
        scheduler.RegisterProject(typeof(TestSceneSequence).Assembly);
        scheduler.AddObject(8, 1, new Dictionary<int, Func<EventRoutine>> { [0] = Caller }, 0);

        Tick(scheduler, 3);

        Assert.Equal(new[] { "0.8.10", "1.8.0", "2.8.3" }, host.Trace);

        static async EventRoutine Caller()
        {
            ScriptMemory.Write(StorageScope.Instance, StorageType.Byte, 0, 3);
            Scripts.RunSharedScript(new ParticipantDefinition(typeof(TestSceneSequence), ParticipantKind.Region));
            Mark(10);
            await Scripts.WaitSharedScript();
            Mark(ScriptMemory.Read(StorageScope.Instance, StorageType.Byte, 0));
        }
    }

    [ObjectSlot(2, 1, 0, 0, 2, 0, 1, "", "")]
    private static class TestSceneSequence
    {
        [Script(0, 0)]
        public static void Init()
        {
            Mark(ScriptMemory.Read(StorageScope.Instance, StorageType.Byte, 0));
            ScriptMemory.Write(StorageScope.Instance, StorageType.Byte, 0, 9);
            Scripts.DeleteObject(255);
        }
    }

    [Fact]
    public void ObjectInstancesKeepIndependentFieldsAcrossAWaitAndShareLocationFields()
    {
        var host = new RecordingHost();
        var scheduler = MakeScheduler(host);
        scheduler.AddObject(1, 1, new Dictionary<int, Func<EventRoutine>> { [0] = () => Handler(7) }, 0);
        scheduler.AddObject(2, 1, new Dictionary<int, Func<EventRoutine>> { [0] = () => Handler(9) }, 0);

        Tick(scheduler, 2);

        Assert.Equal(new[] { "1.1.7", "1.1.9", "1.2.9", "1.2.9" }, host.Trace);

        static async EventRoutine Handler(int value)
        {
            ScriptMemory.Write(StorageScope.Instance, StorageType.Byte, 0, value);
            ScriptMemory.Write(StorageScope.Map, StorageType.Byte, 0, value);
            await Engine.Wait(1);
            Mark(ScriptMemory.Read(StorageScope.Instance, StorageType.Byte, 0));
            Mark(ScriptMemory.Read(StorageScope.Map, StorageType.Byte, 0));
        }
    }

    private static void Mark(int value) => Messages.SetTextVariable(0, value);

    private static EventScheduler MakeScheduler(RecordingHost host)
    {
        var scheduler = new EventScheduler(host, new byte[128], new byte[80]);
        host.Frame = () => scheduler.Frame;
        return scheduler;
    }

    private static void Tick(EventScheduler scheduler, int count)
    {
        for (int index = 0; index < count; index++)
            scheduler.Tick();
    }

    private sealed class RecordingHost : IEventHost
    {
        public List<string> Trace { get; } = new();
        public Func<int> Frame { get; set; } = null!;
        public int OpenThrough { get; init; } = -1;

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            if (opcode == Opcode.MESVALUE)
                Trace.Add($"{Frame()}.{objectId}.{(int)operands[1]}");
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => throw new InvalidOperationException();
        public bool IsWindowOpen(int objectId, int windowId) => Frame() <= OpenThrough;
    }
}
