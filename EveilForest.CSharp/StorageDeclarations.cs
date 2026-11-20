using System.Text;
using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed class StorageDeclarations(string? eventName = null, int? slot = null)
{
    private readonly HashSet<(byte Opcode, int Address)> _views = new();

    internal static IReadOnlyDictionary<(EVStorageType Type, int Address), string> KnownGlobals { get; } =
        new Dictionary<(EVStorageType, int), string>
        {
            [(EVStorageType.UInt16, 0)] = "ScenarioCounter",
            [(EVStorageType.Int16, 2)] = "FieldEntrance",
            [(EVStorageType.UInt16, 2)] = "FieldEntranceUnsigned",
            [(EVStorageType.Int16, 9)] = "EnvironmentSoundSlot0Id",
            [(EVStorageType.Int16, 11)] = "EnvironmentSoundSlot1Id",
            [(EVStorageType.Byte, 13)] = "EnvironmentSoundSlot0State",
            [(EVStorageType.Byte, 14)] = "EnvironmentSoundSlot1State",
            [(EVStorageType.Byte, 16)] = "TranceGaugeFlag",
            [(EVStorageType.Byte, 17)] = "GarnetDepressionFlag"
        };

    public void Add(EVInstructionBody body)
    {
        foreach (EVInstruction instruction in body.Instructions)
        {
            foreach (EVExpressionToken[] expression in instruction.Operands.Where(operand => operand.Expression != null)
                .Select(operand => operand.Expression).Concat(instruction.Expression == null ? Array.Empty<EVExpressionToken[]>() : new[] { instruction.Expression }))
            {
                foreach (EVExpressionToken token in expression)
                {
                    if (IsStorage(token))
                        _views.Add((token.Opcode, token.Value));
                }
            }
        }
    }

    public static bool IsStorage(EVExpressionToken token) => token.Opcode >= 0x80 && token.Opcode != 0xD3;

    public string Qualify(EVExpressionToken token)
    {
        EVVariable view = EVVariable.Decode(token);
        return $"{ContextName(view.Scope)}.{MemberName(token, view)}";
    }

    public void Append(StringBuilder source, EVStorageScope scope, string access, string indent = "")
    {
        var views = _views.Select(view => new EVExpressionToken(view.Opcode, view.Address))
            .Where(token => EVVariable.Decode(token).Scope == scope)
            .OrderBy(token => token.Value).ThenBy(token => token.Opcode).ToArray();
        if (views.Length == 0)
            return;
        source.AppendLine($"{indent}{access} static class {ContextName(scope)}");
        source.AppendLine($"{indent}{{");
        foreach (EVExpressionToken token in views)
        {
            EVVariable view = EVVariable.Decode(token);
            string member = MemberName(token, view);
            bool forcedWide = (token.Opcode & 0x20) != 0 && view.Address <= byte.MaxValue;
            bool alternate = (token.Opcode & 0x40) == 0;
            string attribute = $"StorageView(StorageScope.{scope}, StorageType.{view.Type}, {view.Address}";
            if (forcedWide)
                attribute += ", wide: true";
            if (alternate)
                attribute += ", alternatePrefix: true";
            string arguments = $"StorageScope.{scope}, StorageType.{view.Type}, {view.Address}";
            string type = view.UsesBitAddress ? "bool" : "int";
            string getter = view.UsesBitAddress ? $"ScriptMemory.Read({arguments}) != 0" : $"ScriptMemory.Read({arguments})";
            string value = view.UsesBitAddress ? "value ? 1 : 0" : "value";
            source.AppendLine($"{indent}    [{attribute})]");
            source.AppendLine($"{indent}    public static {type} {member}");
            source.AppendLine($"{indent}    {{");
            source.AppendLine($"{indent}        get => {getter};");
            source.AppendLine($"{indent}        set => ScriptMemory.Write({arguments}, {value});");
            source.AppendLine($"{indent}    }}");
            source.AppendLine();
        }
        source.AppendLine($"{indent}}}");
        source.AppendLine();
    }

    private static string ContextName(EVStorageScope scope) => scope switch
    {
        EVStorageScope.Global => "Globals",
        EVStorageScope.Map => "Location",
        EVStorageScope.Instance => "Local",
        _ => "Special"
    };

    private string MemberName(EVExpressionToken token, EVVariable view)
    {
        string name = view.Scope == EVStorageScope.Global && KnownGlobals.TryGetValue((view.Type, view.Address), out string? known)
            ? known : CargoStorageNames.Lookup(eventName, slot, view) ?? $"{view.Type}_{view.Address}";
        if ((token.Opcode & 0x20) != 0 && view.Address <= byte.MaxValue)
            name += "_Wide";
        if ((token.Opcode & 0x40) == 0)
            name += "_Alternate";
        return name;
    }
}
