using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace EveilForest.CSharp;

public static partial class EventProjectCompiler
{
    private static CSharpCompilation LowerCaseContinuations(CSharpCompilation compilation)
    {
        var vm = RequireType(compilation, "Vm");
        var result = compilation;
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            SyntaxNode root = tree.GetRoot();
            if (!root.DescendantNodes().Any(node => node is LocalFunctionStatementSyntax))
                continue;
            var rewriter = new CaseContinuations(compilation.GetSemanticModel(tree), vm);
            SyntaxNode rewritten = rewriter.Visit(root)!;
            if (rewritten != root)
                result = result.ReplaceSyntaxTree(tree, tree.WithRootAndOptions(rewritten, tree.Options));
        }
        if (result != compilation && result.GetDiagnostics().FirstOrDefault(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error) is { } error)
            throw new InvalidDataException(error.ToString());
        return result;
    }

    private sealed class CaseContinuations(SemanticModel model, INamedTypeSymbol vm) : CSharpSyntaxRewriter
    {
        private readonly Dictionary<IMethodSymbol, LocalFunctionStatementSyntax> _functions = new(SymbolEqualityComparer.Default);
        private readonly Dictionary<SwitchStatementSyntax, Dictionary<IMethodSymbol, SwitchSectionSyntax>> _canonical = new();
        private readonly HashSet<IMethodSymbol> _active = new(SymbolEqualityComparer.Default);
        private readonly List<SyntaxNode> _relocationSites = new();
        private int _returnOrdinal;

        public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var functions = node.DescendantNodes().OfType<LocalFunctionStatementSyntax>().ToArray();
            if (functions.Length == 0)
                return node;

            _functions.Clear();
            _canonical.Clear();
            _returnOrdinal = 0;
            foreach (LocalFunctionStatementSyntax function in functions)
            {
                if (function.Ancestors().Any(ancestor => ancestor is LocalFunctionStatementSyntax or AnonymousFunctionExpressionSyntax))
                    throw Invalid(function, "Declare case continuations directly in a handler's block or switch section");
                if (model.GetDeclaredSymbol(function) is IMethodSymbol symbol && symbol.Parameters.IsEmpty && !symbol.IsGenericMethod &&
                    (symbol.ReturnsVoid || IsSupportType(symbol.ReturnType, vm, "EventRoutine")) && function.Body != null)
                    _functions.Add(symbol, function);
            }
            ValidateCallSites(node);
            foreach (SwitchStatementSyntax selection in node.DescendantNodes().OfType<SwitchStatementSyntax>())
            {
                var candidates = new Dictionary<IMethodSymbol, List<SwitchSectionSyntax>>(SymbolEqualityComparer.Default);
                foreach (SwitchSectionSyntax section in selection.Sections)
                {
                    if (section.Statements is not [var call, BreakStatementSyntax] || BoundCall(call) is not { } method ||
                        _functions[method].Parent != selection.Parent || CaseLabel(section) == null ||
                        _functions[method].Body!.DescendantNodes().Any(statement => statement is ReturnStatementSyntax or LabeledStatementSyntax or LocalDeclarationStatementSyntax))
                        continue;
                    if (!candidates.TryGetValue(method, out var sections))
                        candidates.Add(method, sections = new());
                    sections.Add(section);
                }
                _canonical.Add(selection, candidates.Where(pair => pair.Value.Count == 1)
                    .ToDictionary(pair => pair.Key, pair => pair.Value[0], (IEqualityComparer<IMethodSymbol>)SymbolEqualityComparer.Default));
            }
            return base.VisitMethodDeclaration(node);
        }

        public override SyntaxNode? VisitLocalFunctionStatement(LocalFunctionStatementSyntax node) =>
            model.GetDeclaredSymbol(node) is IMethodSymbol method && _functions.ContainsKey(method) ? null : node;

        public override SyntaxNode VisitSwitchSection(SwitchSectionSyntax node)
        {
            var selection = (SwitchStatementSyntax)node.Parent!;
            if (node.Statements is [var call, BreakStatementSyntax terminal] && BoundCall(call) is { } method &&
                _canonical[selection].TryGetValue(method, out var section) && section == node)
            {
                SyntaxList<StatementSyntax> content = Expand(method, call, selection, node);
                if (content.LastOrDefault() is not GotoStatementSyntax)
                    content = content.Add(terminal);
                return node.WithStatements(content);
            }

            if (TailTransfer(node.Statements, selection, node.SpanStart) is { } jump)
                return node.WithStatements(RewritePrefix(node.Statements, jump));
            return base.VisitSwitchSection(node)!;
        }

        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            if (CaseContinuationExport.EnclosingSwitch(node) is not { } selection ||
                TailTransfer(node.Statements, selection, node.SpanStart) is not { } jump)
                return base.VisitBlock(node)!;
            if (node.Statements.Count == 2 && node.Parent is IfStatementSyntax { Else: null } conditional && conditional.Statement == node)
                return jump.WithTriviaFrom(node);
            return node.WithStatements(RewritePrefix(node.Statements, jump));
        }

        public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            if (BoundCall(node) is not { } method)
                return base.VisitExpressionStatement(node)!;
            SyntaxList<StatementSyntax> statements = Expand(method, node, null, null);
            return SyntaxFactory.Block(statements).WithTriviaFrom(node);
        }

        private SyntaxList<StatementSyntax> Expand(IMethodSymbol method, SyntaxNode use, SwitchStatementSyntax? selection, SwitchSectionSyntax? section)
        {
            LocalFunctionStatementSyntax function = _functions[method];
            if (!_active.Add(method))
                throw Invalid(function, "Recursive case continuations cannot be lowered to EB");
            _relocationSites.Add(use);
            try
            {
                foreach (SyntaxNode site in _relocationSites)
                    ValidateCaptureBindings(function, site);
                SyntaxList<StatementSyntax> original = function.Body!.Statements;
                if (selection != null && section != null && original.Count > 0 && BoundCall(original[^1]) is { } tail &&
                    Forward(tail, selection, section.SpanStart) is { } jump)
                    return SyntaxFactory.List(original.SkipLast(1).Select(statement => (StatementSyntax)Visit(statement)!).Append(jump));

                var expanded = SyntaxFactory.Block(original.Select(statement => (StatementSyntax)Visit(statement)!));
                ReturnStatementSyntax[] returns = expanded.DescendantNodes(node => node is not AnonymousFunctionExpressionSyntax and not LocalFunctionStatementSyntax)
                    .OfType<ReturnStatementSyntax>().ToArray();
                if (returns.Length == 0)
                    return expanded.Statements;

                string label = NextReturnLabel(function);
                expanded = expanded.ReplaceNodes<BlockSyntax, StatementSyntax>(returns, (source, _) =>
                    SyntaxFactory.GotoStatement(SyntaxKind.GotoStatement, SyntaxFactory.IdentifierName(label)).WithTriviaFrom(source));
                return expanded.Statements.Add(SyntaxFactory.LabeledStatement(label, SyntaxFactory.EmptyStatement()));
            }
            finally
            {
                _relocationSites.RemoveAt(_relocationSites.Count - 1);
                _active.Remove(method);
            }
        }

        private void ValidateCaptureBindings(LocalFunctionStatementSyntax function, SyntaxNode use)
        {
            foreach (IdentifierNameSyntax identifier in function.Body!.DescendantNodes().OfType<IdentifierNameSyntax>())
            {
                if (identifier.Parent is MemberAccessExpressionSyntax member && member.Name == identifier ||
                    identifier.Parent is NameColonSyntax or NameEqualsSyntax || model.GetSymbolInfo(identifier).Symbol is not { } symbol ||
                    symbol is ILabelSymbol || symbol is IMethodSymbol { MethodKind: MethodKind.LocalFunction } ||
                    symbol.DeclaringSyntaxReferences.Any(declaration => declaration.SyntaxTree == function.SyntaxTree && function.Span.Contains(declaration.Span)))
                    continue;

                string name = identifier.Identifier.ValueText;
                if (!model.LookupSymbols(use.SpanStart, name: name).Any(candidate => SymbolEqualityComparer.Default.Equals(candidate, symbol)))
                    throw Invalid(use, $"The case continuation cannot be lowered because its captured name '{name}' binds differently here");
            }
        }

        private void ValidateCallSites(MethodDeclarationSyntax handler)
        {
            foreach (IdentifierNameSyntax identifier in handler.DescendantNodes().OfType<IdentifierNameSyntax>())
            {
                if (model.GetSymbolInfo(identifier).Symbol is not IMethodSymbol method || !_functions.ContainsKey(method))
                    continue;
                if (identifier.Parent is not InvocationExpressionSyntax invocation || invocation.Expression != identifier ||
                    invocation.Parent is not ExpressionStatementSyntax and not AwaitExpressionSyntax { Parent: ExpressionStatementSyntax })
                    throw Invalid(identifier, "Call a case continuation as a standalone statement");
            }
        }

        private string NextReturnLabel(LocalFunctionStatementSyntax function)
        {
            var method = function.FirstAncestorOrSelf<MethodDeclarationSyntax>()!;
            string label;
            do
            {
                label = "AfterCaseContinuation_" + ++_returnOrdinal;
            } while (method.DescendantNodes().OfType<LabeledStatementSyntax>().Any(statement => statement.Identifier.ValueText == label));
            return label;
        }

        private SyntaxList<StatementSyntax> RewritePrefix(SyntaxList<StatementSyntax> statements, GotoStatementSyntax jump) =>
            SyntaxFactory.List(statements.SkipLast(2).Select(statement => (StatementSyntax)Visit(statement)!).Append(jump.WithTriviaFrom(statements[^2])));

        private GotoStatementSyntax? TailTransfer(SyntaxList<StatementSyntax> statements, SwitchStatementSyntax selection, int source) =>
            statements.Count >= 2 && statements[^1] is BreakStatementSyntax && BoundCall(statements[^2]) is { } method ? Forward(method, selection, source) : null;

        private GotoStatementSyntax? Forward(IMethodSymbol method, SwitchStatementSyntax selection, int source)
        {
            if (!_canonical[selection].TryGetValue(method, out var target) || target.SpanStart <= source)
                return null;
            return CaseLabel(target) switch
            {
                CaseSwitchLabelSyntax label => SyntaxFactory.GotoStatement(SyntaxKind.GotoCaseStatement, default,
                    SyntaxFactory.Token(SyntaxKind.GotoKeyword), SyntaxFactory.Token(SyntaxKind.CaseKeyword), label.Value.WithoutTrivia(), SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                DefaultSwitchLabelSyntax => SyntaxFactory.GotoStatement(SyntaxKind.GotoDefaultStatement, default,
                    SyntaxFactory.Token(SyntaxKind.GotoKeyword), SyntaxFactory.Token(SyntaxKind.DefaultKeyword), null, SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                _ => null
            };
        }

        private static SwitchLabelSyntax? CaseLabel(SwitchSectionSyntax section) =>
            section.Labels.FirstOrDefault(label => label is CaseSwitchLabelSyntax or DefaultSwitchLabelSyntax);

        private IMethodSymbol? BoundCall(StatementSyntax statement)
        {
            if (statement is not ExpressionStatementSyntax expression)
                return null;
            IOperation? operation = model.GetOperation(expression.Expression);
            bool awaited = operation is IAwaitOperation;
            if (awaited)
                operation = ((IAwaitOperation)operation!).Operation;
            if (operation is not IInvocationOperation { Arguments.IsEmpty: true } invocation || !_functions.ContainsKey(invocation.TargetMethod))
                return null;
            if (awaited == invocation.TargetMethod.ReturnsVoid)
                throw Invalid(expression, "Await an EventRoutine case continuation at its original call site");
            return invocation.TargetMethod;
        }
    }
}
