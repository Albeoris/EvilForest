using System;
using System.Collections.Generic;
using System.IO;
using FF8.JSM;

namespace Memoria.EventEngine.EV
{
    public sealed class EVScriptWriter
    {
        private readonly MemoryStream _ms;
        private readonly List<Byte> _bytecode;

        public EVScriptWriter()
        {
            _bytecode = new List<Byte>();
            _ms = new MemoryStream();
        }

        public Byte[] GetBytecode()
        {
            return _bytecode.ToArray();
        }

        public void WriteOpcode(Jsm.Opcode opcode)
        {
            _bytecode.Add((Byte)opcode);
        }

        public void WriteByte(Byte value)
        {
            _bytecode.Add(value);
        }

        public void WriteSByte(SByte value)
        {
            _bytecode.Add((Byte)value);
        }

        public void WriteInt16(Int16 value)
        {
            _bytecode.Add((Byte)(value & 0xFF));
            _bytecode.Add((Byte)((value >> 8) & 0xFF));
        }

        public void WriteUInt16(UInt16 value)
        {
            _bytecode.Add((Byte)(value & 0xFF));
            _bytecode.Add((Byte)((value >> 8) & 0xFF));
        }

        public void WriteInt24(Int32 value)
        {
            _bytecode.Add((Byte)(value & 0xFF));
            _bytecode.Add((Byte)((value >> 8) & 0xFF));
            _bytecode.Add((Byte)((value >> 16) & 0xFF));
        }

        public void WriteInt32(Int32 value)
        {
            _bytecode.Add((Byte)(value & 0xFF));
            _bytecode.Add((Byte)((value >> 8) & 0xFF));
            _bytecode.Add((Byte)((value >> 16) & 0xFF));
            _bytecode.Add((Byte)((value >> 24) & 0xFF));
        }
    }
}