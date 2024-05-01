using System;

namespace FF8.Core
{
    public sealed class BitByte
    {
        private Byte _value;

        public Boolean this[Int32 bitIndex]
        {
            get
            {
                ThrowIfOutOfRange(bitIndex);
                return ((_value >> bitIndex) & 1) == 1;
            }
            set
            {
                ThrowIfOutOfRange(bitIndex);
                if (value)
                    _value |= (Byte) (1 << bitIndex);
                else
                    _value &= (Byte) ~ (1 << bitIndex);
            }
        }

        private static void ThrowIfOutOfRange(Int32 bitIndex)
        {
            if (bitIndex < 0 || bitIndex > 7) throw new ArgumentOutOfRangeException(nameof(bitIndex), $"Index {bitIndex} is out of range 0...7");
        }

        public static implicit operator Byte(BitByte self) => self._value;
        public static implicit operator BitByte(Byte other) => new BitByte {_value = other};
        public override String ToString() => _value.ToString();
        public override Boolean Equals(Object obj) => obj is BitByte other && _value == other._value;
        public override Int32 GetHashCode() => throw new NotSupportedException();
    }
}