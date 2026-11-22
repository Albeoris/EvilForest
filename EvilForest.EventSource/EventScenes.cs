using System.Reflection;

namespace EvilForest.EventSource;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class EventSceneAttribute(Type contextType) : Attribute
{
    public Type ContextType { get; } = contextType;
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class SceneRootAttribute : Attribute;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SceneCutsceneAttribute : Attribute;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ControlledActorAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ControlledHandlerAttribute(byte tag) : Attribute
{
    public byte Tag { get; } = tag;
}

public abstract class EventSceneContext
{
    public ScriptService Scripts { get; } = new();
    public GlobalState Globals { get; } = new();

    public static T BindControlled<T>(T prototype) where T : EventActor
    {
        prototype.IsControlledBinding = true;
        return prototype;
    }
}

public abstract class EventParticipant(EventSceneContext context)
{
    protected EventSceneContext Context { get; } = context;
    internal bool BelongsTo(EventSceneContext scene) => ReferenceEquals(Context, scene);
    internal byte Slot => checked((byte)(GetType().GetCustomAttribute<ObjectSlotAttribute>()?.Id
        ?? throw new InvalidOperationException("A participant needs an object slot declaration.")));
    internal int? LiveUid { get; set; }
    internal bool IsControlledBinding { get; set; }
    internal byte Receiver => IsControlledBinding ? (byte)ActorSelector.Controlled : checked((byte)(LiveUid ?? Slot));
}

public abstract class EventThread(EventSceneContext context) : EventParticipant(context);
public abstract class EventRegion(EventSceneContext context) : EventParticipant(context);

public interface IActor
{
    ActorFacing Facing { get; }
    ObjectView Properties { get; }
}

public abstract class EventActor(EventSceneContext context) : EventParticipant(context), IActor
{
    public ActorFacing Facing => new(Receiver);
    public ObjectView Properties => new(Receiver);
    public ActorAnimation Animation => new(this);
    public ActorActions Actions => new(this);
}

public sealed partial class ActorActions
{
    private readonly EventActor _actor;

    internal ActorActions(EventActor actor) => _actor = actor;

    private void RequireActing() => EventScheduler.RequireActingReceiver(_actor);
}

public class ControlledActorTarget : IActor
{
    public ActorFacing Facing => new((byte)ActorSelector.Controlled);
    public ObjectView Properties => new((byte)ActorSelector.Controlled);
}

public enum FacingSector
{
    Sector0,
    Sector1,
    Sector2,
    Sector3,
    Sector4,
    Sector5,
    Sector6,
    Sector7
}

public readonly struct ActorFacing(byte receiver)
{
    [ObjectProperty(3)]
    public int RawTurn => EventScheduler.ReadObjectProperty(receiver, 3);
    [ObjectProperty(3, sector8: true)]
    public FacingSector Sector8 => (FacingSector)(RawTurn >> 5);
}

public sealed partial class ScriptService
{
    [SceneInstruction(Opcode.NEW)]
    public void StartThread(EventThread definition, InstanceUid uid = default) => Scripts.StartThread(definition.Slot, uid.Value);

    [SceneInstruction(Opcode.NEW2)]
    public void StartRegion(EventRegion definition, InstanceUid uid = default) => Scripts.StartRegion(definition.Slot, uid.Value);

    [SceneInstruction(Opcode.NEW3)]
    public void StartActor(EventActor definition, InstanceUid uid = default) => Scripts.StartActor(definition.Slot, uid.Value);

    [SceneInstruction(Opcode.STARTSEQ)]
    public void RunSharedScript(EventParticipant definition, int mask = -1) => Scripts.RunSharedScript(definition.Slot, mask);

    [SceneInstruction(Opcode.REQ)]
    public void RequestAsync(Action handler, int level, int mask = -1) => RequestAsyncCore(handler, level, mask);

    [SceneInstruction(Opcode.REQ)]
    public void RequestAsync(Func<EventRoutine> handler, int level, int mask = -1) => RequestAsyncCore(handler, level, mask);

    [SceneInstruction(Opcode.REQEW)]
    public EventAwaitable RequestAndWait(Action handler, int level, int mask = -1) => RequestCore(handler, level, mask, waitForReturn: true);

    [SceneInstruction(Opcode.REQEW)]
    public EventAwaitable RequestAndWait(Func<EventRoutine> handler, int level, int mask = -1) => RequestCore(handler, level, mask, waitForReturn: true);

    [SceneInstruction(Opcode.REQSW)]
    public EventAwaitable RequestWhenReady(Action handler, int level, int mask = -1) => RequestCore(handler, level, mask, waitForReturn: false);

    [SceneInstruction(Opcode.REQSW)]
    public EventAwaitable RequestWhenReady(Func<EventRoutine> handler, int level, int mask = -1) => RequestCore(handler, level, mask, waitForReturn: false);

    [SceneInstruction(Opcode.REQ)]
    public void RequestAsync(Action handler, int level, InstanceUid instance, int mask = -1) => RequestAsyncCore(handler, level, mask, instance);

    [SceneInstruction(Opcode.REQ)]
    public void RequestAsync(Func<EventRoutine> handler, int level, InstanceUid instance, int mask = -1) => RequestAsyncCore(handler, level, mask, instance);

    [SceneInstruction(Opcode.REQSW)]
    public EventAwaitable RequestWhenReady(Action handler, int level, InstanceUid instance, int mask = -1) => RequestCore(handler, level, mask, waitForReturn: false, instance);

    [SceneInstruction(Opcode.REQSW)]
    public EventAwaitable RequestWhenReady(Func<EventRoutine> handler, int level, InstanceUid instance, int mask = -1) => RequestCore(handler, level, mask, waitForReturn: false, instance);

    [SceneInstruction(Opcode.REQEW)]
    public EventAwaitable RequestAndWait(Action handler, int level, InstanceUid instance, int mask = -1) => RequestCore(handler, level, mask, waitForReturn: true, instance);

    [SceneInstruction(Opcode.REQEW)]
    public EventAwaitable RequestAndWait(Func<EventRoutine> handler, int level, InstanceUid instance, int mask = -1) => RequestCore(handler, level, mask, waitForReturn: true, instance);

    private static void RequestAsyncCore(Delegate handler, int level, int mask, InstanceUid? instance = null)
    {
        var (receiver, tag) = Bind(handler);
        Scripts.RequestAsync(level, instance?.Value ?? receiver, tag, mask);
    }

    private static EventAwaitable RequestCore(Delegate handler, int level, int mask, bool waitForReturn, InstanceUid? instance = null)
    {
        var (receiver, tag) = Bind(handler);
        receiver = instance?.Value ?? receiver;
        return waitForReturn ? Scripts.RequestAndWait(level, receiver, tag, mask) : Scripts.RequestWhenReady(level, receiver, tag, mask);
    }

    private static (byte Receiver, byte Tag) Bind(Delegate handler)
    {
        if (handler.Target is EventParticipant participant && handler.Method.GetCustomAttribute<ScriptAttribute>() is { } script)
            return (participant.Receiver, checked((byte)script.Id));
        if (handler.Target is ControlledActorTarget target)
        {
            Type type = target.GetType();
            foreach (Type contract in type.GetInterfaces())
            {
                InterfaceMapping implementation = type.GetInterfaceMap(contract);
                int index = Array.IndexOf(implementation.TargetMethods, handler.Method);
                if (index >= 0 && implementation.InterfaceMethods[index].GetCustomAttribute<ControlledHandlerAttribute>() is { } entry)
                    return ((byte)ActorSelector.Controlled, entry.Tag);
            }
        }
        throw new InvalidOperationException("A request needs a bound scene handler.");
    }
}
