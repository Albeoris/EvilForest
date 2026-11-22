using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace EveilForest.CSharp;

public static partial class EventProjectCompiler
{
    private static bool IsDirectCurrentParticipant(IOperation value, IMethodSymbol caller)
    {
        while (value is IConversionOperation { IsImplicit: true, OperatorMethod: null } conversion)
            value = conversion.Operand;
        return value is IInstanceReferenceOperation { ReferenceKind: InstanceReferenceKind.ContainingTypeInstance } ||
            value is IParameterReferenceOperation parameter &&
            SymbolEqualityComparer.Default.Equals(parameter.Parameter.ContainingSymbol, caller);
    }

    private sealed class ParallelGroups(INamedTypeSymbol vm)
    {
        private readonly Dictionary<IMethodSymbol, Group> _groups = new(SymbolEqualityComparer.Default);

        public (IMethodSymbol Track, IMethodSymbol Complete) Select(IMethodSymbol group, BlockSyntax body, ITypeSymbol owner, SyntaxNode use, Compilation compilation)
        {
            if (!_groups.TryGetValue(group, out Group? binding))
            {
                binding = Read(group, body, compilation);
                _groups.Add(group, binding);
            }
            if (!binding.Tracks.TryGetValue(owner, out IMethodSymbol? track))
                throw Invalid(use, "This participant has no track in the parallel scene phase");
            if (!binding.Callers.Add(owner))
                throw Invalid(use, "A participant can enter this parallel scene phase from one handler arm");
            return (track, binding.Complete);
        }

        public void Validate()
        {
            foreach ((IMethodSymbol method, Group binding) in _groups)
            {
                if (binding.Tracks.Count != binding.Callers.Count)
                    throw Invalid(method.DeclaringSyntaxReferences.Single().GetSyntax(), "Every parallel scene track needs a participant handler arm");
            }
        }

        private Group Read(IMethodSymbol group, BlockSyntax body, Compilation compilation)
        {
            var model = compilation.GetSemanticModel(body.SyntaxTree);
            if (body.Statements is not [ExpressionStatementSyntax { Expression: AwaitExpressionSyntax awaiter }] ||
                model.GetOperation(awaiter.Expression) is not IInvocationOperation { TargetMethod: { Name: "Together" } contract } call ||
                !IsSupportType(contract.ContainingType, vm, "EventCutscene") ||
                call.Arguments is not [var participant, var complete, var tracks] ||
                !IsDirectCurrentParticipant(participant.Value, group) ||
                tracks.Value is not IArrayCreationOperation { IsImplicit: true, Initializer.ElementValues: { Length: >= 2 } elements })
                throw Invalid(body, "A parallel scene method must await Together with the current participant, a completion method, and at least two tracks");

            IMethodSymbol completion = ReadMethod(complete.Value, group, receiverType: null);
            var actions = new Dictionary<ITypeSymbol, IMethodSymbol>(SymbolEqualityComparer.Default);
            foreach (IOperation element in elements)
            {
                if (element is not IInvocationOperation { TargetMethod: { Name: "Track" } factory, Arguments: [var actor, var action] } ||
                    !IsSupportType(factory.ContainingType, vm, "EventCutscene") ||
                    actor.Value is not IPropertyReferenceOperation { Instance: IPropertyReferenceOperation { Instance: IParameterReferenceOperation } } ||
                    !IsBoundActionParticipant(actor.Value, group, vm) ||
                    IsControlledActor(actor.Value, vm) ||
                    ReadSceneParticipant(actor.Value, vm, model.Compilation) is not { Kind: 2 } binding)
                    throw Invalid(element.Syntax, "Bind each parallel track to a fixed actor in the current scene");

                IMethodSymbol method = ReadMethod(action.Value, group, binding.Declaration);
                if (!actions.TryAdd(binding.Declaration, method))
                    throw Invalid(element.Syntax, "A parallel scene phase can declare one track per actor definition");
            }

            return new Group(completion, actions);
        }

        private IMethodSymbol ReadMethod(IOperation value, IMethodSymbol group, ITypeSymbol? receiverType)
        {
            if (value is not IDelegateCreationOperation { Target: IMethodReferenceOperation { Instance: IInstanceReferenceOperation } reference } ||
                !SymbolEqualityComparer.Default.Equals(reference.Method.ContainingType, group.ContainingType) ||
                reference.Method.IsGenericMethod || HasSupportAttribute(reference.Method, vm, "ScriptAttribute") ||
                !IsSupportType(reference.Method.ReturnType, vm, "EventRoutine") ||
                reference.Method.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not MethodDeclarationSyntax { Body: not null } ||
                !HasReceiver(reference.Method))
                throw Invalid(value.Syntax, "Use an ordinary routine on the registered cutscene instance");
            return reference.Method;

            bool HasReceiver(IMethodSymbol method) => receiverType == null ? method.Parameters.Length == 0 :
                method.Parameters is [var parameter] && parameter.RefKind == RefKind.None &&
                SymbolEqualityComparer.Default.Equals(parameter.Type, receiverType);
        }

        private sealed record Group(IMethodSymbol Complete, Dictionary<ITypeSymbol, IMethodSymbol> Tracks)
        {
            public HashSet<ITypeSymbol> Callers { get; } = new(SymbolEqualityComparer.Default);
        }
    }

    private sealed partial class BodyCompiler
    {
        private void InlineTogether(IMethodSymbol entry, BlockSyntax body, SyntaxNode use)
        {
            var (track, complete) = _parallelGroups.Select(entry, body, ExecutionOwner(handler, vm), use, model.Compilation);
            Inline(track, Body(track), use, allowEarlyReturn: false);
            Inline(complete, Body(complete), use, allowEarlyReturn: false);

            static BlockSyntax Body(IMethodSymbol method) => ((MethodDeclarationSyntax)method.DeclaringSyntaxReferences.Single().GetSyntax()).Body!;
        }
    }
}

