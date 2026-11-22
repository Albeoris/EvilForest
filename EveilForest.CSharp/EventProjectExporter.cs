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
    public static void Export(EVContainer container, string eventName, string directory, bool simplify = false, bool structured = true, string? dependency = null, bool useCutsceneNames = true, CutsceneCatalog? cutsceneCatalog = null, bool extractCutscenes = true, bool allowNativePhaseDispatch = false)
    {
        ExportDirectory.Publish(directory, stage => WriteProject(container, eventName, stage, simplify, structured, dependency, useCutsceneNames, cutsceneCatalog, extractCutscenes, allowNativePhaseDispatch));
    }

    public static int ExportAll(string fixtures, string directory, bool useCutsceneNames = true, CutsceneCatalog? cutsceneCatalog = null, bool allowNativePhaseDispatch = false)
    {
        const string extension = ".eb.bytes";
        string[] files = Directory.EnumerateFiles(fixtures, "*" + extension).OrderBy(path => path, StringComparer.OrdinalIgnoreCase).ToArray();
        ExportDirectory.Publish(directory, stage =>
        {
            string dependency = CopyDependencyTo(Path.Combine(stage, "lib"));
            foreach (string file in files)
            {
                string name = Path.GetFileName(file)[..^extension.Length];
                string project = Path.Combine(stage, name);
                try
                {
                    using var stream = File.OpenRead(file);
                    EVContainer container = EVContainer.Read(stream);
                    Directory.CreateDirectory(project);
                    WriteProject(container, name, project, simplify: false, structured: true, dependency, useCutsceneNames, cutsceneCatalog, extractCutscenes: true, allowNativePhaseDispatch);
                }
                catch (Exception error) when (error is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException)
                {
                    throw new InvalidDataException($"{Path.GetFileName(file)}: {error.Message}", error);
                }
            }
        });
        return files.Length;
    }

    public static void DiagnoseCutsceneMemory(EVContainer container, string eventName, string reportDirectory)
    {
        string directory = Directory.CreateDirectory(reportDirectory).FullName;
        var report = new List<string> { "slot\ttag\tscope\towner\tbyte\tread mask\twrite mask\tunresolved mask\tphase\tentry offset" };
        foreach (EVContainerObject participant in container.Objects)
        {
            foreach (EVContainerScript script in participant.Scripts)
            {
                EVInstructionBody body = EVInstructionBody.Decode(script.Body);
                report.AddRange(CutsceneMemoryIndex.Describe(participant.Id, script.Id, body).Select(row => $"{row}\t\t"));
                foreach (var phase in CutscenePhaseGraph.Read(new(participant.Id, script.Id), participant.Type, body))
                {
                    var fragment = new EVInstructionBody(phase.Instructions, body.EndLabel);
                    report.AddRange(CutsceneMemoryIndex.Describe(participant.Id, script.Id, fragment).Select(row => $"{row}\t{phase.Value}\t0x{phase.Start:X}"));
                }
            }
        }
        File.WriteAllLines(Path.Combine(directory, Identifier(eventName) + ".CutsceneMemory.tsv"), report);
    }

    private static void WriteProject(EVContainer container, string eventName, string directory, bool simplify, bool structured, string? dependency, bool useCutsceneNames, CutsceneCatalog? cutsceneCatalog, bool extractCutscenes, bool allowNativePhaseDispatch)
    {
        string eventNamespace = "Event_" + Identifier(eventName);
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
        EventSceneSymbols.SceneNames? names = EventSceneSymbols.FindNames(eventName);
        var objects = new List<(EVContainerObject Definition, EVInstructionBody[] Bodies, StorageDeclarations Storage, string Name)>();
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

            string typeName = names?.Participants.GetValueOrDefault(obj.Id) ??
                (useCutsceneNames && structured && !simplify ? CutsceneCatalog.ParticipantName(eventName, obj.Id) : null) ?? NameObject(obj.Id, bodies);
            objects.Add((obj, bodies, objectStorage, typeName));
        }

        EventSceneSymbols? scene = eventName.StartsWith("EVT_", StringComparison.OrdinalIgnoreCase)
            ? new EventSceneSymbols(eventName, objects.Select(item => (item.Definition, item.Name, item.Bodies)), useCutsceneNames && structured && !simplify) : null;
        CutsceneExport? cutscenes = extractCutscenes && scene != null && structured && !simplify
            ? new CutsceneExport(eventName, scene, objects.Select(item => (item.Definition, item.Bodies)), useCutsceneNames, cutsceneCatalog, allowNativePhaseDispatch)
            : null;
        cutscenes?.NameDirectorStorage(eventStorage);
        var directors = cutscenes?.Scenes.Where(item => item.DirectorMachine is { Standalone: true }).ToDictionary(item => item.DirectorMachine!.Director.Slot) ?? new();
        var composedHandlers = new Dictionary<string, StringBuilder>();
        foreach (var (definition, bodies, storage, name) in objects)
        {
            storage.Scene = scene;
            storage.CutsceneNames = eventStorage.CutsceneNames;
            string source = FormatObject(definition, bodies, eventNamespace, name, simplify, structured, annotations, storage, scene, composedHandlers, cutscenes, directors);
            if (!directors.ContainsKey(definition.Id))
                File.WriteAllText(Path.Combine(directory, $"{definition.Id:D2}_{name}.cs"), source);
        }
        cutscenes?.FinalizeParallelPhases(directory);
        cutscenes?.FinalizeOwnedActions();
        cutscenes?.ValidateDirectorMachines();
        cutscenes?.SimplifyChoreographyJoins();

        var eventSource = new StringBuilder();
        eventSource.AppendLine("using EvilForest.EventSource;");
        eventSource.AppendLine();
        eventSource.AppendLine($"[assembly: EventFormat(version: {container.Header[2]})]");
        File.WriteAllBytes(Path.Combine(directory, "Event.Header.bin"), container.Header[EVContainer.HeaderMetadataOffset..]);
        if (container.Trailer.Length != 0)
            File.WriteAllBytes(Path.Combine(directory, "Event.Trailer.bin"), container.Trailer);
        if (scene != null)
            eventSource.AppendLine($"[assembly: EventScene(typeof({eventNamespace}.SceneContext))]");
        foreach (var (firstBit, bitCount) in EngineStorageReservations.ForEvent(eventName))
            eventSource.AppendLine($"[assembly: MapStorageReservation({firstBit}, {bitCount})]");
        eventSource.AppendLine();
        eventSource.AppendLine($"namespace {eventNamespace};");
        eventSource.AppendLine();
        eventStorage.Append(eventSource, EVStorageScope.Map, "internal", instance: scene != null, emitEmpty: scene != null);
        File.WriteAllText(Path.Combine(directory, "Event.cs"), eventSource.ToString());
        if (scene != null)
        {
            string[] cutsceneGroups = cutscenes?.Scenes.Where(item => item.Methods.Count > 0).Select(item => item.Name).Distinct(StringComparer.Ordinal).ToArray() ?? [];
            var sceneSource = new StringBuilder();
            AppendSourceUsings(sceneSource, eventNamespace);
            sceneSource.AppendLine("internal sealed class SceneContext : EventSceneContext");
            sceneSource.AppendLine("{");
            if (cutsceneGroups.Length == 0)
                sceneSource.AppendLine("    public SceneContext() => Scene = new Scene(this);");
            else
            {
                sceneSource.AppendLine("    public SceneContext()");
                sceneSource.AppendLine("    {");
                sceneSource.AppendLine("        Scene = new Scene(this);");
                foreach (string name in cutsceneGroups)
                    sceneSource.AppendLine($"        {name} = new {name}(this);");
                sceneSource.AppendLine("    }");
            }
            sceneSource.AppendLine("    internal Scene Scene { get; }");
            sceneSource.AppendLine("    internal Location Location { get; } = new();");
            foreach (string name in cutsceneGroups)
                sceneSource.AppendLine($"    internal {name} {name} {{ get; }}");
            sceneSource.AppendLine("}");
            sceneSource.AppendLine();
            sceneSource.AppendLine("[SceneRoot]");
            sceneSource.AppendLine(scene.Participants.Count == 0 ? "internal sealed class Scene(SceneContext _)" : "internal sealed class Scene(SceneContext context)");
            sceneSource.AppendLine("{");
            foreach (var (slot, participant) in scene.Participants)
            {
                if (!directors.ContainsKey(slot))
                    sceneSource.AppendLine($"    internal {participant.Name} {participant.Name} {{ get; }} = new {participant.Name}(context);");
            }
            if (names != null && scene.Participants.GetValueOrDefault(names.ControlledDefinition) is { } controlled)
                sceneSource.AppendLine($"    internal {controlled.Name} ControlledActor {{ get; }} = EventSceneContext.BindControlled(new {controlled.Name}(context));");
            else
                sceneSource.AppendLine("    internal IActor ControlledActor { get; } = new ControlledActorTarget();");
            sceneSource.AppendLine("}");
            foreach (var (owner, handlers) in composedHandlers)
            {
                sceneSource.AppendLine();
                sceneSource.AppendLine($"public sealed partial class {owner}");
                sceneSource.AppendLine("{");
                sceneSource.Append(handlers);
                sceneSource.AppendLine("}");
            }
            File.WriteAllText(Path.Combine(directory, names == null ? "Scene.cs" : "Scene_Interactions.cs"), sceneSource.ToString());
            if (cutscenes != null)
            {
                var sources = objects.Where(item => !directors.ContainsKey(item.Definition.Id)).ToDictionary(item => item.Definition.Id, item => $"{item.Definition.Id:D2}_{item.Name}.cs");
                cutscenes.FinalizeParticipantSources(directory, sources);
                foreach (var (slot, director) in directors)
                    sources.Add(slot, $"Cutscenes/{director.Name}.cs");
                WriteCutscenes(cutscenes, eventNamespace, directory, sources);
            }
        }
        if (structured)
        {
            EmptyJumpLayout.AnnotateExport(directory);
            SharedCaseActionLayout.RewriteExport(directory);
            CaseContinuationExport.RewriteExport(directory);
        }
    }

    private static void WriteCutscenes(CutsceneExport cutscenes, string eventNamespace, string directory, IReadOnlyDictionary<int, string> sources)
    {
        if (cutscenes.Outline.Count > 0 || cutscenes.Scenes.Count > 0)
        {
            var lines = new List<string> { "slot\ttag\tcandidate\tblocks\tstatus\tsource\tscene source\tfragment key\trule" };
            foreach (var scene in cutscenes.Scenes)
            {
                var chapters = scene.DirectorMachine?.OrderedChapters ?? scene.AdditionalChapters.Keys.Prepend(scene.Entry)
                    .SelectMany(entry => scene.For(entry)!.Keys.Select(phase => (Entry: entry, Phase: phase))).ToArray();
                foreach (var (entry, phase) in chapters)
                {
                    bool extracted = scene.ExtractedChapters.Contains((entry, phase));
                    string status = extracted ? "extracted" : "Kept in handler to preserve its original exit";
                    string sceneSource = extracted ? $"Cutscenes/{scene.Name}.cs" : "";
                    string? key = scene.FragmentKeys.GetValueOrDefault((entry, phase));
                    string kind = scene.Whole ? "entry" : $"phase {phase}";
                    string method = scene.ChapterMethod(entry, phase);
                    lines.Add($"{entry.Slot}\t{entry.Tag}\t{scene.Name}.{method} ({kind})\t\t{status}\t{sources[entry.Slot]}\t{sceneSource}\t{key}\t{scene.Rule}");
                }
            }
            foreach (var scene in cutscenes.Scenes)
            {
                foreach (var (entry, name) in scene.Actions)
                    lines.Add($"{entry.Slot}\t{entry.Tag}\t{scene.Name}.{name} ({cutscenes.ActionKind(entry)} entry)\t\textracted\t{sources[entry.Slot]}\tCutscenes/{scene.Name}.cs\t\t{scene.Rule}");
            }
            foreach (string row in cutscenes.Outline)
            {
                string? source = int.TryParse(row.AsSpan(0, row.IndexOf('\t')), out int slot) ? sources.GetValueOrDefault(slot) : null;
                lines.Add($"{row}\t{source}\t\t\t");
            }
            File.WriteAllLines(Path.Combine(directory, "CutsceneOutline.tsv"), lines);
        }
        var emitted = cutscenes.Scenes.Where(scene => scene.Methods.Count > 0).ToArray();
        if (emitted.Length == 0)
            return;
        string path = Directory.CreateDirectory(Path.Combine(directory, "Cutscenes")).FullName;
        foreach (var group in emitted.GroupBy(scene => scene.Name))
        {
            var source = new StringBuilder();
            AppendSourceUsings(source, eventNamespace);
            source.AppendLine($"internal sealed partial class {group.Key}(SceneContext ctx) : EventCutscene");
            source.AppendLine("{");
            source.AppendLine("    private Location Location => ctx.Location;");
            source.AppendLine("    private GlobalState Globals => ctx.Globals;");
            foreach (var binding in group.SelectMany(scene => scene.Bindings))
                source.Append(CutsceneExport.Render(binding));
            foreach (var method in group.SelectMany(scene => scene.Methods))
                source.Append(CutsceneExport.Render(method));
            source.AppendLine("}");
            File.WriteAllText(Path.Combine(path, group.Key + ".cs"), source.ToString());
        }
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
        var animationIds = new HashSet<int>();
        var soundIds = new HashSet<int>();
        foreach (string path in Directory.EnumerateFiles(fixtures, "*.eb.bytes", SearchOption.AllDirectories))
        {
            using var input = File.OpenRead(path);
            foreach (EVContainerObject obj in EVContainer.Read(input).Objects)
            {
                foreach (EVContainerScript script in obj.Scripts)
                {
                    EVInstructionBody body = EVInstructionBody.Decode(script.Body);
                    storage.Add(body);
                    foreach (EVInstruction effect in body.Instructions.Where(instruction => instruction.Opcode == Jsm.Opcode.CHRFX))
                    {
                        if (effect.Operands[0] is not { Expression: null, Value: >= 16 and <= 19 })
                            continue;
                        if (effect.Operands[1] is { Expression: null, Value: >= 0 and <= ushort.MaxValue } animation)
                            animationIds.Add(animation.Value);
                        if (effect.Operands[0].Value is 16 or 17 && effect.Operands[3] is { Expression: null, Value: >= 0 and <= ushort.MaxValue } sound)
                            soundIds.Add(sound.Value);
                    }
                }
            }
        }
        var globals = new StringBuilder("namespace EvilForest.EventSource;");
        globals.AppendLine().AppendLine();
        storage.Append(globals, EVStorageScope.Global, "public");
        storage.Append(globals, EVStorageScope.Special, "public");
        storage.Append(globals, EVStorageScope.Global, "public", instance: true, className: "GlobalState", emitEmpty: true);
        File.WriteAllText(Path.Combine(directory, "Globals.Generated.cs"), globals.ToString());

        var characterResources = new StringBuilder("namespace EvilForest.EventSource;");
        characterResources.AppendLine().AppendLine();
        ResourceCatalog.AppendCharacterResourceIds(characterResources, animationIds, soundIds);
        File.WriteAllText(Path.Combine(directory, "CharacterResources.Generated.cs"), characterResources.ToString());
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

    private static void AppendSourceUsings(StringBuilder source, string eventNamespace)
    {
        source.AppendLine("using EvilForest.EventSource;");
        source.AppendLine("using static EvilForest.EventSource.Vm;");
        source.AppendLine("using static EvilForest.EventSource.Values;");
        source.AppendLine();
        source.AppendLine($"namespace {eventNamespace};");
        source.AppendLine();
    }

    private static string FormatObject(EVContainerObject obj, EVInstructionBody[] bodies, string eventNamespace, string typeName, bool simplify, bool structured, EventAnnotations annotations, StorageDeclarations storage, EventSceneSymbols? scene, Dictionary<string, StringBuilder> composedHandlers, CutsceneExport? cutscenes, IReadOnlyDictionary<int, CutsceneExport.Scene> directors)
    {
        var source = new StringBuilder();
        AppendSourceUsings(source, eventNamespace);
        source.AppendLine($"[ObjectSlot({obj.Id}, {obj.VariableCount}, {obj.Flags}, {obj.Reserved}, {obj.BodyOrder ?? -1}, {obj.EmptyOffset}, {obj.Type}, \"{Convert.ToHexString(obj.Gap)}\", \"{Convert.ToHexString(obj.Prefix)}\")]");
        string parent = obj.Type switch { 1 => "EventRegion", 2 => "EventActor", _ => "EventThread" };
        source.AppendLine(scene == null ? $"public static partial class {typeName}" : $"public sealed partial class {typeName} : {parent}");
        source.AppendLine("{");
        if (scene != null)
        {
            source.AppendLine($"    internal {typeName}(SceneContext context) : base(context)");
            source.AppendLine("    {");
            source.AppendLine("    }");
            source.AppendLine();
            source.AppendLine("    private SceneContext ctx => (SceneContext)Context;");
            source.AppendLine("    private Location Location => ctx.Location;");
            source.AppendLine("    private GlobalState Globals => ctx.Globals;");
            source.AppendLine($"    {(cutscenes?.OwnsLocalStorage(obj.Id) == true ? "internal" : "private")} LocalMemory Local {{ get; }} = new();");
            source.AppendLine();
        }
        storage.Append(source, EVStorageScope.Instance, cutscenes?.OwnsLocalStorage(obj.Id) == true ? "internal" : "private", "    ", instance: scene != null, className: scene == null ? null : "LocalMemory", emitEmpty: scene != null);
        IReadOnlyList<string> methodNames = scene?.Participants.GetValueOrDefault(obj.Id)?.Handlers ?? EventSceneSymbols.HandlerNames(obj, scene?.Names);
        var duplicateTags = obj.Scripts.GroupBy(script => script.Id).Where(group => group.Count() > 1).Select(group => group.Key).ToHashSet();
        for (int index = 0; index < obj.Scripts.Length; index++)
        {
            ushort id = obj.Scripts[index].Id;
            string method = methodNames[index];

            StringBuilder handlerSource = source;
            if (scene?.Names?.ComposedHandlers.Contains((obj.Id, id)) == true)
            {
                if (!composedHandlers.TryGetValue(typeName, out handlerSource!))
                    composedHandlers.Add(typeName, handlerSource = new StringBuilder());
                if (handlerSource.Length > 0)
                    handlerSource.AppendLine();
            }
            else if (index > 0)
            {
                handlerSource.AppendLine();
            }
            StringBuilder destination = handlerSource;
            if (cutscenes?.NeedsHandlerSource(obj.Id) == true)
                handlerSource = new StringBuilder();
            EVInstructionBody body = bodies[index];
            int padding = TrailingPadding(body);
            int instructionCount = body.Instructions.Count - padding;
            bool suspends = body.Instructions.Take(instructionCount).Any(instruction => SourceInstruction.SuspensionOf(instruction.Opcode) != Suspension.Immediate);
            handlerSource.AppendLine($"    [Script({id}, {index})]");
            if (duplicateTags.Contains(id))
                handlerSource.AppendLine($"    [OriginalDuplicateTag({id}, {index})]");
            if (padding != 0)
                handlerSource.AppendLine($"    [TrailingPadding({padding})]");
            IReadOnlySet<int>? unreachablePollingExitLabels = cutscenes?.UnreachablePollingExitLabels(obj.Id, id);
            IReadOnlySet<int>? linkedPollingDispatchLabels = cutscenes?.LinkedPollingDispatchLabels(obj.Id, id);
            EventControlFlowFormatter CreateFormatter(bool useNativeJumps = false, bool expandSwitchRegions = true) =>
                new(body, instructionCount, instruction => FormatInstruction(instruction, storage, scene, obj.Type == 2, directors), tokens => FormatValue(tokens, storage),
                    operand => FormatOperand(operand, storage), tokens => SourceExpression.TryFormatCondition(tokens, storage, out string? condition) ? condition : null,
                    tokens => SourceExpression.FormatSwitchSelector(tokens, storage) ?? $"(int)({FormatValue(tokens, storage)})", simplify, annotations.For)
                {
                    UseNativeJumps = useNativeJumps,
                    ExpandSwitchRegions = expandSwitchRegions,
                    PreserveSwitchPhase = cutscenes == null ? null : (selector, value) => cutscenes.OwnsSwitchPhase(obj.Id, id, selector, value),
                    UnreachablePollingExitLabels = unreachablePollingExitLabels,
                    LinkedPollingDispatchLabels = linkedPollingDispatchLabels
                };

            var formatter = CreateFormatter();
            var statements = new StringBuilder();
            formatter.AppendTo(statements, structured);
            if (structured && formatter.HasUnstructuredTargets && formatter.CanWriteNativeJumps())
            {
                var native = CreateFormatter(useNativeJumps: true);
                var nativeStatements = new StringBuilder();
                native.AppendTo(nativeStatements, structured: true);
                bool hasVisibleNativeJumps = CanUseNative(native) && HasVisibleJumpTargets(nativeStatements.ToString());
                if (native.HasExpandedSwitchRegions && (!hasVisibleNativeJumps || native.NativeGotoCount != 0))
                {
                    var conservative = CreateFormatter(useNativeJumps: true, expandSwitchRegions: false);
                    var conservativeStatements = new StringBuilder();
                    conservative.AppendTo(conservativeStatements, structured: true);
                    if (CanUseNative(conservative) && (!hasVisibleNativeJumps || conservative.NativeGotoCount < native.NativeGotoCount) &&
                        HasVisibleJumpTargets(conservativeStatements.ToString()))
                    {
                        native = conservative;
                        nativeStatements = conservativeStatements;
                        hasVisibleNativeJumps = true;
                    }
                }

                if (!hasVisibleNativeJumps)
                {
                    native = CreateFormatter(useNativeJumps: true, expandSwitchRegions: false);
                    nativeStatements.Clear();
                    native.AppendTo(nativeStatements, structured: false);
                    hasVisibleNativeJumps = CanUseNative(native);
                }

                if (hasVisibleNativeJumps)
                {
                    formatter = native;
                    statements = nativeStatements;
                }
            }
            foreach (string layout in formatter.SwitchLayouts)
                handlerSource.AppendLine(layout);
            string modifier = scene == null ? "static " : "";
            handlerSource.AppendLine(suspends ? $"    public {modifier}async EventRoutine {method}()" : $"    public {modifier}void {method}()");
            handlerSource.AppendLine("    {");
            if (scene != null)
            {
                string bodySource = statements.ToString();
                if (bodySource.Contains("evt.", StringComparison.Ordinal))
                    handlerSource.AppendLine("        var evt = ctx.Scene;");
                if (bodySource.Contains("scripts.", StringComparison.Ordinal))
                    handlerSource.AppendLine("        var scripts = ctx.Scripts;");
            }
            int statementOffset = handlerSource.Length;
            handlerSource.Append(statements);
            handlerSource.AppendLine("    }");
            if (handlerSource != destination)
                destination.Append(cutscenes!.Rewrite(obj.Id, id, index, typeName, handlerSource.ToString(), storage, formatter.LabelName, statementOffset, formatter.InstructionOffsets));
        }
        source.AppendLine("}");
        return source.ToString();
    }

    private static bool CanUseNative(EventControlFlowFormatter formatter) => !formatter.HasRawTransfer;

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

        foreach (GotoStatementSyntax jump in body.DescendantNodes().OfType<GotoStatementSyntax>())
        {
            if (jump.Kind() == SyntaxKind.GotoStatement)
            {
                if (jump.Expression is not IdentifierNameSyntax target || !scopes.TryGetValue(target.Identifier.ValueText, out var scope) ||
                    !jump.Ancestors().Contains(scope))
                    return false;
                continue;
            }

            SwitchStatementSyntax? selection = jump.Ancestors().OfType<SwitchStatementSyntax>().FirstOrDefault();
            if (selection == null || !selection.Sections.SelectMany(section => section.Labels).Any(label => label switch
                {
                    CaseSwitchLabelSyntax entry => jump.Kind() == SyntaxKind.GotoCaseStatement && entry.Value.IsEquivalentTo(jump.Expression),
                    DefaultSwitchLabelSyntax => jump.Kind() == SyntaxKind.GotoDefaultStatement,
                    _ => false
                }))
                return false;
        }
        return true;
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

    private static string FormatInstruction(EVInstruction instruction, StorageDeclarations storage, EventSceneSymbols? scene, bool actingActor, IReadOnlyDictionary<int, CutsceneExport.Scene> directors)
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

        string animationReceiver = scene != null && actingActor ? "Animation" : "CharacterEffects.ActingAnimation";
        if (CharacterEffectSource.Format(instruction, animationReceiver, operand => FormatOperand(operand, storage)) is { } effect)
            return effect;

        if (instruction.Opcode == Jsm.Opcode.NEW && instruction.Operands is [{ Expression: null } definition, { Expression: null, Value: 0 }] &&
            directors.TryGetValue(definition.Value, out var director))
            return $"scripts.StartDirector(ctx.{director.Name}.Direct)";

        if (scene != null && instruction.Opcode is Jsm.Opcode.NEW or Jsm.Opcode.NEW2 or Jsm.Opcode.NEW3 &&
            instruction.Operands.All(operand => operand.Expression == null) &&
            scene.Participants.TryGetValue(instruction.Operands[0].Value, out var participant) &&
            participant.Kind == (int)instruction.Opcode - (int)Jsm.Opcode.NEW)
        {
            int uid = instruction.Operands[1].Value;
            string explicitUid = uid == 0 ? "" : $", new InstanceUid({uid})";
            return $"scripts.{operation.Method}(evt.{participant.Name}{explicitUid})";
        }
        if (scene != null && instruction.Opcode == Jsm.Opcode.STARTSEQ &&
            instruction.Operands[0] is { Expression: null } entry && scene.Participants.TryGetValue(entry.Value, out var sequence))
        {
            string explicitMask = instruction.ArgumentMask == SourceInstruction.InferMask(instruction.Opcode, instruction.Operands) ? "" : $", mask: {mask}";
            return $"scripts.{operation.Method}(evt.{sequence.Name}{explicitMask})";
        }
        if (scene != null && instruction.Opcode is Jsm.Opcode.REQ or Jsm.Opcode.REQSW or Jsm.Opcode.REQEW &&
            instruction.Operands[1].Expression == null && instruction.Operands[2].Expression == null)
        {
            int uid = instruction.Operands[1].Value;
            int tag = instruction.Operands[2].Value;
            var instance = scene.InstanceHandler(uid, tag);
            string? handler = instance?.Method ?? scene.HandlerReceiver(uid, tag);
            if (handler != null)
            {
                string awaitPrefix = instruction.Opcode == Jsm.Opcode.REQ ? "" : "await ";
                string level = FormatOperand(instruction.Operands[0], storage);
                if (instruction.Operands[0].Expression != null)
                    level = $"(int)({level})";
                string instanceArgument = instance.HasValue ? $", instance: new InstanceUid({uid})" : "";
                string explicitMask = instruction.ArgumentMask == SourceInstruction.InferMask(instruction.Opcode, instruction.Operands) ? "" : $", mask: {mask}";
                return $"{awaitPrefix}scripts.{operation.Method}({handler}, level: {level}{instanceArgument}{explicitMask})";
            }
        }

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
        string receiver = scene != null && actingActor && operation.UsesActingAnimation ? "Animation" :
            scene != null && actingActor && operation.UsesActingActor ? "Actions" : operation.Service;
        string method = instruction.Opcode == Jsm.Opcode.FICON && instruction.Operands[0] is { Expression: null } icon &&
            Enum.IsDefined((global::EvilForest.EventSource.BubbleSymbol)icon.Value) ? "Bubble" : operation.Method;
        return $"{awaitKeyword}{receiver}.{method}({string.Join(", ", arguments)})";
    }

    private static string FormatServiceOperand(EVInstruction instruction, int index, string name, StorageDeclarations storage)
    {
        EVOperand operand = instruction.Operands[index];
        if (operand.Expression != null)
            return FormatOperand(operand, storage);

        int value = operand.Value;
        if (instruction.Opcode == Jsm.Opcode.FICON && Enum.IsDefined((global::EvilForest.EventSource.BubbleSymbol)value))
            return $"BubbleSymbol.{(global::EvilForest.EventSource.BubbleSymbol)value}";
        if (instruction.Opcode == Jsm.Opcode.BGIMASK)
        {
            if (value == 127)
                return "TriangleAttributeMask.All & ~TriangleAttributeMask.RestrictPlayer";
            if (Enum.IsDefined((global::EvilForest.EventSource.TriangleAttributeMask)value))
                return $"TriangleAttributeMask.{(global::EvilForest.EventSource.TriangleAttributeMask)value}";
        }
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
