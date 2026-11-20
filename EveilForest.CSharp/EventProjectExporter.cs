using System.Text;
using EvilForest.Resources;
using EvilForest.Resources.Enums;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

public static class EventProjectExporter
{
    public static void Export(EVContainer container, string eventName, string directory, bool simplify = false, bool structured = true, string? dependency = null)
    {
        Directory.CreateDirectory(directory);
        string eventNamespace = "Event_" + Identifier(eventName);
        if (File.Exists(Path.Combine(directory, "EventSource.cs")))
            throw new InvalidDataException("Export to a new directory to migrate an inline EventSource project.");

        dependency ??= EventSourceDependency.CopyTo(Path.Combine(directory, "lib"));
        string referencePath = System.Security.SecurityElement.Escape(Path.GetRelativePath(directory, dependency).Replace('\\', '/'))!;
        File.WriteAllText(Path.Combine(directory, "Event.csproj"), $$"""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <Nullable>enable</Nullable>
              </PropertyGroup>
              <ItemGroup>
                <Reference Include="EvilForest.EventSource">
                  <HintPath>{{referencePath}}</HintPath>
                </Reference>
              </ItemGroup>
            </Project>
            """);

        var annotations = new EventAnnotations(eventName);
        var eventStorage = new StorageDeclarations(eventName);
        foreach (EVContainerObject obj in container.Objects)
        {
            var bodies = new EVInstructionBody[obj.Scripts.Length];
            var objectStorage = new StorageDeclarations(eventName, obj.Id);
            for (int index = 0; index < obj.Scripts.Length; index++)
            {
                EVContainerScript script = obj.Scripts[index];
                try
                {
                    bodies[index] = EVInstructionBody.Decode(script.Body);
                    EVAddressCheckpoint.MarkOriginal(eventName, obj, index, bodies[index]);
                    eventStorage.Add(bodies[index]);
                    objectStorage.Add(bodies[index]);
                }
                catch (InvalidDataException ex)
                {
                    throw new InvalidDataException($"Object {obj.Id}, script {script.Id} at 0x{script.OriginalOffset:X}: {ex.Message}", ex);
                }
            }

            string typeName = NameObject(obj.Id, bodies);
            string source = FormatObject(obj, bodies, eventNamespace, typeName, simplify, structured, annotations, objectStorage);
            File.WriteAllText(Path.Combine(directory, $"{obj.Id:D2}_{typeName}.cs"), source);
        }

        var eventSource = new StringBuilder();
        eventSource.AppendLine("using EvilForest.EventSource;");
        eventSource.AppendLine();
        eventSource.AppendLine($"[assembly: EventLayout(\"{Convert.ToHexString(container.Header)}\", \"{Convert.ToHexString(container.Trailer)}\")]");
        foreach (var (firstBit, bitCount) in EngineStorageReservations.ForEvent(eventName))
            eventSource.AppendLine($"[assembly: MapStorageReservation({firstBit}, {bitCount})]");
        eventSource.AppendLine();
        eventSource.AppendLine($"namespace {eventNamespace};");
        eventSource.AppendLine();
        eventStorage.Append(eventSource, EVStorageScope.Map, "internal");
        File.WriteAllText(Path.Combine(directory, "Event.cs"), eventSource.ToString());
    }

    public static string CopyDependencyTo(string directory) => EventSourceDependency.CopyTo(directory);

    public static void GenerateDependencySources(string fixtures, string directory)
    {
        Directory.CreateDirectory(directory);
        var operations = new StringBuilder("namespace EvilForest.EventSource;");
        operations.AppendLine();
        AppendCommonOperations(operations);
        File.WriteAllText(Path.Combine(directory, "Operations.Generated.cs"), operations.ToString());

        var resources = new StringBuilder("namespace EvilForest.EventSource;");
        resources.AppendLine().AppendLine();
        ResourceCatalog.Append(resources);
        File.WriteAllText(Path.Combine(directory, "Resources.Generated.cs"), resources.ToString());

        var storage = new StorageDeclarations();
        foreach (string path in Directory.EnumerateFiles(fixtures, "*.eb.bytes", SearchOption.AllDirectories))
        {
            using var input = File.OpenRead(path);
            foreach (EVContainerObject obj in EVContainer.Read(input).Objects)
            {
                foreach (EVContainerScript script in obj.Scripts)
                    storage.Add(EVInstructionBody.Decode(script.Body));
            }
        }
        var globals = new StringBuilder("namespace EvilForest.EventSource;");
        globals.AppendLine().AppendLine();
        storage.Append(globals, EVStorageScope.Global, "public");
        storage.Append(globals, EVStorageScope.Special, "public");
        File.WriteAllText(Path.Combine(directory, "Globals.Generated.cs"), globals.ToString());
    }

    private static void AppendCommonOperations(StringBuilder source)
    {
        AppendEnum<Jsm.Opcode>(source, "Opcode");
        AppendEnum<Jsm.Excode>(source, "Excode");
        AppendEnum<EVStorageScope>(source, "StorageScope");
        AppendEnum<EVStorageType>(source, "StorageType");
        AppendEnum<SystemData>(source, "SystemVariable");
        SourceInstruction.AppendDeclarations(source);
        SourceExpression.AppendDeclarations(source);
    }

    private static void AppendEnum<T>(StringBuilder source, string typeName) where T : struct, Enum
    {
        source.AppendLine();
        source.AppendLine($"public enum {typeName}");
        source.AppendLine("{");
        foreach (string name in Enum.GetNames<T>())
            source.AppendLine($"    {name} = 0x{Convert.ToInt32(Enum.Parse<T>(name)):X},");
        source.AppendLine("}");
    }

    private static string NameObject(int id, IEnumerable<EVInstructionBody> scripts)
    {
        foreach (EVInstruction instruction in scripts.SelectMany(body => body.Instructions))
        {
            if (instruction.Opcode == Jsm.Opcode.QUAD)
                return $"Area_{id:D2}";
            if (instruction.Opcode == Jsm.Opcode.MODEL && instruction.Operands[0].Expression == null &&
                DB.Models.TryGetValue((DBModel.Id)instruction.Operands[0].Value, out DBModel? model))
                return $"{Identifier(model.FileName)}_{id:D2}";
        }
        return $"Object_{id:D2}";
    }

    private static string FormatObject(EVContainerObject obj, EVInstructionBody[] bodies, string eventNamespace, string typeName, bool simplify, bool structured, EventAnnotations annotations, StorageDeclarations storage)
    {
        var source = new StringBuilder();
        source.AppendLine("using EvilForest.EventSource;");
        source.AppendLine("using static EvilForest.EventSource.Vm;");
        source.AppendLine("using static EvilForest.EventSource.Values;");
        source.AppendLine();
        source.AppendLine($"namespace {eventNamespace};");
        source.AppendLine();
        source.AppendLine($"[ObjectSlot({obj.Id}, {obj.VariableCount}, {obj.Flags}, {obj.Reserved}, {obj.BodyOrder ?? -1}, {obj.EmptyOffset}, {obj.Type}, \"{Convert.ToHexString(obj.Gap)}\", \"{Convert.ToHexString(obj.Prefix)}\")]");
        source.AppendLine($"public static class {typeName}");
        source.AppendLine("{");
        storage.Append(source, EVStorageScope.Instance, "private", "    ");
        var methodNames = new HashSet<string>(StringComparer.Ordinal);
        for (int index = 0; index < obj.Scripts.Length; index++)
        {
            ushort id = obj.Scripts[index].Id;
            string method = id == 0 ? "Init" : $"Script_{id:D2}";
            if (DB.ScriptNames.TryGetValue((DBScriptName.Id)id, out DBScriptName? script) && !string.IsNullOrEmpty(script.Name))
                method = Identifier(char.ToUpperInvariant(script.Name[0]) + script.Name.Substring(1));
            string baseName = method;
            for (int suffix = index; !methodNames.Add(method); suffix++)
                method = $"{baseName}_{suffix}";

            if (index > 0)
                source.AppendLine();
            EVInstructionBody body = bodies[index];
            int padding = TrailingPadding(body);
            int instructionCount = body.Instructions.Count - padding;
            bool suspends = body.Instructions.Take(instructionCount).Any(instruction => SourceInstruction.SuspensionOf(instruction.Opcode) != Suspension.Immediate);
            source.AppendLine($"    [Script({id}, {index})]");
            if (padding != 0)
                source.AppendLine($"    [TrailingPadding({padding})]");
            EventControlFlowFormatter CreateFormatter(bool useSwitchLayouts, bool useNativeJumps = false) =>
                new(body, instructionCount, instruction => FormatInstruction(instruction, storage), tokens => FormatValue(tokens, storage),
                    operand => FormatOperand(operand, storage), tokens => SourceExpression.TryFormatCondition(tokens, storage, out string? condition) ? condition : null,
                    tokens => SourceExpression.FormatInteger(tokens, storage) ?? $"(int)({FormatValue(tokens, storage)})", simplify, annotations.For)
                {
                    UseSwitchLayouts = useSwitchLayouts,
                    UseNativeJumps = useNativeJumps
                };

            var formatter = CreateFormatter(useSwitchLayouts: true);
            var statements = new StringBuilder();
            formatter.AppendTo(statements, structured);
            if (formatter.HasConflictingSwitchLayouts)
            {
                formatter = CreateFormatter(useSwitchLayouts: false);
                statements.Clear();
                formatter.AppendTo(statements, structured);
            }
            if (structured && formatter.HasUnstructuredTargets && formatter.CanWriteNativeJumps())
            {
                foreach (bool keepRegions in new[] { true, false })
                {
                    var native = CreateFormatter(useSwitchLayouts: true, useNativeJumps: true);
                    var nativeStatements = new StringBuilder();
                    native.AppendTo(nativeStatements, structured: keepRegions);
                    if (native.HasRawTransfer || native.HasConflictingSwitchLayouts ||
                        keepRegions && !HasVisibleJumpTargets(nativeStatements.ToString()))
                        continue;

                    formatter = native;
                    statements = nativeStatements;
                    break;
                }
            }
            foreach (string layout in formatter.SwitchLayouts)
                source.AppendLine(layout);
            source.AppendLine(suspends ? $"    public static async EventRoutine {method}()" : $"    public static void {method}()");
            source.AppendLine("    {");
            source.Append(statements);
            source.AppendLine("    }");
        }
        source.AppendLine("}");
        return source.ToString();
    }

    private static bool HasVisibleJumpTargets(string statements)
    {
        var method = (MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"async void Inspect() {{{statements}}}")!;
        BlockSyntax body = method.Body!;
        var scopes = new Dictionary<string, Microsoft.CodeAnalysis.SyntaxNode>(StringComparer.Ordinal);
        foreach (var label in body.DescendantNodes().OfType<LabeledStatementSyntax>())
        {
            var scope = label.Ancestors().First(node => node is BlockSyntax or SwitchSectionSyntax);
            if (!scopes.TryAdd(label.Identifier.ValueText, scope))
                return false;
        }

        return body.DescendantNodes().OfType<GotoStatementSyntax>().All(jump =>
            jump.Expression is IdentifierNameSyntax target && scopes.TryGetValue(target.Identifier.ValueText, out var scope) &&
            jump.Ancestors().Contains(scope));
    }

    private static int TrailingPadding(EVInstructionBody body)
    {
        int end = body.Instructions.Count;
        while (end > 0 && body.Instructions[end - 1].Opcode == Jsm.Opcode.NOP)
            end--;
        if (end == body.Instructions.Count || end == 0 || body.Instructions[end - 1].Opcode is not (Jsm.Opcode.Return or Jsm.Opcode.JMP))
            return 0;
        int firstPadding = body.Instructions[end].Label;
        if (body.Instructions.Any(instruction => instruction.Targets.Any(target => target >= firstPadding)))
            return 0;
        return body.Instructions.Count - end;
    }

    private static string FormatInstruction(EVInstruction instruction, StorageDeclarations storage)
    {
        if (instruction.Opcode == Jsm.Opcode.EXPR)
        {
            if (SourceExpression.TryFormat(instruction.Expression, storage, statement: true, out string? assignment))
                return assignment;
            return $"Evaluate({FormatValue(instruction.Expression, storage)})";
        }

        string opcode = Enum.IsDefined(instruction.Opcode) ? $"Opcode.{instruction.Opcode}" : $"(Opcode)0x{(int)instruction.Opcode:X}";
        string mask = $"0x{instruction.ArgumentMask:X2}";
        if (instruction.Targets.Length != 0)
        {
            string[] targets = instruction.Targets.Select(target => $"L_{target:X4}").ToArray();
            if (instruction.Opcode is Jsm.Opcode.JMP or Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN)
                return $"Branch({opcode}, {targets[0]})";
            string caseValues = string.Join(", ", instruction.CaseValues);
            return CallWithTail($"Switch({opcode}, new ushort[] {{ {caseValues} }}, {targets[0]}", targets.Skip(1));
        }

        if (instruction.Opcode == Jsm.Opcode.QUAD)
        {
            string[] vertices = instruction.Operands.Select(operand => FormatOperand(operand, storage)).ToArray();
            return CallWithTail($"Regions.DefinePolygon({vertices[0]}, {mask}", Enumerable.Range(0, (vertices.Length - 1) / 2)
                .Select(index => $"Point(x: {vertices[2 * index + 1]}, z: {vertices[2 * index + 2]})"));
        }
        if (!SourceInstruction.TryGet(instruction, out SourceInstruction? operation))
            return CallWithTail($"Emit({opcode}, {mask}", instruction.Operands.Select(operand => FormatOperand(operand, storage)));

        var arguments = new List<string>();
        for (int index = 0; index < operation.Operands.Length; index++)
        {
            int encodedIndex = operation.FixedOperand is { } fixedOperand && index >= fixedOperand.Index ? index + 1 : index;
            string name = operation.Operands[index];
            string value = FormatServiceOperand(instruction, encodedIndex, name, storage);
            arguments.Add($"{name}: {value}");
        }
        if (operation.HasMask && instruction.ArgumentMask != SourceInstruction.InferMask(instruction.Opcode, instruction.Operands))
            arguments.Add($"mask: {mask}");
        string awaitKeyword = SourceInstruction.SuspensionOf(instruction.Opcode) == Suspension.Immediate ? "" : "await ";
        return $"{awaitKeyword}{operation.Service}.{operation.Method}({string.Join(", ", arguments)})";
    }

    private static string FormatServiceOperand(EVInstruction instruction, int index, string name, StorageDeclarations storage)
    {
        EVOperand operand = instruction.Operands[index];
        if (operand.Expression != null)
            return FormatOperand(operand, storage);

        int value = operand.Value;
        if (name == "model" && DB.Models.TryGetValue((DBModel.Id)value, out DBModel? model))
            return ResourceCatalog.Qualify(model.FileName, value);
        if (name == "animation" && DB.Animations.TryGetValue((DBAnimation.Id)value, out DBAnimation? animation))
            return ResourceCatalog.Qualify(animation.FileName, value);
        if (name == "fieldId" && DB.Events.TryGetValue((DBEvent.Id)value, out DBEvent? evt))
            return ResourceCatalog.Qualify(evt.FileName, value);

        if (instruction.Opcode is Jsm.Opcode.FLDSND0 or Jsm.Opcode.FLDSND1 or Jsm.Opcode.FLDSND2 or Jsm.Opcode.FLDSND3 &&
            instruction.Operands[0].Expression == null)
        {
            int command = instruction.Operands[0].Value;
            if (SourceInstruction.SoundCommands.TryGetValue(command & ~0x40, out string? soundCommand))
            {
                if (index == 0)
                    return (command & 0x40) == 0 ? $"SoundCommands.{soundCommand}" : $"SoundCommands.{soundCommand}Slot1";
                if (index == 1 && EventAnnotations.ResolveAudio(instruction) is { } audio)
                    return ResourceCatalog.Qualify(audio.Name, value);
            }
        }
        return FormatOperand(operand, storage);
    }

    private static string CallWithTail(string start, IEnumerable<string> arguments) =>
        start + string.Concat(arguments.Select(argument => ", " + argument)) + ")";

    private static string FormatOperand(EVOperand operand, StorageDeclarations storage) =>
        operand.Expression == null ? operand.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : FormatValue(operand.Expression, storage);

    private static string FormatValue(EVExpressionToken[] tokens, StorageDeclarations storage) =>
        SourceExpression.TryFormat(tokens, storage, statement: false, out string? value) ? value : $"Expression({FormatExpression(tokens)})";

    private static string FormatExpression(IEnumerable<EVExpressionToken> tokens) =>
        string.Join(", ", tokens.Select(FormatToken));

    private static string FormatToken(EVExpressionToken token)
    {
        if (token.Opcode == (byte)Jsm.Excode.IsKeyPressed)
            return "IsKeyPressed()";
        if (token.Opcode == (byte)Jsm.Excode.Const16)
            return $"Literal({token.Value})";
        if (token.Opcode == (byte)Jsm.Excode.Const26)
        {
            int highBits = (int)((uint)token.Value >> 26);
            string preservedBits = highBits == 0 ? "" : $", highBits: {highBits}";
            return $"Literal({token.Value << 6 >> 6}, wide: true{preservedBits})";
        }
        if (token.Opcode >= 0x80 && token.Opcode != 0xD3)
        {
            EVVariable variable = EVVariable.Decode(token);
            string wide = (token.Opcode & 0x20) == 0 ? "" : ", wide: true";
            string prefix = (token.Opcode & 0x40) != 0 ? "" : ", alternatePrefix: true";
            return $"Variable({variable.Scope}[StorageType.{variable.Type}, {variable.Address}]{wide}{prefix})";
        }

        var excode = (Jsm.Excode)token.Opcode;
        string opcode = Enum.IsDefined(excode) ? $"Excode.{excode}" : $"(Excode)0x{token.Opcode:X2}";
        return token.Value == 0 ? $"Op({opcode})" : $"Op({opcode}, 0x{token.Value:X})";
    }

    internal static string Identifier(string value)
    {
        string name = string.Concat(value.Select(character => SyntaxFacts.IsIdentifierPartCharacter(character) ? character : '_'));
        if (!SyntaxFacts.IsValidIdentifier(name) || SyntaxFacts.GetKeywordKind(name) != SyntaxKind.None)
            name = "_" + name;
        return name;
    }
}
