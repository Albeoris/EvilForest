using System.Collections.Immutable;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
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
    private const int MapStorageCapacity = 80;
    private const string SourceNamespace = "EvilForest.EventSource";
    private static readonly MetadataReference CoreLibrary = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
    private static readonly MetadataReference RuntimeFacade = MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll"));
    private static readonly MetadataReference ExpressionLibrary = MetadataReference.CreateFromFile(typeof(System.Linq.Expressions.Expression).Assembly.Location);
    private static readonly ConditionalWeakTable<Compilation, ConcurrentDictionary<ISymbol, EVExpressionToken?>> LayoutViews = new();
    private static readonly ConditionalWeakTable<Compilation, Dictionary<INamedTypeSymbol, IPropertySymbol>> CutsceneBindings = new();

    public static EVContainer Compile(string directory) => Prepare(directory).Container;

    public static byte[] CompileManaged(string directory)
    {
        var (_, compilation) = Prepare(directory);
        using var output = new MemoryStream();
        var result = compilation.Emit(output);
        if (!result.Success)
            throw new InvalidDataException(result.Diagnostics.First(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToString());
        return output.ToArray();
    }

    private static (EVContainer Container, CSharpCompilation Compilation) Prepare(string directory)
    {
        SyntaxTree[] trees = EnumerateSources(directory).Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path)).ToArray();
        var references = new List<MetadataReference> { CoreLibrary, RuntimeFacade, ExpressionLibrary };
        string? dependency = EventSourceDependency.FindForProject(directory);
        if (dependency != null)
            references.Add(MetadataReference.CreateFromFile(dependency));
        var compilation = CSharpCompilation.Create("EventSource", trees, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        Diagnostic? error = compilation.GetDiagnostics().FirstOrDefault(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
        if (error != null)
            throw new InvalidDataException(error.ToString());

        compilation = BindManagedAccessors(AllocateStorage(compilation));
        CSharpCompilation managedCompilation = LowerChoreography(compilation);
        compilation = LowerSharedCaseActions(managedCompilation);
        compilation = LowerCaseContinuations(compilation);
        trees = compilation.SyntaxTrees.ToArray();

        INamedTypeSymbol objectType = RequireType(compilation, "ObjectSlotAttribute");
        INamedTypeSymbol scriptType = RequireType(compilation, "ScriptAttribute");
        INamedTypeSymbol vm = RequireType(compilation, "Vm");
        var (header, trailer) = ReadContainerMetadata(compilation, directory);
        var requests = new List<(int Caller, ushort Handler, int Tag, SyntaxNode Source)>();
        var compiledTypes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        var declarations = new SortedDictionary<int, (INamedTypeSymbol Owner, ClassDeclarationSyntax Source, AttributeData Slot)>();
        var cutsceneEntries = new Dictionary<int, List<MethodDeclarationSyntax>>();
        var cutsceneBindings = GetCutsceneBindings(compilation, vm);
        int? controlledSlot = null;
        foreach (SyntaxTree tree in trees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            SyntaxNode root = tree.GetRoot();
            foreach (PropertyDeclarationSyntax property in root.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
                if (model.GetDeclaredSymbol(property) is IPropertySymbol controlled &&
                    IsControlledActorBinding(controlled, vm, compilation) &&
                    FindAttribute(controlled.Type, objectType) is { } owner)
                    controlledSlot = AttributeValue<int>(owner, 0);
                if (model.GetDeclaredSymbol(property) is IPropertySymbol symbol && ReadStorage(symbol, vm, compilation) is EVExpressionToken token)
                {
                    EVVariable view = EVVariable.Decode(token);
                    ValidateStorageCapacity(property, symbol, view, objectType);
                    if (HasSupportAttribute(symbol, vm, "StorageViewAttribute"))
                        ValidateStorageAccessors(property, model, view, vm);
                }
            }
            foreach (ClassDeclarationSyntax cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                INamedTypeSymbol symbol = model.GetDeclaredSymbol(cls)!;
                if (DerivesFrom(symbol, vm, "EventCutscene"))
                {
                    if (!cutsceneBindings.ContainsKey(symbol))
                        throw Invalid(cls, "Declare one get-only cutscene instance on the event context");
                    foreach (MethodDeclarationSyntax method in cls.Members.OfType<MethodDeclarationSyntax>())
                    {
                        IMethodSymbol action = model.GetDeclaredSymbol(method)!;
                        if (FindAttribute(action, scriptType) == null)
                            continue;
                        if (action.IsStatic || action.Parameters is not [var receiver] || receiver.RefKind != RefKind.None ||
                            FindAttribute(receiver.Type, objectType) is not { } binding ||
                            !SymbolEqualityComparer.Default.Equals(receiver.Type.ContainingAssembly, compilation.Assembly))
                            throw Invalid(method, "A cutscene entry requires one declared participant parameter");
                        int slot = AttributeValue<int>(binding, 0);
                        if (!cutsceneEntries.TryGetValue(slot, out var entries))
                            cutsceneEntries.Add(slot, entries = new());
                        entries.Add(method);
                    }
                }
                AttributeData? declaration = FindAttribute(symbol, objectType);
                if (declaration == null || !compiledTypes.Add(symbol))
                    continue;
                int id = AttributeValue<int>(declaration, 0);
                if ((uint)id >= byte.MaxValue || !declarations.TryAdd(id, (symbol, cls, declaration)))
                    throw Invalid(cls, $"Invalid or duplicate object slot {id}");
            }
        }

        var parallelGroups = new ParallelGroups(vm);
        var directors = new Directors(compilation, vm, declarations.Keys, cutsceneBindings);
        var declaredSlots = declarations.Keys.Concat(directors.Slots).ToHashSet();
        int objectCount = declaredSlots.DefaultIfEmpty(-1).Max() + 1;
        bool hasScene = ReadEventContext(compilation, vm) != null;
        if (hasScene && objectCount != 0 && !declaredSlots.Contains(0))
            throw new InvalidDataException("An event scene requires an object declaration for slot 0.");
        var objects = new EVContainerObject[objectCount];
        header[3] = checked((byte)objects.Length);
        foreach (var (id, declaration) in declarations)
        {
            objects[id] = CompileObject(compilation, declaration.Owner, declaration.Source, declaration.Slot, scriptType, vm, id, objects.Length, declaredSlots, requests,
                cutsceneEntries.GetValueOrDefault(id) ?? [], parallelGroups, directors);
        }
        directors.Compile(compilation, vm, objects, declaredSlots, requests, parallelGroups);
        int emptySlots = 0;
        for (int id = 0; id < objects.Length; id++)
        {
            if (objects[id] != null)
                continue;
            if (!hasScene)
                throw new InvalidDataException($"Missing object declaration for slot {id}.");
            objects[id] = new EVContainerObject(id, 0, 0, 0);
            emptySlots++;
        }
        if (emptySlots != 0)
            CompactBodyOrder(objects, emptySlots);
        parallelGroups.Validate();
        INamedTypeSymbol? contractType = compilation.GetTypeByMetadataName($"{SourceNamespace}.ControlledActorContractAttribute");
        if (contractType != null && FindAttribute(compilation.Assembly, contractType) is AttributeData contract)
            controlledSlot ??= AttributeValue<int>(contract, 0);
        if (controlledSlot is int target)
        {
            if ((uint)target >= objects.Length)
                throw new InvalidDataException($"Invalid controlled actor contract slot {target}");
            foreach (var request in requests)
            {
                if (!objects[target].Scripts.Any(script => script.Id == request.Tag))
                    throw Invalid(request.Source, $"Object {request.Caller}, script {request.Handler}: controlled actor slot {target} has no handler tag {request.Tag}");
            }
        }
        return (new EVContainer(header, objects, trailer), managedCompilation);
    }

    private static void CompactBodyOrder(EVContainerObject[] objects, int emptySlots)
    {
        EVContainerObject[] bodies = objects.Where(obj => obj.BodyOrder.HasValue).OrderBy(obj => obj.BodyOrder).ToArray();
        int previous = -1;
        for (int order = 0; order < bodies.Length; order++)
        {
            int original = bodies[order].BodyOrder!.Value;
            if (original <= previous || original - order > emptySlots)
                throw new InvalidDataException($"Invalid body order {original} for object {bodies[order].Id}.");
            previous = original;
            bodies[order].BodyOrder = order;
        }
    }

    private static Dictionary<INamedTypeSymbol, IPropertySymbol> GetCutsceneBindings(Compilation compilation, INamedTypeSymbol vm) =>
        CutsceneBindings.GetValue(compilation, key => ReadCutsceneBindings(key, vm));

    private static Dictionary<INamedTypeSymbol, IPropertySymbol> ReadCutsceneBindings(Compilation compilation, INamedTypeSymbol vm)
    {
        var bindings = new Dictionary<INamedTypeSymbol, IPropertySymbol>(SymbolEqualityComparer.Default);
        INamedTypeSymbol? context = ReadEventContext(compilation, vm);
        if (context == null)
            return bindings;
        if (!DerivesFrom(context, vm, "EventSceneContext") || context.IsAbstract ||
            context.InstanceConstructors.SingleOrDefault(constructor => constructor.Parameters.Length == 0) is not { } constructor)
            throw new InvalidDataException("The event scene must supply a constructible EventSceneContext.");
        var constructorBody = (constructor.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() as ConstructorDeclarationSyntax)?.Body;
        foreach (IPropertySymbol property in context.GetMembers().OfType<IPropertySymbol>())
        {
            if (!DerivesFrom(property.Type, vm, "EventCutscene") && !HasSupportAttribute(property, vm, "SceneCutsceneAttribute"))
                continue;
            SyntaxNode source = property.DeclaringSyntaxReferences.Single().GetSyntax();
            if (!IsGetOnlyAutoProperty(property) || source is not PropertyDeclarationSyntax { Initializer: null } || property.Type is not INamedTypeSymbol type || type.IsAbstract ||
                !DerivesFrom(type, vm, "EventCutscene") || constructorBody == null ||
                !IsConstructedFromContext(compilation, constructorBody, property, type) || !bindings.TryAdd(type, property))
                throw Invalid(source, "Expected a unique get-only cutscene property initialized once with new CutsceneType(this)");
        }
        return bindings;
    }

    private static bool IsConstructedFromContext(Compilation compilation, BlockSyntax constructor, IPropertySymbol property, INamedTypeSymbol type)
    {
        SemanticModel model = compilation.GetSemanticModel(constructor.SyntaxTree);
        var assignments = constructor.DescendantNodes().OfType<AssignmentExpressionSyntax>().Select(node => model.GetOperation(node))
            .OfType<ISimpleAssignmentOperation>().Where(operation => operation.Target is IPropertyReferenceOperation access &&
                SymbolEqualityComparer.Default.Equals(access.Property, property)).ToArray();
        return assignments is [var assignment] && assignment.Syntax.Parent is ExpressionStatementSyntax statement &&
            statement.Parent == constructor && assignment.Target is IPropertyReferenceOperation { Instance: IInstanceReferenceOperation } &&
            assignment.Value is IObjectCreationOperation { Initializer: null, Arguments: [var argument] } creation &&
            SymbolEqualityComparer.Default.Equals(creation.Type, type) && CurrentInstance(argument.Value);

        static bool CurrentInstance(IOperation operand)
        {
            while (operand is IConversionOperation { IsImplicit: true, OperatorMethod: null } conversion)
                operand = conversion.Operand;
            return operand is IInstanceReferenceOperation { ReferenceKind: InstanceReferenceKind.ContainingTypeInstance };
        }
    }

    private static INamedTypeSymbol? ReadEventContext(Compilation compilation, INamedTypeSymbol vm) =>
        vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.EventSceneAttribute") is { } attribute &&
        FindAttribute(compilation.Assembly, attribute)?.ConstructorArguments[0].Value is INamedTypeSymbol context ? context : null;

    private static bool IsGetOnlyAutoProperty(IPropertySymbol property) => !property.IsStatic && !property.IsVirtual && !property.IsOverride &&
        property.SetMethod == null && property.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is
            PropertyDeclarationSyntax { ExpressionBody: null, AccessorList.Accessors: [var getter] } &&
        getter.IsKind(SyntaxKind.GetAccessorDeclaration) && getter.Body == null && getter.ExpressionBody == null;

    private static (byte[] Header, byte[] Trailer) ReadContainerMetadata(Compilation compilation, string directory)
    {
        var formatType = compilation.GetTypeByMetadataName($"{SourceNamespace}.EventFormatAttribute");
        if (formatType != null && FindAttribute(compilation.Assembly, formatType) is { } format)
        {
            byte version = AttributeValue<byte>(format, 0);
            var legacyType = compilation.GetTypeByMetadataName($"{SourceNamespace}.EventLayoutAttribute");
            if (legacyType != null && FindAttribute(compilation.Assembly, legacyType) != null)
                throw Invalid(format.ApplicationSyntaxReference!.GetSyntax(), "Use one event format declaration");
            if (version != EVContainer.SupportedVersion)
                throw Invalid(format.ApplicationSyntaxReference!.GetSyntax(), $"Unsupported EB format version {version}");
            string extrasPath = Path.Combine(directory, "Event.Header.bin");
            byte[] extras = File.Exists(extrasPath) ? File.ReadAllBytes(extrasPath) : new byte[EVContainer.HeaderSize - EVContainer.HeaderMetadataOffset];
            if (extras.Length != EVContainer.HeaderSize - EVContainer.HeaderMetadataOffset)
                throw new InvalidDataException($"'{extrasPath}' must contain exactly {EVContainer.HeaderSize - EVContainer.HeaderMetadataOffset} bytes.");
            byte[] header = EVContainer.CreateHeader(version, extras);
            string trailerPath = Path.Combine(directory, "Event.Trailer.bin");
            return (header, File.Exists(trailerPath) ? File.ReadAllBytes(trailerPath) : []);
        }
        var layout = FindAttribute(compilation.Assembly, RequireType(compilation, "EventLayoutAttribute"))
            ?? throw new InvalidDataException("Missing assembly EventFormat declaration.");
        byte[] legacy = HexAttribute(layout, 0);
        if (legacy.Length != EVContainer.HeaderSize)
            throw Invalid(layout.ApplicationSyntaxReference!.GetSyntax(), $"The legacy EB header must be {EVContainer.HeaderSize} bytes");
        return (legacy, HexAttribute(layout, 1));
    }

    private static IEnumerable<string> EnumerateSources(string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory, "*.cs").OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
            yield return file;
        foreach (string child in Directory.EnumerateDirectories(directory).OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
        {
            string name = Path.GetFileName(child);
            if (name.Equals("obj", StringComparison.OrdinalIgnoreCase) || name.Equals("bin", StringComparison.OrdinalIgnoreCase) || name.StartsWith('.'))
                continue;
            foreach (string file in EnumerateSources(child))
                yield return file;
        }
    }

    private static CSharpCompilation AllocateStorage(CSharpCompilation compilation)
    {
        INamedTypeSymbol? fieldType = compilation.GetTypeByMetadataName($"{SourceNamespace}.StorageFieldAttribute");
        if (fieldType == null)
            return compilation;

        var requested = new List<(AttributeSyntax Syntax, IPropertySymbol Symbol, EVStorageScope Scope, EVStorageType Type)>();
        var pinned = new List<(IPropertySymbol Symbol, EVVariable View)>();
        INamedTypeSymbol vm = RequireType(compilation, "Vm");
        INamedTypeSymbol objectType = RequireType(compilation, "ObjectSlotAttribute");
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (PropertyDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
                IPropertySymbol symbol = model.GetDeclaredSymbol(declaration)!;
                EVExpressionToken? existing = ReadStorage(symbol, vm, compilation);
                if (existing is EVExpressionToken token)
                    pinned.Add((symbol, EVVariable.Decode(token)));
                if (FindAttribute(symbol, fieldType) is not AttributeData request)
                    continue;

                var syntax = (AttributeSyntax)request.ApplicationSyntaxReference!.GetSyntax();
                if (existing.HasValue)
                    throw Invalid(syntax, "Use either StorageField or StorageView on a property");
                var scope = (EVStorageScope)AttributeValue<int>(request, 0);
                var type = (EVStorageType)AttributeValue<int>(request, 1);
                if (scope is not (EVStorageScope.Map or EVStorageScope.Instance))
                    throw Invalid(syntax, "Automatic allocation supports only location and instance storage");
                requested.Add((syntax, symbol, scope, type));
            }
        }

        if (requested.Count == 0)
            return compilation;

        var reserved = new Dictionary<(EVStorageScope Scope, int Owner), bool[]>();
        INamedTypeSymbol? reservationType = compilation.GetTypeByMetadataName($"{SourceNamespace}.MapStorageReservationAttribute");
        if (reservationType != null)
        {
            var bits = new bool[MapStorageCapacity * 8];
            foreach (AttributeData reservation in compilation.Assembly.GetAttributes().Where(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, reservationType)))
            {
                int firstBit = AttributeValue<int>(reservation, 0);
                int count = AttributeValue<int>(reservation, 1);
                if (firstBit < 0 || count <= 0 || firstBit > bits.Length - count)
                    throw Invalid(reservation.ApplicationSyntaxReference!.GetSyntax(), "Invalid map storage reservation");
                Array.Fill(bits, true, firstBit, count);
            }
            reserved.Add((EVStorageScope.Map, -1), bits);
        }
        foreach (var (symbol, view) in pinned)
        {
            if (view.Scope is not (EVStorageScope.Map or EVStorageScope.Instance))
                continue;
            bool[] occupied = Occupancy(view.Scope, symbol);
            int firstBit = view.UsesBitAddress ? view.Address : view.Address * 8;
            if (firstBit + view.Width > occupied.Length)
                throw Invalid(symbol.DeclaringSyntaxReferences[0].GetSyntax(), $"Storage view {symbol.Name} at {view.Scope} address {view.Address} exceeds its {occupied.Length / 8}-byte allocation");
            Array.Fill(occupied, true, firstBit, view.Width);
        }

        var replacements = new Dictionary<AttributeSyntax, AttributeSyntax>();
        foreach (var (syntax, symbol, scope, type) in requested)
        {
            bool[] occupied = Occupancy(scope, symbol);
            var view = new EVVariable(scope, type, 0);
            int step = view.UsesBitAddress ? 1 : 8;
            int start = 0;
            for (; start + view.Width <= occupied.Length; start += step)
            {
                if (occupied.AsSpan(start, view.Width).IndexOf(true) < 0)
                    break;
            }
            if (start + view.Width > occupied.Length)
                throw Invalid(syntax, $"No {view.Width}-bit storage is available for {symbol.Name} in its {occupied.Length / 8}-byte allocation");
            Array.Fill(occupied, true, start, view.Width);
            int address = start / step;
            AttributeArgumentListSyntax arguments = syntax.ArgumentList!.AddArguments(SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(address)))
                .WithNameColon(SyntaxFactory.NameColon("address")));
            replacements.Add(syntax, syntax.WithName(SyntaxFactory.ParseName($"global::{SourceNamespace}.StorageView")).WithArgumentList(arguments));
        }

        foreach (var group in replacements.Keys.GroupBy(syntax => syntax.SyntaxTree))
        {
            SyntaxTree tree = group.Key;
            SyntaxNode root = tree.GetRoot().ReplaceNodes(group, (original, _) => replacements[original]);
            compilation = compilation.ReplaceSyntaxTree(tree, tree.WithRootAndOptions(root, tree.Options));
        }
        return compilation;

        bool[] Occupancy(EVStorageScope scope, IPropertySymbol symbol)
        {
            var (owner, bytes) = StorageAllocation(scope, symbol, objectType);
            if (!reserved.TryGetValue((scope, owner), out bool[]? bits))
            {
                bits = new bool[bytes * 8];
                reserved.Add((scope, owner), bits);
            }
            return bits;
        }
    }

    private static (int Owner, int Capacity) StorageAllocation(EVStorageScope scope, IPropertySymbol symbol, INamedTypeSymbol objectType)
    {
        if (scope == EVStorageScope.Map)
        {
            // Memoria allocates the field's EventContext.mapvar as an 80-byte buffer.
            return (-1, MapStorageCapacity);
        }

        for (INamedTypeSymbol? type = symbol.ContainingType; type != null; type = type.ContainingType)
        {
            if (FindAttribute(type, objectType) is AttributeData slot)
                return (AttributeValue<int>(slot, 0), AttributeValue<byte>(slot, 1));
        }
        throw Invalid(symbol.DeclaringSyntaxReferences[0].GetSyntax(), "An instance storage view must belong to an object slot");
    }

    private static void ValidateStorageCapacity(PropertyDeclarationSyntax property, IPropertySymbol symbol, EVVariable view, INamedTypeSymbol objectType)
    {
        if (view.Scope is not (EVStorageScope.Map or EVStorageScope.Instance))
            return;
        int capacity = StorageAllocation(view.Scope, symbol, objectType).Capacity;
        int firstBit = view.UsesBitAddress ? view.Address : view.Address * 8;
        if (firstBit + view.Width > capacity * 8)
            throw Invalid(property, $"Storage view {symbol.Name} at {view.Scope} address {view.Address} exceeds its {capacity}-byte allocation");
    }

    private static void ValidateStorageAccessors(PropertyDeclarationSyntax property, SemanticModel model, EVVariable view, INamedTypeSymbol vm)
    {
        INamedTypeSymbol? memory = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.ScriptMemory");
        var getter = property.AccessorList?.Accessors.FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.GetAccessorDeclaration));
        var setter = property.AccessorList?.Accessors.FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.SetAccessorDeclaration));
        IOperation? read = getter?.ExpressionBody is { } getBody ? model.GetOperation(getBody.Expression) : null;
        IOperation? write = setter?.ExpressionBody is { } setBody ? model.GetOperation(setBody.Expression) : null;
        if (view.UsesBitAddress)
        {
            read = read is IBinaryOperation { OperatorKind: BinaryOperatorKind.NotEquals } test && IsNumber(test.RightOperand, 0)
                ? test.LeftOperand : null;
        }
        bool byDeclaration = MatchesNamedView(read, "ReadView", 2) && MatchesNamedView(write, "WriteView", 3) &&
            IsSetterValue(((IInvocationOperation)write!).Arguments[1].Value);
        if (!byDeclaration && (!Matches(read, "Read", 3) || !Matches(write, "Write", 4) || !IsSetterValue(((IInvocationOperation)write!).Arguments[3].Value)))
            throw Invalid(property, $"Storage property {property.Identifier.ValueText} must use ScriptMemory accessors matching its StorageView");

        bool MatchesNamedView(IOperation? operation, string method, int count) =>
            operation is IInvocationOperation call && call.TargetMethod.Name == method &&
            SymbolEqualityComparer.Default.Equals(call.TargetMethod.ContainingType, memory) && call.Arguments.Length == count &&
            call.Arguments.Select((argument, index) => argument.Parameter?.Ordinal == index).All(matches => matches) &&
            call.Arguments[0].Value is ITypeOfOperation owner &&
            SymbolEqualityComparer.Default.Equals(owner.TypeOperand, model.GetDeclaredSymbol(property)?.ContainingType) &&
            call.Arguments[^1].IsImplicit;

        bool Matches(IOperation? operation, string method, int count) =>
            operation is IInvocationOperation call && call.TargetMethod.Name == method &&
            SymbolEqualityComparer.Default.Equals(call.TargetMethod.ContainingType, memory) && call.Arguments.Length == count &&
            call.Arguments.Select((argument, index) => argument.Parameter?.Ordinal == index).All(matches => matches) &&
            IsNumber(call.Arguments[0].Value, (int)view.Scope) && IsNumber(call.Arguments[1].Value, (int)view.Type) &&
            IsNumber(call.Arguments[2].Value, view.Address);

        bool IsSetterValue(IOperation operation) => view.UsesBitAddress
            ? operation is IConditionalOperation conditional && IsValueParameter(conditional.Condition) &&
                IsNumber(conditional.WhenTrue, 1) && IsNumber(conditional.WhenFalse, 0)
            : IsValueParameter(operation);

        static bool IsValueParameter(IOperation operation) =>
            operation is IParameterReferenceOperation { Parameter.ContainingSymbol: IMethodSymbol { MethodKind: MethodKind.PropertySet } };

        static bool IsNumber(IOperation? operation, int expected) =>
            operation?.ConstantValue is { HasValue: true, Value: int value } && value == expected;
    }

    private static CSharpCompilation BindManagedAccessors(CSharpCompilation compilation)
    {
        INamedTypeSymbol vm = RequireType(compilation, "Vm");
        foreach (SyntaxTree tree in compilation.SyntaxTrees.ToArray())
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            SyntaxNode root = tree.GetRoot();
            var views = new Dictionary<PropertyDeclarationSyntax, EVVariable>();
            foreach (PropertyDeclarationSyntax property in root.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
                if (property.Initializer == null && property.AccessorList?.Accessors is { Count: 2 } accessors &&
                    accessors.All(accessor => accessor.Body == null && accessor.ExpressionBody == null) &&
                    accessors.Any(accessor => accessor.IsKind(SyntaxKind.SetAccessorDeclaration)) &&
                    model.GetDeclaredSymbol(property) is IPropertySymbol symbol && ReadStorage(symbol, vm, compilation) is EVExpressionToken token)
                    views.Add(property, EVVariable.Decode(token));
            }
            if (views.Count == 0)
                continue;
            SyntaxNode rewritten = root.ReplaceNodes(views.Keys, (original, property) =>
            {
                EVVariable view = views[original];
                string arguments = $"(global::{SourceNamespace}.StorageScope){(int)view.Scope}, (global::{SourceNamespace}.StorageType){(int)view.Type}, {view.Address}";
                string read = $"global::{SourceNamespace}.ScriptMemory.Read({arguments})";
                string value = view.UsesBitAddress ? "value ? 1 : 0" : "value";
                return property.ReplaceNodes(property.AccessorList!.Accessors, (accessor, _) =>
                {
                    string code = accessor.IsKind(SyntaxKind.GetAccessorDeclaration)
                        ? (view.UsesBitAddress ? read + " != 0" : read)
                        : $"global::{SourceNamespace}.ScriptMemory.Write({arguments}, {value})";
                    return accessor.WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.ParseExpression(code)));
                });
            });
            compilation = compilation.ReplaceSyntaxTree(tree, tree.WithRootAndOptions(rewritten, tree.Options));
            vm = RequireType(compilation, "Vm");
        }
        return compilation;
    }

    private static EVContainerObject CompileObject(CSharpCompilation compilation, INamedTypeSymbol owner, ClassDeclarationSyntax cls, AttributeData declaration, INamedTypeSymbol scriptType, INamedTypeSymbol vm, int id, int objectCount, IReadOnlySet<int> declaredSlots, List<(int Caller, ushort Handler, int Tag, SyntaxNode Source)> requests, IEnumerable<MethodDeclarationSyntax> cutsceneEntries, ParallelGroups parallelGroups, Directors directors)
    {
        int order = AttributeValue<int>(declaration, 4);
        if (order < -1)
            throw Invalid(cls, $"Invalid object {id} body order {order}");
        var obj = new EVContainerObject(id, AttributeValue<byte>(declaration, 1), AttributeValue<byte>(declaration, 2), AttributeValue<ushort>(declaration, 3))
        {
            BodyOrder = order < 0 ? null : order,
            EmptyOffset = AttributeValue<ushort>(declaration, 5),
            Type = AttributeValue<byte>(declaration, 6),
            Gap = HexAttribute(declaration, 7),
            Prefix = HexAttribute(declaration, 8)
        };

        var entries = new SortedDictionary<int, EVContainerScript>();
        var scriptEntries = new List<(ushort Tag, int Index, bool OriginalDuplicate, MethodDeclarationSyntax Method)>();
        INamedTypeSymbol? duplicateType = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.OriginalDuplicateTagAttribute");
        bool instanceParticipant = DerivesFrom(owner, vm, "EventParticipant");
        foreach (MemberDeclarationSyntax member in owner.DeclaringSyntaxReferences.SelectMany(reference => ((ClassDeclarationSyntax)reference.GetSyntax()).Members).Concat(cutsceneEntries))
        {
            SemanticModel model = compilation.GetSemanticModel(member.SyntaxTree);
            if (instanceParticipant && (member is ConstructorDeclarationSyntax ||
                member is PropertyDeclarationSyntax property && model.GetDeclaredSymbol(property)?.Type is { } type &&
                (DerivesFrom(type, vm, "EventSceneContext") || property.Identifier.ValueText is "Location" or "Globals" or "Local")))
                continue;
            if (member is ClassDeclarationSyntax storage && (storage.Modifiers.Any(SyntaxKind.StaticKeyword) || instanceParticipant && storage.Identifier.ValueText == "LocalMemory") &&
                storage.Members.All(child =>
                    child is PropertyDeclarationSyntax property && model.GetDeclaredSymbol(property) is IPropertySymbol symbol &&
                        (ReadStorage(symbol, vm, compilation) != null || symbol.RefKind == RefKind.Ref && IsExplicitLayout(symbol.Type, compilation)) ||
                    child is StructDeclarationSyntax layout && model.GetDeclaredSymbol(layout) is { } type && IsExplicitLayout(type, compilation)))
                continue;
            if (member is MethodDeclarationSyntax helper && model.GetDeclaredSymbol(helper) is IMethodSymbol { DeclaredAccessibility: Accessibility.Private } local &&
                FindAttribute(local, scriptType) == null)
                continue;
            if (member is not MethodDeclarationSyntax method ||
                model.GetDeclaredSymbol(method) is not IMethodSymbol symbol ||
                FindAttribute(symbol, scriptType) is not AttributeData script ||
                symbol.IsStatic == instanceParticipant || (!symbol.ReturnsVoid && !IsSupportType(symbol.ReturnType, vm, "EventRoutine")) ||
                symbol.Parameters.Length != (DerivesFrom(symbol.ContainingType, vm, "EventCutscene") ? 1 : 0) || symbol.TypeParameters.Length != 0 ||
                method.Body == null)
                throw Invalid(member, $"Unsupported script declaration in object {id}");

            ushort sid = AttributeValue<ushort>(script, 0);
            int index = AttributeValue<int>(script, 1);
            if (index < 0 || entries.ContainsKey(index))
                throw Invalid(method, $"Invalid or duplicate script directory index {index} in object {id}");
            bool originalDuplicate = duplicateType != null && FindAttribute(symbol, duplicateType) is AttributeData original &&
                AttributeValue<ushort>(original, 0) == sid && AttributeValue<int>(original, 1) == index;
            scriptEntries.Add((sid, index, originalDuplicate, method));
            entries.Add(index, CompileEntry(model, vm, symbol, method, id, sid, objectCount, declaredSlots, requests, parallelGroups, directors));
        }
        foreach (var group in scriptEntries.GroupBy(entry => entry.Tag))
        {
            if (group.Count() > 1 && !group.All(entry => entry.OriginalDuplicate))
                throw Invalid(group.First(entry => !entry.OriginalDuplicate).Method, $"Object {id}: duplicate handler tag {group.Key}");
        }
        if (!entries.Keys.SequenceEqual(Enumerable.Range(0, entries.Count)))
            throw Invalid(cls, $"Missing script directory index in object {id}");

        obj.Scripts = entries.Values.ToArray();
        return obj;
    }

    private static EVContainerScript CompileEntry(SemanticModel model, INamedTypeSymbol vm, IMethodSymbol symbol, MethodDeclarationSyntax method, int id, ushort tag, int objectCount, IReadOnlySet<int> declaredSlots, List<(int Caller, ushort Handler, int Tag, SyntaxNode Source)> requests, ParallelGroups parallelGroups, Directors directors, bool ensureReturn = false)
    {
        var locations = new Dictionary<int, SyntaxNode>();
        try
        {
            EVInstructionBody body = new BodyCompiler(model, vm, locations, symbol, parallelGroups, directors).Compile(method.Body!, ensureReturn);
            INamedTypeSymbol? paddingType = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.TrailingPaddingAttribute");
            if (paddingType != null && FindAttribute(symbol, paddingType) is AttributeData padding)
            {
                int count = AttributeValue<int>(padding, 0);
                if (count < 0 || count > ushort.MaxValue)
                    throw Invalid(method, "Invalid trailing padding count");
                if (body.Instructions.LastOrDefault()?.Opcode is (Jsm.Opcode.Return or Jsm.Opcode.JMP) &&
                    !body.Instructions.Any(instruction => instruction.Targets.Contains(body.EndLabel)))
                {
                    for (int pad = 0; pad < count; pad++)
                        body.Instructions.Add(new EVInstruction(body.EndLabel + pad, Jsm.Opcode.NOP));
                    body = new EVInstructionBody(body.Instructions, body.EndLabel + count);
                }
            }
            foreach (EVInstruction operation in body.Instructions)
            {
                if (operation.Opcode is (Jsm.Opcode.REQ or Jsm.Opcode.REQSW or Jsm.Opcode.REQEW) &&
                    operation.Operands[1].Expression == null && operation.Operands[1].Value == 250)
                    requests.Add((id, tag, operation.Operands[2].Value, locations[operation.Label]));
            }
            body.ValidateObjectSlots(objectCount, declaredSlots.Contains);
            byte[] bytes = body.Encode(out EVAddressCheckpoint[] checkpoints);
            return new EVContainerScript(tag, bytes) { AddressChecks = checkpoints };
        }
        catch (InvalidDataException error)
        {
            SyntaxNode source = error.Data[nameof(EVInstruction.Label)] is int label && locations.TryGetValue(label, out SyntaxNode? statement)
                ? statement : method;
            throw Invalid(source, $"Object {id}, script {tag}: {error.Message}", error);
        }
    }

    private sealed partial class BodyCompiler
    {
        private SemanticModel model;
        private IMethodSymbol handler;
        private readonly INamedTypeSymbol vm;
        private readonly IMethodSymbol _entryHandler;
        private readonly Dictionary<int, SyntaxNode> locations;
        private readonly List<EVInstruction> _instructions = new();
        private readonly Dictionary<int, int> _labels = new();
        private Dictionary<ILabelSymbol, int> _namedLabels = new(SymbolEqualityComparer.Default);
        private readonly HashSet<IMethodSymbol> _inlineStack = new(SymbolEqualityComparer.Default);
        private readonly Stack<int> _breakTargets = new();
        private readonly Stack<int> _continueTargets = new();
        private readonly Dictionary<EmptyStatementSyntax, int> _emptyJumpSuccessors = new();
        private int _nextLabel = -1;
        private int? _expectedIp;
        private int? _inlineReturnTarget;
        private int? _inlineSwitchExitTarget;

        private readonly ParallelGroups _parallelGroups;
        private readonly Directors _directors;

        public BodyCompiler(SemanticModel model, INamedTypeSymbol vm, Dictionary<int, SyntaxNode> locations, IMethodSymbol handler, ParallelGroups parallelGroups, Directors directors)
        {
            this.model = model;
            this.vm = vm;
            this.locations = locations;
            this.handler = handler;
            _entryHandler = handler;
            _parallelGroups = parallelGroups;
            _directors = directors;
        }

        public EVInstructionBody Compile(BlockSyntax body, bool ensureReturn = false)
        {
            CompileBlock(body);
            if (ensureReturn && model.AnalyzeControlFlow(body)?.EndPointIsReachable == true)
                Add(new EVInstruction(0, Jsm.Opcode.Return), body);
            if (_expectedIp.HasValue)
                throw Invalid(body, "An address checkpoint requires an instruction.");

            foreach (EVInstruction instruction in _instructions)
            {
                instruction.Targets = instruction.Targets.Select(target =>
                {
                    if (!_labels.TryGetValue(target, out int position))
                        throw Invalid(locations[instruction.Label], $"Missing instruction label {target}");
                    return position;
                }).ToArray();
            }
            return new EVInstructionBody(_instructions, _instructions.Count);
        }

        private void CompileBlock(BlockSyntax block)
        {
            foreach (StatementSyntax statement in block.Statements)
                CompileStatement(statement);
        }

        private void CompileStatement(StatementSyntax statement)
        {
            try
            {
                switch (statement)
                {
                    case EmptyStatementSyntax empty when IsPreservedEmptyJump(empty):
                        int successor = _emptyJumpSuccessors.Remove(empty, out int reserved) ? reserved : NewLabel();
                        Jump(Jsm.Opcode.JMP, successor, empty);
                        Mark(successor);
                        return;
                    case LocalDeclarationStatementSyntax { IsConst: true } local
                        when local.Declaration.Variables.All(variable => model.GetDeclaredSymbol(variable) is ILocalSymbol { ConstantValue: int }):
                    case LocalDeclarationStatementSyntax alias when alias.Declaration.Variables.All(variable =>
                        variable.Initializer is { } initializer && model.GetOperation(initializer.Value) is IPropertyReferenceOperation member &&
                        IsSceneContextAlias(member, handler, vm)):
                    case LocalDeclarationStatementSyntax cast when cast.Declaration.Variables is [var variable] &&
                        variable.Initializer is { } castInitializer &&
                        model.GetOperation(castInitializer.Value) is IObjectCreationOperation creation &&
                        creation.Type is INamedTypeSymbol castType &&
                        IsSupportType(castType.OriginalDefinition, vm, "SceneRoleCast`1") && ValidateRoleCast(creation, handler):
                    case LocalDeclarationStatementSyntax roleAlias when roleAlias.Declaration.Variables is [var aliasVariable] &&
                        aliasVariable.Initializer is { } aliasInitializer &&
                        model.GetOperation(aliasInitializer.Value) is IPropertyReferenceOperation roles && IsRoleStorageReceiver(roles, handler, vm):
                    case EmptyStatementSyntax:
                        return;
                    case LabeledStatementSyntax labeled when model.GetOperation(labeled) is ILabeledOperation boundLabel:
                        Mark(NamedLabel(boundLabel.Label));
                        CompileStatement(labeled.Statement);
                        return;
                    case GotoStatementSyntax jump:
                        Jump(Jsm.Opcode.JMP, GotoTarget(jump), jump);
                        return;
                    case BlockSyntax block:
                        CompileBlock(block);
                        return;
                    case ExpressionStatementSyntax expression:
                        CompileExpressionStatement(model.GetOperation(expression.Expression), statement);
                        return;
                    case IfStatementSyntax conditional:
                        CompileIf(conditional);
                        return;
                    case WhileStatementSyntax loop:
                        CompileWhile(loop);
                        return;
                    case ForStatementSyntax loop:
                        CompileFor(loop);
                        return;
                    case DoStatementSyntax postTestLoop:
                        CompileDo(postTestLoop);
                        return;
                    case SwitchStatementSyntax selection:
                        CompileSwitch(selection);
                        return;
                    case BreakStatementSyntax when _breakTargets.TryPeek(out int exit):
                        Jump(Jsm.Opcode.JMP, exit, statement);
                        return;
                    case ContinueStatementSyntax when _continueTargets.TryPeek(out int continuation):
                        Jump(Jsm.Opcode.JMP, continuation, statement);
                        return;
                    case ReturnStatementSyntax { Expression: { } forward } when model.GetOperation(forward) is IInvocationOperation call &&
                        statement.Parent is BlockSyntax { Statements.Count: 1 } && statement.Parent.Parent is MethodDeclarationSyntax &&
                        TryInline(call, forwardEntireEntry: true):
                        return;
                    case ReturnStatementSyntax { Expression: null }:
                        if (_inlineReturnTarget is int returnTarget)
                            Jump(Jsm.Opcode.JMP, returnTarget, statement);
                        else
                            Add(new EVInstruction(0, Jsm.Opcode.Return), statement);
                        return;
                    default:
                        throw Invalid(statement, "Unsupported statement");
                }
            }
            catch (OverflowException ex)
            {
                throw Invalid(statement, ex.Message, ex);
            }
        }

        private void CompileExpressionStatement(IOperation? expression, StatementSyntax statement)
        {
            bool awaited = expression is IAwaitOperation;
            if (expression is IAwaitOperation suspension)
                expression = suspension.Operation;

            if (expression is IInvocationOperation invocation)
            {
                if (IsInlineCandidate(invocation.TargetMethod))
                {
                    if (awaited != IsSupportType(invocation.TargetMethod.ReturnType, vm, "EventRoutine"))
                        throw Invalid(statement, "Await a suspending scene action at its original call site");
                    bool forwardsVoidEntry = invocation.TargetMethod.ReturnsVoid && SymbolEqualityComparer.Default.Equals(handler, _entryHandler) &&
                        statement.Parent is BlockSyntax { Statements.Count: 1, Parent: MethodDeclarationSyntax };
                    TryInline(invocation, forwardEntireEntry: forwardsVoidEntry);
                    return;
                }
                bool requiresAwait = IsSupportType(invocation.TargetMethod.ReturnType, vm, "EventAwaitable");
                if (awaited != requiresAwait)
                    throw Invalid(statement, requiresAwait ? "A suspending operation must be awaited" : "This operation cannot be awaited");
                if (IsStartDirector(invocation.TargetMethod, vm))
                    Add(_directors.Start(invocation, handler, vm), statement);
                else if (TryCompileRoleRequest(invocation, statement))
                    return;
                else if (TryCompileService(invocation, vm) is EVInstruction serviceInstruction)
                    Add(serviceInstruction, statement);
                else if (!awaited && TryGetExpression(invocation, vm, out SourceExpression? operation) && operation.Statement)
                    Evaluate(new EVOperand(new Intrinsic(invocation, vm, service: true).EncodeValue(operation)), statement);
                else
                    CompileCall(new Intrinsic(invocation, vm), statement);
                return;
            }
            if (!awaited && expression is (IAssignmentOperation or IIncrementOrDecrementOperation))
            {
                Evaluate(new EVOperand(ReadNative(expression, vm, expression.Syntax)), statement);
                return;
            }
            throw Invalid(statement, "Unsupported statement");
        }

        private bool IsInlineCandidate(IMethodSymbol method) =>
            SymbolEqualityComparer.Default.Equals(method.ContainingAssembly, model.Compilation.Assembly) &&
            (DerivesFrom(method.ContainingType, vm, "EventCutscene") ||
             method.DeclaredAccessibility == Accessibility.Private && SymbolEqualityComparer.Default.Equals(method.ContainingType, handler.ContainingType));

        private bool TryInline(IInvocationOperation call, bool forwardEntireEntry)
        {
            IMethodSymbol target = call.TargetMethod;
            if (!IsInlineCandidate(target))
                return false;
            if (target.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not MethodDeclarationSyntax { Body: { } body } ||
                HasSupportAttribute(target, vm, "ScriptAttribute") || target.IsGenericMethod)
                throw Invalid(call.Syntax, "Expected an ordinary scene method with a body");
            if (DerivesFrom(target.ContainingType, vm, "EventCutscene"))
            {
                if (!IsBoundCutsceneReceiver(call.Instance, target.ContainingType, handler, vm, model.Compilation))
                    throw Invalid(call.Syntax, "Use the registered cutscene instance on the current event context as the receiver");
                if (call.Syntax.Parent?.HasAnnotation(ChoreographyAction) == true && call.Arguments.IsEmpty && target.ReturnsVoid)
                {
                    Inline(target, body, call.Syntax, forwardEntireEntry);
                    return true;
                }
                if (IsDirectorMethod(target, vm))
                {
                    Inline(target, body, call.Syntax, forwardEntireEntry);
                    if (forwardEntireEntry && model.Compilation.GetSemanticModel(body.SyntaxTree).AnalyzeControlFlow(body)?.EndPointIsReachable == true)
                        Add(new EVInstruction(0, Jsm.Opcode.Return), call.Syntax);
                    return true;
                }
                if (call.Arguments is not [var argument] || argument.Parameter!.RefKind != RefKind.None || !IsCurrentParticipant(argument.Value))
                    throw Invalid(call.Syntax, "An inline scene method must receive the current participant");
                if (IsSupportType(argument.Parameter!.Type, vm, "EventActor"))
                {
                    if (forwardEntireEntry)
                        throw Invalid(call.Syntax, "Await a parallel scene from an existing participant phase");
                    if (!IsDirectCurrentParticipant(argument.Value, handler))
                        throw Invalid(argument.Syntax, "Pass the acting participant directly to the parallel phase");
                    InlineTogether(target, body, call.Syntax);
                    return true;
                }
                if (!SymbolEqualityComparer.Default.Equals(argument.Parameter.Type, ExecutionOwner(handler, vm)))
                    throw Invalid(call.Syntax, "An inline scene method must receive the current participant");
            }
            else if (call.Arguments.Length != 0 || call.Instance is not IInstanceReferenceOperation && !target.IsStatic)
            {
                throw Invalid(call.Syntax, "An inline helper must run on the current participant without arguments");
            }
            Inline(target, body, call.Syntax, forwardEntireEntry);
            return true;
        }

        private void Inline(IMethodSymbol target, BlockSyntax body, SyntaxNode use, bool forwardEntireEntry = false, bool allowEarlyReturn = true)
        {
            StatementSyntax? tail = body.Statements.LastOrDefault();
            var labeledExit = !forwardEntireEntry && tail is LabeledStatementSyntax { Statement: ReturnStatementSyntax { Expression: null } } labeled ? labeled : null;
            bool earlyReturn = !forwardEntireEntry && (labeledExit != null || body.DescendantNodes().OfType<ReturnStatementSyntax>().Any(exit => exit != tail));
            if (earlyReturn && !allowEarlyReturn)
                throw Invalid(use, "This scene method has an early return that cannot be inlined into a continuing handler");
            int? continuation = earlyReturn ? InlineContinuation(use) : null;
            int? localExit = earlyReturn && continuation == null ? NewLabel() : null;
            if (!_inlineStack.Add(target))
                throw Invalid(use, "Recursive scene methods cannot be lowered to EB");
            var previousModel = model;
            var previousHandler = handler;
            var previousLabels = _namedLabels;
            int? previousReturnTarget = _inlineReturnTarget;
            int? previousSwitchExitTarget = _inlineSwitchExitTarget;
            _inlineSwitchExitTarget = use.FirstAncestorOrSelf<ExpressionStatementSyntax>() is { } callStatement ? SwitchExitFollowing(callStatement) : null;
            model = model.Compilation.GetSemanticModel(body.SyntaxTree);
            handler = target;
            _namedLabels = new(SymbolEqualityComparer.Default);
            _inlineReturnTarget = continuation ?? localExit;
            try
            {
                if (labeledExit != null && _inlineReturnTarget is int returnTarget && model.GetOperation(labeledExit) is ILabeledOperation operation)
                    _namedLabels.Add(operation.Label, returnTarget);
                foreach (StatementSyntax statement in body.Statements)
                {
                    if (forwardEntireEntry || statement != tail || (labeledExit == null && statement is not ReturnStatementSyntax { Expression: null }))
                        CompileStatement(statement);
                }
                if (localExit is int end)
                    Mark(end);
            }
            finally
            {
                model = previousModel;
                handler = previousHandler;
                _namedLabels = previousLabels;
                _inlineReturnTarget = previousReturnTarget;
                _inlineSwitchExitTarget = previousSwitchExitTarget;
                _inlineStack.Remove(target);
            }
        }

        private int? InlineContinuation(SyntaxNode use)
        {
            var call = use.Ancestors().OfType<ExpressionStatementSyntax>().FirstOrDefault();
            if (call?.Parent is BlockSyntax block)
            {
                int next = block.Statements.IndexOf(call) + 1;
                if (next < block.Statements.Count && block.Statements[next] is GotoStatementSyntax jump)
                    return GotoTarget(jump);
                if (next < block.Statements.Count && block.Statements[next] is EmptyStatementSyntax empty && IsPreservedEmptyJump(empty))
                {
                    int successor = NewLabel();
                    _emptyJumpSuccessors.Add(empty, successor);
                    return successor;
                }
                if (next < block.Statements.Count && block.Statements[next] is LabeledStatementSyntax label && model.GetOperation(label) is ILabeledOperation operation)
                    return NamedLabel(operation.Label);
            }
            return call?.Parent is SwitchSectionSyntax ? SwitchExitFollowing(call) : null;
        }

        private bool IsPreservedEmptyJump(EmptyStatementSyntax statement) =>
            EmptyJumpLayout.IsMarker(statement) && HasSupportAttribute(handler, vm, nameof(PreserveEmptyJumpsAttribute));

        private void CompileCall(Intrinsic call, StatementSyntax statement)
        {
            switch (call.Name)
            {
                case "Label":
                    int label = call.Int(0);
                    if (label < 0 || !_labels.TryAdd(label, _instructions.Count))
                        throw Invalid(statement, $"Invalid or duplicate instruction label {label}");
                    return;
                case "WatchAddress":
                    int ip = call.Int(0);
                    if (_expectedIp.HasValue || ip < 0)
                        throw Invalid(statement, "Invalid address checkpoint");
                    _expectedIp = ip;
                    return;
                case "WaitThenReturn":
                    foreach (EVInstruction step in EVInstructionSimplifier.ExpandWaitThenReturn(0, 0, call.Operand(0), call.Byte(1)))
                        Add(step, statement);
                    return;
                default:
                    Add(CompileInstruction(call, 0), statement);
                    return;
            }
        }

        private void CompileIf(IfStatementSyntax statement)
        {
            if (statement.Else == null && statement.Statement is ReturnStatementSyntax { Expression: null } && _inlineReturnTarget is int returnTarget)
            {
                var (value, inverted, _) = ReadCondition(statement.Condition, allowTail: false);
                EmitCondition(value, !inverted, returnTarget, statement.Condition);
                return;
            }
            if (statement.Else == null && statement.Statement is GotoStatementSyntax jump)
            {
                var (value, inverted, _) = ReadCondition(statement.Condition, allowTail: false);
                EmitCondition(value, !inverted, GotoTarget(jump), statement.Condition);
                return;
            }

            int alternative = NewLabel();
            Condition(statement.Condition, alternative);
            CompileStatement(statement.Statement);
            if (statement.Else == null)
            {
                Mark(alternative);
                return;
            }

            int join = NewLabel();
            Jump(Jsm.Opcode.JMP, join, statement.Else);
            Mark(alternative);
            CompileStatement(statement.Else.Statement);
            Mark(join);
        }

        private void CompileWhile(WhileStatementSyntax statement)
        {
            int entry = NewLabel();
            int exit = NewLabel();
            if (model.GetConstantValue(statement.Condition) is { HasValue: true, Value: true })
            {
                Mark(entry);
                CompileLoopBody(statement.Statement, exit, entry);
                Jump(Jsm.Opcode.JMP, entry, statement);
            }
            else
            {
                var (value, inverted, atTail) = ReadCondition(statement.Condition, allowTail: true);
                if (atTail)
                {
                    if (inverted)
                        throw Invalid(statement.Condition, "Negate the VM value inside TestAtLoopTail");
                    int test = NewLabel();
                    Jump(Jsm.Opcode.JMP, test, statement);
                    Mark(entry);
                    CompileLoopBody(statement.Statement, exit, test);
                    Mark(test);
                    Evaluate(value, statement.Condition);
                    Jump(Jsm.Opcode.JMP_IF, entry, statement.Condition);
                }
                else
                {
                    Mark(entry);
                    EmitCondition(value, inverted, exit, statement.Condition);
                    CompileLoopBody(statement.Statement, exit, entry);
                    Jump(Jsm.Opcode.JMP, entry, statement);
                }
            }
            Mark(exit);
        }

        private void CompileFor(ForStatementSyntax statement)
        {
            if (statement.Declaration != null || statement.Initializers.Count != 0 || statement.Incrementors.Count != 0 || statement.Condition == null)
                throw Invalid(statement, "Expected a for loop with only a condition");
            var (value, inverted, _) = ReadCondition(statement.Condition, allowTail: false);
            int entry = NewLabel();
            int test = NewLabel();
            int exit = NewLabel();
            Jump(Jsm.Opcode.JMP, test, statement);
            Mark(entry);
            CompileLoopBody(statement.Statement, exit, test);
            Mark(test);
            Evaluate(value, statement.Condition);
            Jump(inverted ? Jsm.Opcode.JMP_IFN : Jsm.Opcode.JMP_IF, entry, statement.Condition);
            Mark(exit);
        }

        private void CompileDo(DoStatementSyntax statement)
        {
            var (value, inverted, _) = ReadCondition(statement.Condition, allowTail: false);
            if (inverted)
                throw Invalid(statement.Condition, "Negate the VM value inside Test");
            int entry = NewLabel();
            int exit = NewLabel();
            int test = NewLabel();
            Mark(entry);
            CompileLoopBody(statement.Statement, exit, test);
            Mark(test);
            Evaluate(value, statement.Condition);
            Jump(Jsm.Opcode.JMP_IF, entry, statement.Condition);
            Mark(exit);
        }

        private void CompileLoopBody(StatementSyntax statement, int exit, int continuation)
        {
            _continueTargets.Push(continuation);
            _breakTargets.Push(exit);
            CompileStatement(statement);
            _breakTargets.Pop();
            _continueTargets.Pop();
        }

        private void Condition(ExpressionSyntax source, int onFalse)
        {
            var (value, inverted, _) = ReadCondition(source, allowTail: false);
            EmitCondition(value, inverted, onFalse, source);
        }

        private void EmitCondition(EVOperand value, bool inverted, int onFalse, SyntaxNode source)
        {
            Evaluate(value, source);
            Jump(inverted ? Jsm.Opcode.JMP_IF : Jsm.Opcode.JMP_IFN, onFalse, source);
        }

        private (EVOperand Value, bool Inverted, bool AtTail) ReadCondition(ExpressionSyntax expression, bool allowTail)
        {
            bool inverted = false;
            while (expression is ParenthesizedExpressionSyntax parenthesis)
                expression = parenthesis.Expression;
            if (expression is PrefixUnaryExpressionSyntax prefix && prefix.IsKind(SyntaxKind.LogicalNotExpression))
            {
                expression = prefix.Operand;
                inverted = true;
            }

            while (expression is ParenthesizedExpressionSyntax parentheses)
                expression = parentheses.Expression;
            IOperation? operation = model.GetOperation(expression);
            if (operation is IInvocationOperation invocation && SymbolEqualityComparer.Default.Equals(invocation.TargetMethod.ContainingType, vm)
                && invocation.TargetMethod.Name is "Test" or "TestAtLoopTail")
            {
                var call = new Intrinsic(invocation, vm);
                bool atTail = call.Name == "TestAtLoopTail";
                if (atTail && !allowTail)
                    throw Invalid(expression, "This loop layout is unavailable for this statement");
                return (call.Operand(0), inverted, atTail);
            }
            if (operation is IInvocationOperation choice && IsInlineCandidate(choice.TargetMethod) && DerivesFrom(choice.TargetMethod.ContainingType, vm, "EventCutscene"))
            {
                var (value, innerInverted, atTail) = ReadCutsceneCondition(choice, allowTail);
                return (value, inverted ^ innerInverted, atTail);
            }
            if (operation?.Type?.SpecialType != SpecialType.System_Boolean)
                throw Invalid(expression, "Expected a Boolean condition");
            return (new EVOperand(ReadNative(operation, vm, expression, truthOnly: true)), inverted, false);
        }

        private (EVOperand Value, bool Inverted, bool AtTail) ReadCutsceneCondition(IInvocationOperation invocation, bool allowTail)
        {
            IMethodSymbol target = invocation.TargetMethod;
            if (target.ReturnType.SpecialType != SpecialType.System_Boolean || target.ReturnsByRef || target.ReturnsByRefReadonly ||
                target.IsGenericMethod || target.IsStatic || target.IsVirtual || target.IsOverride ||
                target.Parameters.Length != 0 || HasSupportAttribute(target, vm, "ScriptAttribute") ||
                target.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not MethodDeclarationSyntax { ExpressionBody.Expression: { } condition } ||
                !IsBoundCutsceneReceiver(invocation.Instance, target.ContainingType, handler, vm, model.Compilation))
                throw Invalid(invocation.Syntax, "Use an expression-bodied Boolean method without parameters on the registered cutscene instance");
            if (!_inlineStack.Add(target))
                throw Invalid(invocation.Syntax, "Recursive scene methods cannot be lowered to EB");

            SemanticModel previousModel = model;
            IMethodSymbol previousHandler = handler;
            model = model.Compilation.GetSemanticModel(condition.SyntaxTree);
            handler = target;
            try
            {
                return ReadCondition(condition, allowTail);
            }
            finally
            {
                model = previousModel;
                handler = previousHandler;
                _inlineStack.Remove(target);
            }
        }

        private void CompileSwitch(SwitchStatementSyntax statement)
        {
            IOperation? selector = (model.GetOperation(statement) as ISwitchOperation)?.Value;
            Intrinsic? legacy = selector is IInvocationOperation invocation && invocation.TargetMethod.Name == "Select"
                ? new Intrinsic(invocation, vm) : null;
            EVOperand value;
            if (legacy != null)
                value = legacy.Operand(1);
            else if (selector is { Type: { } selectorType } &&
                (selectorType.SpecialType == SpecialType.System_Int32 ||
                 selectorType is INamedTypeSymbol { EnumUnderlyingType.SpecialType: SpecialType.System_Int32 }))
                value = new EVOperand(ReadNative(selector, vm, statement.Expression));
            else
                throw Invalid(statement.Expression, "Expected an integer or integer-backed enum switch expression");
            bool directDispatch = IsDirectDispatch(statement);
            int exit = NewLabel();
            int fallback = exit;
            var cases = new Dictionary<int, int>();
            var guards = new Dictionary<int, List<(int[] Values, ExpressionSyntax Condition)>>();
            var guardedValues = new HashSet<int>();
            var sections = new List<(SwitchSectionSyntax Section, int Label)>();
            foreach (SwitchSectionSyntax section in statement.Sections)
            {
                int label = directDispatch ? GotoTarget((GotoStatementSyntax)section.Statements[0]) : NewLabel();
                sections.Add((section, label));
                foreach (SwitchLabelSyntax item in section.Labels)
                {
                    if (!directDispatch && model.GetOperation(item) is ICaseClauseOperation { Label: { } caseSymbol })
                        _namedLabels.Add(caseSymbol, label);
                    if (item is DefaultSwitchLabelSyntax)
                        fallback = label;
                    else if (item is CaseSwitchLabelSyntax caseLabel && model.GetConstantValue(caseLabel.Value) is { HasValue: true, Value: int caseValue } && cases.TryAdd(caseValue, label))
                        continue;
                    else if (item is CasePatternSwitchLabelSyntax { WhenClause.Condition: { } guard } pattern &&
                        TryReadGuardedCaseValues(pattern.Pattern, out int[] guardedCases) && !guardedCases.Any(guardedValues.Contains))
                    {
                        if (!guards.TryGetValue(label, out var sectionGuards))
                        {
                            sectionGuards = new();
                            guards.Add(label, sectionGuards);
                        }
                        sectionGuards.Add((guardedCases, guard));
                        guardedValues.UnionWith(guardedCases);
                    }
                    else
                        throw Invalid(item, "Expected a unique integer case value");
                }
            }
            var guardEntries = new Dictionary<int, List<(int Entry, int Fallback, ExpressionSyntax Condition)>>();
            foreach (var (label, sectionGuards) in guards)
            {
                var entries = new List<(int Entry, int Fallback, ExpressionSyntax Condition)>();
                foreach (var (values, guard) in sectionGuards)
                {
                    foreach (var shared in values.GroupBy(caseValue => cases.GetValueOrDefault(caseValue, fallback)))
                    {
                        int entry = NewLabel();
                        entries.Add((entry, shared.Key, guard));
                        foreach (int caseValue in shared)
                            cases[caseValue] = entry;
                    }
                }
                guardEntries.Add(label, entries);
            }

            int[] order = cases.Keys.ToArray();
            EVVariable? choice = statement.HasAnnotation(ChoiceDispatch) ? EVVariable.Decode(value.Expression!.Single()) : null;
            SwitchTable? choiceTable = null;
            if (choice is { } cell)
            {
                choiceTable = ChoreographyChoiceTable.Infer(order, (StorageType)cell.Type);
                if (choiceTable == null)
                    throw Invalid(statement.Expression, ChoreographyChoiceTable.Diagnostic);
            }
            bool fallsThrough = false;
            HashSet<int>? fallThroughSections = null;
            HashSet<int>? transferExitSections = null;
            HashSet<int>? threadedExitSections = null;
            bool threadDefaultExit = false;
            bool hasUnreachablePrefix = false;
            int mergedCase = -1;
            Jsm.Opcode opcode;
            if (legacy != null)
            {
                opcode = SwitchOpcode(legacy.Int(0), statement.Expression);
                fallsThrough = legacy.Bool(2);
                order = legacy.OptionalIntegers(3) ?? order;
            }
            else if (FindSwitchLayout(statement, value, directDispatch) is { } layout &&
                (choice is not { Width: > 16 } || SwitchOpcode(AttributeValue<int>(layout, 1), statement.Expression) != Jsm.Opcode.JMP_SWITCHEX))
            {
                opcode = SwitchOpcode(AttributeValue<int>(layout, 1), statement.Expression);
                fallsThrough = AttributeValue<bool>(layout, 3);
                try
                {
                    order = AttributeValue<string>(layout, 2).Split(',').Select(n => int.Parse(n, System.Globalization.CultureInfo.InvariantCulture)).ToArray();
                }
                catch (Exception exception) when (exception is FormatException or OverflowException)
                {
                    throw Invalid(statement.Expression, "Invalid switch layout order", exception);
                }
                fallThroughSections = ReadSwitchExitSections(layout, nameof(SwitchLayoutAttribute.FallThroughSections), statement);
                transferExitSections = ReadSwitchExitSections(layout, nameof(SwitchLayoutAttribute.TransferExitSections), statement);
                threadedExitSections = ReadSwitchExitSections(layout, nameof(SwitchLayoutAttribute.ThreadedExitSections), statement);
                threadDefaultExit = layout.NamedArguments.Any(argument => argument.Key == nameof(SwitchLayoutAttribute.ThreadDefaultExit) && argument.Value.Value is true);
                hasUnreachablePrefix = layout.NamedArguments.Any(argument => argument.Key == nameof(SwitchLayoutAttribute.HasUnreachablePrefix) && argument.Value.Value is true);
                if (layout.NamedArguments.FirstOrDefault(argument => argument.Key == nameof(SwitchLayoutAttribute.MergedCaseSection)).Value.Value is int section)
                    mergedCase = section;
            }
            else if (choiceTable is { } table)
            {
                opcode = SwitchOpcode((int)table, statement.Expression);
                Array.Sort(order);
                if (opcode != Jsm.Opcode.JMP_SWITCHEX)
                {
                    int start = ChoreographyChoiceTable.DenseStart(order[0]);
                    order = Enumerable.Range(start, order[^1] - start + 1).ToArray();
                }
            }
            else
            {
                if (cases.Count == 0)
                    throw Invalid(statement.Expression, "A switch requires at least one integer case");
                Array.Sort(order);
                opcode = order.Length <= byte.MaxValue && order.SequenceEqual(Enumerable.Range(order[0], order.Length))
                    ? Jsm.Opcode.JMP_SWITCH : Jsm.Opcode.JMP_SWITCHEX;
            }
            bool explicitValues = opcode == Jsm.Opcode.JMP_SWITCHEX;
            var slots = order.ToHashSet();
            if (order.Length == 0 || slots.Count != order.Length || !slots.IsSupersetOf(cases.Keys) || (explicitValues && slots.Count != cases.Count))
                throw Invalid(statement.Expression, "The switch table order must contain each case exactly once");
            if (choice != null && order.Length > (opcode == Jsm.Opcode.rsv0d ? ChoreographyChoiceTable.MaxWideSpan : byte.MaxValue))
                throw Invalid(statement.Expression, ChoreographyChoiceTable.Diagnostic);
            if (explicitValues && order.Any(value => value < 0 || value > ushort.MaxValue))
                throw Invalid(statement.Expression, "Explicit switch values must fit an unsigned 16-bit integer");
            if (!explicitValues && (order[0] < short.MinValue || order[0] > short.MaxValue || !order.SequenceEqual(Enumerable.Range(order[0], order.Length))))
                throw Invalid(statement.Expression, "A dense switch table requires consecutive values starting with a 16-bit signed value");

            int? threadedExit = threadedExitSections != null || threadDefaultExit ? SwitchExitFollowing(statement) : null;
            if (threadDefaultExit && threadedExit is int outerTarget && fallback == exit)
                fallback = outerTarget;

            BlockSyntax? mergedCaseBody = null;
            int mergedCaseEntry = 0;
            if (guards.Count == 0 && mergedCase >= 0 && mergedCase < sections.Count &&
                TryGetMergedCase(statement, sections[mergedCase], value, out int[] mergedValues) is { } caseBody)
            {
                mergedCaseBody = caseBody;
                mergedCaseEntry = NewLabel();
                foreach (int mergedValue in mergedValues)
                    cases[mergedValue] = mergedCaseEntry;
            }

            Evaluate(value, statement.Expression);
            Add(new EVInstruction(0, opcode)
            {
                CaseValues = explicitValues ? order.Select(value => (ushort)value).ToArray() : new[] { unchecked((ushort)order[0]) },
                Targets = new[] { fallback }.Concat(order.Select(value => cases.GetValueOrDefault(value, fallback))).ToArray()
            }, statement);

            if (directDispatch)
            {
                Mark(exit);
                return;
            }

            _breakTargets.Push(exit);
            BlockSyntax? prefix = null;
            if (hasUnreachablePrefix && sections[0].Section.Statements.FirstOrDefault() is IfStatementSyntax { Statement: BlockSyntax block, Else: null } dead &&
                model.GetConstantValue(dead.Condition) is { HasValue: true, Value: false })
            {
                prefix = block;
                CompileBlock(prefix);
            }
            for (int index = 0; index < sections.Count; index++)
            {
                var (section, label) = sections[index];
                int firstStatement = index == 0 && prefix != null ? 1 : 0;
                if (index == mergedCase && mergedCaseBody != null)
                {
                    Mark(mergedCaseEntry);
                    CompileBlock(mergedCaseBody);
                    firstStatement = 1;
                }
                if (guardEntries.TryGetValue(label, out var sectionGuards))
                {
                    for (int guardIndex = 0; guardIndex < sectionGuards.Count; guardIndex++)
                    {
                        var guard = sectionGuards[guardIndex];
                        Mark(guard.Entry);
                        Condition(guard.Condition, guard.Fallback);
                        if (guardIndex + 1 < sectionGuards.Count)
                            Jump(Jsm.Opcode.JMP, label, guard.Condition);
                    }
                }
                Mark(label);
                bool omitBreak = index == sections.Count - 1 && fallsThrough && section.Statements.LastOrDefault() is BreakStatementSyntax;
                int count = section.Statements.Count - (omitBreak ? 1 : 0);
                int sectionStart = _instructions.Count;
                for (int step = firstStatement; step < count; step++)
                {
                    if (step == count - 1 && index + 1 < sections.Count && fallThroughSections?.Contains(index) == true && !guards.ContainsKey(sections[index + 1].Label) &&
                        section.Statements[step] is GotoStatementSyntax jump && jump.Kind() is SyntaxKind.GotoCaseStatement or SyntaxKind.GotoDefaultStatement &&
                        GotoTarget(jump) == sections[index + 1].Label)
                        break;
                    if (step == count - 1 && section.Statements[step] is BreakStatementSyntax)
                    {
                        if (section.HasAnnotation(OmitUnreachablePollingExit) && CanOmitPollingExit(sectionStart))
                            break;
                        if (transferExitSections?.Contains(index) == true && step > 0 && CanOmitTransferExit(section.Statements[step - 1]))
                            break;
                        if (threadedExit is int target && threadedExitSections?.Contains(index) == true)
                        {
                            Jump(Jsm.Opcode.JMP, target, section.Statements[step]);
                            break;
                        }
                    }
                    CompileStatement(section.Statements[step]);
                }
            }
            _breakTargets.Pop();
            Mark(exit);
        }

        private BlockSyntax? TryGetMergedCase(SwitchStatementSyntax statement, (SwitchSectionSyntax Syntax, int Label) section, EVOperand selector, out int[] values)
        {
            values = [];
            if (selector.Expression is not [var token] || !StorageDeclarations.IsStorage(token) || EVVariable.Decode(token).Scope == EVStorageScope.Special ||
                section.Syntax.Labels.Count < 2 || section.Syntax.Statements.FirstOrDefault() is not IfStatementSyntax { Statement: BlockSyntax block, Else: null } choice)
                return null;

            var cases = section.Syntax.Labels.OfType<CaseSwitchLabelSyntax>().Select(label => model.GetConstantValue(label.Value).Value).OfType<int>().ToHashSet();
            var matched = new HashSet<int>();
            if (!Matches(choice.Condition) || matched.Count == cases.Count && !section.Syntax.Labels.Any(label => label is DefaultSwitchLabelSyntax))
                return null;

            // A transfer into the group does not reevaluate the switch selector and must run its condition.
            foreach (GotoStatementSyntax jump in statement.DescendantNodes().OfType<GotoStatementSyntax>())
            {
                if (jump.Kind() is SyntaxKind.GotoCaseStatement or SyntaxKind.GotoDefaultStatement &&
                    model.GetOperation(jump) is IBranchOperation branch && _namedLabels.TryGetValue(branch.Target, out int target) && target == section.Label)
                    return null;
            }

            values = matched.ToArray();
            return block;

            bool Matches(ExpressionSyntax expression)
            {
                if (expression is ParenthesizedExpressionSyntax parenthesized)
                    return Matches(parenthesized.Expression);
                if (expression is not BinaryExpressionSyntax comparison)
                    return false;
                if (comparison.Kind() is SyntaxKind.LogicalOrExpression or SyntaxKind.BitwiseOrExpression)
                    return Matches(comparison.Left) && Matches(comparison.Right);
                return comparison.IsKind(SyntaxKind.EqualsExpression) && SyntaxFactory.AreEquivalent(statement.Expression, comparison.Left) &&
                    model.GetConstantValue(comparison.Right) is { HasValue: true, Value: int selected } && cases.Contains(selected) && matched.Add(selected);
            }
        }

        private int? SwitchExitFollowing(StatementSyntax statement)
        {
            if (statement.Parent is SwitchSectionSyntax section && section.Statements.Count >= 2 && section.Statements[^2].Span == statement.Span)
            {
                if (section.Statements[^1] is BreakStatementSyntax && _breakTargets.TryPeek(out int exit))
                    return exit;
                if (section.Statements[^1] is GotoStatementSyntax jump)
                    return GotoTarget(jump);
            }

            if (statement.Parent is BlockSyntax { Parent: MethodDeclarationSyntax } block && block.Statements[^1].Span == statement.Span)
                return _inlineSwitchExitTarget;

            return null;
        }

        private static HashSet<int>? ReadSwitchExitSections(AttributeData layout, string name, SwitchStatementSyntax statement)
        {
            if (layout.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value is not string { Length: > 0 } exits)
                return null;

            var sections = new HashSet<int>();
            foreach (string entry in exits.Split(','))
            {
                if (!int.TryParse(entry, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out int section) ||
                    section >= statement.Sections.Count || !sections.Add(section))
                    throw Invalid(statement.Expression, $"Invalid switch layout {name} section");
            }
            return sections;
        }

        private bool CanOmitTransferExit(StatementSyntax previous)
        {
            if (previous is not ExpressionStatementSyntax || _instructions.LastOrDefault() is not { Opcode: Jsm.Opcode.MAPJUMP } transfer)
                return false;
            SyntaxNode source = locations[transfer.Label];
            return source.SyntaxTree == previous.SyntaxTree && source.Span == previous.Span;
        }

        private bool CanOmitPollingExit(int sectionStart)
        {
            if (_instructions.Count == sectionStart || CanJumpTo(_instructions.Count))
                return false;

            EVInstruction tail = _instructions[^1];
            if (CutscenePhaseGraph.IsImmediateSelfDelete(tail))
                return true;

            return tail.Label > sectionStart && tail.Targets is [var target] && CutscenePhaseGraph.TakesLiteralBranch(_instructions[^2], tail) &&
                _labels.TryGetValue(target, out int repeat) && repeat >= sectionStart && repeat < tail.Label &&
                !CanJumpTo(tail.Label);

            // An explicit VM label can be referenced by a later section.
            bool CanJumpTo(int position) => _labels.Any(label => label.Key >= 0 && label.Value == position) ||
                _instructions.SelectMany(instruction => instruction.Targets).Any(target => _labels.GetValueOrDefault(target, -1) == position);
        }

        private AttributeData? FindSwitchLayout(SwitchStatementSyntax statement, EVOperand selector, bool directDispatch)
        {
            INamedTypeSymbol? layoutType = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.SwitchLayoutAttribute");
            if (layoutType == null || selector.Expression is not { Length: > 0 } tokens)
                return null;
            string? key = LayoutKey(statement, directDispatch);
            AttributeData[] hints = MatchingHints(handler);
            if (hints.Length == 0 && !SymbolEqualityComparer.Default.Equals(handler, _entryHandler))
            {
                AttributeData[] entryHints = MatchingHints(_entryHandler);
                return entryHints.Length == 1 ? entryHints[0] : null;
            }
            if (hints.Length <= 1)
                return hints.FirstOrDefault();

            // Ordered layouts belong to the occurrences of one switch shape in this method.
            // If an edit changes their count, let the current switches select their own encoding.
            MethodDeclarationSyntax? method = statement.FirstAncestorOrSelf<MethodDeclarationSyntax>();
            SwitchStatementSyntax[] occurrences = method?.Body?.DescendantNodes().OfType<SwitchStatementSyntax>()
                .Where(candidate => candidate.Expression.IsEquivalentTo(statement.Expression) && LayoutKey(candidate, IsDirectDispatch(candidate)) == key).ToArray() ?? [];
            int ordinal = Array.FindIndex(occurrences, candidate => candidate.Span == statement.Span);
            return ordinal >= 0 && occurrences.Length == hints.Length ? hints[ordinal] : null;

            AttributeData[] MatchingHints(IMethodSymbol method) => method.GetAttributes().Where(attribute =>
                SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, layoutType) && AttributeValue<string>(attribute, 0) == key).ToArray();

            string? LayoutKey(SwitchStatementSyntax selection, bool dispatch)
            {
                var sections = new List<string>();
                foreach (SwitchSectionSyntax section in selection.Sections)
                {
                    var values = new List<int>();
                    foreach (SwitchLabelSyntax label in section.Labels)
                    {
                        if (label is DefaultSwitchLabelSyntax)
                            continue;
                        if (label is CasePatternSwitchLabelSyntax { WhenClause: not null } pattern && TryReadGuardedCaseValues(pattern.Pattern, out int[] guardedValues))
                        {
                            values.AddRange(guardedValues);
                            continue;
                        }
                        if (label is not CaseSwitchLabelSyntax simple || model.GetConstantValue(simple.Value) is not { HasValue: true, Value: int value })
                            return null;
                        values.Add(value);
                    }
                    sections.Add(SwitchLayout.Section(values, section.Labels.Any(label => label is DefaultSwitchLabelSyntax),
                        section.Statements.LastOrDefault() is ReturnStatementSyntax));
                }
                return SwitchLayout.Key(tokens, sections, dispatch);
            }
        }

        private bool TryReadGuardedCaseValues(PatternSyntax pattern, out int[] values)
        {
            var constants = new List<int>();
            bool valid = Collect(pattern);
            values = constants.ToArray();
            return valid && values.Distinct().Count() == values.Length;

            bool Collect(PatternSyntax candidate)
            {
                if (candidate is ParenthesizedPatternSyntax parenthesized)
                    return Collect(parenthesized.Pattern);
                if (candidate is BinaryPatternSyntax either && either.IsKind(SyntaxKind.OrPattern))
                    return Collect(either.Left) && Collect(either.Right);
                if (candidate is not ConstantPatternSyntax constant || model.GetConstantValue(constant.Expression) is not { HasValue: true, Value: int value })
                    return false;
                constants.Add(value);
                return true;
            }
        }

        private static bool IsDirectDispatch(SwitchStatementSyntax statement) => statement.Sections.Count > 0 && statement.Sections.All(section =>
            !section.Labels.Any(label => label is CasePatternSwitchLabelSyntax) && section.Statements.Count == 1 &&
            section.Statements[0] is GotoStatementSyntax jump && jump.IsKind(SyntaxKind.GotoStatement));

        private static Jsm.Opcode SwitchOpcode(int table, SyntaxNode source) => table switch
        {
            0 => Jsm.Opcode.JMP_SWITCH,
            1 => Jsm.Opcode.JMP_SWITCHEX,
            2 => Jsm.Opcode.rsv0d,
            _ => throw Invalid(source, "Unknown switch table encoding")
        };

        private void Evaluate(EVOperand value, SyntaxNode source) =>
            Add(new EVInstruction(0, Jsm.Opcode.EXPR) { Expression = AsExpression(value) }, source);

        private void Jump(Jsm.Opcode opcode, int target, SyntaxNode source) =>
            Add(new EVInstruction(0, opcode) { Targets = new[] { target } }, source);

        private int GotoTarget(GotoStatementSyntax jump)
        {
            if (model.GetOperation(jump) is not IBranchOperation { BranchKind: BranchKind.GoTo } branch)
                throw Invalid(jump, "Expected a bound goto label");
            return NamedLabel(branch.Target);
        }

        private int NamedLabel(ILabelSymbol symbol)
        {
            if (!_namedLabels.TryGetValue(symbol, out int label))
            {
                label = NewLabel();
                _namedLabels.Add(symbol, label);
            }
            return label;
        }

        private int NewLabel() => _nextLabel--;

        private void Mark(int label) => _labels.Add(label, _instructions.Count);

        private void Add(EVInstruction instruction, SyntaxNode source)
        {
            instruction.Label = _instructions.Count;
            instruction.WatchedObjectIp = _expectedIp;
            _expectedIp = null;
            locations.Add(instruction.Label, source);
            _instructions.Add(instruction);
        }
    }

    private static EVInstruction? TryCompileService(IInvocationOperation invocation, INamedTypeSymbol vm)
    {
        if (IsSupportType(invocation.TargetMethod.ContainingType, vm, "ActorAnimation") &&
            vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.CharacterEffectAttribute") is { } effectAttribute &&
            FindAttribute(invocation.TargetMethod, effectAttribute) is { } effect)
            return CompileCharacterEffect(invocation, vm, AttributeValue<byte>(effect, 0));

        INamedTypeSymbol? sceneAttribute = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.SceneInstructionAttribute");
        if (sceneAttribute != null &&
            SymbolEqualityComparer.Default.Equals(invocation.TargetMethod.ContainingAssembly, vm.ContainingAssembly) &&
            FindAttribute(invocation.TargetMethod, sceneAttribute) is AttributeData sceneMapping)
            return CompileSceneInstruction(invocation, vm, (Jsm.Opcode)AttributeValue<int>(sceneMapping, 0));

        INamedTypeSymbol? attribute = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.InstructionAttribute");
        if (attribute == null || FindAttribute(invocation.TargetMethod, attribute) is not AttributeData mapping)
            return null;
        if ((IsSupportType(invocation.TargetMethod.ContainingType, vm, "ActorAnimation") ||
             IsSupportType(invocation.TargetMethod.ContainingType, vm, "ActorActions")) &&
            (invocation.Instance is not IPropertyReferenceOperation { Instance: { } actor } || !IsActingActor(actor, invocation, vm)))
            throw Invalid(invocation.Syntax, "This operation requires the acting actor");
        var call = new Intrinsic(invocation, vm, service: true);
        var opcode = (Jsm.Opcode)AttributeValue<int>(mapping, 0);
        int fixedIndex = AttributeValue<int>(mapping, 1);
        int fixedValue = AttributeValue<int>(mapping, 2);
        if (opcode == Jsm.Opcode.QUAD && fixedIndex == -1 && call.ArgumentCount == 3)
        {
            EVOperand[] vertices = new[] { call.Operand(0) }.Concat(call.Array(2).SelectMany(call.Point)).ToArray();
            return MakeInstruction(0, opcode, call.Byte(1), vertices);
        }
        string layout = EVOperands.For(opcode);
        bool hasMask = layout.StartsWith('m');
        int operandCount = layout.Length - (hasMask ? 1 : 0);
        if (fixedIndex < -1 || fixedIndex >= operandCount ||
            call.ArgumentCount != layout.Length - (fixedIndex < 0 ? 0 : 1))
            throw Invalid(invocation.Syntax, "Instruction declaration does not match the opcode's operands");

        var values = new EVOperand[operandCount];
        int parameter = 0;
        for (int index = 0; index < operandCount; index++)
        {
            if (index == fixedIndex)
                values[index] = new EVOperand(fixedValue);
            else
                values[index] = call.Operand(parameter++);
        }
        int mask = hasMask ? call.Int(call.ArgumentCount - 1) : 0;
        byte encodedMask = mask == -1 ? SourceInstruction.InferMask(opcode, values) : checked((byte)mask);
        return MakeInstruction(0, opcode, encodedMask, values);
    }

    private static EVInstruction CompileCharacterEffect(IInvocationOperation invocation, INamedTypeSymbol vm, byte command)
    {
        if (invocation.Instance is not IPropertyReferenceOperation animation ||
            !(animation.Instance == null && IsSupportType(animation.Property.ContainingType, vm, "CharacterEffects") ||
              animation.Instance != null && IsActingActor(animation.Instance, invocation, vm)))
            throw Invalid(invocation.Syntax, "Character effects require the acting actor");

        var call = new Intrinsic(invocation, vm, service: true);
        var operands = new EVOperand[4];
        operands[0] = new EVOperand(command);
        int semanticCount = call.ArgumentCount - 1;
        for (int index = 0; index < semanticCount; index++)
        {
            IOperation argument = call.Argument(index);
            string domain = invocation.TargetMethod.Parameters[index].Type.Name;
            operands[index + 1] = domain is "AnimationId" or "SoundEffectId" or "ScriptFlag"
                ? ReadCharacterEffectValue(argument, domain, vm)
                : ReadDomainOperand(argument, vm);
        }
        for (int index = semanticCount + 1; index < operands.Length; index++)
            operands[index] = new EVOperand(0);

        int mask = -1;
        IOperation encoding = call.Argument(semanticCount);
        if (encoding is IObjectCreationOperation { Arguments.Length: > 0 } descriptor && IsSupportType(descriptor.Type!, vm, "CharacterEffectEncoding"))
        {
            foreach (IArgumentOperation parameter in descriptor.Arguments)
            {
                int ordinal = parameter.Parameter!.Ordinal;
                if (ordinal == 2)
                    mask = ConstantInt(parameter.Value);
                else if (ordinal + semanticCount + 1 < operands.Length)
                    operands[ordinal + semanticCount + 1] = ReadDomainOperand(parameter.Value, vm);
                else if (!parameter.IsImplicit)
                    throw Invalid(parameter.Syntax, "This character effect has no unused operand at that position");
            }
        }
        else if (encoding is not (IDefaultValueOperation or IObjectCreationOperation { Arguments.Length: 0 }))
        {
            throw Invalid(encoding.Syntax, "Use a character effect encoding declaration");
        }

        byte inferred = SourceInstruction.InferMask(Jsm.Opcode.CHRFX, operands);
        byte actual = mask == -1 ? inferred : (byte)((checked((byte)mask) & 0xF0) | inferred);
        return MakeInstruction(0, Jsm.Opcode.CHRFX, actual, operands);
    }

    private static EVOperand ReadCharacterEffectValue(IOperation value, string domain, INamedTypeSymbol vm)
    {
        if (value is IConversionOperation flag && IsSupportType(flag.Type!, vm, "ScriptFlag") && flag.Operand.Type?.SpecialType == SpecialType.System_Boolean)
            return ReadDomainOperand(flag.Operand, vm);
        if (value is IInvocationOperation { Arguments.Length: 1 } conversion && conversion.TargetMethod.Name == "FromScript" &&
            IsSupportType(conversion.TargetMethod.ContainingType, vm, domain))
            return ReadDomainOperand(conversion.Arguments[0].Value, vm);
        if (value is IObjectCreationOperation { Arguments.Length: 1 } resource && IsSupportType(resource.Type!, vm, domain))
            return new EVOperand(ConstantInt(resource.Arguments[0].Value));
        if (value is IPropertyReferenceOperation property && SymbolEqualityComparer.Default.Equals(property.Property.ContainingAssembly, vm.ContainingAssembly) &&
            vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.ResourceValueAttribute") is { } attribute &&
            FindAttribute(property.Property, attribute) is { } id)
            return new EVOperand(AttributeValue<ushort>(id, 0));
        throw Invalid(value.Syntax, $"Use a declared {domain} or its explicit script conversion");
    }

    private static bool IsCurrentParticipant(IOperation value) =>
        SceneReferenceValue(value) is IInstanceReferenceOperation or IParameterReferenceOperation;

    private static bool IsSceneContextAlias(IPropertyReferenceOperation member, IMethodSymbol caller, INamedTypeSymbol vm)
    {
        if (!IsSupportType(member.Property.Type, vm, "ScriptService") && !IsSupportType(member.Property.Type, vm, "GlobalState") &&
            !HasSupportAttribute(member.Property.Type, vm, "SceneRootAttribute"))
            return false;

        // An alias emits no VM instruction. Its initializer must therefore be a pure access to this context.
        if (member.Instance is IInstanceReferenceOperation && !member.Property.IsVirtual &&
            SymbolEqualityComparer.Default.Equals(member.Property.ContainingType, caller.ContainingType) &&
            member.Property.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is PropertyDeclarationSyntax { ExpressionBody.Expression: { } forwarding } &&
            member.SemanticModel!.Compilation.GetSemanticModel(forwarding.SyntaxTree).GetOperation(forwarding) is IPropertyReferenceOperation forwarded)
            member = forwarded;

        if (!IsContextReference(member.Instance, caller, vm))
            return false;
        if (IsSupportType(member.Property.ContainingType, vm, "EventSceneContext"))
            return member.Property.Name is "Scripts" or "Globals";
        return IsGetOnlyAutoProperty(member.Property) && HasSupportAttribute(member.Property.Type, vm, "SceneRootAttribute") &&
            SymbolEqualityComparer.Default.Equals(member.Property.ContainingType, ReadEventContext(member.SemanticModel!.Compilation, vm));
    }

    private static bool IsBoundCutsceneReceiver(IOperation? operation, INamedTypeSymbol target, IMethodSymbol caller, INamedTypeSymbol vm, Compilation compilation)
    {
        if (operation is IInstanceReferenceOperation)
            return SymbolEqualityComparer.Default.Equals(caller.ContainingType, target);
        return operation is IPropertyReferenceOperation binding && GetCutsceneBindings(compilation, vm).TryGetValue(target, out IPropertySymbol? property) &&
            SymbolEqualityComparer.Default.Equals(binding.Property, property) &&
            IsContextReference(binding.Instance, caller, vm);
    }

    private static bool IsContextReference(IOperation? operation, ISymbol caller, INamedTypeSymbol vm)
    {
        operation = ContextOperand(operation);
        if (operation is IParameterReferenceOperation parameter && parameter.Parameter.ContainingSymbol is IMethodSymbol constructor)
            return constructor.MethodKind == MethodKind.Constructor && SymbolEqualityComparer.Default.Equals(constructor.ContainingType, caller.ContainingType) &&
                DerivesFrom(constructor.ContainingType, vm, "EventCutscene") && DerivesFrom(parameter.Parameter.Type, vm, "EventSceneContext");
        if (operation is not IPropertyReferenceOperation { Instance: IInstanceReferenceOperation } property)
            return false;
        if (property.Property.Name == "Context" && IsSupportType(property.Property.ContainingType, vm, "EventParticipant"))
            return true;
        if (!SymbolEqualityComparer.Default.Equals(property.Property.ContainingType, caller.ContainingType) || property.Property.IsVirtual ||
            property.Property.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not PropertyDeclarationSyntax { ExpressionBody.Expression: { } expression })
            return false;
        IOperation? value = ContextOperand(operation.SemanticModel!.Compilation.GetSemanticModel(expression.SyntaxTree).GetOperation(expression));
        return value is IPropertyReferenceOperation { Property.Name: "Context", Instance: IInstanceReferenceOperation } context &&
            IsSupportType(context.Property.ContainingType, vm, "EventParticipant");

        IOperation? ContextOperand(IOperation? value)
        {
            while (value is IConversionOperation cast)
            {
                // The registered context is the value that each accepted built-in conversion must preserve.
                if (cast.OperatorMethod != null || cast.Type == null || cast.SemanticModel?.Compilation is not CSharpCompilation compilation ||
                    ReadEventContext(compilation, vm) is not { } contextType)
                    return null;
                if (!IsSafeReferenceConversion(compilation, contextType, cast.Type))
                    return null;
                value = cast.Operand;
            }
            return value;
        }
    }

    private static bool IsSafeReferenceConversion(CSharpCompilation compilation, ITypeSymbol source, ITypeSymbol target)
    {
        Conversion conversion = compilation.ClassifyConversion(source, target);
        return conversion.IsIdentity || conversion.IsImplicit && conversion.IsReference;
    }

    private static ITypeSymbol ExecutionOwner(IMethodSymbol method, INamedTypeSymbol vm) =>
        DerivesFrom(method.ContainingType, vm, "EventCutscene") && method.Parameters is [var participant]
            ? participant.Type : method.ContainingType;

    private static bool IsActingActor(IOperation receiver, IInvocationOperation invocation, INamedTypeSymbol vm) =>
        IsCurrentParticipant(receiver) &&
        ReadSceneParticipant(receiver, vm, invocation.SemanticModel!.Compilation) is { Kind: 2 } actor &&
        invocation.SemanticModel.GetEnclosingSymbol(invocation.Syntax.SpanStart) is IMethodSymbol method &&
        SymbolEqualityComparer.Default.Equals(actor.Declaration, ExecutionOwner(method, vm));

    private static EVOperand ReadDomainOperand(IOperation value, INamedTypeSymbol vm)
    {
        if (value.ConstantValue is { HasValue: true, Value: bool flag })
            return new EVOperand(flag ? 1 : 0);
        if (value.ConstantValue.HasValue)
            return new EVOperand(ConstantInt(value));
        return new EVOperand(ReadNative(value, vm, value.Syntax));
    }

    private static EVInstruction CompileSceneInstruction(IInvocationOperation invocation, INamedTypeSymbol vm, Jsm.Opcode opcode)
    {
        var call = new Intrinsic(invocation, vm, service: true);
        Compilation compilation = invocation.SemanticModel!.Compilation;
        if (opcode == Jsm.Opcode.MES && IsSupportType(invocation.TargetMethod.ContainingType, vm, "Dialogue"))
        {
            if (call.Argument(0) is not IObjectCreationOperation { Arguments.Length: 3 } line || !IsSupportType(line.Type!, vm, "DialogueLine"))
                throw Invalid(invocation.Syntax, "Use a DialogueLine with an explicit window, flags, and message");
            var sourceArguments = line.Arguments.OrderBy(argument => argument.Syntax.SpanStart).ToArray();
            var encoded = sourceArguments.OrderBy(argument => argument.Parameter!.Ordinal).ToArray();
            if (!sourceArguments.SequenceEqual(encoded) && sourceArguments.Any(argument => !IsConstantDialogueOperand(argument.Value, vm)))
                throw Invalid(line.Syntax, "List computed DialogueLine operands in window, flags, and message order to preserve evaluation order");
            EVOperand[] operands = encoded.Select(argument => Intrinsic.ReadOperand(argument.Value, vm)).ToArray();
            int mask = call.Int(1);
            return MakeInstruction(0, opcode, mask == -1 ? SourceInstruction.InferMask(opcode, operands) : checked((byte)mask), operands);
        }
        if (opcode == Jsm.Opcode.AIDLE)
        {
            if (invocation.Instance is not IPropertyReferenceOperation { Property.Name: "Animation", Instance: { } receiver } ||
                !(IsActingActor(receiver, invocation, vm) ||
                  receiver.Type is { } type && IsSupportType(type, vm, "ParticipantDefinition") &&
                  ReadSceneParticipant(receiver, vm, compilation) is { Kind: 2 } legacy &&
                  SymbolEqualityComparer.Default.Equals(legacy.Declaration, invocation.SemanticModel.GetEnclosingSymbol(invocation.Syntax.SpanStart)?.ContainingType)))
                throw Invalid(invocation.Syntax, "The animation receiver must be the acting actor in this handler");
            EVOperand[] arguments = [call.Operand(0)];
            int originalMask = call.Int(1);
            byte maskByte = originalMask == -1 ? SourceInstruction.InferMask(opcode, arguments) : checked((byte)originalMask);
            return MakeInstruction(0, opcode, maskByte, arguments);
        }

        IOperation definition = SceneReferenceValue(call.Argument(0));
        bool isCutsceneAction = IsSupportType(invocation.TargetMethod.ContainingType, vm, "ScriptService") &&
            definition.Type is { } actionType && IsSupportType(actionType, vm, "CutsceneAction");
        if (!isCutsceneAction && opcode is (Jsm.Opcode.NEW or Jsm.Opcode.NEW2 or Jsm.Opcode.NEW3 or Jsm.Opcode.STARTSEQ))
        {
            if (ReadSceneParticipant(call.Argument(0), vm, compilation) is not { } participant)
                throw Invalid(invocation.Syntax, "The start target must refer to a participant declaration in this event");

            int slot = checked((byte)AttributeValue<int>(participant.Slot, 0));
            if (opcode == Jsm.Opcode.STARTSEQ)
            {
                EVOperand[] arguments = [new EVOperand(slot)];
                int encodedMask = call.Int(1);
                return MakeInstruction(0, opcode, encodedMask == -1 ? SourceInstruction.InferMask(opcode, arguments) : checked((byte)encodedMask), arguments);
            }
            int kind = opcode switch
            {
                Jsm.Opcode.NEW => 0,
                Jsm.Opcode.NEW2 => 1,
                _ => 2
            };
            if (AttributeValue<byte>(participant.Slot, 6) != kind || participant.Kind != kind)
                throw Invalid(invocation.Syntax, $"Cannot start participant '{participant.Declaration.Name}' as {invocation.TargetMethod.Name}");

            IOperation uidArgument = SceneReferenceValue(call.Argument(1));
            int uid = uidArgument switch
            {
                IDefaultValueOperation => 0,
                IObjectCreationOperation { Arguments.Length: 0 } => 0,
                IObjectCreationOperation { Arguments.Length: 1 } explicitUid => ConstantInt(explicitUid.Arguments[0].Value),
                _ => throw Invalid(uidArgument.Syntax, "Use a constant instance UID or omit it to use the participant's default UID")
            };
            return MakeInstruction(0, opcode, 0, new EVOperand(slot), new EVOperand(checked((byte)uid)));
        }

        if (isCutsceneAction)
        {
            if (definition is not IInvocationOperation binding || binding.TargetMethod.Name != "On" ||
                !IsSupportType(binding.TargetMethod.ContainingType, vm, "EventCutscene"))
                throw Invalid(invocation.Syntax, "Create the action with On(receiver, methodGroup)");
            var action = new Intrinsic(binding, vm, service: true);
            IOperation receiver = action.Argument(0);
            if (invocation.SemanticModel.GetEnclosingSymbol(invocation.Syntax.SpanStart) is not IMethodSymbol caller ||
                !IsBoundScriptService(invocation.Instance, caller, vm) ||
                SceneReferenceValue(action.Argument(1)) is not IDelegateCreationOperation { Target: IMethodReferenceOperation entry } ||
                !DerivesFrom(entry.Method.ContainingType, vm, "EventCutscene") ||
                !IsBoundCutsceneReceiver(entry.Instance, entry.Method.ContainingType, caller, vm, compilation) ||
                FindAttribute(entry.Method, RequireType(compilation, "ScriptAttribute")) is not { } scriptEntry ||
                !IsBoundActionParticipant(receiver, caller, vm) ||
                ReadSceneParticipant(receiver, vm, compilation) is not { } participant ||
                entry.Method.Parameters is not [var parameter] || !SymbolEqualityComparer.Default.Equals(parameter.Type, participant.Declaration))
                throw Invalid(invocation.Syntax, "The deferred cutscene action must be bound to a script entry for its receiver");
            int slot = AttributeValue<int>(participant.Slot, 0);
            int actionTag = AttributeValue<ushort>(scriptEntry, 0);
            int uid = IsControlledActor(receiver, vm) ? 250 : IsCurrentParticipant(receiver) ? 255 : slot;
            if (opcode == Jsm.Opcode.STARTSEQ)
            {
                if (actionTag != 0 || action.ArgumentCount != 2 || IsControlledActor(receiver, vm) || IsCurrentParticipant(receiver))
                    throw Invalid(binding.Syntax, "A shared script requires the initial entry of a participant definition");
                EVOperand[] operands = [new EVOperand(slot)];
                int mask = call.Int(1);
                return MakeInstruction(0, opcode, mask == -1 ? SourceInstruction.InferMask(opcode, operands) : checked((byte)mask), operands);
            }
            if (action.ArgumentCount == 3)
                uid = ReadExplicitInstance(action.Argument(2), uid);
            return EncodeRequest(call.DomainOperand(1), uid, checked((byte)actionTag), call.Int(2));
        }

        if (IsSupportType(invocation.TargetMethod.ContainingType, vm, "ScriptService"))
        {
            if (definition is not IDelegateCreationOperation { Target: IMethodReferenceOperation bound } ||
                bound.Instance == null || !SymbolEqualityComparer.Default.Equals(bound.Method.ContainingAssembly, compilation.Assembly))
                throw Invalid(invocation.Syntax, "Pass a declared scene handler as a method group");
            int receiver;
            int handlerId;
            if (IsControlledActor(bound.Instance, vm) &&
                FindAttribute(bound.Method, RequireType(compilation, "ControlledHandlerAttribute")) is { } controlled)
            {
                receiver = 250;
                handlerId = AttributeValue<byte>(controlled, 0);
            }
            else if (FindAttribute(bound.Method, RequireType(compilation, "ScriptAttribute")) is { } entry &&
                ReadSceneParticipant(bound.Instance, vm, compilation) is { } fixedOwner &&
                SymbolEqualityComparer.Default.Equals(bound.Method.ContainingType, fixedOwner.Declaration))
            {
                receiver = IsControlledActor(bound.Instance, vm) ? 250 :
                    SceneReferenceValue(bound.Instance) is IInstanceReferenceOperation ? 255 : AttributeValue<int>(fixedOwner.Slot, 0);
                handlerId = AttributeValue<ushort>(entry, 0);
            }
            else
            {
                throw Invalid(bound.Syntax, $"'{bound.Method.ToDisplayString()}' is not a declared handler on this receiver");
            }
            if (handlerId > byte.MaxValue)
                throw Invalid(bound.Syntax, $"Handler tag {handlerId} exceeds the request operand range");
            if (call.ArgumentCount == 4)
                receiver = ReadExplicitInstance(call.Argument(2), receiver);
            return EncodeRequest(call.DomainOperand(1), receiver, handlerId, call.Int(call.ArgumentCount - 1));
        }

        if (definition is not IObjectCreationOperation { Arguments.Length: 2 } reference ||
            !IsSupportType(reference.Type!, vm, "HandlerReference") ||
            SceneReferenceValue(reference.Arguments.Single(argument => argument.Parameter!.Ordinal == 1).Value) is not IDelegateCreationOperation { Target: IMethodReferenceOperation method } ||
            !SymbolEqualityComparer.Default.Equals(method.Method.ContainingAssembly, compilation.Assembly) ||
            FindAttribute(method.Method, RequireType(compilation, "ScriptAttribute")) is not AttributeData script ||
            FindAttribute(method.Method.ContainingType, RequireType(compilation, "ObjectSlotAttribute")) is not AttributeData owner)
            throw Invalid(invocation.Syntax, "The request target must refer to a declared handler in this event");

        int selector = ConstantInt(reference.Arguments.Single(argument => argument.Parameter!.Ordinal == 0).Value);
        if (selector is < 250 or > 255)
            throw Invalid(invocation.Syntax, "The request requires a defined actor selector");
        if (selector == 255 &&
            !SymbolEqualityComparer.Default.Equals(method.Method.ContainingType, invocation.SemanticModel.GetEnclosingSymbol(invocation.Syntax.SpanStart)?.ContainingType))
            throw Invalid(invocation.Syntax, $"'{method.Method.ToDisplayString()}' is not a handler of the current participant");
        if (selector == 250 &&
            FindAttribute(compilation.Assembly, RequireType(compilation, "ControlledActorContractAttribute")) is AttributeData contract &&
            AttributeValue<int>(owner, 0) != AttributeValue<int>(contract, 0))
            throw Invalid(invocation.Syntax, $"'{method.Method.ToDisplayString()}' does not implement the controlled actor contract for slot {AttributeValue<int>(contract, 0)}");
        int tag = AttributeValue<ushort>(script, 0);
        if (tag > byte.MaxValue)
            throw Invalid(invocation.Syntax, $"Handler '{method.Method.ToDisplayString()}' has tag {tag}, outside the request operand range");
        return EncodeRequest(call.Operand(1), selector, tag, call.Int(2));

        int ReadExplicitInstance(IOperation argument, int receiver)
        {
            if (receiver >= 250 || SceneReferenceValue(argument) is not IObjectCreationOperation { Arguments: [var uid] } instance ||
                !IsSupportType(instance.Type!, vm, "InstanceUid") || ConstantInt(uid.Value) is not (> 0 and < 250))
                throw Invalid(argument.Syntax, "Use a concrete live instance UID for a declared participant handler");
            return ConstantInt(uid.Value);
        }

        EVInstruction EncodeRequest(EVOperand level, int target, int handler, int mask)
        {
            EVOperand[] operands = [level, new EVOperand(target), new EVOperand(handler)];
            return MakeInstruction(0, opcode, mask == -1 ? SourceInstruction.InferMask(opcode, operands) : checked((byte)mask), operands);
        }
    }

    private static bool IsConstantDialogueOperand(IOperation value, INamedTypeSymbol vm) => value.ConstantValue.HasValue ||
        value is IConversionOperation { OperatorMethod: { } conversion, Operand.ConstantValue.HasValue: true } && IsSupportType(conversion.ContainingType, vm, "Operand");

    private static (ITypeSymbol Declaration, AttributeData Slot, int Kind)? ReadSceneParticipant(IOperation value, INamedTypeSymbol vm, Compilation compilation)
    {
        IOperation source = SceneReferenceValue(value);
        INamedTypeSymbol objectSlot = RequireType(compilation, "ObjectSlotAttribute");
        if (source.Type is { } receiverType && DerivesFrom(receiverType, vm, "EventParticipant") &&
            (source is IInstanceReferenceOperation or IParameterReferenceOperation ||
                source is IPropertyReferenceOperation property && HasSupportAttribute(property.Property.ContainingType, vm, "SceneRootAttribute")) &&
            FindAttribute(receiverType, objectSlot) is { } receiverSlot)
            return (receiverType, receiverSlot, AttributeValue<byte>(receiverSlot, 6));

        if (source is not IObjectCreationOperation { Arguments.Length: 2 } participant ||
            !IsSupportType(participant.Type!, vm, "ParticipantDefinition") ||
            SceneReferenceValue(participant.Arguments.Single(argument => argument.Parameter!.Ordinal == 0).Value) is not ITypeOfOperation declaration ||
            FindAttribute(declaration.TypeOperand, objectSlot) is not AttributeData slot ||
            !SymbolEqualityComparer.Default.Equals(declaration.TypeOperand.ContainingAssembly, compilation.Assembly))
            return null;
        int kind = ConstantInt(participant.Arguments.Single(argument => argument.Parameter!.Ordinal == 1).Value);
        return (declaration.TypeOperand, slot, kind);
    }

    private static bool IsBoundScriptService(IOperation? operation, IMethodSymbol caller, INamedTypeSymbol vm) =>
        operation != null && SceneReferenceValue(operation) is IPropertyReferenceOperation { Property.Name: "Scripts" } service &&
        IsSupportType(service.Property.ContainingType, vm, "EventSceneContext") && IsContextReference(service.Instance, caller, vm);

    private static bool IsBoundActionParticipant(IOperation operation, IMethodSymbol caller, INamedTypeSymbol vm)
    {
        IOperation receiver = SceneReferenceValue(operation);
        if (receiver is IInstanceReferenceOperation || receiver is IParameterReferenceOperation parameter &&
            SymbolEqualityComparer.Default.Equals(parameter.Parameter.ContainingSymbol, caller))
            return true;
        if (receiver is not IPropertyReferenceOperation participant || !IsGetOnlyAutoProperty(participant.Property) ||
            !HasSupportAttribute(participant.Property.ContainingType, vm, "SceneRootAttribute") || participant.Instance == null ||
            SceneReferenceValue(participant.Instance) is not IPropertyReferenceOperation root || !IsBoundSceneRoot(root, caller, vm))
            return false;
        Compilation compilation = receiver.SemanticModel!.Compilation;
        INamedTypeSymbol? context = ReadEventContext(compilation, vm);
        if (participant.Property.DeclaringSyntaxReferences.Single().GetSyntax() is not PropertyDeclarationSyntax { Initializer.Value: { } initializer })
            return false;

        IOperation? binding = compilation.GetSemanticModel(initializer.SyntaxTree).GetOperation(initializer);
        if (binding is IInvocationOperation { TargetMethod.Name: "BindControlled", Arguments: [var actor] } controlled &&
            IsSupportType(controlled.TargetMethod.ContainingType, vm, "EventSceneContext"))
            binding = actor.Value;
        return binding is IObjectCreationOperation { Arguments: [var argument] } creation &&
            SymbolEqualityComparer.Default.Equals(creation.Type, participant.Property.Type) &&
            argument.Value is IParameterReferenceOperation contextParameter && SymbolEqualityComparer.Default.Equals(contextParameter.Parameter.Type, context);
    }

    private static bool IsBoundSceneRoot(IPropertyReferenceOperation root, IMethodSymbol caller, INamedTypeSymbol vm)
    {
        Compilation compilation = root.SemanticModel!.Compilation;
        return IsGetOnlyAutoProperty(root.Property) && root.Property.Type is INamedTypeSymbol rootType &&
            HasSupportAttribute(rootType, vm, "SceneRootAttribute") && IsContextReference(root.Instance, caller, vm) &&
            ReadEventContext(compilation, vm) is { } context && SymbolEqualityComparer.Default.Equals(root.Property.ContainingType, context) &&
            context.InstanceConstructors.FirstOrDefault(constructor => constructor.Parameters.Length == 0)?.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax()
                is ConstructorDeclarationSyntax { Body: { } body } && IsConstructedFromContext(compilation, body, root.Property, rootType);
    }

    private static IOperation SceneReferenceValue(IOperation operation)
    {
        while (operation is IConversionOperation conversion)
            operation = conversion.Operand;
        if (operation is IFieldReferenceOperation { Field.IsStatic: true, Field.IsReadOnly: true } field &&
            field.Field.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is VariableDeclaratorSyntax { Initializer.Value: ExpressionSyntax initializer })
            return operation.SemanticModel!.Compilation.GetSemanticModel(initializer.SyntaxTree).GetOperation(initializer)!;
        if (operation is ILocalReferenceOperation local &&
            local.Local.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is VariableDeclaratorSyntax { Initializer.Value: ExpressionSyntax value })
            return SceneReferenceValue(operation.SemanticModel!.GetOperation(value)!);
        return operation;
    }

    private static bool IsControlledActor(IOperation operation, INamedTypeSymbol vm) =>
        SceneReferenceValue(operation) is IPropertyReferenceOperation role && IsControlledActorBinding(role.Property, vm, role.SemanticModel!.Compilation);

    private static bool IsControlledActorBinding(IPropertySymbol property, INamedTypeSymbol vm, Compilation compilation)
    {
        if (HasSupportAttribute(property, vm, "ControlledActorAttribute"))
            return true;
        return property.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is PropertyDeclarationSyntax { Initializer.Value: { } initializer } &&
            compilation.GetSemanticModel(initializer.SyntaxTree).GetOperation(initializer) switch
            {
                IInvocationOperation binding => binding.TargetMethod.Name == "BindControlled" && IsSupportType(binding.TargetMethod.ContainingType, vm, "EventSceneContext"),
                IObjectCreationOperation proxy => IsSupportType(proxy.Type!, vm, "ControlledActorTarget"),
                _ => false
            };
    }

    private static bool HasSupportAttribute(ISymbol symbol, INamedTypeSymbol vm, string name) =>
        vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.{name}") is { } attribute && FindAttribute(symbol, attribute) != null;

    private static bool DerivesFrom(ITypeSymbol type, INamedTypeSymbol vm, string name)
    {
        for (ITypeSymbol? candidate = type; candidate != null; candidate = candidate.BaseType)
        {
            if (IsSupportType(candidate, vm, name))
                return true;
        }
        return false;
    }

    private static bool IsExplicitLayout(ITypeSymbol type, Compilation compilation) =>
        compilation.GetTypeByMetadataName("System.Runtime.InteropServices.StructLayoutAttribute") is { } attribute &&
        FindAttribute(type, attribute) is { } layout && AttributeValue<int>(layout, 0) == 2;

    private static EVInstruction CompileInstruction(Intrinsic call, int label)
    {
        switch (call.Name)
        {
            case "Emit":
                return MakeInstruction(label, call.Opcode(0), call.Byte(1), call.Operands(2));
            case "Evaluate":
                return new EVInstruction(label, Jsm.Opcode.EXPR)
                {
                    Expression = call.IsTokenList(0) ? call.Tokens(0) : AsExpression(call.Operand(0))
                };
            case "Branch":
                return new EVInstruction(label, call.Opcode(0, Jsm.Opcode.JMP, Jsm.Opcode.JMP_IFN, Jsm.Opcode.JMP_IF)) { Targets = new[] { call.Int(1) } };
            case "Switch":
                return new EVInstruction(label, call.Opcode(0, Jsm.Opcode.JMP_SWITCH, Jsm.Opcode.JMP_SWITCHEX, Jsm.Opcode.rsv0d))
                {
                    CaseValues = call.Array(1).Select(value => checked((ushort)ConstantInt(value))).ToArray(),
                    Targets = new[] { call.Int(2) }.Concat(call.Array(3).Select(ConstantInt)).ToArray()
                };
            case "Region":
                return MakeInstruction(label, Jsm.Opcode.QUAD, call.Byte(0), new[] { call.Operand(1) }.Concat(call.Array(2).SelectMany(call.Point)).ToArray());
            case "Wait":
                return MakeInstruction(label, Jsm.Opcode.WAIT, call.Byte(1), call.Operand(0));
            case "SetScale":
                return MakeInstruction(label, Jsm.Opcode.SPS, call.Byte(4), call.Operand(0), new EVOperand(145), call.Operand(1), call.Operand(2), call.Operand(3));
            case "Model":
                return MakeInstruction(label, Jsm.Opcode.MODEL, call.Byte(2), call.Operand(0), call.Operand(1));
            case "Animation":
                return MakeInstruction(label, call.Opcode(0, Jsm.Opcode.AIDLE, Jsm.Opcode.AWALK, Jsm.Opcode.ARUN, Jsm.Opcode.ASLEEP, Jsm.Opcode.ATURNL, Jsm.Opcode.ATURNR, Jsm.Opcode.ANIM), call.Byte(2), call.Operand(1));
            case "Sps":
                return MakeInstruction(label, call.Opcode(0, Jsm.Opcode.SPS, Jsm.Opcode.SPS2), call.Byte(1), call.Operand(2), call.Operand(3), call.Operand(4), call.Operand(5), call.Operand(6));
            case "Sound":
                return MakeInstruction(label, call.Opcode(0, Jsm.Opcode.FLDSND0, Jsm.Opcode.FLDSND1, Jsm.Opcode.FLDSND2, Jsm.Opcode.FLDSND3), call.Byte(1),
                    new[] { call.Operand(2), call.Operand(3) }.Concat(call.Operands(4)).ToArray());
            case "Wipe":
                return MakeInstruction(label, Jsm.Opcode.WIPERGB, call.Byte(0), call.Operand(1), call.Operand(2), call.Operand(3), call.Operand(4), call.Operand(5), call.Operand(6));
            default:
                throw Invalid(call.Syntax, $"Unsupported instruction {call.Name}");
        }
    }

    private static EVExpressionToken[] AsExpression(EVOperand operand) =>
        operand.Expression ?? new[] { EVExpressionToken.Literal(operand.Value) };

    private static bool TryGetExpression(IInvocationOperation invocation, INamedTypeSymbol vm, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out SourceExpression? operation)
    {
        operation = null;
        INamedTypeSymbol? attribute = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.ExpressionInstructionAttribute");
        if (attribute == null || FindAttribute(invocation.TargetMethod, attribute) is not AttributeData data)
            return false;

        var opcode = (Jsm.Excode)AttributeValue<int>(data, 0);
        if (!SourceExpression.TryGet(opcode, out operation))
            throw Invalid(invocation.Syntax, $"No expression form for {opcode}");
        return true;
    }

    private static EVExpressionToken? ReadStorage(IPropertySymbol property, INamedTypeSymbol vm, Compilation? compilation)
    {
        INamedTypeSymbol? attribute = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.StorageViewAttribute");
        if (attribute == null || FindAttribute(property, attribute) is not AttributeData declaration)
            return compilation == null || property.DeclaredAccessibility != Accessibility.Public || property.DeclaringSyntaxReferences.Length == 0
                ? null
                : LayoutViews.GetValue(compilation, static _ => new ConcurrentDictionary<ISymbol, EVExpressionToken?>(SymbolEqualityComparer.Default))
                    .GetOrAdd(property, _ => ReadLayoutStorage(property, vm, compilation));
        var view = new EVVariable((EVStorageScope)AttributeValue<int>(declaration, 0), (EVStorageType)AttributeValue<int>(declaration, 1),
            AttributeValue<int>(declaration, 2));
        SpecialType expected = view.UsesBitAddress ? SpecialType.System_Boolean : SpecialType.System_Int32;
        if (property.Type.SpecialType != expected || property.IsIndexer)
            throw Invalid(declaration.ApplicationSyntaxReference!.GetSyntax(), "The storage declaration must match the view type");
        return view.Encode(AttributeValue<bool>(declaration, 3), AttributeValue<bool>(declaration, 4));
    }

    private static EVExpressionToken? ReadLayoutStorage(IPropertySymbol property, INamedTypeSymbol vm, Compilation compilation)
    {
        var layoutAttribute = compilation.GetTypeByMetadataName("System.Runtime.InteropServices.StructLayoutAttribute");
        var offsetAttribute = compilation.GetTypeByMetadataName("System.Runtime.InteropServices.FieldOffsetAttribute");
        if (layoutAttribute == null || offsetAttribute == null ||
            property.ContainingType.GetMembers().OfType<IPropertySymbol>().FirstOrDefault(candidate =>
                candidate.RefKind == RefKind.Ref && FindAttribute(candidate.Type, layoutAttribute) != null) is not { } memory)
            return null;

        var declaration = (PropertyDeclarationSyntax)property.DeclaringSyntaxReferences[0].GetSyntax();
        SemanticModel model = compilation.GetSemanticModel(declaration.SyntaxTree);
        var memoryDeclaration = (PropertyDeclarationSyntax)memory.DeclaringSyntaxReferences[0].GetSyntax();
        if (memoryDeclaration.ExpressionBody?.Expression is not RefExpressionSyntax reference ||
            compilation.GetSemanticModel(memoryDeclaration.SyntaxTree).GetOperation(reference.Expression) is not IInvocationOperation view ||
            view.TargetMethod.Name != "View" || !IsSupportType(view.TargetMethod.ContainingType, vm, "ScriptMemory") ||
            view.Arguments.Length != 1 || AttributeValue<int>(FindAttribute(memory.Type, layoutAttribute)!, 0) != 2)
            throw Invalid(memoryDeclaration, "Storage layout must be an explicit ScriptMemory view");

        IOperation? getter = Accessor(SyntaxKind.GetAccessorDeclaration);
        IOperation? setter = Accessor(SyntaxKind.SetAccessorDeclaration);
        if (ReadField(getter) is not { } field ||
            setter is not ISimpleAssignmentOperation { Value: IParameterReferenceOperation { Parameter.ContainingSymbol: IMethodSymbol { MethodKind: MethodKind.PropertySet } } } assignment ||
            !SymbolEqualityComparer.Default.Equals(field, ReadField(assignment.Target)) ||
            FindAttribute(field, offsetAttribute) is not { } offset)
            throw Invalid(declaration, $"Storage property {property.Name} must read and write the same declared memory field");

        if (field.Type is not INamedTypeSymbol cell || !SymbolEqualityComparer.Default.Equals(cell.ContainingAssembly, vm.ContainingAssembly) ||
            property.Type.SpecialType != (cell.Name == "BitCell" ? SpecialType.System_Boolean : SpecialType.System_Int32))
            throw Invalid(declaration, "The public storage type must match its memory cell");

        var encodingType = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.StorageEncodingAttribute");
        AttributeData? encoding = encodingType == null ? null : FindAttribute(field, encodingType);
        bool HasEncoding(int position) => encoding != null && AttributeValue<bool>(encoding, position);
        int address = AttributeValue<int>(offset, 0);
        EVStorageType type = cell.Name switch
        {
            "ByteCell" => EVStorageType.Byte,
            "SByteCell" => EVStorageType.SByte,
            "UInt16Cell" => EVStorageType.UInt16,
            "Int16Cell" => EVStorageType.Int16,
            "Int24Cell" => HasEncoding(3) ? EVStorageType.UInt24 : EVStorageType.Int24,
            "BitCell" => HasEncoding(2) ? EVStorageType.SBit : EVStorageType.Bit,
            _ => throw Invalid(declaration, $"Unsupported memory cell '{cell.Name}'")
        };
        if (type is EVStorageType.SBit or EVStorageType.Bit)
        {
            ITypeSymbol position = cell.TypeArguments[0];
            if (!SymbolEqualityComparer.Default.Equals(position.ContainingAssembly, vm.ContainingAssembly) ||
                position.GetMembers("Position").SingleOrDefault() is not IFieldSymbol { ConstantValue: int bit } || bit is < 0 or > 7)
                throw Invalid(declaration, "Expected a declared bit position in the memory cell");
            address = checked(address * 8 + bit);
        }
        var variable = new EVVariable((EVStorageScope)ConstantInt(view.Arguments[0].Value), type, address);
        return variable.Encode(HasEncoding(0), HasEncoding(1));

        IOperation? Accessor(SyntaxKind kind) =>
            declaration.AccessorList?.Accessors.FirstOrDefault(accessor => accessor.IsKind(kind))?.ExpressionBody is { } body
                ? model.GetOperation(body.Expression) : null;

        IFieldSymbol? ReadField(IOperation? operation) =>
            operation is IPropertyReferenceOperation { Property.Name: "Value", Instance: IFieldReferenceOperation { Instance: IPropertyReferenceOperation owner } fieldAccess } &&
            SymbolEqualityComparer.Default.Equals(owner.Property, memory)
                ? fieldAccess.Field : null;
    }

    private static EVExpressionToken[] ReadNative(IOperation value, INamedTypeSymbol vm, SyntaxNode source, bool truthOnly = false)
    {
        var tokens = new List<EVExpressionToken>();
        Append(value, truthOnly);
        return tokens.ToArray();

        void Append(IOperation operation, bool testTruth = false)
        {
            switch (operation)
            {
                case IPropertyReferenceOperation member when ReadStorageReference(member, vm) is EVExpressionToken storage:
                    tokens.Add(storage);
                    return;
                case IPropertyReferenceOperation member when
                    vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.ObjectPropertyAttribute") is { } attribute &&
                    FindAttribute(member.Property, attribute) is { } objectProperty:
                    int target = ReadObjectPropertyReceiver(member, vm);
                    tokens.Add(new EVExpressionToken((byte)Jsm.Excode.B_OBJSPECA, target | AttributeValue<byte>(objectProperty, 0) << 8));
                    if (AttributeValue<bool>(objectProperty, 1))
                    {
                        tokens.Add(EVExpressionToken.Literal(5));
                        Emit(Jsm.Excode.BitRight);
                    }
                    return;
                case IBinaryOperation binary when binary.OperatorMethod == null:
                    BinaryOperatorKind kind = binary.OperatorKind;
                    if (kind is BinaryOperatorKind.ConditionalAnd or BinaryOperatorKind.ConditionalOr)
                    {
                        if (!IsPure(binary.LeftOperand) || !IsPure(binary.RightOperand))
                            throw Invalid(binary.Syntax, "Short-circuit operands with side effects require a structured condition");
                        kind = kind == BinaryOperatorKind.ConditionalAnd ? BinaryOperatorKind.And : BinaryOperatorKind.Or;
                    }
                    bool logical = binary.LeftOperand.Type?.SpecialType == SpecialType.System_Boolean &&
                        kind is BinaryOperatorKind.And or BinaryOperatorKind.Or or BinaryOperatorKind.ExclusiveOr;
                    var spec = NativeExpression.Binary.FirstOrDefault(entry => entry.Kind == kind &&
                        (kind is not (BinaryOperatorKind.And or BinaryOperatorKind.Or or BinaryOperatorKind.ExclusiveOr) || entry.BooleanResult == logical));
                    if (spec.Symbol == null)
                        throw Invalid(binary.Syntax, "Unsupported binary expression");
                    bool logicalOperands = spec.Opcode is Jsm.Excode.BooleanAnd or Jsm.Excode.BooleanOr;
                    Append(binary.LeftOperand, logicalOperands);
                    Append(binary.RightOperand, logicalOperands);
                    Emit(spec.Opcode);
                    return;
                case ISimpleAssignmentOperation assignment:
                    Target(assignment.Target);
                    Append(assignment.Value, ReferenceEquals(assignment, value) && assignment.Target.Type?.SpecialType == SpecialType.System_Boolean);
                    Emit(Jsm.Excode.Let);
                    return;
                case ICompoundAssignmentOperation compound when compound.OperatorMethod == null:
                    var update = NativeExpression.Compound.FirstOrDefault(entry => entry.Kind == compound.OperatorKind);
                    if (update.Symbol == null)
                        throw Invalid(compound.Syntax, "Unsupported compound assignment");
                    Target(compound.Target);
                    Append(compound.Value);
                    Emit(update.Opcode);
                    return;
                case IIncrementOrDecrementOperation increment when increment.OperatorMethod == null:
                    Target(increment.Target);
                    bool increase = increment.Kind == OperationKind.Increment;
                    Emit(increase ? (increment.IsPostfix ? Jsm.Excode.IncrementPost : Jsm.Excode.B_PRE_PLUS)
                        : (increment.IsPostfix ? Jsm.Excode.DecrementPost : Jsm.Excode.B_PRE_MINUS));
                    return;
                case IUnaryOperation unary when unary.OperatorMethod == null:
                    if (unary.OperatorKind == UnaryOperatorKind.Minus && unary.Syntax is PrefixUnaryExpressionSyntax { Operand: LiteralExpressionSyntax } &&
                        unary.ConstantValue is { HasValue: true, Value: int negative })
                    {
                        tokens.Add(EVExpressionToken.Literal(negative));
                        return;
                    }
                    Append(unary.Operand);
                    Emit(unary.OperatorKind switch
                    {
                        UnaryOperatorKind.Minus => Jsm.Excode.B_SINGLE_MINUS,
                        UnaryOperatorKind.BitwiseNegation => Jsm.Excode.B_COMP,
                        UnaryOperatorKind.Not when unary.Operand.Type?.SpecialType == SpecialType.System_Boolean => Jsm.Excode.B_NOT,
                        _ => throw Invalid(unary.Syntax, "Unsupported unary expression")
                    });
                    return;
                case IInvocationOperation invocation:
                    var intrinsic = new Intrinsic(invocation, vm, service: true);
                    if (TryGetExpression(invocation, vm, out SourceExpression? specifier))
                    {
                        tokens.AddRange(intrinsic.EncodeValue(specifier));
                        return;
                    }
                    if (SymbolEqualityComparer.Default.Equals(invocation.TargetMethod.ContainingType, vm))
                    {
                        EVOperand encoded = Intrinsic.ReadOperand(invocation, vm);
                        tokens.AddRange(AsExpression(encoded));
                        return;
                    }
                    break;
                case IConversionOperation conversion when conversion.OperatorMethod is { Name: "op_Explicit" } cast &&
                    IsSupportType(cast.ContainingType, vm, "Operand") && conversion.Type?.SpecialType == SpecialType.System_Int32:
                    Append(conversion.Operand);
                    return;
                case IConversionOperation conversion when conversion.OperatorMethod == null &&
                    (conversion.Conversion.IsIdentity || conversion.IsImplicit && conversion.Type?.SpecialType == SpecialType.System_Int32 ||
                     conversion.Operand.Type is { } input && IsSupportType(input, vm, "FacingSector") && conversion.Type?.SpecialType == SpecialType.System_Int32 ||
                     conversion.Type is { } output && conversion.Operand.Type?.SpecialType == SpecialType.System_Int32 &&
                     (IsSupportType(output, vm, "FieldBlendMode") || IsSupportType(output, vm, "PlayerInput") || IsSupportType(output, vm, "TriangleAttributeMask") ||
                      IsSupportType(output, vm, "DialogueOption"))):
                    Append(conversion.Operand);
                    return;
                case IIsPatternOperation { Pattern: INegatedPatternOperation { Pattern: IConstantPatternOperation { Value.ConstantValue: { HasValue: true, Value: 0 } } } } truth
                    when truth.Value.Type is { } condition && (condition.SpecialType == SpecialType.System_Int32 || IsSupportType(condition, vm, "DialogueOption")):
                    Append(truth.Value);
                    if (!testTruth)
                    {
                        tokens.Add(EVExpressionToken.Literal(0));
                        Emit(Jsm.Excode.NotEquivalence);
                    }
                    return;
                case IConditionalOperation { WhenTrue.ConstantValue: { HasValue: true, Value: 1 }, WhenFalse.ConstantValue: { HasValue: true, Value: 0 } } numeric:
                    Append(numeric.Condition);
                    return;
                case IParenthesizedOperation parentheses:
                    Append(parentheses.Operand, testTruth);
                    return;
            }
            if (operation.ConstantValue is { HasValue: true, Value: bool boolean })
                tokens.Add(EVExpressionToken.Literal(boolean ? 1 : 0));
            else if (operation.ConstantValue is { HasValue: true, Value: int or short or sbyte or byte or ushort })
                tokens.Add(EVExpressionToken.Literal(ConstantInt(operation)));
            else
                throw Invalid(operation.Syntax ?? source, "Unsupported value expression");
        }

        void Target(IOperation target)
        {
            if (target is not IPropertyReferenceOperation member || ReadStorageReference(member, vm) is not EVExpressionToken storage)
                throw Invalid(target.Syntax, "Expected a declared VM storage member");
            tokens.Add(storage);
        }

        void Emit(Jsm.Excode opcode) => tokens.Add(new EVExpressionToken((byte)opcode));

        static bool IsPure(IOperation operation) => operation switch
        {
            ILiteralOperation => true,
            IPropertyReferenceOperation => true,
            IBinaryOperation binary => IsPure(binary.LeftOperand) && IsPure(binary.RightOperand),
            IUnaryOperation unary => IsPure(unary.Operand),
            IConversionOperation conversion => IsPure(conversion.Operand),
            _ => false
        };
    }

    private static bool IsStorageReceiver(IOperation operation, INamedTypeSymbol vm, HashSet<ISymbol> accessors)
    {
        switch (operation)
        {
            case IInstanceReferenceOperation or IParameterReferenceOperation or ILocalReferenceOperation:
                return true;
            case IConversionOperation { OperatorMethod: null, Type: { } target, Operand.Type: { } source } conversion:
                if (conversion.SemanticModel?.Compilation is not CSharpCompilation compilation)
                    return false;
                if (IsSafeReferenceConversion(compilation, source, target))
                    return IsStorageReceiver(conversion.Operand, vm, accessors);
                return conversion.SemanticModel.GetEnclosingSymbol(conversion.Syntax.SpanStart) is { } caller &&
                    IsContextReference(conversion, caller, vm);
            case IPropertyReferenceOperation member when member.Instance != null && IsStorageReceiver(member.Instance, vm, accessors):
                if (IsDefaultStorageBinding(member, vm) ||
                    member.Property.Name == "Context" && IsSupportType(member.Property.ContainingType, vm, "EventParticipant") ||
                    member.Property.Name == "Globals" && IsSupportType(member.Property.ContainingType, vm, "EventSceneContext"))
                    return true;
                if (!accessors.Add(member.Property) || member.Property.IsVirtual || member.Property.IsOverride ||
                    member.Property.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not PropertyDeclarationSyntax { ExpressionBody.Expression: { } forwarding })
                    return false;
                IOperation? next = member.SemanticModel!.Compilation.GetSemanticModel(forwarding.SyntaxTree).GetOperation(forwarding);
                return next != null && IsStorageReceiver(next, vm, accessors);
            default:
                return false;
        }
    }

    private static bool IsDefaultStorageBinding(IPropertyReferenceOperation member, INamedTypeSymbol vm)
    {
        INamedTypeSymbol owner = member.Property.ContainingType;
        Compilation compilation = member.SemanticModel!.Compilation;
        if (!IsGetOnlyAutoProperty(member.Property) ||
            !(SymbolEqualityComparer.Default.Equals(owner, ReadEventContext(compilation, vm)) ||
                DerivesFrom(owner, vm, "EventParticipant") && SymbolEqualityComparer.Default.Equals(owner, member.Property.Type.ContainingType)) ||
            member.Property.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax() is not PropertyDeclarationSyntax { Initializer.Value: { } initializer } ||
            compilation.GetSemanticModel(initializer.SyntaxTree).GetOperation(initializer) is not IObjectCreationOperation
            {
                Arguments.Length: 0,
                Initializer: null,
                Constructor: { IsImplicitlyDeclared: true, ContainingType: { BaseType.SpecialType: SpecialType.System_Object } type }
            } || !SymbolEqualityComparer.Default.Equals(type, member.Property.Type))
            return false;

        return type.DeclaringSyntaxReferences.All(reference => reference.GetSyntax() is ClassDeclarationSyntax { ParameterList: null } declaration &&
            !declaration.Members.Any(member => member is ConstructorDeclarationSyntax or PropertyDeclarationSyntax { Initializer: not null } ||
                member is BaseFieldDeclarationSyntax field &&
                field.Declaration.Variables.Any(variable => variable.Initializer != null)));
    }

    private static int ReadObjectPropertyReceiver(IPropertyReferenceOperation member, INamedTypeSymbol vm)
    {
        if (member.Instance is IInvocationOperation factory && IsSupportType(factory.TargetMethod.ContainingType, vm, "ObjectProperties"))
        {
            IOperation target = factory.Arguments[0].Value;
            if (target is IObjectCreationOperation { Arguments.Length: 1 } instance && IsSupportType(instance.Type!, vm, "InstanceUid"))
            {
                int uid = checked((byte)ConstantInt(instance.Arguments[0].Value));
                if (uid >= 250)
                    throw Invalid(target.Syntax, $"UID {uid} is reserved for an object selector in this instruction");
                return uid;
            }
            if (IsSupportType(target.Type!, vm, "ActorSelector"))
                return checked((byte)ConstantInt(target));
        }
        else if (member.Instance is IPropertyReferenceOperation { Instance: { } receiver } view &&
            (view.Property.Name is "Facing" or "Properties"))
        {
            if (IsControlledActor(receiver, vm))
                return 250;
            if (ReadSceneParticipant(receiver, vm, member.SemanticModel!.Compilation) is { } participant)
                return SceneReferenceValue(receiver) is IInstanceReferenceOperation ? 255 : checked((byte)AttributeValue<int>(participant.Slot, 0));
        }
        throw Invalid(member.Syntax, "An object property requires an immediate UID or a bound scene selector");
    }

    private static EVInstruction MakeInstruction(int label, Jsm.Opcode opcode, byte mask, params EVOperand[] operands) =>
        new(label, opcode) { ArgumentMask = mask, Operands = operands };

    private static bool IsSupportType(ITypeSymbol type, INamedTypeSymbol vm, string name) =>
        SymbolEqualityComparer.Default.Equals(type, vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.{name}"));

    private static INamedTypeSymbol RequireType(Compilation compilation, string name) =>
        compilation.GetTypeByMetadataName($"{SourceNamespace}.{name}") ?? throw new InvalidDataException($"Missing EV source declaration {name}.");

    private static AttributeData? FindAttribute(ISymbol symbol, INamedTypeSymbol type) =>
        symbol.GetAttributes().SingleOrDefault(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, type));

    private static T AttributeValue<T>(AttributeData attribute, int index)
    {
        if (attribute.ConstructorArguments.Length <= index || attribute.ConstructorArguments[index].Value is not T value)
            throw Invalid(attribute.ApplicationSyntaxReference!.GetSyntax(), "Invalid EV layout attribute");
        return value;
    }

    private static byte[] HexAttribute(AttributeData attribute, int index)
    {
        try
        {
            return Convert.FromHexString(AttributeValue<string>(attribute, index));
        }
        catch (FormatException ex)
        {
            throw Invalid(attribute.ApplicationSyntaxReference!.GetSyntax(), "Invalid hexadecimal EV layout metadata", ex);
        }
    }

    private static int ConstantInt(IOperation value)
    {
        if (value.ConstantValue is { HasValue: true, Value: int or byte or ushort or short or sbyte })
            return Convert.ToInt32(value.ConstantValue.Value);
        throw Invalid(value.Syntax, "Expected an integer constant");
    }

    private static InvalidDataException Invalid(SyntaxNode node, string message, Exception? inner = null)
    {
        FileLinePositionSpan span = node.GetLocation().GetLineSpan();
        return new InvalidDataException($"{span.Path}({span.StartLinePosition.Line + 1},{span.StartLinePosition.Character + 1}): {message}", inner);
    }

    private sealed class Intrinsic
    {
        private readonly INamedTypeSymbol _vm;
        private readonly IOperation[] _arguments;
        public SyntaxNode Syntax { get; }
        public string Name { get; }

        public int ArgumentCount => _arguments.Length;

        public Intrinsic(IInvocationOperation invocation, INamedTypeSymbol vm, bool service = false)
        {
            if (!service && !SymbolEqualityComparer.Default.Equals(invocation.TargetMethod.ContainingType, vm))
                throw Invalid(invocation.Syntax, $"Unsupported call '{invocation.TargetMethod.ToDisplayString()}'");
            _vm = vm;
            Syntax = invocation.Syntax;
            Name = invocation.TargetMethod.Name;
            _arguments = new IOperation[invocation.TargetMethod.Parameters.Length];
            foreach (IArgumentOperation argument in invocation.Arguments)
                _arguments[argument.Parameter!.Ordinal] = argument.Value;
        }

        public int Int(int index) => ConstantInt(_arguments[index]);
        public IOperation Argument(int index) => _arguments[index];
        public byte Byte(int index) => checked((byte)Int(index));

        public Jsm.Opcode Opcode(int index, params Jsm.Opcode[] choices)
        {
            var opcode = (Jsm.Opcode)Int(index);
            if (choices.Length != 0 && !choices.Contains(opcode))
                throw Invalid(_arguments[index].Syntax, $"Opcode {opcode} is unsupported by {Name}");
            return opcode;
        }

        public ImmutableArray<IOperation> Array(int index)
        {
            if (_arguments[index] is IArrayCreationOperation { Initializer: { } initializer })
                return initializer.ElementValues;
            throw Invalid(_arguments[index].Syntax, "Expected an inline array");
        }

        public int[]? OptionalIntegers(int index)
        {
            if (_arguments[index].ConstantValue is { HasValue: true, Value: null })
                return null;
            return Array(index).Select(ConstantInt).ToArray();
        }

        public bool IsTokenList(int index) => _arguments[index].Type is IArrayTypeSymbol;

        public EVOperand Operand(int index) => ReadOperand(_arguments[index], _vm);
        public EVOperand DomainOperand(int index) => ReadDomainOperand(_arguments[index], _vm);
        public EVOperand[] Operands(int index) => Array(index).Select(value => ReadOperand(value, _vm)).ToArray();
        public EVExpressionToken[] Tokens(int index) => Array(index).Select(ReadToken).ToArray();

        public IEnumerable<EVOperand> Point(IOperation value)
        {
            Intrinsic point = Nested(value, "vertex");
            if (point.Name != "Point")
                throw Invalid(value.Syntax, "Expected a Point vertex");
            return new[] { point.Operand(0), point.Operand(1) };
        }

        public static EVOperand ReadOperand(IOperation value, INamedTypeSymbol vm)
        {
            if (value.Type is { TypeKind: TypeKind.Enum } type)
            {
                if (IsSupportType(type, vm, "BubbleSymbol"))
                {
                    int symbol = ConstantInt(value);
                    if (!type.GetMembers().OfType<IFieldSymbol>().Any(field => field.HasConstantValue && (int)field.ConstantValue! == symbol))
                        throw Invalid(value.Syntax, "Use a declared BubbleSymbol or BubbleFromScript for an extended icon");
                    return new EVOperand(symbol);
                }
                if (IsSupportType(type, vm, "PlayerInput") || IsSupportType(type, vm, "TriangleAttributeMask"))
                    return ReadDomainOperand(value, vm);
            }
            if (value is IConversionOperation conversion && conversion.OperatorMethod?.Name == "op_Implicit" &&
                conversion.OperatorMethod.ContainingType.ToDisplayString() == $"{SourceNamespace}.Operand")
            {
                IOperation argument = conversion.Operand;
                if (argument is ILiteralOperation or IFieldReferenceOperation or ILocalReferenceOperation ||
                    argument is IUnaryOperation { Syntax: PrefixUnaryExpressionSyntax { Operand: LiteralExpressionSyntax } } &&
                    argument.ConstantValue.HasValue)
                {
                    if (argument.ConstantValue is { HasValue: true, Value: bool flag })
                        return new EVOperand(flag ? 1 : 0);
                    if (argument.ConstantValue.HasValue)
                        return new EVOperand(ConstantInt(argument));
                }
                return new EVOperand(ReadNative(argument, vm, value.Syntax));
            }

            if (value is not IInvocationOperation invocation)
                throw Invalid(value.Syntax, "Expected an immediate operand or VM expression");
            if (TryGetExpression(invocation, vm, out SourceExpression? operation))
                return new EVOperand(new Intrinsic(invocation, vm, service: true).EncodeValue(operation));

            var expression = new Intrinsic(invocation, vm);
            return expression.Name switch
            {
                "Expression" => new EVOperand(expression.Tokens(0)),
                "Read" => new EVOperand(new[] { expression.ReadVariable(expression._arguments[0]).Encode(expression.Bool(1), expression.Bool(2)) }),
                "Value" => new EVOperand(new[] { EVExpressionToken.Literal(expression.Int(0)) }),
                "Wide" => new EVOperand(new[] { EVExpressionToken.Literal(expression.Int(0), wide: true, highBits: expression.Byte(1)) }),
                _ => throw Invalid(value.Syntax, "Expected an immediate operand or VM expression")
            };
        }

        public EVExpressionToken[] EncodeValue(SourceExpression operation)
        {
            if (ArgumentCount != operation.Parameters.Length)
                throw Invalid(Syntax, $"Incorrect operand count for {operation.Opcode}");

            var tokens = new List<EVExpressionToken>();
            int data = 0;
            switch (operation.Payload)
            {
                case ExpressionPayload.None:
                    for (int index = 0; index < ArgumentCount; index++)
                        tokens.AddRange(AsExpression(Operand(index)));
                    break;
                case ExpressionPayload.ObjectProperty:
                    data = Byte(0) | (Byte(1) << 8);
                    break;
                default:
                    data = Byte(0);
                    break;
            }
            tokens.Add(new EVExpressionToken((byte)operation.Opcode, data));
            return tokens.ToArray();
        }

        private EVExpressionToken ReadToken(IOperation value)
        {
            Intrinsic token = Nested(value, "expression token");
            return token.Name switch
            {
                "Op" => new EVExpressionToken(token.Byte(0), token.Int(1)),
                "Literal" => EVExpressionToken.Literal(token.Int(0), token.Bool(1), token.Byte(2)),
                "Variable" => token.ReadVariable(token._arguments[0]).Encode(token.Bool(1), token.Bool(2)),
                "IsKeyPressed" => new EVExpressionToken((byte)Jsm.Excode.IsKeyPressed),
                _ => throw Invalid(value.Syntax, "Unsupported expression token")
            };
        }

        private EVVariable ReadVariable(IOperation value)
        {
            if (value is not IPropertyReferenceOperation { Property.IsIndexer: true, Instance: { } instance } indexer)
                throw Invalid(value.Syntax, "Expected a VM storage reference");

            string type = indexer.Property.ContainingType.ToDisplayString();
            var arguments = indexer.Arguments.OrderBy(argument => argument.Parameter!.Ordinal).ToArray();
            if (type == $"{SourceNamespace}.VariableRef" && arguments.Length == 1)
            {
                int bit = ConstantInt(arguments[0].Value);
                EVVariable reference = ReadVariable(instance);
                if (reference.UsesBitAddress || (uint)bit >= reference.Width)
                    throw Invalid(value.Syntax, $"Bit {bit} is outside the {reference.Width}-bit storage view");
                return reference.Bit(bit);
            }

            if (type == $"{SourceNamespace}.Storage" && arguments.Length == 2 &&
                instance is IPropertyReferenceOperation storage &&
                SymbolEqualityComparer.Default.Equals(storage.Property.ContainingType, _vm) &&
                Enum.TryParse(storage.Property.Name, out EVStorageScope scope))
                return new EVVariable(scope, (EVStorageType)ConstantInt(arguments[0].Value), ConstantInt(arguments[1].Value));

            throw Invalid(value.Syntax, "Unsupported VM storage reference");
        }

        public bool Bool(int index)
        {
            if (_arguments[index].ConstantValue is { HasValue: true, Value: bool value })
                return value;
            throw Invalid(_arguments[index].Syntax, "Expected a Boolean constant");
        }

        private Intrinsic Nested(IOperation value, string kind)
        {
            if (value is IInvocationOperation invocation)
                return new Intrinsic(invocation, _vm);
            throw Invalid(value.Syntax, $"Unsupported {kind} '{value.Syntax}'");
        }
    }
}
