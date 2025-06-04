using System;
using Memoria.EventEngine.EV;
using FF8.JSM;
using Xunit;

namespace EveilForest.CSharp.Tests;

public class EVScriptWriterTests
{
    [Fact]
    public void WriteOpcode_SingleOpcode_WritesCorrectByte()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteOpcode(Jsm.Opcode.MES);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Single(bytecode);
        Assert.Equal((byte)Jsm.Opcode.MES, bytecode[0]);
    }

    [Fact]
    public void WriteByte_SingleByte_WritesCorrectValue()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteByte(128);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Single(bytecode);
        Assert.Equal(128, bytecode[0]);
    }

    [Fact]
    public void WriteInt16_SingleInt16_WritesLittleEndian()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteInt16(0x1234);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Equal(2, bytecode.Length);
        Assert.Equal(0x34, bytecode[0]); // Low byte first (little endian)
        Assert.Equal(0x12, bytecode[1]); // High byte second
    }

    [Fact]
    public void WriteInt16_NegativeValue_WritesCorrectBytes()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteInt16(-1);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Equal(2, bytecode.Length);
        Assert.Equal(0xFF, bytecode[0]); // Low byte
        Assert.Equal(0xFF, bytecode[1]); // High byte
    }

    [Fact]
    public void WriteUInt16_SingleUInt16_WritesLittleEndian()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteUInt16(0x5678);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Equal(2, bytecode.Length);
        Assert.Equal(0x78, bytecode[0]); // Low byte first
        Assert.Equal(0x56, bytecode[1]); // High byte second
    }

    [Fact]
    public void WriteInt24_SingleInt24_WritesThreeBytes()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteInt24(0x123456);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Equal(3, bytecode.Length);
        Assert.Equal(0x56, bytecode[0]); // Low byte first
        Assert.Equal(0x34, bytecode[1]); // Middle byte
        Assert.Equal(0x12, bytecode[2]); // High byte last
    }

    [Fact]
    public void WriteInt32_SingleInt32_WritesFourBytes()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteInt32(0x12345678);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Equal(4, bytecode.Length);
        Assert.Equal(0x78, bytecode[0]); // Low byte first
        Assert.Equal(0x56, bytecode[1]);
        Assert.Equal(0x34, bytecode[2]);
        Assert.Equal(0x12, bytecode[3]); // High byte last
    }

    [Fact]
    public void WriteSByte_NegativeValue_WritesCorrectByte()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteSByte(-1);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Single(bytecode);
        Assert.Equal(0xFF, bytecode[0]); // -1 as unsigned byte is 0xFF
    }

    [Fact]
    public void WriteSByte_PositiveValue_WritesCorrectByte()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        writer.WriteSByte(127);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Single(bytecode);
        Assert.Equal(127, bytecode[0]);
    }

    [Fact]
    public void MultipleWrites_CombineCorrectly()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act - Simulate MES instruction: Byte(2), Byte(128), Int16(35), MES
        writer.WriteByte(2);      // windowId
        writer.WriteByte(128);    // ui
        writer.WriteInt16(35);    // text
        writer.WriteOpcode(Jsm.Opcode.MES);
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Equal(5, bytecode.Length);
        Assert.Equal(2, bytecode[0]);           // windowId
        Assert.Equal(128, bytecode[1]);         // ui
        Assert.Equal(35, bytecode[2]);          // text low byte
        Assert.Equal(0, bytecode[3]);           // text high byte (35 = 0x0023)
        Assert.Equal((byte)Jsm.Opcode.MES, bytecode[4]); // MES opcode
    }

    [Fact]
    public void GetBytecode_CalledMultipleTimes_ReturnsSameResult()
    {
        // Arrange
        var writer = new EVScriptWriter();
        writer.WriteByte(42);
        writer.WriteOpcode(Jsm.Opcode.Return);

        // Act
        var bytecode1 = writer.GetBytecode();
        var bytecode2 = writer.GetBytecode();

        // Assert
        Assert.Equal(bytecode1, bytecode2);
        Assert.Equal(2, bytecode1.Length);
    }

    [Fact]
    public void EmptyWriter_GetBytecode_ReturnsEmptyArray()
    {
        // Arrange
        var writer = new EVScriptWriter();

        // Act
        var bytecode = writer.GetBytecode();

        // Assert
        Assert.Empty(bytecode);
    }
}