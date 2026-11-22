using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Memoria.EventEngine.EV
{
    public sealed class EVContainer
    {
        public const Int32 HeaderSize = 128;
        public const Int32 HeaderMetadataOffset = 4;
        public const Byte SupportedVersion = 2;
        private const Byte SignatureFirst = (Byte)'E';
        private const Byte SignatureSecond = (Byte)'V';
        public const Int32 ObjectEntrySize = 8;
        public const Int32 ObjectHeaderSize = 2;
        public const Int32 ScriptEntrySize = 4;

        public Byte[] Header { get; }
        public EVContainerObject[] Objects { get; }
        public Byte[] Trailer { get; }

        public EVContainer(Byte[] header, EVContainerObject[] objects, Byte[] trailer)
        {
            Header = header;
            Objects = objects;
            Trailer = trailer;
        }

        public static Byte[] CreateHeader(Byte version, Byte[] metadata)
        {
            if (version != SupportedVersion || metadata.Length != HeaderSize - HeaderMetadataOffset)
                throw new InvalidDataException("Invalid EV header version or metadata size.");

            var header = new Byte[HeaderSize];
            header[0] = SignatureFirst;
            header[1] = SignatureSecond;
            header[2] = version;
            metadata.CopyTo(header, HeaderMetadataOffset);
            return header;
        }

        public static EVContainer Read(Stream input)
        {
            using var buffer = new MemoryStream();
            input.CopyTo(buffer);
            Byte[] data = buffer.ToArray();
            if (data.Length < HeaderSize || data[0] != SignatureFirst || data[1] != SignatureSecond || data[2] != SupportedVersion)
                throw new InvalidDataException("Expected a version 2 EV header.");

            var objects = new EVContainerObject[data[3]];
            Int32 tableEnd = HeaderSize + objects.Length * ObjectEntrySize;
            if (tableEnd > data.Length)
                throw new InvalidDataException("Truncated EV object table.");

            var bodies = new List<(Int32 Offset, Int32 Length, EVContainerObject Object)>();
            for (Int32 id = 0; id < objects.Length; id++)
            {
                Int32 entry = HeaderSize + id * ObjectEntrySize;
                UInt16 offset = ReadUInt16(data, entry);
                Int32 length = ReadUInt16(data, entry + 2);
                var obj = new EVContainerObject(id, data[entry + 4], data[entry + 5], ReadUInt16(data, entry + 6));
                objects[id] = obj;
                if (length == 0)
                    obj.EmptyOffset = offset;
                else
                    bodies.Add((HeaderSize + offset, length, obj));
            }

            Int32 position = tableEnd;
            Int32 order = 0;
            foreach (var body in bodies.OrderBy(body => body.Offset))
            {
                EVContainerObject obj = body.Object;
                Int32 end = body.Offset + body.Length;
                if (body.Offset < position || end > data.Length || body.Length < ObjectHeaderSize)
                    throw new InvalidDataException($"Invalid body range for object {obj.Id} at 0x{body.Offset:X}.");

                obj.BodyOrder = order++;
                obj.Gap = Slice(data, position, body.Offset);
                obj.Type = data[body.Offset];
                Int32 directory = body.Offset + ObjectHeaderSize;
                Int32 count = data[body.Offset + 1];
                Int32 directoryEnd = directory + count * ScriptEntrySize;
                if (directoryEnd > end)
                    throw new InvalidDataException($"Truncated script directory for object {obj.Id}.");

                obj.Scripts = new EVContainerScript[count];
                Int32 start = count == 0 ? end : directory + ReadUInt16(data, directory + 2);
                if (start < directoryEnd || start > end)
                    throw new InvalidDataException($"Invalid first script offset for object {obj.Id}.");

                obj.Prefix = Slice(data, directoryEnd, start);
                for (Int32 index = 0; index < count; index++)
                {
                    Int32 entry = directory + index * ScriptEntrySize;
                    UInt16 scriptId = ReadUInt16(data, entry);
                    Int32 next = index + 1 == count ? end : directory + ReadUInt16(data, entry + ScriptEntrySize + 2);
                    if (next < start || next > end)
                        throw new InvalidDataException($"Invalid script {scriptId} range for object {obj.Id} at 0x{start:X}.");

                    obj.Scripts[index] = new EVContainerScript(scriptId, Slice(data, start, next), start);
                    start = next;
                }
                position = end;
            }

            return new EVContainer(Slice(data, 0, HeaderSize), objects, Slice(data, position, data.Length));
        }

        public Byte[] Serialize()
        {
            if (Header.Length != HeaderSize || Header[0] != SignatureFirst || Header[1] != SignatureSecond || Header[2] != SupportedVersion)
                throw new InvalidDataException("Expected a version 2 EV header.");
            if (Objects.Length > Byte.MaxValue || Header[3] != Objects.Length)
                throw new InvalidDataException($"The EV header declares {Header[3]} object slots but {Objects.Length} were supplied.");

            var offsets = new UInt16[Objects.Length];
            var lengths = new UInt16[Objects.Length];
            for (Int32 id = 0; id < Objects.Length; id++)
            {
                EVContainerObject obj = Objects[id];
                if (obj == null || obj.Id != id)
                    throw new InvalidDataException($"Expected object slot {id}, got {obj?.Id.ToString() ?? "no object"}.");
                if (obj.BodyOrder == null && (obj.Scripts.Length != 0 || obj.Prefix.Length != 0 || obj.Gap.Length != 0))
                    throw new InvalidDataException($"Empty object {id} declares body data.");
                offsets[id] = obj.EmptyOffset;
            }

            using var output = new MemoryStream();
            using var writer = new BinaryWriter(output);
            writer.Write(Header);
            output.SetLength(HeaderSize + Objects.Length * ObjectEntrySize);
            output.Position = output.Length;

            Int32 expectedOrder = 0;
            foreach (EVContainerObject obj in Objects.Where(obj => obj.BodyOrder.HasValue).OrderBy(obj => obj.BodyOrder))
            {
                if (obj.BodyOrder != expectedOrder++)
                    throw new InvalidDataException($"Invalid body order {obj.BodyOrder} for object {obj.Id}.");
                if (obj.Scripts.Length > Byte.MaxValue)
                    throw new InvalidDataException($"Object {obj.Id} has too many scripts.");

                writer.Write(obj.Gap);
                Int64 start = output.Position;
                offsets[obj.Id] = CheckedUInt16(start - HeaderSize, $"offset of object {obj.Id}");
                writer.Write(obj.Type);
                writer.Write((Byte)obj.Scripts.Length);
                Int64 scriptOffset = obj.Scripts.Length * ScriptEntrySize + obj.Prefix.Length;
                foreach (EVContainerScript script in obj.Scripts)
                {
                    writer.Write(script.Id);
                    writer.Write(CheckedUInt16(scriptOffset, $"offset of object {obj.Id}, script {script.Id}"));
                    scriptOffset += script.Body.Length;
                }
                writer.Write(obj.Prefix);
                foreach (EVContainerScript script in obj.Scripts)
                    writer.Write(script.Body);
                lengths[obj.Id] = CheckedUInt16(output.Position - start, $"size of object {obj.Id}");
            }

            writer.Write(Trailer);
            output.Position = HeaderSize;
            foreach (EVContainerObject obj in Objects)
            {
                writer.Write(offsets[obj.Id]);
                writer.Write(lengths[obj.Id]);
                writer.Write(obj.VariableCount);
                writer.Write(obj.Flags);
                writer.Write(obj.Reserved);
            }
            return output.ToArray();
        }

        private static UInt16 CheckedUInt16(Int64 value, String field)
        {
            if (value < 0 || value > UInt16.MaxValue)
                throw new InvalidDataException($"The {field} ({value}) does not fit in 16 bits.");
            return (UInt16)value;
        }

        private static UInt16 ReadUInt16(Byte[] data, Int32 offset) => BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(offset, 2));

        private static Byte[] Slice(Byte[] data, Int32 start, Int32 end) => data.AsSpan(start, end - start).ToArray();
    }

    public sealed class EVContainerObject
    {
        public Int32 Id { get; }
        public Byte VariableCount { get; }
        public Byte Flags { get; }
        public UInt16 Reserved { get; }
        public UInt16 EmptyOffset { get; set; }
        public Int32? BodyOrder { get; set; }
        public Byte Type { get; set; }
        public Byte[] Gap { get; set; } = Array.Empty<Byte>();
        public Byte[] Prefix { get; set; } = Array.Empty<Byte>();
        public EVContainerScript[] Scripts { get; set; } = Array.Empty<EVContainerScript>();

        public EVContainerObject(Int32 id, Byte variableCount, Byte flags, UInt16 reserved)
        {
            Id = id;
            VariableCount = variableCount;
            Flags = flags;
            Reserved = reserved;
        }
    }

    public sealed class EVContainerScript
    {
        public UInt16 Id { get; }
        public Byte[] Body { get; set; }
        public Int32 OriginalOffset { get; }
        public EVAddressCheckpoint[] AddressChecks { get; set; } = Array.Empty<EVAddressCheckpoint>();

        public EVContainerScript(UInt16 id, Byte[] body, Int32 originalOffset = -1)
        {
            Id = id;
            Body = body;
            OriginalOffset = originalOffset;
        }
    }
}
