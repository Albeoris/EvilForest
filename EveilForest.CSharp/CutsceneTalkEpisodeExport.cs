using System.Globalization;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private const ushort TalkCallbackTag = 3;
    private const string DecodedConditionAnnotation = "DecodedCondition";

    internal sealed record TalkEpisode(int ConditionLabel, int ContinuationLabel);

    private readonly record struct TalkGuard(EVExpressionToken Selector, int Value, bool Equality)
    {
        public bool Admits(int value) => (value == Value) == Equality;
    }

    private sealed record TalkActorEvidence(EVInstruction[] Reachable, bool ProvidesDialogue, bool Unsupported);

    private void AddTalkEpisodes(Handler[] handlers, List<Scene> scenes)
    {
        var unique = handlers.GroupBy(handler => handler.Entry).Where(group => group.Count() == 1)
            .ToDictionary(group => group.Key, group => group.Single());
        var actorEvidence = new Dictionary<Entry, TalkActorEvidence>();
        foreach (Handler handler in unique.Values.Where(handler => handler.Owner.Type == 2 && handler.Entry.Tag == TalkCallbackTag))
        {
            IList<EVInstruction> code = handler.Body.Instructions;
            if (scenes.Any(scene => scene.For(handler.Entry) != null) || !code.Any(step => step.Opcode == Jsm.Opcode.REQEW))
                continue;

            var flow = new EpisodeFlow(handler.Body);
            var reachable = flow.Reachable(0, 0, code.Count);
            int chapter = 0;
            int selectedThrough = -1;
            for (int condition = 0; condition + 2 < code.Count; condition++)
            {
                int alternate = flow.ForwardBranch(condition);
                if (alternate < 0 || !reachable.Contains(condition) || !flow.Incoming[condition + 1].SequenceEqual([condition]) ||
                    !TryReadTalkGuard(code[condition], code[condition + 1], out TalkGuard guard))
                    continue;

                int start = condition + 2;
                int tail = alternate - 1;
                if (tail <= start || flow.JumpTarget(tail) is not int continuation || continuation <= alternate ||
                    !flow.Closed(start, tail, condition + 1) || flow.Incoming[tail].Any(source => source < start || source >= tail))
                    continue;

                // The rejecting edge must still reach the continuation through ordinary dialogue.
                var fallback = flow.Reachable(alternate, alternate, continuation + 1);
                if (!fallback.Any(index => index < continuation && IsTalkLine(code[index], handler.Entry.Slot) &&
                        flow.Reachable(index, alternate, continuation + 1).Contains(continuation)))
                    continue;

                EVInstruction[] steps = code.Skip(start).Take(tail - start).ToArray();
                if (!steps.Any(step => step.Opcode == Jsm.Opcode.REQEW))
                    continue;

                // The callback's arm order determines chapter identity. A rejected arm
                // keeps its place so edits to it cannot rename an unchanged sibling.
                chapter++;
                if (condition <= selectedThrough || !HasTalkStaging(handler, flow, start, tail, guard, InspectRequest))
                    continue;

                selectedThrough = tail;
                EVVariable cell = EVVariable.Decode(guard.Selector);
                var fragment = new CutscenePhaseGraph.Phase(handler.Entry, handler.Owner.Type, guard.Selector,
                    CutscenePhaseGraph.Cell.Of(cell, handler.Entry.Slot), chapter, code[start].Label, steps);
                var scene = new Scene($"{Fallback(handler.Entry)}_TalkEpisode{chapter}", handler.Entry, false, new() { [chapter] = "Play" })
                {
                    Rule = "state-guarded talk episode",
                    Talk = new(code[condition].Label, continuation < code.Count ? code[continuation].Label : handler.Body.EndLabel)
                };
                scene.FragmentKeys.Add((handler.Entry, chapter), fragment.Identity());
                scenes.Add(scene);
            }

            TalkActorEvidence? InspectRequest(EVInstruction step)
            {
                if (step.Operands is not [_, { Expression: null } receiver, { Expression: null } tag] ||
                    EpisodeActorSlot(receiver.Value) is not int slot)
                    return null;

                var target = new Entry(slot, (ushort)tag.Value);
                if (slot == handler.Entry.Slot || !unique.TryGetValue(target, out Handler? actor))
                    return null;
                if (!actorEvidence.TryGetValue(target, out TalkActorEvidence? evidence))
                {
                    var calls = new EpisodeFlow(actor.Body);
                    EVInstruction[] live = calls.Reachable(0, 0, calls.Code.Count).Select(index => calls.Code[index]).ToArray();
                    bool unsupported = live.Any(instruction => IsRequest(instruction) || IsUnsupportedTalkStep(instruction));
                    evidence = new(live, !unsupported && HasBlockingTalkDialogue(actor, calls), unsupported);
                    actorEvidence.Add(target, evidence);
                }
                return evidence;
            }
        }
    }

    private static bool IsUnsupportedTalkStep(EVInstruction step) => ReadsEpisodeInput(step) ||
        step.Opcode is Jsm.Opcode.MAPJUMP or Jsm.Opcode.ENCOUNT or Jsm.Opcode.ENCOUNT2 or Jsm.Opcode.MINIGAME or Jsm.Opcode.MENU or Jsm.Opcode.PARTYMENU or
            Jsm.Opcode.STOP or Jsm.Opcode.DRET or Jsm.Opcode.DELETE or Jsm.Opcode.NEW or Jsm.Opcode.NEW2 or Jsm.Opcode.NEW3 or
            Jsm.Opcode.STARTSEQ or Jsm.Opcode.REPLY or Jsm.Opcode.REPLYSW or Jsm.Opcode.REPLYEW;

    private bool HasBlockingTalkDialogue(Handler actor, EpisodeFlow flow)
    {
        if (RequiresOnCompletion(actor.Body, step => IsTalkLine(step, actor.Entry.Slot)))
            return true;
        if (!RequiresOnCompletion(actor.Body, _ => true))
            return false;

        // Track windows definitely carrying the actor's line along paths that
        // have not yet finished one. Alternative paths intersect at joins.
        var pending = new Queue<(int Index, HashSet<int> Windows)>();
        var unproven = new Dictionary<int, HashSet<int>>();
        pending.Enqueue((0, []));
        while (pending.TryDequeue(out var path))
        {
            if (path.Index == flow.Code.Count || flow.Code[path.Index].Opcode == Jsm.Opcode.Return)
                return false;

            EVInstruction step = flow.Code[path.Index];
            if (IsTalkLine(step, actor.Entry.Slot) ||
                step is { Opcode: Jsm.Opcode.WAITMES, Operands: [{ Expression: null } wait] } && path.Windows.Contains(wait.Value))
                continue;

            var windows = new HashSet<int>(path.Windows);
            if (unproven.TryGetValue(path.Index, out HashSet<int>? known))
            {
                windows.IntersectWith(known);
                if (windows.SetEquals(known))
                    continue;
            }
            unproven[path.Index] = windows;

            var after = new HashSet<int>(windows);
            int windowOperand = step.Opcode switch
            {
                Jsm.Opcode.MES or Jsm.Opcode.MESN or Jsm.Opcode.CLOSE => 0,
                Jsm.Opcode.MESA or Jsm.Opcode.MESAN => 1,
                _ => -1
            };
            if (step.Opcode is Jsm.Opcode.CLOSEALL or Jsm.Opcode.MESB ||
                windowOperand >= 0 && step.Operands[windowOperand].Expression != null)
                after.Clear();
            else if (windowOperand >= 0)
            {
                int window = step.Operands[windowOperand].Value;
                if (step.Opcode == Jsm.Opcode.MESN || step.Opcode == Jsm.Opcode.MESAN && IsTalkSpeaker(step, actor.Entry.Slot))
                    after.Add(window);
                else
                    after.Remove(window);
            }

            foreach (int successor in flow.Successors[path.Index])
                pending.Enqueue((successor, after));
        }
        return true;
    }

    private bool HasTalkStaging(Handler handler, EpisodeFlow flow, int start, int tail, TalkGuard guard,
        Func<EVInstruction, TalkActorEvidence?> inspectRequest)
    {
        IList<EVInstruction> code = flow.Code;
        int[] arm = Enumerable.Range(start, tail - start).ToArray();
        if (arm.Any(index => flow.Successors[index].Any(next => next <= index) || IsUnsupportedTalkStep(code[index])))
            return false;

        EVVariable cell = EVVariable.Decode(guard.Selector);
        var register = CutscenePhaseGraph.Cell.Of(cell, handler.Entry.Slot);
        int[] writes = arm.Where(index => MayWriteMode(code[index], register)).Take(2).ToArray();
        if (writes is not [int commit] || !flow.Requires(start, tail, commit) ||
            code[commit].Expression is not [var target, var literal, { Opcode: (byte)Jsm.Excode.Let }] ||
            !StorageDeclarations.IsStorage(target) || !EVVariable.Decode(target).Equals(cell) ||
            !TryReadLiteral(literal, out int next) || !FitsTalkState(cell, next) || guard.Admits(next))
            return false;

        var requests = arm.Where(index => IsRequest(code[index])).ToDictionary(index => index, index => inspectRequest(code[index]));
        if (requests.Values.Any(proof => proof is not { Unsupported: false }))
            return false;
        EVInstruction[] requested = requests.Values.SelectMany(proof => proof!.Reachable).ToArray();
        if (requested.Any(instruction => MayWriteMode(instruction, register)))
            return false;

        int[] required = arm.TakeWhile(index => index < commit).Where(index => flow.Requires(start, commit, index)).ToArray();
        int request = required.FirstOrDefault(index => code[index].Opcode == Jsm.Opcode.REQEW &&
            requests.GetValueOrDefault(index) is { ProvidesDialogue: true }, -1);
        if (request < 0)
            return false;

        // The cross-actor exchange and the actor's own choreography must precede the commit.
        return required.Any(index => index > request && IsTalkLine(code[index], handler.Entry.Slot)) &&
            required.Any(index => code[index].Opcode == Jsm.Opcode.WAITTURN &&
                required.Any(turn => turn < index && code[turn].Opcode is Jsm.Opcode.TURN or Jsm.Opcode.TURNA or Jsm.Opcode.TURNDS or Jsm.Opcode.TURNTO)) &&
            HasTalkControlHandoff(flow, start, tail, request, commit, requested);
    }

    private static bool HasTalkControlHandoff(EpisodeFlow flow, int start, int tail, int request, int commit, EVInstruction[] requested)
    {
        IList<EVInstruction> code = flow.Code;
        int[] enters = Enumerable.Range(start, request - start).Where(index => code[index].Opcode == Jsm.Opcode.UCOFF).ToArray();
        int[] restores = Enumerable.Range(commit + 1, tail - commit - 1).Where(index => code[index].Opcode == Jsm.Opcode.UCON).ToArray();
        if (enters.Length == 0 || restores.Length == 0)
            return false;

        // Control may be managed conditionally. Repeated tests of an unchanged
        // register cannot supply opposing facts for the lock and its restoration.
        var stateChanges = code.Skip(start).Take(tail - start).Concat(requested).SelectMany(CutsceneMemoryIndex.Accesses)
            .Where(access => access.Write || !access.Read).Select(access => CutscenePhaseGraph.Cell.Of(access.Variable, -1)).Distinct().ToArray();
        var stable = new List<(int Branch, int Alternate, TalkGuard Guard)>();
        for (int index = start; index + 2 < tail; index++)
        {
            int alternate = flow.ForwardBranch(index);
            if (alternate < 0 || !TryReadTalkGuard(code[index], code[index + 1], out TalkGuard guard))
                continue;

            var cell = CutscenePhaseGraph.Cell.Of(EVVariable.Decode(guard.Selector), -1);
            if (!stateChanges.Any(change => change.Overlaps(cell)))
                stable.Add((index + 1, alternate, guard));
        }

        var conditions = enters.Concat(restores).ToDictionary(index => index, Facts);
        return enters.Any(enter =>
        {
            var afterEnter = flow.Reachable(enter, start, tail);
            return restores.Any(restore => afterEnter.Contains(restore) && !conditions[enter].Any(before =>
                conditions[restore].Any(after => EVVariable.Decode(before.Selector).Equals(EVVariable.Decode(after.Selector)) &&
                    (before.Equality && !after.Admits(before.Value) || after.Equality && !before.Admits(after.Value)))));
        });

        TalkGuard[] Facts(int index)
        {
            var result = new List<TalkGuard>();
            foreach (var (branch, alternate, guard) in stable)
            {
                if (branch >= index || !flow.Requires(start, index, branch))
                    continue;
                bool whenTrue = flow.Reachable(branch + 1, start, index + 1).Contains(index);
                bool whenFalse = flow.Reachable(alternate, start, index + 1).Contains(index);
                if (whenTrue != whenFalse)
                    result.Add(whenTrue ? guard : guard with { Equality = !guard.Equality });
            }
            return result.ToArray();
        }
    }

    private bool IsTalkLine(EVInstruction step, int owner) => step.Opcode == Jsm.Opcode.MES ||
        step.Opcode == Jsm.Opcode.MESA && IsTalkSpeaker(step, owner);

    private bool IsTalkSpeaker(EVInstruction step, int owner) =>
        step.Operands is [{ Expression: null } speaker, ..] && EpisodeActorSlot(speaker.Value) == owner;

    private static bool TryReadTalkGuard(EVInstruction step, EVInstruction branch, out TalkGuard guard)
    {
        guard = default;
        if (step.Opcode != Jsm.Opcode.EXPR)
            return false;

        bool positive = branch.Opcode == Jsm.Opcode.JMP_IFN;
        if (step.Expression is [var bit] && IsTalkState(bit) && EVVariable.Decode(bit).UsesBitAddress)
        {
            guard = new(bit, positive ? 1 : 0, true);
            return true;
        }

        if (step.Expression is not [var left, var right, { Opcode: (byte)Jsm.Excode.Equivalence or (byte)Jsm.Excode.NotEquivalence } comparison])
            return false;
        if (IsTalkState(right))
            (left, right) = (right, left);
        if (!IsTalkState(left) || !TryReadLiteral(right, out int value) || !FitsTalkState(EVVariable.Decode(left), value))
            return false;

        guard = new(left, value, positive == (comparison.Opcode == (byte)Jsm.Excode.Equivalence));
        return true;
    }

    private static bool IsTalkState(EVExpressionToken token) => StorageDeclarations.IsStorage(token) &&
        EVVariable.Decode(token).Scope is EVStorageScope.Global or EVStorageScope.Map;

    private static bool FitsTalkState(EVVariable cell, int value) => cell.IsSigned || cell.Type == EVStorageType.UInt24
        ? value >= -(1 << (cell.Width - 1)) && value < 1 << (cell.Width - 1)
        : value >= 0 && value < 1 << cell.Width;

    private static MethodDeclarationSyntax AnnotateDecodedConditions(MethodDeclarationSyntax method, int statementOffset,
        IReadOnlyDictionary<int, int> instructionOffsets) =>
        method.ReplaceNodes(method.DescendantNodes().OfType<StatementSyntax>().Where(statement => statement is IfStatementSyntax or ForStatementSyntax), (original, rewritten) =>
            instructionOffsets.TryGetValue(original.SpanStart - statementOffset, out int label)
                ? rewritten.WithAdditionalAnnotations(new SyntaxAnnotation(DecodedConditionAnnotation, label.ToString(CultureInfo.InvariantCulture)))
                : rewritten);

    private static IfStatementSyntax? FindDecodedCondition(MethodDeclarationSyntax method, int label)
    {
        string identity = label.ToString(CultureInfo.InvariantCulture);
        return method.GetAnnotatedNodes(DecodedConditionAnnotation).OfType<IfStatementSyntax>()
            .SingleOrDefault(conditional => conditional.GetAnnotations(DecodedConditionAnnotation).Any(annotation => annotation.Data == identity));
    }

    private static MethodDeclarationSyntax RewriteTalkEpisode(Scene scene, Entry entry, MethodDeclarationSyntax method, ParameterListSyntax parameter,
        RewriteBody rewriter, Func<int, string> jumpLabel)
    {
        TalkEpisode episode = scene.Talk!;
        if (FindDecodedCondition(method, episode.ConditionLabel) is not { Statement: BlockSyntax arm } guard)
            return method;

        // In a structured else, the formatter represents the original terminal jump
        // through the if/else layout. Keep an explicit jump in the callback when present.
        int length = arm.Statements.Count;
        if (guard.Else == null)
        {
            if (arm.Statements.LastOrDefault() is not GotoStatementSyntax { Expression: IdentifierNameSyntax target } ||
                target.Identifier.ValueText != jumpLabel(episode.ContinuationLabel))
                return method;
            length--;
        }

        StatementSyntax[] fragment = arm.Statements.Take(length).ToArray();
        if (fragment.Length == 0 || !IsClosedFragment(method, fragment, arm) ||
            fragment.SelectMany(statement => statement.DescendantNodesAndSelf())
                .Any(node => node is LocalDeclarationStatementSyntax or DeclarationExpressionSyntax or SwitchStatementSyntax))
            return method;

        var (chapter, name) = scene.Chapters.Single();
        BlockSyntax body = SyntaxFactory.Block(method.Body!.Statements.OfType<LocalDeclarationStatementSyntax>().Cast<StatementSyntax>().Concat(fragment));
        scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter).WithAttributeLists(default)
            .WithModifiers(Modifiers("internal", method)).WithBody((BlockSyntax)rewriter.Visit(body)!));
        scene.ExtractedChapters.Add((entry, chapter));
        StatementSyntax call = SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.{name}(this);");
        return method.ReplaceNode(guard, guard.WithStatement(arm.WithStatements(SyntaxFactory.List(arm.Statements.Skip(length).Prepend(call)))));
    }
}
