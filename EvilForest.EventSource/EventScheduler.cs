using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EvilForest.EventSource;

public enum EventCommandFlow
{
    Continue,
    Retry,
    Yield,
    EndEvent
}

public interface IEventHost
{
    EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands);
    int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands);
    bool IsWindowOpen(int objectId, int windowId);
}

public sealed class EventScheduler(IEventHost host, byte[] globals, byte[] location)
{
    private static readonly AsyncLocal<Activation?> Current = new();
    private readonly Dictionary<int, ScriptObject> _objects = new();
    private readonly List<ScriptObject> _order = new();
    private bool _ticking;

    public int Frame { get; private set; } = -1;
    public bool HasEnded { get; private set; }

    public void AddObject(int id, int instanceByteCount, IReadOnlyDictionary<int, Func<EventRoutine>> handlers, int? initialHandler = null, int initialLevel = 7)
    {
        var entry = new ScriptObject(this, id, handlers, new ScriptMemory(globals, location, new byte[instanceByteCount]));
        if (initialHandler is int handler)
            entry.Stack.Push(new Activation(entry, initialLevel, handlers[handler], null));
        _objects.Add(id, entry);
        _order.Add(entry);
    }

    public void Tick()
    {
        if (HasEnded)
            return;
        if (_ticking)
            throw new InvalidOperationException("An event tick cannot start another tick.");

        _ticking = true;
        try
        {
            Frame++;
            // A host may append newly created objects during the current visit.
            for (int index = 0; index < _order.Count && !HasEnded; index++)
                Visit(_order[index]);
        }
        finally
        {
            _order.RemoveAll(entry => entry.Deleted);
            _ticking = false;
        }
    }

    private static void Visit(ScriptObject entry)
    {
        if (entry.Deleted || !entry.Stack.TryPeek(out Activation? routine))
            return;
        Activation? previous = Current.Value;
        Current.Value = routine;
        using var memory = entry.Memory.Enter();
        try
        {
            if (routine.Task == null)
                routine.Task = routine.Handler().Completion;
            else if (routine.Pending is { } pending && pending.Ready())
            {
                routine.Pending = null;
                pending.Resume();
            }

            if (entry.Deleted || !routine.Task.IsCompleted)
                return;
            routine.Task.GetAwaiter().GetResult();
            routine.Finished = true;
            entry.Stack.Pop();
        }
        catch (ExitScriptException)
        {
            if (!entry.Deleted && entry.Stack.TryPeek(out Activation? active) && active == routine)
                entry.Stack.Pop();
            routine.Finished = true;
        }
        finally
        {
            Current.Value = previous;
        }
    }

    internal static EventAwaitable Wait(int frames)
    {
        Activation current = Running();
        if (frames == 0)
            return Completed();
        int remaining = frames > 254 ? 253 : unchecked((byte)(frames - 1));
        if (remaining == 254)
            return WaitWindow(current, byte.MaxValue, static () => EventCommandFlow.Continue);
        if (remaining == 255)
            return Suspend(current, static () => false);
        return Suspend(current, () => remaining-- <= 0);
    }

    internal static void Execute(Opcode opcode, params Operand[] operands)
    {
        Activation current = Running();
        switch (opcode)
        {
            case Opcode.REQ:
                current.Owner.Scheduler.TryRequest(current, (int)operands[1], (int)operands[0], (int)operands[2], out _);
                return;
            case Opcode.REPLY:
                current.Owner.Scheduler.TryRequest(current, current.Sender?.Id, (int)operands[0], (int)operands[1], out _);
                return;
            case Opcode.DELETE:
                EventScheduler scheduler = current.Owner.Scheduler;
                int id = (int)operands[0];
                if (id == byte.MaxValue)
                    id = current.Owner.Id;
                if (scheduler._objects.Remove(id, out ScriptObject? deleted))
                {
                    deleted.Deleted = true;
                    foreach (Activation activation in deleted.Stack)
                        activation.Finished = true;
                    if (deleted == current.Owner)
                        throw new ExitScriptException();
                }
                return;
            default:
                if (RunHost(current, opcode, operands) != EventCommandFlow.Continue)
                    throw new InvalidOperationException($"The event host suspended immediate operation {opcode}.");
                return;
        }
    }

    internal static EventAwaitable ExecuteAsync(Opcode opcode, params Operand[] operands)
    {
        Activation current = Running();
        switch (opcode)
        {
            case Opcode.NOP:
                return Wait(1);
            case Opcode.WAIT:
                return Wait((int)operands[0]);
            case Opcode.REQSW:
            case Opcode.REQEW:
                return Request(current, (int)operands[1], (int)operands[0], (int)operands[2], opcode == Opcode.REQEW);
            case Opcode.REPLYSW:
            case Opcode.REPLYEW:
                return Request(current, current.Sender?.Id, (int)operands[0], (int)operands[1], opcode == Opcode.REPLYEW);
            case Opcode.WAITMES:
                return WaitWindow(current, (int)operands[0], static () => EventCommandFlow.Continue);
            case Opcode.MES:
                return WaitWindow(current, (int)operands[0], () => RunHost(current, opcode, operands));
            case Opcode.MESA:
                return WaitWindow(current, (int)operands[1], () => RunHost(current, opcode, operands));
        }

        EventCommandFlow result = RunHost(current, opcode, operands);
        if (result == EventCommandFlow.Continue)
            return Completed();
        return Suspend(current, () =>
        {
            if (result == EventCommandFlow.Yield)
                return true;
            result = RunHost(current, opcode, operands);
            return result == EventCommandFlow.Continue;
        });
    }

    internal static EventAwaitable WaitThenReturn(int frames) => Wait(frames).OnCompletion(static () => throw new ExitScriptException());

    internal static void DefinePolygon(Operand vertexCount, Vertex[] vertices)
    {
        var arguments = new Operand[1 + vertices.Length * 2];
        arguments[0] = vertexCount;
        for (int index = 0; index < vertices.Length; index++)
        {
            arguments[index * 2 + 1] = vertices[index].X;
            arguments[index * 2 + 2] = vertices[index].Z;
        }
        Execute(Opcode.QUAD, arguments);
    }

    internal static int Read(Excode opcode, params Operand[] operands)
    {
        Activation current = Running();
        return current.Owner.Scheduler.Host.Read(current.Owner.Id, opcode, operands);
    }

    private IEventHost Host => host;

    private static EventCommandFlow RunHost(Activation current, Opcode opcode, Operand[] operands)
    {
        EventCommandFlow result = current.Owner.Scheduler.Host.Execute(current.Owner.Id, opcode, operands);
        if (result == EventCommandFlow.EndEvent)
        {
            current.Owner.Scheduler.HasEnded = true;
            throw new ExitScriptException();
        }
        return result;
    }

    private static EventAwaitable WaitWindow(Activation current, int windowId, Func<EventCommandFlow> begin)
    {
        bool shown = begin() != EventCommandFlow.Retry;
        bool cleared = false;
        return Suspend(current, () =>
        {
            if (!shown)
            {
                shown = begin() != EventCommandFlow.Retry;
                return false;
            }
            if (cleared)
                return true;

            // EBin advances to the next entry after clearing a message wait.
            cleared = windowId == byte.MaxValue || !current.Owner.Scheduler.Host.IsWindowOpen(current.Owner.Id, windowId);
            return false;
        });
    }

    private static EventAwaitable Request(Activation current, int? targetId, int level, int handler, bool waitForReturn)
    {
        EventScheduler scheduler = current.Owner.Scheduler;
        bool accepted = scheduler.TryRequest(current, targetId, level, handler, out Activation? target);
        if (accepted && !waitForReturn)
            return Completed();

        return Suspend(current, () =>
        {
            if (accepted)
                return target is not { Finished: false };
            accepted = scheduler.TryRequest(current, targetId, level, handler, out target);
            return accepted && !waitForReturn;
        });
    }

    private bool TryRequest(Activation caller, int? targetId, int level, int handler, out Activation? activation)
    {
        activation = null;
        if (targetId is not int id)
            return false;
        if (id == byte.MaxValue)
            id = caller.Owner.Id;
        if (!_objects.TryGetValue(id, out ScriptObject? target))
            return false;
        if (level >= (target.Stack.TryPeek(out Activation? active) ? active.Level : 7))
            return false;
        if (target.Handlers.TryGetValue(handler, out Func<EventRoutine>? routine))
        {
            if (target == caller.Owner)
                throw new NotSupportedException("Scheduling a request on the currently executing object requires a VM continuation.");
            activation = new Activation(target, level, routine, caller.Owner);
            target.Stack.Push(activation);
        }
        return true;
    }

    private static Activation Running() => Current.Value ?? throw new InvalidOperationException("An event operation requires an active scheduler visit.");
    private static EventAwaitable Completed() => new(true, static _ => { });
    private static EventAwaitable Suspend(Activation current, Func<bool> ready) => new(false, resume => current.Pending = new(ready, resume));

    private sealed class ScriptObject(EventScheduler scheduler, int id, IReadOnlyDictionary<int, Func<EventRoutine>> handlers, ScriptMemory memory)
    {
        public EventScheduler Scheduler { get; } = scheduler;
        public int Id { get; } = id;
        public IReadOnlyDictionary<int, Func<EventRoutine>> Handlers { get; } = handlers;
        public ScriptMemory Memory { get; } = memory;
        public Stack<Activation> Stack { get; } = new();
        public bool Deleted { get; set; }
    }

    private sealed class Activation(ScriptObject owner, int level, Func<EventRoutine> handler, ScriptObject? sender)
    {
        public ScriptObject Owner { get; } = owner;
        public int Level { get; } = level;
        public Func<EventRoutine> Handler { get; } = handler;
        public ScriptObject? Sender { get; } = sender;
        public Task? Task { get; set; }
        public PendingWait? Pending { get; set; }
        public bool Finished { get; set; }
    }

    private sealed record PendingWait(Func<bool> Ready, Action Resume);
    private sealed class ExitScriptException : Exception;
}
