using System.Text;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    internal readonly record struct Entry(int Slot, ushort Tag);
    private sealed record Handler(Entry Entry, EVContainerObject Owner, EVInstructionBody Body, int Ordinal);
    private sealed record Request(Entry Caller, int? Receiver, int Tag, Jsm.Opcode Mode, int? Level, int Position, int Ordinal);
    internal sealed record Scene(string DefaultName, Entry Entry, bool Whole, Dictionary<int, string> Chapters)
    {
        public string Name { get; set; } = DefaultName;
        public string Rule { get; init; } = "";
        public Dictionary<Entry, string> Actions { get; } = new();
        public List<MethodDeclarationSyntax> Methods { get; } = new();
        public List<MemberDeclarationSyntax> Bindings { get; } = new();
        public Dictionary<Entry, Dictionary<int, string>> AdditionalChapters { get; } = new();
        public Dictionary<Entry, EVExpressionToken> Selectors { get; } = new();
        public Dictionary<(Entry Entry, int Phase), string> FragmentKeys { get; } = new();
        public Dictionary<(Entry Entry, int Phase), int> GuardedChapters { get; } = new();
        public HashSet<(Entry Entry, int Phase)> ExtractedChapters { get; } = new();
        public Dictionary<(Entry Entry, int Phase), string> SharedChapters { get; } = new();
        public (Entry Entry, int Phase)[]? ParallelTracks { get; set; }
        public string? ParallelName { get; set; }
        public EVExpressionToken[]? StoryGuard { get; init; }
        public OptionalIntroduction? Introduction { get; init; }
        public RegionEpisode? Region { get; init; }
        public TalkEpisode? Talk { get; init; }
        public DirectorPhaseMachine? DirectorMachine { get; set; }
        public Dictionary<int, string>? For(Entry entry) => entry == Entry ? Chapters : AdditionalChapters.GetValueOrDefault(entry);
        public string ChapterMethod(Entry entry, int phase) => SharedChapters.GetValueOrDefault((entry, phase)) ?? For(entry)![phase];
    }

    private void AddPhaseScenes(Handler[] handlers, IReadOnlyDictionary<Entry, int> counts, List<Scene> scenes, List<string> outline)
    {
        var occupied = scenes.SelectMany(scene => scene.AdditionalChapters.Keys.Prepend(scene.Entry)
            .SelectMany(entry => scene.For(entry)!.Keys.Select(value => (Entry: entry, Value: value)))).ToHashSet();
        var phases = handlers.Where(handler => counts[handler.Entry] == 1)
            .SelectMany(handler => CutscenePhaseGraph.Read(handler.Entry, handler.Owner.Type, handler.Body)).ToArray();
        var inventory = handlers.SelectMany(handler => CutscenePhaseGraph.Read(handler.Entry, handler.Owner.Type, handler.Body, includeSharedArms: true))
            .ToLookup(phase => phase.Register);
        var bodies = handlers.Where(handler => counts[handler.Entry] == 1).ToDictionary(handler => handler.Entry, handler => handler.Body);
        foreach (var dispatch in phases.GroupBy(phase => phase.Register))
        {
            CutscenePhaseGraph.Phase[] arms = dispatch.GroupBy(phase => (phase.Entry, phase.Value)).Where(group => group.Count() == 1)
                .Select(group => group.Single()).ToArray();
            foreach (var job in arms.Where(phase => phase.OwnerKind == 2 && phase.Chooses && phase.Speaks && phase.Has(Jsm.Opcode.REQEW)))
            {
                var next = After(job).Where(phase => phase.Entry == job.Entry && phase.Has(Jsm.Opcode.REQSW) && phase.Has(Jsm.Opcode.MESN)).ToArray();
                if (next is [var dispatchJobs])
                    Add("choice and dispatch", job, dispatchJobs);
            }
            foreach (var call in arms.Where(phase => phase.OwnerKind == 2 && phase.Has(Jsm.Opcode.MESN)))
            {
                var responses = After(call).Where(phase => phase.OwnerKind == 2 && phase.Entry != call.Entry && phase.Has(Jsm.Opcode.BGSSCROLL) && phase.Has(Jsm.Opcode.TURNA)).ToArray();
                if (responses is [var response] && After(response).Any(phase => phase.Entry == call.Entry && phase.Has(Jsm.Opcode.WAITMES)))
                    Add("asynchronous window handoff", call, response);
            }
            foreach (var menu in arms.Where(phase => phase.OwnerKind == 2 && phase.Has(Jsm.Opcode.MENU)))
            {
                var partners = arms.Where(phase => phase.OwnerKind == 2 && phase.Entry != menu.Entry && phase.Value == menu.Value && phase.Has(Jsm.Opcode.MES)).ToArray();
                var coordinators = arms.Where(phase => phase.OwnerKind == 0 && phase.Value == menu.Value && phase.Successors.Any() &&
                    menu.Writes().Any(signal => signal.Value == 1 && phase.Reads().Any(cell => cell.Overlaps(signal.Cell)) &&
                        phase.Writes().Any(clear => clear.Cell.Overlaps(signal.Cell) && clear.Value == 0)));
                foreach (var coordinator in coordinators)
                {
                    var reply = After(coordinator).Where(phase => phase.Entry == menu.Entry && phase.Speaks).ToArray();
                    var preceding = arms.Where(phase => phase.Entry == coordinator.Entry && phase.Successors.Contains(menu.Value)).ToArray();
                    if (partners is not [var partner] || reply is not [var answer] || preceding is not [var precedingPhase])
                        continue;
                    var setup = arms.Where(phase => phase.Entry == menu.Entry && phase.Value == precedingPhase.Value && phase.Has(Jsm.Opcode.ANIM)).ToArray();
                    if (setup is not [var preparation])
                        continue;
                    var beforeSetup = arms.Where(phase => phase.Entry == coordinator.Entry && phase.Successors.Contains(preparation.Value)).Select(phase => phase.Value).ToHashSet();
                    var entrance = arms.Where(phase => phase.Entry == menu.Entry && beforeSetup.Contains(phase.Value) && phase.Speaks).ToArray();
                    if (entrance.Length <= 1 && Add("shared completion", entrance.FirstOrDefault() ?? preparation, [preparation, menu, partner, answer]) is { } scene)
                        scene.ParallelTracks = [(menu.Entry, menu.Value), (partner.Entry, partner.Value)];
                    outline.Add($"{coordinator.Entry.Slot}\t{coordinator.Entry.Tag}\tShared phase coordinator\t\tPhase {coordinator.Value} polls a signal from slot {menu.Entry.Slot}/{menu.Entry.Tag}");
                    break;
                }
            }

            if (dispatch.Key.Scope == EVStorageScope.Map)
                AddDirectorMachines(handlers, scenes, occupied, inventory[dispatch.Key].ToArray());

            var available = arms.Where(phase => phase.OwnerKind == 2 && !occupied.Contains((phase.Entry, phase.Value))).ToArray();
            foreach (var exchange in CutscenePhaseGraph.ActorExchanges(available))
                Add("actor phase handoff", exchange[0], exchange);

            foreach (var external in arms.Where(phase => phase.Has(Jsm.Opcode.MAPJUMP) && (phase.Speaks || phase.Has(Jsm.Opcode.FMV))))
                Add("external transfer", external);

            foreach (var directed in arms.Where(phase => phase.OwnerKind == 0 && phase.Speaks && phase.Has(Jsm.Opcode.REQEW, Jsm.Opcode.REQSW)))
            {
                if (!occupied.Contains((directed.Entry, directed.Value)) && HasDirectedDialogue(directed, bodies[directed.Entry]))
                    Add("directed dialogue phase", directed);
            }

            IEnumerable<CutscenePhaseGraph.Phase> After(CutscenePhaseGraph.Phase phase) =>
                phase.Successors.SelectMany(value => arms.Where(candidate => candidate.Value == value));
        }

        foreach (Handler handler in handlers)
        {
            if (handler.Body.Instructions.Any(instruction => instruction.Opcode == Jsm.Opcode.FMV) &&
                handler.Body.Instructions.Any(instruction => instruction.Opcode == Jsm.Opcode.MAPJUMP))
                outline.Add($"{handler.Entry.Slot}\t{handler.Entry.Tag}\tMovie and map transfer\t\tThe field continues after the FMV synchronization read");
        }

        Scene? Add(string rule, CutscenePhaseGraph.Phase first, params CutscenePhaseGraph.Phase[] others)
        {
            var candidates = others.Prepend(first).DistinctBy(phase => (phase.Entry, phase.Value)).ToArray();
            if (candidates.Any(phase => occupied.Contains((phase.Entry, phase.Value))) || candidates.GroupBy(phase => phase.Entry)
                .Any(group => group.Select(phase => (phase.Selector.Opcode, phase.Selector.Value)).Distinct().Count() != 1))
                return null;
            var scene = new Scene($"{Fallback(first.Entry)}_At{PhaseIdentifier(first.Value)}", first.Entry, false, new()) { Rule = rule };
            foreach (var group in candidates.GroupBy(phase => phase.Entry))
            {
                var chapters = group.ToDictionary(phase => phase.Value, phase =>
                    candidates.Count(other => other.Entry.Slot == phase.Entry.Slot && other.Value == phase.Value) == 1
                        ? $"Phase{PhaseIdentifier(phase.Value)}" : $"Phase{PhaseIdentifier(phase.Value)}_Handler{phase.Entry.Tag}");
                if (group.Key == first.Entry)
                {
                    foreach (var (phase, name) in chapters)
                        scene.Chapters.Add(phase, name);
                }
                else
                    scene.AdditionalChapters.Add(group.Key, chapters);
                scene.Selectors.Add(group.Key, group.First().Selector);
                foreach (var phase in group)
                    scene.FragmentKeys.Add((phase.Entry, phase.Value), phase.Identity());
            }
            occupied.UnionWith(candidates.Select(phase => (phase.Entry, phase.Value)));
            scenes.Add(scene);
            return scene;
        }

        static string PhaseIdentifier(int phase) => phase < 0 ? $"Minus{-phase}" : phase.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    private readonly EventSceneSymbols _symbols;
    private readonly List<string> _outline = new();
    public IReadOnlyList<Scene> Scenes { get; }
    public IReadOnlyList<string> Outline => _outline;

    private readonly bool _allowNativePhaseDispatch;

    public CutsceneExport(string eventName, EventSceneSymbols symbols, IEnumerable<(EVContainerObject Object, EVInstructionBody[] Bodies)> objects, bool useNames, CutsceneCatalog? catalog, bool allowNativePhaseDispatch)
    {
        _allowNativePhaseDispatch = allowNativePhaseDispatch;
        _symbols = symbols;
        _eventName = eventName;
        _catalog = useNames ? CutsceneCatalog.Default : catalog;
        Handler[] handlers = objects.SelectMany(item => item.Object.Scripts.Select((script, index) =>
            new Handler(new Entry(item.Object.Id, script.Id), item.Object, item.Bodies[index], index))).ToArray();
        var counts = handlers.GroupBy(handler => handler.Entry).ToDictionary(group => group.Key, group => group.Count());
        var possibleDefinitions = new Dictionary<int, HashSet<int>> { [0] = [0] };
        _possibleDefinitions = possibleDefinitions;
        foreach (EVInstruction spawn in handlers.SelectMany(handler => handler.Body.Instructions).Where(instruction =>
            instruction.Opcode is Jsm.Opcode.NEW or Jsm.Opcode.NEW2 or Jsm.Opcode.NEW3 && instruction.Operands.All(operand => operand.Expression == null)))
        {
            int definition = spawn.Operands[0].Value;
            int uid = spawn.Operands[1].Value == 0 ? definition : spawn.Operands[1].Value;
            if (!possibleDefinitions.TryGetValue(uid, out var definitions))
                possibleDefinitions.Add(uid, definitions = new());
            definitions.Add(definition);
        }
        var requests = handlers.SelectMany(handler => handler.Body.Instructions.Where(IsRequest).Select(instruction => new Request(
            handler.Entry, instruction.Operands[1].Expression == null && instruction.Operands[1].Value < 250 ? instruction.Operands[1].Value : null,
            instruction.Operands[2].Value, instruction.Opcode, instruction.Operands[0].Expression == null ? instruction.Operands[0].Value : null, instruction.Label, handler.Ordinal))).ToArray();
        var scenes = new List<Scene>();
        var outline = _outline;
        var dialogueCandidates = new List<(Handler Handler, string Row)>();
        foreach (Handler handler in handlers)
        {
            IReadOnlyList<EVBasicBlock> blocks = handler.Body.GetBasicBlocks();
            Request[] outgoing = requests.Where(request => request.Caller == handler.Entry).ToArray();
            if (handler.Owner.Type == 1 && handler.Body.Instructions.Any(instruction => instruction.Opcode == Jsm.Opcode.MINIGAME))
            {
                if (outgoing is [{ Mode: Jsm.Opcode.REQ }, { Mode: Jsm.Opcode.REQEW }] && outgoing[0].Receiver == outgoing[1].Receiver &&
                    HasIndependentActorRequests(handler.Entry, outgoing))
                {
                    scenes.Add(new Scene(Fallback(handler.Entry), handler.Entry, true, new Dictionary<int, string> { [0] = "Play" }) { Rule = "external callback" });
                    continue;
                }
                outline.Add($"{handler.Entry.Slot}\t{handler.Entry.Tag}\tCard callback\t{blocks.Count}\tAn action has an unresolved receiver or another caller");
            }
            else if (handler.Owner.Type == 0 && FindBattleChapters(handler) is { } battle)
            {
                var phaseRequests = outgoing.Where(request => battle.Approach.Instructions.Any(instruction => instruction.Label == request.Position)).Take(3).ToArray();
                if (phaseRequests is [{ Mode: Jsm.Opcode.REQSW }, { Mode: Jsm.Opcode.REQSW }, { Mode: Jsm.Opcode.REQEW }] &&
                    HasIndependentActorRequests(handler.Entry, phaseRequests))
                {
                    var scene = new Scene(Fallback(handler.Entry), handler.Entry, false,
                        new Dictionary<int, string> { [battle.Approach.Value] = "Approach", [battle.Aftermath.Value] = "Aftermath" }) { Rule = "battle entries" };
                    scene.Selectors.Add(handler.Entry, battle.Approach.Selector);
                    scene.FragmentKeys.Add((handler.Entry, battle.Approach.Value), battle.Approach.Identity());
                    scene.FragmentKeys.Add((handler.Entry, battle.Aftermath.Value), battle.Aftermath.Identity());
                    scenes.Add(scene);
                    continue;
                }
                outline.Add($"{handler.Entry.Slot}\t{handler.Entry.Tag}\tBattle chapters\t{blocks.Count}\tThe approach requests cannot all be bound");
            }
            else if (handler.Entry.Tag == 1 && handler.Body.Instructions.Any(instruction => instruction.Opcode == Jsm.Opcode.JMP_SWITCHEX ||
                instruction.Opcode == Jsm.Opcode.JMP_SWITCH || instruction.Opcode == Jsm.Opcode.rsv0d) &&
                handler.Body.Instructions.Any(instruction => instruction.Opcode is Jsm.Opcode.MES or Jsm.Opcode.MESA or Jsm.Opcode.MESN))
            {
                string reason = outgoing.Any(request => request.Receiver == null) ? "Indirect receiver and phase dependencies" : "Cross-phase entry and exit proof pending";
                dialogueCandidates.Add((handler, $"{handler.Entry.Slot}\t{handler.Entry.Tag}\tDialogue phases\t{blocks.Count}\t{reason}"));
            }
        }
        AddPhaseScenes(handlers, counts, scenes, outline);
        foreach (var (handler, row) in dialogueCandidates)
        {
            var dispatches = handler.Body.Instructions.Where(step => step.Opcode is Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d).ToArray();
            var arms = CutscenePhaseGraph.Read(handler.Entry, handler.Owner.Type, handler.Body, includeSharedArms: true).ToArray();
            if (dispatches.Length == 1 && arms.Length > 0 && arms.All(arm => scenes.Any(scene =>
                scene.DirectorMachine != null && scene.Selectors.TryGetValue(handler.Entry, out var selector) && selector.Equals(arm.Selector) && scene.For(handler.Entry)?.ContainsKey(arm.Value) == true)))
                continue;
            outline.Add(row);
        }
        AddEntranceScenes(handlers.Where(handler => counts[handler.Entry] == 1), scenes);
        AddGuardedStoryScenes(handlers.Where(handler => counts[handler.Entry] == 1), scenes);
        AddOptionalIntroductions(handlers, scenes);
        var speakingEntries = handlers.Where(handler => counts[handler.Entry] == 1 && handler.Owner.Type == 2 && HasReachableDialogue(handler)).Select(handler => handler.Entry).ToHashSet();
        foreach (Handler director in handlers.Where(handler => IsScriptedSequence(handler) && counts[handler.Entry] == 1))
        {
            if (scenes.Any(scene => scene.For(director.Entry) != null))
                continue;

            Request[] calls = requests.Where(request => request.Caller == director.Entry).ToArray();
            var actorCalls = calls.Where(call => call.Receiver is int uid && possibleDefinitions.TryGetValue(uid, out var defs) &&
                defs.Count == 1 && symbols.Participants.TryGetValue(defs.Single(), out var participant) && participant.Kind == 2 && participant.Scripts.Any(script => script.Id == call.Tag))
                .ToDictionary(call => call.Position, call => (Uid: call.Receiver!.Value, Speaks:
                    speakingEntries.Contains(new Entry(possibleDefinitions[call.Receiver.Value].Single(), (ushort)call.Tag))));
            if (!HasStagedPath(director, actorCalls))
                continue;

            scenes.Add(new Scene(Fallback(director.Entry), director.Entry, true, new Dictionary<int, string> { [0] = "Play" }) { Rule = "directed actor sequence" });
        }
        _liveParticipants = IndexLiveParticipants(handlers);
        AddDirectorEpisodes(handlers, scenes);
        AddRegionEpisodes(handlers, scenes);
        AddTalkEpisodes(handlers, scenes);
        ValidateDirectorOwnership(scenes);
        if (outline.Count > 0 || scenes.Count > 0)
        {
            foreach (var group in requests.Where(request => request.Receiver != null).GroupBy(request => (request.Receiver, request.Tag)))
            {
                Entry[] callers = group.Select(request => request.Caller).Distinct().ToArray();
                if (callers.Length > 1)
                    outline.Add($"{group.Key.Receiver}\t{group.Key.Tag}\tShared request entry\t\tCallers: {string.Join(", ", callers.Select(caller => $"{caller.Slot}/{caller.Tag}"))}");
            }
            foreach (Request request in requests.Where(request => request.Receiver == null))
                outline.Add($"{request.Caller.Slot}\t{request.Caller.Tag}\t{request.Mode}\t\tIndirect target for tag {request.Tag}, priority {request.Level?.ToString() ?? "computed"}, instruction 0x{request.Position:X}");
        }
        _actionNames = ApplyCatalog(scenes, _catalog, eventName, symbols, outline);
        Scenes = scenes;
        IndexActionCandidates(handlers, counts, requests);

        bool HasIndependentActorRequests(Entry caller, Request[] calls)
        {
            foreach (Request request in calls)
            {
                if (request.Receiver is not int receiver || request.Tag is < 0 or > 255 ||
                    !symbols.Participants.TryGetValue(receiver, out var participant) || participant.Kind != 2 ||
                    !possibleDefinitions.TryGetValue(receiver, out var definitions) || !definitions.SetEquals([receiver]))
                    return false;
                var entry = new Entry(receiver, (ushort)request.Tag);
                if (counts.GetValueOrDefault(entry) != 1 ||
                    requests.Any(other => other.Tag == entry.Tag && (other.Receiver == null ||
                        other.Receiver == entry.Slot && other.Caller != caller ||
                        other.Receiver is int alias && alias != entry.Slot && possibleDefinitions.TryGetValue(alias, out var owners) && owners.Contains(entry.Slot))))
                    return false;
            }
            return true;
        }
    }

    private static List<CutsceneCatalogEntry> ApplyCatalog(List<Scene> scenes, CutsceneCatalog? catalog, string eventName, EventSceneSymbols symbols, List<string> outline)
    {
        var actionNames = new List<CutsceneCatalogEntry>();
        if (catalog == null)
            return actionNames;
        foreach (var group in catalog.Entries.Where(row => string.Equals(row.Event, eventName, StringComparison.OrdinalIgnoreCase))
            .GroupBy(row => (row.Slot, row.Tag, row.Phase)))
        {
            var row = group.First();
            var entry = new Entry(row.Slot, row.Tag);
            Scene[] matches = scenes.Where(scene =>
                row.Phase is int phase ? scene.For(entry)?.ContainsKey(phase) == true &&
                    row.Fragment != null && string.Equals(row.Fragment, scene.FragmentKeys.GetValueOrDefault((entry, phase)), StringComparison.OrdinalIgnoreCase) :
                scene.Entry == entry).ToArray();
            if (row.Phase == null && matches.Length == 0 && group.Count() == 1)
            {
                actionNames.Add(row);
                continue;
            }
            if (group.Count() != 1 || matches.Length != 1)
            {
                Diagnose("Stale or ambiguous catalog entry");
                continue;
            }
            Scene scene = matches[0];
            if (row.Parallel is { } parallel)
            {
                if (scene.ParallelTracks == null || !SyntaxFacts.IsValidIdentifier(parallel))
                    Diagnose("Invalid parallel scene entry name");
                else
                    scene.ParallelName = parallel;
            }
            if (row.Group != null)
            {
                if (!IsValidGroup(row.Group, symbols))
                    Diagnose("Invalid or conflicting group name");
                else
                    scene.Name = row.Group;
            }
            if (row.Name == null)
                continue;
            string? currentName = row.Phase is int existing ? scene.For(entry)!.GetValueOrDefault(existing) :
                scene.Whole ? scene.Chapters.GetValueOrDefault(0) : null;
            if (row.Name == currentName)
                continue;
            if (!SyntaxFacts.IsValidIdentifier(row.Name) || row.Name == scene.Name || (scene.DirectorMachine != null && row.Name == "Direct") ||
                (scene.DirectorMachine?.Choreography != null && row.Name is "Choreography" or "Role" or "Perform" or "PerformPhase") ||
                scenes.Where(candidate => candidate.Name == scene.Name).Any(candidate =>
                    candidate.Chapters.Values.Concat(candidate.AdditionalChapters.Values.SelectMany(values => values.Values))
                        .Contains(row.Name)))
            {
                Diagnose("Invalid or conflicting action name");
                continue;
            }
            if (row.Phase is int selected)
                scene.For(entry)![selected] = row.Name;
            else if (scene.Whole)
                scene.Chapters[0] = row.Name;
            else
                Diagnose("A chapter name requires a phase");

            void Diagnose(string reason) => outline.Add($"{row.Slot}\t{row.Tag}\tCatalog {row.Phase?.ToString() ?? "entry"}\t\t{reason}");
        }
        foreach (var group in scenes.GroupBy(scene => scene.Name).Where(group => group.Count() > 1).ToArray())
        {
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (Scene scene in group)
            {
                string[] methods = scene.Chapters.Values.Concat(scene.AdditionalChapters.Values.SelectMany(chapters => chapters.Values)).ToArray();
                if (methods.Any(names.Contains))
                {
                    outline.Add($"{scene.Entry.Slot}\t{scene.Entry.Tag}\tCatalog group\t\tConflicting methods in group {scene.Name}");
                    scene.Name = scene.DefaultName;
                }
                names.UnionWith(methods);
            }
        }
        return actionNames;
    }

    private static bool IsValidGroup(string name, EventSceneSymbols symbols) => SyntaxFacts.IsValidIdentifier(name) &&
        symbols.Participants.Values.All(participant => participant.Name != name) && name is not ("Location" or "Scene" or "SceneContext") &&
        typeof(EvilForest.EventSource.EventCutscene).Assembly.GetType($"EvilForest.EventSource.{name}") == null;

    private static string Fallback(Entry entry) => $"Scene_{entry.Slot:D2}_{entry.Tag:D2}";

    private static bool IsScriptedSequence(Handler handler)
    {
        IList<EVInstruction> code = handler.Body.Instructions;
        bool transfersField = code.Any(step => step.Opcode is Jsm.Opcode.MAPJUMP or Jsm.Opcode.ENCOUNT or Jsm.Opcode.ENCOUNT2);
        if (handler.Owner.Type != 0 && !(handler.Owner.Type == 2 && handler.Entry.Tag > 10 && transfersField))
            return false;
        if (code.Any(step => step.Opcode is Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d))
            return false;
        return transfersField || code.Any(step => step.Opcode is Jsm.Opcode.BGSSCROLL or Jsm.Opcode.UCOFF);
    }

    private static bool IsDialogue(EVInstruction step) => step.Opcode is Jsm.Opcode.MES or Jsm.Opcode.MESA or Jsm.Opcode.MESN or Jsm.Opcode.MESAN;

    private static bool HasReachableDialogue(Handler handler)
    {
        if (!handler.Body.Instructions.Any(IsDialogue))
            return false;
        var blocks = handler.Body.GetBasicBlocks().ToDictionary(block => block.Label);
        var visited = new HashSet<int>();
        var pending = new Stack<int>();
        pending.Push(handler.Body.Instructions[0].Label);
        while (pending.TryPop(out int label))
        {
            if (!visited.Add(label) || !blocks.TryGetValue(label, out var block))
                continue;
            if (block.Instructions.Any(IsDialogue))
                return true;
            foreach (int next in block.Successors)
                pending.Push(next);
        }
        return false;
    }

    private readonly record struct StageEvidence(int Actor, int SpeakingActor, int ExplicitSpeaker, int Line,
        bool Waits = false, bool Admits = false, bool Camera = false, bool LocksControl = false, bool ChangesField = false);

    private static bool HasStagedPath(Handler director, Dictionary<int, (int Uid, bool Speaks)> actorCalls)
    {
        const int multiple = -2;
        var blocks = director.Body.GetBasicBlocks().ToDictionary(block => block.Label);
        var pending = new Stack<(int, StageEvidence)>();
        var visited = new HashSet<(int, StageEvidence)>();
        pending.Push((director.Body.Instructions[0].Label, new StageEvidence(-1, -1, -1, -1)));
        while (pending.TryPop(out var item))
        {
            var (label, state) = item;
            if (!visited.Add(item) || !blocks.TryGetValue(label, out var block))
                continue;
            foreach (EVInstruction step in block.Instructions)
            {
                bool requestsActor = actorCalls.TryGetValue(step.Label, out var target);
                if (requestsActor)
                    state = state with { Actor = Add(state.Actor, target.Uid), SpeakingActor = target.Speaks ? Add(state.SpeakingActor, target.Uid) : state.SpeakingActor };
                if (IsDialogue(step))
                    state = state with { Line = Add(state.Line, step.Label) };
                if (step.Opcode is Jsm.Opcode.MESA or Jsm.Opcode.MESAN && step.Operands[0].Expression == null)
                    state = state with { ExplicitSpeaker = Add(state.ExplicitSpeaker, step.Operands[0].Value) };
                state = step.Opcode switch
                {
                    Jsm.Opcode.REQEW when requestsActor => state with { Waits = true },
                    Jsm.Opcode.REQSW when requestsActor => state with { Admits = true },
                    Jsm.Opcode.BGSSCROLL => state with { Camera = true },
                    Jsm.Opcode.UCOFF => state with { LocksControl = true },
                    Jsm.Opcode.MAPJUMP or Jsm.Opcode.ENCOUNT or Jsm.Opcode.ENCOUNT2 => state with { ChangesField = true },
                    _ => state
                };
                bool dialogue = state.Line == multiple || state.ChangesField && state.SpeakingActor == multiple;
                bool requested = state.Actor == multiple && (state.Waits || state.ChangesField && state.Admits);
                if ((director.Owner.Type == 0 || state.ChangesField) && dialogue && (requested && (state.ChangesField || state.LocksControl || state.Camera) ||
                    state.ChangesField && state.Camera && state.ExplicitSpeaker == multiple))
                    return true;
            }
            foreach (int next in block.Successors)
                pending.Push((next, state));
        }
        return false;

        static int Add(int existing, int next) => existing == -1 || existing == next ? next : multiple;
    }

    private static bool IsRequest(EVInstruction instruction) => instruction.Opcode is Jsm.Opcode.REQ or Jsm.Opcode.REQSW or Jsm.Opcode.REQEW;

    private static (CutscenePhaseGraph.Phase Approach, CutscenePhaseGraph.Phase Aftermath)? FindBattleChapters(Handler handler)
    {
        IList<EVInstruction> instructions = handler.Body.Instructions;
        if (!instructions.Any(instruction => instruction.Opcode == Jsm.Opcode.ENCOUNT) || !instructions.Any(instruction => instruction.Opcode == Jsm.Opcode.MAPJUMP))
            return null;
        var pairs = CutscenePhaseGraph.Read(handler.Entry, handler.Owner.Type, handler.Body).GroupBy(phase => (phase.Selector.Opcode, phase.Selector.Value))
            .SelectMany(group => group.Where(phase => phase.Has(Jsm.Opcode.ENCOUNT)).SelectMany(approach => group
                .Where(phase => phase.Value != approach.Value && phase.Has(Jsm.Opcode.MAPJUMP) && !phase.Has(Jsm.Opcode.ENCOUNT)).Select(after => (approach, after))))
            .Take(2).ToArray();
        return pairs is [var pair] ? pair : null;
    }

    public bool OwnsLocalStorage(int slot) => Scenes.Any(scene => scene.Entry.Slot == slot || scene.AdditionalChapters.Keys.Any(entry => entry.Slot == slot));

    public bool NeedsHandlerSource(int slot) => OwnsLocalStorage(slot) || _actionCandidates.Keys.Any(entry => entry.Slot == slot);

    public bool OwnsSwitchPhase(int slot, ushort tag, EVExpressionToken selector, int value)
    {
        var entry = new Entry(slot, tag);
        return Scenes.Any(scene => scene.Selectors.TryGetValue(entry, out EVExpressionToken phaseSelector) && phaseSelector.Equals(selector) &&
            scene.FragmentKeys.ContainsKey((entry, value)));
    }

    public string Rewrite(int slot, ushort tag, int ordinal, string owner, string source, StorageDeclarations storage, Func<int, string> jumpLabel, int statementOffset, IReadOnlyDictionary<int, int> instructionOffsets)
    {
        var key = new Entry(slot, tag);
        Scene[] scenes = Scenes.Where(candidate => candidate.For(key) != null).ToArray();
        bool actionCandidate = _actionCandidates.ContainsKey(key);
        if (scenes.Length == 0 && !actionCandidate)
            return source;

        var method = (MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(source)!;
        method = AnnotateCalls(method, slot, ordinal, statementOffset, instructionOffsets);
        if (scenes.Any(scene => scene.Talk != null || scene.Introduction != null || scene.GuardedChapters.Keys.Any(chapter => chapter.Entry == key)))
            method = AnnotateDecodedConditions(method, statementOffset, instructionOffsets);
        if (actionCandidate)
            _actionSources.Add(key, method);
        if (scenes.Length == 0)
            return source;

        foreach (Scene scene in scenes)
        {
            method = Rewrite(scene, key, owner, method, storage, jumpLabel);
            if (scene.DirectorMachine is { Choreography: not null } machine && machine.Director != key && key.Tag == 1)
                method = RewriteLinearRole(scene, key, method, storage);
        }
        _retainedCalls.UnionWith(AnnotatedCalls(method).Select(call => call.Site));
        if (scenes.Any(scene => scene.DirectorMachine is { } machine && machine.Director == key && (machine.Standalone || machine.Choreography != null)))
            return "";
        return Render(method);
    }

    private MethodDeclarationSyntax Rewrite(Scene scene, Entry key, string owner, MethodDeclarationSyntax method, StorageDeclarations storage, Func<int, string> jumpLabel)
    {
        var rewriter = new RewriteBody();
        var parameter = SyntaxFactory.ParseParameterList($"({owner} participant)");
        if (scene.DirectorMachine is { } machine && machine.Director == key)
            return RewriteDirector(scene, method, parameter, machine.Standalone, rewriter, storage);
        if (scene.Whole || scene.DirectorMachine?.Choreography != null && key.Tag > 1)
        {
            string name;
            if (scene.Whole)
            {
                name = scene.Chapters[0];
                scene.ExtractedChapters.Add((key, 0));
            }
            else
            {
                var names = Scenes.Where(candidate => candidate.Name == scene.Name)
                    .SelectMany(candidate => candidate.Methods.Select(member => member.Identifier.ValueText).Concat(candidate.Chapters.Values)
                        .Concat(candidate.AdditionalChapters.Values.SelectMany(values => values.Values)))
                    .Append(scene.Name).ToHashSet(StringComparer.Ordinal);
                name = ReserveName($"{method.Identifier.ValueText}_{_symbols.DirectorRoleName(key.Slot)}", names);
                MarkExtractedPhases(scene, key, scene.For(key)!.Keys.ToArray(), name);
            }
            var body = (BlockSyntax)rewriter.Visit(method.Body!)!;
            if (scene.Rule == "external callback")
            {
                body = ExtractCardHelpers(scene, method, parameter, StructureCommonExit(body));
                body = (BlockSyntax)new CardDialogue().Visit(body)!;
            }
            var helper = method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter)
                .WithAttributeLists(SyntaxFactory.List(method.AttributeLists.Where(list => list.Attributes.Any(attribute => attribute.Name.ToString() == "SwitchLayout"))))
                .WithModifiers(Modifiers("internal", method))
                .WithBody(body);
            scene.Methods.Insert(0, helper);
            string call = $"ctx.{scene.Name}.{name}(this);";
            if (!scene.Whole)
                method = method.WithAttributeLists(SyntaxFactory.List(method.AttributeLists.Where(list => list.Attributes.All(attribute => attribute.Name.ToString() != "SwitchLayout"))));
            return method.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement(method.ReturnType.ToString() == "void" ? call : $"return {call}")));
        }
        IReadOnlyDictionary<int, string> chapters = scene.For(key)!;
        if (scene.Rule == "field entrance dialogue")
            return RewriteEntrance(scene, key, method, parameter, rewriter, storage);
        if (scene.StoryGuard != null)
            return RewriteGuardedStory(scene, key, method, parameter, rewriter, storage);
        if (scene.Introduction != null)
            return RewriteOptionalIntroduction(scene, key, method, parameter, rewriter, storage, jumpLabel);
        if (scene.Region != null)
            return RewriteRegionEpisode(scene, key, method, parameter, rewriter);
        if (scene.Talk != null)
            return RewriteTalkEpisode(scene, key, method, parameter, rewriter, jumpLabel);
        var selections = method.Body!.DescendantNodes().OfType<SwitchStatementSyntax>();
        if (scene.Selectors.TryGetValue(key, out EVExpressionToken selector))
        {
            if (scene.GuardedChapters.Keys.Any(chapter => chapter.Entry == key))
            {
                method = RewriteGuardedPhases(scene, key, method, parameter, rewriter, storage.Qualify(selector));
                selections = method.Body!.DescendantNodes().OfType<SwitchStatementSyntax>();
            }
            selections = selections.Where(selection => selection.Expression.ToString() == storage.Qualify(selector));
        }
        SwitchStatementSyntax[] matching = selections.Where(selection => selection.Sections.SelectMany(section => section.Labels)
            .OfType<CaseSwitchLabelSyntax>().Any(label => PhaseValue(label) is int number && chapters.ContainsKey(number))).Take(2).ToArray();
        if (matching is not [var selection])
            return method;
        bool directDispatch = scene.DirectorMachine != null
            ? selection.Sections.All(JumpsToNamedLabel)
            : selection.Sections.Any(JumpsToNamedLabel);
        if (directDispatch)
            return RewriteJumpPhases(method, selection, scene, key, chapters, parameter, rewriter);
        string? dispatchExit = scene.DirectorMachine != null && selection.Parent is BlockSyntax block &&
            block.Statements.Skip(block.Statements.IndexOf(selection) + 1).FirstOrDefault() is LabeledStatementSyntax exitLabel
            ? exitLabel.Identifier.ValueText : null;
        var sections = new List<SwitchSectionSyntax>();
        foreach (SwitchSectionSyntax section in selection.Sections)
        {
            int[] phases = MatchingPhases(section, scene, key, chapters);
            if (phases.Length == 0)
            {
                sections.Add(section);
                continue;
            }
            int split = section.Statements.Count;
            while (split > 0 && section.Statements[split - 1] is BreakStatementSyntax or ContinueStatementSyntax or
                ReturnStatementSyntax { Expression: null } or GotoStatementSyntax { Expression: IdentifierNameSyntax })
                split--;
            bool phaseBreak = scene.DirectorMachine != null && section.Statements.Skip(split).FirstOrDefault() is BreakStatementSyntax;
            string? gotoExit = phaseBreak ? dispatchExit : null;
            if ((split == 0 && scene.DirectorMachine == null) || split == section.Statements.Count || !IsClosedFragment(method, section.Statements.Take(split), section, phaseBreak, gotoExit))
            {
                sections.Add(section);
                continue;
            }
            var fragment = section.Statements.Take(split).ToArray();
            if (phaseBreak)
            {
                fragment = fragment.Select(statement => statement.ReplaceNodes(statement.DescendantNodesAndSelf().OfType<BreakStatementSyntax>()
                    .Where(jump => !IsNestedLoopTransfer(jump, section)), (jump, _) => (StatementSyntax)SyntaxFactory.ReturnStatement().WithTriviaFrom(jump))).ToArray();
            }
            BlockSyntax body = PreparePhaseBody(scene, key, phases[0], method, parameter, rewriter, PhaseBody(method, fragment, gotoExit));
            string name = chapters[phases[0]];
            bool routine = split != 0 && method.ReturnType.ToString() == "EventRoutine";
            scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter)
                .WithAttributeLists(default).WithModifiers(routine ? Modifiers("internal", method) : SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.InternalKeyword)))
                .WithReturnType(routine ? method.ReturnType : SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword))).WithBody(body));
            MarkExtractedPhases(scene, key, phases, name);
            var call = SyntaxFactory.ParseStatement($"{(routine ? "await " : "")}ctx.{scene.Name}.{name}(this);");
            sections.Add(section.WithStatements(SyntaxFactory.List(section.Statements.Skip(split).Prepend(call))));
        }
        return method.ReplaceNode(selection, selection.WithSections(SyntaxFactory.List(sections)));
    }

    private static BlockSyntax ExtractCardHelpers(Scene scene, MethodDeclarationSyntax method, ParameterListSyntax parameter, BlockSyntax body)
    {
        method = method.WithBody(body);
        body = method.Body!;
        var statements = body.Statements;
        int tryStart = IndexOfCall(statements, "scripts.RequestAsync");
        int runAndWait = IndexOfCall(statements, "scripts.RequestAndWait");
        int entrance = statements.IndexOf(statements.OfType<IfStatementSyntax>().FirstOrDefault()!) + 1;
        if (tryStart < entrance || runAndWait < tryStart || statements.LastOrDefault() is not ReturnStatementSyntax)
            return body;
        if (!IsClosedFragment(method, statements.Skip(entrance).Take(tryStart - entrance), body) ||
            !IsClosedFragment(method, statements.Skip(runAndWait + 1).SkipLast(1), body))
            return body;

        var decks = body.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => call.Expression.ToString() == "Inventory.TetraMaster").Take(2).ToArray();
        var deck = decks.Length == 1 ? decks[0] : null;
        if (deck?.Ancestors().OfType<BlockSyntax>().FirstOrDefault() is { } match &&
            match.Statements.FirstOrDefault() is ExpressionStatementSyntax { Expression: AwaitExpressionSyntax { Expression: InvocationExpressionSyntax firstCall } } &&
            firstCall.Expression.ToString() == "Messages.ShowAndWait" && match.Statements.Count > 1 &&
            IsClosedFragment(method, match.Statements.Skip(1), match))
        {
            StatementSyntax call = AddHelper("PlayCardMatch", match.Statements.Skip(1));
            body = body.ReplaceNode(match, match.WithStatements(SyntaxFactory.List(new[] { match.Statements[0], call })));
            statements = body.Statements;
        }

        var rewritten = statements.Take(entrance).Append(AddHelper("EnterCardInteraction", statements.Skip(entrance).Take(tryStart - entrance)))
            .Concat(statements.Skip(tryStart).Take(runAndWait - tryStart + 1))
            .Append(AddHelper("RestoreCardInteraction", statements.Skip(runAndWait + 1).SkipLast(1)))
            .Append(statements.Last());
        return body.WithStatements(SyntaxFactory.List(rewritten));

        StatementSyntax AddHelper(string name, IEnumerable<StatementSyntax> statements)
        {
            var content = SyntaxFactory.Block(statements);
            bool suspends = content.DescendantNodes().OfType<AwaitExpressionSyntax>().Any();
            scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter)
                .WithAttributeLists(default)
                .WithReturnType(SyntaxFactory.ParseTypeName(suspends ? "EventRoutine" : "void"))
                .WithModifiers(suspends ? Modifiers("private", method) : SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                .WithBody(content));
            return SyntaxFactory.ParseStatement($"{(suspends ? "await " : "")}{name}(participant);");
        }

        static int IndexOfCall(SyntaxList<StatementSyntax> source, string name)
        {
            for (int index = 0; index < source.Count; index++)
            {
                if (source[index]!.DescendantNodes().OfType<InvocationExpressionSyntax>().FirstOrDefault()?.Expression.ToString() == name)
                    return index;
            }
            return -1;
        }
    }

    private MethodDeclarationSyntax RewriteJumpPhases(MethodDeclarationSyntax method, SwitchStatementSyntax selection, Scene scene, Entry key,
        IReadOnlyDictionary<int, string> chapters, ParameterListSyntax parameter, RewriteBody rewriter)
    {
        if (selection.Parent is not BlockSyntax dispatchBlock)
            return method;
        var entrances = selection.Sections.Where(JumpsToNamedLabel)
            .Select(section => ((GotoStatementSyntax)section.Statements[0]).Expression?.ToString()).ToHashSet(StringComparer.Ordinal);
        var edits = new Dictionary<int, (int End, string Name)>();
        foreach (SwitchSectionSyntax section in selection.Sections)
        {
            int[] phases = MatchingPhases(section, scene, key, chapters);
            if (phases.Length == 0 ||
                section.Statements is not [GotoStatementSyntax { Expression: { } target }])
                continue;
            int start = dispatchBlock.Statements.IndexOf(dispatchBlock.Statements.OfType<LabeledStatementSyntax>()
                .FirstOrDefault(label => label.Identifier.ValueText == target.ToString())!);
            if (start < 0)
                continue;
            int end = start + 1;
            while (end < dispatchBlock.Statements.Count &&
                !(dispatchBlock.Statements[end] is LabeledStatementSyntax next && entrances.Contains(next.Identifier.ValueText)))
                end++;
            bool machine = scene.DirectorMachine != null;
            if (!machine && (end <= start + 2 || dispatchBlock.Statements[end - 1] is not GotoStatementSyntax))
                continue;
            int stop = end;
            if (end > start + 1 && dispatchBlock.Statements[end - 1] is GotoStatementSyntax or ReturnStatementSyntax)
                stop--;
            string? exit = machine && stop < dispatchBlock.Statements.Count ? dispatchBlock.Statements[stop] switch
            {
                GotoStatementSyntax { Expression: IdentifierNameSyntax destination } => destination.Identifier.ValueText,
                LabeledStatementSyntax label => label.Identifier.ValueText,
                _ => null
            } : null;
            StatementSyntax[] fragment = dispatchBlock.Statements.Skip(start + 1).Take(stop - start - 1).ToArray();
            if (!IsClosedFragment(method, fragment, dispatchBlock, gotoExit: exit))
                continue;
            int phase = phases[0];
            BlockSyntax body = PreparePhaseBody(scene, key, phase, method, parameter, rewriter, PhaseBody(method, fragment, exit));
            string name = chapters[phase];
            bool routine = method.ReturnType.ToString() == "EventRoutine";
            scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter).WithAttributeLists(default)
                .WithModifiers(Modifiers("internal", method)).WithBody(body));
            MarkExtractedPhases(scene, key, phases, name);
            edits.Add(start + 1, (stop, $"{(routine ? "await " : "")}ctx.{scene.Name}.{name}(this);"));
        }
        if (edits.Count == 0)
            return method;
        var statements = new List<StatementSyntax>();
        for (int index = 0; index <= dispatchBlock.Statements.Count; index++)
        {
            if (edits.TryGetValue(index, out var edit))
            {
                statements.Add(SyntaxFactory.ParseStatement(edit.Name));
                index = edit.End;
            }
            if (index < dispatchBlock.Statements.Count)
                statements.Add(dispatchBlock.Statements[index]);
        }
        return method.ReplaceNode(dispatchBlock, dispatchBlock.WithStatements(SyntaxFactory.List(statements)));
    }

    private BlockSyntax PreparePhaseBody(Scene scene, Entry entry, int phase, MethodDeclarationSyntax method, ParameterListSyntax parameter, RewriteBody rewriter, BlockSyntax fragment)
    {
        var body = (BlockSyntax)rewriter.Visit(fragment)!;
        if (scene.Rule != "choice and dispatch")
            return body;
        return AddRoleDispatch(scene, method, SplitDispatchContinuation(scene, method, parameter, body), entry, phase);
    }

    private static BlockSyntax PhaseBody(MethodDeclarationSyntax method, StatementSyntax[] fragment, string? gotoExit)
    {
        var body = SyntaxFactory.Block(method.Body!.Statements.OfType<LocalDeclarationStatementSyntax>().Cast<StatementSyntax>().Concat(fragment));
        if (gotoExit == null)
            return body;
        var exits = body.DescendantNodes().OfType<GotoStatementSyntax>().Where(jump =>
            jump.IsKind(SyntaxKind.GotoStatement) && jump.Expression is IdentifierNameSyntax target && target.Identifier.ValueText == gotoExit).ToArray();
        if (exits.Length == 0)
            return body;

        // The dispatcher owns this continuation. Inlining an early return must reach
        // that continuation even when another native arm physically follows the role.
        return body.ReplaceNodes(exits, (jump, _) => (StatementSyntax)SyntaxFactory.ReturnStatement().WithTriviaFrom(jump)).AddStatements(SyntaxFactory.ReturnStatement());
    }

    private static int[] MatchingPhases(SwitchSectionSyntax section, Scene scene, Entry key, IReadOnlyDictionary<int, string> chapters)
    {
        if (scene.DirectorMachine == null && section.Labels.Count != 1)
            return [];
        var values = section.Labels.OfType<CaseSwitchLabelSyntax>().Select(PhaseValue).OfType<int>().Where(chapters.ContainsKey).ToHashSet();
        return scene.DirectorMachine is { } machine
            ? machine.OrderedChapters.Where(chapter => chapter.Entry == key && values.Contains(chapter.Phase)).Select(chapter => chapter.Phase).ToArray()
            : values.ToArray();
    }

    private static int? PhaseValue(CaseSwitchLabelSyntax label) => label.Value switch
    {
        LiteralExpressionSyntax { Token.Value: int value } => value,
        PrefixUnaryExpressionSyntax { Operand: LiteralExpressionSyntax { Token.Value: int value } } prefix when prefix.IsKind(SyntaxKind.UnaryMinusExpression) => -value,
        _ => null
    };

    private static void MarkExtractedPhases(Scene scene, Entry entry, int[] phases, string name)
    {
        foreach (int phase in phases)
        {
            if (scene.For(entry)![phase] != name)
                scene.SharedChapters.Add((entry, phase), name);
            scene.ExtractedChapters.Add((entry, phase));
        }
    }

    private static bool HasExternalLoopTransfer(IEnumerable<StatementSyntax> statements, SyntaxNode boundary, bool allowBreak = false) =>
        statements.SelectMany(statement => statement.DescendantNodesAndSelf()).Any(node =>
            (node is ContinueStatementSyntax || node is BreakStatementSyntax && !allowBreak) && !IsNestedLoopTransfer(node, boundary));

    private static bool IsNestedLoopTransfer(SyntaxNode node, SyntaxNode boundary) =>
        node.Ancestors().TakeWhile(ancestor => ancestor != boundary).Any(ancestor =>
            ancestor is WhileStatementSyntax or ForStatementSyntax or DoStatementSyntax || node is BreakStatementSyntax && ancestor is SwitchStatementSyntax);

    private static bool JumpsToNamedLabel(SwitchSectionSyntax section) =>
        section.Statements is [GotoStatementSyntax jump] && jump.IsKind(SyntaxKind.GotoStatement);

    private static bool IsClosedFragment(MethodDeclarationSyntax method, IEnumerable<StatementSyntax> statements, SyntaxNode boundary, bool allowBreak = false, string? gotoExit = null)
    {
        StatementSyntax[] fragment = statements.ToArray();
        var innerLabels = fragment.SelectMany(statement => statement.DescendantNodesAndSelf().OfType<LabeledStatementSyntax>())
            .Select(label => label.Identifier.ValueText).ToHashSet(StringComparer.Ordinal);
        var innerJumps = fragment.SelectMany(statement => statement.DescendantNodesAndSelf().OfType<GotoStatementSyntax>()).ToHashSet();
        return innerJumps.All(jump => jump.IsKind(SyntaxKind.GotoStatement)
            ? jump.Expression is IdentifierNameSyntax target && (innerLabels.Contains(target.Identifier.ValueText) || target.Identifier.ValueText == gotoExit)
            : jump.Ancestors().TakeWhile(ancestor => ancestor != boundary).OfType<SwitchStatementSyntax>().Any()) &&
            !method.DescendantNodes().OfType<GotoStatementSyntax>().Any(jump => jump.IsKind(SyntaxKind.GotoStatement) && innerLabels.Contains(jump.Expression?.ToString() ?? "") && !innerJumps.Contains(jump)) &&
            !fragment.Any(statement => statement.DescendantNodesAndSelf().OfType<ReturnStatementSyntax>().Any()) && !HasExternalLoopTransfer(fragment, boundary, allowBreak);
    }

    private static BlockSyntax SplitDispatchContinuation(Scene scene, MethodDeclarationSyntax handler, ParameterListSyntax parameter, BlockSyntax body)
    {
        int wait = body.Statements.IndexOf(body.Statements.FirstOrDefault(IsWindowWait)!);
        if (wait < 0 || body.Statements.Take(wait).SelectMany(statement => statement.DescendantNodesAndSelf()).OfType<InvocationExpressionSyntax>()
                .Count(call => call.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "RequestWhenReady" }) < 2)
            return body;
        var method = handler.WithBody(body);
        var candidate = method.Body!;
        var continuation = candidate.Statements.Skip(wait + 1).ToArray();
        if (continuation.Length == 0 || !IsClosedFragment(method, continuation, candidate))
            return body;
        const string helperName = "ContinueAfterHelperDispatch";
        if (scene.Methods.Any(member => member.Identifier.ValueText == helperName))
            return body;
        var locals = body.Statements.Take(wait).OfType<LocalDeclarationStatementSyntax>();
        scene.Methods.Add(handler.WithIdentifier(SyntaxFactory.Identifier(helperName)).WithParameterList(parameter).WithAttributeLists(default)
            .WithModifiers(Modifiers("private", handler)).WithBody(SyntaxFactory.Block(locals.Cast<StatementSyntax>().Concat(continuation))));
        return body.WithStatements(SyntaxFactory.List(body.Statements.Take(wait + 1).Append(SyntaxFactory.ParseStatement($"await {helperName}(participant);"))));

        static bool IsWindowWait(StatementSyntax statement) => statement is ExpressionStatementSyntax
        {
            Expression: AwaitExpressionSyntax { Expression: InvocationExpressionSyntax
            {
                Expression: MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Messages" }, Name.Identifier.ValueText: "WaitWindow" }
            } }
        };
    }

    private static SyntaxTokenList Modifiers(string access, MethodDeclarationSyntax method) =>
        SyntaxFactory.TokenList(new[] { SyntaxFactory.ParseToken(access) }.Concat(method.Modifiers.Where(token => token.IsKind(SyntaxKind.AsyncKeyword))));

    private static BlockSyntax StructureCommonExit(BlockSyntax body)
    {
        for (int i = 0; i + 2 < body.Statements.Count; i++)
        {
            if (body.Statements[i] is not IfStatementSyntax { Else: null, Statement: BlockSyntax branch } conditional ||
                branch.Statements.LastOrDefault() is not GotoStatementSyntax jump)
                continue;
            int join = i + 1;
            while (join < body.Statements.Count && body.Statements[join] is not LabeledStatementSyntax)
                join++;
            if (join == i + 1 || join == body.Statements.Count || body.Statements[join] is not LabeledStatementSyntax label ||
                jump.Expression?.ToString() != label.Identifier.ValueText ||
                body.DescendantNodes().OfType<GotoStatementSyntax>().Count(other => other.Expression?.ToString() == label.Identifier.ValueText) != 1)
                continue;
            var replacement = conditional.WithStatement(branch.WithStatements(branch.Statements.RemoveAt(branch.Statements.Count - 1)))
                .WithElse(SyntaxFactory.ElseClause(SyntaxFactory.Block(body.Statements.Skip(i + 1).Take(join - i - 1))));
            return body.WithStatements(SyntaxFactory.List(body.Statements.Take(i).Append<StatementSyntax>(replacement).Concat(body.Statements.Skip(join + 1))));
        }
        return body;
    }

    public static string Render(MemberDeclarationSyntax declaration)
    {
        var formatted = declaration.NormalizeWhitespace(eol: Environment.NewLine);
        if (formatted is MethodDeclarationSyntax method && (method.Identifier.ValueText == "Direct" || IsChoreographyRoute(method)))
        {
            var groups = formatted.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call =>
                call.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Together" or "Until" or "RunUntilCue" } &&
                call.ArgumentList.Arguments.Count > 2);
            var breaks = new Dictionary<SyntaxToken, SyntaxTriviaList>();
            foreach (var group in groups)
            {
                var statement = group.Ancestors().OfType<ExpressionStatementSyntax>().First();
                string indent = statement.GetLeadingTrivia().LastOrDefault(trivia => trivia.IsKind(SyntaxKind.WhitespaceTrivia)).ToString() + "    ";
                var arguments = group.ArgumentList.Arguments;
                for (int i = 1; i < arguments.Count; i++)
                {
                    if (arguments[i].Expression is InvocationExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Role" } })
                        breaks.Add(arguments.GetSeparator(i - 1), SyntaxFactory.TriviaList(SyntaxFactory.EndOfLine(Environment.NewLine), SyntaxFactory.Whitespace(indent)));
                }
            }
            formatted = formatted.ReplaceTokens(breaks.Keys, (original, token) => token.WithTrailingTrivia(breaks[original]));
        }
        return Environment.NewLine + string.Join(Environment.NewLine, formatted.ToFullString()
            .Split(Environment.NewLine).Select(line => line.Length == 0 ? "" : "    " + line)) + Environment.NewLine;
    }

    private sealed class RewriteBody : CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node.Expression is IdentifierNameSyntax identifier && identifier.Identifier.ValueText is "Local" or "Animation" or "Actions")
                return node.WithExpression(SyntaxFactory.ParseExpression($"participant.{identifier}").WithTriviaFrom(identifier));
            return base.VisitMemberAccessExpression(node);
        }
    }

    private sealed class CardDialogue : CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Messages" }, Name.Identifier.ValueText: "ShowAndWait" } &&
                node.ArgumentList.Arguments.Count is 3 or 4)
            {
                var arguments = node.ArgumentList.Arguments;
                string line = string.Join(", ", arguments.Take(3));
                string mask = arguments.Count > 3 ? $", {arguments[3]}" : "";
                return SyntaxFactory.ParseExpression($"Dialogue.Say(new DialogueLine({line}){mask})").WithTriviaFrom(node);
            }
            return base.VisitInvocationExpression(node);
        }

        public override SyntaxNode? VisitIsPatternExpression(IsPatternExpressionSyntax node)
        {
            ExpressionSyntax expression = node.Expression;
            while (expression is ParenthesizedExpressionSyntax parenthesized)
                expression = parenthesized.Expression;
            if (expression is InvocationExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Engine" }, Name.Identifier.ValueText: "ReadSystem" },
                    ArgumentList.Arguments: [var argument]
                } && argument.Expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "SystemVariable" }, Name.Identifier.ValueText: "DialogChoice" } &&
                node.Pattern is UnaryPatternSyntax { Pattern: ConstantPatternSyntax { Expression: LiteralExpressionSyntax { Token.Value: 0 } } })
                return SyntaxFactory.ParseExpression("((DialogueOption)Engine.ReadSystem(SystemVariable.DialogChoice)) is not DialogueOption.First").WithTriviaFrom(node);
            return base.VisitIsPatternExpression(node);
        }
    }
}
