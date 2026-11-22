using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FF8.JSM;

namespace Memoria.EventEngine.EV
{
    public sealed class EVInstruction
    {
        public Int32 Label { get; set; }
        public Jsm.Opcode Opcode { get; set; }
        public Int32 OriginalLength { get; internal set; }
        public Int32? WatchedObjectIp { get; set; }
        public Byte ArgumentMask { get; set; }
        public EVOperand[] Operands { get; set; } = Array.Empty<EVOperand>();
        public EVExpressionToken[] Expression { get; set; }
        public Int32[] Targets { get; set; } = Array.Empty<Int32>();
        public UInt16[] CaseValues { get; set; } = Array.Empty<UInt16>();

        public EVInstruction(Int32 label, Jsm.Opcode opcode)
        {
            Label = label;
            Opcode = opcode;
        }
    }

    public readonly struct EVOperand
    {
        public Int32 Value { get; }
        public EVExpressionToken[] Expression { get; }

        public EVOperand(Int32 value)
        {
            Value = value;
            Expression = null;
        }

        public EVOperand(EVExpressionToken[] expression)
        {
            Value = 0;
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }
    }

    public readonly struct EVExpressionToken
    {
        public Byte Opcode { get; }
        public Int32 Value { get; }

        public EVExpressionToken(Byte opcode, Int32 value = 0)
        {
            Opcode = opcode;
            Value = value;
        }

        public static EVExpressionToken Literal(Int32 value, Boolean wide = false, Byte highBits = 0)
        {
            if (highBits > 63 || value < -(1 << 25) || value >= (1 << 25))
                throw new InvalidDataException($"Invalid VM literal {value} with high bits {highBits}.");
            if (!wide && highBits == 0 && value >= Int16.MinValue && value <= Int16.MaxValue)
                return new EVExpressionToken((Byte)Jsm.Excode.Const16, value);
            return new EVExpressionToken((Byte)Jsm.Excode.Const26, (value & 0x03FFFFFF) | (highBits << 26));
        }
    }

    public sealed class EVBasicBlock
    {
        public Int32 Label => Instructions[0].Label;
        public IReadOnlyList<EVInstruction> Instructions { get; }
        public IReadOnlyList<Int32> Successors { get; }

        internal EVBasicBlock(EVInstruction[] instructions, Int32[] successors)
        {
            Instructions = instructions;
            Successors = successors;
        }
    }

    public sealed class EVInstructionBody
    {
        public IList<EVInstruction> Instructions { get; }
        public Int32 EndLabel { get; }

        public EVInstructionBody(IList<EVInstruction> instructions, Int32 endLabel)
        {
            Instructions = instructions;
            EndLabel = endLabel;
        }

        public static EVInstructionBody Decode(Byte[] data)
        {
            using var stream = new MemoryStream(data, writable: false);
            using var reader = new BinaryReader(stream);
            var instructions = new List<EVInstruction>();
            while (stream.Position < stream.Length)
            {
                Int32 start = (Int32)stream.Position;
                Int32 opcode = 0;
                try
                {
                    Byte prefix;
                    while ((prefix = reader.ReadByte()) == Byte.MaxValue)
                        opcode += 256;
                    opcode += prefix;
                    var instruction = new EVInstruction(start, (Jsm.Opcode)opcode);
                    ReadInstruction(reader, instruction);
                    instruction.OriginalLength = (Int32)stream.Position - start;
                    instructions.Add(instruction);
                }
                catch (Exception ex) when (ex is EndOfStreamException || ex is InvalidDataException)
                {
                    throw new InvalidDataException($"Instruction 0x{opcode:X} at script offset 0x{start:X}: {ex.Message}", ex);
                }
            }
            var result = new EVInstructionBody(instructions, data.Length);
            result.GetSuccessors();
            return result;
        }

        public IReadOnlyDictionary<Int32, Int32[]> GetSuccessors()
        {
            var successors = new Dictionary<Int32, Int32[]>();
            for (Int32 index = 0; index < Instructions.Count; index++)
            {
                EVInstruction instruction = Instructions[index];
                Int32 next = index + 1 < Instructions.Count ? Instructions[index + 1].Label : EndLabel;
                Int32[] edges = instruction.Opcode switch
                {
                    Jsm.Opcode.Return => Array.Empty<Int32>(),
                    Jsm.Opcode.JMP or Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d => instruction.Targets,
                    _ => instruction.Targets.Concat(new[] { next }).ToArray()
                };
                if (instruction.Label == EndLabel || successors.ContainsKey(instruction.Label))
                    throw new InvalidDataException($"Duplicate instruction label {instruction.Label}.");
                successors.Add(instruction.Label, edges);
            }
            foreach (var (label, targets) in successors)
            {
                foreach (Int32 target in targets)
                {
                    if (target != EndLabel && !successors.ContainsKey(target))
                        throw new InvalidDataException($"Instruction at label {label} targets missing instruction label {target}.");
                }
            }
            return successors;
        }

        public void ValidateObjectSlots(Int32 objectCount) => ValidateObjectSlots(objectCount, null);

        public void ValidateObjectSlots(Int32 objectCount, Func<Int32, Boolean> isDeclared)
        {
            foreach (EVInstruction instruction in Instructions)
            {
                if (instruction.Opcode is not (Jsm.Opcode.NEW or Jsm.Opcode.NEW2 or Jsm.Opcode.NEW3) || instruction.Operands.Length != 2)
                    continue;

                Int32 target = instruction.Operands[0].Value;
                Boolean partySlot = instruction.Opcode == Jsm.Opcode.NEW3 && target >= 251 && target <= 254;
                if (!partySlot && ((UInt32)target >= objectCount || isDeclared?.Invoke(target) == false))
                {
                    var error = new InvalidDataException($"{instruction.Opcode} references missing object slot {target}.");
                    error.Data[nameof(EVInstruction.Label)] = instruction.Label;
                    throw error;
                }
            }
        }

        public Byte[] Encode() => Encode(out _);

        public Byte[] Encode(out EVAddressCheckpoint[] checkpoints)
        {
            GetSuccessors();
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            var positions = new Dictionary<Int32, Int32>();
            var addressChecks = new List<EVAddressCheckpoint>();
            var relocations = new List<(Int32 Slot, Int32 Base, Int32 Target, Boolean Signed)>();
            foreach (EVInstruction instruction in Instructions)
            {
                Int32 start = (Int32)stream.Position;
                positions.Add(instruction.Label, start);
                try
                {
                    WriteInstruction(writer, instruction, relocations);
                    if (instruction.WatchedObjectIp is Int32 expected)
                    {
                        if (instruction.Opcode != Jsm.Opcode.MSPEED)
                            throw new InvalidDataException("An address check must precede MSPEED.");
                        addressChecks.Add(new EVAddressCheckpoint(expected, (Int32)stream.Position));
                    }
                }
                catch (InvalidDataException ex)
                {
                    var error = new InvalidDataException($"Instruction {instruction.Opcode} at script offset 0x{start:X}: {ex.Message}", ex);
                    error.Data[nameof(EVInstruction.Label)] = instruction.Label;
                    throw error;
                }
            }
            positions.Add(EndLabel, (Int32)stream.Position);
            foreach (var relocation in relocations)
            {
                stream.Position = relocation.Slot;
                Int32 displacement = positions[relocation.Target] - relocation.Base;
                WriteInteger(writer, displacement, 2, relocation.Signed, $"branch displacement to label {relocation.Target}");
            }
            checkpoints = addressChecks.ToArray();
            return stream.ToArray();
        }

        public IReadOnlyList<EVBasicBlock> GetBasicBlocks()
        {
            IReadOnlyDictionary<Int32, Int32[]> successors = GetSuccessors();
            var starts = Instructions.SelectMany(instruction => instruction.Targets).ToHashSet();
            var blocks = new List<EVBasicBlock>();
            var current = new List<EVInstruction>();
            for (Int32 index = 0; index < Instructions.Count; index++)
            {
                EVInstruction instruction = Instructions[index];
                current.Add(instruction);
                if (instruction.Targets.Length != 0 || instruction.Opcode == Jsm.Opcode.Return ||
                    index + 1 == Instructions.Count || starts.Contains(Instructions[index + 1].Label))
                {
                    blocks.Add(new EVBasicBlock(current.ToArray(), successors[instruction.Label]));
                    current.Clear();
                }
            }
            return blocks;
        }

        private static void ReadInstruction(BinaryReader reader, EVInstruction instruction)
        {
            Int32 position = (Int32)reader.BaseStream.Position;
            switch (instruction.Opcode)
            {
                case Jsm.Opcode.EXPR:
                    instruction.Expression = ReadExpression(reader);
                    return;
                case Jsm.Opcode.JMP:
                case Jsm.Opcode.JMP_IF:
                case Jsm.Opcode.JMP_IFN:
                    instruction.Targets = new[] { position + 2 + ReadInteger(reader, 2, instruction.Opcode != Jsm.Opcode.JMP_IFN) };
                    return;
                case Jsm.Opcode.JMP_SWITCH:
                case Jsm.Opcode.JMP_SWITCHEX:
                case Jsm.Opcode.rsv0d:
                    ReadSwitch(reader, instruction, position);
                    return;
            }

            if (instruction.Opcode == Jsm.Opcode.QUAD)
            {
                instruction.ArgumentMask = reader.ReadByte();
                EVOperand count = ReadOperand(reader, instruction.Opcode, instruction.ArgumentMask, 0, '1');
                Int32 vertices = GetVertexCount(count);
                if ((UInt32)vertices > Byte.MaxValue)
                    throw new InvalidDataException($"Invalid QUAD vertex count {vertices}.");
                var operands = new EVOperand[1 + 2 * vertices];
                operands[0] = count;
                for (Int32 index = 1; index < operands.Length; index++)
                    operands[index] = ReadOperand(reader, instruction.Opcode, instruction.ArgumentMask, index, 'b');
                instruction.Operands = operands;
                return;
            }

            String layout = EVOperands.For(instruction.Opcode);
            Boolean hasMask = layout.StartsWith("m", StringComparison.Ordinal);
            if (hasMask)
                instruction.ArgumentMask = reader.ReadByte();

            Int32 start = hasMask ? 1 : 0;
            instruction.Operands = new EVOperand[layout.Length - start];
            for (Int32 index = 0; index < instruction.Operands.Length; index++)
                instruction.Operands[index] = ReadOperand(reader, instruction.Opcode, instruction.ArgumentMask, index, layout[index + start]);
        }

        private static EVOperand ReadOperand(BinaryReader reader, Jsm.Opcode opcode, Byte mask, Int32 index, Char kind)
        {
            if (EVOperands.UsesExpression(opcode, mask, index))
                return new EVOperand(ReadExpression(reader));
            return new EVOperand(ReadInteger(reader, EVOperands.Width(kind), EVOperands.IsSigned(kind)));
        }

        private static void ReadSwitch(BinaryReader reader, EVInstruction instruction, Int32 position)
        {
            Boolean extended = instruction.Opcode == Jsm.Opcode.JMP_SWITCHEX;
            Boolean wide = instruction.Opcode == Jsm.Opcode.rsv0d;
            Int32 offsetBase = position + (extended ? 3 : wide ? 1 : 0);
            Int32 count = wide ? reader.ReadUInt16() : reader.ReadByte();
            instruction.CaseValues = new UInt16[extended ? count : 1];
            if (!extended)
                instruction.CaseValues[0] = reader.ReadUInt16();
            instruction.Targets = new Int32[count + 1];
            instruction.Targets[0] = offsetBase + reader.ReadUInt16();
            for (Int32 index = 0; index < count; index++)
            {
                if (extended)
                    instruction.CaseValues[index] = reader.ReadUInt16();
                instruction.Targets[index + 1] = offsetBase + reader.ReadUInt16();
            }
        }

        private static void WriteInstruction(BinaryWriter writer, EVInstruction instruction, List<(Int32 Slot, Int32 Base, Int32 Target, Boolean Signed)> relocations)
        {
            Boolean isExpression = instruction.Opcode == Jsm.Opcode.EXPR;
            Boolean isJump = instruction.Opcode is Jsm.Opcode.JMP or Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN;
            Boolean isSwitch = instruction.Opcode is Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d;
            if (isExpression != (instruction.Expression != null) ||
                (!isJump && !isSwitch && instruction.Targets.Length != 0) ||
                (!isSwitch && instruction.CaseValues.Length != 0) ||
                ((isExpression || isJump || isSwitch) && (instruction.Operands.Length != 0 || instruction.ArgumentMask != 0)))
                throw new InvalidDataException("Instruction fields do not match the opcode.");
            Int32 opcode = (Int32)instruction.Opcode;
            if (opcode < 0 || opcode > 0x1FE || opcode == 0xFF)
                throw new InvalidDataException($"Invalid opcode 0x{opcode:X}.");
            if (opcode > Byte.MaxValue)
                writer.Write(Byte.MaxValue);
            writer.Write((Byte)opcode);

            Int32 position = (Int32)writer.BaseStream.Position;
            switch (instruction.Opcode)
            {
                case Jsm.Opcode.EXPR:
                    WriteExpression(writer, instruction.Expression);
                    return;
                case Jsm.Opcode.JMP:
                case Jsm.Opcode.JMP_IF:
                case Jsm.Opcode.JMP_IFN:
                    if (instruction.Targets.Length != 1)
                        throw new InvalidDataException("Expected one branch target.");
                    relocations.Add((position, position + 2, instruction.Targets[0], instruction.Opcode != Jsm.Opcode.JMP_IFN));
                    writer.Write((UInt16)0);
                    return;
                case Jsm.Opcode.JMP_SWITCH:
                case Jsm.Opcode.JMP_SWITCHEX:
                case Jsm.Opcode.rsv0d:
                    WriteSwitch(writer, instruction, relocations, position);
                    return;
            }

            if (instruction.Opcode == Jsm.Opcode.QUAD)
            {
                Int32 count = instruction.Operands.Length == 0 ? -1 : GetVertexCount(instruction.Operands[0]);
                if ((UInt32)count > Byte.MaxValue || instruction.Operands.Length != 1 + 2 * count)
                    throw new InvalidDataException("QUAD requires a vertex count followed by that many X/Z pairs.");
                writer.Write(instruction.ArgumentMask);
                for (Int32 index = 0; index < instruction.Operands.Length; index++)
                    WriteOperand(writer, instruction, index, index == 0 ? '1' : 'b');
                return;
            }

            String layout = EVOperands.For(instruction.Opcode);
            Boolean hasMask = layout.StartsWith("m", StringComparison.Ordinal);
            if (hasMask)
                writer.Write(instruction.ArgumentMask);
            else if (instruction.ArgumentMask != 0)
                throw new InvalidDataException("This opcode has no argument mask.");

            Int32 start = hasMask ? 1 : 0;
            if (instruction.Operands.Length != layout.Length - start)
                throw new InvalidDataException($"Expected {layout.Length - start} operands, got {instruction.Operands.Length}.");
            for (Int32 index = 0; index < instruction.Operands.Length; index++)
                WriteOperand(writer, instruction, index, layout[index + start]);
        }

        private static void WriteOperand(BinaryWriter writer, EVInstruction instruction, Int32 index, Char kind)
        {
            EVOperand operand = instruction.Operands[index];
            Boolean expression = EVOperands.UsesExpression(instruction.Opcode, instruction.ArgumentMask, index);
            if (expression != (operand.Expression != null))
                throw new InvalidDataException($"Argument mask does not match operand {index + 1}.");
            if (expression)
                WriteExpression(writer, operand.Expression, requiresValue: true);
            else
                WriteInteger(writer, operand.Value, EVOperands.Width(kind), EVOperands.IsSigned(kind), $"operand {index + 1}");
        }

        private static void WriteSwitch(BinaryWriter writer, EVInstruction instruction, List<(Int32 Slot, Int32 Base, Int32 Target, Boolean Signed)> relocations, Int32 position)
        {
            Boolean extended = instruction.Opcode == Jsm.Opcode.JMP_SWITCHEX;
            Boolean wide = instruction.Opcode == Jsm.Opcode.rsv0d;
            Int32 count = instruction.Targets.Length - 1;
            if (instruction.CaseValues.Length != (extended ? count : 1))
                throw new InvalidDataException("Invalid switch case table.");

            WriteInteger(writer, count, wide ? 2 : 1, false, "case count");
            if (!extended)
                writer.Write(instruction.CaseValues[0]);

            Int32 offsetBase = position + (extended ? 3 : wide ? 1 : 0);
            for (Int32 index = 0; index <= count; index++)
            {
                if (extended && index > 0)
                    writer.Write(instruction.CaseValues[index - 1]);
                relocations.Add(((Int32)writer.BaseStream.Position, offsetBase, instruction.Targets[index], false));
                writer.Write((UInt16)0);
            }
        }

        private static Int32 GetVertexCount(EVOperand count)
        {
            if (count.Expression == null)
                return count.Value;
            if (count.Expression.Length == 1)
            {
                EVExpressionToken token = count.Expression[0];
                if (token.Opcode == (Byte)Jsm.Excode.Const16)
                    return token.Value;
                if (token.Opcode == (Byte)Jsm.Excode.Const26)
                    return token.Value << 6 >> 6;
            }
            throw new InvalidDataException("QUAD vertex count must be constant to determine the operand layout.");
        }

        private static EVExpressionToken[] ReadExpression(BinaryReader reader)
        {
            var tokens = new List<EVExpressionToken>();
            while (true)
            {
                Byte opcode = reader.ReadByte();
                if (opcode == (Byte)Jsm.Excode.End)
                    return tokens.ToArray();
                Int32 width = ExpressionWidth(opcode);
                tokens.Add(new EVExpressionToken(opcode, ReadInteger(reader, width, opcode == (Byte)Jsm.Excode.Const16)));
            }
        }

        private static void WriteExpression(BinaryWriter writer, EVExpressionToken[] tokens, Boolean requiresValue = false)
        {
            if (tokens == null)
                throw new InvalidDataException("Expected an expression.");
            Int32 depth = 0;
            foreach (EVExpressionToken token in tokens)
            {
                if (token.Opcode == (Byte)Jsm.Excode.End)
                    throw new InvalidDataException("Unexpected expression terminator in the token list.");
                Int32 inputs = ExpressionInputs(token);
                if (depth < inputs)
                    throw new InvalidDataException($"Expression opcode 0x{token.Opcode:X2} needs {inputs} stack values, found {depth}.");
                if (inputs >= 0)
                    depth += 1 - inputs;
                writer.Write(token.Opcode);
                WriteInteger(writer, token.Value, ExpressionWidth(token.Opcode), token.Opcode == (Byte)Jsm.Excode.Const16, $"expression operand 0x{token.Opcode:X2}");
            }
            if (requiresValue && depth == 0)
                throw new InvalidDataException("An expression operand must leave a value on the stack.");
            writer.Write((Byte)Jsm.Excode.End);
        }

        private static Int32 ExpressionInputs(EVExpressionToken token) => token.Opcode switch
        {
            0xD3 => (token.Value >> 16) & Byte.MaxValue,
            >= 0x80 or 41 or 95 or 120 or 121 or 122 or 125 or 126 => 0,
            >= 4 and <= 11 or 13 or 14 or 15 or 16 or 42 or 43 or >= 70 and <= 77 or >= 79 and <= 83 or
                >= 88 and <= 92 or >= 96 and <= 101 or 106 or 107 or >= 109 and <= 113 => 1,
            >= 17 and <= 40 or >= 44 and <= 69 or 78 or >= 84 and <= 87 or 93 or 94 or 102 or 108 => 2,
            0 or 1 or 2 or 3 or 12 or 103 or 104 or 105 or 123 or 124 => 0,
            _ => -1
        };

        private static Int32 ExpressionWidth(Byte opcode) => opcode switch
        {
            0xD3 => 3,
            >= 0x80 => (opcode & 0x20) != 0 ? 2 : 1,
            (Byte)Jsm.Excode.Const26 => 4,
            (Byte)Jsm.Excode.Const16 or (Byte)Jsm.Excode.B_OBJSPECA => 2,
            (Byte)Jsm.Excode.B_PTR or (Byte)Jsm.Excode.B_MEMBER or (Byte)Jsm.Excode.B_SYSLIST or (Byte)Jsm.Excode.B_SYSVAR => 1,
            _ => 0
        };

        private static Int32 ReadInteger(BinaryReader reader, Int32 width, Boolean signed)
        {
            Int32 result = 0;
            for (Int32 index = 0; index < width; index++)
                result |= reader.ReadByte() << (8 * index);
            if (signed && width < 4)
                result = result << (32 - 8 * width) >> (32 - 8 * width);
            return result;
        }

        private static void WriteInteger(BinaryWriter writer, Int32 value, Int32 width, Boolean signed, String field)
        {
            if (width == 0 && value != 0)
                throw new InvalidDataException($"Unexpected value {value} for {field}.");
            if (width > 0 && width < 4)
            {
                Int32 limit = 1 << (8 * width - (signed ? 1 : 0));
                if (value < (signed ? -limit : 0) || value >= limit)
                    throw new InvalidDataException($"Value {value} does not fit {field} ({width * 8} bits).");
            }
            for (Int32 index = 0; index < width; index++)
                writer.Write((Byte)(value >> (8 * index)));
        }
    }
}
