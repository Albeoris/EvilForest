using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private static void AddEntranceScenes(IEnumerable<Handler> handlers, List<Scene> scenes)
    {
        foreach (Handler handler in handlers.Where(handler => handler.Owner.Type == 0 && handler.Entry.Tag == 1))
        {
            if (scenes.Any(scene => scene.For(handler.Entry) != null))
                continue;

            IList<EVInstruction> instructions = handler.Body.Instructions;
            var candidates = new List<CutscenePhaseGraph.Phase>();
            IReadOnlyDictionary<int, int[]>? successors = null;
            for (int index = 1; index + 1 < instructions.Count; index++)
            {
                EVInstruction expression = instructions[index - 1];
                EVInstruction branch = instructions[index];
                if (branch.Opcode != Jsm.Opcode.JMP_IFN || expression.Opcode != Jsm.Opcode.EXPR ||
                    expression.Expression is not [var left, var right, { Opcode: (byte)Jsm.Excode.Equivalence }] ||
                    !(IsEntrance(left, right) || IsEntrance(right, left)) ||
                    branch.Targets is not [int end] || end <= instructions[index + 1].Label)
                    continue;

                EVExpressionToken selector = StorageDeclarations.IsStorage(left) ? left : right;
                EVExpressionToken value = StorageDeclarations.IsStorage(left) ? right : left;
                int entrance = value.Opcode == (byte)Jsm.Excode.Const16 ? value.Value : value.Value << 6 >> 6;
                int start = instructions[index + 1].Label;
                EVInstruction[] arm = instructions.Skip(index + 1).TakeWhile(step => step.Label < end).ToArray();
                if (instructions.Any(step => (step.Label < start || step.Label >= end) && step.Targets.Any(target => target >= start && target < end)))
                    continue;
                successors ??= handler.Body.GetSuccessors();
                if (!HasEntranceStaging(arm, start, successors))
                    continue;

                candidates.Add(new(handler.Entry, handler.Owner.Type, selector, CutscenePhaseGraph.Cell.Of(EVVariable.Decode(selector), handler.Entry.Slot),
                    entrance, start, arm));
            }

            foreach (var group in candidates.GroupBy(candidate => candidate.Value).Where(group => group.Count() == 1))
            {
                CutscenePhaseGraph.Phase arm = group.Single();
                string number = arm.Value.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace("-", "Minus", StringComparison.Ordinal);
                var scene = new Scene($"{Fallback(arm.Entry)}_Entrance{number}", arm.Entry, false, new() { [arm.Value] = "Play" })
                {
                    Rule = "field entrance dialogue"
                };
                scene.Selectors.Add(arm.Entry, arm.Selector);
                scene.FragmentKeys.Add((arm.Entry, arm.Value), arm.Identity());
                scenes.Add(scene);
            }
        }

        static bool IsEntrance(EVExpressionToken token, EVExpressionToken value)
        {
            if (!StorageDeclarations.IsStorage(token) || value.Opcode is not ((byte)Jsm.Excode.Const16 or (byte)Jsm.Excode.Const26))
                return false;
            EVVariable view = EVVariable.Decode(token);
            return view.Scope == EVStorageScope.Global &&
                StorageDeclarations.KnownGlobals.GetValueOrDefault((view.Type, view.Address)) == nameof(EvilForest.EventSource.GlobalState.FieldEntrance);
        }
    }

    private static bool HasEntranceStaging(EVInstruction[] arm, int start, IReadOnlyDictionary<int, int[]> successors)
    {
        var instructions = arm.ToDictionary(instruction => instruction.Label);
        var visited = new HashSet<int>();
        var speakers = new HashSet<int>();
        var stagedActors = new HashSet<int>();
        bool cameraMove = false, pause = false, request = false;
        int label = start;
        EVInstruction? previous = null;
        while (visited.Add(label) && instructions.TryGetValue(label, out EVInstruction? instruction))
        {
            if (instruction.Opcode is Jsm.Opcode.MAPJUMP or Jsm.Opcode.ENCOUNT or Jsm.Opcode.ENCOUNT2)
                return false;

            cameraMove |= instruction.Opcode == Jsm.Opcode.BGSSCROLL;
            pause |= instruction.Opcode == Jsm.Opcode.WAIT && instruction.Operands[0] is { Expression: null, Value: > 0 };
            if (IsRequest(instruction) && instruction.Operands[1] is { Expression: null, Value: < 250 } requested)
            {
                stagedActors.Add(requested.Value);
                request = true;
            }
            if (instruction.Opcode is Jsm.Opcode.DTURN or Jsm.Opcode.DANIM &&
                instruction.Operands[0] is { Expression: null, Value: < 250 } updated)
                stagedActors.Add(updated.Value);
            if (instruction.Opcode is Jsm.Opcode.MESA or Jsm.Opcode.MESAN &&
                instruction.Operands[0] is { Expression: null, Value: < 250 } speaker)
                speakers.Add(speaker.Value);

            if (speakers.Count >= 2 && (cameraMove || pause && request && speakers.Count(stagedActors.Contains) >= 2))
                return true;
            int[] edges = successors[label];
            if (edges is [int next])
                label = next;
            else if (instruction.Opcode is Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN && edges is [int first, int second] &&
                previous is { Opcode: Jsm.Opcode.EXPR } guard &&
                guard.Expression.Any(token => StorageDeclarations.IsStorage(token) || token.Opcode is (byte)Jsm.Excode.B_SYSVAR or (byte)Jsm.Excode.B_OBJSPECA) &&
                (IsPollingBody(first, guard.Label) || IsPollingBody(second, guard.Label)))
                label = IsPollingBody(first, guard.Label) ? second : first;
            else
                break;
            previous = instruction;
        }
        return false;

        bool IsPollingBody(int target, int guard)
        {
            return instructions.GetValueOrDefault(target) is { Opcode: Jsm.Opcode.WAIT, Operands: [{ Expression: null, Value: > 0 }] } &&
                successors[target] is [int afterWait] &&
                (afterWait == guard || instructions.GetValueOrDefault(afterWait) is { Opcode: Jsm.Opcode.JMP, Targets: [int next] } && next == guard);
        }
    }

    private static MethodDeclarationSyntax RewriteEntrance(Scene scene, Entry entry, MethodDeclarationSyntax method, ParameterListSyntax parameter,
        RewriteBody rewriter, StorageDeclarations storage)
    {
        int entrance = scene.Chapters.Keys.Single();
        string selector = storage.Qualify(scene.Selectors[entry]);
        IfStatementSyntax[] matches = method.Body!.DescendantNodes().OfType<IfStatementSyntax>().Where(statement =>
            statement.Condition is BinaryExpressionSyntax comparison && comparison.IsKind(SyntaxKind.EqualsExpression) &&
            (IsEntrance(comparison.Left, comparison.Right) || IsEntrance(comparison.Right, comparison.Left))).Take(2).ToArray();
        if (matches is not [{ Statement: BlockSyntax arm } guard] || guard.Ancestors().TakeWhile(node => node != method.Body).Any(node => node switch
            {
                BlockSyntax block => block.Statements.Any(statement => statement is LocalDeclarationStatementSyntax),
                IfStatementSyntax conditional => conditional.Condition.DescendantNodesAndSelf().Any(expression => expression is DeclarationPatternSyntax),
                ElseClauseSyntax => false,
                _ => true
            }))
            return method;

        StatementSyntax[] chapter = [];
        for (int split = arm.Statements.Count; split > 0; split--)
        {
            StatementSyntax[] prefix = arm.Statements.Take(split).ToArray();
            if (!IsClosedFragment(method, prefix, arm))
                continue;
            chapter = prefix;
            break;
        }
        if (chapter.Length == 0 || chapter.SelectMany(statement => statement.DescendantNodesAndSelf())
            .Any(node => node is LocalDeclarationStatementSyntax or DeclarationExpressionSyntax))
            return method;
        var prefixSpeakers = chapter.SelectMany(statement => statement.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>())
            .Where(invocation => invocation.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "ShowForActorAndWait" or "ShowForActor" })
            .Select(invocation => invocation.ArgumentList.Arguments.FirstOrDefault()?.Expression)
            .OfType<LiteralExpressionSyntax>().Select(literal => literal.Token.Value).Distinct();
        if (prefixSpeakers.Take(2).Count() < 2)
            return method;

        string name = scene.Chapters[entrance];
        BlockSyntax body = SyntaxFactory.Block(method.Body.Statements.OfType<LocalDeclarationStatementSyntax>().Cast<StatementSyntax>().Concat(chapter));
        scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter).WithAttributeLists(default)
            .WithModifiers(Modifiers("internal", method)).WithBody((BlockSyntax)rewriter.Visit(body)!));
        scene.ExtractedChapters.Add((entry, entrance));
        var call = SyntaxFactory.ParseStatement($"await ctx.{scene.Name}.{name}(this);");
        return method.ReplaceNode(guard, guard.WithStatement(arm.WithStatements(SyntaxFactory.List(arm.Statements.Skip(chapter.Length).Prepend(call)))));

        bool IsEntrance(ExpressionSyntax reference, ExpressionSyntax constant)
        {
            return reference is MemberAccessExpressionSyntax && reference.ToString() == selector &&
                (constant is LiteralExpressionSyntax { Token.Value: int number } && entrance == number ||
                 constant is PrefixUnaryExpressionSyntax { Operand: LiteralExpressionSyntax { Token.Value: int magnitude } } unary &&
                 unary.IsKind(SyntaxKind.UnaryMinusExpression) && entrance == -magnitude);
        }
    }
}
