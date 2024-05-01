using System;

namespace FF8.JSM
{
    public readonly struct Int26
    {
        public readonly UInt32 Raw;

        public Int26(Int32 raw)
        {
            Raw = (UInt32)raw;
        }
        
        public Int26(Int32 raw, Jsm.Expression.VariableSource source, Jsm.Expression.VariableType type)
        {
            Raw = (UInt32) ((raw & 0x3FFFFFF) | ((Int32) source << 26) | ((Int32) type << 29));
        }
        
        public Int32 Value => (Int32)(Raw & 0x3FFFFFF);
        public Jsm.Expression.VariableSource Source => (Jsm.Expression.VariableSource) ((Raw >> 26) & 7);
        public Jsm.Expression.VariableType Type => (Jsm.Expression.VariableType) (Raw >> 29);

        public override String ToString() => $"{Type} {Source} = {Value}";

        public override Boolean Equals(Object obj)
        {
            if (obj is Int26 other)
                return Raw == other.Raw;
            return false;
        }

        public Boolean Equals(Int26 other) => Raw == other.Raw;
        public override Int32 GetHashCode() => (Int32)Raw;
        public static Boolean operator ==(Int26 left, Int26 right) => left.Raw == right.Raw;
        public static Boolean operator !=(Int26 left, Int26 right) => left.Raw != right.Raw;

        public String GetTypeName()
        {
            return Type switch
            {
                Jsm.Expression.VariableType.Default => "Int32",
                Jsm.Expression.VariableType.Bit => "Boolean",
                Jsm.Expression.VariableType.Int24 => "Int32",
                Jsm.Expression.VariableType.UInt24 => "UInt32",
                Jsm.Expression.VariableType.SByte => "SByte",
                Jsm.Expression.VariableType.Byte => "Byte",
                Jsm.Expression.VariableType.Int16 => "Int16",
                Jsm.Expression.VariableType.UInt16 => "UInt16",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public Int32 GetOffsetBits()
        {
            return Type switch
            {
                Jsm.Expression.VariableType.Default => throw new NotSupportedException(),
                Jsm.Expression.VariableType.Bit => Value,
                Jsm.Expression.VariableType.Int24 => Value * 8,
                Jsm.Expression.VariableType.UInt24 => Value * 8,
                Jsm.Expression.VariableType.SByte => Value * 8,
                Jsm.Expression.VariableType.Byte => Value * 8,
                Jsm.Expression.VariableType.Int16 => Value * 8,
                Jsm.Expression.VariableType.UInt16 => Value * 8,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
                
        public Int32 GetTypeSizeBits()
        {
            return Type switch
            {
                Jsm.Expression.VariableType.Default => throw new NotSupportedException(),
                Jsm.Expression.VariableType.Bit => 1,
                Jsm.Expression.VariableType.Int24 => 3 * 8,
                Jsm.Expression.VariableType.UInt24 => 3 * 8,
                Jsm.Expression.VariableType.SByte => 1 * 8,
                Jsm.Expression.VariableType.Byte => 1 * 8,
                Jsm.Expression.VariableType.Int16 => 2 * 8,
                Jsm.Expression.VariableType.UInt16 => 2 * 8,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}