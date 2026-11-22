using System.Text;
using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed class StorageDeclarations(string? eventName = null, int? slot = null)
{
    private readonly HashSet<(byte Opcode, int Address)> _views = new();
    public EventSceneSymbols? Scene { get; set; }
    public IReadOnlyDictionary<(EVStorageType Type, int Address), string>? CutsceneNames { get; set; }
    private static readonly IReadOnlyDictionary<int, string> ObjectMembers = typeof(ObjectView).GetProperties()
        .Where(property => property.IsDefined(typeof(ObjectPropertyAttribute)))
        .ToDictionary(property => (int)property.GetCustomAttribute<ObjectPropertyAttribute>()!.Ordinal, property => property.Name);

    public string? FormatObjectProperty(int payload)
    {
        int selector = payload & 0xFF;
        int ordinal = payload >> 8;
        if (!ObjectMembers.TryGetValue(ordinal, out string? property))
            return null;
        if (Scene != null)
        {
            string? actor = selector == 250 ? "ControlledActor" :
                Scene.Participants.GetValueOrDefault(selector) is { Kind: 2 } participant ? participant.Name : null;
            if (actor != null)
                return ordinal == 3 ? $"evt.{actor}.Facing.RawTurn" : $"evt.{actor}.Properties.{property}";
        }
        string receiver = Enum.IsDefined((ActorSelector)selector) ? $"ActorSelector.{(ActorSelector)selector}" : $"new InstanceUid({selector})";
        return $"ObjectProperties.Of({receiver}).{property}";
    }

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

    public void NameCutsceneStorage(EVVariable selector, int? cueAddress)
    {
        var names = new Dictionary<(EVStorageType Type, int Address), string> { [(selector.Type, selector.Address)] = "CutscenePhase" };
        if (cueAddress is int address)
        {
            EVStorageType[] types = _views.Select(view => EVVariable.Decode(new EVExpressionToken(view.Opcode, view.Address)))
                .Where(view => view.Scope == EVStorageScope.Map && view.Address == address && view.UsesBitAddress).Select(view => view.Type).Distinct().ToArray();
            if (types is [var type])
                names.Add((type, address), "AdvanceCutscenePhaseRequested");
        }
        CutsceneNames = names;
    }

    public string Qualify(EVExpressionToken token)
    {
        EVVariable view = EVVariable.Decode(token);
        return $"{ContextName(view.Scope)}.{MemberName(token, view)}";
    }

    public void Append(StringBuilder source, EVStorageScope scope, string access, string indent = "", bool instance = false, string? className = null, bool emitEmpty = false)
    {
        var views = _views.Select(view => new EVExpressionToken(view.Opcode, view.Address))
            .Where(token => EVVariable.Decode(token).Scope == scope)
            .OrderBy(token => token.Value).ThenBy(token => token.Opcode).ToArray();
        if (views.Length == 0 && !emitEmpty)
            return;
        string modifier = instance ? "sealed" : "static";
        string propertyModifier = instance ? "" : "static ";
        string owner = className ?? ContextName(scope);
        source.AppendLine($"{indent}{access} {modifier} class {owner}");
        source.AppendLine($"{indent}{{");
        if (scope is EVStorageScope.Map or EVStorageScope.Instance && views.Length != 0)
        {
            AppendLayout(source, scope, views, indent, propertyModifier);
            source.AppendLine($"{indent}}}");
            source.AppendLine();
            return;
        }
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
            string type = view.UsesBitAddress ? "bool" : "int";
            string getter = $"ScriptMemory.ReadView(typeof({owner}))" + (view.UsesBitAddress ? " != 0" : "");
            string value = view.UsesBitAddress ? "value ? 1 : 0" : "value";
            source.AppendLine($"{indent}    [{attribute})]");
            source.AppendLine($"{indent}    public {propertyModifier}{type} {member}");
            source.AppendLine($"{indent}    {{");
            source.AppendLine($"{indent}        get => {getter};");
            source.AppendLine($"{indent}        set => ScriptMemory.WriteView(typeof({owner}), {value});");
            source.AppendLine($"{indent}    }}");
            source.AppendLine();
        }
        source.AppendLine($"{indent}}}");
        source.AppendLine();
    }

    private void AppendLayout(StringBuilder source, EVStorageScope scope, EVExpressionToken[] views, string indent, string propertyModifier)
    {
        source.AppendLine($"{indent}    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Pack = 1)]");
        source.AppendLine($"{indent}    private struct MemoryLayout");
        source.AppendLine($"{indent}    {{");
        foreach (EVExpressionToken token in views)
        {
            EVVariable view = EVVariable.Decode(token);
            int offset = view.UsesBitAddress ? view.Address / 8 : view.Address;
            var encoding = new List<string>();
            if ((token.Opcode & 0x20) != 0 && view.Address <= byte.MaxValue)
                encoding.Add("wide: true");
            if ((token.Opcode & 0x40) == 0)
                encoding.Add("alternatePrefix: true");
            if (view.Type == EVStorageType.SBit)
                encoding.Add("signedBit: true");
            if (view.Type == EVStorageType.UInt24)
                encoding.Add("unsigned24: true");
            if (encoding.Count != 0)
                source.AppendLine($"{indent}        [StorageEncoding({string.Join(", ", encoding)})]");
            source.AppendLine($"{indent}        [System.Runtime.InteropServices.FieldOffset({offset})]");
            string cell = view.UsesBitAddress ? $"BitCell<Bit{view.Address % 8}>" :
                view.Type == EVStorageType.UInt24 ? "Int24Cell" : $"{view.Type}Cell";
            source.AppendLine($"{indent}        internal {cell} {MemberName(token, view)};");
        }
        source.AppendLine($"{indent}    }}");
        source.AppendLine();
        source.AppendLine($"{indent}    private {propertyModifier}ref MemoryLayout Memory => ref ScriptMemory.View<MemoryLayout>(StorageScope.{scope});");
        foreach (EVExpressionToken token in views)
        {
            EVVariable view = EVVariable.Decode(token);
            string member = MemberName(token, view);
            source.AppendLine();
            source.AppendLine($"{indent}    public {propertyModifier}{(view.UsesBitAddress ? "bool" : "int")} {member}");
            source.AppendLine($"{indent}    {{");
            source.AppendLine($"{indent}        get => Memory.{member}.Value;");
            source.AppendLine($"{indent}        set => Memory.{member}.Value = value;");
            source.AppendLine($"{indent}    }}");
        }
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
            ? known : CargoStorageNames.Lookup(eventName, slot, view) ??
                (view.Scope == EVStorageScope.Map ? CutsceneNames?.GetValueOrDefault((view.Type, view.Address)) : null) ?? $"{view.Type}_{view.Address}";
        if ((token.Opcode & 0x20) != 0 && view.Address <= byte.MaxValue)
            name += "_Wide";
        if ((token.Opcode & 0x40) == 0)
            name += "_Alternate";
        return name;
    }
}
