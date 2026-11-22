using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EveilForest.CSharp;

internal sealed class CaseContinuationExport : CSharpSyntaxRewriter
{
    private readonly HashSet<string> _names;

    private CaseContinuationExport(MethodDeclarationSyntax method)
    {
        _names = method.DescendantTokens().Where(token => token.IsKind(SyntaxKind.IdentifierToken)).Select(token => token.ValueText).ToHashSet(StringComparer.Ordinal);
    }

    public static void RewriteExport(string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories))
        {
            string content = File.ReadAllText(file);
            if (!content.Contains("goto case ", StringComparison.Ordinal) && !content.Contains("goto default", StringComparison.Ordinal))
                continue;

            var source = SourceText.From(content);
            SyntaxNode root = CSharpSyntaxTree.ParseText(source).GetRoot();
            var changes = new List<TextChange>();
            foreach (MethodDeclarationSyntax method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                if (new CaseContinuationExport(method).Visit(method) is not MethodDeclarationSyntax rewritten || rewritten == method)
                    continue;
                string text = rewritten.WithoutLeadingTrivia().WithoutTrailingTrivia().NormalizeWhitespace(eol: Environment.NewLine).ToFullString();
                int start = source.Lines.GetLineFromPosition(method.SpanStart).Start;
                string indent = source.ToString(TextSpan.FromBounds(start, method.SpanStart));
                string[] lines = text.Split(Environment.NewLine);
                changes.Add(new TextChange(method.Span, lines[0] + string.Concat(lines.Skip(1).Select(line => Environment.NewLine + (line.Length == 0 ? "" : indent + line)))));
            }
            if (changes.Count != 0)
                File.WriteAllText(file, source.WithChanges(changes).ToString());
        }
    }

    public override SyntaxNode VisitBlock(BlockSyntax node)
    {
        var block = (BlockSyntax)base.VisitBlock(node)!;
        return block.WithStatements(RewriteStatements(block.Statements));
    }

    public override SyntaxNode VisitSwitchSection(SwitchSectionSyntax node)
    {
        var section = (SwitchSectionSyntax)base.VisitSwitchSection(node)!;
        return section.WithStatements(RewriteStatements(section.Statements));
    }

    private SyntaxList<StatementSyntax> RewriteStatements(SyntaxList<StatementSyntax> statements)
    {
        List<StatementSyntax>? rewritten = null;
        for (int index = 0; index < statements.Count; index++)
        {
            StatementSyntax statement = statements[index];
            if (statement is not SwitchStatementSyntax selection)
            {
                rewritten?.Add(statement);
                continue;
            }
            var additions = new List<StatementSyntax>();
            SwitchStatementSyntax structured = RewriteSwitch(selection, additions);
            if (structured != selection)
                rewritten ??= statements.Take(index).ToList();
            rewritten?.Add(structured);
            rewritten?.AddRange(additions);
        }
        return rewritten == null ? statements : SyntaxFactory.List(rewritten);
    }

    private SwitchStatementSyntax RewriteSwitch(SwitchStatementSyntax selection, List<StatementSyntax> additions)
    {
        var actions = new Dictionary<int, (string Name, bool Awaits)>();
        for (int target = selection.Sections.Count - 1; target >= 1; target--)
        {
            var previous = selection.Sections[target - 1];
            var section = selection.Sections[target];
            if (previous.Statements.Count < 2 || previous.Statements[^1] is not GotoStatementSyntax jump || !Targets(jump, section) ||
                section.Statements.Count < 2 || section.Statements is [var action, BreakStatementSyntax] && SharedCaseActionLayout.ReadAction(action) != null)
                continue;

            bool callsNext = target + 1 < selection.Sections.Count && actions.ContainsKey(target + 1) &&
                section.Statements[^1] is GotoStatementSyntax onward && Targets(onward, selection.Sections[target + 1]);
            if (!callsNext && section.Statements[^1] is not BreakStatementSyntax)
                continue;
            var body = section.Statements.Take(section.Statements.Count - 1).ToArray();
            if (!CanMove(body, selection))
                continue;

            string name = Name(section, body);
            bool awaits = body.SelectMany(statement => statement.DescendantNodesAndSelf()).Any(node => node is AwaitExpressionSyntax) ||
                callsNext && actions[target + 1].Awaits;
            actions.Add(target, (name, awaits));
        }
        if (actions.Count == 0)
            return selection;

        var sections = new List<SwitchSectionSyntax>();
        for (int index = 0; index < selection.Sections.Count; index++)
        {
            SwitchSectionSyntax section = selection.Sections[index];
            var rewriter = new References(selection, index, actions);
            var statements = new List<StatementSyntax>();
            foreach (StatementSyntax statement in section.Statements)
            {
                if (statement is GotoStatementSyntax jump && rewriter.Call(jump) is { } call)
                {
                    statements.Add(call);
                    statements.Add(SyntaxFactory.BreakStatement());
                }
                else
                {
                    statements.Add((StatementSyntax)rewriter.Visit(statement)!);
                }
            }
            if (actions.TryGetValue(index, out var action))
            {
                StatementSyntax terminal = statements[^1];
                statements.RemoveAt(statements.Count - 1);
                var local = SyntaxFactory.LocalFunctionStatement(SyntaxFactory.ParseTypeName(action.Awaits ? "EventRoutine" : "void"), action.Name)
                    .WithBody(SyntaxFactory.Block(statements));
                if (action.Awaits)
                    local = local.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.AsyncKeyword)));
                additions.Add(local);
                sections.Add(section.WithStatements(SyntaxFactory.List([MakeCall(action), terminal])));
            }
            else
            {
                sections.Add(section.WithStatements(SyntaxFactory.List(statements)));
            }
        }
        return selection.WithSections(SyntaxFactory.List(sections));
    }

    private static bool CanMove(StatementSyntax[] statements, SwitchStatementSyntax selection)
    {
        foreach (SyntaxNode node in statements.SelectMany(statement => statement.DescendantNodesAndSelf()))
        {
            if (node is ReturnStatementSyntax or LabeledStatementSyntax or LocalDeclarationStatementSyntax or LocalFunctionStatementSyntax)
                return false;
            if (node is GotoStatementSyntax jump && (jump.IsKind(SyntaxKind.GotoStatement) || EnclosingSwitch(jump) == selection))
                return false;
            if (node is BreakStatementSyntax && EnclosingSwitch(node) == selection)
                return false;
            if (node is ContinueStatementSyntax && !node.Ancestors().TakeWhile(ancestor => ancestor != selection).Any(IsLoop))
                return false;
        }
        return true;
    }

    private string Name(SwitchSectionSyntax section, StatementSyntax[] body)
    {
        var calls = body.SelectMany(statement => statement.DescendantNodesAndSelf()).OfType<InvocationExpressionSyntax>().ToArray();
        string action = calls.Any(call => call.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "WalkTo" }) ? "Walk" :
            calls.Any(call => call.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "FadeScreen" }) ? "FadeScreen" :
            calls.Any(call => call.Expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Animation" }, Name.Identifier.ValueText: "Play" }) ? "PlayAnimation" : "Run";
        string label = section.Labels.OfType<CaseSwitchLabelSyntax>().FirstOrDefault()?.Value.ToString() ?? "Default";
        string labelName = new(label.Where(char.IsLetterOrDigit).ToArray());
        string prefix = action + (label == "Default" ? "DefaultCase" : "Case" + labelName);
        string name = prefix;
        for (int ordinal = 2; !_names.Add(name); ordinal++)
            name = prefix + "_" + ordinal;
        return name;
    }

    public static SwitchStatementSyntax? EnclosingSwitch(SyntaxNode node) =>
        node.Ancestors().FirstOrDefault(ancestor => ancestor is SwitchStatementSyntax or LocalFunctionStatementSyntax or AnonymousFunctionExpressionSyntax || IsLoop(ancestor)) as SwitchStatementSyntax;

    private static bool IsLoop(SyntaxNode node) => node is WhileStatementSyntax or DoStatementSyntax or ForStatementSyntax or CommonForEachStatementSyntax;

    private static bool Targets(GotoStatementSyntax jump, SwitchSectionSyntax section) => section.Labels.Any(label => label switch
    {
        CaseSwitchLabelSyntax entry => jump.IsKind(SyntaxKind.GotoCaseStatement) && SyntaxFactory.AreEquivalent(entry.Value, jump.Expression),
        DefaultSwitchLabelSyntax => jump.IsKind(SyntaxKind.GotoDefaultStatement),
        _ => false
    });

    private static StatementSyntax MakeCall((string Name, bool Awaits) action)
    {
        ExpressionSyntax call = SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(action.Name));
        return SyntaxFactory.ExpressionStatement(action.Awaits ? SyntaxFactory.AwaitExpression(call) : call);
    }

    private sealed class References(SwitchStatementSyntax selection, int source, Dictionary<int, (string Name, bool Awaits)> actions) : CSharpSyntaxRewriter
    {
        public StatementSyntax? Call(GotoStatementSyntax jump)
        {
            if (EnclosingSwitch(jump) != selection)
                return null;
            foreach (var (index, action) in actions)
            {
                if (index > source && Targets(jump, selection.Sections[index]))
                    return MakeCall(action).WithTriviaFrom(jump);
            }
            return null;
        }

        public override SyntaxNode? VisitGotoStatement(GotoStatementSyntax node) =>
            Call(node) is { } call ? SyntaxFactory.Block(call, SyntaxFactory.BreakStatement()) : base.VisitGotoStatement(node);
    }
}
