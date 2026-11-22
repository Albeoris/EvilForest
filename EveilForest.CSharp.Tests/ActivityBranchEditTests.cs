using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class ActivityBranchEditTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-activity-edit-").FullName;

    [Fact]
    public void ASecondCallFromTheMinigameKeepsTheActorMethodOnItsParticipant()
    {
        const string name = "EVT_KUINA_KM_SWP_1";
        using var input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
        var field = EVContainer.Read(input);
        string source = Path.Combine(_directory, "source");
        EventProjectExporter.Export(field, name, source, useCutsceneNames: false, extractCutscenes: false);
        string root = Directory.GetFiles(source, "00_*.cs").Single();
        string original = File.ReadAllText(root);
        const string arm = "Location.Bit_613 = false;";
        int trigger = original.IndexOf(arm, original.IndexOf("messageId: 358)", StringComparison.Ordinal), StringComparison.Ordinal);
        Assert.True(trigger >= 0);
        const string additionalRequest = "await scripts.RequestAndWait(evt.GEO_MAIN_F0_KUI_22.Script_28, level: 6);";
        File.WriteAllText(root, original.Insert(trigger, additionalRequest + Environment.NewLine));
        var edited = EventProjectCompiler.Compile(source);
        string reopened = Path.Combine(_directory, "reopened");
        EventProjectExporter.Export(edited, name, reopened, useCutsceneNames: false);
        string scene = File.ReadAllText(Directory.GetFiles(Path.Combine(reopened, "Cutscenes"), "*.cs").Single(file => File.ReadAllText(file).Contains("messageId: 355)", StringComparison.Ordinal)));
        string actor = File.ReadAllText(Directory.GetFiles(reopened, "22_*.cs").Single());
        Assert.Contains(additionalRequest, scene);
        Assert.Contains("public void Script_28()", actor);
        Assert.Contains("Action_22_11", scene);
        Assert.DoesNotContain(" Script_11()", actor);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Theory]
    [InlineData("messageId: 358)")]
    [InlineData("Resources.ANH_MAIN_F0_KUI_LOSE_1_6526")]
    public void InsertingAWaitInOneAnswerMatchesTheSameEditInTheParticipantProgram(string anchor)
    {
        const string name = "EVT_KUINA_KM_SWP_1";
        byte[] original = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
        var field = EVContainer.Read(new MemoryStream(original));
        string participantProject = Export(field, "participants", false);
        string sceneProject = Export(field, "scene", true);
        string participant = Directory.GetFiles(participantProject, "00_*.cs").Single();
        string scene = Directory.GetFiles(Path.Combine(sceneProject, "Cutscenes"), "*.cs")
            .Single(file => File.ReadAllText(file).Contains("messageId: 355)", StringComparison.Ordinal));
        InsertWait(participant);
        InsertWait(scene);

        byte[] expected = EventProjectCompiler.Compile(participantProject).Serialize();
        var edited = EventProjectCompiler.Compile(sceneProject);
        Assert.True(expected.Length > original.Length);
        Assert.Equal(expected, edited.Serialize());
        string reopened = Export(edited, "reopened", true);
        string chapter = Directory.GetFiles(Path.Combine(reopened, "Cutscenes"), "*.cs")
            .Single(file => File.ReadAllText(file).Contains("messageId: 355)", StringComparison.Ordinal));
        Assert.Contains("await Engine.Wait(frames: 73);", File.ReadAllText(chapter));
        Assert.Equal(expected, EventProjectCompiler.Compile(reopened).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(reopened));

        string Export(EVContainer container, string variant, bool extract)
        {
            string destination = Path.Combine(_directory, variant);
            EventProjectExporter.Export(container, name, destination, useCutsceneNames: false, extractCutscenes: extract);
            return SourceProjectTest.CopyWithoutComments(destination);
        }

        void InsertWait(string file)
        {
            var lines = File.ReadAllLines(file).ToList();
            int index = Assert.Single(Enumerable.Range(0, lines.Count).Where(i => lines[i].Contains(anchor, StringComparison.Ordinal)));
            string indent = lines[index][..(lines[index].Length - lines[index].TrimStart().Length)];
            lines.Insert(index, indent + "await Engine.Wait(frames: 73);");
            File.WriteAllLines(file, lines);
        }
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
