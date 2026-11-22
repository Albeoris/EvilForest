using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EveilForest.CSharp;

internal static class SharedCaseActionLayout
{
    public static void RewriteExport(string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories))
        {
            string source = File.ReadAllText(file);
            if (!source.Contains("goto case ", StringComparison.Ordinal))
                continue;

            var root = CSharpSyntaxTree.ParseText(source).GetRoot();
            var edits = new List<TextChange>();
            foreach (MethodDeclarationSyntax method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
                CollectChanges(method, source, edits);
            if (edits.Count != 0)
                File.WriteAllText(file, SourceText.From(source).WithChanges(edits.OrderBy(edit => edit.Span.Start)).ToString());
        }
    }

    private static void CollectChanges(MethodDeclarationSyntax method, string source, List<TextChange> edits)
    {
        int before = edits.Count;
        foreach (GotoStatementSyntax jump in method.DescendantNodes().OfType<GotoStatementSyntax>())
        {
            if (!jump.IsKind(SyntaxKind.GotoCaseStatement) || EnclosingSwitch(jump) is not { } selection)
                continue;
            IfStatementSyntax? conditional = jump.Parent as IfStatementSyntax;
            bool directBranch = conditional is { Else: null } && conditional.Statement == jump;
            var sibling = jump.Parent as SwitchSectionSyntax;
            if (!directBranch && sibling == null && (jump.Parent is not BlockSyntax { Statements.Count: > 1 } block || block.Statements[^1] != jump))
                continue;

            SwitchSectionSyntax? target = selection.Sections.FirstOrDefault(section => section.Labels.OfType<CaseSwitchLabelSyntax>()
                .Any(label => SyntaxFactory.AreEquivalent(label.Value, jump.Expression)));
            if (target == null || target.SpanStart <= jump.SpanStart || target.Statements is not [var statement, BreakStatementSyntax] || ReadAction(statement) is not { } action ||
                selection.DescendantNodes().OfType<ExpressionStatementSyntax>().Count(candidate => SyntaxFactory.AreEquivalent(candidate, action)) != 1)
                continue;
            if (sibling != null && (sibling.Statements.Count < 2 || sibling.Statements[^1] != jump || selection.Sections.IndexOf(target) != selection.Sections.IndexOf(sibling) + 1))
                continue;

            string newline = Environment.NewLine;
            if (directBranch)
            {
                int start = LineStart(source, jump.SpanStart);
                int ifStart = LineStart(source, conditional!.SpanStart);
                if (start == ifStart || !source.AsSpan(start, jump.SpanStart - start).Trim().IsEmpty || !source.AsSpan(ifStart, conditional.SpanStart - ifStart).Trim().IsEmpty)
                    continue;
                string indent = source[ifStart..conditional.SpanStart];
                string body = indent + "    ";
                edits.Add(new TextChange(TextSpan.FromBounds(start, jump.Span.End), $"{indent}{{{newline}{body}{action}{newline}{body}break;{newline}{indent}}}"));
            }
            else
            {
                string indent = source[LineStart(source, jump.SpanStart)..jump.SpanStart];
                if (indent.AsSpan().Trim().IsEmpty)
                    edits.Add(new TextChange(jump.Span, $"{action}{newline}{indent}break;"));
            }
        }
        if (edits.Count == before)
            return;

        int offset = method.SpanStart;
        string methodIndent = source[LineStart(source, offset)..offset];
        edits.Add(new TextChange(new TextSpan(offset, 0), "[ShareSwitchCaseActions]" + Environment.NewLine + methodIndent));
    }

    public static ExpressionStatementSyntax? ReadAction(StatementSyntax statement) =>
        statement is ExpressionStatementSyntax
        {
            Expression: AwaitExpressionSyntax
            {
                Expression: InvocationExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Expression: MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "ctx" } }
                    },
                    ArgumentList.Arguments: [{ Expression: ThisExpressionSyntax }]
                }
            }
        } action ? action : null;

    public static SwitchStatementSyntax? EnclosingSwitch(SyntaxNode node) =>
        node.Ancestors().FirstOrDefault(ancestor => ancestor is SwitchStatementSyntax or WhileStatementSyntax or DoStatementSyntax or ForStatementSyntax or
            CommonForEachStatementSyntax or AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax) as SwitchStatementSyntax;

    private static int LineStart(string source, int offset) => source.LastIndexOf('\n', offset) + 1;
}
