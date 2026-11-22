using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class GuardedSwitchLayoutTests : IDisposable
{
    private const string EventName = "EVT_LIND2_TN_LB_STN_0";
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-guarded-switch-").FullName;

    [Fact]
    public void EditingTheAirCabGuardAndMovingItsFallbackPreservesTheSharedCase()
    {
        using var stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", EventName + ".eb.bytes"));
        EVContainer original = EVContainer.Read(stream);
        string project = Export(original, "source");
        string path = Directory.GetFiles(project, "03_*.cs").Single();
        SyntaxNode root = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        CasePatternSwitchLabelSyntax guard = Assert.Single(root.DescendantNodes().OfType<CasePatternSwitchLabelSyntax>());
        SwitchStatementSyntax selection = guard.FirstAncestorOrSelf<SwitchStatementSyntax>()!;
        Assert.Equal("Engine.ReadSystem(SystemVariable.DialogChoice)", selection.Expression.ToString());
        Assert.Equal("0", guard.Pattern.ToString());
        Assert.Equal(new[] { "case 0:", "case 1:" }, selection.Sections[1].Labels.Select(label => label.ToString()));
        Assert.Empty(selection.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        ExpressionSyntax predicate = guard.WhenClause!.Condition;
        SyntaxNode inverted = root.ReplaceNode(predicate, SyntaxFactory.ParseExpression($"!({predicate})").WithTriviaFrom(predicate));
        File.WriteAllText(path, inverted.ToFullString());
        EVContainerScript script = original.Objects[3].Scripts.Single(candidate => candidate.Id == 1);
        EVInstructionBody native = EVInstructionBody.Decode(script.Body);
        EVInstruction branch = GuardBranch(native, out _);
        Assert.Equal(Jsm.Opcode.JMP_IFN, branch.Opcode);
        branch.Opcode = Jsm.Opcode.JMP_IF;
        script.Body = native.Encode();
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        selection = Assert.Single(inverted.DescendantNodes().OfType<CasePatternSwitchLabelSyntax>()).FirstAncestorOrSelf<SwitchStatementSyntax>()!;
        var inserted = (SwitchStatementSyntax)SyntaxFactory.ParseStatement("switch (0) { case 2: Location.Byte_32 = 8; break; }");
        SwitchStatementSyntax moved = selection.WithSections(selection.Sections.Insert(1, inserted.Sections[0]));
        File.WriteAllText(path, inverted.ReplaceNode(selection, moved).NormalizeWhitespace().ToFullString());

        EVContainer rebuilt = EventProjectCompiler.Compile(project);
        EVInstructionBody edited = EVInstructionBody.Decode(rebuilt.Objects[3].Scripts.Single(candidate => candidate.Id == 1).Body);
        Assert.Equal(Jsm.Opcode.JMP_IF, GuardBranch(edited, out EVInstruction table).Opcode);
        Assert.Equal(4, table.Targets.Length);
        Assert.True(table.Targets[3] < table.Targets[2]);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        string reopened = Export(rebuilt, "reopened");
        Assert.Equal(rebuilt.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void EditingOneGuardForTwoMadainSariEntrancesPreservesTheNativeDispatch()
    {
        const string eventName = "EVT_SARI1_MS_ENT_0";
        using var stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));
        EVContainer original = EVContainer.Read(stream);
        string raw = Path.Combine(_directory, "madain-sari-entrance");
        EventProjectExporter.Export(original, eventName, raw, useCutsceneNames: false);
        string project = SourceProjectTest.CopyWithoutComments(raw);
        string path = Directory.GetFiles(project, "12_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        CasePatternSwitchLabelSyntax guard = Assert.Single(syntax.DescendantNodes().OfType<CasePatternSwitchLabelSyntax>());
        Assert.Equal("2 or 9999", guard.Pattern.ToString());
        Assert.Empty(guard.FirstAncestorOrSelf<SwitchStatementSyntax>()!.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        BinaryExpressionSyntax condition = Assert.IsType<BinaryExpressionSyntax>(guard.WhenClause!.Condition);
        Assert.Equal("6600", condition.Right.ToString());
        File.WriteAllText(path, syntax.ReplaceNode(condition.Right, SyntaxFactory.ParseExpression("6601").WithTriviaFrom(condition.Right)).ToFullString());

        EVContainerScript script = original.Objects[12].Scripts.Single(candidate => candidate.Id == 0);
        EVInstructionBody native = EVInstructionBody.Decode(script.Body);
        EVInstruction table = native.Instructions.First(instruction => instruction.Opcode == Jsm.Opcode.JMP_SWITCHEX);
        int entry = table.Targets[Array.IndexOf(table.CaseValues, (ushort)2) + 1];
        Assert.Equal(entry, table.Targets[Array.IndexOf(table.CaseValues, (ushort)9999) + 1]);
        EVInstruction comparison = native.Instructions.Single(instruction => instruction.Label == entry);
        Assert.Equal(EVExpressionToken.Literal(6600), comparison.Expression[1]);
        comparison.Expression[1] = EVExpressionToken.Literal(6601);
        script.Body = native.Encode();

        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    private static EVInstruction GuardBranch(EVInstructionBody body, out EVInstruction table)
    {
        table = body.Instructions.First(instruction => instruction.Opcode == Jsm.Opcode.JMP_SWITCH);
        Assert.Equal((ushort)0, Assert.Single(table.CaseValues));
        var instructions = body.Instructions.ToDictionary(instruction => instruction.Label);
        EVInstruction guard = instructions[table.Targets[1]];
        Assert.Equal(Jsm.Opcode.EXPR, guard.Opcode);
        EVInstruction branch = instructions[guard.Label + guard.OriginalLength];
        Assert.Equal(table.Targets[2], Assert.Single(branch.Targets));
        return branch;
    }

    private string Export(EVContainer container, string name)
    {
        string project = Path.Combine(_directory, name);
        EventProjectExporter.Export(container, EventName, project, useCutsceneNames: false);
        return SourceProjectTest.CopyWithoutComments(project);
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
