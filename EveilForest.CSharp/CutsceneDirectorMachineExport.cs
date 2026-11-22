using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    internal sealed record DirectorPhaseMachine(Entry Director, bool Standalone, IReadOnlyList<(Entry Entry, int Phase)> OrderedChapters, IReadOnlySet<CutscenePhaseGraph.Cell> Cues)
    {
        public LinearChoreography? Choreography { get; init; }
    }
    private sealed record DirectorComponent(CutscenePhaseGraph.Phase EntryPhase, CutscenePhaseGraph.Phase? Speaker, HashSet<int> Values, CutscenePhaseGraph.Phase[] Arms, HashSet<CutscenePhaseGraph.Cell> Cues);

    private void AddDirectorMachines(Handler[] handlers, List<Scene> scenes, HashSet<(Entry Entry, int Value)> occupied, CutscenePhaseGraph.Phase[] arms)
    {
        var entries = handlers.ToLookup(handler => handler.Entry);
        foreach (var director in arms.Where(phase => phase.OwnerKind == 0).GroupBy(phase => phase.Entry).Where(group => group.Any(phase => phase.Successors.Any())))
        {
            CutscenePhaseGraph.Phase[] directing = director.ToArray();
            foreach (DirectorComponent discovered in DiscoverDirectorComponents(handlers, arms, directing))
            {
                Handler owner = entries[director.Key].Single();
                DirectorComponent machine = ExtendDirectorPollingDispatch(owner, handlers, arms, discovered);
                machine = IncludeParticipantOnlyPhases(owner, handlers, arms, machine);
                var (entryPhase, speaker, connected, component, cues) = machine;
                CutscenePhaseGraph.Phase[] componentDirector = component.Where(phase => phase.Entry == director.Key).ToArray();
                bool usesCues = cues.Count != 0;

                var componentKeys = component.Select(phase => (phase.Entry, phase.Value)).ToHashSet();
                var embedded = scenes.Where(scene => scene.FragmentKeys.Keys.Any(componentKeys.Contains)).ToArray();
                foreach (Scene existing in embedded)
                {
                    if (existing.DirectorMachine != null || existing.Whole || existing.FragmentKeys.Keys.Any(key => !componentKeys.Contains(key)) ||
                        existing.FragmentKeys.Any(pair => !component.Any(arm => (arm.Entry, arm.Value) == pair.Key && arm.Identity() == pair.Value)))
                    {
                        var overlap = existing.FragmentKeys.Keys.First(componentKeys.Contains);
                        throw DirectorPhaseError(overlap.Entry, overlap.Phase, $"has incompatible ownership with {existing.Rule}", director.Key);
                    }
                }
                if (embedded.Count(scene => scene.ParallelTracks != null) > 1)
                    throw DirectorPhaseError(director.Key, entryPhase.Value, "contains multiple independent parallel groups");
                foreach (var phase in component.GroupBy(phase => (phase.Entry, phase.Value)))
                {
                    if (phase.FirstOrDefault(arm => arm.GuardConditionLabel != null) is { } guard && phase.FirstOrDefault(arm => !ReferenceEquals(guard, arm)) is { } other)
                        throw DirectorPhaseError(phase.Key.Entry, phase.Key.Value, $"has overlapping role arms at 0x{guard.GuardConditionLabel:X} and 0x{other.GuardConditionLabel ?? other.Start:X}", director.Key);
                    if (phase.Skip(1).Any() || entries[phase.Key.Entry].Count() != 1)
                        throw DirectorPhaseError(phase.Key.Entry, phase.Key.Value, "has an ambiguous dispatch arm or handler", director.Key);
                    if (occupied.Contains(phase.Key) && embedded.All(scene => !scene.FragmentKeys.ContainsKey(phase.Key)))
                        throw DirectorPhaseError(phase.Key.Entry, phase.Key.Value, $"is selected by director {director.Key.Slot}/{director.Key.Tag} and {scenes.First(scene => scene.For(phase.Key.Entry)?.ContainsKey(phase.Key.Value) == true).Rule}");
                }

                if (usesCues)
                {
                    foreach (CutscenePhaseGraph.Phase phase in component)
                    {
                        if (CutscenePhaseGraph.FindUnmappedTransition(phase) is { } transition)
                            throw DirectorPhaseError(phase.Entry, phase.Value, $"changes the phase selector at 0x{transition.Label:X} without a resolved destination", director.Key);
                    }
                }

                bool standalone = CanEmitStandaloneDirector(owner, handlers);
                LinearChoreography? choreography = null;
                string? reason = "There is no proven phase completion signal.";
                if (usesCues)
                    (choreography, reason) = ReadLinearChoreography(owner, component, cues, entries, standalone || CanMoveSceneDirectorEntry(owner, handlers));
                if (reason != null)
                {
                    if (!_allowNativePhaseDispatch)
                        throw DirectorPhaseError(owner.Entry, null, $"cannot export one choreography script. {reason.TrimEnd('.')}", owner.Entry);
                    _outline.Add($"{owner.Entry.Slot}\t{owner.Entry.Tag}\tEditable choreography\t\tExplicit dispatch preserved. {reason}");
                }
                CutscenePhaseGraph.Phase anchor = usesCues ? entryPhase : speaker!;
                string name = usesCues ? Fallback(anchor.Entry) : $"{Fallback(anchor.Entry)}_At{PhaseIdentifier(anchor.Value)}";
                int[] order = OrderDirectorPhases(componentDirector, ReadTransitions(component).ToArray(), connected);
                var phaseOrder = order.Select((value, index) => (value, index)).ToDictionary(item => item.value, item => item.index);
                var ordered = component.OrderBy(phase => phaseOrder[phase.Value]).ThenBy(phase => phase.Entry != director.Key)
                    .ThenBy(phase => phase.Entry.Slot).ThenBy(phase => phase.Entry.Tag).ToArray();
                var scene = new Scene(name, anchor.Entry, false, new())
                {
                    Rule = usesCues ? "director phase machine" : "coordinated dialogue",
                    ParallelTracks = embedded.SingleOrDefault(candidate => candidate.ParallelTracks != null)?.ParallelTracks,
                    DirectorMachine = new DirectorPhaseMachine(director.Key, standalone, ordered.Select(phase => (phase.Entry, phase.Value)).ToArray(), cues)
                    {
                        Choreography = choreography
                    }
                };
                foreach (var group in ordered.GroupBy(phase => phase.Entry))
                {
                    Dictionary<int, string> chapters = group.Key == anchor.Entry ? scene.Chapters : new();
                    if (group.Key != anchor.Entry)
                        scene.AdditionalChapters.Add(group.Key, chapters);
                    scene.Selectors.Add(group.Key, group.First().Selector);
                    bool hasSeveralTracks = ordered.Where(phase => phase.Entry.Slot == group.Key.Slot).Select(phase => phase.Entry).Distinct().Skip(1).Any();
                    string role = group.Key == director.Key ? "Director" : _symbols.DirectorRoleName(group.Key.Slot);
                    if (hasSeveralTracks && (choreography == null || group.Key.Tag != 1))
                        role += $"_Handler{group.Key.Tag}";
                    foreach (CutscenePhaseGraph.Phase phase in group)
                    {
                        chapters.Add(phase.Value, $"Phase{PhaseIdentifier(phase.Value)}_{role}");
                        scene.FragmentKeys.Add((phase.Entry, phase.Value), phase.Identity());
                        if (phase.GuardConditionLabel is int condition)
                            scene.GuardedChapters.Add((phase.Entry, phase.Value), condition);
                    }
                }
                occupied.UnionWith(component.Select(phase => (phase.Entry, phase.Value)));
                foreach (Scene existing in embedded)
                    scenes.Remove(existing);
                scenes.Add(scene);
            }
        }
    }

    private DirectorComponent ExtendDirectorPollingDispatch(Handler owner, Handler[] handlers, CutscenePhaseGraph.Phase[] arms, DirectorComponent discovered)
    {
        if (discovered.Cues.Count != 1 ||
            !TryReadDirectorPollingArms(owner, discovered.EntryPhase.Selector, out var polling, out _, out _))
            return discovered;
        int[] additional = polling.Keys.Except(discovered.Values).ToArray();
        if (additional.Length == 0)
            return discovered;

        // The original switch is one physical callback, including its externally selected phases.
        // Include those arms only when they belong to the same proven polling dispatch.
        int[] coordinatorArms = arms.Where(arm => arm.Entry == owner.Entry && arm.Selector.Equals(discovered.EntryPhase.Selector)).Select(arm => arm.Value).ToArray();
        if (!additional.All(coordinatorArms.Contains))
            return discovered;
        return ExpandDirectorComponent(handlers, arms, discovered.EntryPhase, discovered.Speaker, [.. discovered.Values, .. additional], [.. discovered.Cues]);
    }

    private List<DirectorComponent> DiscoverDirectorComponents(Handler[] handlers, CutscenePhaseGraph.Phase[] arms, CutscenePhaseGraph.Phase[] directing)
    {
        Entry director = directing[0].Entry;
        var transitions = ReadTransitions(directing).ToArray();
        var adjacent = transitions.Concat(transitions.Select(edge => (From: edge.To, To: edge.From))).ToLookup(edge => edge.From, edge => edge.To);
        var remaining = transitions.Select(edge => edge.From).ToHashSet();
        var machines = new List<DirectorComponent>();
        foreach (CutscenePhaseGraph.Phase entryPhase in directing)
        {
            if (!remaining.Contains(entryPhase.Value))
                continue;
            var connected = new HashSet<int>();
            var pending = new Queue<int>();
            pending.Enqueue(entryPhase.Value);
            while (pending.TryDequeue(out int value))
            {
                if (!connected.Add(value))
                    continue;
                foreach (int neighbor in adjacent[value])
                    pending.Enqueue(neighbor);
            }
            remaining.ExceptWith(connected);

            CutscenePhaseGraph.Phase[] initial = arms.Where(phase => connected.Contains(phase.Value)).ToArray();
            CutscenePhaseGraph.Phase[] roles = initial.Where(phase => phase.Entry != director).ToArray();
            CutscenePhaseGraph.Phase[] speakers = roles.Where(phase => phase.OwnerKind == 2 && phase.Speaks).ToArray();
            var cues = ReadRepeatedRoleCues(initial.Where(phase => phase.Entry == director), roles);
            bool exchange = speakers.Select(phase => phase.Entry.Slot).Distinct().Skip(1).Any() && speakers.Select(phase => phase.Value).Distinct().Skip(1).Any();
            if (cues.Count == 0 && !exchange)
                continue;

            DirectorComponent current = ExpandDirectorComponent(handlers, arms, entryPhase, speakers.FirstOrDefault(), connected, cues);
            int firstOverlap;
            while ((firstOverlap = machines.FindIndex(machine => machine.Values.Overlaps(current.Values))) >= 0)
            {
                DirectorComponent prior = machines[firstOverlap];
                machines.RemoveAt(firstOverlap);
                connected.UnionWith(prior.Values);
                cues.UnionWith(prior.Cues);
                current = ExpandDirectorComponent(handlers, arms, prior.EntryPhase, prior.Speaker ?? current.Speaker, connected, cues);
            }
            machines.Add(current);
        }
        if (machines.Count < 2)
            return machines;

        // Different externally selected entrances can use the same dispatch callback.
        // Keep their paths disconnected in the graph and give them one physical director.
        var first = machines[0];
        var values = machines.SelectMany(machine => machine.Values).ToHashSet();
        var sharedCues = machines.SelectMany(machine => machine.Cues).ToHashSet();
        return [ExpandDirectorComponent(handlers, arms, first.EntryPhase, first.Speaker, values, sharedCues)];
    }

    private DirectorComponent ExpandDirectorComponent(Handler[] handlers, CutscenePhaseGraph.Phase[] arms, CutscenePhaseGraph.Phase entryPhase, CutscenePhaseGraph.Phase? speaker,
        HashSet<int> connected, HashSet<CutscenePhaseGraph.Cell> cues)
    {
        CutscenePhaseGraph.Phase[] component;
        int phaseCount;
        int cueCount;
        do
        {
            phaseCount = connected.Count;
            cueCount = cues.Count;
            component = arms.Where(phase => connected.Contains(phase.Value)).ToArray();
            if (cues.Count != 0)
                component = [.. component, .. FindGuardedRoles(handlers, arms, component, connected, entryPhase.Register, cues)];
            cues.UnionWith(ReadRepeatedRoleCues(component.Where(phase => phase.Entry == entryPhase.Entry), component.Where(phase => phase.Entry != entryPhase.Entry)));
            connected.UnionWith(ReadTransitions(component).Select(edge => edge.To));
        }
        while (connected.Count != phaseCount || cues.Count != cueCount);
        return new DirectorComponent(entryPhase, speaker, connected, component, cues);
    }

    private static IEnumerable<(int From, int To)> ReadTransitions(IEnumerable<CutscenePhaseGraph.Phase> phases) =>
        phases.SelectMany(phase => phase.Successors.Select(next => (From: phase.Value, To: next)));

    public void NameDirectorStorage(StorageDeclarations storage)
    {
        Scene[] candidates = Scenes.Where(scene => scene.DirectorMachine?.Cues.Count > 0).Take(2).ToArray();
        if (candidates is not [var scene])
            return;

        DirectorPhaseMachine machine = scene.DirectorMachine!;
        var selector = EVVariable.Decode(scene.Selectors[machine.Director]);
        var cues = machine.Cues.Where(cell => cell is { Scope: EVStorageScope.Map, Width: 1 }).ToArray();
        storage.NameCutsceneStorage(selector, cues is [var cue] ? cue.FirstBit : null);
    }

    private static IEnumerable<CutscenePhaseGraph.Phase> FindGuardedRoles(Handler[] handlers, CutscenePhaseGraph.Phase[] arms, CutscenePhaseGraph.Phase[] component,
        HashSet<int> connected, CutscenePhaseGraph.Cell register, HashSet<CutscenePhaseGraph.Cell> cues)
    {
        var roles = component.Select(phase => phase.Entry).ToHashSet();
        var known = arms.ToLookup(phase => phase.Entry);
        foreach (Handler handler in handlers.Where(handler => handler.Entry.Tag == 1 || roles.Contains(handler.Entry)))
        {
            foreach (var guard in CutscenePhaseGraph.ReadPhaseGuards(handler.Body, register, handler.Entry, handler.Owner.Type, connected, known[handler.Entry], roles.Contains(handler.Entry), cues))
                yield return guard;
        }
    }

    private static HashSet<CutscenePhaseGraph.Cell> ReadRepeatedRoleCues(IEnumerable<CutscenePhaseGraph.Phase> director, IEnumerable<CutscenePhaseGraph.Phase> roles)
    {
        var signals = roles.SelectMany(phase => phase.Writes().Where(write => write.Value == 1 && write.Cell != phase.Register)
            .Select(write => (phase.Entry, phase.Value, Cue: write.Cell))).ToLookup(signal => signal.Value);
        var uses = director.Where(phase => phase.Successors.Any()).SelectMany(phase =>
        {
            var reads = phase.Reads().ToHashSet();
            var resets = phase.Writes().Where(write => write.Value == 0).Select(write => write.Cell).ToHashSet();
            return signals[phase.Value].Where(signal => reads.Contains(signal.Cue) && resets.Contains(signal.Cue));
        });
        return uses.GroupBy(signal => signal.Cue).Where(group => group.Select(signal => signal.Value).Distinct().Skip(1).Any() &&
            group.Select(signal => signal.Entry).Distinct().Skip(1).Any()).Select(group => group.Key).ToHashSet();
    }

    private static int[] OrderDirectorPhases(CutscenePhaseGraph.Phase[] director, (int From, int To)[] transitions, HashSet<int> connected)
    {
        var next = transitions.ToLookup(edge => edge.From, edge => edge.To);
        var reached = new HashSet<int>();
        var order = new List<int>();
        var incoming = transitions.Where(edge => edge.From != edge.To).Select(edge => edge.To).ToHashSet();
        var starts = director.Select(phase => phase.Value).ToArray();
        foreach (int start in starts.Where(value => !incoming.Contains(value)).Concat(starts).Concat(connected.Order()))
        {
            var pending = new Stack<int>();
            pending.Push(start);
            while (pending.TryPop(out int value))
            {
                if (!connected.Contains(value) || !reached.Add(value))
                    continue;
                order.Add(value);
                foreach (int successor in next[value].Reverse())
                    pending.Push(successor);
            }
        }
        // A depth-first walk can visit a join before the other arm of the branch.
        // Keep a loop together and require all predecessors outside it to be printed first.
        var known = transitions.Where(edge => reached.Contains(edge.From) && reached.Contains(edge.To));
        return ReadChoreographyComponents(order.ToArray(), known).SelectMany(component => component.Values).ToArray();
    }

    private void ValidateDirectorOwnership(List<Scene> scenes)
    {
        foreach (Scene scene in scenes.Where(candidate => candidate.DirectorMachine != null))
        {
            var machine = scene.DirectorMachine!;
            if (scenes.Any(other => !ReferenceEquals(other, scene) && other.For(machine.Director) != null))
                throw DirectorPhaseError(machine.Director, null, "has more than one scene claiming its coordinating callback", machine.Director);
        }
    }

    private static bool CanEmitStandaloneDirector(Handler director, Handler[] handlers)
    {
        EVContainerObject owner = director.Owner;
        if (director.Entry is not { Slot: > 0, Tag: 1 } || director.Ordinal != 1 || owner.VariableCount != 0 || owner.Flags != 0 || owner.Reserved != 0 ||
            owner.BodyOrder != owner.Id || owner.EmptyOffset != 0 || owner.Gap.Length != 0 || owner.Prefix.Length != 0 || owner.Scripts.Length != 2 ||
            owner.Scripts[0].Id != 0 || handlers.Single(handler => handler.Entry == new Entry(owner.Id, 0)).Body.Instructions is not [{ Opcode: Jsm.Opcode.Return }])
            return false;

        bool spawned = false;
        foreach (EVInstruction step in handlers.SelectMany(handler => handler.Body.Instructions))
        {
            if (step.Opcode is Jsm.Opcode.NEW or Jsm.Opcode.NEW2 or Jsm.Opcode.NEW3)
            {
                if (step.Operands[0].Expression != null || step.Operands[1].Expression != null)
                    return false;
                if (step.Operands[0].Value == owner.Id)
                {
                    if (step.Opcode != Jsm.Opcode.NEW || step.Operands[1] is not { Expression: null, Value: 0 })
                        return false;
                    spawned = true;
                }
                else if (step.Operands[1].Value == owner.Id)
                    return false;
            }
            else if (IsRequest(step) && (step.Operands[1].Expression != null || step.Operands[1].Value == owner.Id))
                return false;
        }
        return spawned;
    }

    private static bool CanMoveSceneDirectorEntry(Handler director, Handler[] handlers) =>
        director.Entry is { Slot: 0, Tag: 1 } && director.Owner.Type == 0 &&
        handlers.SelectMany(handler => handler.Body.Instructions).All(step =>
            !IsRequest(step) || step.Operands[1] is not { Expression: null, Value: 0 } || step.Operands[2] is { Expression: null, Value: not 1 });

    private MethodDeclarationSyntax RewriteDirector(Scene scene, MethodDeclarationSyntax method, ParameterListSyntax parameter, bool standalone, RewriteBody rewriter, StorageDeclarations storage)
    {
        var key = scene.DirectorMachine!.Director;
        bool movesEntry = !standalone && scene.DirectorMachine.Choreography != null;
        var originalAttributes = SyntaxFactory.List(method.AttributeLists.Where(list =>
            list.Attributes.All(attribute => attribute.Name.ToString() == "SwitchLayout" || (standalone || movesEntry) && attribute.Name.ToString() == "TrailingPadding" ||
                movesEntry && attribute.Name.ToString() == "Script")));
        if (standalone)
        {
            var originalSlot = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("OriginalDirectorSlot"), SyntaxFactory.ParseAttributeArgumentList($"({key.Slot})"))));
            originalAttributes = originalAttributes.Insert(0, originalSlot);
        }
        var body = standalone ? method.Body! : (BlockSyntax)rewriter.Visit(method.Body!)!;
        if (standalone && body.DescendantNodes().Any(node => node is ThisExpressionSyntax ||
            node is IdentifierNameSyntax { Identifier.ValueText: "Local" or "Animation" or "Actions" }))
            throw DirectorPhaseError(key, null, "uses its original participant instance in the director routine");

        bool routine = method.ReturnType.ToString() == "EventRoutine";
        var source = method.WithIdentifier(SyntaxFactory.Identifier("Direct"))
            .WithParameterList(standalone ? SyntaxFactory.ParameterList() : parameter)
            .WithAttributeLists(originalAttributes).WithModifiers(Modifiers("internal", method)).WithBody(body);
        if (standalone && !routine)
        {
            source = source.WithReturnType(SyntaxFactory.IdentifierName("EventRoutine"))
                .WithModifiers(source.Modifiers.Add(SyntaxFactory.Token(SyntaxKind.AsyncKeyword)));
        }
        if (scene.DirectorMachine.Choreography is { } choreography)
            source = RewriteLinearDirector(scene, source, choreography, storage);
        scene.Methods.Add(source);
        foreach (int phase in scene.For(key)!.Keys.Where(phase => !scene.ExtractedChapters.Contains((key, phase))))
            MarkExtractedPhases(scene, key, [phase], "Direct");
        string call = $"ctx.{scene.Name}.Direct(this);";
        return method.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement(routine ? $"return {call}" : call)));
    }

    public void ValidateDirectorMachines()
    {
        foreach (Scene scene in Scenes.Where(scene => scene.DirectorMachine != null))
        {
            DirectorPhaseMachine machine = scene.DirectorMachine!;
            var methods = scene.Methods.ToLookup(method => method.Identifier.ValueText, StringComparer.Ordinal);
            var ordered = new List<MethodDeclarationSyntax>();
            if (machine.Choreography?.UsesRouteMethods == true)
            {
                ordered.Add(methods["Direct"].Single());
                ordered.AddRange(scene.Methods.Where(IsChoreographyRoute));
            }
            foreach (var (entry, phase) in machine.OrderedChapters)
            {
                string name = scene.ChapterMethod(entry, phase);
                if (!scene.ExtractedChapters.Contains((entry, phase)) || methods[name].ToArray() is not [var method])
                    throw DirectorPhaseError(entry, phase, "could not be emitted without losing its control flow", machine.Director);
                if (!ordered.Contains(method))
                    ordered.Add(method);
            }
            scene.Methods.Clear();
            scene.Methods.AddRange(ordered.Concat(methods.SelectMany(group => group).Except(ordered)));
        }
    }

    private InvalidDataException DirectorPhaseError(Entry entry, int? phase, string reason, Entry? director = null) =>
        new($"{_eventName}, director phase machine{(director is { } owner ? $" {owner.Slot}/{owner.Tag}" : "")}, slot {entry.Slot}, tag {entry.Tag}, phase {phase?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "entry"}: {reason}.");

    private static string PhaseIdentifier(int phase) => phase < 0 ? $"Minus{-(long)phase}" : phase.ToString(System.Globalization.CultureInfo.InvariantCulture);
}
