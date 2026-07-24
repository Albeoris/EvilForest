using System;
using System.IO;
using System.Runtime.CompilerServices;
using FF8.JSM;

namespace Memoria.EventEngine.EV
{
    /// <summary>
    /// Writes EVObject arrays back to the .eb.bytes binary format.
    /// </summary>
    public sealed class EVFileWriter
    {
        private const UInt16 MagicNumber     = 0x5645; // "EV"
        private const Byte   FileVersion     = 2;
        private const Int32  FileHeaderSize  = 128; // sizeof(EVFileHeader)
        private const Int32  ObjectEntrySize = 8;   // sizeof(EVFileObject)
        private const Int32  ScriptsHeaderSize = 2; // sizeof(EVFileScriptsHeader)
        private const Int32  ScriptInfoSize   = 4;  // sizeof(EVFileScriptInfo)

        public static void Write(String evPath, EVObject[] objects)
        {
            using var output = File.Create(evPath);
            new EVFileWriter().Write(output, objects);
        }

        public void Write(Stream output, EVObject[] objects)
        {
            using var bw = new BinaryWriter(output, System.Text.Encoding.UTF8, leaveOpen: true);

            // File header (128 bytes)
            bw.Write(MagicNumber);
            bw.Write(FileVersion);
            bw.Write((Byte)objects.Length);
            bw.Write(new Byte[124]);

            // Pre-compute object bodies
            var bodies = new Byte[objects.Length][];
            for (Int32 i = 0; i < objects.Length; i++)
                bodies[i] = SerializeObjectBody(objects[i]);

            // Object entry table
            // EVFileObject.Offset is relative to the end of EVFileHeader.
            // EVFileReader adds FileHeaderSize when seeking to an object body.
            Int32 bodyOffset = ObjectEntrySize * objects.Length;
            for (Int32 i = 0; i < objects.Length; i++)
            {
                bw.Write((UInt16)bodyOffset);
                bw.Write((UInt16)bodies[i].Length);
                bw.Write(objects[i].VariableCount);
                bw.Write(objects[i].Flags);
                bw.Write((Int16)0);
                bodyOffset += bodies[i].Length;
            }

            foreach (var body in bodies)
                bw.Write(body);
        }

        private static Byte[] SerializeObjectBody(EVObject obj)
        {
            if (obj.Scripts.Length == 0)
                return Array.Empty<Byte>();

            using var ms  = new MemoryStream();
            using var bw  = new BinaryWriter(ms, System.Text.Encoding.UTF8, leaveOpen: true);

            bw.Write((Byte)0);
            bw.Write((Byte)obj.Scripts.Length);

            var bytecodes = new Byte[obj.Scripts.Length][];
            for (Int32 s = 0; s < obj.Scripts.Length; s++)
                bytecodes[s] = TryExtractBytecode(obj.Scripts[s]);

            // EVFileScriptInfo.Offset is relative to the byte immediately after
            // EVFileScriptsHeader, so the two-byte header is deliberately excluded.
            Int32 codeOffset = ScriptInfoSize * obj.Scripts.Length;
            for (Int32 s = 0; s < obj.Scripts.Length; s++)
            {
                bw.Write((UInt16)obj.Scripts[s].Id);
                bw.Write((UInt16)codeOffset);
                codeOffset += bytecodes[s].Length;
            }

            foreach (var code in bytecodes)
                bw.Write(code);

            return ms.ToArray();
        }

        private static Byte[] TryExtractBytecode(EVScript script)
        {
            Byte[] stored = CompiledBytecodeTag.Get(script.Segment);
            if (stored != null) return stored;
            throw new InvalidOperationException(
                $"Script {script.Id} has no preserved bytecode and cannot be serialized losslessly.");
        }
    }

    /// <summary>
    /// Attaches raw compiled bytecode to an ExecutableSegment so EVFileWriter can recover it.
    /// </summary>
    public static class CompiledBytecodeTag
    {
        private static readonly ConditionalWeakTable<Jsm.ExecutableSegment, ByteTag> Table = new();

        public static void Set(Jsm.ExecutableSegment segment, Byte[] bytecode)
            => Table.AddOrUpdate(segment, new ByteTag(bytecode));

        public static Byte[] Get(Jsm.ExecutableSegment segment)
            => Table.TryGetValue(segment, out ByteTag tag) ? tag.Data : null;

        private sealed class ByteTag
        {
            public Byte[] Data { get; }
            public ByteTag(Byte[] data) => Data = data;
        }
    }
}
