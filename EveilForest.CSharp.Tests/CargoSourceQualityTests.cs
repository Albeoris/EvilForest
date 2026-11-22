using EveilForest.CSharp;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Xunit;
using Xunit.Abstractions;

namespace EveilForest.CSharp.Tests;

public sealed class CargoSourceQualityTests(ITestOutputHelper output)
{
    [Fact]
    public void CargoHandlersBindToNativeOperatorsStorageSwitchesAndAwaitedServices()
    {
        const string name = "EVT_ALEX1_TS_CARGO_0";
        string project = Path.Combine(Path.GetTempPath(), $"evilforest-quality-{Guid.NewGuid():N}");
        try
        {
            using var input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
            EventProjectExporter.Export(EVContainer.Read(input), name, project, allowNativePhaseDispatch: true);
            Assert.False(File.Exists(Path.Combine(project, "EventSource.cs")));
            Assert.True(File.Exists(Path.Combine(project, "lib", "EvilForest.EventSource.dll")));
            SyntaxTree[] trees = Directory.GetFiles(project, "*.cs", SearchOption.AllDirectories)
                .Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path)).ToArray();
            var compilation = CSharpCompilation.Create("Cargo", trees,
                SourceProjectTest.EventReferences(project),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            Assert.DoesNotContain(compilation.GetDiagnostics(), diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

            INamedTypeSymbol objectType = compilation.GetTypeByMetadataName("EvilForest.EventSource.ObjectSlotAttribute")!;
            INamedTypeSymbol cutsceneType = compilation.GetTypeByMetadataName("EvilForest.EventSource.EventCutscene")!;
            INamedTypeSymbol location = compilation.GetTypeByMetadataName("Event_EVT_ALEX1_TS_CARGO_0.Location")!;
            Assert.Equal(Accessibility.Internal, location.DeclaredAccessibility);
            Assert.All(location.GetMembers().OfType<IPropertySymbol>(), property => Assert.DoesNotMatch(@"^\w+_\d+$", property.Name));
            INamedTypeSymbol globals = compilation.GetTypeByMetadataName("EvilForest.EventSource.Globals")!;
            Assert.Equal("EvilForest.EventSource", globals.ContainingAssembly.Name);
            Assert.Equal(1470, globals.GetMembers().OfType<IPropertySymbol>().Count());
            var operations = new List<IOperation>();
            var objects = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
            foreach (SyntaxTree tree in trees)
            {
                SemanticModel model = compilation.GetSemanticModel(tree);
                foreach (ClassDeclarationSyntax declaration in tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    INamedTypeSymbol? type = model.GetDeclaredSymbol(declaration);
                    if (type == null)
                        continue;
                    bool isParticipant = type.GetAttributes().Any(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, objectType));
                    if (!isParticipant && !SymbolEqualityComparer.Default.Equals(type.BaseType, cutsceneType))
                        continue;
                    if (isParticipant && objects.Add(type) && type.GetTypeMembers("LocalMemory").SingleOrDefault() is { } locals)
                        Assert.All(locals.GetMembers().OfType<IPropertySymbol>(), property => Assert.DoesNotMatch(@"^\w+_\d+$", property.Name));
                    foreach (MethodDeclarationSyntax handler in declaration.Members.OfType<MethodDeclarationSyntax>())
                        operations.AddRange(model.GetOperation(handler.Body!)!.Descendants());
                }
            }

            Assert.Equal(25, objects.Count);
            Assert.DoesNotContain(objects, type => type.GetAttributes().Any(attribute =>
                SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, objectType) &&
                attribute.ConstructorArguments[0].Value is 3));
            var launch = Assert.Single(operations.OfType<IInvocationOperation>(), operation =>
                operation.TargetMethod.ContainingType.Name == "ScriptService" && operation.TargetMethod.Name == "StartDirector");
            var binding = Assert.IsAssignableFrom<IDelegateCreationOperation>(Assert.Single(launch.Arguments).Value);
            var director = Assert.IsAssignableFrom<IMethodReferenceOperation>(binding.Target).Method;
            Assert.Equal("LightTheCandle", director.ContainingType.Name);
            Assert.Equal("Direct", director.Name);
            Assert.Empty(director.Parameters);
            Assert.Equal("EventRoutine", director.ReturnType.Name);
            Assert.Contains(operations.OfType<IBinaryOperation>(), operation => operation.OperatorKind == BinaryOperatorKind.LessThan &&
                operation.LeftOperand is IPropertyReferenceOperation property && property.Property.Name == "EnvironmentSoundSlot1Id" &&
                operation.RightOperand.ConstantValue is { HasValue: true, Value: 0 });
            Assert.Contains(operations.OfType<ISimpleAssignmentOperation>(), operation =>
                operation.Target is IPropertyReferenceOperation property && property.Property.Name == "FieldEntrance");
            Assert.Contains(operations.OfType<IInvocationOperation>(), operation =>
                operation.TargetMethod.ContainingType.Name == "SceneChoreography" && operation.TargetMethod.Name == "Together" &&
                operation.Arguments[0].Value.ConstantValue is { HasValue: true, Value: 3 });
            Assert.DoesNotContain(operations.OfType<ISwitchOperation>(), operation =>
                operation.Value is IPropertyReferenceOperation property && property.Property.Name == "SceneStep");
            Assert.Contains(operations.OfType<IAwaitOperation>(), operation =>
                operation.Operation is IInvocationOperation invocation && invocation.TargetMethod.ContainingType.Name == "Engine" && invocation.TargetMethod.Name == "Wait");
            Assert.DoesNotContain(operations.OfType<IInvocationOperation>(), operation =>
                operation.TargetMethod.ContainingType.ToDisplayString() == "EvilForest.EventSource.Vm" &&
                operation.TargetMethod.Name is "Branch" or "Label" or "Switch" or "TestAtLoopTail" or "Test" or "Select");
            Assert.DoesNotContain(operations.OfType<IInvocationOperation>(), operation =>
                operation.TargetMethod.ContainingType.ToDisplayString() == "EvilForest.EventSource.Variables" && operation.TargetMethod.Name == "Set");
            var inventory = operations.OfType<IInvocationOperation>().Select(invocation => invocation.TargetMethod)
                .Where(method => method.ContainingType.ContainingNamespace.ToDisplayString() == "EvilForest.EventSource" &&
                    (method.ContainingType.Name == "Vm" || (method.ContainingType.Name == "Variables" && method.Name == "Set")))
                .GroupBy(method => $"{method.ContainingType.Name}.{method.Name}").OrderBy(group => group.Key);
            foreach (var group in inventory)
                output.WriteLine($"{group.Key}\t{group.Count()}");
        }
        finally
        {
            if (Directory.Exists(project))
                Directory.Delete(project, recursive: true);
        }
    }
}
