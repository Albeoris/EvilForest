using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EveilForest.CSharp;

internal sealed partial class CutsceneExport
{
    public void SimplifyChoreographyJoins()
    {
        foreach (var scene in Scenes.Where(scene => scene.DirectorMachine != null))
        {
            int index = scene.Methods.FindIndex(method => method.Identifier.ValueText == "Direct");
            if (index < 0)
                continue;
            MethodDeclarationSyntax director = scene.Methods[index];
            var setup = director.DescendantNodes().OfType<InvocationExpressionSyntax>().SingleOrDefault(call => call.Expression.ToString() == "Choreography");
            if (setup?.ArgumentList.Arguments is not [_, { Expression: ParenthesizedLambdaExpressionSyntax { Body: ExpressionSyntax cue } }, ..] argumentsOfSetup)
                continue;
            ExpressionSyntax? pulse = argumentsOfSetup.Count == 3 ? (argumentsOfSetup[2].Expression as ParenthesizedLambdaExpressionSyntax)?.ExpressionBody : null;
            var methods = scene.Methods.ToDictionary(method => method.Identifier.ValueText, StringComparer.Ordinal);
            var sharedNames = scene.SharedChapters.ToDictionary(chapter => scene.For(chapter.Key.Entry)![chapter.Key.Phase], chapter => chapter.Value, StringComparer.Ordinal);
            var choreography = scene.Methods.Where(method => method == director || IsChoreographyRoute(method)).ToArray();
            var rows = choreography.SelectMany(method => method.DescendantNodes().OfType<InvocationExpressionSyntax>()).Where(call => call.Expression is MemberAccessExpressionSyntax
                { Expression: IdentifierNameSyntax { Identifier.ValueText: "play" }, Name.Identifier.ValueText: "Together" or "RunUntilCue" or "Idle" or "Finish" or "FallThrough" }).ToArray();
            var uses = rows.SelectMany(row => row.ArgumentList.Arguments.Skip(RoleOffset(row))).Select(SharedRoleName).ToLookup(name => name);
            var changes = new Dictionary<InvocationExpressionSyntax, InvocationExpressionSyntax>();
            foreach (var row in rows)
            {
                var arguments = row.ArgumentList.Arguments;
                int roleOffset = RoleOffset(row);
                var roles = arguments.Skip(roleOffset).ToList();
                string[] names = roles.Select(SharedRoleName).ToArray();
                if (names.Any(name => !methods.ContainsKey(name)))
                    throw new InvalidDataException($"The phase in {scene.Name}.{director.Identifier} has an unresolved role.");
                if (names.Length == 0)
                    continue;
                for (int i = 0; i < names.Length; i++)
                {
                    var role = (InvocationExpressionSyntax)roles[i].Expression;
                    ArgumentSyntax action = role.ArgumentList.Arguments[1];
                    if (RoleName(roles[i]) != names[i])
                        roles[i] = roles[i].ReplaceNode(action.Expression, SyntaxFactory.IdentifierName(names[i]));
                }
                var bodies = names.Select(name => methods[name].Body!.Statements).ToArray();
                var aliases = bodies.Select(body => body.TakeWhile(IsChoreographyAlias).Count()).ToArray();
                var signal = ChoreographyBarrier.Signal(cue.ToString());
                var senders = Enumerable.Range(0, names.Length).Where(i => bodies[i].LastOrDefault() is { } last && SyntaxFactory.AreEquivalent(last, signal) &&
                    !bodies[i].SelectMany(statement => statement.DescendantNodesAndSelf()).OfType<ReturnStatementSyntax>().Any()).ToArray();
                var receiver = (MemberAccessExpressionSyntax)row.Expression;
                string operation = receiver.Name.Identifier.ValueText;
                bool unique = operation is "Together" or "RunUntilCue" && names.All(name => uses[name].Count() == 1) &&
                    roles.All(role => role.Expression is InvocationExpressionSyntax { ArgumentList.Arguments.Count: 2 });
                if (pulse != null && unique && senders.Length == names.Length && names.Length > 1 && bodies[0].Count >= 4 &&
                    bodies[0][^4] is ExpressionStatementSyntax { Expression: PostfixUnaryExpressionSyntax { Operand: MemberAccessExpressionSyntax counter } })
                {
                    var tail = ChoreographyBarrier.Complete(counter.ToString(), pulse.ToString(), cue.ToString());
                    var begin = ChoreographyBarrier.Begin(counter.ToString(), names.Length);
                    int[] leaders = Enumerable.Range(0, bodies.Length).Where(i => Matches(bodies[i].Skip(aliases[i]).Take(2), begin)).ToArray();
                    if (leaders is [var leader] && bodies.All(body => Matches(body.TakeLast(4), tail)))
                    {
                        for (int i = 0; i < names.Length; i++)
                            Trim(i, i == leader ? 2 : 0, 4);
                        First(leader);
                        roles.Insert(0, SyntaxFactory.Argument(SyntaxFactory.ParseExpression($"Barrier(() => {counter})")));
                        operation = "Together";
                    }
                }
                else if (unique && senders is [var completing])
                {
                    Trim(completing, 0, 1);
                    First(completing);
                    operation = roles.Count == 1 ? "Together" : "Until";
                }
                changes.Add(row, row.WithExpression(receiver.WithName(SyntaxFactory.IdentifierName(operation)))
                    .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments.Take(roleOffset).Concat(roles)))));

                void First(int i)
                {
                    var role = roles[i];
                    roles.RemoveAt(i);
                    roles.Insert(0, role);
                }

                void Trim(int i, int prefix, int suffix)
                {
                    var method = methods[names[i]];
                    int start = aliases[i] + prefix;
                    var statements = bodies[i].Take(aliases[i]).Concat(bodies[i].Skip(start).Take(bodies[i].Count - start - suffix));
                    scene.Methods[scene.Methods.IndexOf(method)] = method.WithBody(method.Body!.WithStatements(SyntaxFactory.List(statements)));
                }
            }
            foreach (var method in choreography)
            {
                var edits = changes.Keys.Where(call => call.FirstAncestorOrSelf<MethodDeclarationSyntax>() == method);
                scene.Methods[scene.Methods.IndexOf(method)] = method.ReplaceNodes(edits, (original, _) => changes[original]);
            }

            string SharedRoleName(ArgumentSyntax argument)
            {
                string name = RoleName(argument);
                return sharedNames.GetValueOrDefault(name, name);
            }
        }

        static bool Matches(IEnumerable<StatementSyntax> actual, StatementSyntax[] expected) =>
            actual.Count() == expected.Length && actual.Zip(expected).All(pair => SyntaxFactory.AreEquivalent(pair.First, pair.Second));

        static int RoleOffset(InvocationExpressionSyntax row) => ((MemberAccessExpressionSyntax)row.Expression).Name.Identifier.ValueText switch
        {
            "Finish" => 2,
            "FallThrough" => 3,
            _ => 1
        };

        static string RoleName(ArgumentSyntax argument) => argument.Expression is InvocationExpressionSyntax
            { ArgumentList.Arguments: [_, { Expression: IdentifierNameSyntax method }, ..] } ? method.Identifier.ValueText : "";
    }
}
