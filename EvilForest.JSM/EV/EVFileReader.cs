using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Albeoris.Framework.Collections;
using FF8.JSM;
using FF8.JSM.Instructions;

namespace Memoria.EventEngine.EV
{
    public sealed class EVFileReader
    {
        private readonly Stream _input;

        public EVFileReader(Stream input)
        {
            _input = input;
        }

        /// <summary>
        /// Parses a raw JSM bytecode array into an ExecutableSegment, using the same
        /// logic as when reading from a binary .eb.bytes file.
        /// </summary>
        public static Jsm.ExecutableSegment ParseScript(Byte[] bytecode)
        {
            var maker = new EVScriptMaker(new ByteSegment(bytecode, 0, bytecode.Length));
            try
            {
                return MakeScript(maker);
            }
            catch (Exception ex)
            {
                ex.Data["ScriptOffset"] = maker.Offset;
                throw;
            }
        }

        public static EVObject[] Read(String evPath)
        {
            using var input = File.OpenRead(evPath);
            var reader = new EVFileReader(input);
            return reader.Read();
        }

        public EVObject[] Read()
        {
            EVContainer container = EVContainer.Read(_input);
            return container.Objects.Select(obj =>
            {
                EVScript[] scripts = obj.Scripts.Select(script =>
                {
                    try
                    {
                        return new EVScript(script.Id, EVInstructionBody.Decode(script.Body));
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidDataException($"Object {obj.Id}, script {script.Id} at file offset 0x{script.OriginalOffset:X}: {ex.Message}", ex);
                    }
                }).ToArray();
                return new EVObject(obj.Id, obj.VariableCount, obj.Flags, scripts);
            }).ToArray();
        }

        private static Jsm.ExecutableSegment MakeScript(EVScriptMaker maker)
        {
            List<JsmInstruction> instructions = new List<JsmInstruction>();
            Jsm.LabeledStack stack = new Jsm.LabeledStack();
            Jsm.LabelBuilder labelBuilder = new Jsm.LabelBuilder(maker.Size);

            var currentLabel = maker.Offset;

            while (maker.TryReadOpcode(out Jsm.Opcode opcode))
            {
                CheckCodeAllowed(opcode, instructions, maker.Offset - 1);

                if (opcode == Jsm.Opcode.EXPR)
                {
                    currentLabel = maker.Offset - 1;
                        
                    stack.Clear();
                    Jsm.Expression.Evaluate(maker, stack);

                    if (stack.TryPeek() is JsmInstruction setter)
                    {
                        setter.IsStandalone = true;
                        labelBuilder.TraceInstruction(maker.Offset, currentLabel, new Jsm.IndexedInstruction(instructions.Count, setter));
                        instructions.Add(setter);
                        currentLabel = maker.Offset;
                    }

                    continue;
                }

                JsmInstruction instruction = JsmInstruction.TryMake(opcode, maker, stack);
                if (instruction != null)
                {
                    instruction.IsStandalone = true;
                    labelBuilder.TraceInstruction(maker.Offset, currentLabel, new Jsm.IndexedInstruction(instructions.Count, instruction));
                    instructions.Add(instruction);
                    currentLabel = maker.Offset;
                    continue;
                }

                throw new NotSupportedException(opcode.ToString());
            }

            stack.Clear();

            // if (!(instructions.First() is LBL))
            //     throw new InvalidProgramException("Script must start with a label.");
            //

            for (Int32 i = instructions.Count - 1; i >= 0; i--)
            {
                JsmInstruction instruction = instructions[i];
                if (instruction is NOP)
                    continue;
                
                if (instruction is JsmReturn)
                    break;
                
                if (instruction is JMP jmp && jmp.Offset < 0)
                    break;
                
                throw new InvalidProgramException("Script must end with a return.");
            }
            
            if (labelBuilder.AdditionalNopNumber > 0)
                throw new InvalidProgramException("if (labelBuilder.AdditionalNopNumber > 0)");
            
            // for (int i = 0; i < labelBuilder.AdditionalNopNumber; i++)
            // {
            //     JsmInstruction instruction = new NOP();
            //     labelBuilder.TraceInstruction(maker.Offset, currentLabel, new Jsm.IndexedInstruction(instructions.Count, instruction));
            //     instructions.Add(instruction);
            //     currentLabel = maker.Offset + i + 1;
            // }

            // Switch from opcodes to instructions
            HashSet<Int32> labelIndices = labelBuilder.Commit();

            // Merge similar instructions
            // instructions = InstructionMerger.Merge(instructions, labelIndices);

            // Combine instructions to logical blocks
            Jsm.IJsmControl[] controls = Jsm.Control.Builder.Build(instructions);

            // Arrange instructions by segments and return root
            return Jsm.Segment.Builder.Build(instructions, controls);
        }

        private static void CheckCodeAllowed(Jsm.Opcode opcode, List<JsmInstruction> instructions, Int32 offset)
        {
            if (opcode == Jsm.Opcode.NOP)
            {
                var isAllowed = IsNopOpcodeAllowed(instructions);
                if (!isAllowed)
                    throw new InvalidProgramException(
                        $"Unexpected NOP at byte offset {offset}; previous instruction: {instructions.LastOrDefault()}.");
            }
        }

        private static Boolean IsNopOpcodeAllowed(List<JsmInstruction> instructions)
        {
            var lastInstruction = instructions.LastOrDefault();
            return lastInstruction switch
            {
                JsmReturn _ => true,
                JMP _ => true, // switch
                NOP _ => true, // while
                _ => false
            };
        }
    }
}
