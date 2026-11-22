using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
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
    private const int SharedScriptUidOffset = 64;
    private static readonly AsyncLocal<Activation?> Current = new();
    private readonly Dictionary<int, ScriptObject> _objects = new();
    private readonly List<ScriptObject> _order = new();
    private readonly Dictionary<int, Definition> _definitions = new();
    private readonly Dictionary<MethodInfo, (EventCutscene Scene, int Slot)> _directors = new();
    private bool _ticking;
    private readonly int?[] _party = new int?[4];

    public int Frame { get; private set; } = -1;
    public bool HasEnded { get; private set; }
    public bool IsExternalModeActive { get; private set; }
    public int? ControlledActorId { get; set; }
    public EventSceneContext? SceneContext { get; private set; }

    public void SetPartyMember(int position, int? objectId) => _party[position] = objectId;

    public void RegisterProject(Assembly assembly)
    {
        var definitions = new Dictionary<int, Definition>();
        Type? contextType = assembly.GetCustomAttribute<EventSceneAttribute>()?.ContextType;
        PropertyInfo[] cutsceneBindings = contextType == null ? [] : ManagedSceneBindings.ReadCutscenes(contextType);
        EventSceneContext? context = contextType != null
            ? (EventSceneContext)Activator.CreateInstance(contextType, nonPublic: true)!
            : null;
        Type[] types = assembly.GetTypes();
        foreach (Type type in types)
        {
            const BindingFlags members = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            foreach (PropertyInfo property in type.GetProperties(members))
            {
                bool unbound = property.IsDefined(typeof(StorageFieldAttribute)) ||
                    property.IsDefined(typeof(StorageViewAttribute)) && property.GetMethod?.IsDefined(typeof(CompilerGeneratedAttribute)) == true;
                if (unbound)
                    throw new InvalidOperationException($"Storage property {type.FullName}.{property.Name} needs the EvilForest managed compiler.");
            }
            if (type.GetCustomAttribute<ObjectSlotAttribute>() is not ObjectSlotAttribute slot)
                continue;
            var handlers = new Dictionary<int, MethodInfo>();
            var directory = type.GetMethods(members)
                .Select(method => (Method: method, Script: method.GetCustomAttribute<ScriptAttribute>()))
                .Where(entry => entry.Script != null)
                .OrderBy(entry => entry.Script!.DirectoryIndex);
            foreach (var (method, script) in directory)
                handlers.TryAdd(script!.Id, method);
            if (_definitions.ContainsKey(slot.Id) || !definitions.TryAdd(slot.Id, new Definition(slot, type, handlers, context)))
                throw new InvalidOperationException($"Object slot {slot.Id} is already registered.");
        }
        var registered = new Dictionary<Type, EventCutscene>();
        if (context != null)
        {
            foreach (PropertyInfo binding in cutsceneBindings)
            {
                if (binding.GetValue(context) is not EventCutscene cutscene || cutscene.GetType() != binding.PropertyType || !registered.TryAdd(binding.PropertyType, cutscene))
                    throw new InvalidOperationException($"Invalid or duplicate cutscene binding {binding.Name}.");
                foreach (MethodInfo method in cutscene.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (method.GetCustomAttribute<ScriptAttribute>() is not { } script)
                        continue;
                    if (method.GetParameters() is not [var receiver] || receiver.ParameterType.GetCustomAttribute<ObjectSlotAttribute>() is not { } owner ||
                        !definitions.TryGetValue(owner.Id, out Definition? definition) || definition.Type != receiver.ParameterType ||
                        definition.Handlers.ContainsKey(script.Id) || definition.CutsceneHandlers.ContainsKey(script.Id))
                        throw new InvalidOperationException($"Invalid or duplicate cutscene entry {method.Name}.");
                    SceneDirector? choreography = null;
                    if (ManagedSceneBindings.DeclaresChoreography(method))
                    {
                        if (owner.Id != 0 || script.Id != 1 || method.IsStatic || method.IsVirtual || method.IsGenericMethod || method.ReturnType != typeof(EventRoutine))
                            throw new InvalidOperationException($"Invalid choreography callback {method.Name}.");
                        choreography = cutscene.BindChoreography(method, () => (EventRoutine)InvokeCutscene(cutscene, method, null)!, context);
                    }
                    definition.CutsceneHandlers.Add(script.Id, (cutscene, method, choreography));
                }
            }
        }
        if (types.FirstOrDefault(type => !type.IsAbstract && type.IsSubclassOf(typeof(EventCutscene)) && !registered.ContainsKey(type)) is { } unboundCutscene)
            throw new InvalidOperationException($"Cutscene {unboundCutscene.FullName} has no event context binding.");
        var directors = new Dictionary<MethodInfo, (EventCutscene Scene, int Slot)>();
        if (registered.Count != 0)
        {
            var entries = ManagedSceneBindings.ReadDirectors(types).ToDictionary(method => $"{method.DeclaringType!.FullName}.{method.Name}", StringComparer.Ordinal);
            var slots = SceneDirectorSlots.Assign(definitions.Keys, entries.Select(entry => (entry.Key, entry.Value.GetCustomAttribute<OriginalDirectorSlotAttribute>()?.Slot)));
            foreach (var (name, slot) in slots)
            {
                MethodInfo method = entries[name];
                if (!registered.TryGetValue(method.DeclaringType!, out EventCutscene? scene) ||
                    method.IsStatic || method.IsVirtual || method.IsGenericMethod || method.IsSpecialName ||
                    method.ReturnType != typeof(EventRoutine) || method.GetParameters().Length != 0 ||
                    method.IsDefined(typeof(ScriptAttribute)) || _directors.ContainsKey(method))
                    throw new InvalidOperationException($"Invalid scene director {name}.");
                var declaration = new ObjectSlotAttribute(slot, 0, 0, 0, -1, 0, (byte)ParticipantKind.Thread, "", "");
                SceneDirector run = method.CreateDelegate<SceneDirector>(scene);
                if (ManagedSceneBindings.DeclaresChoreography(method))
                    run = scene.BindChoreography(run, context!);
                if (_definitions.ContainsKey(slot) || !definitions.TryAdd(slot, new Definition(declaration, method.DeclaringType!, new(), context)
                    { Director = run.Invoke }))
                    throw new InvalidOperationException($"Object slot {slot} is already registered.");
                directors.Add(method, (scene, slot));
            }
        }
        if (context != null)
            SceneContext = context;
        foreach (var (id, definition) in definitions)
            _definitions.Add(id, definition);
        foreach (var (method, binding) in directors)
            _directors.Add(method, binding);
    }

    internal static void StartDirector(ScriptService scripts, SceneDirector director)
    {
        EventScheduler scheduler = Running().Owner.Scheduler;
        if (!ReferenceEquals(scripts, scheduler.SceneContext?.Scripts) ||
            !scheduler._directors.TryGetValue(director.Method, out var binding) ||
            !ReferenceEquals(binding.Scene, director.Target) || director.GetInvocationList().Length != 1)
            throw new InvalidOperationException("Start a director on the registered cutscene instance in the current event context.");
        Scripts.StartThread(binding.Slot, 0);
    }

    public void StartScene()
    {
        if (_ticking || _objects.Count != 0)
            throw new InvalidOperationException("A scene requires an idle, empty scheduler.");
        Spawn(0, 0, ParticipantKind.Thread, skipFirstVisit: false);
    }

    public void CreateParticipant(int slot, int uid = 0)
    {
        if (_ticking)
            throw new InvalidOperationException("Create participants between event ticks.");
        Spawn(slot, uid, kind: null);
    }

    public bool TryTrigger(int uid, int tag, int level)
    {
        if (_ticking)
            throw new InvalidOperationException("Trigger callbacks between event ticks.");
        if (!_objects.TryGetValue(uid, out ScriptObject? target) ||
            level >= (target.Stack.TryPeek(out Activation? active) ? active.Level : 7) ||
            !target.Handlers.TryGetValue(tag, out var routine))
            return false;
        target.Stack.Push(new Activation(target, level, routine, null));
        return true;
    }

    private int? ResolveSelector(Activation caller, int selector) => selector switch
    {
        250 => ControlledActorId,
        >= 251 and <= 254 => _party[selector - 251],
        255 => caller.Owner.ActingObjectId,
        _ => selector
    };

    public void AddObject(int id, int instanceByteCount, IReadOnlyDictionary<int, Func<EventRoutine>> handlers, int? initialHandler = null, int initialLevel = 7)
    {
        var entry = new ScriptObject(this, id, handlers, new ScriptMemory(globals, location, new byte[instanceByteCount]));
        if (initialHandler is int handler)
            entry.Stack.Push(new Activation(entry, initialLevel, handlers[handler], null));
        _objects.Add(id, entry);
        _order.Add(entry);
    }

    private void Spawn(int slot, int uid, ParticipantKind? kind, bool skipFirstVisit = true, int? sequenceOwner = null)
    {
        if (!_definitions.TryGetValue(slot, out var definition) || kind is { } expected && definition.Slot.Type != (byte)expected)
            throw new InvalidOperationException($"No matching definition is registered for object slot {slot}.");
        if (uid == 0)
            uid = slot;
        Delete(uid);
        EventParticipant? receiver = null;
        if (typeof(EventParticipant).IsAssignableFrom(definition.Type))
        {
            receiver = (EventParticipant)Activator.CreateInstance(definition.Type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null, args: [definition.Context ?? throw new InvalidOperationException("Missing scene context.")], culture: null)!;
            receiver.LiveUid = uid;
        }
        var handlers = new Dictionary<int, Func<EventRoutine>>();
        foreach (var (tag, method) in definition.Handlers)
        {
            if (method.ReturnType == typeof(void))
            {
                Action run = method.IsStatic ? method.CreateDelegate<Action>() : method.CreateDelegate<Action>(receiver);
                handlers.Add(tag, () => EventRoutine.Run(run));
            }
            else
            {
                handlers.Add(tag, method.IsStatic ? method.CreateDelegate<Func<EventRoutine>>() : method.CreateDelegate<Func<EventRoutine>>(receiver));
            }
        }
        foreach (var (tag, (cutscene, method, choreography)) in definition.CutsceneHandlers)
        {
            handlers.Add(tag, choreography != null ? choreography.Invoke : method.ReturnType == typeof(void)
                ? () => EventRoutine.Run(() => InvokeCutscene(cutscene, method, receiver!))
                : () => (EventRoutine)InvokeCutscene(cutscene, method, receiver!)!);
        }
        if (definition.Director is { } direct)
        {
            handlers.Add(0, static () => EventRoutine.Run(static () => { }));
            handlers.Add(1, direct);
        }
        var entry = new ScriptObject(this, uid, handlers, new ScriptMemory(globals, location, new byte[(definition.Slot.VariableCount + 3) & ~3]))
        {
            Participant = receiver,
            SkipFirstVisit = skipFirstVisit,
            DefinitionSlot = slot,
            SequenceOwnerId = sequenceOwner
        };
        if (handlers.TryGetValue(0, out Func<EventRoutine>? init))
            entry.Stack.Push(new Activation(entry, 0, init, null) { IsInit = true });
        _objects.Add(uid, entry);
        _order.Add(entry);
    }

    private ScriptObject? Delete(int uid)
    {
        if (!_objects.Remove(uid, out ScriptObject? deleted))
            return null;
        deleted.Deleted = true;
        deleted.Stack.Clear();
        if (ControlledActorId == uid)
            ControlledActorId = 0;
        return deleted;
    }

    public void Tick()
    {
        if (HasEnded || IsExternalModeActive)
            return;
        if (_ticking)
            throw new InvalidOperationException("An event tick cannot start another tick.");

        _ticking = true;
        try
        {
            Frame++;
            // A host may append newly created objects during the current visit.
            for (int index = 0; index < _order.Count && !HasEnded && !IsExternalModeActive; index++)
                Visit(_order[index]);
        }
        finally
        {
            _order.RemoveAll(entry => entry.Deleted);
            _ticking = false;
        }
    }

    public void ResumeExternalMode()
    {
        if (_ticking || !IsExternalModeActive)
            throw new InvalidOperationException("Resume an active external mode between event ticks.");
        IsExternalModeActive = false;
    }

    private static void Visit(ScriptObject entry)
    {
        if (entry.Deleted)
            return;
        if (entry.SkipFirstVisit)
        {
            entry.SkipFirstVisit = false;
            return;
        }
        if (!entry.Stack.TryPeek(out Activation? routine))
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

            if (entry.Stack.TryPeek(out Activation? preempting) && preempting != routine && preempting.Sender == entry)
            {
                Visit(entry);
                return;
            }
            if (entry.Deleted || !routine.Task.IsCompleted)
                return;
            routine.Task.GetAwaiter().GetResult();
            CompleteActivation(entry, routine);
        }
        catch (ExitScriptException)
        {
            CompleteActivation(entry, routine);
        }
        finally
        {
            Current.Value = previous;
        }
    }

    private static void CompleteActivation(ScriptObject entry, Activation routine)
    {
        if (entry.Deleted || !entry.Stack.TryPeek(out Activation? active) || active != routine)
            return;
        // Only a live activation's return releases a caller that is waiting for it.
        routine.Returned = true;
        entry.Stack.Pop();
        if (routine.IsInit && entry.Handlers.TryGetValue(1, out Func<EventRoutine>? loop))
            entry.Stack.Push(new Activation(entry, 7, loop, null));
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

    internal static void RequireActingDefinition(int slot)
    {
        ScriptObject entry = Running().Owner;
        if (!entry.Scheduler._objects.TryGetValue(entry.ActingObjectId, out ScriptObject? acting) || acting.DefinitionSlot != slot)
            throw new InvalidOperationException($"The acting object does not use the actor definition in slot {slot}.");
    }

    internal static void RequireActingReceiver(EventActor actor)
    {
        ScriptObject entry = Running().Owner;
        if (entry.ActingObjectId != actor.Receiver)
            throw new InvalidOperationException("The receiver is not the acting instance.");
        RequireActingDefinition(actor.Slot);
    }

    internal static void RequirePerformingParticipant(EventParticipant participant)
    {
        Activation activation = Running();
        if (!ReferenceEquals(activation.Owner.Participant, participant) ||
            !activation.Owner.Handlers.TryGetValue(1, out var loop) || !ReferenceEquals(activation.Handler, loop))
            throw new InvalidOperationException("Choreography must run on the live participant's own loop.");
    }

    internal static void Execute(Opcode opcode, params Operand[] operands)
    {
        Activation current = Running();
        switch (opcode)
        {
            case Opcode.REQ:
                current.Owner.Scheduler.TryRequest(current, current.Owner.Scheduler.ResolveSelector(current, (int)operands[1]), (int)operands[0], (int)operands[2], out _);
                return;
            case Opcode.REPLY:
                current.Owner.Scheduler.TryRequest(current, current.Sender?.Id, (int)operands[0], (int)operands[1], out _);
                return;
            case Opcode.CC:
                if (RunHost(current, opcode, operands) != EventCommandFlow.Continue)
                    throw new InvalidOperationException("The event host suspended control assignment.");
                current.Owner.Scheduler.ControlledActorId = current.Owner.ActingObjectId;
                return;
            case Opcode.NEW or Opcode.NEW2 or Opcode.NEW3 when current.Owner.Scheduler._definitions.Count != 0:
                EventScheduler scene = current.Owner.Scheduler;
                int slot = (int)operands[0];
                if (opcode == Opcode.NEW3 && slot is >= 251 and <= 254)
                {
                    if (scene._party[slot - 251] is not int partySlot)
                        return;
                    slot = partySlot;
                }
                if (RunHost(current, opcode, operands) != EventCommandFlow.Continue)
                    throw new InvalidOperationException($"The event host suspended creation of object slot {slot}.");
                ParticipantKind kind = opcode switch
                {
                    Opcode.NEW => ParticipantKind.Thread,
                    Opcode.NEW2 => ParticipantKind.Region,
                    _ => ParticipantKind.Actor
                };
                int uid = (int)operands[1];
                scene.Spawn(slot, uid, kind);
                if (current.Owner.Deleted)
                    throw new ExitScriptException();
                return;
            case Opcode.STARTSEQ when current.Owner.Scheduler._definitions.Count != 0:
                current.Owner.Scheduler.Spawn((int)operands[0], SharedScriptId(current), kind: null, sequenceOwner: current.Owner.Id);
                return;
            case Opcode.ENDSEQ when current.Owner.Scheduler._definitions.Count != 0:
                current.Owner.Scheduler.Delete(SharedScriptId(current));
                return;
            case Opcode.DELETE:
                EventScheduler scheduler = current.Owner.Scheduler;
                int selector = (int)operands[0];
                int? target = selector == 255 ? current.Owner.Id : scheduler.ResolveSelector(current, selector);
                if (target is int id &&
                    scheduler.Delete(id) == current.Owner)
                    throw new ExitScriptException();
                return;
            case Opcode.Return:
                throw new ExitScriptException();
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
            case Opcode.MINIGAME:
                EventScheduler scheduler = current.Owner.Scheduler;
                bool accepted = AcceptExternalMode();
                return Suspend(current, () =>
                {
                    if (accepted)
                        return !scheduler.IsExternalModeActive;
                    accepted = AcceptExternalMode();
                    return false;
                });

                bool AcceptExternalMode()
                {
                    if (RunHost(current, opcode, operands) == EventCommandFlow.Retry)
                        return false;
                    scheduler.IsExternalModeActive = true;
                    return true;
                }
            case Opcode.NOP:
                return Wait(1);
            case Opcode.WAIT:
                return Wait((int)operands[0]);
            case Opcode.REQSW:
            case Opcode.REQEW:
                return Request(current, () => current.Owner.Scheduler.ResolveSelector(current, (int)operands[1]), (int)operands[0], (int)operands[2], opcode == Opcode.REQEW);
            case Opcode.REPLYSW:
            case Opcode.REPLYEW:
                return Request(current, () => current.Sender?.Id, (int)operands[0], (int)operands[1], opcode == Opcode.REPLYEW);
            case Opcode.WAITSEQ when current.Owner.Scheduler._definitions.Count != 0:
                bool Finished() => !current.Owner.Scheduler._objects.ContainsKey(SharedScriptId(current));
                return Finished() ? Completed() : Suspend(current, Finished);
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
        return current.Owner.Scheduler.Host.Read(current.Owner.ActingObjectId, opcode, operands);
    }

    internal static int ReadObjectProperty(byte receiver, byte ordinal)
    {
        Activation current = Running();
        EventScheduler scheduler = current.Owner.Scheduler;
        int target = scheduler.ResolveSelector(current, receiver) ?? -1;
        return scheduler.Host.Read(current.Owner.ActingObjectId, Excode.B_OBJSPECA, [(Operand)target, (Operand)(int)ordinal]);
    }

    private IEventHost Host => host;

    private static EventCommandFlow RunHost(Activation current, Opcode opcode, Operand[] operands)
    {
        EventCommandFlow result = current.Owner.Scheduler.Host.Execute(current.Owner.ActingObjectId, opcode, operands);
        // Memoria's MAPJUMP transfers control out of the current field even when
        // the host only records the map change and reports synchronous completion.
        if (result == EventCommandFlow.EndEvent || opcode == Opcode.MAPJUMP && result == EventCommandFlow.Continue)
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
            cleared = windowId == byte.MaxValue || !current.Owner.Scheduler.Host.IsWindowOpen(current.Owner.ActingObjectId, windowId);
            return false;
        });
    }

    private static EventAwaitable Request(Activation current, Func<int?> targetId, int level, int handler, bool waitForReturn)
    {
        EventScheduler scheduler = current.Owner.Scheduler;
        bool accepted = scheduler.TryRequest(current, targetId(), level, handler, out Activation? target, allowSelf: true);
        if (accepted && CanContinue())
            return Completed();

        return Suspend(current, () =>
        {
            if (accepted)
                return target is not { Returned: false };
            accepted = scheduler.TryRequest(current, targetId(), level, handler, out target, allowSelf: true);
            return accepted && CanContinue();
        });

        bool CanContinue() => target == null || !waitForReturn && target.Owner != current.Owner;
    }

    private bool TryRequest(Activation caller, int? targetId, int level, int handler, out Activation? activation, bool allowSelf = false)
    {
        activation = null;
        if (targetId is not int id)
            return false;
        if (!_objects.TryGetValue(id, out ScriptObject? target))
            return false;
        if (level >= (target.Stack.TryPeek(out Activation? active) ? active.Level : 7))
            return false;
        if (target.Handlers.TryGetValue(handler, out Func<EventRoutine>? routine))
        {
            if (target == caller.Owner && !allowSelf)
                throw new NotSupportedException("Scheduling a request on the currently executing object requires a VM continuation.");
            activation = new Activation(target, level, routine, caller.Owner);
            target.Stack.Push(activation);
        }
        return true;
    }

    private static Activation Running() => Current.Value ?? throw new InvalidOperationException("An event operation requires an active scheduler visit.");
    private static int SharedScriptId(Activation activation) => activation.Owner.Id + SharedScriptUidOffset;
    private static EventAwaitable Completed() => new(true, static _ => { });
    private static EventAwaitable Suspend(Activation current, Func<bool> ready) => new(false, resume => current.Pending = new(ready, resume));

    private sealed class ScriptObject(EventScheduler scheduler, int id, IReadOnlyDictionary<int, Func<EventRoutine>> handlers, ScriptMemory memory)
    {
        public EventParticipant? Participant { get; set; }
        public EventScheduler Scheduler { get; } = scheduler;
        public int Id { get; } = id;
        public IReadOnlyDictionary<int, Func<EventRoutine>> Handlers { get; } = handlers;
        public ScriptMemory Memory { get; } = memory;
        public Stack<Activation> Stack { get; } = new();
        public bool Deleted { get; set; }
        public bool SkipFirstVisit { get; set; }
        public int? DefinitionSlot { get; init; }
        public int? SequenceOwnerId { get; init; }
        public int ActingObjectId => SequenceOwnerId ?? Id;
    }

    private sealed class Activation(ScriptObject owner, int level, Func<EventRoutine> handler, ScriptObject? sender)
    {
        public ScriptObject Owner { get; } = owner;
        public int Level { get; } = level;
        public Func<EventRoutine> Handler { get; } = handler;
        public ScriptObject? Sender { get; } = sender;
        public Task? Task { get; set; }
        public PendingWait? Pending { get; set; }
        public bool Returned { get; set; }
        public bool IsInit { get; init; }
    }

    private sealed record PendingWait(Func<bool> Ready, Action Resume);
    private static object? InvokeCutscene(EventCutscene cutscene, MethodInfo method, EventParticipant? receiver)
    {
        try
        {
            return method.Invoke(cutscene, [receiver]);
        }
        catch (TargetInvocationException error) when (error.InnerException != null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw(error.InnerException);
            throw;
        }
    }

    private sealed record Definition(ObjectSlotAttribute Slot, Type Type, Dictionary<int, MethodInfo> Handlers, EventSceneContext? Context)
    {
        public Dictionary<int, (EventCutscene Cutscene, MethodInfo Method, SceneDirector? Choreography)> CutsceneHandlers { get; } = new();
        public Func<EventRoutine>? Director { get; init; }
    }
    private sealed class ExitScriptException : Exception;
}
