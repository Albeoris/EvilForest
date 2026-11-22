using EveilForest.CSharp;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class DirectedSceneTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-directed-{Guid.NewGuid():N}");

    [Fact]
    public void ForestEscapeKeepsActorRequestsAndEditsInTheirOriginalEntries()
    {
        const string name = "EVT_EFOREST1_EF_FR6_0";
        byte[] fixture = Export(name);
        string scene = Directory.GetFiles(Path.Combine(_directory, "Cutscenes"), "*.cs").Single(path =>
            File.ReadAllText(path).Contains("messageId: 160)", StringComparison.Ordinal));
        string source = File.ReadAllText(scene);
        Assert.Contains("scripts.StartWhenReady(On(", source);
        Assert.Contains("scripts.RunAndWait(On(", source);
        Assert.Contains("Field.ChangeMap(", source);
        Assert.Contains("(actor entry)", File.ReadAllText(Path.Combine(_directory, "CutsceneOutline.tsv")));
        Assert.Equal(fixture, EventProjectCompiler.Compile(_directory).Serialize());

        File.WriteAllText(scene, source.Replace("messageId: 160)", "messageId: 161)", StringComparison.Ordinal));
        byte[] edited = EventProjectCompiler.Compile(_directory).Serialize();
        Assert.Equal(fixture.Length, edited.Length);
        int index = Assert.Single(Enumerable.Range(0, edited.Length).Where(index => fixture[index] != edited[index]));
        Assert.Equal(160, fixture[index]);
        Assert.Equal(161, edited[index]);
    }

    [Fact]
    public void MutuallyExclusiveSoloRequestsDoNotBecomeAnEnsemble()
    {
        const string name = "EVT_EFOREST1_EF_FR6_0";
        Export(name);
        string file = Directory.GetFiles(Path.Combine(_directory, "Cutscenes"), "*.cs").Single();
        string source = File.ReadAllText(file);
        var play = CSharpSyntaxTree.ParseText(source).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "Play");
        string body = """
            {
                var evt = ctx.Scene;
                var scripts = ctx.Scripts;
                if (Globals.ScenarioCounter == 7)
                {
                    await scripts.RunAndWait(On(evt.GEO_MAIN_F0_VIV_11, Action_11_15), level: 2);
                }
                else
                {
                    await scripts.RunAndWait(On(evt.GEO_MAIN_F0_STD_06, Action_06_15), level: 2);
                }
                await Messages.Show(windowId: 1, flags: 0, messageId: 160);
                await Messages.Show(windowId: 1, flags: 0, messageId: 161);
                Field.ChangeMap(fieldId: Resources.EVT_EFOREST1_EF_EXT_0);
            }
            """;
        File.WriteAllText(file, source.Remove(play.Body!.SpanStart, play.Body.Span.Length).Insert(play.Body.SpanStart, body));
        EVContainer edited = EventProjectCompiler.Compile(_directory);
        string reopened = Path.Combine(_directory, ".reopened");
        EventProjectExporter.Export(edited, name, reopened, useCutsceneNames: false);
        Assert.False(Directory.Exists(Path.Combine(reopened, "Cutscenes")));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void IpsenDirectorKeepsTheConversationInsideItsRequestedActions()
    {
        byte[] fixture = Export("EVT_IPSEN_IP_CNT_2");
        var chapters = Directory.GetFiles(Path.Combine(_directory, "Cutscenes"), "*.cs").Select(File.ReadAllText);
        string scene = Assert.Single(chapters.Where(source => source.Contains("EVT_IPSEN_IP_CNT_0", StringComparison.Ordinal)));
        Assert.Contains("scripts.RunAndWait(", scene);
        Assert.Contains("messageId: 130)", scene);
        Assert.Equal(fixture, EventProjectCompiler.Compile(_directory).Serialize());
    }

    [Fact]
    public void LindblumChapterLeavesItsSharedReturnInTheOriginalHandler()
    {
        byte[] fixture = Export("EVT_LIND2_TN_LB_OMG_W");
        string scene = Directory.GetFiles(Path.Combine(_directory, "Cutscenes"), "*.cs").Single();
        string chapter = File.ReadAllText(scene);
        string root = File.ReadAllText(Directory.GetFiles(_directory, "00_*.cs").Single());
        Assert.Contains("EVT_LIND2_TN_LB_PLZ_0", chapter);
        Assert.Contains("await ctx.", root);
        Assert.DoesNotContain("EVT_LIND2_TN_LB_PLZ_0", root);
        Assert.Equal(fixture, EventProjectCompiler.Compile(_directory).Serialize());
    }

    [Fact]
    public void ReopeningAWaitAfterTheCardMatchPreservesItsSuspension()
    {
        const string name = "EVT_LAST_CW_LTP_0";
        Export(name);
        string file = Directory.GetFiles(Path.Combine(_directory, "Cutscenes"), "*.cs").Single();
        string source = File.ReadAllText(file);
        const string restore = "        RestoreCardInteraction(participant);";
        Assert.Contains(restore, source);
        File.WriteAllText(file, source.Replace(restore, "        await Engine.Wait(frames: 11);" + Environment.NewLine + restore, StringComparison.Ordinal));
        EVContainer edited = EventProjectCompiler.Compile(_directory);
        string reopened = Path.Combine(_directory, ".reopened");
        EventProjectExporter.Export(edited, name, reopened, useCutsceneNames: false);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(reopened));
    }

    [Theory]
    [InlineData("var unused = ChooseContext().Globals;")]
    [InlineData("Location.SceneStep = ChooseContext().Globals.ScenarioCounter;")]
    [InlineData("ChooseContext().Globals.ScenarioCounter = 8;")]
    public void EvaluatingAStorageReceiverCannotSilentlyLoseSideEffects(string edit)
    {
        Export("EVT_ALEX1_TS_CARGO_0");
        string file = Directory.GetFiles(_directory, "04_*.cs").Single();
        string original = File.ReadAllText(file);
        int entry = original.IndexOf("    [Script(0, 0)]", StringComparison.Ordinal);
        Assert.True(entry >= 0);
        string source = original.Insert(entry, """
                private SceneContext ChooseContext()
                {
                    Globals.ScenarioCounter = 7;
                    return ctx;
                }

            """);
        const string polygon = "        Regions.DefinePolygon(";
        Assert.Contains(polygon, source);
        File.WriteAllText(file, source.Replace(polygon, "        " + edit + Environment.NewLine + polygon, StringComparison.Ordinal));
        Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(_directory));
        Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(_directory));
    }

    private byte[] Export(string name)
    {
        byte[] fixture = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
        using var input = new MemoryStream(fixture);
        EventProjectExporter.Export(EVContainer.Read(input), name, _directory, useCutsceneNames: false, allowNativePhaseDispatch: name == "EVT_ALEX1_TS_CARGO_0");
        return fixture;
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
