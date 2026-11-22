using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private void AddGuardedStoryScenes(IEnumerable<Handler> handlers, List<Scene> scenes)
    {
        foreach (Handler handler in handlers.Where(candidate => candidate.Owner.Type == 0 && candidate.Entry.Tag == 1))
        {
            if (scenes.Any(scene => scene.Whole && scene.Entry == handler.Entry))
                continue;

            IList<EVInstruction> code = handler.Body.Instructions;
            var candidates = new List<(EVInstruction Test, CutscenePhaseGraph.Phase Phase)>();
            IReadOnlyDictionary<int, int[]>? successors = null;
            for (int index = 1; index + 1 < code.Count; index++)
            {
                EVInstruction test = code[index - 1];
                EVInstruction branch = code[index];
                if (test.Opcode != Jsm.Opcode.EXPR || branch.Opcode != Jsm.Opcode.JMP_IFN ||
                    branch.Targets is not [int end] || end <= code[index + 1].Label ||
                    !TryReadStoryGuard(test.Expression, out EVExpressionToken selector, out int limit))
                    continue;

                int start = code[index + 1].Label;
                EVInstruction[] instructions = code.Skip(index + 1).TakeWhile(step => step.Label < end).ToArray();
                if (code.Any(step => (step.Label < start || step.Label >= end) &&
                    step.Targets.Any(target => target >= start && target < end)))
                    continue;
                successors ??= handler.Body.GetSuccessors();
                if (!HasGuardedStaging(instructions, successors, limit))
                    continue;
                candidates.Add((test, new(handler.Entry, handler.Owner.Type, selector,
                    CutscenePhaseGraph.Cell.Of(EVVariable.Decode(selector), handler.Entry.Slot), limit, start, instructions)));
            }

            foreach (var byLimit in candidates.GroupBy(candidate => candidate.Phase.Value).Where(group => group.Count() == 1))
            {
                var (test, phase) = byLimit.Single();
                var scene = new Scene($"{Fallback(phase.Entry)}_BeforeStoryStep{phase.Value}", phase.Entry, false,
                    new() { [phase.Value] = "Play" })
                {
                    Rule = "story-guarded dialogue",
                    StoryGuard = test.Expression
                };
                scene.Selectors.Add(phase.Entry, phase.Selector);
                scene.FragmentKeys.Add((phase.Entry, phase.Value), phase.Identity());
                scenes.Add(scene);
            }
        }
    }

    private static bool TryReadStoryGuard(EVExpressionToken[] expression, out EVExpressionToken selector, out int limit)
    {
        selector = default;
        limit = 0;
        if (expression.Length == 3)
            return ReadUpperBound(expression, out selector, out limit);
        if (expression.Length != 7 || expression[^1].Opcode is not ((byte)Jsm.Excode.BooleanAnd or (byte)Jsm.Excode.BitAnd))
            return false;

        var first = expression.AsSpan(0, 3);
        var second = expression.AsSpan(3, 3);
        return IsSimpleComparison(first) && ReadUpperBound(second, out selector, out limit) ||
            IsSimpleComparison(second) && ReadUpperBound(first, out selector, out limit);

        static bool ReadUpperBound(ReadOnlySpan<EVExpressionToken> condition, out EVExpressionToken cell, out int maximum)
        {
            cell = condition[0];
            maximum = 0;
            return condition[2].Opcode == (byte)Jsm.Excode.LessThan && IsStoryCounter(cell) &&
                TryReadLiteral(condition[1], out maximum) && maximum > 0;
        }

        static bool IsSimpleComparison(ReadOnlySpan<EVExpressionToken> condition) =>
            condition[2].Opcode is >= (byte)Jsm.Excode.LessThan and <= (byte)Jsm.Excode.GreatOrEquals or
                (byte)Jsm.Excode.Equivalence or (byte)Jsm.Excode.NotEquivalence &&
            (StorageDeclarations.IsStorage(condition[0]) && TryReadLiteral(condition[1], out _) ||
                StorageDeclarations.IsStorage(condition[1]) && TryReadLiteral(condition[0], out _));
    }

    private static bool IsStoryCounter(EVExpressionToken token)
    {
        if (!StorageDeclarations.IsStorage(token))
            return false;
        EVVariable view = EVVariable.Decode(token);
        return view.Scope == EVStorageScope.Global &&
            StorageDeclarations.KnownGlobals.GetValueOrDefault((view.Type, view.Address)) == "ScenarioCounter";
    }

    private static bool TryReadLiteral(EVExpressionToken token, out int value)
    {
        value = token.Opcode == (byte)Jsm.Excode.Const26 ? token.Value << 6 >> 6 : token.Value;
        return token.Opcode is (byte)Jsm.Excode.Const16 or (byte)Jsm.Excode.Const26;
    }

    private readonly record struct GuardedStaging(int Speaker, bool HasOtherSpeaker = false, bool Requested = false, bool Waited = false, bool Advanced = false);

    private bool HasGuardedStaging(EVInstruction[] instructions, IReadOnlyDictionary<int, int[]> successors, int limit)
    {
        var byLabel = instructions.ToDictionary(step => step.Label);
        var pending = new Stack<(int Label, GuardedStaging Progress)>();
        var visited = new HashSet<(int Label, GuardedStaging Progress)>();
        pending.Push((instructions[0].Label, new GuardedStaging(-1)));
        while (pending.TryPop(out var path))
        {
            if (!visited.Add(path) || !byLabel.TryGetValue(path.Label, out EVInstruction? step))
                continue;
            GuardedStaging progress = path.Progress;
            if (step.Opcode is Jsm.Opcode.MESA or Jsm.Opcode.MESAN && step.Operands[0].Expression == null &&
                Actor(step.Operands[0].Value) != null && !progress.HasOtherSpeaker)
            {
                int uid = step.Operands[0].Value;
                progress = progress.Speaker == -1 ? progress with { Speaker = uid } :
                    progress with { HasOtherSpeaker = progress.Speaker != uid };
            }
            bool actorRequest = IsRequest(step) && step.Operands[1].Expression == null &&
                Actor(step.Operands[1].Value) is { } target && target.Scripts.Any(script => script.Id == step.Operands[2].Value);
            if (actorRequest)
                progress = progress with { Requested = true };
            if (step.Opcode == Jsm.Opcode.WAIT && step.Operands[0] is { Expression: null, Value: > 0 } ||
                actorRequest && step.Opcode is Jsm.Opcode.REQSW or Jsm.Opcode.REQEW)
                progress = progress with { Waited = true };
            if (step.Expression is [var cell, var value, { Opcode: (byte)Jsm.Excode.Let }] && IsStoryCounter(cell) &&
                TryReadLiteral(value, out int story) && story >= limit)
                progress = progress with { Advanced = true };
            if (progress is { HasOtherSpeaker: true, Requested: true, Waited: true, Advanced: true })
                return true;

            foreach (int next in successors[path.Label])
                pending.Push((next, progress));
        }
        return false;

        EventSceneSymbols.Participant? Actor(int uid) =>
            _possibleDefinitions.TryGetValue(uid, out HashSet<int>? definitions) && definitions.Count == 1 &&
            _symbols.Participants.GetValueOrDefault(definitions.Single()) is { Kind: 2 } actor ? actor : null;
    }

    private static MethodDeclarationSyntax RewriteGuardedStory(Scene scene, Entry entry, MethodDeclarationSyntax method, ParameterListSyntax parameter,
        RewriteBody rewriter, StorageDeclarations storage)
    {
        if (!SourceExpression.TryFormatCondition(scene.StoryGuard!, storage, out string? guardExpression))
            return method;
        ExpressionSyntax condition = SyntaxFactory.ParseExpression(guardExpression);
        IfStatementSyntax[] matches = method.Body!.Statements.OfType<IfStatementSyntax>().Where(guard =>
            SyntaxFactory.AreEquivalent(guard.Condition, condition)).Take(2).ToArray();
        if (matches is not [{ Statement: BlockSyntax arm } guard])
            return method;

        StatementSyntax[] fragment = [];
        for (int split = arm.Statements.Count; split > 0; split--)
        {
            StatementSyntax[] prefix = arm.Statements.Take(split).ToArray();
            if (IsClosedFragment(method, prefix, arm))
            {
                fragment = prefix;
                break;
            }
        }
        if (fragment.Length == 0 || fragment.SelectMany(statement => statement.DescendantNodesAndSelf())
            .Any(node => node is LocalDeclarationStatementSyntax or DeclarationExpressionSyntax))
            return method;

        // A branch can end in an externally targeted exit or a shared routing tail.
        // Move its closed prefix only if the evidence for staging stays with that prefix.
        StatementSyntax[] remainder = arm.Statements.Skip(fragment.Length).ToArray();
        var storyRegister = SyntaxFactory.ParseExpression(storage.Qualify(scene.Selectors[entry]));
        if (remainder.SelectMany(statement => statement.DescendantNodesAndSelf()).Any(node =>
                node is AwaitExpressionSyntax ||
                node is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax
                {
                    Name.Identifier.ValueText: "RequestAsync" or "RequestWhenReady" or "RequestAndWait" or "TryStart" or "StartWhenReady" or "RunAndWait"
                } } ||
                node is AssignmentExpressionSyntax assignment && SyntaxFactory.AreEquivalent(assignment.Left, storyRegister)))
            return method;

        int limit = scene.Chapters.Keys.Single();
        string name = scene.Chapters[limit];
        BlockSyntax body = SyntaxFactory.Block(method.Body.Statements.OfType<LocalDeclarationStatementSyntax>().Cast<StatementSyntax>().Concat(fragment));
        scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter).WithAttributeLists(default)
            .WithModifiers(Modifiers("internal", method)).WithBody((BlockSyntax)rewriter.Visit(body)!));
        scene.ExtractedChapters.Add((entry, limit));
        var call = SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.{name}(this);");
        return method.ReplaceNode(guard, guard.WithStatement(arm.WithStatements(SyntaxFactory.List(remainder.Prepend(call)))));
    }
}

