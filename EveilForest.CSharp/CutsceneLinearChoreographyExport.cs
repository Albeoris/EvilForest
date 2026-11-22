using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    internal sealed record ChoreographyRoute(IReadOnlyList<int> Phases, int? Continuation, bool Entrance, bool Repeats = false);
    internal sealed record ChoreographyChoice(EVExpressionToken Selector, IReadOnlyList<(int Key, int Destination)> Cases);
    private sealed record PollingArm(EVInstruction[] Steps, int Exit, int? UnreachableExit, int? FallThrough);
    internal sealed record LinearChoreography(EVExpressionToken Selector, EVExpressionToken Cue, EVExpressionToken Pulse,
        IReadOnlyList<ChoreographyRoute> Routes, IReadOnlySet<int> IdlePhases, IReadOnlyDictionary<int, ChoreographyChoice> Choices,
        IReadOnlyDictionary<(Entry Entry, int Phase), int> UnreachablePollingExits, IReadOnlySet<Entry> ParticipantPollWork,
        IReadOnlySet<Entry> ConditionalParticipants, IReadOnlySet<Entry> ParticipantFallbacks, bool DirectorPollWork,
        IReadOnlyDictionary<int, int> FallThrough, IReadOnlyList<int> PhysicalCaseOrder,
        IReadOnlyDictionary<Entry, Dictionary<int, int>> RoleContinuations, IReadOnlySet<Entry> RolePollJumps,
        IReadOnlyDictionary<Entry, int> LinkedPollingDispatches)
    {
        public bool UsesRouteMethods => Routes.Count > 1 || Choices.Count != 0 || Routes.Any(route => route.Repeats);
    }

    private static (LinearChoreography? Choreography, string? Reason) ReadLinearChoreography(Handler director, CutscenePhaseGraph.Phase[] component,
        HashSet<CutscenePhaseGraph.Cell> cues, ILookup<Entry, Handler> handlers, bool canBindDirector)
    {
        if (!canBindDirector)
            return (null, "The coordinator requires its original participant entry.");
        if (cues.ToArray() is not [{ Scope: EVStorageScope.Map, Width: 1 } cue])
            return (null, "The coordinator does not use one map signal.");

        var directorPhases = component.Where(phase => phase.Entry == director.Entry).ToArray();
        EVExpressionToken selector = directorPhases[0].Selector;
        CutscenePhaseGraph.Cell register = directorPhases[0].Register;
        if (!TryReadDirectorPollingArms(director, selector, out var directorArms, out bool hasPulse, out int prefixLength))
            return (null, "The coordinator has additional dispatch or polling work.");
        if (!directorArms.Keys.ToHashSet().SetEquals(directorPhases.Select(phase => phase.Value)))
            return (null, "The coordinator dispatches phases outside the recognized path.");
        EVExpressionToken pulseToken = default;
        if (hasPulse)
        {
            if (!TryReadChoreographyWrite(director.Body.Instructions[prefixLength], 0, out pulseToken) || !EVVariable.Decode(pulseToken).UsesBitAddress)
                return (null, "The coordinator performs other work before each dispatch.");
            var pulse = CutscenePhaseGraph.Cell.Of(EVVariable.Decode(pulseToken), director.Entry.Slot);
            if (pulse.Scope != EVStorageScope.Map || pulse.Overlaps(cue) || pulse.Overlaps(register))
                return (null, "The polling pulse is not a separate map bit.");
        }

        var successors = new Dictionary<int, int>();
        var choices = new Dictionary<int, ChoreographyChoice>();
        var fallsThrough = directorArms.Where(arm => arm.Value.FallThrough != null).ToDictionary(arm => arm.Key, arm => arm.Value.FallThrough!.Value);
        var linkedPollingDispatches = new Dictionary<Entry, int>();
        if (fallsThrough.Count != 0)
            linkedPollingDispatches.Add(director.Entry, director.Body.Instructions[prefixLength + (hasPulse ? 1 : 0)].Label);
        EVExpressionToken? cueToken = null;
        foreach (var phase in directorPhases)
        {
            int[] next = phase.Successors.ToArray();
            if (next.Length == 0)
                continue;
            var (steps, exit, _, _) = directorArms[phase.Value];
            if (fallsThrough.ContainsKey(phase.Value))
                return (null, $"Coordinator phase {phase.Value} changes the selector before entering another phase's work.");
            if (steps is not [var condition, var branch, ..] ||
                condition.Opcode != Jsm.Opcode.EXPR || condition.Expression is not [var read, var expected, { Opcode: (byte)Jsm.Excode.Equivalence }] ||
                !StorageDeclarations.IsStorage(read) || CutscenePhaseGraph.Cell.Of(EVVariable.Decode(read), director.Entry.Slot) != cue ||
                !expected.Equals(EVExpressionToken.Literal(1)) || branch.Opcode != Jsm.Opcode.JMP_IFN ||
                branch.Targets is not [var skip] ||
                cueToken is { } prior && !prior.Equals(read))
                return (null, $"Coordinator phase {phase.Value} has other work or a different transition condition.");

            bool exitsThroughJump = IsChoreographyJump(steps[^1], exit);
            int end = steps.Length - (exitsThroughJump ? 1 : 0);
            if (end < 4 || skip != (exitsThroughJump ? steps[^1].Label : exit) ||
                !TryReadChoreographyWrite(steps[end - 1], 0, out var writtenCue) || !writtenCue.Equals(read))
                return (null, $"Coordinator phase {phase.Value} has other work or a different transition condition.");

            if (next is [var destination] && end == 4 && TryReadChoreographyWrite(steps[2], destination, out var writtenSelector) && writtenSelector.Equals(selector))
                successors.Add(phase.Value, destination);
            else if (ReadChoreographyChoice(steps[2..end], selector, read, pulseToken, director.Entry, next) is { } choice)
                choices.Add(phase.Value, choice);
            else
                return (null, $"Coordinator phase {phase.Value} has other work or a different transition condition.");
            cueToken = read;
        }

        var idle = successors.Values.Concat(choices.Values.SelectMany(choice => choice.Cases.Select(option => option.Destination)))
            .Where(value => !directorArms.ContainsKey(value)).ToHashSet();
        // A participant may enter the next phase itself. The director then takes its
        // default one-frame poll, while that phase's actor roles still run normally.
        idle.UnionWith(component.Where(phase => phase.Entry != director.Entry && !directorArms.ContainsKey(phase.Value)).Select(phase => phase.Value));
        int[] values = directorArms.Keys.Concat(idle).ToArray();
        if (!SceneChoreography.HasValidFallThrough(fallsThrough, values.ToHashSet()))
            return (null, "The coordinator has a same-tick cycle between phase arms.");
        var routes = ReadChoreographyRoutes(values, successors, choices);
        if (routes == null && choices.Count != 0)
            routes = ReadChoreographyRoutes(values, successors, choices, cutChoiceEdges: true);
        if (routes == null)
            routes = ReadRepeatingChoreographyRoutes(values, successors, choices);
        if (routes == null)
            return (null, "The coordinator has cyclic phase transitions.");

        var roles = component.Where(phase => phase.Entry != director.Entry).ToLookup(phase => phase.Entry);
        var roleContinuations = new Dictionary<Entry, Dictionary<int, int>>();
        var rolePollJumps = new HashSet<Entry>();
        var unreachablePollingExits = new Dictionary<(Entry Entry, int Phase), int>();
        var participantPollWork = new HashSet<Entry>();
        var conditionalParticipants = new HashSet<Entry>();
        var participantFallbacks = new HashSet<Entry>();
        foreach (var role in roles)
        {
            if (role.FirstOrDefault(phase => phase.Successors.Any(value => !directorArms.ContainsKey(value) && !idle.Contains(value))) is { } transition)
                return (null, $"Participant {role.Key.Slot}/{role.Key.Tag} changes the selector to an undeclared phase from phase {transition.Value}.");
            string participant = $"Participant {role.Key.Slot}/{role.Key.Tag}";
            // Requests and interactions run on their own VM activation. Their
            // callback is bound directly to the scene, never scheduled as a polling role.
            if (role.Key.Tag > 1 && role.All(phase => phase.Selector.Equals(selector)))
                continue;
            if (role.Key.Tag != 1 || role.Any(phase => !phase.Selector.Equals(selector)))
                return (null, $"{participant} uses an additional handler or phase selector.");
            Handler handler = handlers[role.Key].Single();
            if (role.ToArray() is [var guarded] && IsConditionalParticipant(handler, guarded))
            {
                conditionalParticipants.Add(role.Key);
                continue;
            }
            if (role.Any(phase => phase.GuardConditionLabel != null))
                return (null, $"{participant} has multiple shared dispatch sites.");
            if (!TryReadParticipantPollingPrefix(handler, selector, out int rolePrefixLength) ||
                !TryReadPollingArms(handler, selector, hasPulse: false, out var arms, participantLoop: true, prefixLength: rolePrefixLength))
                return (null, $"{participant} has control flow that leaves the shared phase arms.");
            if (!arms.Keys.ToHashSet().SetEquals(role.Select(phase => phase.Value)))
                return (null, $"{participant} dispatches phases outside the choreography: {string.Join(", ", arms.Keys.Except(role.Select(phase => phase.Value)).Order())}.");
            var continuations = arms.Where(arm => arm.Value.FallThrough != null).ToDictionary(arm => arm.Key, arm => arm.Value.FallThrough!.Value);
            if (continuations.Count != 0)
            {
                bool hasFallback = handler.Body.Instructions[rolePrefixLength + 1].Targets[0] != arms.Values.First().Exit;
                if (rolePrefixLength != 0 || hasFallback || arms.Values.Distinct().Count() != arms.Count || arms.Values.Any(arm => arm.UnreachableExit != null) ||
                    !SceneChoreography.HasValidFallThrough(continuations, arms.Keys.ToHashSet()))
                    return (null, $"{participant} combines linked roles with another polling shape.");
                roleContinuations.Add(role.Key, continuations);
                linkedPollingDispatches.Add(role.Key, handler.Body.Instructions[rolePrefixLength].Label);
                PollingArm lastArm = arms.Values.MaxBy(arm => arm.Steps[0].Label)!;
                if (IsChoreographyJump(lastArm.Steps[^1], lastArm.Exit))
                    rolePollJumps.Add(role.Key);
            }
            if (rolePrefixLength != 0)
                participantPollWork.Add(role.Key);
            if (handler.Body.Instructions[rolePrefixLength + 1].Targets[0] != arms.Values.First().Exit)
                participantFallbacks.Add(role.Key);
            foreach (var (phase, arm) in arms)
            {
                if (arm.UnreachableExit is int label)
                    unreachablePollingExits.Add((role.Key, phase), label);
            }
        }
        // The coordinator polls the cue even when its current roles never write it. A
        // different script or a role still running from an earlier phase can change it.
        if (cueToken is not { } signal)
            return (null, "The coordinator has no cue-driven transition.");
        int[] physicalOrder = directorArms.OrderBy(arm => arm.Value.Steps[0].Label).Select(arm => arm.Key).ToArray();
        if (fallsThrough.Count != 0 && IsChoreographyJump(directorArms[physicalOrder[^1]].Steps[^1], directorArms[physicalOrder[^1]].Exit))
            return (null, "The coordinator's closing arm uses an additional branch to its polling wait.");
        return (new LinearChoreography(selector, signal, pulseToken, routes, idle, choices, unreachablePollingExits, participantPollWork, conditionalParticipants, participantFallbacks, prefixLength != 0, fallsThrough, physicalOrder, roleContinuations, rolePollJumps, linkedPollingDispatches), null);
    }

    private static bool IsConditionalParticipant(Handler handler, CutscenePhaseGraph.Phase phase)
    {
        if (phase.GuardConditionLabel is not int label)
            return false;
        var steps = handler.Body.Instructions;
        int index = steps.TakeWhile(step => step.Label != label).Count();
        return index + 2 < steps.Count &&
            steps[index] is { Opcode: Jsm.Opcode.EXPR, Expression: [var selector, var value, { Opcode: (byte)Jsm.Excode.Equivalence }] } &&
            selector.Equals(phase.Selector) && value.Equals(EVExpressionToken.Literal(phase.Value)) &&
            steps[index + 1] is { Opcode: Jsm.Opcode.JMP_IFN } && steps[index + 2].Label == phase.Start;
    }

    private static bool TryReadDirectorPollingArms(Handler handler, EVExpressionToken selector, out Dictionary<int, PollingArm> arms, out bool hasPulse, out int prefixLength)
    {
        prefixLength = 0;
        hasPulse = TryReadPollingArms(handler, selector, hasPulse: true, out arms);
        if (hasPulse || TryReadPollingArms(handler, selector, hasPulse: false, out arms))
            return true;
        if (!TryReadParticipantPollingPrefix(handler, selector, out int selectorIndex) || selectorIndex < 2 ||
            handler.Body.Instructions.Take(selectorIndex - 1).Any(step => step.Opcode != Jsm.Opcode.EXPR) ||
            !TryReadPollingArms(handler, selector, hasPulse: true, out arms, prefixLength: selectorIndex - 1))
            return false;
        hasPulse = true;
        prefixLength = selectorIndex - 1;
        return true;
    }

    private static ChoreographyChoice? ReadChoreographyChoice(EVInstruction[] steps, EVExpressionToken selector, EVExpressionToken cue, EVExpressionToken pulse, Entry entry, int[] destinations)
    {
        if (steps is not [{ Opcode: Jsm.Opcode.EXPR, Expression: [var read] }, var table, .., var reset] ||
            !StorageDeclarations.IsStorage(read) || EVVariable.Decode(read) is not { Scope: EVStorageScope.Map, UsesBitAddress: false } view ||
            table.Targets is not [var fallback, ..] || fallback != reset.Label)
            return null;

        var cell = CutscenePhaseGraph.Cell.Of(view, entry.Slot);
        if (new[] { selector, cue, pulse }.Where(StorageDeclarations.IsStorage).Any(token => cell.Overlaps(CutscenePhaseGraph.Cell.Of(EVVariable.Decode(token), entry.Slot))))
            return null;

        int[] keys = ReadChoreographySwitchKeys(table);
        if (keys.Length == 0 || keys.Length != table.Targets.Length - 1 || keys.Distinct().Count() != keys.Length)
            return null;

        var cases = keys.Zip(table.Targets.Skip(1)).Where(item => item.Second != fallback).OrderBy(item => item.Second).ToArray();
        // Dense tables may fill unused keys with the default target. Each authored case has its own body.
        if (cases.Length == 0 || table.Opcode == Jsm.Opcode.JMP_SWITCHEX && cases.Length != keys.Length ||
            cases.Select(option => option.Second).Distinct().Count() != cases.Length)
            return null;
        // An explicit VM table discards the selector's high bits before comparing its keys.
        if (table.Opcode == Jsm.Opcode.JMP_SWITCHEX && view.Width > 16 ||
            ChoreographyChoiceTable.Infer(cases.Select(option => option.First).ToArray(), (StorageType)view.Type) == null)
            return null;

        // Every case encloses one phase assignment, then reaches the unconditional cue reset.
        var options = new List<(int Key, int Destination)>();
        int index = 2;
        foreach (var (key, target) in cases)
        {
            if (index >= steps.Length - 1 || steps[index].Label != target)
                return null;
            if (steps[index] is not { Opcode: Jsm.Opcode.EXPR, Expression: [var written, var literal, { Opcode: (byte)Jsm.Excode.Let }] } ||
                !written.Equals(selector) || !CutscenePhaseGraph.TryReadConstant(literal, out int next) || !literal.Equals(EVExpressionToken.Literal(next)))
                return null;
            options.Add((key, next));
            index++;
            if (index < steps.Length - 1 && IsChoreographyJump(steps[index], reset.Label))
                index++;
            else if (index != steps.Length - 1)
                return null;
        }
        if (index != steps.Length - 1 || !destinations.ToHashSet().SetEquals(options.Select(option => option.Destination)))
            return null;
        return new ChoreographyChoice(read, options);
    }

    private static int[] ReadChoreographySwitchKeys(EVInstruction table)
    {
        if (table.CaseValues.Length == 0)
            return [];
        return table.Opcode switch
        {
            Jsm.Opcode.JMP_SWITCHEX => table.CaseValues.Select(value => (int)value).ToArray(),
            Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.rsv0d => Enumerable.Range(unchecked((short)table.CaseValues[0]), table.Targets.Length - 1).ToArray(),
            _ => []
        };
    }

    private static bool IsChoreographyJump(EVInstruction step, int target) => step.Opcode == Jsm.Opcode.JMP && step.Targets is [var jump] && jump == target;

    private static List<ChoreographyRoute>? ReadRepeatingChoreographyRoutes(int[] values, IReadOnlyDictionary<int, int> successors, IReadOnlyDictionary<int, ChoreographyChoice> choices)
    {
        var repeats = new Dictionary<int, int>();
        foreach (var component in ReadChoreographyComponents(values, ChoreographyEdges(successors, new Dictionary<int, int[]>())))
        {
            int first = component.Values[0];
            foreach (int phase in component.Values)
            {
                if (successors.TryGetValue(phase, out int next) && next == first)
                    repeats.Add(phase, first);
            }
        }
        return repeats.Count == 0 ? null : ReadChoreographyRoutes(values, successors, choices, cutChoiceEdges: true, repeats);
    }

    private static List<ChoreographyRoute>? ReadChoreographyRoutes(int[] values, IReadOnlyDictionary<int, int> successors, IReadOnlyDictionary<int, ChoreographyChoice> choices,
        bool cutChoiceEdges = false, IReadOnlyDictionary<int, int>? repeats = null)
    {
        var branches = choices.ToDictionary(pair => pair.Key, pair => pair.Value.Cases.Select(option => option.Destination).Distinct().ToArray());
        var incoming = successors.Where(edge => repeats?.ContainsKey(edge.Key) != true).Select(edge => edge.Value)
            .Concat(cutChoiceEdges ? [] : branches.Values.SelectMany(destinations => destinations)).ToLookup(value => value);
        // A source cycle needs one route to declare it. The selector still determines the phase on every tick.
        var entrances = cutChoiceEdges
            ? ReadChoreographyComponents(values, ChoreographyEdges(successors, branches)).Where(component => component.Entrance).Select(component => component.Values[0]).ToHashSet()
            : values.Where(value => !incoming.Contains(value)).ToHashSet();
        var routeStarts = cutChoiceEdges ? branches.Values.SelectMany(destinations => destinations).Concat(entrances).Concat(repeats?.Values ?? []).ToHashSet() : new HashSet<int>();
        var remaining = values.ToDictionary(value => value, value => incoming[value].Count());
        var pending = new Queue<int>(remaining.Keys.Where(value => remaining[value] == 0));
        var routes = new List<ChoreographyRoute>();
        var visited = new HashSet<int>();
        // End a route at each choice or join. Emit a destination after all of its predecessors.
        while (pending.TryDequeue(out int first))
        {
            var phases = new List<int> { first };
            int? continuation = null;
            bool repeat = false;
            while (successors.TryGetValue(phases[^1], out int next))
            {
                if (repeats?.ContainsKey(phases[^1]) == true)
                {
                    continuation = next;
                    repeat = true;
                    break;
                }
                if (incoming[next].Count() == 1 && !routeStarts.Contains(next))
                {
                    phases.Add(next);
                    continue;
                }
                continuation = next;
                if (--remaining[next] == 0)
                    pending.Enqueue(next);
                break;
            }
            if (!cutChoiceEdges && branches.TryGetValue(phases[^1], out var destinations))
            {
                foreach (int destination in destinations)
                {
                    if (--remaining[destination] == 0)
                        pending.Enqueue(destination);
                }
            }
            routes.Add(new ChoreographyRoute(phases, continuation, entrances.Contains(first), repeat));
            foreach (int phase in phases)
            {
                if (!visited.Add(phase))
                    return null;
            }
        }
        if (visited.Count != remaining.Count)
            return null;
        if (!cutChoiceEdges)
            return routes;

        // Keep shared continuations after their callers unless they are part of the same cycle.
        var byStart = routes.ToDictionary(route => route.Phases[0]);
        var continuations = routes.Where(route => route.Continuation.HasValue).ToDictionary(route => route.Phases[0], route => route.Continuation!.Value);
        var routeChoices = routes.Where(route => branches.ContainsKey(route.Phases[^1])).ToDictionary(route => route.Phases[0], route => branches[route.Phases[^1]]);
        return ReadChoreographyComponents(values.Where(byStart.ContainsKey).ToArray(), ChoreographyEdges(continuations, routeChoices))
            .SelectMany(component => component.Values).Select(start => byStart[start]).ToList();
    }

    private static IEnumerable<(int From, int To)> ChoreographyEdges(IReadOnlyDictionary<int, int> successors, IReadOnlyDictionary<int, int[]> choices) =>
        successors.Select(edge => (From: edge.Key, To: edge.Value))
            .Concat(choices.SelectMany(choice => choice.Value.Select(destination => (From: choice.Key, To: destination))));

    private static IEnumerable<(int[] Values, bool Entrance)> ReadChoreographyComponents(int[] values, IEnumerable<(int From, int To)> edges)
    {
        var transitions = edges.ToArray();
        var outgoing = transitions.ToLookup(edge => edge.From, edge => edge.To);
        var incoming = transitions.ToLookup(edge => edge.To, edge => edge.From);
        var remaining = values.ToHashSet();
        var components = new List<(int[] Values, HashSet<int> Predecessors)>();
        foreach (int phase in values)
        {
            if (!remaining.Contains(phase))
                continue;
            var component = Walk(phase, outgoing);
            component.IntersectWith(Walk(phase, incoming));
            remaining.ExceptWith(component);
            components.Add((values.Where(component.Contains).ToArray(), component.SelectMany(value => incoming[value]).Where(value => !component.Contains(value)).ToHashSet()));
        }
        var emitted = new HashSet<int>();
        while (components.Count > 0)
        {
            int index = components.FindIndex(component => component.Predecessors.IsSubsetOf(emitted));
            var component = components[index];
            yield return (component.Values, component.Predecessors.Count == 0);
            emitted.UnionWith(component.Values);
            components.RemoveAt(index);
        }

        static HashSet<int> Walk(int start, ILookup<int, int> edges)
        {
            var reached = new HashSet<int>();
            var pending = new Stack<int>();
            pending.Push(start);
            while (pending.TryPop(out int value))
            {
                if (!reached.Add(value))
                    continue;
                foreach (int next in edges[value])
                    pending.Push(next);
            }
            return reached;
        }
    }

    private static bool TryReadParticipantPollingPrefix(Handler handler, EVExpressionToken selector, out int prefixLength)
    {
        var steps = handler.Body.Instructions;
        prefixLength = 0;
        while (prefixLength + 1 < steps.Count && (steps[prefixLength] is not { Opcode: Jsm.Opcode.EXPR, Expression: [var token] } ||
            !token.Equals(selector) || ReadChoreographySwitchKeys(steps[prefixLength + 1]).Length == 0))
            prefixLength++;
        if (prefixLength + 1 >= steps.Count)
            return false;
        int dispatch = steps[prefixLength].Label;
        return steps.Take(prefixLength).All(step => step.Opcode != Jsm.Opcode.Return && step.Targets.All(target => target <= dispatch));
    }

    private static bool TryReadPollingArms(Handler handler, EVExpressionToken selector, bool hasPulse, out Dictionary<int, PollingArm> arms, bool participantLoop = false, int prefixLength = 0)
    {
        arms = new();
        EVInstruction[] steps = handler.Body.Instructions.Reverse().SkipWhile(step => step.Opcode == Jsm.Opcode.NOP).Reverse().ToArray();
        int switchIndex = prefixLength + (hasPulse ? 2 : 1);
        if (steps.Length <= switchIndex + 3 || steps[switchIndex - 1] is not { Opcode: Jsm.Opcode.EXPR, Expression: [var selected] } || !selected.Equals(selector) ||
            steps[^2] is not { Opcode: Jsm.Opcode.WAIT, ArgumentMask: 0, Operands: [{ Expression: null, Value: 1 }] } ||
            steps[^1] is not { Opcode: Jsm.Opcode.JMP, Targets: [var restart] } || restart != steps[0].Label)
            return false;
        EVInstruction table = steps[switchIndex];
        int exit = steps[^2].Label;
        if (table.Targets is not [var fallback, ..] || fallback != exit && !participantLoop || table.CaseValues.Length == 0)
            return false;
        int[] values = ReadChoreographySwitchKeys(table);
        var cases = values.Zip(table.Targets.Skip(1)).Where(item => item.Second != fallback && item.Second != exit).ToLookup(item => item.Second, item => item.First);
        int[] starts = cases.Select(group => group.Key).Concat(fallback == exit ? [] : [fallback]).Order().ToArray();
        if (values.Length != table.Targets.Length - 1 || cases.Count == 0 || starts[0] != steps[switchIndex + 1].Label ||
            fallback != exit && table.Targets.Skip(1).Contains(exit) ||
            !participantLoop && cases.Any(group => group.Skip(1).Any()))
            return false;

        var index = steps.Select((step, position) => (step.Label, position)).ToDictionary(item => item.Label, item => item.position);
        var edges = handler.Body.GetSuccessors();
        int start = switchIndex + 1;
        for (int arm = 0; arm < starts.Length; arm++)
        {
            int nextLabel = arm + 1 == starts.Length ? exit : starts[arm + 1];
            if (!index.TryGetValue(nextLabel, out int end) || end <= start || end > steps.Length - 2)
                return false;
            var region = steps[start..end];
            var labels = region.Select(step => step.Label).ToHashSet();
            EVInstruction last = region[^1];
            // A self-deletion or an always-taken internal branch prevents execution from reaching the next arm.
            bool unreachableExit = participantLoop && starts[arm] != fallback && nextLabel != exit && (CutscenePhaseGraph.IsImmediateSelfDelete(last) ||
                region.Length > 1 && CutscenePhaseGraph.TakesLiteralBranch(region[^2], last) &&
                last.Targets is [var target] && labels.Contains(target) && !region.Any(step => step.Targets.Contains(last.Label)));
            bool fallsThrough = !unreachableExit && nextLabel != exit && last.Targets.Length == 0 && edges[last.Label] is [var next] && next == nextLabel &&
                (!participantLoop || starts[arm] != fallback && cases[nextLabel].Take(2).Count() == 1);
            int? linkedLabel = fallsThrough ? nextLabel : null;
            if (last.Opcode == Jsm.Opcode.JMP && last.Targets is [var destination] && destination != nextLabel &&
                !labels.Contains(destination) && cases[destination].Take(2).Count() == 1)
                linkedLabel = destination;
            int boundary = unreachableExit ? nextLabel : linkedLabel ?? exit;
            foreach (EVInstruction step in region)
            {
                if (step.Opcode == Jsm.Opcode.Return)
                    return false;
                foreach (int successor in edges[step.Label])
                {
                    if (labels.Contains(successor) || step == last && successor == boundary)
                        continue;
                    // A role can return early to its own one-tick poll. Other exits may enter another role.
                    if (participantLoop && linkedLabel == null && successor == exit && step.Targets.Contains(successor))
                        continue;
                    if (participantLoop && successor == linkedLabel && step.Targets.Contains(successor))
                        continue;
                    return false;
                }
            }
            var pollingArm = new PollingArm(region, exit, unreachableExit ? last.Label : null, linkedLabel is int linked ? cases[linked].Single() : null);
            if (!edges[last.Label].Contains(boundary))
                return false;
            foreach (int value in cases[starts[arm]])
            {
                if (!arms.TryAdd(value, pollingArm))
                    return false;
            }
            start = end;
        }
        return true;
    }

    private static bool TryReadChoreographyWrite(EVInstruction step, int value, out EVExpressionToken variable)
    {
        if (step.Opcode == Jsm.Opcode.EXPR && step.Expression is [var target, var literal, { Opcode: (byte)Jsm.Excode.Let }] &&
            StorageDeclarations.IsStorage(target) && literal.Equals(EVExpressionToken.Literal(value)))
        {
            variable = target;
            return true;
        }
        variable = default;
        return false;
    }

    private MethodDeclarationSyntax RewriteLinearDirector(Scene scene, MethodDeclarationSyntax director, LinearChoreography choreography, StorageDeclarations storage)
    {
        Entry entry = scene.DirectorMachine!.Director;
        string selector = storage.Qualify(choreography.Selector);
        string cue = storage.Qualify(choreography.Cue);
        string? pulse = StorageDeclarations.IsStorage(choreography.Pulse) ? storage.Qualify(choreography.Pulse) : null;
        var directorPhases = choreography.Routes.SelectMany(route => route.Phases).Where(phase => !choreography.IdlePhases.Contains(phase));
        if (!TryReadSourcePolling(director, selector, pulse, out var cases, out var dispatch, choreography.DirectorPollWork, choreography.FallThrough) || !cases.Keys.ToHashSet().SetEquals(directorPhases))
            throw DirectorPhaseError(entry, null, "could not extract the proven choreography coordinator loop", entry);
        if (choreography.FallThrough.Count != 0)
        {
            string sourceKey = SwitchLayout.Key([choreography.Selector], dispatch!.Sections.Select(section => SwitchLayout.Section(
                section.Labels.OfType<CaseSwitchLabelSyntax>().Select(label => PhaseValue(label)!.Value), section.Labels is [DefaultSwitchLabelSyntax], false)), directDispatch: true);
            var layout = director.AttributeLists.SelectMany(list => list.Attributes).SingleOrDefault(attribute =>
                attribute.Name.ToString() == "SwitchLayout" && attribute.ArgumentList!.Arguments[0].Expression is LiteralExpressionSyntax key &&
                key.Token.ValueText == sourceKey);
            if (layout == null)
                throw DirectorPhaseError(entry, null, "could not match the coordinator's original switch layout", entry);
            ExpressionSyntax originalOrder = layout.ArgumentList!.Arguments[0].Expression;
            string[] sections = choreography.PhysicalCaseOrder.Select(phase => SwitchLayout.Section([phase], false, false)).Append("DB").ToArray();
            director = director.ReplaceNode(originalOrder, SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Literal(SwitchLayout.Key([choreography.Selector], sections, directDispatch: true))));
        }

        var names = Scenes.Where(candidate => candidate.Name == scene.Name)
            .SelectMany(candidate => candidate.Chapters.Values.Concat(candidate.AdditionalChapters.Values.SelectMany(chapters => chapters.Values)))
            .ToHashSet(StringComparer.Ordinal);
        names.UnionWith([scene.Name, "Direct"]);
        string pulseArgument = pulse == null ? "" : $", () => {pulse}";
        var setup = new List<string> { $"var play = Choreography(() => {selector}, () => {cue}{pulseArgument});" };
        if (choreography.DirectorPollWork)
        {
            var loop = (BlockSyntax)dispatch!.Parent!;
            StatementSyntax[] work = loop.Statements.Take(loop.Statements.IndexOf(dispatch) - 1).ToArray();
            if (work.Length == 0 || work.Any(statement => statement is not ExpressionStatementSyntax) || !IsClosedFragment(director, work, loop))
                throw DirectorPhaseError(entry, null, "could not extract the proven coordinator work before polling", entry);
            string name = ReserveName("BeforePoll", names);
            scene.Methods.Add(director.WithIdentifier(SyntaxFactory.Identifier(name)).WithAttributeLists(default)
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))).WithParameterList(SyntaxFactory.ParameterList())
                .WithReturnType(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword))).WithBody(PhaseBody(director, work, null)));
            setup.Add($"play.BeforePoll({name});");
        }
        if (!choreography.UsesRouteMethods)
        {
            var lines = setup.Prepend("var evt = ctx.Scene;").Concat(ReadRoute(choreography.Routes.Single(), new Dictionary<int, string>()));
            return director.WithBody(SyntaxFactory.Block(lines.Select(line => SyntaxFactory.ParseStatement(line))));
        }

        var choiceDestinations = choreography.Choices.Values.SelectMany(choice => choice.Cases.Select(option => option.Destination)).ToHashSet();
        var routeNames = choreography.Routes.ToDictionary(route => route.Phases[0], route =>
            ReserveName($"{(route.Entrance || choiceDestinations.Contains(route.Phases[0]) ? "FromPhase" : "ContinueFromPhase")}{PhaseIdentifier(route.Phases[0])}", names));
        foreach (var route in choreography.Routes)
        {
            string name = routeNames[route.Phases[0]];
            var lines = new List<string>();
            if (scene.DirectorMachine.OrderedChapters.Any(chapter => chapter.Entry != entry && route.Phases.Contains(chapter.Phase)))
                lines.Add("var evt = ctx.Scene;");
            lines.AddRange(ReadRoute(route, routeNames));
            if (route.Continuation is int continuation)
                lines.Add($"await play.{(route.Repeats ? "RepeatFrom" : "ContinueWith")}({routeNames[continuation]});");
            scene.Methods.Add(director.WithIdentifier(SyntaxFactory.Identifier(name)).WithAttributeLists(default)
                .WithModifiers(Modifiers("private", director)).WithParameterList(SyntaxFactory.ParseParameterList("(SceneChoreography play)"))
                .WithBody(SyntaxFactory.Block(lines.Select(line => SyntaxFactory.ParseStatement(line)))));
            MarkExtractedPhases(scene, entry, route.Phases.Where(phase => cases.ContainsKey(phase) && !scene.ExtractedChapters.Contains((entry, phase))).ToArray(), name);
        }
        string entrances = string.Join(", ", choreography.Routes.Where(route => route.Entrance).Select(route => routeNames[route.Phases[0]]));
        return director.WithBody(SyntaxFactory.Block(setup.Append($"await play.Routes({entrances});").Select(line => SyntaxFactory.ParseStatement(line))));

        IEnumerable<string> ReadRoute(ChoreographyRoute route, IReadOnlyDictionary<int, string> destinations)
        {
            for (int index = 0; index < route.Phases.Count; index++)
            {
                int phase = route.Phases[index];
                int? destination = index + 1 < route.Phases.Count ? route.Phases[index + 1] : route.Continuation;
                if (choreography.Choices.TryGetValue(phase, out var choice))
                {
                    string choiceSelector = storage.Qualify(choice.Selector);
                    string branches = string.Concat(choice.Cases.Select(option => $"case {option.Key}: {selector} = {option.Destination}; break;"));
                    StatementSyntax transition = SyntaxFactory.ParseStatement($"if ({cue} == true) {{ switch ({choiceSelector}) {{ {branches} }} {cue} = false; }}");
                    if (cases[phase].Statements is not [var gate, BreakStatementSyntax] || !SyntaxFactory.AreEquivalent(gate, transition))
                        throw DirectorPhaseError(entry, phase, "could not extract the proven choreography choice", entry);
                    yield return $"await play.RunUntilCue({phase}{RoleArguments(phase)});";
                    string casesSource = string.Join(", ", choice.Cases.Select(option => $"play.Case({option.Key}, {destinations[option.Destination]})"));
                    yield return $"await play.Choose(() => {choiceSelector}, {casesSource});";
                }
                else if (destination is int next)
                {
                    StatementSyntax transition = SyntaxFactory.ParseStatement($"if ({cue} == true) {{ {selector} = {next}; {cue} = false; }}");
                    if (cases[phase].Statements is not [var gate, BreakStatementSyntax] || !SyntaxFactory.AreEquivalent(gate, transition))
                        throw DirectorPhaseError(entry, phase, "could not extract the proven choreography transition", entry);
                    yield return $"await play.RunUntilCue({phase}{RoleArguments(phase)});";
                }
                else if (choreography.IdlePhases.Contains(phase))
                {
                    yield return $"await play.Idle({phase}{RoleArguments(phase)});";
                }
                else if (choreography.FallThrough.TryGetValue(phase, out int sharedPhase))
                {
                    string work = ExtractFinish(phase, $"RunPhase{PhaseIdentifier(phase)}");
                    yield return $"await play.FallThrough({phase}, {work}, {sharedPhase}{RoleArguments(phase)});";
                }
                else
                {
                    string finish = ExtractFinish(phase);
                    yield return $"await play.Finish({phase}, {finish}{RoleArguments(phase)});";
                }
            }
        }

        string ExtractFinish(int phase, string? methodName = null)
        {
            var section = cases[phase];
            var statements = section.Statements.SkipLast(1).ToArray();
            if (section.Statements.LastOrDefault() is not BreakStatementSyntax || statements.Length == 0 || !IsClosedFragment(director, statements, section))
                throw DirectorPhaseError(entry, phase, "could not extract the proven choreography terminal arm", entry);
            string name = ReserveName(methodName ?? (choreography.Routes.Count == 1 ? "FinishScene" : $"FinishPhase{PhaseIdentifier(phase)}"), names);
            var body = PhaseBody(director, statements, null);
            bool suspends = body.DescendantNodes().OfType<AwaitExpressionSyntax>().Any();
            if (director.ParameterList.Parameters.Any(parameter => body.DescendantNodes().OfType<IdentifierNameSyntax>().Any(reference => reference.Identifier.ValueText == parameter.Identifier.ValueText)))
                throw DirectorPhaseError(entry, phase, "the choreography finish uses its coordinator participant", entry);
            scene.Methods.Add(director.WithIdentifier(SyntaxFactory.Identifier(name)).WithAttributeLists(default).WithParameterList(SyntaxFactory.ParameterList())
                .WithModifiers(suspends ? Modifiers("private", director) : SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                .WithReturnType(SyntaxFactory.ParseTypeName(suspends ? "EventRoutine" : "void")).WithBody(body));
            MarkExtractedPhases(scene, entry, [phase], name);
            return name;
        }

        string RoleArguments(int phase) => string.Concat(scene.DirectorMachine.OrderedChapters.Where(chapter => chapter.Phase == phase && chapter.Entry != entry && chapter.Entry.Tag == 1)
            .Select(chapter =>
            {
                string continuation = choreography.RoleContinuations.TryGetValue(chapter.Entry, out var links) && links.TryGetValue(phase, out int next) ? $", thenPhase: {next}" : "";
                return $", Role(evt.{_symbols.Participants[chapter.Entry.Slot].Name}, {scene.ChapterMethod(chapter.Entry, phase)}{continuation})";
            }));
    }

    private static bool IsChoreographyRoute(MethodDeclarationSyntax method) =>
        method.ParameterList.Parameters is [{ Identifier.ValueText: "play", Type: IdentifierNameSyntax { Identifier.ValueText: "SceneChoreography" } }];

    private static bool IsChoreographyAlias(StatementSyntax statement) =>
        SyntaxFactory.AreEquivalent(statement, SyntaxFactory.ParseStatement("var evt = ctx.Scene;")) ||
        SyntaxFactory.AreEquivalent(statement, SyntaxFactory.ParseStatement("var scripts = ctx.Scripts;"));

    public IReadOnlySet<int>? UnreachablePollingExitLabels(int slot, ushort tag)
    {
        var entry = new Entry(slot, tag);
        var labels = Scenes.Where(scene => scene.DirectorMachine?.Choreography != null)
            .SelectMany(scene => scene.DirectorMachine!.Choreography!.UnreachablePollingExits.Where(arm => arm.Key.Entry == entry).Select(arm => arm.Value)).ToHashSet();
        return labels.Count == 0 ? null : labels;
    }

    public IReadOnlySet<int>? LinkedPollingDispatchLabels(int slot, ushort tag)
    {
        var entry = new Entry(slot, tag);
        var labels = Scenes.Where(scene => scene.DirectorMachine?.Choreography?.LinkedPollingDispatches.ContainsKey(entry) == true)
            .Select(scene => scene.DirectorMachine!.Choreography!.LinkedPollingDispatches[entry]).ToHashSet();
        return labels.Count == 0 ? null : labels;
    }

    private MethodDeclarationSyntax RewriteLinearRole(Scene scene, Entry entry, MethodDeclarationSyntax method, StorageDeclarations storage)
    {
        var machine = scene.DirectorMachine!;
        string selector = storage.Qualify(machine.Choreography!.Selector);
        var chapters = scene.For(entry)!;
        if (machine.Choreography.ConditionalParticipants.Contains(entry))
        {
            int phase = chapters.Keys.Single();
            string name = scene.ChapterMethod(entry, phase);
            MethodDeclarationSyntax? helper = scene.Methods.SingleOrDefault(candidate => candidate.Identifier.ValueText == name);
            string awaiter = helper?.ReturnType.ToString() == "EventRoutine" ? "await " : "";
            var expected = SyntaxFactory.ParseStatement($"{awaiter}ctx.{scene.Name}.{name}(this);");
            int label = scene.GuardedChapters[(entry, phase)];
            IfStatementSyntax? condition = FindDecodedCondition(method, label);
            if (helper == null || condition is not { Else: null, Statement: BlockSyntax { Statements: [var call] } } || !SyntaxFactory.AreEquivalent(call, expected))
                throw DirectorPhaseError(entry, phase, "could not extract the proven conditional choreography arm", machine.Director);
            return method.ReplaceNode(condition, SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.PerformPhase(this);"));
        }
        bool hasPollWork = machine.Choreography.ParticipantPollWork.Contains(entry);
        bool hasFallback = machine.Choreography.ParticipantFallbacks.Contains(entry);
        var continuations = machine.Choreography.RoleContinuations.GetValueOrDefault(entry);
        if (!TryReadSourcePolling(method, selector, null, out var cases, out var selection, hasPollWork, continuations, hasFallback) || !cases.Keys.ToHashSet().SetEquals(chapters.Keys))
            throw DirectorPhaseError(entry, null, "could not extract the proven choreography participant loop", machine.Director);
        foreach (var (phase, section) in cases)
        {
            string name = scene.ChapterMethod(entry, phase);
            MethodDeclarationSyntax? helper = scene.Methods.SingleOrDefault(candidate => candidate.Identifier.ValueText == name);
            string awaiter = helper?.ReturnType.ToString() == "EventRoutine" ? "await " : "";
            var expected = SyntaxFactory.ParseStatement($"{awaiter}ctx.{scene.Name}.{name}(this);");
            if (helper == null || !scene.ExtractedChapters.Contains((entry, phase)) || section.Statements is not [var call, BreakStatementSyntax] || !SyntaxFactory.AreEquivalent(call, expected))
                throw DirectorPhaseError(entry, phase, "could not extract the proven choreography participant arm", machine.Director);
            if (machine.Choreography.UnreachablePollingExits.ContainsKey((entry, phase)) && !helper.AttributeLists.SelectMany(list => list.Attributes).Any(attribute => attribute.Name.ToString() == "OmitUnreachablePollingExit"))
            {
                var attribute = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("OmitUnreachablePollingExit"));
                int helperIndex = scene.Methods.IndexOf(helper);
                scene.Methods[helperIndex] = helper.AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(attribute)));
            }
        }
        if (hasFallback)
        {
            var fallback = selection!.Sections.SingleOrDefault(section => section.Labels.Any(label => label is DefaultSwitchLabelSyntax));
            if (fallback == null)
                throw DirectorPhaseError(entry, null, "could not locate work for unmatched choreography phases", machine.Director);
            var steps = fallback.Statements.SkipLast(1).ToArray();
            if (fallback.Statements.LastOrDefault() is not BreakStatementSyntax || !IsClosedFragment(method, steps, fallback))
                throw DirectorPhaseError(entry, null, "could not extract work for unmatched choreography phases", machine.Director);
            var lambda = SyntaxFactory.ParenthesizedLambdaExpression().WithBlock(SyntaxFactory.Block(steps));
            if (steps.SelectMany(step => step.DescendantNodesAndSelf()).Any(node => node is AwaitExpressionSyntax))
                lambda = lambda.WithAsyncKeyword(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
            var run = SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.PerformPhase(this, () => {{ }});");
            return method.ReplaceNode(selection, run.ReplaceNode(run.DescendantNodes().OfType<ParenthesizedLambdaExpressionSyntax>().Single(), lambda));
        }
        if (hasPollWork)
            return method.ReplaceNode(selection!, SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.PerformPhase(this);"));
        if (continuations != null)
        {
            // Re-key the direct table to the physical order of work. Dense table
            // entries that already target its default wait need no authored case.
            string prefix = SwitchLayout.Key([machine.Choreography.Selector], [], directDispatch: true);
            AttributeSyntax? layout = method.AttributeLists.SelectMany(list => list.Attributes).FirstOrDefault(attribute =>
                attribute.Name.ToString() == "SwitchLayout" && attribute.ArgumentList!.Arguments[0].Expression is LiteralExpressionSyntax key &&
                key.Token.ValueText.StartsWith(prefix, StringComparison.Ordinal));
            if (layout == null)
                throw DirectorPhaseError(entry, null, "could not preserve the linked choreography role table", machine.Director);
            string[] sections = cases.Keys.Select(phase => SwitchLayout.Section([phase], false, false)).Append("DB").ToArray();
            string key = SwitchLayout.Key([machine.Choreography.Selector], sections, directDispatch: true);
            var arguments = layout.ArgumentList!.Arguments;
            arguments = arguments.Replace(arguments[0], SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(key))));
            if (!machine.Choreography.RolePollJumps.Contains(entry))
                arguments = arguments.Replace(arguments[3], SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)));
            method = method.ReplaceNode(layout, layout.WithArgumentList(layout.ArgumentList.WithArguments(arguments)));
        }
        return method.WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.Perform(this);")));
    }

    private static bool TryReadSourcePolling(MethodDeclarationSyntax method, string selector, string? pulse, out Dictionary<int, SwitchSectionSyntax> cases, out SwitchStatementSyntax? dispatch,
        bool allowPollWork, IReadOnlyDictionary<int, int>? fallsThrough = null, bool allowFallback = false)
    {
        cases = new();
        dispatch = null;
        if (method.Body!.Statements.LastOrDefault() is not WhileStatementSyntax { Condition.RawKind: (int)SyntaxKind.TrueLiteralExpression, Statement: BlockSyntax loop } ||
            method.Body.Statements.SkipLast(1).Any(statement => !IsChoreographyAlias(statement)))
            return false;
        int dispatchIndex = pulse == null ? 0 : 1;
        if (allowPollWork)
        {
            var matches = loop.Statements.OfType<SwitchStatementSyntax>().Where(selection => SyntaxFactory.AreEquivalent(selection.Expression, SyntaxFactory.ParseExpression(selector))).ToArray();
            if (matches is not [var match])
                return false;
            dispatchIndex = loop.Statements.IndexOf(match);
        }
        if (loop.Statements.Count < dispatchIndex + 2 || !IsOneFrameWait(loop.Statements[^1]) ||
            loop.Statements[dispatchIndex] is not SwitchStatementSyntax selection || !SyntaxFactory.AreEquivalent(selection.Expression, SyntaxFactory.ParseExpression(selector)) ||
            pulse != null && (dispatchIndex == 0 || !SyntaxFactory.AreEquivalent(loop.Statements[dispatchIndex - 1], SyntaxFactory.ParseStatement($"{pulse} = false;"))))
            return false;
        dispatch = selection;
        if (fallsThrough?.Count > 0)
            return TryReadSourcePollingLabels(loop, selection, fallsThrough, cases);
        var between = loop.Statements.Skip(dispatchIndex + 1).SkipLast(1).ToArray();
        if (between.Length != 0 && (pulse != null || between is not [LabeledStatementSyntax { Statement: EmptyStatementSyntax } labelAtWait] ||
            method.DescendantNodes().OfType<GotoStatementSyntax>().Any(jump => jump.Expression is IdentifierNameSyntax name && name.Identifier.ValueText == labelAtWait.Identifier.ValueText)))
            return false;
        foreach (SwitchSectionSyntax section in selection.Sections)
        {
            if (allowFallback && section.Labels.Any(label => label is DefaultSwitchLabelSyntax))
                continue;
            foreach (SwitchLabelSyntax label in section.Labels)
            {
                if (label is not CaseSwitchLabelSyntax site || PhaseValue(site) is not int value || !cases.TryAdd(value, section))
                    return false;
            }
        }
        return true;
    }

    private static bool TryReadSourcePollingLabels(BlockSyntax loop, SwitchStatementSyntax selection, IReadOnlyDictionary<int, int> fallsThrough,
        Dictionary<int, SwitchSectionSyntax> cases)
    {
        if (loop.Statements[^2] is not LabeledStatementSyntax { Statement: EmptyStatementSyntax } atWait || selection.Sections.LastOrDefault() is not { } fallback ||
            fallback.Labels is not [DefaultSwitchLabelSyntax] || fallback.Statements is not [GotoStatementSyntax { Expression: IdentifierNameSyntax wait }] || wait.Identifier.ValueText != atWait.Identifier.ValueText)
            return false;

        var byLabel = new Dictionary<string, (int Phase, SwitchSectionSyntax Section)>();
        foreach (SwitchSectionSyntax section in selection.Sections.SkipLast(1))
        {
            if (section.Statements is not [GotoStatementSyntax { Expression: IdentifierNameSyntax target }])
                return false;
            if (target.Identifier.ValueText == atWait.Identifier.ValueText)
                continue;
            if (section.Labels is not [CaseSwitchLabelSyntax label] || PhaseValue(label) is not int phase || !byLabel.TryAdd(target.Identifier.ValueText, (phase, section)))
                return false;
        }
        var arms = loop.Statements.Select((statement, index) => (statement, index)).Where(item => item.statement is LabeledStatementSyntax label && byLabel.ContainsKey(label.Identifier.ValueText)).ToArray();
        if (arms.Length == 0 || arms.Length != byLabel.Count || arms[0].index != loop.Statements.IndexOf(selection) + 1)
            return false;
        for (int index = 0; index < arms.Length; index++)
        {
            if (arms[index].statement is not LabeledStatementSyntax { Statement: EmptyStatementSyntax } label)
                return false;
            var (phase, section) = byLabel[label.Identifier.ValueText];
            int end = index + 1 == arms.Length ? loop.Statements.Count - 2 : arms[index + 1].index;
            var body = loop.Statements.Skip(arms[index].index + 1).Take(end - arms[index].index - 1).ToList();
            if (fallsThrough.TryGetValue(phase, out int next))
            {
                if (body.LastOrDefault() is GotoStatementSyntax { Expression: IdentifierNameSyntax jump } && byLabel.TryGetValue(jump.Identifier.ValueText, out var destination) && destination.Phase == next)
                    body.RemoveAt(body.Count - 1);
                else if (index + 1 == arms.Length || byLabel[((LabeledStatementSyntax)arms[index + 1].statement).Identifier.ValueText].Phase != next)
                    return false;
            }
            else if (body.LastOrDefault() is GotoStatementSyntax { Expression: IdentifierNameSyntax exit } && exit.Identifier.ValueText == atWait.Identifier.ValueText)
            {
                body.RemoveAt(body.Count - 1);
            }
            else if (index + 1 != arms.Length)
            {
                return false;
            }
            body.Add(SyntaxFactory.BreakStatement());
            cases.Add(phase, section.WithStatements(SyntaxFactory.List(body)));
        }
        return true;
    }
}
