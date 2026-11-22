using System.Reflection;
using EvilForest.EventSource;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class CutsceneActionOwnershipTests : IDisposable
{
    private const string Garland = "EVT_PANDEMO_PD_CAO_0";
    private const string GarlandScene = "Scene_00_01_BeforeStoryStep10940";
    private const string Pandemonium = "EVT_PANDEMO_PD_PEV_0";
    private const string Pinnacle = "EVT_PINA_PR_GAL_0";
    private const string PinnacleScene = "Scene_02_01_At15";
    private const string EvilForest = "EVT_EFOREST1_EF_DEP_0";
    private const string BlackWaltz = "EVT_ICE_IC_WBF_0";
    private const string Treno = "EVT_TRENO1_TR_BAR_0";
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-action-ownership-").FullName;

    [Theory]
    [InlineData(Garland, GarlandScene, 7, 11, 2, "RunAndWait", "x: -18, z: 20", "x: -19, z: 20")]
    [InlineData(Treno, "Scene_00_01_Entrance1", 17, 11, 2, "TryStart", "x: 7342, z: -14732", "x: 7343, z: -14732")]
    [InlineData(Pinnacle, PinnacleScene, 13, 53, 2, "StartWhenReady", "Local.Byte_0 < 112", "Local.Byte_0 < 113")]
    [InlineData(BlackWaltz, "Scene_00_01", 1, 11, 0, "TryStart", "frames: 1", "frames: 2")]
    [InlineData(Pandemonium, "Scene_00_01_BeforeStoryStep10990", 8, 13, 2, "StartWhenReady", "x: -1190, z: -1995", "x: -1191, z: -1995")]
    [InlineData(Pandemonium, "Scene_00_19", 1, 11, 0, "TryStart", "frames: 118", "frames: 119")]
    public void ExclusiveActionsKeepTheirOriginalEntryAndEditsReachThatEntry(string name, string sceneName, int slot, int tag, byte type, string request, string before, string after)
    {
        byte[] original = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
        EVContainer field = EVContainer.Read(new MemoryStream(original));
        string flat = Export(field, name, "flat", extract: false);
        string project = SourceProjectTest.CopyWithoutComments(Export(field, name, "extracted"));
        Assert.Equal(original, EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

        string participantFile = Participant(project, slot);
        string receiver = Path.GetFileNameWithoutExtension(participantFile)[3..];
        string sceneFile = Scene(project, sceneName);
        MethodDeclarationSyntax action = Method(sceneFile, Action(slot, tag));
        Assert.Equal(receiver, Assert.Single(action.ParameterList.Parameters).Type!.ToString());
        Assert.Equal(type, field.Objects[slot].Type);
        int ordinal = Array.FindIndex(field.Objects[slot].Scripts, script => script.Id == tag);
        Assert.True(ordinal >= 0);
        Assert.Contains($"[Script({tag}, {ordinal})]", action.ToFullString());
        Assert.DoesNotContain($"[Script({tag}, {ordinal})]", File.ReadAllText(participantFile));
        Assert.Contains($"scripts.{request}(On(evt.{receiver}, {Action(slot, tag)})", File.ReadAllText(sceneFile));

        string flatFile = Participant(flat, slot);
        EditMethod(flatFile, Method(flatFile, $"Script_{tag:D2}"), before, after);
        EditMethod(sceneFile, action, before, after);
        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(EventProjectCompiler.Compile(flat).Serialize(), edited.Serialize());
        Assert.Equal(type, edited.Objects[slot].Type);
        Assert.Equal(tag, edited.Objects[slot].Scripts[ordinal].Id);
        Assert.False(field.Objects[slot].Scripts[ordinal].Body.AsSpan().SequenceEqual(edited.Objects[slot].Scripts[ordinal].Body));
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void EntrancesInOneHandlerHaveSeparateOwnersButRepeatedRequestsInOneEntranceShareTheAction()
    {
        EVContainer field = Fixture(Treno);
        string project = Export(field, Treno, "entrances");
        string firstEntrance = File.ReadAllText(Scene(project, "Scene_00_01_Entrance1"));
        string secondEntrance = File.ReadAllText(Scene(project, "Scene_00_01_Entrance5"));
        string actor = Participant(project, 17);

        Assert.Contains("RequestWhenReady(evt.GEO_SUB_F0_MRC_17.Script_14, level: 2)", firstEntrance);
        Assert.Contains("RequestWhenReady(evt.GEO_SUB_F0_MRC_17.Script_14, level: 2)", secondEntrance);
        Assert.Contains("[Script(14,", Method(actor, "Script_14").ToFullString());
        Assert.Equal(3, secondEntrance.Split("TryStart(On(evt.GEO_SUB_F0_MRC_17, Action_17_19), level: 2)", StringSplitOptions.None).Length);
        Assert.Contains("[Script(19,", Method(Scene(project, "Scene_00_01_Entrance5"), "Action_17_19").ToFullString());
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData("7")]
    [InlineData("Globals.FieldEntrance")]
    public void ARequestOutsideAnExtractedGuardCanKeepTheEntryOnItsActor(string target)
    {
        string flat = Export(Fixture(Garland), Garland, "flat", extract: false);
        string controller = Participant(flat, 0);
        MethodDeclarationSyntax loop = Method(controller, "OnLoop");
        string branch = loop.Body!.Statements.OfType<IfStatementSyntax>().Single(statement =>
            statement.Condition.ToString() == "Globals.ScenarioCounter < 10940").ToFullString();
        EditMethod(controller, loop, branch, branch + $"        Scripts.RequestAsync(level: 2, targetUid: {target}, scriptId: 11);" + Environment.NewLine);
        EVContainer edited = EventProjectCompiler.Compile(flat);
        string project = Export(edited, Garland, "extracted");

        Assert.Contains("RequestAndWait(evt.GEO_MAIN_F0_ZDN_07.Script_11, level: 2)", File.ReadAllText(Scene(project, GarlandScene)));
        Assert.Contains("[Script(11, 1)]", Method(Participant(project, 7), "Script_11").ToFullString());
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void AnExclusiveActionCanOwnAnotherRequestedEntry()
    {
        EVContainer field = Fixture(Garland);
        string flat = Export(field, Garland, "flat", extract: false);
        string zidane = Participant(flat, 7);
        string garland = Participant(flat, 2);
        const string walk = "await Actions.WalkTo(x: -18, z: 20);";
        EditMethod(zidane, Method(zidane, "Script_11"), walk, walk + Environment.NewLine + "        Scripts.RequestAsync(level: 2, targetUid: 2, scriptId: 21);");
        int ordinal = field.Objects[2].Scripts.Length;
        string garlandSource = File.ReadAllText(garland);
        string helper = $$"""
                [Script(21, {{ordinal}})]
                public void Script_21()
                {
                    Actions.SetWalkSpeed(speed: 99);
                    return;
                }

            """;
        File.WriteAllText(garland, garlandSource.Insert(garlandSource.LastIndexOf('}'), helper));
        EVContainer edited = EventProjectCompiler.Compile(flat);
        string project = Export(edited, Garland, "extracted");
        string scene = Scene(project, GarlandScene);

        Assert.Contains("TryStart(On(evt.GEO_SUB_F0_GRL_02, Action_02_21), level: 2)", Method(scene, "Action_07_11").ToFullString());
        Assert.Contains($"[Script(21, {ordinal})]", Method(scene, "Action_02_21").ToFullString());
        Assert.DoesNotContain($"[Script(21, {ordinal})]", File.ReadAllText(Participant(project, 2)));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void AnExclusiveSharedSequenceKeepsItsRegionDefinitionAndInitialEntry()
    {
        EVContainer field = Fixture(EvilForest);
        string flat = Export(field, EvilForest, "flat", extract: false);
        string project = SourceProjectTest.CopyWithoutComments(Export(field, EvilForest, "extracted"));
        string sceneFile = Scene(project, "Scene_00_01");
        MethodDeclarationSyntax initial = Method(sceneFile, "Action_03_00");

        Assert.Contains("RunSharedScript(On(evt.Object_03, Action_03_00))", File.ReadAllText(sceneFile));
        Assert.Contains("[Script(0, 0)]", initial.ToFullString());
        Assert.Contains("[TrailingPadding(3)]", initial.ToFullString());
        Assert.Equal("Object_03", Assert.Single(initial.ParameterList.Parameters).Type!.ToString());
        Assert.DoesNotContain("[Script(0, 0)]", File.ReadAllText(Participant(project, 3)));
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

        string flatInitial = Participant(flat, 3);
        EditMethod(flatInitial, Method(flatInitial, "OnInit"), "frames: 1", "frames: 2");
        EditMethod(sceneFile, initial, "frames: 1", "frames: 2");
        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(EventProjectCompiler.Compile(flat).Serialize(), edited.Serialize());
        Assert.Equal(1, edited.Objects[3].Type);
        Assert.Equal(0, Assert.Single(edited.Objects[3].Scripts).Id);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData("scripts.StartRegion(evt.Object_03);")]
    [InlineData("scripts.RunSharedScript(evt.Object_03);")]
    public void ASeparateUseOfASequenceDefinitionKeepsItsInitialEntryOnTheRegion(string use)
    {
        string flat = Export(Fixture(EvilForest), EvilForest, "flat", extract: false);
        string controller = Participant(flat, 0);
        const string initialize = "Location.Bit_159 = false;";
        EditMethod(controller, Method(controller, "OnInit"), initialize, initialize + Environment.NewLine + "        " + use);
        EVContainer edited = EventProjectCompiler.Compile(flat);
        string project = Export(edited, EvilForest, "extracted");
        string scene = File.ReadAllText(Scene(project, "Scene_00_01"));

        Assert.Contains("RunSharedScript(evt.Object_03)", scene);
        Assert.DoesNotContain("Action_03_00", scene);
        Assert.Contains("[Script(0, 0)]", Method(Participant(project, 3), "OnInit").ToFullString());
        Assert.Contains(use, Method(Participant(project, 0), "OnInit").ToFullString());
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData(198, false)]
    [InlineData(199, true)]
    public void ASequenceUidCanWrapOntoARequestedActorUid(int sequenceOwnerUid, bool collides)
    {
        string flat = Export(Fixture(Garland), Garland, "flat", extract: false);
        string controller = Participant(flat, 0);
        const string spawn = "scripts.StartActor(evt.GEO_SUB_F0_GRL_02);";
        string alias = $"scripts.StartActor(evt.GEO_SUB_F0_GRL_02, new InstanceUid({sequenceOwnerUid}));";
        EditMethod(controller, Method(controller, "OnInit"), spawn, spawn + Environment.NewLine + "        " + alias);
        EVContainer edited = EventProjectCompiler.Compile(flat);
        string project = Export(edited, Garland, "extracted");
        string scene = File.ReadAllText(Scene(project, GarlandScene));

        // The VM offsets the sequence owner's UID by 64 in a byte, so only 199 wraps to Zidane's UID 7.
        Assert.Equal(collides, scene.Contains("RequestAndWait(evt.GEO_MAIN_F0_ZDN_07.Script_11, level: 2)", StringComparison.Ordinal));
        Assert.Equal(!collides, scene.Contains("RunAndWait(On(evt.GEO_MAIN_F0_ZDN_07, Action_07_11), level: 2)", StringComparison.Ordinal));
        Assert.Equal(collides, File.ReadAllText(Participant(project, 7)).Contains("[Script(11, 1)]", StringComparison.Ordinal));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void NestedRamuhActionsKeepTheirDistinctLiveInstanceUids()
    {
        EVContainer field = Fixture(Pinnacle);
        string project = SourceProjectTest.CopyWithoutComments(Export(field, Pinnacle, "extracted"));
        string sceneFile = Scene(project, PinnacleScene);
        Assert.Contains("TryStart(On(evt.GEO_MON_F0_RAM_02, Action_02_40, new InstanceUid(128)), level: 6)", Method(sceneFile, "Action_08_36").ToFullString());
        Assert.Contains("TryStart(On(evt.GEO_MON_F0_RAM_02, Action_02_41, new InstanceUid(129)), level: 6)", Method(sceneFile, "Action_02_40").ToFullString());
        Assert.Contains("GEO_MON_F0_RAM_02", Assert.Single(Method(sceneFile, "Action_02_41").ParameterList.Parameters).Type!.ToString());
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void AnExclusiveRegionRequestKeepsItsReceiverAndDirectoryEntry()
    {
        string flat = Export(Fixture(BlackWaltz), BlackWaltz, "flat", extract: false);
        string definition = Participant(flat, 1);
        ReplaceOnce(definition, "[ObjectSlot(1, 1, 0, 0, 1, 0, 0,", "[ObjectSlot(1, 1, 0, 0, 1, 0, 1,");
        ReplaceOnce(definition, "Object_01 : EventThread", "Object_01 : EventRegion");
        string controller = Participant(flat, 0);
        ReplaceOnce(controller, "scripts.StartThread(evt.Object_01);", "scripts.StartRegion(evt.Object_01);");
        EVContainer field = EventProjectCompiler.Compile(flat);
        string project = SourceProjectTest.CopyWithoutComments(Export(field, BlackWaltz, "extracted"));
        string sceneFile = Scene(project, "Scene_00_01");

        Assert.Contains("Object_01 : EventRegion", File.ReadAllText(Participant(project, 1)));
        Assert.Contains("TryStart(On(evt.Object_01, Action_01_11), level: 2)", File.ReadAllText(sceneFile));
        MethodDeclarationSyntax action = Method(sceneFile, "Action_01_11");
        Assert.Equal("Object_01", Assert.Single(action.ParameterList.Parameters).Type!.ToString());
        Assert.Contains("[Script(11, 1)]", action.ToFullString());
        EVContainer rebuilt = EventProjectCompiler.Compile(project);
        Assert.Equal(field.Serialize(), rebuilt.Serialize());
        Assert.Equal(1, rebuilt.Objects[1].Type);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void AnActionCatalogEntryCanNameItsProvenSceneAndMethod()
    {
        EVContainer field = Fixture(Garland);
        var catalog = new CutsceneCatalog([new(Garland, 7, 11, Name: "OpenWithZidane", Group: "GarlandScene")]);
        string project = Path.Combine(_directory, "catalog");
        EventProjectExporter.Export(field, Garland, project, useCutsceneNames: false, cutsceneCatalog: catalog);
        string sceneFile = Scene(project, "GarlandScene");

        Assert.Equal(sceneFile, Assert.Single(Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs")));
        Assert.Contains("await ctx.GarlandScene.Play(this);", File.ReadAllText(Participant(project, 0)));
        Assert.Contains("await scripts.RunAndWait(On(evt.GEO_MAIN_F0_ZDN_07, OpenWithZidane), level: 2)", File.ReadAllText(sceneFile));
        Assert.Contains("[Script(11, 1)]", Method(sceneFile, "OpenWithZidane").ToFullString());
        Assert.DoesNotContain("[Script(11, 1)]", File.ReadAllText(Participant(project, 7)));
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void ConflictingActionCatalogGroupsLeaveTheProvenSceneTogether()
    {
        EVContainer field = Fixture(Garland);
        var catalog = new CutsceneCatalog([new(Garland, 7, 11, Group: "ZidaneScene"), new(Garland, 2, 11, Group: "GarlandScene")]);
        string project = Path.Combine(_directory, "catalog");
        EventProjectExporter.Export(field, Garland, project, useCutsceneNames: false, cutsceneCatalog: catalog);
        string sceneFile = Scene(project, GarlandScene);

        Assert.Equal(sceneFile, Assert.Single(Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs")));
        Assert.Contains($"await ctx.{GarlandScene}.Play(this);", File.ReadAllText(Participant(project, 0)));
        Assert.Contains("Action_07_11", File.ReadAllText(sceneFile));
        Assert.Contains("Action_02_11", File.ReadAllText(sceneFile));
        string[] outline = File.ReadAllLines(Path.Combine(project, "CutsceneOutline.tsv"));
        Assert.Contains(outline, row => row.StartsWith("7\t11\tCatalog entry\t\tInvalid or conflicting group name", StringComparison.Ordinal));
        Assert.Contains(outline, row => row.StartsWith("2\t11\tCatalog entry\t\tInvalid or conflicting group name", StringComparison.Ordinal));
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void ASharedSequenceCannotBindToANonInitialEntry()
    {
        string project = Export(Fixture(EvilForest), EvilForest, "extracted");
        string sceneFile = Scene(project, "Scene_00_01");
        EditMethod(sceneFile, Method(sceneFile, "Action_03_00"), "[Script(0, 0)]", "[Script(11, 0)]");
        AssertRejected(project, "A shared script requires the initial entry of a participant definition");
    }

    [Theory]
    [InlineData("0")]
    [InlineData("250")]
    [InlineData("(byte)Globals.FieldEntrance")]
    public void ADeferredBindingRequiresAConcreteLiveInstanceUid(string uid)
    {
        string project = Export(Fixture(Pinnacle), Pinnacle, "extracted");
        string sceneFile = Scene(project, PinnacleScene);
        EditMethod(sceneFile, Method(sceneFile, "Action_08_36"), "Action_02_40, new InstanceUid(128)", $"Action_02_40, new InstanceUid({uid})");
        AssertRejected(project, uid == "(byte)Globals.FieldEntrance" ? "constant" : "Use a concrete live instance UID");
    }

    [Theory]
    [InlineData(Garland, GarlandScene, "Action_07_11", "EventActor")]
    [InlineData(Pandemonium, "Scene_00_19", "Action_01_11", "EventThread")]
    public void RequestMethodGroupsResolveToTheTypedActionInTheScene(string name, string sceneName, string actionName, string receiverBase)
    {
        string project = Export(Fixture(name), name, "navigation");
        SyntaxTree[] trees = Directory.GetFiles(project, "*.cs", SearchOption.AllDirectories)
            .Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path)).ToArray();
        var compilation = CSharpCompilation.Create(name, trees, SourceProjectTest.EventReferences(project), new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        Assert.Empty(compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        SyntaxTree scene = Assert.Single(trees.Where(tree => tree.FilePath == Scene(project, sceneName)));
        IdentifierNameSyntax reference = Assert.Single(scene.GetRoot().DescendantNodes().OfType<IdentifierNameSyntax>().Where(identifier =>
            identifier.Identifier.ValueText == actionName));
        var resolved = Assert.IsAssignableFrom<IMethodSymbol>(compilation.GetSemanticModel(scene).GetSymbolInfo(reference).Symbol);

        Assert.Equal(scene, Assert.Single(resolved.Locations).SourceTree);
        Assert.Equal(receiverBase, Assert.Single(resolved.Parameters).Type.BaseType!.Name);
        Assert.Equal(sceneName, resolved.ContainingType.Name);
    }

    [Fact]
    public void MovingTheSpriteThreadPreservesItsFirstVisitAndWaitFrames()
    {
        EVContainer field = Fixture(Pandemonium);
        string flat = Export(field, Pandemonium, "flat", extract: false);
        string extracted = Export(field, Pandemonium, "extracted");

        var reference = Run(flat);
        Assert.Collection(reference, first => Assert.Equal((2, 1220), first), last => Assert.Equal((120, -1), last));
        Assert.Equal(reference, Run(extracted));

        static List<(int Frame, int Sprite)> Run(string project)
        {
            var host = new SpriteHost();
            var scheduler = new EventScheduler(host, new byte[4096], new byte[80]);
            scheduler.RegisterProject(Assembly.Load(EventProjectCompiler.CompileManaged(project)));
            scheduler.CreateParticipant(1);
            scheduler.Tick();
            scheduler.Tick();
            Assert.True(scheduler.TryTrigger(1, 11, 4));
            for (int frame = 0; frame < 140; frame++)
            {
                host.Frame = scheduler.Frame + 1;
                scheduler.Tick();
            }
            return host.Sprites;
        }
    }

    private sealed class SpriteHost : IEventHost
    {
        public int Frame { get; set; }
        public List<(int Frame, int Sprite)> Sprites { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            const int loadSprite = 130;
            if (objectId == 1 && opcode == Opcode.SPS && (int)operands[0] == 0 && (int)operands[1] == loadSprite)
                Sprites.Add((Frame, (int)operands[2]));
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;
        public bool IsWindowOpen(int objectId, int windowId) => false;
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
    private static string Action(int slot, int tag) => $"Action_{slot:D2}_{tag:D2}";

    private static MethodDeclarationSyntax Method(string file, string name) => Assert.Single(CSharpSyntaxTree.ParseText(File.ReadAllText(file))
        .GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Identifier.ValueText == name));

    private static void EditMethod(string file, MethodDeclarationSyntax method, string before, string after)
    {
        string source = File.ReadAllText(file);
        string original = method.ToFullString();
        Assert.Equal(2, original.Split(before, StringSplitOptions.None).Length);
        string edited = original.Replace(before, after, StringComparison.Ordinal);
        File.WriteAllText(file, source.Remove(method.FullSpan.Start, method.FullSpan.Length).Insert(method.FullSpan.Start, edited));
    }

    private static void ReplaceOnce(string file, string before, string after)
    {
        string source = File.ReadAllText(file);
        Assert.Equal(2, source.Split(before, StringSplitOptions.None).Length);
        File.WriteAllText(file, source.Replace(before, after, StringComparison.Ordinal));
    }

    private static void AssertRejected(string project, string reason)
    {
        Assert.Contains(reason, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
        Assert.Contains(reason, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project)).Message);
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
