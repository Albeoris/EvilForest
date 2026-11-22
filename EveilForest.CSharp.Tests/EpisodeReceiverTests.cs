using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class EpisodeReceiverTests
{
    [Theory]
    [InlineData(197, true)]
    [InlineData(198, false)]
    public void ASharedSequenceAtTheActorUidCannotProveRegionChoreography(int threadUid, bool expectedScene)
    {
        const string name = "EVT_ICE_IC_MEN_0";
        string directory = Directory.CreateTempSubdirectory("evilforest-episode-receiver-").FullName;
        try
        {
            using var input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
            string flat = Path.Combine(directory, "flat");
            EventProjectExporter.Export(EVContainer.Read(input), name, flat, useCutsceneNames: false, extractCutscenes: false);

            string controller = Directory.GetFiles(flat, "00_*.cs").Single();
            const string spawn = "scripts.StartThread(evt.Object_01);";
            string source = File.ReadAllText(controller);
            Assert.Contains(spawn, source);
            File.WriteAllText(controller, source.Replace(spawn, $"scripts.StartThread(evt.Object_01, uid: new InstanceUid({threadUid}));", StringComparison.Ordinal));

            string thread = Directory.GetFiles(flat, "01_*.cs").Single();
            source = File.ReadAllText(thread);
            int init = source.IndexOf("public void OnInit()", StringComparison.Ordinal);
            Assert.True(init >= 0);
            int body = source.IndexOf('{', init);
            File.WriteAllText(thread, source.Insert(body + 1, Environment.NewLine + "        ctx.Scripts.RunSharedScript(ctx.Scene.GEO_MAIN_F0_ZDN_06);"));

            EVContainer edited = EventProjectCompiler.Compile(flat);
            string project = Path.Combine(directory, "edited");
            EventProjectExporter.Export(edited, name, project, useCutsceneNames: false);
            Assert.Equal(expectedScene, File.Exists(Path.Combine(project, "Cutscenes", "Scene_05_02.cs")));
            Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(project).Serialize());
            Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
