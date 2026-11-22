using EvilForest.EventSource;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace EveilForest.CSharp;

public static partial class EventProjectCompiler
{
    private static CSharpCompilation LowerSharedCaseActions(CSharpCompilation compilation)
    {
        var vm = RequireType(compilation, "Vm");
        var lowered = compilation;
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            if (!tree.GetText().ToString().Contains("ShareSwitchCaseActions", StringComparison.Ordinal))
                continue;

            SemanticModel model = compilation.GetSemanticModel(tree);
            SyntaxNode root = tree.GetRoot();
            var transfers = new Dictionary<SyntaxNode, (ExpressionSyntax Label, bool Direct)>();
            foreach (MethodDeclarationSyntax method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                if (model.GetDeclaredSymbol(method) is not IMethodSymbol caller || !HasSupportAttribute(caller, vm, nameof(ShareSwitchCaseActionsAttribute)))
                    continue;
                foreach (BlockSyntax block in method.DescendantNodes().OfType<BlockSyntax>())
                {
                    if (block.Statements.Count < 2 || block.Statements[^1] is not BreakStatementSyntax ||
                        SharedCaseActionLayout.ReadAction(block.Statements[^2]) is not { } action ||
                        SharedCaseActionLayout.EnclosingSwitch(block) is not { } selection ||
                        BoundCaseAction(action, caller, vm, model) is not { } target)
                        continue;

                    SwitchSectionSyntax[] destinations = selection.Sections.Where(section => section.Statements is [var statement, BreakStatementSyntax] &&
                        SharedCaseActionLayout.ReadAction(statement) is { } existing && SyntaxFactory.AreEquivalent(existing, action) &&
                        SymbolEqualityComparer.Default.Equals(BoundCaseAction(existing, caller, vm, model), target)).ToArray();
                    if (destinations is not [var destination] || destination.SpanStart <= block.SpanStart ||
                        destination.Labels.OfType<CaseSwitchLabelSyntax>().FirstOrDefault() is not { } label)
                        continue;

                    bool direct = block.Statements.Count == 2 && block.Parent is IfStatementSyntax { Else: null } conditional && conditional.Statement == block;
                    transfers.Add(block, (label.Value, direct));
                }
                foreach (SwitchStatementSyntax selection in method.DescendantNodes().OfType<SwitchStatementSyntax>())
                {
                    for (int index = 0; index + 1 < selection.Sections.Count; index++)
                    {
                        SwitchSectionSyntax section = selection.Sections[index];
                        SwitchSectionSyntax destination = selection.Sections[index + 1];
                        if (section.Statements.Count < 3 || section.Statements[^1] is not BreakStatementSyntax ||
                            destination.Statements is not [var statement, BreakStatementSyntax] ||
                            SharedCaseActionLayout.ReadAction(section.Statements[^2]) is not { } action || SharedCaseActionLayout.ReadAction(statement) is not { } existing ||
                            BoundCaseAction(action, caller, vm, model) is not { } target ||
                            !SymbolEqualityComparer.Default.Equals(target, BoundCaseAction(existing, caller, vm, model)) ||
                            destination.Labels.OfType<CaseSwitchLabelSyntax>().FirstOrDefault() is not { } label)
                            continue;

                        transfers.Add(section, (label.Value, false));
                    }
                }
            }
            if (transfers.Count == 0)
                continue;

            SyntaxNode replacement = root.ReplaceNodes(transfers.Keys, (original, node) =>
            {
                var transfer = transfers[original];
                StatementSyntax branch = SyntaxFactory.GotoStatement(SyntaxKind.GotoCaseStatement, default,
                    SyntaxFactory.Token(SyntaxKind.GotoKeyword), SyntaxFactory.Token(SyntaxKind.CaseKeyword), transfer.Label.WithoutTrivia(), SyntaxFactory.Token(SyntaxKind.SemicolonToken));
                if (transfer.Direct)
                    return branch.WithTriviaFrom(node);

                SyntaxList<StatementSyntax> statements = node is BlockSyntax block ? block.Statements : ((SwitchSectionSyntax)node).Statements;
                var rewritten = SyntaxFactory.List(statements.SkipLast(2).Append(branch.WithTriviaFrom(statements[^2])));
                return node is BlockSyntax body ? body.WithStatements(rewritten) : ((SwitchSectionSyntax)node).WithStatements(rewritten);
            });
            lowered = lowered.ReplaceSyntaxTree(tree, tree.WithRootAndOptions(replacement, tree.Options));
        }
        return lowered;
    }

    private static IMethodSymbol? BoundCaseAction(ExpressionStatementSyntax statement, IMethodSymbol caller, INamedTypeSymbol vm, SemanticModel model)
    {
        if (model.GetOperation(statement.Expression) is not IAwaitOperation { Operation: IInvocationOperation invocation } ||
            invocation.Arguments is not [var argument] || argument.Parameter!.RefKind != RefKind.None || !IsCurrentParticipant(argument.Value))
            return null;

        IMethodSymbol target = invocation.TargetMethod;
        return !target.IsStatic && !target.IsGenericMethod && !target.IsVirtual && !target.IsOverride &&
            DerivesFrom(target.ContainingType, vm, "EventCutscene") && IsBoundCutsceneReceiver(invocation.Instance, target.ContainingType, caller, vm, model.Compilation)
            ? target
            : null;
    }
}
