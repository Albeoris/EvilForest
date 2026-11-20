using System;
using System.IO;

namespace Memoria.EventEngine.EV
{
    /// <summary>
    /// Writes EVObject arrays back to the .eb.bytes binary format.
    /// </summary>
    public sealed class EVFileWriter
    {
        public static void Write(String evPath, EVObject[] objects)
        {
            Write(evPath, CreateContainer(objects));
        }

        public static void Write(String evPath, EVContainer container)
        {
            Byte[] data = container.Serialize();
            String destination = Path.GetFullPath(evPath);
            String temporary = Path.Combine(Path.GetDirectoryName(destination), $".evilforest-{Guid.NewGuid():N}.tmp");
            try
            {
                using (var output = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    output.Write(data, 0, data.Length);
                    output.Flush(flushToDisk: true);
                }
                if (File.Exists(destination))
                    File.Replace(temporary, destination, null);
                else
                    File.Move(temporary, destination);
            }
            finally
            {
                File.Delete(temporary);
            }
        }

        public void Write(Stream output, EVObject[] objects)
        {
            Byte[] data = CreateContainer(objects).Serialize();
            output.Write(data, 0, data.Length);
        }

        private static EVContainer CreateContainer(EVObject[] objects)
        {
            if (objects.Length > Byte.MaxValue)
                throw new InvalidDataException("Too many EV object slots.");

            var header = new Byte[EVContainer.HeaderSize];
            header[0] = (Byte)'E';
            header[1] = (Byte)'V';
            header[2] = 2;
            header[3] = (Byte)objects.Length;
            var entries = new EVContainerObject[objects.Length];
            Int32 order = 0;
            for (Int32 i = 0; i < objects.Length; i++)
            {
                EVObject obj = objects[i];
                if (obj.Id != i)
                    throw new InvalidDataException($"Expected object slot {i}, got {obj.Id}.");
                var entry = new EVContainerObject(i, obj.VariableCount, obj.Flags, 0);
                entries[i] = entry;
                if (obj.Scripts.Length == 0)
                    continue;
                entry.BodyOrder = order++;
                entry.Scripts = new EVContainerScript[obj.Scripts.Length];
                for (Int32 j = 0; j < obj.Scripts.Length; j++)
                {
                    EVScript script = obj.Scripts[j];
                    entry.Scripts[j] = new EVContainerScript(checked((UInt16)script.Id), TryExtractBytecode(script));
                }
            }
            return new EVContainer(header, entries, Array.Empty<Byte>());
        }

        private static Byte[] TryExtractBytecode(EVScript script)
        {
            if (script.Instructions == null)
                throw new InvalidDataException($"Script {script.Id} has no encodable instruction model.");
            return script.Instructions.Encode();
        }
    }
}
