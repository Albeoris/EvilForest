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
