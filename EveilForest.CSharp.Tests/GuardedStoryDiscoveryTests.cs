using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class GuardedStoryDiscoveryTests : IDisposable
{
    private const string Hilda = "EVT_HILDA3_BN_DCR_0";
    private const string Patas = "EVT_PATA_T_CP_GL1_0";
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-story-guards-").FullName;

    [Theory]
    [InlineData(Hilda, "Globals.ScenarioCounter < 9400", 9400, 82, 99)]
    [InlineData(Patas, "(Globals.FieldEntrance == 3) & (Globals.ScenarioCounter < 6100)", 6100, 112, 118)]
    public void StoryGuardsStayOnTheirHandlerAndOnlyTheStagedBranchMoves(string name, string guard, int cutoff, int firstLine, int nextSpeakerLine)
    {
        byte[] original = Fixture(name);
        var field = EVContainer.Read(new MemoryStream(original));
        string exported = Path.Combine(_directory, "exported");
        EventProjectExporter.Export(field, name, exported, useCutsceneNames: false);
        string project = SourceProjectTest.CopyWithoutComments(exported);
        string scene = $"Scene_00_01_BeforeStoryStep{cutoff}";
        string sceneFile = Path.Combine(project, "Cutscenes", scene + ".cs");
        string source = File.ReadAllText(sceneFile);
        string handler = File.ReadAllText(Path.Combine(project, "00_Object_00.cs"));
        Assert.Contains($"if ({guard})", handler);
        Assert.Contains($"await ctx.{scene}.Play(this);", handler);
        Assert.DoesNotContain($"if ({guard})", source);
        Assert.Contains($"messageId: {nextSpeakerLine})", source);
        Assert.Contains("switch (", source);
        if (name == Patas)
        {
            var story = Assert.Single(CSharpSyntaxTree.ParseText(handler).GetRoot().DescendantNodes().OfType<IfStatementSyntax>()
                .Where(branch => branch.Condition.ToString() == guard && branch.Statement.ToString().Contains($"ctx.{scene}.Play(this)")));
            Assert.Contains("Scene_00_01_EntranceMinus1.Play(this)", story.Else!.Statement.ToString());
            Assert.Contains("switch ((++Globals.Byte_303))", source);
        }

        Assert.Equal(original, EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

        string message = $"messageId: {firstLine})";
        Assert.Equal(2, source.Split(message, StringSplitOptions.None).Length);
        File.WriteAllText(sceneFile, source.Replace(message, $"messageId: {firstLine + 1})", StringComparison.Ordinal));
        byte[] edited = EventProjectCompiler.Compile(project).Serialize();
        Assert.Equal(original.Length, edited.Length);
        Assert.Equal((firstLine, firstLine + 1), Assert.Single(original.Zip(edited).Where(bytes => bytes.First != bytes.Second)
            .Select(bytes => ((int)bytes.First, (int)bytes.Second))));
    }

    [Fact]
    public void AnEarlyExternalExitCannotLeaveOnlyTheLeadingWaitInAScene()
    {
        var field = EVContainer.Read(new MemoryStream(Fixture(Patas)));
        string project = Path.Combine(_directory, "baseline");
        EventProjectExporter.Export(field, Patas, project, useCutsceneNames: false, extractCutscenes: false);
        string handlerFile = Path.Combine(project, "00_Object_00.cs");
        string source = File.ReadAllText(handlerFile);
        int start = source.IndexOf("public async EventRoutine OnLoop()", StringComparison.Ordinal);
        int nextWait = source.IndexOf("await Engine.Wait(frames: 30);", start, StringComparison.Ordinal);
        Assert.True(nextWait > start);
        int lineEnd = source.IndexOf('\n', nextWait);
        var loop = Assert.Single(CSharpSyntaxTree.ParseText(source).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(method => method.Identifier.ValueText == "OnLoop"));
        var exit = Assert.IsType<ReturnStatementSyntax>(loop.Body!.Statements.Last());
        source = source.Insert(exit.SpanStart, "LeaveStory: ");
        const string externalExit = """
            if (Globals.FieldEntrance != 3)
            {
                goto LeaveStory;
            }
            """;
        source = source.Insert(lineEnd + 1, externalExit + Environment.NewLine);
        File.WriteAllText(handlerFile, source);
        var edited = EventProjectCompiler.Compile(project);

        string reopened = Path.Combine(_directory, "reopened");
        EventProjectExporter.Export(edited, Patas, reopened, useCutsceneNames: false);
        string scenePath = Path.Combine(reopened, "Cutscenes", "Scene_00_01_BeforeStoryStep6100.cs");
        Assert.False(File.Exists(scenePath));
        string reopenedHandler = File.ReadAllText(Path.Combine(reopened, "00_Object_00.cs"));
        Assert.Contains("await Engine.Wait(frames: 30);", reopenedHandler);
        Assert.Contains("messageId: 112)", reopenedHandler);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    private static byte[] Fixture(string name) =>
        File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));

    public void Dispose()
    {
        Directory.Delete(_directory, recursive: true);
    }
}

