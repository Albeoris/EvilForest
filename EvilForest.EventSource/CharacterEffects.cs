namespace EvilForest.EventSource;

[AttributeUsage(AttributeTargets.Method)]
public sealed class CharacterEffectAttribute(byte command) : Attribute
{
    public byte Command { get; } = command;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class ResourceValueAttribute(ushort value) : Attribute
{
    public ushort Value { get; } = value;
}

public readonly record struct AnimationId(ushort Value)
{
    public static AnimationId FromScript(int value) => new(unchecked((ushort)value));
}

public readonly record struct SoundEffectId(ushort Value)
{
    public static SoundEffectId FromScript(int value) => new(unchecked((ushort)value));
}

public enum FieldBlendMode
{
    ShaderAbr0,
    ShaderAbr1,
    ShaderAbr2,
    ShaderAbr3,
    Disabled = 255
}

public readonly struct ScriptFlag
{
    internal int EncodedValue { get; }
    private ScriptFlag(int value) => EncodedValue = value;
    public static ScriptFlag FromScript(int value) => new(value);
    public static implicit operator ScriptFlag(bool value) => new(value ? 1 : 0);
}

public readonly struct CharacterEffectEncoding(int unused1 = 0, int unused2 = 0, int mask = -1)
{
    public int Unused1 { get; } = unused1;
    public int Unused2 { get; } = unused2;
    public int Mask { get; } = mask;
}

public static partial class Animations
{
    public static class Zidane
    {
        [ResourceValue(Resources.ANH_MAIN_F0_ZDN_RUN)]
        public static AnimationId Run => new(Resources.ANH_MAIN_F0_ZDN_RUN);
    }
}

public static class CharacterEffects
{
    public static ActorAnimation ActingAnimation { get; } = new();
}

public sealed partial class ActorAnimation
{
    private readonly bool _usesCurrentActor;

    internal ActorAnimation()
    {
        _usesCurrentActor = true;
    }

    [CharacterEffect(0)]
    public void SetFieldBlendMode(FieldBlendMode mode, CharacterEffectEncoding encoding = default) =>
        Dispatch(0, (int)mode, encoding.Unused1, encoding.Unused2);

    [CharacterEffect(4)]
    public void SetModelSlicing(ScriptFlag enabled, int slice, CharacterEffectEncoding encoding = default) =>
        Dispatch(4, enabled.EncodedValue, slice, encoding.Unused1);

    [CharacterEffect(8)]
    public void SetMirrorVisibility(ScriptFlag enabled, CharacterEffectEncoding encoding = default) =>
        Dispatch(8, enabled.EncodedValue, encoding.Unused1, encoding.Unused2);

    [CharacterEffect(9)]
    public void SetMirrorPoint(int x, int y, int z, CharacterEffectEncoding encoding = default) =>
        Dispatch(9, x, y, z);

    [CharacterEffect(10)]
    public void SetMirrorNormal(int fixedX, int fixedY, int fixedZ, CharacterEffectEncoding encoding = default) =>
        Dispatch(10, fixedX, fixedY, fixedZ);

    [CharacterEffect(11)]
    public void SetMirrorColor(int red, int green, int blue, CharacterEffectEncoding encoding = default) =>
        Dispatch(11, red, green, blue);

    [CharacterEffect(16)]
    public void SetPrimaryFrameSound(AnimationId animation, int frame, SoundEffectId sound, CharacterEffectEncoding encoding = default) =>
        Dispatch(16, animation.Value, frame, sound.Value);

    [CharacterEffect(17)]
    public void SetAlternateFrameSound(AnimationId animation, int frame, SoundEffectId sound, CharacterEffectEncoding encoding = default) =>
        Dispatch(17, animation.Value, frame, sound.Value);

    [CharacterEffect(18)]
    public void SetFrameSoundPitchRandomization(AnimationId animation, int frame, ScriptFlag enabled, CharacterEffectEncoding encoding = default) =>
        Dispatch(18, animation.Value, frame, enabled.EncodedValue);

    [CharacterEffect(19)]
    public void RemoveFrameSound(AnimationId animation, int frame, CharacterEffectEncoding encoding = default) =>
        Dispatch(19, animation.Value, frame, encoding.Unused1);

    private void Dispatch(int command, int first, int second, int third)
    {
        RequireActing();
        EventScheduler.Execute(Opcode.CHRFX, command, first, second, third);
    }
}
