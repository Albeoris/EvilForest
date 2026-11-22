using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class SharedCaseActionEditTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-case-action-").FullName;

    [Fact]
    public void TheKitchenRoleKeepsItsEncodingAndAcceptsIndependentCaseEdits()
    {
        const string field = "EVT_SARI1_MS_KTN_0";
        byte[] original = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", field + ".eb.bytes"));
        string project = Path.Combine(_directory, "source");
        using var input = new MemoryStream(original);
        EventProjectExporter.Export(EVContainer.Read(input), field, project);
        project = SourceProjectTest.CopyWithoutComments(project);
        string file = Path.Combine(project, "36_Eiko.cs");
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();
        MethodDeclarationSyntax loop = syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "OnLoop");
        Assert.Contains(loop.AttributeLists.SelectMany(list => list.Attributes), attribute => attribute.Name.ToString() == "ShareSwitchCaseActions");
        Assert.Empty(loop.DescendantNodes().OfType<GotoStatementSyntax>());
        SwitchStatementSyntax selection = loop.DescendantNodes().OfType<SwitchStatementSyntax>().First();
        SwitchSectionSyntax forwarding = CaseSection(selection, "36");
        SwitchSectionSyntax destination = CaseSection(selection, "94");
        var action = Assert.IsType<ExpressionStatementSyntax>(destination.Statements[0]);
        BlockSyntax branch = Assert.Single(forwarding.DescendantNodes().OfType<BlockSyntax>().Where(block =>
            block.Statements is [var call, BreakStatementSyntax] && SyntaxFactory.AreEquivalent(call, action)));
        Assert.Equal(original, EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

        StatementSyntax marker = SyntaxFactory.ParseStatement("Messages.SetTextVariable(variableId: 0, value: 777);");
        Write(syntax.ReplaceNode(branch, branch.WithStatements(branch.Statements.Insert(0, marker))));
        EVInstruction[] editedCaller = ReadPollBody(project);
        int targetLabel = CaseLabel(editedCaller, 94);
        int markerIndex = Array.FindIndex(editedCaller, instruction => instruction is { Opcode: Jsm.Opcode.MESVALUE } && instruction.Operands[1].Value == 777);
        Assert.True(markerIndex >= 0);
        Assert.Equal(Jsm.Opcode.JMP, editedCaller[markerIndex + 1].Opcode);
        Assert.Equal(targetLabel, Assert.Single(editedCaller[markerIndex + 1].Targets));
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

        WhileStatementSyntax nestedLoop = SyntaxFactory.WhileStatement(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression), branch);
        Write(syntax.ReplaceNode(branch, SyntaxFactory.Block(nestedLoop, marker, SyntaxFactory.BreakStatement())));
        EVInstruction[] loopBody = ReadPollBody(project);
        int callerLabel = CaseLabel(loopBody, 36);
        targetLabel = CaseLabel(loopBody, 94);
        Assert.Single(loopBody.Where(instruction => instruction.Label >= callerLabel && instruction.Label < targetLabel && instruction.Targets.Contains(targetLabel)));
        Assert.Contains(loopBody, instruction => instruction.Label >= callerLabel && instruction.Label < targetLabel &&
            instruction.Opcode == Jsm.Opcode.MESVALUE && instruction.Operands[1].Value == 777);

        Write(syntax.ReplaceNode(destination, destination.WithStatements(destination.Statements.Insert(0, marker))));
        EVInstruction[] editedDestination = ReadPollBody(project);
        int forwardingLabel = CaseLabel(editedDestination, 36);
        targetLabel = CaseLabel(editedDestination, 94);
        Assert.Equal(777, editedDestination.Single(instruction => instruction.Label == targetLabel).Operands[1].Value);
        Assert.DoesNotContain(editedDestination, instruction => instruction.Label >= forwardingLabel && instruction.Label < targetLabel && instruction.Targets.Contains(targetLabel));
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

        EVContainer modified = EventProjectCompiler.Compile(project);
        string reopened = Path.Combine(_directory, "reopened");
        EventProjectExporter.Export(modified, field, reopened);
        Assert.Equal(modified.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());

        void Write(SyntaxNode root) => File.WriteAllText(file, root.NormalizeWhitespace().ToFullString());
    }

    private static SwitchSectionSyntax CaseSection(SwitchStatementSyntax selection, string value) =>
        Assert.Single(selection.Sections.Where(section => section.Labels.OfType<CaseSwitchLabelSyntax>().Any(label => label.Value.ToString() == value)));

    private static EVInstruction[] ReadPollBody(string project)
    {
        EVContainer compiled = EventProjectCompiler.Compile(project);
        return EVInstructionBody.Decode(compiled.Objects.Single(participant => participant.Id == 36).Scripts.Single(script => script.Id == 1).Body).Instructions.ToArray();
    }

    private static int CaseLabel(EVInstruction[] body, ushort value)
    {
        EVInstruction table = body.First(instruction => instruction is { Opcode: Jsm.Opcode.JMP_SWITCHEX } && instruction.CaseValues.Contains((ushort)94));
        return table.Targets[Array.IndexOf(table.CaseValues, value) + 1];
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
