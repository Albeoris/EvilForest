using System;
using System.IO;

namespace Memoria.EventEngine.EV
{
    public enum EVStorageScope
    {
        Global,
        Map,
        Instance,
        Special
    }

    public enum EVStorageType
    {
        SBit,
        Bit,
        Int24,
        UInt24,
        SByte,
        Byte,
        Int16,
        UInt16
    }

    public readonly struct EVVariable
    {
        public EVStorageScope Scope { get; }
        public EVStorageType Type { get; }
        public Int32 Address { get; }
        public Boolean UsesBitAddress => Type == EVStorageType.Bit || Type == EVStorageType.SBit;
        public Boolean IsSigned => Type == EVStorageType.SByte || Type == EVStorageType.Int16 || Type == EVStorageType.Int24;
        public Int32 Width => Type switch
        {
            EVStorageType.Bit or EVStorageType.SBit => 1,
            EVStorageType.SByte or EVStorageType.Byte => 8,
            EVStorageType.Int16 or EVStorageType.UInt16 => 16,
            _ => 24
        };

        public EVVariable(EVStorageScope scope, EVStorageType type, Int32 address)
        {
            if ((UInt32)scope > 3 || (UInt32)type > 7 || (UInt32)address > UInt16.MaxValue)
                throw new InvalidDataException($"Invalid VM variable {scope}.{type}[{address}].");
            Scope = scope;
            Type = type;
            Address = address;
        }

        public EVExpressionToken Encode(Boolean wide = false, Boolean alternatePrefix = false)
        {
            Int32 opcode = (alternatePrefix ? 0x80 : 0xC0) | ((Int32)Type << 2) | (Int32)Scope;
            if (wide || Address > Byte.MaxValue)
                opcode |= 0x20;
            if (opcode == 0xD3)
                throw new InvalidDataException("Opcode 0xD3 is reserved for a Memoria expression command.");
            return new EVExpressionToken((Byte)opcode, Address);
        }

        public static EVVariable Decode(EVExpressionToken token)
        {
            if (token.Opcode < 0x80 || token.Opcode == 0xD3)
                throw new InvalidDataException($"Expression token 0x{token.Opcode:X2} is not a variable.");
            return new EVVariable((EVStorageScope)(token.Opcode & 3), (EVStorageType)((token.Opcode >> 2) & 7), token.Value);
        }

        public EVVariable Bit(Int32 bit)
        {
            if (UsesBitAddress || bit < 0 || bit >= Width)
                throw new ArgumentOutOfRangeException(nameof(bit));
            return new EVVariable(Scope, EVStorageType.Bit, checked(Address * 8 + bit));
        }
    }
}
