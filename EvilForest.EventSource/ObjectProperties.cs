namespace EvilForest.EventSource;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ObjectPropertyAttribute(byte ordinal, bool sector8 = false) : Attribute
{
    public byte Ordinal { get; } = ordinal;
    public bool Sector8 { get; } = sector8;
}

public readonly struct ObjectView
{
    private readonly byte _receiver;

    internal ObjectView(byte receiver) => _receiver = receiver;

    [ObjectProperty(0)]
    public int PositionX => Read(0);
    [ObjectProperty(1)]
    public int InvertedPositionY => Read(1);
    [ObjectProperty(2)]
    public int PositionZ => Read(2);
    [ObjectProperty(3)]
    public int FacingTurn => Read(3);
    [ObjectProperty(4)]
    public int ObjectFlags => Read(4);
    [ObjectProperty(5)]
    public int Uid => Read(5);
    [ObjectProperty(6)]
    public int ScriptPriority => Read(6);
    [ObjectProperty(7)]
    public int AnimationFrame => Read(7);

    private int Read(byte ordinal) => EventScheduler.ReadObjectProperty(_receiver, ordinal);
}

public static class ObjectProperties
{
    public static ObjectView Of(InstanceUid uid) => new(uid.Value);
    public static ObjectView Of(ActorSelector selector) => new((byte)selector);
}
