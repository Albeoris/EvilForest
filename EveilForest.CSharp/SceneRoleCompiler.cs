using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace EveilForest.CSharp;

public static partial class EventProjectCompiler
{
    private static bool IsRoleStorageReceiver(IPropertyReferenceOperation holder, IMethodSymbol caller, INamedTypeSymbol vm)
    {
        if (holder.Instance is not IInstanceReferenceOperation ||
            !SymbolEqualityComparer.Default.Equals(holder.Property.ContainingType, caller.ContainingType) ||
            !DerivesFrom(caller.ContainingType, vm, "EventCutscene") || !IsGetOnlyAutoProperty(holder.Property) ||
            holder.Property.Type is not INamedTypeSymbol holderType ||
            holderType.DeclaringSyntaxReferences is not [var reference] || reference.GetSyntax() is not ClassDeclarationSyntax roles ||
            roles.BaseList != null || roles.ParameterList?.Parameters.Count != 1 ||
            roles.Members.Any(member => member is not PropertyDeclarationSyntax { Initializer: null }) ||
            !holderType.GetMembers().OfType<IPropertySymbol>().Any(property => HasSupportAttribute(property, vm, "RoleStorageAliasAttribute")) ||
            holder.Property.DeclaringSyntaxReferences is not [var propertyReference] ||
            propertyReference.GetSyntax() is not PropertyDeclarationSyntax { Initializer.Value: { } initializer })
            return false;
        IOperation? binding = holder.SemanticModel!.Compilation.GetSemanticModel(initializer.SyntaxTree).GetOperation(initializer);
        return binding is IObjectCreationOperation { Arguments: [var source], Initializer: null } &&
            source.Value is IPropertyReferenceOperation globals && IsSupportType(globals.Property.ContainingType, vm, "EventSceneContext") &&
            IsSupportType(globals.Property.Type, vm, "GlobalState") && IsContextReference(globals.Instance, caller, vm);
    }

    private static EVExpressionToken? ReadStorageReference(IPropertyReferenceOperation member, INamedTypeSymbol vm)
    {
        var compilation = member.SemanticModel?.Compilation;
        if (!HasSupportAttribute(member.Property, vm, "RoleStorageAliasAttribute"))
        {
            EVExpressionToken? token = ReadStorage(member.Property, vm, compilation);
            if (token != null && member.Instance != null && !IsStorageReceiver(member.Instance, vm, new HashSet<ISymbol>(SymbolEqualityComparer.Default)))
                throw Invalid(member.Syntax, "The storage receiver must be a side-effect-free reference to event storage");
            return token;
        }
        if (compilation == null || member.Instance == null || SceneReferenceValue(member.Instance) is not IPropertyReferenceOperation holder ||
            member.SemanticModel!.GetEnclosingSymbol(member.Syntax.SpanStart) is not IMethodSymbol caller || !IsRoleStorageReceiver(holder, caller, vm) ||
            member.Property.DeclaringSyntaxReferences is not [var reference] ||
            reference.GetSyntax() is not PropertyDeclarationSyntax { AccessorList: { } accessors })
            throw Invalid(member.Syntax, "A role storage alias must use the current cutscene's global storage view");

        var model = compilation.GetSemanticModel(accessors.SyntaxTree);
        var getter = accessors.Accessors.FirstOrDefault(accessor => accessor.Keyword.ValueText == "get");
        var setter = accessors.Accessors.FirstOrDefault(accessor => accessor.Keyword.ValueText == "set");
        if (getter?.ExpressionBody is not { } read || model.GetOperation(read.Expression) is not IPropertyReferenceOperation
            { Instance: IParameterReferenceOperation source } storage ||
            !IsSupportType(storage.Property.ContainingType, vm, "GlobalState") ||
            !SymbolEqualityComparer.Default.Equals(source.Parameter.ContainingSymbol.ContainingType, member.Property.ContainingType) ||
            setter?.ExpressionBody is not { } write || model.GetOperation(write.Expression) is not ISimpleAssignmentOperation
            {
                Target: IPropertyReferenceOperation { Instance: IParameterReferenceOperation destination } target,
                Value: IParameterReferenceOperation value
            } || !SymbolEqualityComparer.Default.Equals(value.Parameter.ContainingSymbol, member.Property.SetMethod) ||
            !SymbolEqualityComparer.Default.Equals(storage.Property, target.Property) ||
            !SymbolEqualityComparer.Default.Equals(source.Parameter, destination.Parameter) ||
            ReadStorage(storage.Property, vm, compilation) is not EVExpressionToken view)
            throw Invalid(member.Syntax, "A role storage alias must read and write the same declared global property");
        return view;
    }

    private sealed partial class BodyCompiler
    {
        private bool ValidateRoleCast(IObjectCreationOperation cast, IMethodSymbol caller)
        {
            ReadRoleCast(cast, caller);
            return true;
        }

        private (IPropertySymbol Definition, int[] Instances) ReadRoleCast(IObjectCreationOperation cast, IMethodSymbol caller)
        {
            if (cast.Arguments is not [var prototype, { Value: IArrayCreationOperation { Initializer: { } list } }] ||
                list.ElementValues.Length is < 1 or > byte.MaxValue ||
                !IsBoundActionParticipant(prototype.Value, caller, vm) ||
                SceneReferenceValue(prototype.Value) is not IPropertyReferenceOperation actor ||
                ReadSceneParticipant(actor, vm, model.Compilation) is not { Kind: 2 })
                throw Invalid(cast.Syntax, "A role cast needs a declared scene actor and a list of fixed live UIDs");

            var instances = new int[list.ElementValues.Length];
            for (int index = 0; index < instances.Length; index++)
            {
                IOperation value = SceneReferenceValue(list.ElementValues[index]);
                if (value is not IObjectCreationOperation { Arguments: [var uid] } identifier ||
                    !IsSupportType(identifier.Type!, vm, "InstanceUid") || ConstantInt(uid.Value) is not (> 0 and < 250))
                    throw Invalid(value.Syntax, "Use a fixed, ordinary actor UID in the role cast");
                instances[index] = ConstantInt(uid.Value);
            }
            return (actor.Property, instances);
        }

        private bool TryCompileRoleRequest(IInvocationOperation invocation, StatementSyntax statement)
        {
            if (invocation.TargetMethod.Name != "StartWhenReady" ||
                !IsSupportType(invocation.TargetMethod.ContainingType, vm, "ScriptService") ||
                invocation.TargetMethod.Parameters is not [var selection, _, _] ||
                selection.Type is not INamedTypeSymbol { ContainingType: { } castType } ||
                !IsSupportType(castType.OriginalDefinition, vm, "SceneRoleCast`1"))
                return false;

            var call = new Intrinsic(invocation, vm, service: true);
            if (!IsBoundScriptService(invocation.Instance, handler, vm) ||
                invocation.Arguments.Where(argument => !argument.IsImplicit).Select(argument => argument.Parameter!.Ordinal).SequenceEqual([0, 1, 2]) == false ||
                call.Argument(0) is not IInvocationOperation { Arguments: [var choice], Instance: { } castReceiver } resolve ||
                !SymbolEqualityComparer.Default.Equals(resolve.TargetMethod.ContainingType, castType) || resolve.TargetMethod.Name != "Resolve" ||
                SceneReferenceValue(castReceiver) is not IObjectCreationOperation cast)
                throw Invalid(invocation.Syntax, "Use the current scene's script service and a direct cast.Resolve(choice) before the handler and level");
            var (definition, instances) = ReadRoleCast(cast, handler);
            if (call.Argument(1) is not IDelegateCreationOperation { Target: IMethodReferenceOperation method } || method.Instance == null ||
                !IsBoundActionParticipant(method.Instance, handler, vm) ||
                SceneReferenceValue(method.Instance) is not IPropertyReferenceOperation bound ||
                !SymbolEqualityComparer.Default.Equals(bound.Property, definition) ||
                !SymbolEqualityComparer.Default.Equals(method.Method.ContainingType, definition.Type) ||
                FindAttribute(method.Method, RequireType(model.Compilation, "ScriptAttribute")) is not { } entry)
                throw Invalid(call.Argument(1).Syntax, "Pass a script method group on the same scene actor used to create the cast");

            int tag = checked((byte)AttributeValue<ushort>(entry, 0));
            int level = checked((byte)call.Int(2));
            var selector = new EVOperand(ReadNative(choice.Value, vm, choice.Syntax));
            int exit = NewLabel();
            int[] targets = instances.Select(_ => NewLabel()).ToArray();
            Evaluate(selector, choice.Syntax);
            Add(new EVInstruction(0, Jsm.Opcode.JMP_SWITCH)
            {
                CaseValues = [0],
                Targets = new[] { exit }.Concat(targets).ToArray()
            }, statement);
            for (int index = 0; index < instances.Length; index++)
            {
                Mark(targets[index]);
                Add(MakeInstruction(0, Jsm.Opcode.REQSW, 0, [new EVOperand(level), new EVOperand(instances[index]), new EVOperand(tag)]), statement);
                Jump(Jsm.Opcode.JMP, exit, statement);
            }
            Mark(exit);
            return true;
        }
    }
}
