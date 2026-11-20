using System.Buffers.Binary;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class InstructionCodecTests
{
    [Fact]
    public void ArgumentMaskAndLiteralWidthTakeEffectWhenEdited()
    {
        byte[] original = Convert.FromHexString("22800104");
        EVInstructionBody body = EVInstructionBody.Decode(original);
        EVInstruction wait = body.Instructions[0];
        wait.ArgumentMask |= 1;
        wait.Operands[0] = new EVOperand(new[] { EVExpressionToken.Literal(32768) });
        byte[] edited = body.Encode();
        Assert.Equal(Convert.FromHexString("22817E008000007F04"), edited);
        Assert.Equal(new[] { 32768 }, WaitTrace.Run(edited));

        wait.Operands[0] = new EVOperand(new[] { EVExpressionToken.Literal(32767) });
        Assert.Equal(Convert.FromHexString("22817DFF7F7F04"), body.Encode());
        Assert.Throws<InvalidDataException>(() => EVExpressionToken.Literal(1 << 25));
    }

    [Fact]
    public void InsertionBeforeATargetRelocatesTheBranchToTheOriginalInstruction()
    {
        byte[] original = Convert.FromHexString("01030022000122000204");
        EVInstructionBody body = EVInstructionBody.Decode(original);
        body.Instructions.Insert(2, new EVInstruction(100, FF8.JSM.Jsm.Opcode.WAIT)
        {
            Operands = new[] { new EVOperand(3) }
        });
        Assert.Equal(6, Assert.Single(body.GetBasicBlocks()[0].Successors));
        byte[] edited = body.Encode();
        Assert.Equal(Convert.FromHexString("01060022000122000322000204"), edited);
        Assert.Equal(new[] { 2 }, WaitTrace.Run(original));
        Assert.Equal(new[] { 2 }, WaitTrace.Run(edited));
    }

    [Fact]
    public void VariableDescriptorsKeepByteAndBitAddressesAndBoth24BitEncodings()
    {
        var signed = new EVVariable(EVStorageScope.Map, EVStorageType.Int24, 9);
        var unsigned = new EVVariable(EVStorageScope.Map, EVStorageType.UInt24, 9);
        Assert.Equal((0xC9, 9), ((int)signed.Encode().Opcode, signed.Encode().Value));
        Assert.Equal((0xCD, 9), ((int)unsigned.Encode().Opcode, unsigned.Encode().Value));
        Assert.IsType<EVVariable>(EVVariable.Decode(signed.Encode()));
        Assert.True(signed.IsSigned);
        Assert.False(unsigned.IsSigned);
        Assert.Equal(24, unsigned.Width);
        EVVariable bit = unsigned.Bit(19);
        Assert.Equal(91, bit.Address);
        Assert.True(bit.UsesBitAddress);
        Assert.Equal(9, bit.Address / 8 - 2);
        Assert.Equal(5, new EVVariable(EVStorageScope.Instance, EVStorageType.Int16, 5).Encode().Value);
    }

    [Fact]
    public void EvaluationOrderOfSideEffectingExpressionsRemainsInTheByteStream()
    {
        // Assign Byte_0 = 0, then wait for Byte_0++ * 10 + Byte_0++.
        byte[] source = Convert.FromHexString("05D6007D00002C7F2201D600047D0A0011D60004147F04");
        byte[] rebuilt = EVInstructionBody.Decode(source).Encode();
        Assert.Equal(new[] { 1 }, WaitTrace.Run(source));
        Assert.Equal(WaitTrace.Run(source), WaitTrace.Run(rebuilt));
    }
}

internal static class WaitTrace
{
    internal static int[] Run(byte[] bytes, byte initialValue = 0)
    {
        var waits = new List<int>();
        var memory = new byte[256];
        memory[0] = initialValue;
        int ip = 0;
        int condition = 0;
        for (int steps = 0; steps < 1000; steps++)
        {
            switch (bytes[ip++])
            {
                case 0x01:
                    ip = ip + 2 + BinaryPrimitives.ReadInt16LittleEndian(bytes.AsSpan(ip));
                    break;
                case 0x02:
                    ip = ip + 2 + (condition == 0 ? BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(ip)) : 0);
                    break;
                case 0x03:
                    ip = ip + 2 + (condition != 0 ? BinaryPrimitives.ReadInt16LittleEndian(bytes.AsSpan(ip)) : 0);
                    break;
                case 0x04:
                    return waits.ToArray();
                case 0x05:
                    condition = Evaluate();
                    break;
                case 0x06:
                case 0x0B:
                    bool explicitCases = bytes[ip - 1] == 0x06;
                    int offsetBase = ip;
                    int count = bytes[ip++];
                    int first = explicitCases ? 0 : ReadSigned();
                    if (explicitCases)
                        offsetBase += 3;
                    int target = ReadUnsigned();
                    for (int index = 0; index < count; index++)
                    {
                        int value = explicitCases ? ReadUnsigned() : first + index;
                        int candidate = ReadUnsigned();
                        if (value == (explicitCases ? condition & 0xFFFF : condition))
                            target = candidate;
                    }
                    ip = offsetBase + target;
                    break;
                case 0x22:
                    int arg = (bytes[ip++] & 1) == 0 ? bytes[ip++] : Evaluate();
                    if (arg != 0)
                        waits.Add(arg);
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected opcode 0x{bytes[ip - 1]:X2} at {ip - 1}.");
            }
        }
        throw new InvalidOperationException("Fixture failed to return.");

        int ReadSigned()
        {
            int value = BinaryPrimitives.ReadInt16LittleEndian(bytes.AsSpan(ip));
            ip += 2;
            return value;
        }

        int ReadUnsigned() => (ushort)ReadSigned();

        int Evaluate()
        {
            var stack = new Stack<(int Value, bool Address)>();
            while (true)
            {
                int token = bytes[ip++];
                switch (token)
                {
                    case 0xD6:
                        stack.Push((bytes[ip++], true));
                        break;
                    case 0x7D:
                        stack.Push((ReadSigned(), false));
                        break;
                    case 0x7E:
                        int value = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(ip)) << 6 >> 6;
                        ip += 4;
                        stack.Push((value, false));
                        break;
                    case 0x7F:
                        return Take();
                    case 0x04:
                        int address = stack.Pop().Value;
                        stack.Push((memory[address]++, false));
                        break;
                    case 0x11:
                        stack.Push((Take() * Take(), false));
                        break;
                    case 0x14:
                        stack.Push((Take() + Take(), false));
                        break;
                    case 0x18:
                        int upper = Take();
                        stack.Push((Take() < upper ? 1 : 0, false));
                        break;
                    case 0x21:
                        stack.Push((Take() != Take() ? 1 : 0, false));
                        break;
                    case 0x2C:
                        byte assigned = checked((byte)Take());
                        memory[stack.Pop().Value] = assigned;
                        stack.Push((assigned, false));
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected expression opcode 0x{token:X2}.");
                }
            }

            int Take()
            {
                var item = stack.Pop();
                return item.Address ? memory[item.Value] : item.Value;
            }
        }
    }
}
