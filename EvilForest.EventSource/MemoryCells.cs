using System.Runtime.InteropServices;

namespace EvilForest.EventSource;

// These flags select equivalent EV encodings of the same underlying memory cell.
[AttributeUsage(AttributeTargets.Field)]
public sealed class StorageEncodingAttribute(bool wide = false, bool alternatePrefix = false, bool signedBit = false, bool unsigned24 = false) : Attribute
{
    public bool Wide { get; } = wide;
    public bool AlternatePrefix { get; } = alternatePrefix;
    public bool SignedBit { get; } = signedBit;
    public bool Unsigned24 { get; } = unsigned24;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ByteCell
{
    private byte _value;
    public int Value
    {
        readonly get => _value;
        set => _value = unchecked((byte)value);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SByteCell
{
    private sbyte _value;
    public int Value
    {
        readonly get => _value;
        set => _value = unchecked((sbyte)value);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Int16Cell
{
    private byte _low;
    private byte _high;

    public int Value
    {
        readonly get => unchecked((short)(_low | _high << 8));
        set
        {
            _low = unchecked((byte)value);
            _high = unchecked((byte)(value >> 8));
        }
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct UInt16Cell
{
    private byte _low;
    private byte _high;

    public int Value
    {
        readonly get => _low | _high << 8;
        set
        {
            _low = unchecked((byte)value);
            _high = unchecked((byte)(value >> 8));
        }
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Int24Cell
{
    private byte _low;
    private byte _middle;
    private sbyte _high;

    public int Value
    {
        readonly get => _low | _middle << 8 | _high << 16;
        set
        {
            _low = unchecked((byte)value);
            _middle = unchecked((byte)(value >> 8));
            _high = unchecked((sbyte)(value >> 16));
        }
    }
}

public interface IBitPosition
{
    static abstract int Index { get; }
}

public readonly struct Bit0 : IBitPosition
{
    public const int Position = 0;
    public static int Index => Position;
}

public readonly struct Bit1 : IBitPosition
{
    public const int Position = 1;
    public static int Index => Position;
}

public readonly struct Bit2 : IBitPosition
{
    public const int Position = 2;
    public static int Index => Position;
}

public readonly struct Bit3 : IBitPosition
{
    public const int Position = 3;
    public static int Index => Position;
}

public readonly struct Bit4 : IBitPosition
{
    public const int Position = 4;
    public static int Index => Position;
}

public readonly struct Bit5 : IBitPosition
{
    public const int Position = 5;
    public static int Index => Position;
}

public readonly struct Bit6 : IBitPosition
{
    public const int Position = 6;
    public static int Index => Position;
}

public readonly struct Bit7 : IBitPosition
{
    public const int Position = 7;
    public static int Index => Position;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct BitCell<T> where T : struct, IBitPosition
{
    private byte _value;

    public bool Value
    {
        readonly get => (_value & (1 << T.Index)) != 0;
        set => _value = value ? (byte)(_value | (1 << T.Index)) : (byte)(_value & ~(1 << T.Index));
    }
}
