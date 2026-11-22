using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class ChoreographyChoiceCompilerTests : IDisposable
{
    private const string Field = "EVT_ALEX1_AC_LIB";
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-choice-compiler-{Guid.NewGuid():N}");

    [Fact]
    public void DifferentOriginalLayoutsForOtherwiseIdenticalChoicesCannotSilentlyPublishDifferentBytes()
    {
        EVContainer field = Fixture("EVT_ALEX1_AT_TICKET");
        var director = field.Objects[2].Scripts.Single(script => script.Id == 1);
        var body = EVInstructionBody.Decode(director.Body);
        var tables = body.Instructions.Where((step, index) => index > 0 && step.Opcode == Jsm.Opcode.JMP_SWITCH &&
            body.Instructions[index - 1].Expression is [var selector] && EVVariable.Decode(selector).Address == 27).ToArray();
        Assert.Equal(2, tables.Length);
        tables[1].Opcode = Jsm.Opcode.JMP_SWITCHEX;
        tables[1].CaseValues = [0, 1];
        director.Body = body.Encode();
        string destination = Path.Combine(_directory, "mixed-layout");
        try
        {
            EventProjectExporter.Export(field, "EVT_MIXED_CHOICE_TABLES", destination, useCutsceneNames: false);
        }
        catch (InvalidDataException error)
        {
            Assert.Contains("EVT_MIXED_CHOICE_TABLES", error.Message);
            Assert.False(Directory.Exists(destination));
            return;
        }
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(destination).Serialize());
    }
    [Fact]
    public void ReorderedCasesPreserveTheNativeSwitchLayout()
    {
        EVContainer original = Fixture();
        string project = Export(original, "choice");
        EditChoice(project, choice =>
        {
            SeparatedSyntaxList<ArgumentSyntax> arguments = choice.ArgumentList.Arguments;
            Assert.Equal(3, arguments.Count);
            return choice.WithArgumentList(choice.ArgumentList.WithArguments(SyntaxFactory.SeparatedList(new[] { arguments[0], arguments[2], arguments[1] })));
        });

        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void CanChooseToReenterTheSameRoute()
    {
        const string field = "EVT_ALEX3_AT_WEAPON";
        EVContainer original = Fixture(field);
        string project = Export(original, "cycle", field: field);
        var (_, choice) = FindChoice(project);
        string owner = choice.Ancestors().OfType<MethodDeclarationSyntax>().First().Identifier.ValueText;
        Assert.Contains(choice.ArgumentList.Arguments.Skip(1), arm => arm.Expression is InvocationExpressionSyntax entry &&
            entry.ArgumentList.Arguments[1].Expression is IdentifierNameSyntax target && target.Identifier.ValueText == owner);

        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void SignedSparseKeysCanShareARoute()
    {
        EVContainer original = Fixture();
        string project = Export(original, "choice");
        var (scenePath, choice) = FindChoice(project);
        var first = Assert.IsType<InvocationExpressionSyntax>(choice.ArgumentList.Arguments[1].Expression);
        var second = Assert.IsType<InvocationExpressionSyntax>(choice.ArgumentList.Arguments[2].Expression);
        string route = first.ArgumentList.Arguments[1].Expression.ToString();
        var sharedRoute = SyntaxFactory.ParseExpression($"play.Choose(() => Location.Int16_21, play.Case(-1, {route}), play.Case(1, {route}))");
        SyntaxNode scene = choice.SyntaxTree.GetRoot().ReplaceNode(choice, sharedRoute);
        var routes = Assert.Single(scene.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => IsCall(call, "Routes")));
        Write(scenePath, scene.ReplaceNode(routes, routes.WithArgumentList(routes.ArgumentList.AddArguments(second.ArgumentList.Arguments[1]))));

        string flat = Export(original, "flat", extract: false);
        string flatPath = Assert.Single(Directory.GetFiles(flat, "02_*.cs"));
        SyntaxNode root = Parse(flatPath);
        var selection = Assert.Single(root.DescendantNodes().OfType<SwitchStatementSyntax>().Where(node => node.Expression.ToString() == "Location.Byte_27"));
        var edited = SyntaxFactory.ParseStatement("""
            switch (Location.Int16_21)
            {
                case -1:
                    Location.Byte_24 = 2;
                    break;
                case 1:
                    Location.Byte_24 = 2;
                    break;
            }
            """);
        var loop = selection.Ancestors().OfType<MethodDeclarationSyntax>().First();
        var editedLoop = loop.ReplaceNode(selection, edited).AddAttributeLists(Layout("D9:21|-1B|1B", "Dense", "-1,0,1"));
        Write(flatPath, root.ReplaceNode(loop, editedLoop));

        Assert.Equal(EventProjectCompiler.Compile(flat).Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void AChoiceOnA24BitCellDoesNotUseAMaskingSwitchTable()
    {
        string project = Export(Fixture(), "choice");
        AddChoiceCell(project, "Int24");
        EditKeys(project, "ChoiceInput", [1, 3]);
        var (scene, choice) = FindChoice(project);
        SyntaxNode root = choice.SyntaxTree.GetRoot();
        var director = Assert.Single(root.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(method => method.DescendantNodes().OfType<InvocationExpressionSyntax>().Any(call => IsCall(call, "Routes"))));
        Write(scene, root.ReplaceNode(director, director.AddAttributeLists(Layout("C9:72|1B|3B", "Explicit", "1,3"))));

        // The explicit table would truncate 65537 into case 1. Dense tables compare the full map cell value.
        EVInstruction table = ChoiceTable(project);
        Assert.Equal(Jsm.Opcode.JMP_SWITCH, table.Opcode);
        Assert.Equal((ushort)1, Assert.Single(table.CaseValues));
        Assert.Equal(table.Targets[0], table.Targets[2]);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData("Int16", -32768, -32513, Jsm.Opcode.rsv0d, 32768)]
    [InlineData("UInt16", 65534, 65535, Jsm.Opcode.JMP_SWITCHEX, 65534)]
    public void ChoiceUsesAnEncodingThatFitsBoundaryKeys(string type, int first, int last, Jsm.Opcode expected, int firstEncoded)
    {
        string project = Export(Fixture(), "choice");
        AddChoiceCell(project, type);
        EditKeys(project, "ChoiceInput", [first, last]);

        EVInstruction table = ChoiceTable(project);
        Assert.Equal(expected, table.Opcode);
        Assert.Equal(firstEncoded, table.CaseValues[0]);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void TwoHundredFiftySixKeysUseAWideChoiceTable()
    {
        string project = Export(Fixture(), "choice");
        EditKeys(project, "Byte_27", Enumerable.Range(0, 256).ToArray());

        EVInstruction table = ChoiceTable(project);
        Assert.Equal(Jsm.Opcode.rsv0d, table.Opcode);
        Assert.Equal(257, table.Targets.Length);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData("play.Choose(() => Location.Byte_27)", "at least one inline Case")]
    [InlineData("play.Choose(() => Location.Byte_27, play.Case(0, TARGET), play.Case(0, TARGET))", "unique constant integer keys")]
    [InlineData("play.Choose(() => Location.Byte_27, play.Case(Location.Byte_27, TARGET))", "unique constant integer keys")]
    [InlineData("play.Choose(() => Location.Byte_27 + 1, play.Case(0, TARGET))", "writable map storage property")]
    [InlineData("play.Choose(() => Location.Byte_27, play.Case(70000, TARGET), play.Case(1, OTHER))", "fit the selected map cell")]
    public void RejectsInvalidChoiceDeclarations(string expression, string diagnostic)
    {
        string project = Export(Fixture(), "choice");
        EditChoice(project, choice =>
        {
            var first = Assert.IsType<InvocationExpressionSyntax>(choice.ArgumentList.Arguments[1].Expression);
            var second = Assert.IsType<InvocationExpressionSyntax>(choice.ArgumentList.Arguments[2].Expression);
            return SyntaxFactory.ParseExpression(expression.Replace("TARGET", first.ArgumentList.Arguments[1].Expression.ToString(), StringComparison.Ordinal)
                .Replace("OTHER", second.ArgumentList.Arguments[1].Expression.ToString(), StringComparison.Ordinal));
        });

        Assert.Contains(diagnostic, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
        Assert.Contains(diagnostic, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project)).Message);
    }

    [Theory]
    [InlineData(-32769, 0)]
    [InlineData(1, 65537)]
    public void RejectsUnencodableChoicesOn24BitCells(int first, int last)
    {
        string project = Export(Fixture(), "choice");
        AddChoiceCell(project, "Int24");
        EditKeys(project, "ChoiceInput", [first, last]);

        Assert.Contains("supported event switch table", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
        Assert.Contains("supported event switch table", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project)).Message);
    }

    private static AttributeListSyntax Layout(string key, string table, string order)
    {
        var declaration = (MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($$"""
            [SwitchLayout("{{key}}", SwitchTable.{{table}}, "{{order}}", false)]
            void Layout() { }
            """)!;
        return declaration.AttributeLists[0];
    }

    private static void AddChoiceCell(string project, string type)
    {
        string file = Path.Combine(project, "Event.cs");
        SyntaxNode root = Parse(file);
        var location = Assert.Single(root.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(declaration => declaration.Identifier.ValueText == "Location"));
        var cell = SyntaxFactory.ParseMemberDeclaration($$"""
            [StorageView(StorageScope.Map, StorageType.{{type}}, 72)]
            public int ChoiceInput
            {
                get => ScriptMemory.Read(StorageScope.Map, StorageType.{{type}}, 72);
                set => ScriptMemory.Write(StorageScope.Map, StorageType.{{type}}, 72, value);
            }
            """)!;
        Write(file, root.ReplaceNode(location, location.AddMembers(cell)));
    }

    private static void EditKeys(string project, string selector, int[] keys)
    {
        EditChoice(project, choice =>
        {
            var first = Assert.IsType<InvocationExpressionSyntax>(choice.ArgumentList.Arguments[1].Expression);
            var second = Assert.IsType<InvocationExpressionSyntax>(choice.ArgumentList.Arguments[2].Expression);
            string cases = string.Join(", ", keys.Select((key, index) => $"play.Case({key}, {(index == keys.Length - 1 ? second : first).ArgumentList.Arguments[1].Expression})"));
            return SyntaxFactory.ParseExpression($"play.Choose(() => Location.{selector}, {cases})");
        });
    }

    private static EVInstruction ChoiceTable(string project)
    {
        EVContainer field = EventProjectCompiler.Compile(project);
        EVContainerScript director = field.Objects[2].Scripts.Single(script => script.Id == 1);
        EVInstruction[] switches = EVInstructionBody.Decode(director.Body).Instructions
            .Where(instruction => instruction.Opcode is Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d).ToArray();
        Assert.Equal(2, switches.Length);
        return switches[1];
    }

    private static EVContainer Fixture(string field = Field)
    {
        using Stream stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", field + ".eb.bytes"));
        return EVContainer.Read(stream);
    }

    private string Export(EVContainer container, string leaf, bool extract = true, string field = Field)
    {
        string project = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(container, field, project, useCutsceneNames: false, extractCutscenes: extract);
        return project;
    }

    private static void EditChoice(string project, Func<InvocationExpressionSyntax, ExpressionSyntax> edit)
    {
        var (scene, choice) = FindChoice(project);
        Write(scene, choice.SyntaxTree.GetRoot().ReplaceNode(choice, edit(choice)));
    }

    private static (string Path, InvocationExpressionSyntax Call) FindChoice(string project)
    {
        var declarations = Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs")
            .SelectMany(path => Parse(path).DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Where(call => IsCall(call, "Choose"))
                .Select(call => (Path: path, Call: call)));
        return Assert.Single(declarations);
    }

    private static bool IsCall(InvocationExpressionSyntax call, string name) =>
        call.Expression is MemberAccessExpressionSyntax member && member.Name.Identifier.ValueText == name;

    private static SyntaxNode Parse(string path) => CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();

    private static void Write(string path, SyntaxNode root) => File.WriteAllText(path, root.NormalizeWhitespace().ToFullString());

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }
}
