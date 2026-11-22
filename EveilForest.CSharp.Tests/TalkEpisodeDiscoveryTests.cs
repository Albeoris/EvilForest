using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class TalkEpisodeDiscoveryTests : IDisposable
{
    private const string Kitchen = "EVT_SARI2_MS_KTN_1";
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-talk-episodes-").FullName;

    [Fact]
    public void EachGuardOwnsItsExchangeAndTheCallbackKeepsFallbacksAndTheSharedReturn()
    {
        EVContainer field = Fixture(Kitchen);
        string baseline = Export(field, "baseline", extract: false);
        string project = SourceProjectTest.CopyWithoutComments(Export(field, "extracted"));
        string participant = Participant(project, 7);
        MethodDeclarationSyntax callback = Script(participant, 3);
        string talk = callback.ToString();
        Assert.Contains("if (Globals.Byte_289 == 0)", talk);
        Assert.Contains("if (Globals.Bit_2066 == false)", talk);
        Assert.Contains("if (Globals.Byte_290 == 1)", talk);
        Assert.Contains("await ctx.Scene_07_03_TalkEpisode1.Play(this);", talk);
        Assert.Contains("await ctx.Scene_07_03_TalkEpisode2.Play(this);", talk);
        foreach (int line in new[] { 91, 92, 107, 108 })
            Assert.Contains($"messageId: {line})", talk);

        Assert.Empty(callback.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Empty(callback.DescendantNodes().OfType<LabeledStatementSyntax>());
        Assert.IsType<ReturnStatementSyntax>(callback.Body!.Statements.Last());
        Assert.True(SyntaxFactory.AreEquivalent(Script(Participant(baseline, 7), 8), Script(participant, 8)));

        string first = File.ReadAllText(Episode(project, 1));
        string second = File.ReadAllText(Episode(project, 2));
        Assert.Contains("Globals.Bit_2066 = true;", first);
        Assert.Contains("Globals.Byte_290 = 2;", second);
        Assert.Contains("participant.Actions.TurnAtSpeed(angle: Location.Byte_16", second);
        Assert.Contains("Engine.EnableMove();", first);
        Assert.Contains("Engine.EnableMove();", second);
        Assert.DoesNotContain("goto ", first);
        Assert.DoesNotContain("goto ", second);
        foreach (int line in new[] { 87, 88, 89, 90 })
        {
            Assert.Contains($"messageId: {line})", first);
            Assert.DoesNotContain($"messageId: {line})", talk);
            Assert.DoesNotContain($"messageId: {line})", second);
        }
        foreach (int line in Enumerable.Range(99, 8))
        {
            Assert.Contains($"messageId: {line})", second);
            Assert.DoesNotContain($"messageId: {line})", talk);
            Assert.DoesNotContain($"messageId: {line})", first);
        }

        string otherActor = File.ReadAllText(Participant(project, 12));
        foreach (int tag in new[] { 15, 16, 17 })
        {
            string scene = tag == 15 ? first : second;
            Assert.Contains($"RunAndWait(On(evt.GEO_MAIN_F0_ZDN_12, Action_12_{tag}), level: 6)", scene);
            Assert.Contains($"[Script({tag},", scene);
            Assert.DoesNotContain($"[Script({tag},", otherActor);
        }

        EVContainer rebuilt = EventProjectCompiler.Compile(project);
        Assert.Equal(field.Serialize(), rebuilt.Serialize());
        var instructions = EVInstructionBody.Decode(TalkScript(rebuilt).Body).Instructions.ToDictionary(instruction => instruction.Label);
        Assert.Equal(Jsm.Opcode.Return, instructions[0x029B].Opcode);
        foreach (int exit in new[] { 0x00A3, 0x0178 })
        {
            Assert.Equal(Jsm.Opcode.JMP, instructions[exit].Opcode);
            Assert.Equal(0x029B, Assert.Single(instructions[exit].Targets));
        }
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void EditedDialogueAndStateSurviveReexportWithTheSameContinuation()
    {
        EVContainer field = Fixture(Kitchen);
        string baseline = Export(field, "baseline", extract: false);
        string project = SourceProjectTest.CopyWithoutComments(Export(field, "extracted"));
        Replace(Participant(baseline, 7), "messageId: 88)", "messageId: 188)");
        Replace(Participant(baseline, 7), "Globals.Byte_290 = 2;", "Globals.Byte_290 = 4;");
        Replace(Episode(project, 1), "messageId: 88)", "messageId: 188)");
        Replace(Episode(project, 2), "Globals.Byte_290 = 2;", "Globals.Byte_290 = 4;");

        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(EventProjectCompiler.Compile(baseline).Serialize(), edited.Serialize());
        string reopened = Export(edited, "reopened");
        Assert.Contains("messageId: 188)", File.ReadAllText(Episode(reopened, 1)));
        Assert.Contains("Globals.Byte_290 = 4;", File.ReadAllText(Episode(reopened, 2)));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(reopened));
    }

    [Fact]
    public void NonAdvancingGuardStaysInlineAndAnOutsideCallerKeepsItsAction()
    {
        string baseline = Export(Fixture(Kitchen), "baseline", extract: false);
        string participant = Participant(baseline, 7);
        MethodDeclarationSyntax talk = Script(participant, 3);
        string source = File.ReadAllText(participant);
        string editedTalk = talk.ToString().Replace("Globals.Bit_2066 = true;", "Globals.Bit_2066 = false;", StringComparison.Ordinal)
            .Replace("if (Globals.Byte_290 == 1)", """
                if (Globals.Byte_290 == 1)
                {
                    await scripts.RequestAndWait(evt.GEO_MAIN_F0_ZDN_12.Script_16, level: 6);
                }
                if (Globals.Byte_290 == 1)
                """, StringComparison.Ordinal);
        File.WriteAllText(participant, source[..talk.SpanStart] + editedTalk + source[talk.Span.End..]);

        EVContainer edited = EventProjectCompiler.Compile(baseline);
        string reopened = Export(edited, "reopened");
        string episode = File.ReadAllText(Episode(reopened, 2));
        string callback = Script(Participant(reopened, 7), 3).ToString();
        Assert.False(File.Exists(Episode(reopened, 1)));
        Assert.Contains("Globals.Bit_2066 = false;", callback);
        Assert.Contains("messageId: 88)", callback);
        Assert.Contains("messageId: 100)", episode);
        Assert.Contains("RequestAndWait(evt.GEO_MAIN_F0_ZDN_12.Script_16, level: 6)", callback);
        Assert.Contains("RequestAndWait(evt.GEO_MAIN_F0_ZDN_12.Script_16, level: 6)", episode);
        Assert.Contains("[Script(16,", File.ReadAllText(Participant(reopened, 12)));
        Assert.DoesNotContain("Action_12_16", episode);
        Assert.Contains("Action_12_17", episode);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(reopened));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AnUnchangedSiblingKeepsItsCatalogNameWhenOnlyTheFirstEpisodeStopsQualifying(bool silenceCallee)
    {
        EVContainer field = Fixture(Kitchen);
        string original = Export(field, "original");
        string fragment = Assert.Single(File.ReadLines(Path.Combine(original, "CutsceneOutline.tsv")).Select(row => row.Split('\t'))
            .Where(columns => columns[2] == "Scene_07_03_TalkEpisode2.Play (phase 2)"))[7];
        var catalog = new CutsceneCatalog([new(Kitchen, 7, 3, 2, "RememberAmarant", "LaniRemembersAmarant", fragment)]);
        string authored = Export(field, "authored", catalog: catalog);
        string sceneFile = Path.Combine("Cutscenes", "LaniRemembersAmarant.cs");
        string second = File.ReadAllText(Path.Combine(authored, sceneFile));
        Assert.Contains("RememberAmarant(", second);

        if (silenceCallee)
            Replace(Episode(authored, 1), "await Messages.ShowAndWait(windowId: 2, flags: 128, messageId: 87);", "");
        else
            Replace(Episode(authored, 1), "Globals.Bit_2066 = true;", "Globals.Bit_2066 = false;");

        EVContainer edited = EventProjectCompiler.Compile(authored);
        string unnamed = Export(edited, "unnamed");
        AssertOnlySecondMoves(edited, unnamed);
        Assert.Contains("await ctx.Scene_07_03_TalkEpisode2.Play(this);", Script(Participant(unnamed, 7), 3).ToString());

        string reopened = Export(edited, "reopened", catalog: catalog);
        Assert.Equal(second, File.ReadAllText(Path.Combine(reopened, sceneFile)));
        Assert.False(File.Exists(Episode(reopened, 1)));
        Assert.False(File.Exists(Episode(reopened, 2)));
        Assert.Contains("await ctx.LaniRemembersAmarant.RememberAmarant(this);", Script(Participant(reopened, 7), 3).ToString());
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(reopened));
    }

    [Fact]
    public void ARejectedOuterGuardStillLetsAnEligibleNestedEpisodeBeExtracted()
    {
        EVContainer edited = Fixture(Kitchen);
        EVContainerScript script = TalkScript(edited);
        EVInstructionBody body = EVInstructionBody.Decode(script.Body);
        var instructions = body.Instructions.ToList();
        int nestedGuard = instructions.FindIndex(instruction => instruction.Label == 0x0014);
        int nextEpisode = instructions.FindIndex(instruction => instruction.Label == 0x00A6);
        int fallbackLine = body.EndLabel + 1;
        int innerJoin = fallbackLine + 1;
        EVInstruction branch = instructions[nestedGuard + 1];
        EVInstruction exit = instructions[nextEpisode - 1];
        EVInstruction message = instructions.Single(instruction => instruction.Opcode == Jsm.Opcode.MES && instruction.Operands[2].Value == 91);
        Assert.Equal(Jsm.Opcode.JMP_IFN, branch.Opcode);
        Assert.Equal(0x00A6, Assert.Single(branch.Targets));
        Assert.Equal(Jsm.Opcode.JMP, exit.Opcode);
        Assert.Equal(0x029B, Assert.Single(exit.Targets));
        branch.Targets = [fallbackLine];
        exit.Targets = [innerJoin];
        instructions.InsertRange(nextEpisode,
        [
            new(fallbackLine, Jsm.Opcode.MES) { ArgumentMask = message.ArgumentMask, Operands = message.Operands },
            new(innerJoin, Jsm.Opcode.WAIT) { Operands = [new(2)] },
            new(innerJoin + 1, Jsm.Opcode.JMP) { Targets = [0x029B] }
        ]);
        instructions.InsertRange(nestedGuard,
        [
            new(innerJoin + 2, Jsm.Opcode.EXPR)
            {
                Expression = [new EVVariable(EVStorageScope.Global, EVStorageType.Bit, 2079).Encode(), EVExpressionToken.Literal(1), new((byte)Jsm.Excode.Equivalence)]
            },
            new(innerJoin + 3, Jsm.Opcode.JMP_IFN) { Targets = [0x00A6] }
        ]);
        script.Body = new EVInstructionBody(instructions, body.EndLabel).Encode();

        string project = Export(edited, "reopened");
        Assert.False(File.Exists(Episode(project, 1)));
        Assert.Contains("messageId: 88)", File.ReadAllText(Episode(project, 2)));
        Assert.Contains("messageId: 100)", File.ReadAllText(Episode(project, 3)));
        string callback = Script(Participant(project, 7), 3).ToString();
        Assert.Contains("if (Globals.Bit_2079 == true)", callback);
        Assert.Contains("await ctx.Scene_07_03_TalkEpisode2.Play(this);", callback);
        Assert.DoesNotContain("messageId: 88)", callback);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData("""
        if (Globals.Byte_289 == 0)
        {
            return;
        }
        """)]
    [InlineData("Globals.Bit_2066 = false;")]
    [InlineData("""
        if (Engine.ReadSystem(SystemVariable.DialogChoice) != 0)
        {
            await Engine.Wait(frames: 1);
        }
        """)]
    public void ARequestCannotSupplyEvidenceWhenItCanSkipDialogueWriteTheGuardOrReadInput(string statement)
    {
        const string line = "await Messages.ShowAndWait(windowId: 2, flags: 128, messageId: 87);";
        CheckCalleeEdit(statement + Environment.NewLine + line, firstMoves: false);
    }

    [Theory]
    [InlineData("""
        await Messages.Show(windowId: 2, flags: 128, messageId: 87);
        await Messages.WaitWindow(windowId: 2);
        """, true)]
    [InlineData("""
        await Messages.Show(windowId: 2, flags: 128, messageId: 87);
        await Messages.WaitWindow(windowId: 3);
        """, false)]
    [InlineData("""
        await Messages.Show(windowId: 2, flags: 128, messageId: 87);
        await Messages.CloseWindow(windowId: 2);
        await Messages.WaitWindow(windowId: 2);
        """, false)]
    [InlineData("""
        await Messages.Show(windowId: 2, flags: 128, messageId: 87);
        Scripts.DeleteObject(uid: 12);
        await Messages.WaitWindow(windowId: 2);
        """, false)]
    [InlineData("""
        if (Globals.Byte_289 == 0)
        {
            await Messages.Show(windowId: 2, flags: 128, messageId: 87);
        }
        else
        {
            await Messages.WaitWindow(windowId: 2);
        }
        """, false)]
    [InlineData("""
        if (Globals.Byte_289 == 0)
        {
            await Messages.Show(windowId: 2, flags: 128, messageId: 87);
        }
        else
        {
            await Messages.ShowAndWait(windowId: 2, flags: 128, messageId: 87);
            return;
        }
        await Messages.WaitWindow(windowId: 2);
        """, true)]
    public void ADeferredLineMustRemainOpenUntilTheSamePathWaitsForItsWindow(string replacement, bool firstMoves) =>
        CheckCalleeEdit(replacement, firstMoves);

    [Fact]
    public void OpposingControlGuardsCannotProveTheSameControlHandoff()
    {
        string baseline = Export(Fixture(Kitchen), "baseline", extract: false);
        string participant = Participant(baseline, 7);
        MethodDeclarationSyntax talk = Script(participant, 3);
        var episode = Assert.Single(talk.DescendantNodes().OfType<IfStatementSyntax>()
            .Where(condition => condition.Condition.ToString() == "Globals.Bit_2066 == false"));
        var restore = Assert.Single(episode.Statement.DescendantNodes().OfType<IfStatementSyntax>()
            .Where(condition => condition.Condition.ToString() == "Location.Bit_159 == true" &&
                condition.DescendantNodes().OfType<InvocationExpressionSyntax>().Any(call => call.Expression.ToString() == "Engine.EnableMove")));
        string source = File.ReadAllText(participant);
        File.WriteAllText(participant, source[..restore.Condition.SpanStart] + "Location.Bit_159 == false" + source[restore.Condition.Span.End..]);

        EVContainer edited = EventProjectCompiler.Compile(baseline);
        string project = Export(edited, "reopened");
        AssertOnlySecondMoves(edited, project);
    }

    private void CheckCalleeEdit(string replacement, bool firstMoves)
    {
        string baseline = Export(Fixture(Kitchen), "baseline", extract: false);
        Replace(Participant(baseline, 12), "await Messages.ShowAndWait(windowId: 2, flags: 128, messageId: 87);", replacement);

        EVContainer edited = EventProjectCompiler.Compile(baseline);
        string project = Export(edited, "reopened");
        Assert.Equal(firstMoves, File.Exists(Episode(project, 1)));
        string callback = Script(Participant(project, 7), 3).ToString();
        Assert.Equal(!firstMoves, callback.Contains("messageId: 88)", StringComparison.Ordinal));
        Assert.Contains("messageId: 100)", File.ReadAllText(Episode(project, 2)));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    private static void AssertOnlySecondMoves(EVContainer edited, string project)
    {
        Assert.False(File.Exists(Episode(project, 1)));
        Assert.Contains("messageId: 88)", Script(Participant(project, 7), 3).ToString());
        Assert.Contains("messageId: 100)", File.ReadAllText(Episode(project, 2)));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData("EVT_MOGNET_MOG_VER1", 11)]
    [InlineData("EVT_SARI2_MS_EIK_1", 1)]
    [InlineData(Kitchen, 4)]
    public void OrdinaryTalkCallbacksStayInParticipantSource(string field, int participant)
    {
        EVContainer original = Fixture(field);
        string baseline = Export(original, "baseline", extract: false, field);
        string project = Export(original, "extracted", field: field);
        Assert.True(SyntaxFactory.AreEquivalent(Script(Participant(baseline, participant), 3),
            Script(Participant(project, participant), 3)));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    private static EVContainer Fixture(string name) => EVContainer.Read(new MemoryStream(File.ReadAllBytes(
        Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"))));

    private static EVContainerScript TalkScript(EVContainer field) => Assert.Single(field.Objects[7].Scripts.Where(script => script.Id == 3));

    private string Export(EVContainer fieldData, string name, bool extract = true, string field = Kitchen, CutsceneCatalog? catalog = null)
    {
        string project = Path.Combine(_directory, name);
        EventProjectExporter.Export(fieldData, field, project, useCutsceneNames: false, extractCutscenes: extract, cutsceneCatalog: catalog);
        return project;
    }

    private static string Participant(string project, int slot) => Assert.Single(Directory.GetFiles(project, $"{slot:D2}_*.cs"));

    private static string Episode(string project, int number) => Path.Combine(project, "Cutscenes", $"Scene_07_03_TalkEpisode{number}.cs");

    private static MethodDeclarationSyntax Script(string file, int tag) => Assert.Single(CSharpSyntaxTree.ParseText(File.ReadAllText(file))
        .GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.AttributeLists.SelectMany(list => list.Attributes)
            .Any(attribute => attribute.Name.ToString() == "Script" &&
                attribute.ArgumentList?.Arguments[0].Expression is LiteralExpressionSyntax { Token.Value: int value } && value == tag)));

    private static void Replace(string file, string oldValue, string newValue)
    {
        string source = File.ReadAllText(file);
        Assert.Equal(2, source.Split(oldValue, StringSplitOptions.None).Length);
        File.WriteAllText(file, source.Replace(oldValue, newValue, StringComparison.Ordinal));
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
