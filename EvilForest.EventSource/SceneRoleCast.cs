namespace EvilForest.EventSource;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RoleStorageAliasAttribute : Attribute;

public sealed class SceneRoleCast<TActor>(TActor definition, params InstanceUid[] instances) where TActor : EventActor
{
    public Selection Resolve(int choice) => new(definition, (uint)choice < (uint)instances.Length ? instances[choice] : null);

    public readonly struct Selection
    {
        internal TActor Definition { get; }
        internal InstanceUid? Instance { get; }

        internal Selection(TActor definition, InstanceUid? instance)
        {
            Definition = definition;
            Instance = instance;
        }
    }
}

public sealed partial class ScriptService
{
    public EventAwaitable StartWhenReady<TActor>(SceneRoleCast<TActor>.Selection receiver, Func<EventRoutine> action, int level) where TActor : EventActor
    {
        if (!ReferenceEquals(receiver.Definition, action.Target))
            throw new InvalidOperationException("The role request must use a handler on the cast's actor definition.");
        return receiver.Instance is { } instance
            ? RequestWhenReady(action, level, instance)
            : new EventAwaitable(true, static _ => { });
    }
}
