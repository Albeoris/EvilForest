using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

public static class StorageUsageReport
{
    private sealed record Use(EVVariable View, string Access, string Event, int ObjectId, ushort ScriptId, string Context);

    private static readonly Dictionary<(EVStorageType, int), string> Evidence = new()
    {
        [(EVStorageType.UInt16, 0)] = "Memoria EventState.ScenarioCounter and EBin.SC_COUNTER_SVR",
        [(EVStorageType.Int16, 2)] = "Memoria EBin.MAP_INDEX_SVR (0x2D8)",
        [(EVStorageType.UInt16, 2)] = "Memoria EventState.FieldEntrance accesses the same two bytes",
        [(EVStorageType.Int16, 9)] = "Cargo and Alexandria Center slot 0 resident sound IDs; Memoria FF9Snd.GetExtEnvObjNo maps Cargo ID 643",
        [(EVStorageType.Int16, 11)] = "Alexandria Center slot 1 resident sound ID 415; Cargo uses -1 and has no slot 1 play",
        [(EVStorageType.Byte, 13)] = "Cargo sets dialogue variable 2 to slot 0, shows Error Env Play(), and clears this byte",
        [(EVStorageType.Byte, 14)] = "Cargo sets dialogue variable 2 to slot 1, shows Error Env Play(), and clears this byte",
        [(EVStorageType.Byte, 16)] = "Memoria battle.TRANCE_GAUGE_FLAG",
        [(EVStorageType.Byte, 17)] = "Memoria battle.GARNET_DEPRESS_FLAG"
    };

    public static int Write(string input, string summaryPath, string referencesPath)
    {
        var uses = new List<Use>();
        string[] files = Directory.GetFiles(input, "*.eb.bytes").OrderBy(path => path, StringComparer.OrdinalIgnoreCase).ToArray();
        foreach (string file in files)
        {
            using var stream = File.OpenRead(file);
            EVContainer container = EVContainer.Read(stream);
            string name = Path.GetFileName(file)[..^".eb.bytes".Length];
            foreach (EVContainerObject obj in container.Objects)
            {
                foreach (EVContainerScript script in obj.Scripts)
                {
                    IList<EVInstruction> instructions = EVInstructionBody.Decode(script.Body).Instructions;
                    for (int index = 0; index < instructions.Count; index++)
                    {
                        EVInstruction instruction = instructions[index];
                        string previous = index > 0 ? instructions[index - 1].Opcode.ToString() : "Entry";
                        string next = index + 1 < instructions.Count ? instructions[index + 1].Opcode.ToString() : "End";
                        string context = $"{instruction.Label:X4} {previous} / {instruction.Opcode} / {next}";
                        if (instruction.Expression is { } direct)
                            AddUses(direct, name, obj.Id, script.Id, context, uses);
                        foreach (EVOperand operand in instruction.Operands)
                        {
                            if (operand.Expression is { } expression)
                                AddUses(expression, name, obj.Id, script.Id, context, uses);
                        }
                    }
                }
            }
        }

        using var summary = new StreamWriter(summaryPath);
        using var references = new StreamWriter(referencesPath);
        const string columns = "scope\ttype\taddress\tunit\twidthBits\tsigned";
        summary.WriteLine(columns + "\tname\tstatus\treadOnly\twriteOnly\treadWrite\tunknownAccess\tevidence");
        references.WriteLine(columns + "\taccess\tevent\tobject\thandler\tnearbyActions");
        foreach (var group in uses.GroupBy(use => (use.View.Scope, use.View.Type, use.View.Address))
            .OrderBy(group => group.Key.Scope).ThenBy(group => group.Key.Address).ThenBy(group => group.Key.Type))
        {
            EVVariable view = group.First().View;
            string physical = $"{view.Scope}\t{view.Type}\t{view.Address}\t{(view.UsesBitAddress ? "bit" : "byte")}\t{view.Width}\t{view.IsSigned}";
            string name = $"{view.Type}_{view.Address}";
            string status = view.Scope == EVStorageScope.Global ? "Unresolved" : "Physical";
            string evidence = "";
            if (view.Scope == EVStorageScope.Global && StorageDeclarations.KnownGlobals.TryGetValue((view.Type, view.Address), out string? known))
            {
                name = known;
                status = "Confirmed";
                evidence = Evidence[(view.Type, view.Address)];
            }
            summary.WriteLine($"{physical}\t{name}\t{status}\t{group.Count(use => use.Access == "Read")}\t{group.Count(use => use.Access == "Write")}\t{group.Count(use => use.Access == "ReadWrite")}\t{group.Count(use => use.Access == "Unknown")}\t{evidence}");
            foreach (Use use in group)
                references.WriteLine($"{physical}\t{use.Access}\t{use.Event}\t{use.ObjectId}\t{use.ScriptId}\t{use.Context}");
        }
        return files.Length;
    }

    private static void AddUses(EVExpressionToken[] expression, string name, int objectId, ushort scriptId, string context, List<Use> uses)
    {
        var stack = new Stack<int>();
        var writes = new Dictionary<int, string>();
        bool classified = true;
        for (int index = 0; index < expression.Length; index++)
        {
            EVExpressionToken token = expression[index];
            if (StorageDeclarations.IsStorage(token) || token.Opcode is (byte)Jsm.Excode.Const16 or (byte)Jsm.Excode.Const26)
            {
                stack.Push(index);
                continue;
            }
            if (!SourceExpression.TryGet((Jsm.Excode)token.Opcode, out SourceExpression? operation))
            {
                classified = false;
                break;
            }

            int inputs = operation.Payload == ExpressionPayload.None ? operation.Parameters.Length : 0;
            if (stack.Count < inputs)
            {
                classified = false;
                break;
            }
            for (int input = inputs - 1; input >= 0; input--)
            {
                int argument = stack.Pop();
                if (input == 0 && operation.Statement && argument >= 0 && StorageDeclarations.IsStorage(expression[argument]))
                    writes.Add(argument, operation.Opcode == Jsm.Excode.Let ? "Write" : "ReadWrite");
            }
            stack.Push(-1);
        }

        for (int index = 0; index < expression.Length; index++)
        {
            if (!StorageDeclarations.IsStorage(expression[index]))
                continue;
            string access = writes.TryGetValue(index, out string? write) ? write : classified ? "Read" : "Unknown";
            uses.Add(new Use(EVVariable.Decode(expression[index]), access, name, objectId, scriptId, context));
        }
    }
}
