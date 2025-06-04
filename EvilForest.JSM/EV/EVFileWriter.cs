using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using FF8.JSM;
using FF8.JSM.Instructions;

namespace Memoria.EventEngine.EV
{
    public sealed class EVFileWriter
    {
        private readonly Stream _output;

        public EVFileWriter(Stream output)
        {
            _output = output;
        }

        public static void Write(string evPath, EVObject[] objects)
        {
            using var output = File.Create(evPath);
            var writer = new EVFileWriter(output);
            writer.Write(objects);
        }

        public unsafe void Write(EVObject[] objects)
        {
            // Write file header
            var fileHeader = CreateFileHeader((byte)objects.Length);
            WriteStruct(fileHeader);

            // Calculate object data and write object headers
            var objectData = new List<(EVFileObject header, byte[] data)>();
            int currentOffset = sizeof(EVFileHeader) + sizeof(EVFileObject) * objects.Length;

            foreach (var obj in objects)
            {
                byte[] data = SerializeObject(obj);
                var header = new EVFileObject
                {
                    Offset = (ushort)currentOffset,
                    Size = (ushort)data.Length,
                    VariableCount = obj.VariableCount,
                    Flags = obj.Flags
                };

                objectData.Add((header, data));
                currentOffset += data.Length;
            }

            // Write object headers
            foreach (var (header, _) in objectData)
            {
                WriteStruct(header);
            }

            // Write object data
            foreach (var (_, data) in objectData)
            {
                _output.Write(data, 0, data.Length);
            }
        }

        private void WriteStruct<T>(T value) where T : unmanaged
        {
            int size = Marshal.SizeOf<T>();
            byte[] bytes = new byte[size];
            
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    Marshal.StructureToPtr(value, (IntPtr)ptr, false);
                }
            }
            
            _output.Write(bytes, 0, bytes.Length);
        }

        private unsafe EVFileHeader CreateFileHeader(byte objectCount)
        {
            var header = new EVFileHeader();
            
            // Use reflection to set private fields
            var type = typeof(EVFileHeader);
            var magicField = type.GetField("_magicNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var unknownField = type.GetField("_unknown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            magicField?.SetValue(header, (ushort)0x5645); // "EV"
            unknownField?.SetValue(header, (byte)2);
            
            // Set public field
            var objectCountField = type.GetField("ObjectCount");
            objectCountField?.SetValue(header, objectCount);
            
            return header;
        }

        private unsafe byte[] SerializeObject(EVObject obj)
        {
            if (obj.Scripts.Length == 0)
            {
                return Array.Empty<byte>();
            }

            using var ms = new MemoryStream();

            // Write scripts header
            var scriptsHeader = new EVFileScriptsHeader
            {
                Unknown = 0,
                ScriptCount = (byte)obj.Scripts.Length
            };
            WriteStructToStream(ms, scriptsHeader);

            // Collect script bytecode and calculate offsets
            var scriptBytecodes = new List<byte[]>();
            var scriptInfos = new List<EVFileScriptInfo>();
            int scriptOffset = sizeof(EVFileScriptsHeader) + sizeof(EVFileScriptInfo) * obj.Scripts.Length;

            foreach (var script in obj.Scripts)
            {
                byte[] bytecode = ExtractScriptBytecode(script);
                scriptBytecodes.Add(bytecode);

                var scriptInfo = new EVFileScriptInfo
                {
                    Id = (ushort)script.Id,
                    Offset = (ushort)scriptOffset
                };
                scriptInfos.Add(scriptInfo);

                scriptOffset += bytecode.Length;
            }

            // Write script infos
            foreach (var scriptInfo in scriptInfos)
            {
                WriteStructToStream(ms, scriptInfo);
            }

            // Write script bytecodes
            foreach (var bytecode in scriptBytecodes)
            {
                ms.Write(bytecode, 0, bytecode.Length);
            }

            return ms.ToArray();
        }

        private void WriteStructToStream<T>(Stream stream, T value) where T : unmanaged
        {
            int size = Marshal.SizeOf<T>();
            byte[] bytes = new byte[size];
            
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    Marshal.StructureToPtr(value, (IntPtr)ptr, false);
                }
            }
            
            stream.Write(bytes, 0, bytes.Length);
        }

        private byte[] ExtractScriptBytecode(EVScript script)
        {
            // Try to extract bytecode from the script segment
            // This is a simplified approach - in reality, we'd need to properly
            // convert the ExecutableSegment back to raw bytecode
            
            if (script.Segment.GetType().Name.Contains("BasicExecutableSegment"))
            {
                return script.Segment.GetBytecode();
            }

            // For other types of segments, we need to reconstruct the bytecode
            // This is a complex process that involves walking the instruction tree
            // and converting back to JSM opcodes and arguments
            
            return ReconstructBytecodeFromSegment(script.Segment);
        }

        private byte[] ReconstructBytecodeFromSegment(Jsm.ExecutableSegment segment)
        {
            // This is a complex reconstruction process
            // For now, return empty bytecode as a placeholder
            var writer = new EVScriptWriter();
            
            try
            {
                // Walk through all instructions in the segment and convert them back to bytecode
                foreach (var instruction in segment.EnumerateAllInstruction())
                {
                    ConvertInstructionToBytecode(instruction, writer);
                }
            }
            catch (Exception)
            {
                // If conversion fails, return a simple return instruction
                writer.WriteOpcode(Jsm.Opcode.Return);
            }

            return writer.GetBytecode();
        }

        private void ConvertInstructionToBytecode(IJsmInstruction instruction, EVScriptWriter writer)
        {
            // This is where we'd convert each instruction type back to bytecode
            // For now, we'll handle some basic cases and throw for others
            
            if (instruction is JsmReturn)
            {
                writer.WriteOpcode(Jsm.Opcode.Return);
            }
            else
            {
                // For unhandled instructions, we'll add a NOP
                writer.WriteOpcode(Jsm.Opcode.NOP);
            }
        }
    }

    // Extension class for the BasicExecutableSegment to expose bytecode
    public static class ExecutableSegmentExtensions
    {
        public static byte[] GetBytecode(this Jsm.ExecutableSegment segment)
        {
            // Use reflection to try to extract bytecode from BasicExecutableSegment
            var type = segment.GetType();
            var field = type.GetField("_bytecode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field?.GetValue(segment) is byte[] bytecode)
            {
                return bytecode;
            }
            
            // Fallback - return a simple return instruction
            var writer = new EVScriptWriter();
            writer.WriteOpcode(Jsm.Opcode.Return);
            return writer.GetBytecode();
        }
    }
}