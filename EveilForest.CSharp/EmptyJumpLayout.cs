using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EveilForest.CSharp;

internal static class EmptyJumpLayout
{
    public static bool IsMarker(EmptyStatementSyntax statement) => statement.Parent is BlockSyntax or SwitchSectionSyntax;

    public static void AnnotateExport(string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories))
        {
            string source = File.ReadAllText(file);
            if (!source.Contains("goto ", StringComparison.Ordinal))
                continue;

            var root = CSharpSyntaxTree.ParseText(source).GetRoot();
            var changes = new List<TextChange>();
            foreach (MethodDeclarationSyntax method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
                CollectChanges(method, source, changes);
            if (changes.Count == 0)
                continue;

            SourceText annotated = SourceText.From(source).WithChanges(changes.OrderBy(change => change.Span.Start));
            File.WriteAllText(file, annotated.ToString());
        }
    }

    private static void CollectChanges(MethodDeclarationSyntax method, string source, List<TextChange> changes)
    {
        var jumps = method.DescendantNodes().OfType<GotoStatementSyntax>().Where(jump => jump.IsKind(SyntaxKind.GotoStatement) && jump.Expression is IdentifierNameSyntax)
            .ToLookup(jump => ((IdentifierNameSyntax)jump.Expression!).Identifier.ValueText);
        bool found = false;
        foreach (var group in jumps)
        {
            foreach (GotoStatementSyntax jump in group)
            {
                if (PhysicalSuccessor(jump) is not LabeledStatementSyntax label || label.Identifier.ValueText != group.Key)
                    continue;

                changes.Add(new TextChange(jump.Span, "; /* Preserve the original jump to the next instruction. */"));
                if (group.Count() == 1)
                {
                    TextSpan prefix = TextSpan.FromBounds(label.SpanStart, label.Statement.SpanStart);
                    changes.Add(new TextChange(label.Statement is EmptyStatementSyntax ? WholeLineIfEmpty(source, label.Span) : prefix, ""));
                }
                found = true;
            }
        }
        if (!found)
            return;

        int offset = method.SpanStart;
        int lineStart = source.LastIndexOf('\n', offset) + 1;
        string indent = source[lineStart..offset];
        changes.Add(new TextChange(new TextSpan(offset, 0), "[PreserveEmptyJumps]" + Environment.NewLine + indent));
    }

    private static TextSpan WholeLineIfEmpty(string source, TextSpan span)
    {
        int start = source.LastIndexOf('\n', span.Start) + 1;
        int end = source.IndexOf('\n', span.End);
        if (end >= 0 && source.AsSpan(start, span.Start - start).Trim().IsEmpty && source.AsSpan(span.End, end - span.End).Trim().IsEmpty)
            return TextSpan.FromBounds(start, end + 1);
        return span;
    }

    private static StatementSyntax? PhysicalSuccessor(StatementSyntax statement)
    {
        switch (statement.Parent)
        {
            case BlockSyntax block:
                int next = block.Statements.IndexOf(statement) + 1;
                if (next < block.Statements.Count)
                    return block.Statements[next];
                return block.Parent switch
                {
                    BlockSyntax => PhysicalSuccessor(block),
                    IfStatementSyntax { Else: null } conditional => PhysicalSuccessor(conditional),
                    ElseClauseSyntax { Parent: IfStatementSyntax conditional } => PhysicalSuccessor(conditional),
                    _ => null
                };
            case SwitchSectionSyntax section:
                int sectionNext = section.Statements.IndexOf(statement) + 1;
                if (sectionNext < section.Statements.Count)
                    return section.Statements[sectionNext];
                return null;
            default:
                return null;
        }
    }
}
