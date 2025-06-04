using System;
using System.Collections.Generic;
using System.IO;
using FF8.JSM;

namespace EveilForest.CSharp;

/// <summary>
/// Writer for EVScript bytecode generation
/// </summary>
public sealed class EVScriptWriter
{
    private readonly MemoryStream _stream;
    private readonly List<Label> _labels;
    private readonly List<JumpPlaceholder> _jumpPlaceholders;

    public EVScriptWriter()
    {
        _stream = new MemoryStream();
        _labels = new List<Label>();
        _jumpPlaceholders = new List<JumpPlaceholder>();
    }

    public void WriteOpcode(Jsm.Opcode opcode)
    {
        _stream.WriteByte((byte)opcode);
    }

    public void WriteByte(byte value)
    {
        _stream.WriteByte(value);
    }

    public void WriteSByte(sbyte value)
    {
        _stream.WriteByte((byte)value);
    }

    public void WriteInt16(short value)
    {
        var bytes = BitConverter.GetBytes(value);
        _stream.Write(bytes, 0, 2);
    }

    public void WriteUInt16(ushort value)
    {
        var bytes = BitConverter.GetBytes(value);
        _stream.Write(bytes, 0, 2);
    }

    public void WriteInt24(int value)
    {
        // Write 24-bit integer (3 bytes)
        var bytes = BitConverter.GetBytes(value);
        _stream.Write(bytes, 0, 3);
    }

    public void WriteInt32(int value)
    {
        var bytes = BitConverter.GetBytes(value);
        _stream.Write(bytes, 0, 4);
    }

    public Label CreateLabel()
    {
        var label = new Label(_labels.Count);
        _labels.Add(label);
        return label;
    }

    public void PlaceLabel(Label label)
    {
        label.Position = (int)_stream.Position;
    }

    public void WriteJump(Label targetLabel)
    {
        // Write JMP opcode
        WriteOpcode(Jsm.Opcode.JMP);
        
        // Add placeholder for jump address
        var placeholder = new JumpPlaceholder
        {
            Position = (int)_stream.Position,
            TargetLabel = targetLabel
        };
        _jumpPlaceholders.Add(placeholder);
        
        // Write placeholder bytes (will be filled in later)
        WriteInt16(0);
    }

    public void WriteConditionalJump(Jsm.Opcode jumpOpcode, object leftOperand, object rightOperand, Label targetLabel)
    {
        // For now, write a simplified conditional jump
        // In a full implementation, this would need proper operand encoding
        
        WriteOpcode(jumpOpcode);
        
        // Write operands based on their types
        WriteOperand(leftOperand);
        WriteOperand(rightOperand);
        
        // Add placeholder for jump address
        var placeholder = new JumpPlaceholder
        {
            Position = (int)_stream.Position,
            TargetLabel = targetLabel
        };
        _jumpPlaceholders.Add(placeholder);
        
        // Write placeholder bytes
        WriteInt16(0);
    }

    public void WriteVariableAssignment(string targetVariable, object value)
    {
        // For now, write a simplified assignment instruction
        // In a full implementation, this would map to proper SET instructions
        
        // Use EXPR opcode for variable assignment
        WriteOpcode(Jsm.Opcode.EXPR);
        
        // Write target variable (simplified)
        var variableId = GetVariableId(targetVariable);
        WriteByte(variableId);
        
        // Write value
        WriteOperand(value);
    }

    private void WriteOperand(object operand)
    {
        switch (operand)
        {
            case byte b:
                WriteByte(b);
                break;
            case sbyte sb:
                WriteSByte(sb);
                break;
            case short s:
                WriteInt16(s);
                break;
            case ushort us:
                WriteUInt16(us);
                break;
            case int i:
                WriteInt32(i);
                break;
            case bool b:
                WriteByte((byte)(b ? 1 : 0));
                break;
            case string str:
                // For string operands, we need to map them to appropriate values
                // This is a simplified implementation
                var mappedValue = MapStringOperand(str);
                WriteOperand(mappedValue);
                break;
            default:
                throw new NotSupportedException($"Operand type {operand?.GetType().Name} is not supported");
        }
    }

    private object MapStringOperand(string operand)
    {
        // Map string operands like "@evt.Byte_44" to appropriate values
        // This is a simplified mapping - in practice would be more sophisticated
        
        if (operand.Contains("Byte_"))
        {
            // Extract byte variable index
            var parts = operand.Split('_');
            if (parts.Length > 1 && int.TryParse(parts[^1], out int index))
            {
                return (byte)index;
            }
        }
        
        // Default to 0 for unmapped operands
        return (byte)0;
    }

    private byte GetVariableId(string targetVariable)
    {
        // Map variable names to IDs
        // This is a simplified mapping - in practice would be more sophisticated
        
        if (targetVariable.Contains("Byte_"))
        {
            var parts = targetVariable.Split('_');
            if (parts.Length > 1 && int.TryParse(parts[^1], out int index))
            {
                return (byte)index;
            }
        }
        
        // Default variable ID
        return 0;
    }

    public byte[] GetBytecode()
    {
        // Resolve all jump placeholders
        ResolveJumps();
        
        return _stream.ToArray();
    }

    private void ResolveJumps()
    {
        foreach (var placeholder in _jumpPlaceholders)
        {
            if (placeholder.TargetLabel.Position == -1)
            {
                throw new InvalidOperationException($"Label {placeholder.TargetLabel.Id} was not placed");
            }
            
            // Calculate relative jump offset
            int jumpOffset = placeholder.TargetLabel.Position - (placeholder.Position + 2);
            
            // Write the resolved jump offset
            var originalPosition = _stream.Position;
            _stream.Position = placeholder.Position;
            WriteInt16((short)jumpOffset);
            _stream.Position = originalPosition;
        }
    }

    public sealed class Label
    {
        public int Id { get; }
        public int Position { get; set; } = -1;

        public Label(int id)
        {
            Id = id;
        }
    }

    private sealed class JumpPlaceholder
    {
        public int Position { get; set; }
        public Label TargetLabel { get; set; } = null!;
    }
}