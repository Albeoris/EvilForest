using System.Collections.Immutable;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace EveilForest.CSharp;

public static class EventProjectCompiler
{
    private const int MapStorageCapacity = 80;
    private const string SourceNamespace = "EvilForest.EventSource";
    private static readonly MetadataReference CoreLibrary = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
    private static readonly MetadataReference RuntimeFacade = MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll"));

    public static EVContainer Compile(string directory)
    {
        SyntaxTree[] trees = EnumerateSources(directory).Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path)).ToArray();
        var references = new List<MetadataReference> { CoreLibrary, RuntimeFacade };
        string? dependency = EventSourceDependency.FindForProject(directory);
        if (dependency != null)
            references.Add(MetadataReference.CreateFromFile(dependency));
        var compilation = CSharpCompilation.Create("EventSource", trees, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        Diagnostic? error = compilation.GetDiagnostics().FirstOrDefault(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
        if (error != null)
            throw new InvalidDataException(error.ToString());

        compilation = AllocateStorage(compilation);
        trees = compilation.SyntaxTrees.ToArray();

        INamedTypeSymbol layoutType = RequireType(compilation, "EventLayoutAttribute");
        INamedTypeSymbol objectType = RequireType(compilation, "ObjectSlotAttribute");
        INamedTypeSymbol scriptType = RequireType(compilation, "ScriptAttribute");
        INamedTypeSymbol vm = RequireType(compilation, "Vm");
        AttributeData layout = FindAttribute(compilation.Assembly, layoutType)
            ?? throw new InvalidDataException("Missing assembly EventLayout declaration.");
        byte[] header = HexAttribute(layout, 0);
        if (header.Length != EVContainer.HeaderSize)
            throw Invalid(layout.ApplicationSyntaxReference!.GetSyntax(), "EventLayout header size must be 128 bytes");

        var objects = new EVContainerObject[header[3]];
        foreach (SyntaxTree tree in trees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            SyntaxNode root = tree.GetRoot();
            foreach (PropertyDeclarationSyntax property in root.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
                if (model.GetDeclaredSymbol(property) is IPropertySymbol symbol && ReadStorage(symbol, vm) is EVExpressionToken token)
                    ValidateStorageCapacity(property, symbol, EVVariable.Decode(token), objectType);
            }
            foreach (ClassDeclarationSyntax cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                INamedTypeSymbol symbol = model.GetDeclaredSymbol(cls)!;
                AttributeData? declaration = FindAttribute(symbol, objectType);
                if (declaration == null)
                    continue;
                int id = AttributeValue<int>(declaration, 0);
                if ((uint)id >= objects.Length || objects[id] != null)
                    throw Invalid(cls, $"Invalid or duplicate object slot {id}");
                objects[id] = CompileObject(cls, model, declaration, scriptType, vm, id, objects.Length);
            }
        }

        for (int id = 0; id < objects.Length; id++)
        {
            if (objects[id] == null)
                throw new InvalidDataException($"Missing object declaration for slot {id}.");
        }
        return new EVContainer(header, objects, HexAttribute(layout, 1));
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
                EVExpressionToken? existing = ReadStorage(symbol, vm);
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

    private static EVContainerObject CompileObject(ClassDeclarationSyntax cls, SemanticModel model, AttributeData declaration, INamedTypeSymbol scriptType, INamedTypeSymbol vm, int id, int objectCount)
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
        foreach (MemberDeclarationSyntax member in cls.Members)
        {
            if (member is ClassDeclarationSyntax storage && storage.Modifiers.Any(SyntaxKind.StaticKeyword) &&
                storage.Members.Count > 0 && storage.Members.All(child => child is PropertyDeclarationSyntax property &&
                    model.GetDeclaredSymbol(property) is IPropertySymbol symbol && ReadStorage(symbol, vm) != null))
                continue;
            if (member is not MethodDeclarationSyntax method ||
                model.GetDeclaredSymbol(method) is not IMethodSymbol symbol ||
                FindAttribute(symbol, scriptType) is not AttributeData script ||
                !symbol.IsStatic || (!symbol.ReturnsVoid && !IsSupportType(symbol.ReturnType, vm, "EventRoutine")) || symbol.Parameters.Length != 0 || symbol.TypeParameters.Length != 0 ||
                method.Body == null)
                throw Invalid(member, $"Unsupported script declaration in object {id}");

            ushort sid = AttributeValue<ushort>(script, 0);
            int index = AttributeValue<int>(script, 1);
            if (index < 0 || entries.ContainsKey(index))
                throw Invalid(method, $"Invalid or duplicate script directory index {index} in object {id}");
            var locations = new Dictionary<int, SyntaxNode>();
            try
            {
                EVInstructionBody body = CompileBody(method.Body, model, vm, locations, symbol);
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
                body.ValidateObjectSlots(objectCount);
                byte[] bytes = body.Encode(out EVAddressCheckpoint[] checkpoints);
                entries.Add(index, new EVContainerScript(sid, bytes) { AddressChecks = checkpoints });
            }
            catch (InvalidDataException ex)
            {
                SyntaxNode source = ex.Data[nameof(EVInstruction.Label)] is int label && locations.TryGetValue(label, out SyntaxNode? statement)
                    ? statement : method;
                throw Invalid(source, $"Object {id}, script {sid}: {ex.Message}", ex);
            }
        }
        if (!entries.Keys.SequenceEqual(Enumerable.Range(0, entries.Count)))
            throw Invalid(cls, $"Missing script directory index in object {id}");

        obj.Scripts = entries.Values.ToArray();
        return obj;
    }

    private static EVInstructionBody CompileBody(BlockSyntax body, SemanticModel model, INamedTypeSymbol vm, Dictionary<int, SyntaxNode> locations, IMethodSymbol handler) =>
        new BodyCompiler(model, vm, locations, handler).Compile(body);

    private sealed class BodyCompiler(SemanticModel model, INamedTypeSymbol vm, Dictionary<int, SyntaxNode> locations, IMethodSymbol handler)
    {
        private readonly List<EVInstruction> _instructions = new();
        private readonly Dictionary<int, int> _labels = new();
        private readonly Dictionary<ILabelSymbol, int> _namedLabels = new(SymbolEqualityComparer.Default);
        private readonly Stack<int> _breakTargets = new();
        private readonly Stack<int> _continueTargets = new();
        private int _nextLabel = -1;
        private int? _expectedIp;

        public EVInstructionBody Compile(BlockSyntax body)
        {
            CompileBlock(body);
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
                    case LocalDeclarationStatementSyntax { IsConst: true } local
                        when local.Declaration.Variables.All(variable => model.GetDeclaredSymbol(variable) is ILocalSymbol { ConstantValue: int }):
                    case EmptyStatementSyntax:
                        return;
                    case LabeledStatementSyntax labeled when model.GetOperation(labeled) is ILabeledOperation boundLabel:
                        Mark(NamedLabel(boundLabel.Label));
                        CompileStatement(labeled.Statement);
                        return;
                    case GotoStatementSyntax jump when jump.IsKind(SyntaxKind.GotoStatement):
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
                    case ReturnStatementSyntax { Expression: null }:
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
                bool requiresAwait = IsSupportType(invocation.TargetMethod.ReturnType, vm, "EventAwaitable");
                if (awaited != requiresAwait)
                    throw Invalid(statement, requiresAwait ? "A suspending operation must be awaited" : "This operation cannot be awaited");
                if (TryCompileService(invocation, vm) is EVInstruction serviceInstruction)
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
            if (statement.Else == null && statement.Statement is GotoStatementSyntax jump && jump.IsKind(SyntaxKind.GotoStatement))
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
            if (operation?.Type?.SpecialType != SpecialType.System_Boolean)
                throw Invalid(expression, "Expected a Boolean condition");
            return (new EVOperand(ReadNative(operation, vm, expression)), inverted, false);
        }

        private void CompileSwitch(SwitchStatementSyntax statement)
        {
            IOperation? selector = (model.GetOperation(statement) as ISwitchOperation)?.Value;
            Intrinsic? legacy = selector is IInvocationOperation invocation && invocation.TargetMethod.Name == "Select"
                ? new Intrinsic(invocation, vm) : null;
            EVOperand value;
            if (legacy != null)
                value = legacy.Operand(1);
            else if (selector?.Type?.SpecialType == SpecialType.System_Int32)
                value = new EVOperand(ReadNative(selector, vm, statement.Expression));
            else
                throw Invalid(statement.Expression, "Expected an integer switch expression");
            bool directDispatch = statement.Sections.Count > 0 && statement.Sections.All(section =>
                section.Statements.Count == 1 && section.Statements[0] is GotoStatementSyntax jump && jump.IsKind(SyntaxKind.GotoStatement));
            int exit = NewLabel();
            int fallback = exit;
            var cases = new Dictionary<int, int>();
            var sections = new List<(SwitchSectionSyntax Section, int Label)>();
            foreach (SwitchSectionSyntax section in statement.Sections)
            {
                int label = directDispatch ? GotoTarget((GotoStatementSyntax)section.Statements[0]) : NewLabel();
                sections.Add((section, label));
                foreach (SwitchLabelSyntax item in section.Labels)
                {
                    if (item is DefaultSwitchLabelSyntax)
                        fallback = label;
                    else if (item is CaseSwitchLabelSyntax caseLabel && model.GetConstantValue(caseLabel.Value) is { HasValue: true, Value: int caseValue } && cases.TryAdd(caseValue, label))
                        continue;
                    else
                        throw Invalid(item, "Expected a unique integer case value");
                }
            }

            int[] order = cases.Keys.ToArray();
            bool fallsThrough = false;
            Jsm.Opcode opcode;
            if (legacy != null)
            {
                opcode = SwitchOpcode(legacy.Int(0), statement.Expression);
                fallsThrough = legacy.Bool(2);
                order = legacy.OptionalIntegers(3) ?? order;
            }
            else if (FindSwitchLayout(statement, value, directDispatch) is { } layout)
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
            if (explicitValues && order.Any(value => value < 0 || value > ushort.MaxValue))
                throw Invalid(statement.Expression, "Explicit switch values must fit an unsigned 16-bit integer");
            if (!explicitValues && (order[0] < short.MinValue || order[0] > short.MaxValue || !order.SequenceEqual(Enumerable.Range(order[0], order.Length))))
                throw Invalid(statement.Expression, "A dense switch table requires consecutive values starting with a 16-bit signed value");

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
            for (int index = 0; index < sections.Count; index++)
            {
                var (section, label) = sections[index];
                Mark(label);
                bool omitBreak = index == sections.Count - 1 && fallsThrough;
                if (omitBreak && section.Statements.LastOrDefault() is not BreakStatementSyntax)
                    throw Invalid(section, "The fall-through section must end with break");
                int count = section.Statements.Count - (omitBreak ? 1 : 0);
                for (int step = 0; step < count; step++)
                    CompileStatement(section.Statements[step]);
            }
            _breakTargets.Pop();
            Mark(exit);
        }

        private AttributeData? FindSwitchLayout(SwitchStatementSyntax statement, EVOperand selector, bool directDispatch)
        {
            INamedTypeSymbol? layoutType = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.SwitchLayoutAttribute");
            if (layoutType == null || selector.Expression is not { Length: > 0 } tokens)
                return null;
            string key = SwitchLayout.Key(tokens, statement.Sections.Select(section =>
                SwitchLayout.Section(section.Labels.OfType<CaseSwitchLabelSyntax>().Select(label => (int)model.GetConstantValue(label.Value).Value!),
                    section.Labels.Any(label => label is DefaultSwitchLabelSyntax), section.Statements.LastOrDefault() is ReturnStatementSyntax)), directDispatch);
            return handler.GetAttributes().FirstOrDefault(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, layoutType) &&
                AttributeValue<string>(attribute, 0) == key);
        }

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
        INamedTypeSymbol? attribute = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.InstructionAttribute");
        if (attribute == null || FindAttribute(invocation.TargetMethod, attribute) is not AttributeData mapping)
            return null;
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

    private static EVExpressionToken? ReadStorage(IPropertySymbol property, INamedTypeSymbol vm)
    {
        INamedTypeSymbol? attribute = vm.ContainingAssembly.GetTypeByMetadataName($"{SourceNamespace}.StorageViewAttribute");
        if (attribute == null || FindAttribute(property, attribute) is not AttributeData declaration)
            return null;
        var view = new EVVariable((EVStorageScope)AttributeValue<int>(declaration, 0), (EVStorageType)AttributeValue<int>(declaration, 1),
            AttributeValue<int>(declaration, 2));
        SpecialType expected = view.UsesBitAddress ? SpecialType.System_Boolean : SpecialType.System_Int32;
        if (property.Type.SpecialType != expected || !property.IsStatic || property.IsIndexer)
            throw Invalid(declaration.ApplicationSyntaxReference!.GetSyntax(), "The storage declaration must match the view type");
        return view.Encode(AttributeValue<bool>(declaration, 3), AttributeValue<bool>(declaration, 4));
    }

    private static EVExpressionToken[] ReadNative(IOperation value, INamedTypeSymbol vm, SyntaxNode source)
    {
        var tokens = new List<EVExpressionToken>();
        Append(value);
        return tokens.ToArray();

        void Append(IOperation operation)
        {
            switch (operation)
            {
                case IPropertyReferenceOperation member when ReadStorage(member.Property, vm) is EVExpressionToken storage:
                    tokens.Add(storage);
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
                    Append(binary.LeftOperand);
                    Append(binary.RightOperand);
                    Emit(spec.Opcode);
                    return;
                case ISimpleAssignmentOperation assignment:
                    Target(assignment.Target);
                    Append(assignment.Value);
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
                    (conversion.Conversion.IsIdentity || conversion.IsImplicit && conversion.Type?.SpecialType == SpecialType.System_Int32):
                    Append(conversion.Operand);
                    return;
                case IIsPatternOperation { Pattern: INegatedPatternOperation { Pattern: IConstantPatternOperation { Value.ConstantValue: { HasValue: true, Value: 0 } } } } truth
                    when truth.Value.Type?.SpecialType == SpecialType.System_Int32:
                    Append(truth.Value);
                    return;
                case IConditionalOperation { WhenTrue.ConstantValue: { HasValue: true, Value: 1 }, WhenFalse.ConstantValue: { HasValue: true, Value: 0 } } numeric:
                    Append(numeric.Condition);
                    return;
                case IParenthesizedOperation parentheses:
                    Append(parentheses.Operand);
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
            if (target is not IPropertyReferenceOperation member || ReadStorage(member.Property, vm) is not EVExpressionToken storage)
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
