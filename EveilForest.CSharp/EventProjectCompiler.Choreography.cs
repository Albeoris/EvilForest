using EvilForest.EventSource;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace EveilForest.CSharp;

public static partial class EventProjectCompiler
{
    private static readonly SyntaxAnnotation ChoiceDispatch = new();
    private static readonly SyntaxAnnotation OmitUnreachablePollingExit = new();
    private static readonly SyntaxAnnotation ChoreographyAction = new();

    private static CSharpCompilation LowerChoreography(CSharpCompilation compilation)
    {
        INamedTypeSymbol vm = RequireType(compilation, "Vm");
        if (vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.SceneChoreography") == null)
            return compilation;
        return new ChoreographyLowering(compilation, vm).Rewrite();
    }

    private sealed class ChoreographyLowering(CSharpCompilation compilation, INamedTypeSymbol vm)
    {
        private const string Engine = $"global::{SourceNamespace}.Engine";
        private readonly Dictionary<INamedTypeSymbol, Play> _plays = new(SymbolEqualityComparer.Default);
        private readonly HashSet<IMethodSymbol> _started = new(SymbolEqualityComparer.Default);
        private readonly Dictionary<IMethodSymbol, BlockSyntax> _bodies = new(SymbolEqualityComparer.Default);
        private readonly Dictionary<IMethodSymbol, Phase?> _routes = new(SymbolEqualityComparer.Default);
        private readonly HashSet<SyntaxNode> _routeReferences = [];
        private readonly HashSet<SyntaxNode> _used = new();
        private readonly List<IInvocationOperation> _calls = [];

        public CSharpCompilation Rewrite()
        {
            ReadCalls();
            foreach (IInvocationOperation call in _calls.Where(call => IsCall(call, "Choreography", "EventCutscene")))
                ReadPlay(call);
            foreach (IInvocationOperation call in _calls.Where(call => IsCall(call, "Perform", "EventCutscene") || IsCall(call, "PerformPhase", "EventCutscene")))
                ReadParticipant(call);
            ValidateRouteUses();
            foreach (IInvocationOperation call in _calls)
            {
                if (!_used.Contains(call.Syntax))
                    throw Invalid(call.Syntax, "Use choreography operations in a director declaration or participant loop");
            }

            foreach (Play play in _plays.Values)
            {
                if (!IsAttachedDirector(play.Director) && !_started.Contains(play.Director))
                    throw Invalid(play.Source, "Start the choreography director through its registered cutscene binding");
                foreach (ITypeSymbol participant in play.Tracks.Keys)
                {
                    if (!play.Handlers.ContainsKey(participant))
                        throw Invalid(play.Source, $"The choreography participant {participant.Name} needs a loop that performs this cutscene");
                }
            }

            foreach (var group in _bodies.Keys.Concat(_routes.Keys).GroupBy(method => method.DeclaringSyntaxReferences.Single().SyntaxTree))
            {
                SyntaxTree tree = group.Key;
                var methods = group.ToDictionary(method => (MethodDeclarationSyntax)method.DeclaringSyntaxReferences.Single().GetSyntax(), method => _bodies.GetValueOrDefault(method));
                SyntaxNode root = new MethodRewriter(methods).Visit(tree.GetRoot())!;
                compilation = compilation.ReplaceSyntaxTree(tree, tree.WithRootAndOptions(root, tree.Options));
            }

            if (_bodies.Count != 0 && compilation.GetDiagnostics().FirstOrDefault(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error) is { } error)
                throw new InvalidDataException(error.ToString());
            return compilation;
        }

        private void ReadCalls()
        {
            foreach (SyntaxTree tree in compilation.SyntaxTrees)
            {
                SemanticModel model = compilation.GetSemanticModel(tree);
                foreach (InvocationExpressionSyntax call in tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    string? name = call.Expression switch
                    {
                        MemberAccessExpressionSyntax member => member.Name.Identifier.ValueText,
                        SimpleNameSyntax simple => simple.Identifier.ValueText,
                        _ => null
                    };
                    if (name is not ("Choreography" or "Perform" or "PerformPhase" or "Role" or "Barrier" or "BeforePoll" or "Together" or "Until" or "RunUntilCue" or "Finish" or "FallThrough" or "Idle" or "Routes" or "ContinueWith" or "RepeatFrom" or "Choose" or "Case" or "StartDirector") ||
                        model.GetOperation(call) is not IInvocationOperation operation)
                        continue;
                    if (IsStartDirector(operation.TargetMethod, vm) && operation.Arguments is [{ Value: IDelegateCreationOperation
                        { Target: IMethodReferenceOperation reference } }])
                    {
                        _started.Add(reference.Method);
                    }
                    else if ((name is "Choreography" or "Perform" or "PerformPhase" or "Role" or "Barrier") && IsSupportType(operation.TargetMethod.ContainingType, vm, "EventCutscene") ||
                        (name is "BeforePoll" or "Together" or "Until" or "RunUntilCue" or "Finish" or "FallThrough" or "Idle" or "Routes" or "ContinueWith" or "RepeatFrom" or "Choose" or "Case") && IsSupportType(operation.TargetMethod.ContainingType, vm, "SceneChoreography"))
                    {
                        _calls.Add(operation);
                    }
                }
            }
        }

        private void ReadPlay(IInvocationOperation call)
        {
            SemanticModel model = call.SemanticModel!;
            if (call.Syntax.Parent is not EqualsValueClauseSyntax { Parent: VariableDeclaratorSyntax local } ||
                local.Parent is not VariableDeclarationSyntax { Variables.Count: 1, Parent: LocalDeclarationStatementSyntax { Parent: BlockSyntax body } declaration } ||
                body.Parent is not MethodDeclarationSyntax source || model.GetDeclaredSymbol(source) is not IMethodSymbol director ||
                model.GetDeclaredSymbol(local) is not ILocalSymbol playLocal || (!IsDirectorMethod(director, vm) && !IsAttachedDirector(director)) || !director.IsAsync ||
                call.Instance is not IInstanceReferenceOperation { ReferenceKind: InstanceReferenceKind.ContainingTypeInstance } ||
                !GetCutsceneBindings(compilation, vm).ContainsKey(director.ContainingType) || !InArgumentOrder(call) ||
                call.Arguments is not [var selectorArgument, var cueArgument, ..])
                throw Invalid(call.Syntax, "Declare the choreography locally in an async director on the registered cutscene");

            int index = body.Statements.IndexOf(declaration);
            ILocalSymbol? sceneAlias = index == 1 ? ReadSceneAlias(body.Statements[0], model, director) : null;
            if (index != 0 && sceneAlias == null)
                throw Invalid(call.Syntax, "Only a fixed scene binding may precede the choreography declaration");

            Storage selector = ReadCell(selectorArgument, director, bit: false);
            Storage cue = ReadCell(cueArgument, director, bit: true);
            Storage? pulse = call.Arguments.Length == 3 ? ReadCell(call.Arguments[2], director, bit: true) : null;
            if (selector.Overlaps(cue) || pulse != null && (selector.Overlaps(pulse) || cue.Overlaps(pulse)))
                throw Invalid(call.Syntax, "The choreography selector, cue, and pulse must occupy distinct map storage");
            _used.Add(call.Syntax);

            var play = new Play(director, source, selector, cue, pulse);
            if (!_plays.TryAdd(director.ContainingType, play))
                throw Invalid(call.Syntax, "Each registered cutscene can declare one choreography");

            int start = index + 1;
            IMethodSymbol? pollWork = null;
            if (start < body.Statements.Count && body.Statements[start] is ExpressionStatementSyntax beforePoll &&
                model.GetOperation(beforePoll.Expression) is IInvocationOperation work && IsCall(work, "BeforePoll", "SceneChoreography"))
            {
                if (!HasReceiver(work, playLocal) || work.Arguments is not [var action])
                    throw Invalid(beforePoll, "Declare BeforePoll once on the current choreography before its phases");
                pollWork = ReadAction(action.Value, director, null);
                if (!pollWork.ReturnsVoid)
                    throw Invalid(action.Syntax, "BeforePoll requires a synchronous cutscene method");
                _used.Add(work.Syntax);
                start++;
            }

            if (start + 1 == body.Statements.Count && ReadAwaitedCall(body.Statements[start], model, playLocal) is { } routesCall &&
                IsCall(routesCall, "Routes", "SceneChoreography"))
            {
                if (routesCall.Arguments is not [{ Value: IArrayCreationOperation { IsImplicit: true, Initializer.ElementValues: { Length: > 0 } routes } }])
                    throw Invalid(routesCall.Syntax, "Declare choreography routes with cutscene method groups");
                _used.Add(routesCall.Syntax);
                var entries = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);
                foreach (IOperation route in routes)
                {
                    IMethodSymbol method = ReadRouteMethod(route, play);
                    if (!entries.Add(method))
                        throw Invalid(route.Syntax, "Declare each choreography route entry once");
                    ReadRoute(method, play, route.Syntax);
                }
                if (entries.Count == 1 && play.Choices.Count == 0 && play.Repeats.Count == 0 && play.FallThrough.Count == 0)
                    throw Invalid(routesCall.Syntax, "Declare at least two choreography routes unless a route ends with Choose or RepeatFrom");

                // Register ContinueWith chains before following edges that can return to a declared route.
                while (play.Choices.Count != 0 || play.Repeats.Count != 0)
                {
                    while (play.Choices.TryDequeue(out Choice? choice))
                    {
                        foreach (ChoiceArm arm in choice.Arms)
                            ReadRoute(arm.Target, play, arm.Reference);
                    }
                    if (play.Repeats.TryDequeue(out var repeat))
                        repeat.Source.Next = ReadRoute(repeat.Target, play, repeat.Reference);
                }
            }
            else
            {
                ReadChain(body, start, play, director, playLocal, sceneAlias, isRoute: false);
                if (play.Phases.Count < 2)
                    throw Invalid(body, "Declare at least one choreography phase followed by Finish or Idle");
            }

            var sections = new List<SwitchSectionSyntax>();
            foreach (Phase phase in play.Phases.Values)
            {
                StatementSyntax? transition = null;
                if (phase.Choice is { } choice)
                {
                    var arms = choice.Arms.Select(arm => Section(arm.Key, Assign(selector.Source, Number(_routes[arm.Target]!.Value)))).ToList();
                    transition = Dispatch(director, choice.Selector.Source, choice.Selector.Token, arms).WithAdditionalAnnotations(ChoiceDispatch);
                }
                else if (phase.Next is { } next)
                {
                    transition = Assign(selector.Source, Number(next.Value));
                }

                if (transition != null)
                {
                    var completed = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, cue.Source, SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression));
                    var reset = Assign(cue.Source, SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression));
                    sections.Add(Section(phase.Value, SyntaxFactory.IfStatement(completed, SyntaxFactory.Block(transition, reset))));
                }
                else if (phase.Finish != null)
                {
                    sections.Add(Section(phase.Value, Call(phase.Finish, "this", null).WithAdditionalAnnotations(ChoreographyAction)));
                }
            }
            StatementSyntax? resetPulse = pulse == null ? null : Assign(pulse.Source, SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression));
            StatementSyntax? workCall = pollWork == null ? null : Call(pollWork, "this", null).WithAdditionalAnnotations(ChoreographyAction);
            StatementSyntax? prefix = workCall == null ? resetPulse : resetPulse == null ? workCall : SyntaxFactory.Block(workCall, resetPulse);
            if (!SceneChoreography.HasValidFallThrough(play.FallThrough, play.Phases.Keys.ToHashSet()))
                throw Invalid(body, "FallThrough must reach a declared phase without a same-tick cycle");
            _bodies.Add(director, Loop(director, selector.Source, selector.Token, sections, prefix, play.FallThrough));
        }

        private bool IsAttachedDirector(IMethodSymbol method) =>
            DerivesFrom(method.ContainingType, vm, "EventCutscene") &&
            !method.IsStatic && !method.IsVirtual && !method.IsOverride && !method.IsGenericMethod &&
            method.MethodKind == MethodKind.Ordinary && IsSupportType(method.ReturnType, vm, "EventRoutine") &&
            FindAttribute(method, RequireType(compilation, "ScriptAttribute")) is { } script && AttributeValue<ushort>(script, 0) == 1 &&
            method.Parameters is [{ RefKind: RefKind.None, IsOptional: false } receiver] &&
            DerivesFrom(receiver.Type, vm, "EventParticipant") &&
            FindAttribute(receiver.Type, RequireType(compilation, "ObjectSlotAttribute")) is { } slot && AttributeValue<int>(slot, 0) == 0;

        private Phase ReadRoute(IMethodSymbol method, Play play, SyntaxNode reference)
        {
            if (_routes.TryGetValue(method, out Phase? entry))
                return entry ?? throw Invalid(reference, "Choreography route continuations must not form a cycle");

            _routes.Add(method, null);
            var body = ((MethodDeclarationSyntax)method.DeclaringSyntaxReferences.Single().GetSyntax()).Body!;
            SemanticModel model = compilation.GetSemanticModel(body.SyntaxTree);
            ILocalSymbol? sceneAlias = body.Statements.Count == 0 ? null : ReadSceneAlias(body.Statements[0], model, method);
            entry = ReadChain(body, sceneAlias == null ? 0 : 1, play, method, method.Parameters[0], sceneAlias, isRoute: true);
            _routes[method] = entry;
            return entry;
        }

        private Phase ReadChain(BlockSyntax body, int start, Play play, IMethodSymbol method, ISymbol receiver, ILocalSymbol? sceneAlias, bool isRoute)
        {
            SemanticModel model = compilation.GetSemanticModel(body.SyntaxTree);
            Phase? first = null;
            Phase? previous = null;
            for (int step = start; step < body.Statements.Count; step++)
            {
                StatementSyntax statement = body.Statements[step];
                if (ReadAwaitedCall(statement, model, receiver) is not { } invocation)
                    throw Invalid(statement, "Await each choreography phase in sequence with a unique phase value that fits the map selector");
                _used.Add(invocation.Syntax);

                bool last = step == body.Statements.Count - 1;
                if (IsCall(invocation, "ContinueWith", "SceneChoreography"))
                {
                    if (!isRoute || !last || first == null || previous == null || invocation.Arguments is not [var continuation])
                        throw Invalid(statement, "Place ContinueWith after the route's own phases as its last statement");
                    IMethodSymbol tail = ReadRouteMethod(continuation.Value, play);
                    previous.Next = ReadRoute(tail, play, continuation.Syntax);
                    return first;
                }
                if (IsCall(invocation, "RepeatFrom", "SceneChoreography"))
                {
                    if (!isRoute || !last || first == null || previous == null || invocation.Arguments is not [var continuation])
                        throw Invalid(statement, "Place RepeatFrom after the route's own phases as its last statement");
                    IMethodSymbol tail = ReadRouteMethod(continuation.Value, play);
                    play.Repeats.Enqueue((previous, tail, continuation.Syntax));
                    return first;
                }
                if (IsCall(invocation, "Choose", "SceneChoreography"))
                {
                    if (!isRoute || !last || first == null || previous == null)
                        throw Invalid(statement, "Place Choose after the route's own phases as its last statement");
                    Choice choice = ReadChoice(invocation, play, method, receiver);
                    previous.Choice = choice;
                    play.Choices.Enqueue(choice);
                    return first;
                }

                if (invocation.Arguments.Length == 0 || invocation.Arguments[0].Value.ConstantValue is not { HasValue: true, Value: int value } ||
                    !play.Selector.CanStore(value))
                    throw Invalid(statement, "Use a constant phase value that fits the map selector");
                var phase = new Phase(value);
                if (!play.Phases.TryAdd(value, phase))
                    throw Invalid(statement, "Declare each choreography phase once across all routes");
                first ??= phase;
                if (previous != null)
                    previous.Next = phase;
                previous = phase;

                if (last && IsCall(invocation, "Finish", "SceneChoreography") && invocation.Arguments is [_, var finishAction, ..])
                {
                    phase.Finish = ReadAction(finishAction.Value, play.Director, null);
                    if (invocation.Arguments.Length > 2)
                    {
                        if (invocation.Arguments is not [_, _, { Value: IArrayCreationOperation { IsImplicit: true, Initializer.ElementValues: { } roles } }])
                            throw Invalid(statement, "Declare each Finish role inline with Role");
                        ReadRoles(roles, play, method, value, sceneAlias, null, signalOnFirst: false);
                    }
                    return first;
                }
                if (last && IsCall(invocation, "FallThrough", "SceneChoreography") && invocation.Arguments is [_, var work, var destination, var fallThroughRoles])
                {
                    if (destination.Value.ConstantValue is not { HasValue: true, Value: int next } ||
                        fallThroughRoles.Value is not IArrayCreationOperation { IsImplicit: true, Initializer.ElementValues: { } tracks })
                        throw Invalid(statement, "Declare a constant FallThrough destination and each role inline with Role");
                    phase.Finish = ReadAction(work.Value, play.Director, null);
                    play.FallThrough.Add(value, next);
                    ReadRoles(tracks, play, method, value, sceneAlias, counter: null, signalOnFirst: false);
                    return first;
                }
                if (last && IsCall(invocation, "Idle", "SceneChoreography"))
                {
                    ReadPhase(invocation, play, method, value, sceneAlias, idle: true);
                    return first;
                }
                ReadPhase(invocation, play, method, value, sceneAlias);
            }

            throw Invalid(body, isRoute
                ? "Declare a route with at least one phase and end with Finish, Idle, ContinueWith, RepeatFrom, or Choose"
                : "Declare at least one choreography phase followed by Finish or Idle");
        }

        private Choice ReadChoice(IInvocationOperation invocation, Play play, IMethodSymbol method, ISymbol receiver)
        {
            if (invocation.Arguments is not [var value, { Value: IArrayCreationOperation { IsImplicit: true, Initializer.ElementValues: { Length: > 0 } cases } }])
                throw Invalid(invocation.Syntax, "Choose requires an integer map property and at least one inline Case");

            Storage choice = ReadCell(value, method, bit: false);
            var keys = new HashSet<int>();
            var arms = new List<ChoiceArm>();
            foreach (IOperation item in cases)
            {
                if (item is not IInvocationOperation call || !IsCall(call, "Case", "SceneChoreography") ||
                    !HasReceiver(call, receiver) || !InArgumentOrder(call) || call.Arguments is not [var key, var target] ||
                    key.Value.ConstantValue is not { HasValue: true, Value: int caseValue } || !keys.Add(caseValue))
                    throw Invalid(item.Syntax, "Declare inline Case calls on the same choreography with unique constant integer keys");

                _used.Add(call.Syntax);
                IMethodSymbol route = ReadRouteMethod(target.Value, play);
                arms.Add(new ChoiceArm(caseValue, route, target.Syntax));
            }
            return new Choice(choice, arms);
        }

        private ILocalSymbol? ReadSceneAlias(StatementSyntax statement, SemanticModel model, IMethodSymbol caller) =>
            statement is LocalDeclarationStatementSyntax
                { UsingKeyword.RawKind: 0, Declaration.Variables: [VariableDeclaratorSyntax { Initializer.Value: { } initializer } alias] } &&
            model.GetDeclaredSymbol(alias) is ILocalSymbol localAlias && model.GetOperation(initializer) is IPropertyReferenceOperation root &&
            IsBoundSceneRoot(root, caller, vm) ? localAlias : null;

        private static IInvocationOperation? ReadAwaitedCall(StatementSyntax statement, SemanticModel model, ISymbol receiver)
        {
            if (statement is not ExpressionStatementSyntax { Expression: AwaitExpressionSyntax awaiter } ||
                model.GetOperation(awaiter.Expression) is not IInvocationOperation invocation || !InArgumentOrder(invocation))
                return null;
            return HasReceiver(invocation, receiver) ? invocation : null;
        }

        private static bool HasReceiver(IInvocationOperation invocation, ISymbol receiver)
        {
            ISymbol? actual = invocation.Instance switch
            {
                ILocalReferenceOperation local => local.Local,
                IParameterReferenceOperation parameter => parameter.Parameter,
                _ => null
            };
            return SymbolEqualityComparer.Default.Equals(receiver, actual);
        }

        private IMethodSymbol ReadRouteMethod(IOperation operation, Play play)
        {
            if (operation is not IDelegateCreationOperation { IsImplicit: true, Target: IMethodReferenceOperation
                { Instance: IInstanceReferenceOperation { ReferenceKind: InstanceReferenceKind.ContainingTypeInstance }, Method: var method } reference } ||
                !SymbolEqualityComparer.Default.Equals(method.ContainingType, play.Director.ContainingType) || method.IsGenericMethod ||
                method.IsStatic || method.IsVirtual || method.IsOverride || !method.IsAsync || method.MethodKind != MethodKind.Ordinary ||
                HasSupportAttribute(method, vm, "ScriptAttribute") || !IsSupportType(method.ReturnType, vm, "EventRoutine") ||
                method.Parameters is not [{ RefKind: RefKind.None, IsOptional: false } parameter] ||
                !IsSupportType(parameter.Type, vm, "SceneChoreography") ||
                method.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not MethodDeclarationSyntax { Body: not null })
                throw Invalid(operation.Syntax, "Use an ordinary async cutscene method group with one SceneChoreography parameter");
            _routeReferences.Add(reference.Syntax);
            return method;
        }

        private void ValidateRouteUses()
        {
            if (_routes.Count == 0)
                return;
            var routeNames = _routes.Keys.Select(method => method.Name).ToHashSet(StringComparer.Ordinal);
            foreach (SyntaxTree tree in compilation.SyntaxTrees)
            {
                SemanticModel model = compilation.GetSemanticModel(tree);
                foreach (IdentifierNameSyntax name in tree.GetRoot().DescendantNodes().OfType<IdentifierNameSyntax>())
                {
                    if (!routeNames.Contains(name.Identifier.ValueText))
                        continue;
                    ExpressionSyntax expression = name.Parent is MemberAccessExpressionSyntax member && member.Name == name ? member : name;
                    if (_routeReferences.Contains(expression) || _routeReferences.Contains(name))
                        continue;
                    SymbolInfo target = model.GetSymbolInfo(expression);
                    if (target.Symbol is IMethodSymbol method && _routes.ContainsKey(method) ||
                        target.CandidateSymbols.OfType<IMethodSymbol>().Any(_routes.ContainsKey))
                        throw Invalid(expression, "Reference a choreography route only through Routes or ContinueWith, through RepeatFrom, or as a Case target in Choose");
                }
            }
        }

        private void ReadPhase(IInvocationOperation invocation, Play play, IMethodSymbol caller, int phase, ILocalSymbol? sceneAlias, bool idle = false)
        {
            if (idle && invocation.Arguments.Length == 1)
                return;
            if (!IsSupportType(invocation.TargetMethod.ContainingType, vm, "SceneChoreography") ||
                invocation.Arguments[^1].Value is not IArrayCreationOperation { IsImplicit: true, Initializer.ElementValues: { } tracks })
                throw Invalid(invocation.Syntax, "Declare each choreography phase with Together, Until, or RunUntilCue and end with Finish or Idle");

            IOperation[] roles;
            Storage? counter = null;
            bool signalOnFirst = false;
            switch (invocation.TargetMethod.Name)
            {
                case "Idle" when idle && invocation.Arguments.Length == 2:
                case "RunUntilCue" when invocation.Arguments.Length == 2:
                    roles = tracks.ToArray();
                    break;
                case "Together" when invocation.Arguments.Length == 2 && tracks.Length == 1:
                    roles = tracks.ToArray();
                    signalOnFirst = true;
                    break;
                case "Together" when invocation.Arguments is [_, { Value: IInvocationOperation barrier }, _] &&
                    IsCall(barrier, "Barrier", "EventCutscene") && barrier.Instance is IInstanceReferenceOperation { ReferenceKind: InstanceReferenceKind.ContainingTypeInstance } &&
                    barrier.Arguments is [var storage] && tracks.Length > 0:
                    if (play.Pulse == null)
                        throw Invalid(barrier.Syntax, "A phase barrier requires choreography pulse storage");
                    counter = ReadCell(storage, caller, bit: false);
                    if (counter.Overlaps(play.Selector) || counter.Overlaps(play.Cue) || counter.Overlaps(play.Pulse) || !counter.CanStore(tracks.Length))
                        throw Invalid(barrier.Syntax, "Select a separate map counter that can hold the number of barrier roles");
                    _used.Add(barrier.Syntax);
                    roles = tracks.ToArray();
                    break;
                case "Until" when invocation.Arguments is [_, var leader, _]:
                    roles = [leader.Value, .. tracks];
                    signalOnFirst = true;
                    break;
                default:
                    throw Invalid(invocation.Syntax, "Use one role with Together, a map barrier with Together for multiple roles, or a completing role followed by background roles with Until");
            }

            ReadRoles(roles, play, caller, phase, sceneAlias, counter, signalOnFirst);
        }

        private void ReadRoles(IReadOnlyList<IOperation> roles, Play play, IMethodSymbol caller, int phase, ILocalSymbol? sceneAlias, Storage? counter, bool signalOnFirst)
        {
            var actors = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
            for (int index = 0; index < roles.Count; index++)
            {
                IOperation role = roles[index];
                if (role is not IInvocationOperation track || !IsCall(track, "Role", "EventCutscene") || !InArgumentOrder(track) ||
                    track.Instance is not IInstanceReferenceOperation { ReferenceKind: InstanceReferenceKind.ContainingTypeInstance } ||
                    track.Arguments is not [var actor, var action, ..] || actor.Value is not IPropertyReferenceOperation prototype ||
                    !IsDirectRoleReceiver(prototype.Instance, caller, sceneAlias) ||
                    !IsBoundActionParticipant(actor.Value, caller, vm) || IsControlledActor(actor.Value, vm) ||
                    ReadSceneParticipant(actor.Value, vm, compilation) is not { } participant || !actors.Add(participant.Declaration))
                    throw Invalid(role.Syntax, "Bind each choreography phase role to a different fixed scene participant");

                _used.Add(track.Syntax);
                IMethodSymbol method = ReadAction(action.Value, play.Director, participant.Declaration);
                bool signals = signalOnFirst && index == 0;
                int? continuation = null;
                if (track.Arguments.Length == 3)
                {
                    if (track.Arguments[2].Value.ConstantValue is not { HasValue: true, Value: int next } || counter != null || signals)
                        throw Invalid(role.Syntax, "A linked role requires a constant phase and must declare any completion cue in its action");
                    continuation = next;
                }
                if (!play.Tracks.TryGetValue(participant.Declaration, out var chapters))
                    play.Tracks.Add(participant.Declaration, chapters = []);
                chapters.Add(new Chapter(phase, method, counter, index == 0 ? roles.Count : 0, signals, continuation));
            }
        }

        private bool IsDirectRoleReceiver(IOperation? receiver, IMethodSymbol director, ILocalSymbol? sceneAlias) => receiver switch
        {
            IPropertyReferenceOperation root => IsBoundSceneRoot(root, director, vm),
            ILocalReferenceOperation local => SymbolEqualityComparer.Default.Equals(local.Local, sceneAlias),
            _ => false
        };

        private void ReadParticipant(IInvocationOperation call)
        {
            SemanticModel model = call.SemanticModel!;
            bool performsPhase = IsCall(call, "PerformPhase", "EventCutscene");
            if (call.Syntax.Parent is not AwaitExpressionSyntax { Parent: ExpressionStatementSyntax statement } ||
                ReadParticipantHandler(statement, performsPhase) is not { } source ||
                model.GetDeclaredSymbol(source) is not IMethodSymbol handler || !handler.IsAsync ||
                FindAttribute(handler, RequireType(compilation, "ScriptAttribute")) is not { } script || AttributeValue<ushort>(script, 0) != 1 ||
                !DerivesFrom(handler.ContainingType, vm, "EventParticipant") || handler.Parameters.Length != 0 ||
                !HasSupportAttribute(handler.ContainingType, vm, "ObjectSlotAttribute") ||
                call.Instance is not IPropertyReferenceOperation { Instance.Syntax: ExpressionSyntax context } binding ||
                binding.Property.Type is not INamedTypeSymbol cutscene || !_plays.TryGetValue(cutscene, out Play? play) ||
                !IsBoundCutsceneReceiver(binding, cutscene, handler, vm, compilation) ||
                !InArgumentOrder(call) || call.Arguments is not [{ Value.Syntax: ThisExpressionSyntax }, ..] ||
                !play.Handlers.TryAdd(handler.ContainingType, handler))
                throw Invalid(call.Syntax, "Await Perform(this) as the participant handler or await PerformPhase(this) in its script body");

            _used.Add(call.Syntax);
            string cue = Access(play.Cue);
            var sections = new List<SwitchSectionSyntax>();
            var chapters = play.Tracks.GetValueOrDefault(handler.ContainingType) ?? [];
            var links = chapters.Where(chapter => chapter.Continuation.HasValue).ToDictionary(chapter => chapter.Phase, chapter => chapter.Continuation!.Value);
            if (!SceneChoreography.HasValidFallThrough(links, chapters.Select(chapter => chapter.Phase).ToHashSet()))
                throw Invalid(call.Syntax, "A role continuation must reach a declared phase for the same participant without a same-tick cycle");
            if (links.Count > 0 && performsPhase)
                throw Invalid(call.Syntax, "Use Perform for a participant with linked choreography roles");
            // An edited phase can separate from a shared arm by selecting another action or completion behavior.
            foreach (var group in chapters.GroupBy(chapter => (chapter.Method, chapter.Counter, BeginsWith: chapter.Counter == null ? 0 : chapter.BeginsWith, chapter.Signal,
                SeparatePhase: links.Count == 0 ? (int?)null : chapter.Phase)))
            {
                Chapter chapter = group.First();
                StatementSyntax action = Call(chapter.Method, call.Instance.Syntax.ToString(), "this");
                var steps = new List<StatementSyntax>();
                if (chapter.Counter == null)
                {
                    steps.Add(action);
                    if (chapter.Signal)
                        steps.Add(ChoreographyBarrier.Signal(cue));
                }
                else
                {
                    string counter = Access(chapter.Counter);
                    if (chapter.BeginsWith != 0)
                        steps.AddRange(ChoreographyBarrier.Begin(counter, chapter.BeginsWith, Engine));
                    steps.Add(action);
                    steps.AddRange(ChoreographyBarrier.Complete(counter, Access(play.Pulse!), cue, Engine));
                }
                var labels = group.Select(item => (SwitchLabelSyntax)SyntaxFactory.CaseSwitchLabel(Number(item.Phase)));
                SwitchSectionSyntax section = SyntaxFactory.SwitchSection(SyntaxFactory.List(labels), SyntaxFactory.List([.. steps, SyntaxFactory.BreakStatement()]));
                if (steps.Count == 1 && HasSupportAttribute(chapter.Method, vm, nameof(OmitUnreachablePollingExitAttribute)))
                    section = section.WithAdditionalAnnotations(OmitUnreachablePollingExit);
                sections.Add(section);
            }
            BlockSyntax? fallback = null;
            if (call.Arguments.Length > 1)
            {
                if (call.Arguments[1].Value.Syntax is not ParenthesizedLambdaExpressionSyntax { ParameterList.Parameters.Count: 0, Block: { } body } ||
                    body.DescendantNodes().OfType<ReturnStatementSyntax>().Any())
                    throw Invalid(call.Syntax, "Write work for unmatched phases in a parameterless block lambda without return statements");
                fallback = body;
            }
            if (fallback != null && sections.Count != 0)
                sections.Add(SyntaxFactory.SwitchSection(SyntaxFactory.SingletonList<SwitchLabelSyntax>(SyntaxFactory.DefaultSwitchLabel()),
                    SyntaxFactory.List(fallback.Statements.Append(SyntaxFactory.BreakStatement()))));
            ExpressionSyntax selector = SyntaxFactory.ParseExpression(Access(play.Selector));
            if (performsPhase)
            {
                StatementSyntax dispatch;
                if (sections.Count == 0)
                {
                    dispatch = fallback ?? SyntaxFactory.Block();
                }
                else if (sections is [{ Labels: [CaseSwitchLabelSyntax label] } single] && !HasSwitchLayout(handler, play.Selector.Token))
                {
                    var condition = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, selector, label.Value);
                    dispatch = SyntaxFactory.IfStatement(condition, SyntaxFactory.Block(single.Statements.SkipLast(1)));
                }
                else
                {
                    dispatch = Dispatch(handler, selector, play.Selector.Token, sections);
                }
                _bodies.Add(handler, source.Body!.ReplaceNode(statement, dispatch.WithTriviaFrom(statement)));
            }
            else
            {
                _bodies.Add(handler, Loop(handler, selector, play.Selector.Token, sections, fallThrough: links, useFinalExitLayout: true));
            }

            string Access(Storage storage) => $"({context}).{Name(storage.Holder)}.{Name(storage.Property)}";
        }

        private static MethodDeclarationSyntax? ReadParticipantHandler(ExpressionStatementSyntax statement, bool performsPhase)
        {
            if (performsPhase)
            {
                MethodDeclarationSyntax? handler = statement.FirstAncestorOrSelf<MethodDeclarationSyntax>();
                if (handler != null && statement.Ancestors().TakeWhile(ancestor => ancestor != handler)
                    .Any(ancestor => ancestor is AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax))
                    return null;
                return handler;
            }
            return statement.Parent is BlockSyntax { Statements: [var only], Parent: MethodDeclarationSyntax method } && only == statement ? method : null;
        }

        private bool HasSwitchLayout(IMethodSymbol method, EVExpressionToken selector) =>
            method.GetAttributes().Any(layout => layout.AttributeClass is { } type && IsSupportType(type, vm, "SwitchLayoutAttribute") &&
                AttributeValue<string>(layout, 0).StartsWith(SwitchLayout.Key([selector], []), StringComparison.Ordinal));

        private Storage ReadCell(IArgumentOperation argument, IMethodSymbol director, bool bit)
        {
            SemanticModel model = argument.SemanticModel!;
            if (argument.Value.Syntax is not ParenthesizedLambdaExpressionSyntax { ParameterList.Parameters.Count: 0, ExpressionBody: { } expression } ||
                model.GetOperation(expression) is not IPropertyReferenceOperation { Instance: { } receiver } storage ||
                storage.Property is { IsVirtual: true } or { IsOverride: true } or { GetMethod: null } or { SetMethod: null } ||
                ReadStorageReference(storage, vm) is not EVExpressionToken token ||
                EVVariable.Decode(token) is not { Scope: EVStorageScope.Map } variable || variable.UsesBitAddress != bit ||
                ReadMapHolder(receiver, director, new HashSet<ISymbol>(SymbolEqualityComparer.Default)) is not { } holder)
                throw Invalid(argument.Syntax, "Select a writable map storage property on the current context using a zero-argument lambda");
            return new Storage(expression, storage.Property, holder, token, variable);
        }

        private IPropertySymbol? ReadMapHolder(IOperation receiver, IMethodSymbol director, HashSet<ISymbol> visited)
        {
            if (receiver is not IPropertyReferenceOperation { Instance: { } instance } access)
                return null;
            if (IsDefaultStorageBinding(access, vm) && IsContextReference(instance, director, vm))
                return access.Property;
            if (instance is not IInstanceReferenceOperation || !SymbolEqualityComparer.Default.Equals(access.Property.ContainingType, director.ContainingType) ||
                access.Property.IsVirtual || access.Property.IsOverride || !visited.Add(access.Property) ||
                access.Property.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not PropertyDeclarationSyntax
                    { ExpressionBody.Expression: { } expression } ||
                compilation.GetSemanticModel(expression.SyntaxTree).GetOperation(expression) is not { } next)
                return null;
            return ReadMapHolder(next, director, visited);
        }

        private IMethodSymbol ReadAction(IOperation operation, IMethodSymbol director, ITypeSymbol? participant)
        {
            if (operation is not IDelegateCreationOperation { IsImplicit: true, Target: IMethodReferenceOperation
                { Instance: IInstanceReferenceOperation { ReferenceKind: InstanceReferenceKind.ContainingTypeInstance }, Method: var method } } ||
                !SymbolEqualityComparer.Default.Equals(method.ContainingType, director.ContainingType) || method.IsGenericMethod ||
                method.IsStatic || method.IsVirtual || method.IsOverride || method.MethodKind != MethodKind.Ordinary ||
                HasSupportAttribute(method, vm, "ScriptAttribute") ||
                !(method.ReturnsVoid && !method.IsAsync || IsSupportType(method.ReturnType, vm, "EventRoutine")) ||
                method.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not MethodDeclarationSyntax { Body: not null } ||
                (participant == null ? method.Parameters.Length != 0 : method.Parameters is not [var parameter] || parameter.RefKind != RefKind.None ||
                    !SymbolEqualityComparer.Default.Equals(parameter.Type, participant)) || SymbolEqualityComparer.Default.Equals(method, director))
                throw Invalid(operation.Syntax, "Use an ordinary cutscene method group with the exact participant parameter, or a parameterless Finish method");
            return method;
        }

        private bool IsCall(IInvocationOperation call, string name, string type) =>
            call.TargetMethod.Name == name && IsSupportType(call.TargetMethod.ContainingType, vm, type);

        private static bool InArgumentOrder(IInvocationOperation call) =>
            call.Arguments.Select(argument => argument.Parameter!.Ordinal).SequenceEqual(Enumerable.Range(0, call.Arguments.Length));

        private BlockSyntax Loop(IMethodSymbol method, ExpressionSyntax selector, EVExpressionToken token,
            List<SwitchSectionSyntax> sections, StatementSyntax? prefix = null, IReadOnlyDictionary<int, int>? fallThrough = null, bool useFinalExitLayout = false)
        {
            StatementSyntax wait = SyntaxFactory.ParseStatement($"await {Engine}.Wait(frames: 1);");
            if (sections.Count == 0)
                return Poll(prefix == null ? SyntaxFactory.Block(wait) : SyntaxFactory.Block(prefix, wait));
            if (fallThrough?.Count > 0)
                return Poll(LowerFallThroughDispatch(method, selector, token, sections, fallThrough, prefix, wait, useFinalExitLayout));

            var dispatch = Dispatch(method, selector, token, sections);
            BlockSyntax tick = prefix == null ? SyntaxFactory.Block(dispatch, wait) : SyntaxFactory.Block(prefix, dispatch, wait);
            return Poll(tick);
        }

        private BlockSyntax LowerFallThroughDispatch(IMethodSymbol method, ExpressionSyntax selector, EVExpressionToken token, List<SwitchSectionSyntax> sections,
            IReadOnlyDictionary<int, int> fallThrough, StatementSyntax? prefix, StatementSyntax wait, bool useFinalExitLayout)
        {
            const string waitLabel = "ChoreographyWait";
            var cases = sections.ToDictionary(item => (int)((LiteralExpressionSyntax)((CaseSwitchLabelSyntax)item.Labels.Single()).Value).Token.Value!);
            int[] order = cases.Keys.ToArray();
            bool finalExitJump = false;
            foreach (AttributeData layout in method.GetAttributes().Where(attribute => attribute.AttributeClass is { } type && IsSupportType(type, vm, "SwitchLayoutAttribute")))
            {
                string key = AttributeValue<string>(layout, 0);
                string[] groups = key.Split('|').Skip(2).ToArray();
                if (groups.Length != cases.Count + 1 || groups[^1] != "DB" || SwitchLayout.Key([token], groups, directDispatch: true) != key)
                    continue;
                int?[] values = groups.SkipLast(1).Select(group => group.EndsWith('B') && int.TryParse(group[..^1], out int phase) ? (int?)phase : null).ToArray();
                if (values.Any(value => value == null) || !cases.Keys.ToHashSet().SetEquals(values.Select(value => value!.Value)))
                    continue;
                order = values.Select(value => value!.Value).ToArray();
                finalExitJump = useFinalExitLayout && !AttributeValue<bool>(layout, 3);
                break;
            }

            var statements = new List<StatementSyntax>();
            if (prefix != null)
                statements.Add(prefix);
            var dispatch = order.Select(phase => cases[phase].WithStatements(SyntaxFactory.SingletonList(Jump(Label(phase))))).ToList();
            dispatch.Add(SyntaxFactory.SwitchSection(SyntaxFactory.SingletonList<SwitchLabelSyntax>(SyntaxFactory.DefaultSwitchLabel()), SyntaxFactory.SingletonList(Jump(waitLabel))));
            statements.Add(SyntaxFactory.SwitchStatement(selector).WithSections(SyntaxFactory.List(dispatch)));
            for (int index = 0; index < order.Length; index++)
            {
                int phase = order[index];
                statements.Add(SyntaxFactory.LabeledStatement(Label(phase), SyntaxFactory.EmptyStatement()));
                statements.AddRange(cases[phase].Statements.SkipLast(1));
                if (fallThrough.TryGetValue(phase, out int destination))
                {
                    if (index + 1 == order.Length || order[index + 1] != destination)
                        statements.Add(Jump(cases.ContainsKey(destination) ? Label(destination) : waitLabel));
                }
                else if (index + 1 != order.Length || finalExitJump)
                {
                    statements.Add(Jump(waitLabel));
                }
            }
            statements.Add(SyntaxFactory.LabeledStatement(waitLabel, SyntaxFactory.EmptyStatement()));
            statements.Add(wait);
            return SyntaxFactory.Block(statements);

            static string Label(int phase) => phase >= 0 ? $"ChoreographyPhase{phase}" : $"ChoreographyPhaseMinus{-(long)phase}";
            static StatementSyntax Jump(string label) => SyntaxFactory.GotoStatement(SyntaxKind.GotoStatement, SyntaxFactory.IdentifierName(label));
        }

        private SwitchStatementSyntax Dispatch(IMethodSymbol method, ExpressionSyntax selector, EVExpressionToken token, List<SwitchSectionSyntax> sections)
        {
            // The original switch can place its cases in a different physical order than the play.
            // A layout only applies when it describes the same selector and cases.
            var fallback = sections.SingleOrDefault(section => section.Labels.Any(label => label is DefaultSwitchLabelSyntax));
            var byKey = sections.Where(section => section != fallback).ToDictionary(section => SectionKey(section.Labels.Cast<CaseSwitchLabelSyntax>().Select(label =>
                (int)((LiteralExpressionSyntax)label.Value).Token.Value!)));
            // Redundant case labels can name the default target. Preserve them only while no role claims those phase values.
            var phaseKeys = sections.SelectMany(section => section.Labels).OfType<CaseSwitchLabelSyntax>().Select(label =>
                (int)((LiteralExpressionSyntax)label.Value).Token.Value!).ToHashSet();
            foreach (AttributeData layout in method.GetAttributes().Where(attribute => attribute.AttributeClass is { } type && IsSupportType(type, vm, "SwitchLayoutAttribute")))
            {
                string key = AttributeValue<string>(layout, 0);
                string[] keys = key.Split('|').Skip(1).ToArray();
                if (keys.Length != sections.Count || keys.Any(section => !section.EndsWith('B')) || SwitchLayout.Key([token], keys) != key ||
                    keys.Count(section => section.EndsWith("DB", StringComparison.Ordinal)) != (fallback == null ? 0 : 1))
                    continue;
                var order = keys.Select(section => section[..^1].TrimEnd('D').Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(value => int.TryParse(value, out int parsed) ? (int?)parsed : null).ToArray()).ToArray();
                if (order.Any(values => values.Any(value => value == null)))
                    continue;
                int[][] valuesBySection = order.Select(values => values.Select(value => value!.Value).ToArray()).ToArray();
                string[] normalizedKeys = valuesBySection.Select((values, index) => IsFallback(keys[index]) ? "DB" : SectionKey(values)).ToArray();
                var distinctValues = valuesBySection.SelectMany(values => values).ToHashSet();
                if (distinctValues.Count != valuesBySection.Sum(values => values.Length) ||
                    keys.Where((section, index) => SwitchLayout.Section(valuesBySection[index], IsFallback(section), false) != section).Any())
                    continue;
                if (normalizedKeys.Distinct(StringComparer.Ordinal).Count() != sections.Count ||
                    normalizedKeys.Any(section => section != "DB" && !byKey.ContainsKey(section)) ||
                    normalizedKeys.Select((section, index) => (section, index)).Any(item => item.section == "DB" && valuesBySection[item.index].Any(phaseKeys.Contains)))
                    continue;
                sections = valuesBySection.Zip(normalizedKeys).Select(item =>
                {
                    var labels = item.First.Select(value => (SwitchLabelSyntax)SyntaxFactory.CaseSwitchLabel(Number(value)));
                    return item.Second == "DB"
                        ? fallback!.WithLabels(SyntaxFactory.List(labels.Append(SyntaxFactory.DefaultSwitchLabel())))
                        : byKey[item.Second].WithLabels(SyntaxFactory.List(labels));
                }).ToList();
                break;
            }

            return SyntaxFactory.SwitchStatement(selector).WithSections(SyntaxFactory.List(sections));

            static string SectionKey(IEnumerable<int> values) => SwitchLayout.Section(values.Order(), false, false);
            static bool IsFallback(string section) => section.EndsWith("DB", StringComparison.Ordinal);
        }

        private static BlockSyntax Poll(BlockSyntax tick) =>
            SyntaxFactory.Block(SyntaxFactory.WhileStatement(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression), tick));

        private static string Name(ISymbol symbol) => "@" + symbol.Name;

        private static LiteralExpressionSyntax Number(int value) =>
            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value));

        private static StatementSyntax Assign(ExpressionSyntax target, ExpressionSyntax value) =>
            SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, target, value));

        private static SwitchSectionSyntax Section(int phase, params StatementSyntax[] statements) =>
            SyntaxFactory.SwitchSection(SyntaxFactory.SingletonList<SwitchLabelSyntax>(SyntaxFactory.CaseSwitchLabel(Number(phase))),
                SyntaxFactory.List([.. statements, SyntaxFactory.BreakStatement()]));

        private static ExpressionStatementSyntax Call(IMethodSymbol method, string receiver, string? argument) =>
            (ExpressionStatementSyntax)SyntaxFactory.ParseStatement($"{(method.ReturnsVoid ? "" : "await ")}{receiver}.{Name(method)}({argument});");

        private sealed record Play(IMethodSymbol Director, MethodDeclarationSyntax Source, Storage Selector, Storage Cue, Storage? Pulse)
        {
            public Dictionary<int, Phase> Phases { get; } = new();
            public Queue<Choice> Choices { get; } = new();
            public Queue<(Phase Source, IMethodSymbol Target, SyntaxNode Reference)> Repeats { get; } = new();
            public Dictionary<int, int> FallThrough { get; } = new();
            public Dictionary<ITypeSymbol, List<Chapter>> Tracks { get; } = new(SymbolEqualityComparer.Default);
            public Dictionary<ITypeSymbol, IMethodSymbol> Handlers { get; } = new(SymbolEqualityComparer.Default);
        }

        private sealed class Phase(int value)
        {
            public int Value { get; } = value;
            public Phase? Next { get; set; }
            public Choice? Choice { get; set; }
            public IMethodSymbol? Finish { get; set; }
        }

        private sealed record Choice(Storage Selector, List<ChoiceArm> Arms);

        private sealed record ChoiceArm(int Key, IMethodSymbol Target, SyntaxNode Reference);

        private sealed class MethodRewriter(Dictionary<MethodDeclarationSyntax, BlockSyntax?> methods) : CSharpSyntaxRewriter
        {
            public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax method)
            {
                if (!methods.TryGetValue(method, out BlockSyntax? body))
                    return base.VisitMethodDeclaration(method);
                return body == null ? null : method.WithBody(body.WithTriviaFrom(method.Body!));
            }
        }

        private sealed record Chapter(int Phase, IMethodSymbol Method, Storage? Counter, int BeginsWith, bool Signal, int? Continuation);

        private sealed record Storage(ExpressionSyntax Source, IPropertySymbol Property, IPropertySymbol Holder, EVExpressionToken Token, EVVariable Variable)
        {
            private int Start => Variable.UsesBitAddress ? Variable.Address : Variable.Address * 8;

            public bool Overlaps(Storage other) => Start < other.Start + other.Variable.Width && other.Start < Start + Variable.Width;

            public bool CanStore(int value) => ChoreographyChoiceTable.CanStore((StorageType)Variable.Type, value);
        }
    }
}
