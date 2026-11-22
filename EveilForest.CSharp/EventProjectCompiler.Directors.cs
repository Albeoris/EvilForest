using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace EveilForest.CSharp;

public static partial class EventProjectCompiler
{
    private static bool IsStartDirector(IMethodSymbol method, INamedTypeSymbol vm) =>
        method.Name == "StartDirector" && IsSupportType(method.ContainingType, vm, "ScriptService");

    private static bool IsDirectorMethod(IMethodSymbol method, INamedTypeSymbol vm) =>
        DerivesFrom(method.ContainingType, vm, "EventCutscene") &&
        !method.IsStatic && !method.IsVirtual && !method.IsOverride && !method.IsGenericMethod &&
        method.MethodKind == MethodKind.Ordinary && method.Parameters.Length == 0 &&
        IsSupportType(method.ReturnType, vm, "EventRoutine") && !HasSupportAttribute(method, vm, "ScriptAttribute") &&
        method.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is MethodDeclarationSyntax { Body: not null };

    private static string DirectorName(IMethodSymbol method) => $"{MetadataName(method.ContainingType)}.{method.MetadataName}";

    private static string MetadataName(INamedTypeSymbol type)
    {
        if (type.ContainingType != null)
            return $"{MetadataName(type.ContainingType)}+{type.MetadataName}";
        return type.ContainingNamespace.IsGlobalNamespace ? type.MetadataName : $"{type.ContainingNamespace.ToDisplayString()}.{type.MetadataName}";
    }

    private sealed class Directors
    {
        private readonly Dictionary<IMethodSymbol, int> _slots = new(SymbolEqualityComparer.Default);
        private readonly Dictionary<int, MethodDeclarationSyntax> _entries = new();

        public IEnumerable<int> Slots => _entries.Keys;

        public Directors(CSharpCompilation compilation, INamedTypeSymbol vm, IEnumerable<int> occupied, IReadOnlyDictionary<INamedTypeSymbol, IPropertySymbol> bindings)
        {
            var entries = new Dictionary<string, (IMethodSymbol Method, MethodDeclarationSyntax Source, int? Slot)>(StringComparer.Ordinal);
            foreach (SyntaxTree tree in compilation.SyntaxTrees)
            {
                SemanticModel model = compilation.GetSemanticModel(tree);
                foreach (InvocationExpressionSyntax call in tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    if (call.Expression is not MemberAccessExpressionSyntax { Name.Identifier.ValueText: "StartDirector" } &&
                        call.Expression is not IdentifierNameSyntax { Identifier.ValueText: "StartDirector" })
                        continue;
                    if (model.GetOperation(call) is not IInvocationOperation invocation || !IsStartDirector(invocation.TargetMethod, vm))
                        continue;
                    if (model.GetEnclosingSymbol(call.SpanStart) is not IMethodSymbol caller)
                        throw Invalid(call, "Start a director from an event method");
                    IMethodSymbol method = ReadDirector(invocation, caller, vm);
                    if (!bindings.ContainsKey(method.ContainingType))
                        throw Invalid(call, "A scene director needs a registered cutscene instance");
                    var source = (MethodDeclarationSyntax)method.DeclaringSyntaxReferences.Single().GetSyntax();
                    AttributeData? original = method.GetAttributes().FirstOrDefault(attribute =>
                        attribute.AttributeClass is { } type && IsSupportType(type, vm, "OriginalDirectorSlotAttribute"));
                    int? slot = original == null ? null : AttributeValue<int>(original, 0);
                    entries.TryAdd(DirectorName(method), (method, source, slot));
                }
            }

            Dictionary<string, int> slots;
            try
            {
                slots = SceneDirectorSlots.Assign(occupied, entries.Select(entry => (entry.Key, entry.Value.Slot)));
            }
            catch (InvalidOperationException exception)
            {
                throw new InvalidDataException(exception.Message, exception);
            }
            foreach (var (name, slot) in slots)
            {
                var (method, source, _) = entries[name];
                _slots.Add(method, slot);
                _entries.Add(slot, source);
            }
        }

        public EVInstruction Start(IInvocationOperation invocation, IMethodSymbol caller, INamedTypeSymbol vm)
        {
            IMethodSymbol method = ReadDirector(invocation, caller, vm);
            if (!_slots.TryGetValue(method, out int slot))
                throw Invalid(invocation.Syntax, "The scene director has no object slot");
            return MakeInstruction(0, Jsm.Opcode.NEW, 0, new EVOperand(slot), new EVOperand(0));
        }

        private static IMethodSymbol ReadDirector(IInvocationOperation invocation, IMethodSymbol caller, INamedTypeSymbol vm)
        {
            if (invocation.Instance == null || !IsBoundScriptService(invocation.Instance, caller, vm) ||
                invocation.Arguments is not [{ Value: IDelegateCreationOperation { IsImplicit: true, Target: IMethodReferenceOperation reference } }] ||
                reference.Instance is not IPropertyReferenceOperation ||
                !IsDirectorMethod(reference.Method, vm) ||
                !IsBoundCutsceneReceiver(reference.Instance, reference.Method.ContainingType, caller, vm, invocation.SemanticModel!.Compilation))
                throw Invalid(invocation.Syntax, "Start a director on the registered cutscene instance in the current event context using a method group");
            return reference.Method;
        }

        public void Compile(CSharpCompilation compilation, INamedTypeSymbol vm, EVContainerObject[] objects, IReadOnlySet<int> declaredSlots, List<(int Caller, ushort Handler, int Tag, SyntaxNode Source)> requests, ParallelGroups parallelGroups)
        {
            var occupiedOrders = objects.Where(obj => obj?.BodyOrder != null).Select(obj => obj.BodyOrder!.Value).ToHashSet();
            int nextOrder = 0;
            foreach (var (slot, source) in _entries.OrderBy(entry => entry.Key))
            {
                while (occupiedOrders.Contains(nextOrder))
                    nextOrder++;

                SemanticModel model = compilation.GetSemanticModel(source.SyntaxTree);
                IMethodSymbol method = model.GetDeclaredSymbol(source)!;
                var init = new EVInstructionBody([new EVInstruction(0, Jsm.Opcode.Return)], 1);
                EVContainerScript loop = CompileEntry(model, vm, method, source, slot, 1, objects.Length, declaredSlots, requests, parallelGroups, this, ensureReturn: true);
                objects[slot] = new EVContainerObject(slot, 0, 0, 0)
                {
                    BodyOrder = nextOrder++,
                    Scripts = [new EVContainerScript(0, init.Encode()), loop]
                };
            }
        }
    }
}
