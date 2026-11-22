using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed partial class ConditionalContinuationEditTests
{
    [Fact]
    public void RenamingTheLocalActionKeepsTheLayoutAndEarlyReturnResumesItsCaller()
    {
        EVInstruction[] instructions =
        [
            Read(0, 0),
            new(1, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [1, 3, 5], Targets = [6, 2, 3, 4] },
            Marker(2, 10),
            Marker(3, 20),
            Marker(4, 30),
            Branch(5, Jsm.Opcode.JMP, 8),
            Marker(6, 35),
            Branch(7, Jsm.Opcode.JMP, 8),
            Marker(8, 40),
            new(9, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "LocalCaseContinuationFixture", "local-case-continuation");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        LocalFunctionStatementSyntax action = syntax.DescendantNodes().OfType<LocalFunctionStatementSyntax>().First();
        string originalName = action.Identifier.ValueText;
        Assert.Empty(syntax.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, baseline, (1, 0, 0, [10, 20, 30, 40]), (3, 0, 0, [20, 30, 40]), (5, 0, 0, [30, 40]));

        syntax = syntax.ReplaceTokens(syntax.DescendantTokens().Where(token => token.IsKind(SyntaxKind.IdentifierToken) && token.ValueText == originalName),
            (token, _) => SyntaxFactory.Identifier("FinishSetup").WithTriviaFrom(token));
        File.WriteAllText(path, syntax.ToFullString());
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        action = syntax.DescendantNodes().OfType<LocalFunctionStatementSyntax>().Single(function => function.Identifier.ValueText == "FinishSetup");
        var earlyReturn = SyntaxFactory.ParseStatement("if (Location.Byte_0 == 1) { return; }");
        syntax = syntax.ReplaceNode(action, action.WithBody(action.Body!.WithStatements(action.Body.Statements.Insert(1, earlyReturn))));
        SwitchSectionSyntax first = syntax.DescendantNodes().OfType<SwitchSectionSyntax>().First();
        var marker = SyntaxFactory.ParseStatement("Messages.SetTextVariable(variableId: 0, value: 80);");
        syntax = syntax.ReplaceNode(first, first.WithStatements(first.Statements.Insert(first.Statements.Count - 1, marker)));
        File.WriteAllText(path, syntax.NormalizeWhitespace().ToFullString());

        EVContainer edited = EventProjectCompiler.Compile(project);
        ExpectPaths(project, edited, (1, 0, 0, [10, 20, 80, 40]), (3, 0, 0, [20, 30, 40]), (5, 0, 0, [30, 40]));
        string reopened = Export(edited, "LocalCaseContinuationFixture", "local-case-continuation-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Theory]
    [InlineData("{ { const int SongPlaySlot1 = 8; Record(); } return; void Record() { Messages.SetTextVariable(variableId: 0, value: SongPlaySlot1); } }",
        "captured name 'SongPlaySlot1' binds differently")]
    [InlineData("{ Outer(); return; void Outer() { const int SongPlaySlot1 = 8; Middle(); } void Middle() { Record(); } void Record() { Messages.SetTextVariable(variableId: 0, value: SongPlaySlot1); } }",
        "captured name 'SongPlaySlot1' binds differently")]
    [InlineData("{ Record(); return; async EventRoutine Record() { await Engine.Wait(frames: 2); } }", "Await an EventRoutine case continuation")]
    [InlineData("{ var routine = Record(); return; async EventRoutine Record() { await Engine.Wait(frames: 2); } }", "case continuation as a standalone statement")]
    [InlineData("{ Outer(); return; void Outer() { Record(); void Record() { Messages.SetTextVariable(variableId: 0, value: 9); } } }",
        "Declare case continuations directly")]
    public void AChangedBindingOrUnawaitedActionCannotSilentlyChangeExecution(string body, string diagnostic)
    {
        EVContainer baseline = SingleHandler([new(0, Jsm.Opcode.WAIT) { Operands = [new(2)] }, new(1, Jsm.Opcode.Return)]);
        string project = Export(baseline, "LocalCaseBindingFixture", "local-case-binding");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        MethodDeclarationSyntax method = syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(candidate => candidate.Identifier.ValueText == "OnInit");
        syntax = syntax.ReplaceNode(method.Body!, SyntaxFactory.ParseStatement(body));
        var file = (CompilationUnitSyntax)syntax;
        file = file.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("EvilForest.EventSource.SoundCommands")).WithStaticKeyword(SyntaxFactory.Token(SyntaxKind.StaticKeyword)));
        File.WriteAllText(path, file.NormalizeWhitespace().ToFullString());
        Assert.Contains(diagnostic, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
    }
}
