using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal static class CutsceneMemoryIndex
{
    public static IEnumerable<string> Describe(int slot, ushort tag, EVInstructionBody body)
    {
        var bytes = new SortedDictionary<(EVStorageScope Scope, int Owner, int Byte), (int Read, int Write, int Unknown)>();
        foreach (var (variable, read, write) in body.Instructions.SelectMany(Accesses))
            Record(variable, read, write);
        foreach (var (cell, access) in bytes)
            yield return $"{slot}\t{tag}\t{cell.Scope}\t{cell.Owner}\t{cell.Byte}\t{access.Read:X2}\t{access.Write:X2}\t{access.Unknown:X2}";

        void Record(EVVariable variable, bool read, bool write)
        {
            int address = variable.UsesBitAddress ? variable.Address / 8 : variable.Address;
            int mask = variable.UsesBitAddress ? 1 << (variable.Address % 8) : 0xFF;
            int width = variable.UsesBitAddress ? 1 : variable.Width / 8;
            for (int index = 0; index < width; index++)
            {
                var key = (variable.Scope, variable.Scope == EVStorageScope.Instance ? slot : -1, address + index);
                var previous = bytes.GetValueOrDefault(key);
                bytes[key] = (previous.Read | (read ? mask : 0), previous.Write | (write ? mask : 0),
                    previous.Unknown | (!read && !write ? mask : 0));
            }
        }
    }

    public static IEnumerable<(EVVariable Variable, bool Read, bool Write)> Accesses(EVInstruction instruction)
    {
        if (instruction.Expression != null)
        {
            foreach (var access in Accesses(instruction.Expression))
                yield return access;
        }

        foreach (EVOperand operand in instruction.Operands)
        {
            if (operand.Expression == null)
                continue;
            foreach (var access in Accesses(operand.Expression))
                yield return access;
        }
    }

    private static IEnumerable<(EVVariable Variable, bool Read, bool Write)> Accesses(IReadOnlyList<EVExpressionToken> tokens)
    {
        var values = new Stack<EVVariable?>();
        var accesses = new List<(EVVariable Variable, bool Read, bool Write)>();
        foreach (EVExpressionToken token in tokens)
        {
            if (StorageDeclarations.IsStorage(token))
            {
                values.Push(EVVariable.Decode(token));
                continue;
            }
            if ((Jsm.Excode)token.Opcode is Jsm.Excode.Const16 or Jsm.Excode.Const26)
            {
                values.Push(null);
                continue;
            }

            if (!SourceExpression.TryGet((Jsm.Excode)token.Opcode, out var operation) ||
                operation.Payload == ExpressionPayload.None && operation.Parameters.Length > values.Count)
                return tokens.Where(StorageDeclarations.IsStorage).Select(unresolved => (EVVariable.Decode(unresolved), false, false));

            int arity = operation.Payload == ExpressionPayload.None ? operation.Parameters.Length : 0;
            for (int argument = arity - 1; argument >= 0; argument--)
            {
                if (values.Pop() is not EVVariable variable)
                    continue;
                bool write = argument == 0 && operation.Statement && operation.Parameters[0] == "target";
                accesses.Add((variable, !write || operation.Opcode != Jsm.Excode.Let, write));
            }
            values.Push(null);
        }

        foreach (EVVariable? value in values)
        {
            if (value is EVVariable variable)
                accesses.Add((variable, true, false));
        }
        return accesses;
    }
}
