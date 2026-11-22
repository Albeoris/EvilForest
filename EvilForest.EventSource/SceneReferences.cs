using System.Reflection;

namespace EvilForest.EventSource;

public enum ParticipantKind : byte
{
    Thread,
    Region,
    Actor
}

public sealed class ParticipantDefinition(Type declaration, ParticipantKind kind)
{
    public byte Slot { get; } = checked((byte)(declaration.GetCustomAttribute<ObjectSlotAttribute>()?.Id
        ?? throw new ArgumentException("A participant declaration needs an object slot.", nameof(declaration))));
    public ParticipantKind Kind { get; } = kind;
    public ActorAnimation Animation => new(SlotFor(ParticipantKind.Actor));

    internal byte SlotFor(ParticipantKind expected)
    {
        if (Kind != expected)
            throw new InvalidOperationException($"Cannot start a {Kind} definition as {expected}.");
        return Slot;
    }
}

public sealed partial class ActorAnimation
{
    private readonly byte _slot;
    private readonly EventActor? _actor;

    internal ActorAnimation(byte slot) => _slot = slot;
    internal ActorAnimation(EventActor actor) => _actor = actor;

    [SceneInstruction(Opcode.AIDLE)]
    public void SetIdle(Operand animation, int mask = -1)
    {
        RequireActing();
        Animations.SetIdle(animation, mask);
    }

    private void RequireActing()
    {
        if (_actor != null)
            EventScheduler.RequireActingReceiver(_actor);
        else if (!_usesCurrentActor)
            EventScheduler.RequireActingDefinition(_slot);
    }
}

public readonly record struct InstanceUid(byte Value);

public enum ActorSelector : byte
{
    Controlled = 250,
    PartyMember1 = 251,
    PartyMember2 = 252,
    PartyMember3 = 253,
    PartyMember4 = 254,
    Current = 255
}

public sealed class HandlerReference
{
    public HandlerReference(ActorSelector receiver, Action handler) : this(receiver, handler.Method)
    {
    }

    public HandlerReference(ActorSelector receiver, Func<EventRoutine> handler) : this(receiver, handler.Method)
    {
    }

    private HandlerReference(ActorSelector receiver, MethodInfo handler)
    {
        Receiver = receiver;
        Tag = checked((byte)(handler.GetCustomAttribute<ScriptAttribute>()?.Id
            ?? throw new ArgumentException("A handler reference needs a script declaration.", nameof(handler))));
    }

    public ActorSelector Receiver { get; }
    public byte Tag { get; }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class SceneInstructionAttribute(Opcode opcode) : Attribute
{
    public Opcode Opcode { get; } = opcode;
}

public static partial class Scripts
{
    [SceneInstruction(Opcode.NEW)]
    public static void StartThread(ParticipantDefinition definition, InstanceUid uid = default) =>
        StartThread(definition.SlotFor(ParticipantKind.Thread), uid.Value);

    [SceneInstruction(Opcode.NEW2)]
    public static void StartRegion(ParticipantDefinition definition, InstanceUid uid = default) =>
        StartRegion(definition.SlotFor(ParticipantKind.Region), uid.Value);

    [SceneInstruction(Opcode.NEW3)]
    public static void StartActor(ParticipantDefinition definition, InstanceUid uid = default) =>
        StartActor(definition.SlotFor(ParticipantKind.Actor), uid.Value);

    [SceneInstruction(Opcode.STARTSEQ)]
    public static void RunSharedScript(ParticipantDefinition definition, int mask = -1) => RunSharedScript(definition.Slot, mask);

    [SceneInstruction(Opcode.REQ)]
    public static void RequestAsync(HandlerReference handler, Operand level, int mask = -1) =>
        RequestAsync(level, (byte)handler.Receiver, handler.Tag, mask);

    [SceneInstruction(Opcode.REQEW)]
    public static EventAwaitable RequestAndWait(HandlerReference handler, Operand level, int mask = -1) =>
        RequestAndWait(level, (byte)handler.Receiver, handler.Tag, mask);

    [SceneInstruction(Opcode.REQSW)]
    public static EventAwaitable RequestWhenReady(HandlerReference handler, Operand level, int mask = -1) =>
        RequestWhenReady(level, (byte)handler.Receiver, handler.Tag, mask);
}
