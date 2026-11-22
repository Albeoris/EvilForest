using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    private const int RendezvousStatementCount = 4;

    public void FinalizeParallelPhases(string directory)
    {
        foreach (Scene scene in Scenes)
        {
            if (scene.DirectorMachine?.Choreography != null || scene.ParallelTracks is not [var first, var second] ||
                !scene.ExtractedChapters.Contains(first) || !scene.ExtractedChapters.Contains(second))
                continue;

            string firstName = scene.For(first.Entry)![first.Phase];
            string secondName = scene.For(second.Entry)![second.Phase];
            string firstActor = _symbols.Participants[first.Entry.Slot].Name;
            string secondActor = _symbols.Participants[second.Entry.Slot].Name;
            int firstIndex = scene.Methods.FindIndex(method => method.Identifier.ValueText == firstName);
            int secondIndex = scene.Methods.FindIndex(method => method.Identifier.ValueText == secondName);
            if (firstIndex < 0 || secondIndex < 0 || firstIndex == secondIndex)
                continue;

            MethodDeclarationSyntax firstMethod = scene.Methods[firstIndex];
            MethodDeclarationSyntax secondMethod = scene.Methods[secondIndex];
            if (!TrySplitRendezvous(firstMethod, secondMethod, out BlockSyntax completion))
                continue;

            // Wait until both original phase arms have been exported before
            // replacing either call. A partially extracted group stays explicit.
            var firstFile = Path.Combine(directory, $"{first.Entry.Slot:D2}_{firstActor}.cs");
            var secondFile = Path.Combine(directory, $"{second.Entry.Slot:D2}_{secondActor}.cs");
            if (!File.Exists(firstFile) || !File.Exists(secondFile))
                continue;

            var used = Scenes.Where(candidate => candidate.Name == scene.Name)
                .SelectMany(candidate => candidate.Methods.Select(method => method.Identifier.ValueText))
                .ToHashSet(StringComparer.Ordinal);
            used.Add(scene.Name);
            string play = ReserveName(scene.ParallelName ?? "PlayTogether", used);
            string complete = ReserveName("WaitForEveryone", used);
            if (RewriteEntry(firstFile, scene.Name, firstName, play) is not { } firstSource ||
                RewriteEntry(secondFile, scene.Name, secondName, play) is not { } secondSource)
                continue;

            File.WriteAllText(firstFile, firstSource);
            File.WriteAllText(secondFile, secondSource);
            scene.Methods[firstIndex] = Track(firstMethod);
            scene.Methods[secondIndex] = Track(secondMethod);
            var entry = (MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($$"""
                internal async EventRoutine {{play}}(EventActor participant)
                {
                    await Together(participant, {{complete}}, Track(ctx.Scene.{{firstActor}}, {{firstName}}), Track(ctx.Scene.{{secondActor}}, {{secondName}}));
                }
                """)!;
            scene.Methods.Insert(0, entry);
            scene.Methods.Add(firstMethod.WithIdentifier(SyntaxFactory.Identifier(complete))
                .WithParameterList(SyntaxFactory.ParameterList()).WithModifiers(Modifiers("private", firstMethod)).WithBody(completion));
        }
    }

    private static bool TrySplitRendezvous(MethodDeclarationSyntax first, MethodDeclarationSyntax second, out BlockSyntax complete)
    {
        complete = null!;
        if (first.Body!.Statements.Count < RendezvousStatementCount || second.Body!.Statements.Count < RendezvousStatementCount)
            return false;

        StatementSyntax[] suffix = first.Body.Statements.TakeLast(RendezvousStatementCount).ToArray();
        if (!suffix.Zip(second.Body.Statements.TakeLast(RendezvousStatementCount), (left, right) => left.IsEquivalentTo(right)).All(same => same) ||
            suffix is not
            [
                ExpressionStatementSyntax { Expression: PostfixUnaryExpressionSyntax decrement },
                ForStatementSyntax { Declaration: null, Initializers.Count: 0, Incrementors.Count: 0, Condition: BinaryExpressionSyntax remaining, Statement: BlockSyntax spin },
                ForStatementSyntax { Declaration: null, Initializers.Count: 0, Incrementors.Count: 0, Condition: BinaryExpressionSyntax observed, Statement: BlockSyntax poll },
                ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax end }
            ] ||
            !decrement.IsKind(SyntaxKind.PostDecrementExpression) || !remaining.IsKind(SyntaxKind.GreaterThanExpression) ||
            !SameLocationField(remaining.Left, decrement.Operand) || remaining.Right is not LiteralExpressionSyntax { Token.Value: 0 } ||
            spin.Statements is not [ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax signal }, var spinWait] ||
            poll.Statements is not [var pollWait] || !spinWait.IsEquivalentTo(pollWait) || !IsOneFrameWait(pollWait) ||
            !signal.IsKind(SyntaxKind.SimpleAssignmentExpression) || !signal.Right.IsKind(SyntaxKind.TrueLiteralExpression) || !observed.IsKind(SyntaxKind.EqualsExpression) ||
            !SameLocationField(observed.Left, signal.Left) || !observed.Right.IsKind(SyntaxKind.TrueLiteralExpression) ||
            !end.IsKind(SyntaxKind.SimpleAssignmentExpression) || LocationField(end.Left) == null || !end.Right.IsKind(SyntaxKind.TrueLiteralExpression) ||
            first.Body.Statements.SkipLast(RendezvousStatementCount).All(statement => statement is not ExpressionStatementSyntax
            {
                Expression: AssignmentExpressionSyntax { Right: LiteralExpressionSyntax { Token.Value: 2 } } reset
            } || !reset.IsKind(SyntaxKind.SimpleAssignmentExpression) || !SameLocationField(reset.Left, decrement.Operand)))
            return false;

        complete = SyntaxFactory.Block(suffix);
        return true;
    }

    private static bool SameLocationField(ExpressionSyntax left, ExpressionSyntax right) =>
        LocationField(left) is { } field && LocationField(right) == field;

    private static string? LocationField(ExpressionSyntax expression) =>
        expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Location" } } member
            ? member.Name.Identifier.ValueText : null;

    private static bool IsOneFrameWait(StatementSyntax statement) => statement is ExpressionStatementSyntax
    {
        Expression: AwaitExpressionSyntax
        {
            Expression: InvocationExpressionSyntax
            {
                Expression: MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "Engine" }, Name.Identifier.ValueText: "Wait" },
                ArgumentList.Arguments: [ { Expression: LiteralExpressionSyntax { Token.Value: 1 } } ]
            }
        }
    };

    private static MethodDeclarationSyntax Track(MethodDeclarationSyntax method) => method.WithModifiers(Modifiers("private", method))
        .WithBody(method.Body!.WithStatements(SyntaxFactory.List(method.Body.Statements.SkipLast(RendezvousStatementCount))));

    private static string ReserveName(string suggested, HashSet<string> used)
    {
        string name = suggested;
        for (int suffix = 2; !used.Add(name); suffix++)
            name = $"{suggested}{suffix}";
        return name;
    }

    private static string? RewriteEntry(string file, string scene, string track, string play)
    {
        SyntaxNode root = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();
        var calls = root.DescendantNodes().OfType<AwaitExpressionSyntax>().Select(awaiter => awaiter.Expression)
            .OfType<InvocationExpressionSyntax>().Where(call => call is
            {
                Expression: MemberAccessExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "ctx" } } receiver
                } method,
                ArgumentList.Arguments: [{ Expression: ThisExpressionSyntax }]
            } && receiver.Name.Identifier.ValueText == scene && method.Name.Identifier.ValueText == track).Take(2).ToArray();
        if (calls is not [var call] || call.Expression is not MemberAccessExpressionSyntax member)
            return null;
        return root.ReplaceNode(member.Name, SyntaxFactory.IdentifierName(play).WithTriviaFrom(member.Name)).ToFullString();
    }
}

