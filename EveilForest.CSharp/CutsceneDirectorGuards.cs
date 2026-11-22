using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private MethodDeclarationSyntax RewriteGuardedPhases(Scene scene, Entry entry, MethodDeclarationSyntax method, ParameterListSyntax parameter, RewriteBody rewriter, string selector)
    {
        foreach (var (chapter, label) in scene.GuardedChapters.Where(pair => pair.Key.Entry == entry))
        {
            IfStatementSyntax? guard = FindDecodedCondition(method, label);
            if (guard == null)
                throw DirectorPhaseError(entry, chapter.Phase, $"has no structured conditional at 0x{label:X}", scene.DirectorMachine!.Director);
            if (Unwrap(guard.Condition) is not BinaryExpressionSyntax test ||
                !test.IsKind(SyntaxKind.EqualsExpression) && !test.IsKind(SyntaxKind.NotEqualsExpression))
                throw DirectorPhaseError(entry, chapter.Phase, $"has unsupported conditional '{guard.Condition}' at 0x{label:X}", scene.DirectorMachine!.Director);

            ExpressionSyntax left = Unwrap(test.Left);
            ExpressionSyntax right = Unwrap(test.Right);
            ExpressionSyntax storage = SyntaxFactory.ParseExpression(selector);
            if (!SyntaxFactory.AreEquivalent(left, storage))
                (left, right) = (right, left);
            ExpressionSyntax value = SyntaxFactory.ParseExpression(chapter.Phase.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!SyntaxFactory.AreEquivalent(left, storage) || !SyntaxFactory.AreEquivalent(right, value))
                throw DirectorPhaseError(entry, chapter.Phase, $"cannot bind '{guard.Condition}' to {selector} at 0x{label:X}", scene.DirectorMachine!.Director);

            StatementSyntax? selected = test.IsKind(SyntaxKind.EqualsExpression) ? guard.Statement : guard.Else?.Statement;
            if (selected is not BlockSyntax block)
                throw DirectorPhaseError(entry, chapter.Phase, $"has no structured equality branch at 0x{label:X}", scene.DirectorMachine!.Director);
            // Keep the original continuation in the handler, including a loop exit or a jump.
            int count = block.Statements.Count;
            while (count > 0 && block.Statements[count - 1] is BreakStatementSyntax or ContinueStatementSyntax or ReturnStatementSyntax or GotoStatementSyntax)
                count--;
            StatementSyntax[] fragment = block.Statements.Take(count).ToArray();
            if (fragment.Length == 0 || !IsClosedFragment(method, fragment, block))
                continue;

            string name = scene.For(entry)![chapter.Phase];
            bool suspends = fragment.SelectMany(statement => statement.DescendantNodesAndSelf()).OfType<AwaitExpressionSyntax>().Any();
            var body = (BlockSyntax)rewriter.Visit(SyntaxFactory.Block(fragment))!;
            scene.Methods.Add(method.WithIdentifier(SyntaxFactory.Identifier(name)).WithParameterList(parameter)
                .WithAttributeLists(default).WithReturnType(SyntaxFactory.ParseTypeName(suspends ? "EventRoutine" : "void"))
                .WithModifiers(suspends ? Modifiers("internal", method) : SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.InternalKeyword))).WithBody(body));
            var call = SyntaxFactory.ParseStatement($"{(suspends ? "await " : "")}ctx.{scene.Name}.{name}(this);");
            method = method.ReplaceNode(block, block.WithStatements(SyntaxFactory.List(block.Statements.Skip(count).Prepend(call))));
            MarkExtractedPhases(scene, entry, [chapter.Phase], name);
        }
        return method;

        static ExpressionSyntax Unwrap(ExpressionSyntax expression)
        {
            while (expression is ParenthesizedExpressionSyntax parentheses)
                expression = parentheses.Expression;
            return expression;
        }
    }
}
