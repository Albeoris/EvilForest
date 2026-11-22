using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed partial class ConditionalContinuationEditTests : IDisposable
{
    private const string Kitchen = "EVT_SARI1_MS_KTN_0";
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-conditional-continuation-").FullName;

    [Fact]
    public void EditingEikosKitchenConversationPreservesBothConditionalContinuations()
    {
        byte[] bytes = ReadFixture(Kitchen);
        using var stream = new MemoryStream(bytes);
        EVContainer fixture = EVContainer.Read(stream);
        string project = Export(fixture, Kitchen, "kitchen");
        string path = Directory.GetFiles(project, "36_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        MethodDeclarationSyntax speak = syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "OnSpeak");

        Assert.Empty(speak.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(bytes, EventProjectCompiler.Compile(project).Serialize());

        var guard = speak.DescendantNodes().OfType<BinaryExpressionSyntax>().Single(binary => binary.ToString() == "Location.SByte_35 == 1");
        var fallback = speak.DescendantNodes().OfType<LiteralExpressionSyntax>().Single(literal => literal.Token.Value is 470);
        syntax = syntax.ReplaceNodes(new[] { (LiteralExpressionSyntax)guard.Right, fallback }, (literal, _) =>
            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(literal == fallback ? 1470 : 2)).WithTriviaFrom(literal));
        File.WriteAllText(path, syntax.ToFullString());

        EVContainer rebuilt = EventProjectCompiler.Compile(project);
        EVContainerScript originalSpeak = Assert.Single(fixture.Objects[36].Scripts.Where(script => script.Id == 3));
        EVInstructionBody expected = EVInstructionBody.Decode(originalSpeak.Body);
        var instructions = expected.Instructions.ToDictionary(instruction => instruction.Label);
        Assert.Equal(Jsm.Opcode.JMP_IFN, instructions[0x0010].Opcode);
        Assert.Equal(0x01A2, Assert.Single(instructions[0x0010].Targets));
        Assert.Equal(0x02A9, Assert.Single(instructions[0x019F].Targets));
        Assert.Equal(0x026E, Assert.Single(instructions[0x01EA].Targets));
        Assert.Equal(0x026B, Assert.Single(instructions[0x0209].Targets));
        Assert.Equal(0x026B, Assert.Single(instructions[0x0228].Targets));
        Assert.Equal(0x02A9, Assert.Single(instructions[0x026B].Targets));
        Assert.Equal(Jsm.Opcode.Return, instructions[0x02A9].Opcode);

        Assert.Equal(new EVVariable(EVStorageScope.Map, EVStorageType.SByte, 35).Encode(), instructions[0x0008].Expression[0]);
        Assert.Equal(EVExpressionToken.Literal(1), instructions[0x0008].Expression[1]);
        instructions[0x0008].Expression[1] = EVExpressionToken.Literal(2);
        Assert.Equal(470, instructions[0x026E].Operands[2].Value);
        instructions[0x026E].Operands[2] = new(1470);
        originalSpeak.Body = expected.Encode();

        Assert.Equal(fixture.Serialize(), rebuilt.Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        string reopened = Export(rebuilt, Kitchen, "kitchen-edited");
        Assert.Equal(rebuilt.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void RetargetingOneOfEikosKitchenLoopBranchesChangesOnlyThatEdge()
    {
        byte[] bytes = ReadFixture(Kitchen);
        using var stream = new MemoryStream(bytes);
        EVContainer fixture = EVContainer.Read(stream);
        string project = Export(fixture, Kitchen, "eiko-loop");
        string path = Directory.GetFiles(project, "36_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        MethodDeclarationSyntax loop = syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "OnLoop");
        var transfers = loop.DescendantNodes().OfType<GotoStatementSyntax>().Where(jump => jump.IsKind(SyntaxKind.GotoCaseStatement) && jump.Expression?.ToString() == "94").ToArray();
        Assert.Equal(2, transfers.Length);
        GotoStatementSyntax conditional = Assert.Single(transfers.Where(jump => jump.Parent is IfStatementSyntax));
        Assert.Equal(bytes, EventProjectCompiler.Compile(project).Serialize());

        syntax = syntax.ReplaceNode(conditional.Expression!, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(95)).WithTriviaFrom(conditional.Expression!));
        File.WriteAllText(path, syntax.ToFullString());

        EVContainerScript originalLoop = Assert.Single(fixture.Objects[36].Scripts.Where(script => script.Id == 1));
        EVInstructionBody expected = EVInstructionBody.Decode(originalLoop.Body);
        var instructions = expected.Instructions.ToDictionary(instruction => instruction.Label);
        EVInstruction switchTable = expected.Instructions[1];
        int case94 = switchTable.Targets[Array.IndexOf(switchTable.CaseValues, (ushort)94) + 1];
        int case95 = switchTable.Targets[Array.IndexOf(switchTable.CaseValues, (ushort)95) + 1];
        Assert.Equal(0x1AE0, case94);
        Assert.Equal(0x1AFE, case95);
        Assert.Equal(Jsm.Opcode.JMP_IFN, instructions[0x1A20].Opcode);
        Assert.Equal(case94, Assert.Single(instructions[0x1A20].Targets));
        Assert.Equal(Jsm.Opcode.JMP, instructions[0x1A86].Opcode);
        Assert.Equal(case94, Assert.Single(instructions[0x1A86].Targets));
        instructions[0x1A20].Targets = [case95];
        originalLoop.Body = expected.Encode();

        EVContainer rebuilt = EventProjectCompiler.Compile(project);
        Assert.Equal(fixture.Serialize(), rebuilt.Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        string reopened = Export(rebuilt, Kitchen, "eiko-loop-edited");
        Assert.Equal(rebuilt.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void RetargetingAFossilRooCaseUpdatesTheInnerSwitchExits()
    {
        const string eventName = "EVT_FOSSIL_FR_DN3_0";
        byte[] bytes = ReadFixture(eventName);
        using var stream = new MemoryStream(bytes);
        EVContainer fixture = EVContainer.Read(stream);
        string project = Export(fixture, eventName, "fossil-roo");
        string path = Directory.GetFiles(project, "22_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        MethodDeclarationSyntax handler = syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "Script_26");
        SwitchStatementSyntax outer = Assert.Single(handler.DescendantNodes().OfType<SwitchStatementSyntax>().Where(selection => selection.Expression.ToString() == "Location.UInt16_53"));
        SwitchSectionSyntax arm = Assert.Single(outer.Sections.Where(section => section.Labels.Any(label => label.ToString() == "case 137:")));
        SwitchSectionSyntax originalDestination = Assert.Single(outer.Sections.Where(section => section.Labels.Any(label => label.ToString() == "case 154:")));
        Assert.IsType<BreakStatementSyntax>(arm.Statements[^1]);
        Assert.True(SyntaxFactory.AreEquivalent(originalDestination.Statements[0], arm.Statements[^2]));
        Assert.Equal(bytes, EventProjectCompiler.Compile(project).Serialize());

        StatementSyntax exit = SyntaxFactory.ParseStatement("goto case 171;").WithTriviaFrom(arm.Statements[^2]);
        syntax = syntax.ReplaceNode(arm, arm.WithStatements(SyntaxFactory.List(arm.Statements.SkipLast(2).Append(exit))));
        File.WriteAllText(path, syntax.ToFullString());

        EVContainerScript original = Assert.Single(fixture.Objects[22].Scripts.Where(script => script.Id == 26));
        EVInstructionBody expected = EVInstructionBody.Decode(original.Body);
        var instructions = expected.Instructions.ToDictionary(instruction => instruction.Label);
        EVInstruction outerTable = instructions[0x0118];
        int oldTarget = outerTable.Targets[Array.IndexOf(outerTable.CaseValues, (ushort)154) + 1];
        int newTarget = outerTable.Targets[Array.IndexOf(outerTable.CaseValues, (ushort)171) + 1];
        Assert.Equal(0x1A36, oldTarget);
        Assert.Equal(0x1C27, newTarget);
        Assert.Equal(Jsm.Opcode.JMP_SWITCHEX, instructions[0x16EF].Opcode);
        Assert.Equal(oldTarget, instructions[0x16EF].Targets[0]);
        instructions[0x16EF].Targets[0] = newTarget;
        foreach (int jump in new[] { 0x171A, 0x1875, 0x1888, 0x19E3, 0x1A33 })
        {
            Assert.Equal(Jsm.Opcode.JMP, instructions[jump].Opcode);
            Assert.Equal(oldTarget, Assert.Single(instructions[jump].Targets));
            instructions[jump].Targets = [newTarget];
        }
        original.Body = expected.Encode();

        EVContainer rebuilt = EventProjectCompiler.Compile(project);
        Assert.Equal(fixture.Serialize(), rebuilt.Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        string reopened = Export(rebuilt, eventName, "fossil-roo-edited");
        Assert.Equal(rebuilt.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void EditingTheUnenteredPrefixOfQuinasRouteKeepsItOutsideTheDispatchTargets()
    {
        const string eventName = "EVT_KUINA_KM_SWP_1";
        byte[] bytes = ReadFixture(eventName);
        using var stream = new MemoryStream(bytes);
        EVContainer fixture = EVContainer.Read(stream);
        string project = Export(fixture, eventName, "quina-prefix");
        string path = Directory.GetFiles(project, "17_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        MethodDeclarationSyntax handler = syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "Script_17");
        SwitchStatementSyntax dispatch = Assert.Single(handler.DescendantNodes().OfType<SwitchStatementSyntax>());
        IfStatementSyntax prefix = Assert.IsType<IfStatementSyntax>(dispatch.Sections[0].Statements[0]);
        Assert.True(prefix.Condition.IsKind(SyntaxKind.FalseLiteralExpression));
        LiteralExpressionSyntax value = Assert.Single(prefix.Statement.DescendantNodes().OfType<LiteralExpressionSyntax>());
        Assert.Equal(200, value.Token.Value);
        Assert.Empty(handler.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(bytes, EventProjectCompiler.Compile(project).Serialize());

        syntax = syntax.ReplaceNode(value, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(201)).WithTriviaFrom(value));
        File.WriteAllText(path, syntax.ToFullString());

        EVContainerScript original = Assert.Single(fixture.Objects[17].Scripts.Where(script => script.Id == 17));
        EVInstructionBody expected = EVInstructionBody.Decode(original.Body);
        Assert.Equal(0, expected.Instructions[0].Label);
        Assert.Equal(Jsm.Opcode.JMP_SWITCH, expected.Instructions[1].Opcode);
        Assert.Equal(0x001C, expected.Instructions[1].Targets[1]);
        Assert.DoesNotContain(expected.GetSuccessors().Values, successors => successors.Contains(0x0014));
        Assert.Equal(0x0014, expected.Instructions[2].Label);
        Assert.Equal(EVExpressionToken.Literal(200), expected.Instructions[2].Expression[1]);
        expected.Instructions[2].Expression[1] = EVExpressionToken.Literal(201);
        original.Body = expected.Encode();

        EVContainer rebuilt = EventProjectCompiler.Compile(project);
        Assert.Equal(fixture.Serialize(), rebuilt.Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        string reopened = Export(rebuilt, eventName, "quina-prefix-edited");
        Assert.Equal(rebuilt.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AJumpIntoADispatchPrefixPreventsTheUnreachableLayout(bool entered)
    {
        EVInstruction[] instructions =
        [
            Read(0, 0),
            new(1, Jsm.Opcode.JMP_SWITCH) { CaseValues = [0], Targets = [8, 4, 6] },
            Marker(2, 99),
            Marker(3, 100),
            Marker(4, 10),
            Branch(5, Jsm.Opcode.JMP, 8),
            Marker(6, 20),
            Branch(7, Jsm.Opcode.JMP, entered ? 2 : 8),
            Marker(8, 30),
            new(9, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "SwitchPrefixFixture", "switch-prefix");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        string source = File.ReadAllText(path);
        Assert.Equal(!entered, source.Contains("HasUnreachablePrefix = true", StringComparison.Ordinal));
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, baseline, (0, 0, 0, [10, 30]), (1, 0, 0, entered ? [20, 99, 100, 10, 30] : [20, 30]), (2, 0, 0, [30]));

        if (!entered)
        {
            Assert.Contains("if (false)", source);
            File.WriteAllText(path, source.Replace("if (false)", "if (true)", StringComparison.Ordinal));
            EVContainer edited = EventProjectCompiler.Compile(project);
            ExpectPaths(project, edited, (0, 0, 0, [99, 100, 10, 30]), (1, 0, 0, [20, 30]), (2, 0, 0, [30]));
            string reopened = Export(edited, "SwitchPrefixFixture", "switch-prefix-edited");
            Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        }
    }

    [Fact]
    public void InsertingWorkInTheLastAlternativePreservesTheSharedContinuation()
    {
        EVInstruction[] instructions =
        [
            Read(0, 0),
            Branch(1, Jsm.Opcode.JMP_IFN, 12),
            Read(2, 1),
            Branch(3, Jsm.Opcode.JMP_IFN, 6),
            Marker(4, 10),
            Branch(5, Jsm.Opcode.JMP, 11),
            Read(6, 2),
            Branch(7, Jsm.Opcode.JMP_IFN, 10),
            Marker(8, 20),
            Branch(9, Jsm.Opcode.JMP, 11),
            Marker(10, 30),
            Branch(11, Jsm.Opcode.JMP, 13),
            Marker(12, 40),
            Marker(13, 50),
            new(14, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "ConditionalContinuationFixture", "synthetic");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        string source = File.ReadAllText(path);

        Assert.DoesNotContain("goto ", source);
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, baseline, (1, 0, 0, [30, 50]), (1, 0, 1, [20, 50]), (1, 1, 0, [10, 50]), (1, 1, 1, [10, 50]), (0, 1, 1, [40, 50]));

        const string statement = "Messages.SetTextVariable(variableId: 0, value: 40);";
        Assert.Contains(statement, source);
        File.WriteAllText(path, source.Replace(statement, "Messages.SetTextVariable(variableId: 0, value: 99);\n                " + statement, StringComparison.Ordinal));
        EVContainer edited = EventProjectCompiler.Compile(project);
        ExpectPaths(project, edited, (1, 0, 0, [30, 50]), (1, 0, 1, [20, 50]), (1, 1, 0, [10, 50]), (1, 1, 1, [10, 50]), (0, 1, 1, [99, 40, 50]));
        string reopened = Export(edited, "ConditionalContinuationFixture", "synthetic-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void EditingTransfersBetweenSwitchArmsFollowsTheBoundCaseAndDefault()
    {
        EVInstruction[] instructions =
        [
            Read(0, 0),
            new(1, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [1, 3, 5], Targets = [9, 2, 7, 7] },
            Marker(2, 5),
            Read(3, 1),
            Branch(4, Jsm.Opcode.JMP_IF, 7),
            Marker(5, 10),
            Branch(6, Jsm.Opcode.JMP, 9),
            Marker(7, 20),
            Branch(8, Jsm.Opcode.JMP, 11),
            Marker(9, 30),
            Branch(10, Jsm.Opcode.JMP, 11),
            Marker(11, 40),
            new(12, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "SwitchTransferFixture", "switch-transfer");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        GotoStatementSyntax[] transfers = syntax.DescendantNodes().OfType<GotoStatementSyntax>().ToArray();
        Assert.Equal(2, transfers.Length);
        GotoStatementSyntax conditional = Assert.Single(transfers.Where(jump => jump.IsKind(SyntaxKind.GotoCaseStatement)));
        Assert.IsType<IfStatementSyntax>(conditional.Parent);
        Assert.Single(transfers.Where(jump => jump.IsKind(SyntaxKind.GotoDefaultStatement)));
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, baseline, (1, 0, 0, [5, 10, 30, 40]), (1, 1, 0, [5, 20, 40]), (3, 0, 0, [20, 40]), (5, 0, 0, [20, 40]), (0, 0, 0, [30, 40]));

        syntax = syntax.ReplaceNodes(transfers, (jump, _) => SyntaxFactory.ParseStatement(jump == conditional ? "goto default;" : "goto case 5;").WithTriviaFrom(jump));
        File.WriteAllText(path, syntax.ToFullString());
        instructions[4].Targets = [9];
        instructions[6].Targets = [7];

        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(SingleHandler(instructions).Serialize(), edited.Serialize());
        ExpectPaths(project, edited, (1, 0, 0, [5, 10, 20, 40]), (1, 1, 0, [5, 30, 40]), (3, 0, 0, [20, 40]), (5, 0, 0, [20, 40]), (0, 0, 0, [30, 40]));
        string reopened = Export(edited, "SwitchTransferFixture", "switch-transfer-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void AnUnguardedCaseCanShareTheBodyOfAGuardedCase()
    {
        EVInstruction[] instructions =
        [
            Read(0, 0),
            new(1, Jsm.Opcode.JMP_SWITCH) { CaseValues = [0], Targets = [8, 2, 6] },
            Read(2, 1),
            Branch(3, Jsm.Opcode.JMP_IFN, 6),
            Marker(4, 10),
            Branch(5, Jsm.Opcode.JMP, 10),
            Marker(6, 20),
            Branch(7, Jsm.Opcode.JMP, 10),
            Marker(8, 30),
            Branch(9, Jsm.Opcode.JMP, 10),
            Marker(10, 40),
            new(11, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "MixedGuardedCaseFixture", "guarded-case-group");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        CasePatternSwitchLabelSyntax guard = Assert.Single(syntax.DescendantNodes().OfType<CasePatternSwitchLabelSyntax>());
        SwitchSectionSyntax section = Assert.IsType<SwitchSectionSyntax>(guard.Parent);
        var caseTwo = SyntaxFactory.CaseSwitchLabel(SyntaxFactory.ParseExpression("2"));
        syntax = syntax.ReplaceNode(section, section.WithLabels(section.Labels.Add(caseTwo)));
        File.WriteAllText(path, syntax.NormalizeWhitespace().ToFullString());

        instructions[1].Targets = [8, 2, 6, 4];
        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(SingleHandler(instructions).Serialize(), edited.Serialize());
        ExpectPaths(project, edited, (0, 0, 0, [20, 40]), (0, 1, 0, [10, 40]), (1, 0, 0, [20, 40]), (2, 0, 0, [10, 40]), (2, 1, 0, [10, 40]), (3, 0, 0, [30, 40]));
        string reopened = Export(edited, "MixedGuardedCaseFixture", "guarded-case-group-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void ATransferToAnUnguardedLabelSkipsANewGuardEvenWithTheOldFallThroughHint()
    {
        EVInstruction[] instructions =
        [
            Read(0, 1),
            Read(1, 0),
            new(2, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [0, 4, 1, 2, 3, 6], Targets = [7, 3, 3, 4, 4, 6, 9] },
            Marker(3, 10),
            Marker(4, 20),
            new(5, Jsm.Opcode.Return),
            Marker(6, 25),
            Marker(7, 30),
            Branch(8, Jsm.Opcode.JMP, 11),
            Marker(9, 35),
            Branch(10, Jsm.Opcode.JMP, 11),
            Marker(11, 40),
            new(12, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "GuardedFallThroughFixture", "guarded-fall-through");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        string source = File.ReadAllText(path);
        Assert.Contains("FallThroughSections = \"0,2\"", source);
        Assert.Contains("goto case 1;", source);
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        File.WriteAllText(path, source.Replace("case 2:", "case 2 when Location.Byte_1 == 0:", StringComparison.Ordinal));
        EVContainer edited = EventProjectCompiler.Compile(project);
        ExpectPaths(project, edited, (0, 1, 0, [10, 20]), (1, 0, 0, [20]), (2, 0, 0, [20]), (2, 1, 0, [30, 40]), (4, 0, 0, [10, 20]));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void EditingTheGuardOfAMergedCaseChangesWhichLabelsRunThePrefix(int prefixCase)
    {
        EVInstruction[] instructions =
        [
            Read(0, 0),
            new(1, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [0, 4, 1], Targets = [9, 2, 2, 7] },
            Marker(2, 5),
            Read(3, 1),
            Branch(4, Jsm.Opcode.JMP_IFN, 7),
            Marker(5, 10),
            Marker(6, 11),
            Marker(7, 20),
            Branch(8, Jsm.Opcode.JMP, 11),
            Marker(9, 30),
            Branch(10, Jsm.Opcode.JMP, 11),
            Marker(11, 40),
            new(12, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "MergedCaseGuardFixture", "merged-case-guard");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        string source = File.ReadAllText(path);
        Assert.Contains("MergedCaseSection = 0", source);
        Assert.DoesNotContain("goto ", source);
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, baseline, (0, 0, 0, [5, 20, 40]), (4, 1, 0, [5, 10, 11, 20, 40]), (1, 0, 0, [20, 40]), (5, 1, 0, [30, 40]));

        SyntaxNode syntax = CSharpSyntaxTree.ParseText(source).GetRoot();
        IfStatementSyntax guard = Assert.IsType<IfStatementSyntax>(syntax.DescendantNodes().OfType<SwitchSectionSyntax>().First().Statements[0]);
        ExpressionSyntax condition = SyntaxFactory.ParseExpression($"Location.Byte_0 == {prefixCase}").WithTriviaFrom(guard.Condition);
        File.WriteAllText(path, syntax.ReplaceNode(guard.Condition, condition).ToFullString());

        EVContainer edited = EventProjectCompiler.Compile(project);
        ExpectPaths(project, edited, (0, 0, 0, [20, 40]), (4, 1, 0, [20, 40]),
            (1, 1, 0, prefixCase == 1 ? [5, 10, 11, 20, 40] : [20, 40]), (5, 1, 0, [30, 40]));
        string reopened = Export(edited, "MergedCaseGuardFixture", "merged-case-guard-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void TwoValuesCanShareAGuardAndUseDifferentFallbacksAfterAnEdit()
    {
        EVInstruction[] instructions =
        [
            Read(0, 0),
            new(1, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [0, 4, 1], Targets = [8, 2, 2, 6] },
            Read(2, 1),
            Branch(3, Jsm.Opcode.JMP_IFN, 6),
            Marker(4, 10),
            Branch(5, Jsm.Opcode.JMP, 10),
            Marker(6, 20),
            Branch(7, Jsm.Opcode.JMP, 10),
            Marker(8, 30),
            Branch(9, Jsm.Opcode.JMP, 10),
            Marker(10, 40),
            new(11, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "MultipleGuardFallbackFixture", "multiple-guard-fallback");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        SwitchStatementSyntax selection = Assert.Single(syntax.DescendantNodes().OfType<SwitchStatementSyntax>());
        CasePatternSwitchLabelSyntax guard = Assert.Single(selection.DescendantNodes().OfType<CasePatternSwitchLabelSyntax>());
        Assert.Equal("0 or 4", guard.Pattern.ToString());
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        SwitchSectionSyntax firstFallback = selection.Sections[1];
        SwitchSectionSyntax secondFallback = selection.Sections[2];
        CaseSwitchLabelSyntax moved = Assert.Single(firstFallback.Labels.OfType<CaseSwitchLabelSyntax>().Where(label => label.Value.ToString() == "4"));
        var sections = selection.Sections.Replace(firstFallback, firstFallback.WithLabels(firstFallback.Labels.Remove(moved)));
        sections = sections.Replace(sections[2], secondFallback.WithLabels(secondFallback.Labels.Insert(0, moved)));
        File.WriteAllText(path, syntax.ReplaceNode(selection, selection.WithSections(sections)).NormalizeWhitespace().ToFullString());

        EVContainer edited = EventProjectCompiler.Compile(project);
        ExpectPaths(project, edited, (0, 0, 0, [20, 40]), (4, 0, 0, [30, 40]), (0, 1, 0, [10, 40]), (4, 1, 0, [10, 40]), (1, 0, 0, [20, 40]));
        string reopened = Export(edited, "MultipleGuardFallbackFixture", "multiple-guard-fallback-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Theory]
    [InlineData("goto default;")]
    [InlineData("break;")]
    public void ReplacingAnAdjacentCaseTransferOverridesTheFallThroughLayout(string replacement)
    {
        var selector = new EVVariable(EVStorageScope.Map, EVStorageType.Byte, 0).Encode();
        EVInstruction[] instructions =
        [
            new(0, Jsm.Opcode.EXPR) { Expression = [selector, EVExpressionToken.Literal(0), new((byte)Jsm.Excode.Add)] },
            new(1, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [1, 3], Targets = [6, 2, 4] },
            Marker(2, 10),
            Marker(4, 20),
            Branch(5, Jsm.Opcode.JMP, 8),
            Marker(6, 30),
            Branch(7, Jsm.Opcode.JMP, 8),
            Marker(8, 40),
            new(9, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "CaseFallThroughFixture", "case-fall-through");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        string source = File.ReadAllText(path);
        Assert.Contains("FallThroughSections = \"0\"", source);
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(source).GetRoot();
        SwitchSectionSyntax first = syntax.DescendantNodes().OfType<SwitchSectionSyntax>().First();
        LocalFunctionStatementSyntax local = Assert.Single(syntax.DescendantNodes().OfType<LocalFunctionStatementSyntax>());
        Assert.Equal(local.Identifier.ValueText + "();", first.Statements[^2].ToString());
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, baseline, (1, 0, 0, [10, 20, 40]), (3, 0, 0, [20, 40]), (0, 0, 0, [30, 40]));

        StatementSyntax replacementStatement = SyntaxFactory.ParseStatement(replacement).WithTriviaFrom(first.Statements[^2]);
        syntax = syntax.ReplaceNode(first, first.WithStatements(SyntaxFactory.List(first.Statements.SkipLast(2).Append(replacementStatement))));
        File.WriteAllText(path, syntax.ToFullString());
        bool usesDefault = replacement == "goto default;";
        EVInstruction omitted = Branch(3, Jsm.Opcode.JMP, usesDefault ? 6 : 8);
        EVContainer expected = SingleHandler(instructions.Take(3).Append(omitted).Concat(instructions.Skip(3)).ToArray());
        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(expected.Serialize(), edited.Serialize());
        ExpectPaths(project, edited, (1, 0, 0, usesDefault ? [10, 30, 40] : [10, 40]), (3, 0, 0, [20, 40]), (0, 0, 0, [30, 40]));
        string reopened = Export(edited, "CaseFallThroughFixture", "case-fall-through-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void ANewCaseTransferIntoAGroupedArmEvaluatesItsCondition()
    {
        EVInstruction[] instructions =
        [
            Read(0, 0),
            new(1, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [1, 3, 5], Targets = [7, 2, 3, 5] },
            Marker(2, 10),
            Marker(3, 20),
            Branch(4, Jsm.Opcode.JMP, 9),
            Marker(5, 25),
            Branch(6, Jsm.Opcode.JMP, 9),
            Marker(7, 30),
            Branch(8, Jsm.Opcode.JMP, 9),
            Marker(9, 40),
            new(10, Jsm.Opcode.Return)
        ];
        EVContainer baseline = SingleHandler(instructions);
        string project = Export(baseline, "GroupedCaseFixture", "grouped-case");
        string path = Directory.GetFiles(project, "00_*.cs").Single();
        string source = File.ReadAllText(path);
        Assert.Contains("MergedCaseSection = 0", source);
        Assert.DoesNotContain("goto ", source);
        Assert.Equal(baseline.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, baseline, (1, 0, 0, [10, 20, 40]), (3, 0, 0, [20, 40]), (5, 0, 0, [25, 40]), (0, 0, 0, [30, 40]));

        SyntaxNode syntax = CSharpSyntaxTree.ParseText(source).GetRoot();
        SwitchSectionSyntax changed = syntax.DescendantNodes().OfType<SwitchSectionSyntax>().Single(section => section.Labels[0].ToString() == "case 5:");
        var jump = SyntaxFactory.ParseStatement("goto case 1;").WithTriviaFrom(changed.Statements[^1]);
        syntax = syntax.ReplaceNode(changed.Statements[^1], jump);
        File.WriteAllText(path, syntax.ToFullString());

        EVContainer edited = EventProjectCompiler.Compile(project);
        ExpectPaths(project, edited, (1, 0, 0, [10, 20, 40]), (3, 0, 0, [20, 40]), (5, 0, 0, [25, 20, 40]), (0, 0, 0, [30, 40]));
        string reopened = Export(edited, "GroupedCaseFixture", "grouped-case-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void EditingTheGroupedEstGazaRouteKeepsTheSharedSuffix()
    {
        const string eventName = "EVT_EST_EG_EXT_0";
        byte[] bytes = ReadFixture(eventName);
        using var stream = new MemoryStream(bytes);
        EVContainer fixture = EVContainer.Read(stream);
        string project = Export(fixture, eventName, "est-gaza");
        string path = Directory.GetFiles(project, "02_*.cs").Single();
        string source = File.ReadAllText(path);
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(source).GetRoot();
        MethodDeclarationSyntax loop = syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "OnLoop");
        Assert.Empty(loop.DescendantNodes().OfType<GotoStatementSyntax>());
        SwitchSectionSyntax route = loop.DescendantNodes().OfType<SwitchSectionSyntax>().Single(section => section.Labels.Any(label => label.ToString() == "case 7:"));
        Assert.Contains(route.Labels, label => label.ToString() == "case 6:");
        Assert.Contains(route.Labels, label => label.IsKind(SyntaxKind.DefaultSwitchLabel));
        IfStatementSyntax prefix = Assert.IsType<IfStatementSyntax>(route.Statements[0]);
        Assert.Equal("Local.Byte_0 == 7", prefix.Condition.ToString());
        InvocationExpressionSyntax turn = prefix.DescendantNodes().OfType<InvocationExpressionSyntax>().Single(call => call.Expression.ToString() == "Actions.SetWalkTurnSpeed");
        ExpressionSyntax speed = Assert.Single(turn.ArgumentList.Arguments).Expression;
        Assert.Equal("6", speed.ToString());
        Assert.Equal(bytes, EventProjectCompiler.Compile(project).Serialize());

        syntax = syntax.ReplaceNode(speed, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(8)).WithTriviaFrom(speed));
        File.WriteAllText(path, syntax.ToFullString());

        EVContainerScript original = Assert.Single(fixture.Objects[2].Scripts.Where(script => script.Id == 1));
        EVInstructionBody expected = EVInstructionBody.Decode(original.Body);
        var instructions = expected.Instructions.ToDictionary(instruction => instruction.Label);
        Assert.Equal(Jsm.Opcode.JMP_SWITCH, instructions[0x0004].Opcode);
        Assert.Equal(0x006B, instructions[0x0004].Targets[8]);
        Assert.Equal(0x0080, instructions[0x0004].Targets[7]);
        Assert.Equal(0x0080, instructions[0x0004].Targets[0]);
        Assert.Equal(Jsm.Opcode.MROT, instructions[0x007D].Opcode);
        Assert.Equal(6, Assert.Single(instructions[0x007D].Operands).Value);
        instructions[0x007D].Operands[0] = new(8);
        original.Body = expected.Encode();

        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(fixture.Serialize(), edited.Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        string reopened = Export(edited, eventName, "est-gaza-edited");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    private string Export(EVContainer container, string eventName, string leaf)
    {
        string project = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(container, eventName, project, useCutsceneNames: false, extractCutscenes: false);
        return SourceProjectTest.CopyWithoutComments(project);
    }

    private static byte[] ReadFixture(string eventName) => File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));

    private static EVContainer SingleHandler(EVInstruction[] instructions)
    {
        byte[] body = new EVInstructionBody(instructions, instructions[^1].Label + 1).Encode();
        var root = new EVContainerObject(0, 0, 0, 0)
        {
            BodyOrder = 0,
            Type = (byte)ParticipantKind.Thread,
            Scripts = [new EVContainerScript(0, body)]
        };
        byte[] header = EVContainer.CreateHeader(EVContainer.SupportedVersion, new byte[EVContainer.HeaderSize - EVContainer.HeaderMetadataOffset]);
        header[3] = 1;
        return new EVContainer(header, [root], []);
    }

    private static EVInstruction Read(int label, int address) => new(label, Jsm.Opcode.EXPR) { Expression = [new EVVariable(EVStorageScope.Map, EVStorageType.Byte, address).Encode()] };

    private static EVInstruction Branch(int label, Jsm.Opcode opcode, int target) => new(label, opcode) { Targets = [target] };

    private static EVInstruction Marker(int label, int value) => new(label, Jsm.Opcode.MESVALUE) { Operands = [new(0), new(value)] };

    private static void ExpectPaths(string project, EVContainer container, params (byte First, byte Second, byte Third, int[] Expected)[] paths)
    {
        EVInstructionBody body = EVInstructionBody.Decode(Assert.Single(container.Objects[0].Scripts).Body);
        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        foreach (var (first, second, third, expected) in paths)
        {
            var location = new byte[80];
            location[0] = first;
            location[1] = second;
            location[2] = third;
            Assert.Equal(expected, RunNative(body, location));

            var host = new RecordingHost();
            var scheduler = new EventScheduler(host, new byte[4096], location);
            scheduler.RegisterProject(assembly);
            scheduler.StartScene();
            scheduler.Tick();
            Assert.Equal(expected, host.Values);
        }
    }

    private static int[] RunNative(EVInstructionBody body, byte[] location)
    {
        var instructions = body.Instructions.ToDictionary(instruction => instruction.Label);
        var values = new List<int>();
        int position = 0;
        int selector = 0;
        for (int step = 0; step < instructions.Count; step++)
        {
            EVInstruction instruction = instructions[position];
            position += instruction.OriginalLength;
            switch (instruction.Opcode)
            {
                case Jsm.Opcode.EXPR:
                    if (instruction.Expression is [var left, { Opcode: (byte)Jsm.Excode.Const16 } right, var operation])
                    {
                        int value = location[EVVariable.Decode(left).Address];
                        selector = (Jsm.Excode)operation.Opcode switch
                        {
                            Jsm.Excode.Equivalence => value == right.Value ? 1 : 0,
                            Jsm.Excode.Add => value + right.Value,
                            _ => throw new InvalidDataException($"Unexpected expression opcode {operation.Opcode}")
                        };
                        break;
                    }
                    EVExpressionToken token = Assert.Single(instruction.Expression);
                    if (token.Opcode == (byte)Jsm.Excode.Const16)
                    {
                        selector = token.Value;
                        break;
                    }
                    var variable = EVVariable.Decode(token);
                    Assert.Equal(EVStorageScope.Map, variable.Scope);
                    Assert.Equal(EVStorageType.Byte, variable.Type);
                    selector = location[variable.Address];
                    break;
                case Jsm.Opcode.JMP_IFN:
                    if (selector == 0)
                        position = Assert.Single(instruction.Targets);
                    break;
                case Jsm.Opcode.JMP_IF:
                    if (selector != 0)
                        position = Assert.Single(instruction.Targets);
                    break;
                case Jsm.Opcode.JMP:
                    position = Assert.Single(instruction.Targets);
                    break;
                case Jsm.Opcode.JMP_SWITCHEX:
                    position = instruction.Targets[Array.IndexOf(instruction.CaseValues, (ushort)selector) + 1];
                    break;
                case Jsm.Opcode.JMP_SWITCH:
                    int index = selector - (short)Assert.Single(instruction.CaseValues) + 1;
                    position = instruction.Targets[index > 0 && index < instruction.Targets.Length ? index : 0];
                    break;
                case Jsm.Opcode.MESVALUE:
                    values.Add(instruction.Operands[1].Value);
                    break;
                case Jsm.Opcode.Return:
                    return values.ToArray();
                default:
                    throw new InvalidDataException($"Unexpected opcode {instruction.Opcode}");
            }
        }
        throw new InvalidDataException("The expected path never returned.");
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);

    private sealed class RecordingHost : IEventHost
    {
        public List<int> Values { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Assert.Equal(Opcode.MESVALUE, opcode);
            Values.Add((int)operands[^1]);
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => throw new InvalidOperationException();

        public bool IsWindowOpen(int objectId, int windowId) => throw new InvalidOperationException();
    }
}
