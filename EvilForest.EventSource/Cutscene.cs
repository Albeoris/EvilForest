using System.Reflection;

namespace EvilForest.EventSource;

public abstract partial class EventCutscene
{
    protected static CutsceneAction On<T>(T receiver, Func<T, EventRoutine> action) where T : EventParticipant =>
        CutsceneAction.Bind(receiver, action);

    protected static CutsceneAction On<T>(T receiver, Action<T> action) where T : EventParticipant =>
        CutsceneAction.Bind(receiver, action);

    protected static CutsceneAction On<T>(T definition, Func<T, EventRoutine> action, InstanceUid instance) where T : EventParticipant =>
        CutsceneAction.Bind(definition, action, instance);

    protected static CutsceneAction On<T>(T definition, Action<T> action, InstanceUid instance) where T : EventParticipant =>
        CutsceneAction.Bind(definition, action, instance);

    protected static CutsceneTrack Track<T>(T receiver, Func<T, EventRoutine> action) where T : EventActor =>
        new(receiver, actor => action((T)actor));

    // The original participant loops enter the phase independently. Each visit
    // runs its own track and joins via the same coordinator-polled completion.
    protected static async EventRoutine Together(EventActor participant, Func<EventRoutine> complete, params CutsceneTrack[] tracks)
    {
        EventScheduler.RequireActingReceiver(participant);
        CutsceneTrack track = tracks.Single(candidate => candidate.Receiver.GetType() == participant.GetType());
        await track.Run(participant);
        await complete();
    }
}

public readonly struct CutsceneTrack
{
    internal CutsceneTrack(EventActor receiver, Func<EventActor, EventRoutine> run)
    {
        Receiver = receiver;
        Run = run;
    }

    internal EventActor Receiver { get; }
    internal Func<EventActor, EventRoutine> Run { get; }
}

public readonly struct CutsceneAction
{
    internal byte Receiver { get; }
    internal byte Tag { get; }
    private readonly byte _definitionSlot;
    private readonly bool _isSequenceEntry;

    private CutsceneAction(byte receiver, byte tag, byte definitionSlot, bool isSequenceEntry)
    {
        Receiver = receiver;
        Tag = tag;
        _definitionSlot = definitionSlot;
        _isSequenceEntry = isSequenceEntry;
    }

    internal byte SequenceSlot => _isSequenceEntry ? _definitionSlot : throw new InvalidOperationException("A shared script requires the initial entry of a participant definition.");

    internal static CutsceneAction Bind(EventParticipant receiver, Delegate action, InstanceUid? instance = null)
    {
        if (action.Target is not EventCutscene || action.Method.GetCustomAttribute<ScriptAttribute>() is not { } entry ||
            action.Method.GetParameters() is not [var parameter] || parameter.ParameterType != receiver.GetType())
            throw new InvalidOperationException("The cutscene action must have a script entry for this receiver.");
        if (instance is { } live && (live.Value is 0 or >= 250 || receiver.IsControlledBinding))
            throw new InvalidOperationException("Use a concrete live instance UID for a declared participant handler.");
        return new CutsceneAction(instance?.Value ?? receiver.Receiver, checked((byte)entry.Id), receiver.Slot, entry.Id == 0 && instance is null && !receiver.IsControlledBinding);
    }
}

public sealed partial class ScriptService
{
    [SceneInstruction(Opcode.STARTSEQ)]
    public void RunSharedScript(CutsceneAction action, int mask = -1) =>
        Scripts.RunSharedScript(action.SequenceSlot, mask);

    [SceneInstruction(Opcode.REQ)]
    public void TryStart(CutsceneAction action, int level, int mask = -1) =>
        Scripts.RequestAsync(level, action.Receiver, action.Tag, mask);

    [SceneInstruction(Opcode.REQSW)]
    public EventAwaitable StartWhenReady(CutsceneAction action, int level, int mask = -1) =>
        Scripts.RequestWhenReady(level, action.Receiver, action.Tag, mask);

    [SceneInstruction(Opcode.REQEW)]
    public EventAwaitable RunAndWait(CutsceneAction action, int level, int mask = -1) =>
        Scripts.RequestAndWait(level, action.Receiver, action.Tag, mask);
}
