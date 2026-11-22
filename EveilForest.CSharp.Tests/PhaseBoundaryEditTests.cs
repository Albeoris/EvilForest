using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class PhaseBoundaryEditTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-owned-case-").FullName;

    [Theory]
    [InlineData("EVT_DALI_V_DL_SHP", 11, "Scene_03_01_At45", 30, 32, "messageId", 357, 1357, Jsm.Opcode.MES, 2)]
    [InlineData("EVT_EVA1_IF_ENT_0", 19, "Scene_19_01_At1", 17, 20, "speed", 16, 19, Jsm.Opcode.TURNA, 1)]
    public void ACaseWithAnAdjacentPredecessorRetainsItsOwnEditableScenePhase(string field, int slot, string scene, int predecessor, int phase, string argument, int originalValue, int newValue, Jsm.Opcode opcode, int operand)
    {
        byte[] bytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", field + ".eb.bytes"));
        using var input = new MemoryStream(bytes);
        EVContainer fixture = EVContainer.Read(input);
        string project = SourceProjectTest.CopyWithoutComments(Export(fixture, field, "source"));
        string scenePath = Path.Combine(project, "Cutscenes", scene + ".cs");
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(scenePath)).GetRoot();
        MethodDeclarationSyntax ownedPhase = AssertDispatch(project, slot, scene, predecessor, phase, syntax);
        Assert.Equal(bytes, EventProjectCompiler.Compile(project).Serialize());
        CheckIndependentCallEdits(project, slot, predecessor, phase, opcode, operand, originalValue);

        ArgumentSyntax selected = Assert.Single(ownedPhase.DescendantNodes().OfType<ArgumentSyntax>().Where(candidate =>
            candidate.NameColon?.Name.Identifier.ValueText == argument && candidate.Expression is LiteralExpressionSyntax { Token.Value: int value } && value == originalValue));
        syntax = syntax.ReplaceNode(selected.Expression, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(newValue)).WithTriviaFrom(selected.Expression));
        File.WriteAllText(scenePath, syntax.ToFullString());

        EVContainerScript script = Assert.Single(fixture.Objects[slot].Scripts.Where(entry => entry.Id == 1));
        EVInstructionBody expected = EVInstructionBody.Decode(script.Body);
        EVInstruction table = expected.Instructions[1];
        Assert.Equal(Jsm.Opcode.JMP_SWITCHEX, table.Opcode);
        int phaseIndex = Array.IndexOf(table.CaseValues, (ushort)phase);
        Assert.True(phaseIndex >= 0);
        EVInstruction first = Assert.Single(expected.Instructions.Where(instruction => instruction.Label == table.Targets[phaseIndex + 1]));
        Assert.Equal(opcode, first.Opcode);
        Assert.Equal(originalValue, first.Operands[operand].Value);
        first.Operands[operand] = new(newValue);
        script.Body = expected.Encode();

        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(fixture.Serialize(), edited.Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        string reopened = Export(edited, field, "reopened");
        SyntaxNode restoredScene = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(reopened, "Cutscenes", scene + ".cs"))).GetRoot();
        AssertDispatch(reopened, slot, scene, predecessor, phase, restoredScene);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    private static MethodDeclarationSyntax AssertDispatch(string project, int slot, string scene, int predecessor, int phase, SyntaxNode sceneSyntax)
    {
        string file = Assert.Single(Directory.GetFiles(project, $"{slot:D2}_*.cs"));
        string participant = Path.GetFileNameWithoutExtension(file)[3..];
        var root = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();
        MethodDeclarationSyntax loop = Assert.Single(root.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Identifier.ValueText == "OnLoop"));
        string call = $"await ctx.{scene}.Phase{phase}(this);";
        SwitchSectionSyntax continuation = CaseSection(loop, phase);
        SwitchStatementSyntax selection = Assert.IsType<SwitchStatementSyntax>(continuation.Parent);
        SwitchSectionSyntax previous = CaseSection(selection, predecessor);
        Assert.Equal(selection.Sections.IndexOf(previous) + 1, selection.Sections.IndexOf(continuation));
        foreach (SwitchSectionSyntax section in new[] { previous, continuation })
        {
            Assert.Equal(call, section.Statements[^2].ToString());
            Assert.IsType<BreakStatementSyntax>(section.Statements[^1]);
        }
        Assert.Empty(selection.DescendantNodes().OfType<GotoStatementSyntax>());
        return Assert.Single(sceneSyntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method =>
            method.Identifier.ValueText == $"Phase{phase}" && method.ParameterList.Parameters is [var parameter] && parameter.Type?.ToString() == participant));
    }

    private static void CheckIndependentCallEdits(string project, int slot, int predecessor, int phase, Jsm.Opcode opcode, int operand, int originalValue)
    {
        const int editedFrames = 71;
        string file = Assert.Single(Directory.GetFiles(project, $"{slot:D2}_*.cs"));
        string original = File.ReadAllText(file);
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(original).GetRoot();
        MethodDeclarationSyntax loop = Assert.Single(syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Identifier.ValueText == "OnLoop"));

        foreach (var (edited, retained) in new[] { (predecessor, phase), (phase, predecessor) })
        {
            StatementSyntax call = Assert.IsType<ExpressionStatementSyntax>(CaseSection(loop, edited).Statements[^2]);
            StatementSyntax replacement = SyntaxFactory.ParseStatement($"await Engine.Wait(frames: {editedFrames});").WithTriviaFrom(call);
            File.WriteAllText(file, syntax.ReplaceNode(call, replacement).ToFullString());

            EVContainer output = EventProjectCompiler.Compile(project);
            EVInstructionBody body = EVInstructionBody.Decode(Assert.Single(output.Objects[slot].Scripts.Where(script => script.Id == 1)).Body);
            EVInstruction table = body.Instructions[1];
            EVInstruction[] editedArm = Arm(edited);
            EVInstruction[] retainedArm = Arm(retained);
            Assert.Single(editedArm.Where(IsEditedWait));
            Assert.DoesNotContain(editedArm, IsOriginalAction);
            Assert.Contains(retainedArm, IsOriginalAction);
            Assert.DoesNotContain(retainedArm, IsEditedWait);
            Assert.DoesNotContain(Arm(predecessor), step => step.Targets.Contains(Arm(phase)[0].Label));
            Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

            EVInstruction[] Arm(int value)
            {
                int start = table.Targets[1 + Array.IndexOf(table.CaseValues, (ushort)value)];
                int end = table.Targets.Where(target => target > start).Min();
                return body.Instructions.Where(step => step.Label >= start && step.Label < end).ToArray();
            }
            bool IsEditedWait(EVInstruction step) => step.Opcode == Jsm.Opcode.WAIT && step.Operands[0].Value == editedFrames;
            bool IsOriginalAction(EVInstruction step) => step.Opcode == opcode && step.Operands[operand].Value == originalValue;
        }
        File.WriteAllText(file, original);
    }

    private static SwitchSectionSyntax CaseSection(SyntaxNode root, int value) =>
        Assert.Single(root.DescendantNodes().OfType<SwitchSectionSyntax>().Where(section => section.Labels.Any(label => label.ToString() == $"case {value}:")));

    private string Export(EVContainer fixture, string field, string leaf)
    {
        string project = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(fixture, field, project, useCutsceneNames: false);
        return project;
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
