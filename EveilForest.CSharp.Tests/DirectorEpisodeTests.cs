using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class DirectorEpisodeTests : IDisposable
{
    private const string Train = "EVT_GATE_S_SG_TRN_2";
    private const string Daguerreo = "EVT_DAG_DG_ENT_0";
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-director-{Guid.NewGuid():N}");

    [Fact]
    public void TrainOwnsItsMainExchangeAndLeavesSetupAndSharedActionsInPlace()
    {
        EVContainer field = Fixture(Train);
        string project = Export(field, Train, "extracted");
        string scene = File.ReadAllText(Scene(project, "Scene_00_01"));
        string controller = Participant(project, 0);

        foreach (int message in Enumerable.Range(184, 5).Concat(Enumerable.Range(190, 14)))
            Assert.Contains($"messageId: {message})", scene);
        foreach (string action in new[] { "Action_05_11", "Action_09_11", "Action_09_12", "Action_10_11" })
            Assert.Contains(action, scene);
        Assert.Contains("RunSharedScript(On(", scene);
        Assert.Contains("Engine.EnableMove();", scene);
        Assert.Contains("ctx.Scene_00_01.Play(this)", Method(controller, "OnLoop").ToFullString());
        Assert.Contains("messageId: 52)", Method(controller, "OnInit").ToFullString());
        Assert.Contains("messageId: 205)", File.ReadAllText(Participant(project, 5)));
        Assert.Contains("Script_13", File.ReadAllText(Participant(project, 9)));
        Assert.Contains("Script_12", File.ReadAllText(Participant(project, 13)));
        Assert.Contains("messageId: 206)", File.ReadAllText(Scene(project, "Scene_00_14")));

        AssertCompiles(field, SourceProjectTest.CopyWithoutComments(project));
    }

    [Fact]
    public void FarewellOwnsTheDepartureAndLeavesTheTradeAndTransferOnTheirOriginalEntries()
    {
        EVContainer field = Fixture(Daguerreo);
        string project = Export(field, Daguerreo, "extracted");
        string scene = File.ReadAllText(Scene(project, "Scene_00_18"));
        string controller = Participant(project, 0);
        string caller = Method(Participant(project, 14), "OnSpeak").ToFullString();

        foreach (int message in Enumerable.Range(247, 8).Concat(new[] { 66, 67 }))
            Assert.Contains($"messageId: {message})", scene);
        foreach (int tag in Enumerable.Range(19, 6))
            Assert.Contains($"Action_14_{tag}", scene);
        Assert.Contains("Inventory.AddItem(item: 323, amount: 1);", scene);
        Assert.Contains("ctx.Scene_00_18.Play(this)", Method(controller, "Script_18").ToFullString());
        Assert.Contains("Engine.DisableMove();", caller);
        Assert.Contains("evt.Object_00.Script_18", caller);
        Assert.Contains("Messages.CloseWindow(", scene);
        Assert.Contains("Scripts.DeleteObject(uid: 14);", scene);
        Assert.True(scene.IndexOf("Messages.CloseWindow(", StringComparison.Ordinal) < scene.IndexOf("Scripts.DeleteObject(", StringComparison.Ordinal));
        Assert.True(scene.IndexOf("Scripts.DeleteObject(", StringComparison.Ordinal) < scene.IndexOf("Engine.EnableMove();", StringComparison.Ordinal));
        Assert.Contains("Inventory.RemoveItem(", Method(controller, "Script_15").ToFullString());
        Assert.Contains("messageId: 107)", Method(controller, "Script_15").ToFullString());
        Assert.Contains("Field.ChangeMap(", Method(Participant(project, 19), "InRange").ToFullString());

        AssertCompiles(field, SourceProjectTest.CopyWithoutComments(project));
    }

    [Theory]
    [InlineData(Train, "Scene_00_01", "OnLoop", "messageId: 188)", "messageId: 189)")]
    [InlineData(Daguerreo, "Scene_00_18", "Script_18", "messageId: 253)", "messageId: 255)")]
    public void EpisodeEditsCompileTheSameAsParticipantEditsAndReopenAsScenes(string fieldName, string scene, string controllerMethod, string before, string after)
    {
        EVContainer field = Fixture(fieldName);
        string flat = Export(field, fieldName, "flat", extract: false);
        string extracted = SourceProjectTest.CopyWithoutComments(Export(field, fieldName, "extracted"));
        EditMethod(Participant(flat, 0), controllerMethod, before, after);
        EditMethod(Scene(extracted, scene), "Play", before, after);
        EVContainer expected = EventProjectCompiler.Compile(flat);
        EVContainer edited = EventProjectCompiler.Compile(extracted);
        Assert.NotEqual(field.Serialize(), edited.Serialize());
        Assert.Equal(expected.Serialize(), edited.Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(extracted));

        string reopened = Export(edited, fieldName, "reopened");
        Assert.Contains(after, File.ReadAllText(Scene(reopened, scene)));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void AControllerWithAnIndependentTalkAndReturnIsNotMovedWhole()
    {
        string flat = Export(Fixture(Train), Train, "flat", extract: false);
        const string first = "scripts.RunSharedScript(evt.Object_01);";
        EditMethod(Participant(flat, 0), "OnLoop", first, """
            if (Globals.ScenarioCounter == 7)
            {
                await Messages.ShowAndWait(windowId: 0, flags: 0, messageId: 205);
                return;
            }
            """ + Environment.NewLine + first);
        EVContainer edited = EventProjectCompiler.Compile(flat);

        string reopened = Export(edited, Train, "reopened");
        Assert.False(File.Exists(Scene(reopened, "Scene_00_01")));
        string loop = Method(Participant(reopened, 0), "OnLoop").ToFullString();
        Assert.Contains("messageId: 205)", loop);
        Assert.Contains("messageId: 184)", loop);
        AssertCompiles(edited, reopened);
    }

    [Fact]
    public void AnActorStagedForTalkWithoutAnExitDoesNotEstablishAFarewell()
    {
        string flat = Export(Fixture(Daguerreo), Daguerreo, "flat", extract: false);
        EditMethod(Participant(flat, 0), "Script_18", "Scripts.DeleteObject(uid: 14);", "await Engine.Wait(frames: 1);");
        EVContainer edited = EventProjectCompiler.Compile(flat);

        string reopened = Export(edited, Daguerreo, "reopened");
        Assert.False(File.Exists(Scene(reopened, "Scene_00_18")));
        Assert.Contains("messageId: 254)", Method(Participant(reopened, 0), "Script_18").ToFullString());
        AssertCompiles(edited, reopened);
    }

    [Theory]
    [InlineData("Inventory.RemoveItem(item: 254, amount: 1);", "Inventory.RemoveItem(")]
    [InlineData("""
        if (Input.IsKeyPressed(PlayerInput.Cancel))
        {
            await Engine.Wait(frames: 1);
        }
        """, "Input.IsKeyPressed(PlayerInput.Cancel)")]
    [InlineData("""
        if (Input.IsKeyHeld(PlayerInput.Cancel))
        {
            await Engine.Wait(frames: 1);
        }
        """, "Input.IsKeyHeld(PlayerInput.Cancel)")]
    [InlineData("await Engine.Wait(frames: Engine.ReadSystem(SystemVariable.DialogChoice));", "SystemVariable.DialogChoice")]
    public void InteractiveControllerRoutesKeepTheirOriginalOwner(string statement, string evidence)
    {
        string flat = Export(Fixture(Daguerreo), Daguerreo, "flat", extract: false);
        const string reward = "Inventory.AddItem(item: 323, amount: 1);";
        EditMethod(Participant(flat, 0), "Script_18", reward, statement + Environment.NewLine + reward);
        EVContainer edited = EventProjectCompiler.Compile(flat);

        string reopened = Export(edited, Daguerreo, "reopened");
        Assert.False(File.Exists(Scene(reopened, "Scene_00_18")));
        Assert.Contains(evidence, Method(Participant(reopened, 0), "Script_18").ToFullString());
        AssertCompiles(edited, reopened);
    }

    [Fact]
    public void ActorActionsThatCanCompleteBeforeStagingDoNotEstablishAControllerEpisode()
    {
        string flat = Export(Fixture(Daguerreo), Daguerreo, "flat", extract: false);
        string actor = Participant(flat, 14);
        foreach (int tag in Enumerable.Range(19, 6))
        {
            string method = $"Script_{tag}";
            string original = Method(actor, method).Body!.ToFullString();
            int entry = original.IndexOf('{') + 1;
            EditMethod(actor, method, original, original.Insert(entry, """

                    if (Globals.ScenarioCounter == 7)
                    {
                        return;
                    }

                """));
        }

        EVContainer edited = EventProjectCompiler.Compile(flat);
        string reopened = Export(edited, Daguerreo, "reopened");
        Assert.False(File.Exists(Scene(reopened, "Scene_00_18")));
        Assert.Contains("messageId: 254)", Method(Participant(reopened, 0), "Script_18").ToFullString());
        AssertCompiles(edited, reopened);
    }

    [Fact]
    public void DifferentActorActionsOnEveryCompletingBranchCanEstablishAControllerEpisode()
    {
        string flat = Export(Fixture(Daguerreo), Daguerreo, "flat", extract: false);
        string actor = Participant(flat, 14);
        string original = Method(actor, "Script_24").Body!.ToFullString();
        EditMethod(actor, "Script_24", original, """
            {
                if (Globals.ScenarioCounter == 7)
                {
                    await Actions.WalkTo(x: -78, z: -3257);
                }
                else
                {
                    Actions.TurnAtSpeed(angle: 0, speed: 32);
                    await Actions.WaitForTurn();
                }
                return;
            }
            """);

        EVContainer edited = EventProjectCompiler.Compile(flat);
        string reopened = Export(edited, Daguerreo, "reopened");
        string scene = Scene(reopened, "Scene_00_18");
        string departure = Method(scene, "Action_14_24").ToFullString();
        Assert.Contains("await participant.Actions.WalkTo", departure);
        Assert.Contains("participant.Actions.TurnAtSpeed", departure);
        AssertCompiles(edited, reopened);
    }

    private static EVContainer Fixture(string name)
    {
        using Stream input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
        return EVContainer.Read(input);
    }

    private string Export(EVContainer field, string name, string folder, bool extract = true)
    {
        string project = Path.Combine(_directory, folder);
        EventProjectExporter.Export(field, name, project, useCutsceneNames: false, extractCutscenes: extract);
        return project;
    }

    private static string Scene(string project, string name) => Path.Combine(project, "Cutscenes", name + ".cs");

    private static string Participant(string project, int slot) => Assert.Single(Directory.GetFiles(project, $"{slot:D2}_*.cs"));

    private static MethodDeclarationSyntax Method(string path, string name) => Assert.Single(CSharpSyntaxTree.ParseText(File.ReadAllText(path))
        .GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Identifier.ValueText == name));

    private static void EditMethod(string path, string name, string before, string after)
    {
        MethodDeclarationSyntax method = Method(path, name);
        string source = File.ReadAllText(path);
        string original = method.ToFullString();
        Assert.Equal(2, original.Split(before, StringSplitOptions.None).Length);
        string edited = original.Replace(before, after, StringComparison.Ordinal);
        File.WriteAllText(path, source.Remove(method.FullSpan.Start, method.FullSpan.Length).Insert(method.FullSpan.Start, edited));
    }

    private static void AssertCompiles(EVContainer field, string project)
    {
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
