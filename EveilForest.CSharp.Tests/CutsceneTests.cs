using System.Reflection;
using EveilForest.CSharp;
using EvilForest.EventSource;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using static EveilForest.CSharp.Tests.SourceProjectTest;

namespace EveilForest.CSharp.Tests;

public sealed class CutsceneTests : IDisposable
{
    private const string Phantom = "EVT_LAST_CW_LTP_0";
    private const string Forest = "EVT_EFOREST1_EF_DEP_0";
    private const string Kitchen = "EVT_SARI1_MS_KTN_0";
    private const string Cargo = "EVT_ALEX1_TS_CARGO_0";
    private readonly string _directory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), $"evilforest-cutscenes-{Guid.NewGuid():N}")).FullName;

    [Fact]
    public void NumericTruthConversionProducesZeroOrOneInManagedAndEmittedCode()
    {
        var input = new EVVariable(EVStorageScope.Global, EVStorageType.Int16, 9).Encode();
        var target = new EVVariable(EVStorageScope.Map, EVStorageType.Byte, 24).Encode();
        var body = new EVInstructionBody([
            new(0, FF8.JSM.Jsm.Opcode.EXPR) { Expression = [target, input, new((byte)FF8.JSM.Jsm.Excode.Let)] },
            new(1, FF8.JSM.Jsm.Opcode.Return)
        ], 2);
        var root = new EVContainerObject(0, 0, 0, 0) { BodyOrder = 0, Scripts = [new(0, body.Encode())] };
        byte[] header = EVContainer.CreateHeader(EVContainer.SupportedVersion, new byte[EVContainer.HeaderSize - EVContainer.HeaderMetadataOffset]);
        var baseline = new EVContainer(header, [root], []);
        string project = Path.Combine(_directory, "normalized");
        EventProjectExporter.Export(baseline, "EVT_NORMALIZATION_TEST", project);
        string path = Path.Combine(project, "00_Object_00.cs");
        string source = File.ReadAllText(path);
        Assert.Contains("= Globals.EnvironmentSoundSlot0Id;", source);
        File.WriteAllText(path, source.Replace("= Globals.EnvironmentSoundSlot0Id;", "= (Globals.EnvironmentSoundSlot0Id is not 0) ? 1 : 0;", StringComparison.Ordinal));
        EVContainer compiled = EventProjectCompiler.Compile(project);
        EVExpressionToken[] expression = EVInstructionBody.Decode(compiled.Objects[0].Scripts[0].Body).Instructions[0].Expression;
        Assert.Equal(new[] { target, input, EVExpressionToken.Literal(0), new((byte)FF8.JSM.Jsm.Excode.NotEquivalence), new((byte)FF8.JSM.Jsm.Excode.Let) }, expression);
        MemoriaRuntimeTests.VerifyCompiledTruthOperator(expression[3].Opcode, [0, 1, 7, -7]);
        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        foreach (short value in new short[] { 0, 1, 7, -7 })
        {
            var globals = new byte[4096];
            var location = new byte[80];
            System.Buffers.Binary.BinaryPrimitives.WriteInt16LittleEndian(globals.AsSpan(9), value);
            var scheduler = new EventScheduler(new TraceHost(0, 0), globals, location);
            scheduler.RegisterProject(assembly);
            scheduler.StartScene();
            scheduler.Tick();
            Assert.Equal(value == 0 ? 0 : 1, location[24]);
        }
    }

    [Fact]
    public void ComputedInputAndExplicitExtendedIconsRoundTrip()
    {
        string project = Export(Cargo, "typed-input");
        string hero = Path.Combine(project, "Cutscenes", "LightTheCandle.cs");
        string source = File.ReadAllText(hero);
        const string confirm = "Input.IsKeyPressed(PlayerInput.Confirm)";
        Assert.Contains(confirm, source);
        File.WriteAllText(hero, source.Replace(confirm, "Input.IsKeyPressed((PlayerInput)Globals.ScenarioCounter)", StringComparison.Ordinal));
        EVContainer computed = EventProjectCompiler.Compile(project);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

        string icon = Directory.GetFiles(project, "*.cs").First(file => File.ReadAllText(file).Contains("Input.Bubble(symbol: BubbleSymbol.", StringComparison.Ordinal));
        string iconSource = File.ReadAllText(icon);
        const string symbol = "Input.Bubble(symbol: BubbleSymbol.";
        int start = iconSource.IndexOf(symbol, StringComparison.Ordinal);
        int end = iconSource.IndexOf(')', start);
        string edit = iconSource[start..end];
        File.WriteAllText(icon, iconSource.Replace(edit, "Input.Bubble(symbol: BubbleSymbol.Beach", StringComparison.Ordinal));
        EVContainer named = EventProjectCompiler.Compile(project);
        Assert.NotEqual(computed.Serialize(), named.Serialize());
        File.WriteAllText(icon, iconSource.Replace(edit, "Input.Bubble(symbol: (BubbleSymbol)5", StringComparison.Ordinal));
        AssertBothReject(project, "Use a declared BubbleSymbol");
        File.WriteAllText(icon, iconSource.Replace(edit, "Input.BubbleFromScript(symbol: 5", StringComparison.Ordinal));
        EVContainer extended = EventProjectCompiler.Compile(project);
        string reopened = Path.Combine(_directory, "typed-reopened");
        EventProjectExporter.Export(extended, Cargo, reopened, useCutsceneNames: false, allowNativePhaseDispatch: true);
        Assert.Equal(extended.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Theory]
    [InlineData(Phantom, "ChallengeTheDarkPhantom", "scripts.RunAndWait(On(")]
    [InlineData(Forest, "PlantBrain", "internal async EventRoutine RescueGarnet")]
    [InlineData(Kitchen, "AssignKitchenJobs", "internal async EventRoutine ChooseHelpers")]
    [InlineData(Cargo, "LightTheCandle", "await play.Together(3, Barrier(() => Location.SceneActorsRemaining)")]
    public void NamedAndUnnamedCutscenesBuildFromSourceAlone(string name, string file, string expected)
    {
        byte[] original = ReadFixture(name);
        string named = Export(name, "named");
        string text = File.ReadAllText(Path.Combine(named, "Cutscenes", file + ".cs"));
        Assert.Contains(expected, text);
        if (name == Phantom)
        {
            Assert.Contains("scripts.TryStart(On(", text);
            Assert.DoesNotContain("goto", text);
        }
        if (name == Forest)
        {
            Assert.Contains("scripts.StartWhenReady(On(", text);
            Assert.Contains("scripts.RunAndWait(On(", text);
        }
        if (name == Kitchen)
        {
            Assert.Contains("TurnTowardCatch", File.ReadAllText(Path.Combine(named, "Cutscenes", "TheFishingLine.cs")));
            Assert.Contains("await ContinueAfterHelperDispatch(participant);", text);
            Assert.Contains("instance: new InstanceUid(135)", text);
            Assert.Contains("instance: new InstanceUid(136)", text);
            Assert.Contains("targetUid: 133", text);
        }
        string first = Export(name, "first", useNames: false);
        string second = Export(name, "second", useNames: false);
        var snapshot = Snapshot(first);
        var repeated = Snapshot(second);
        Assert.Equal(snapshot.Keys, repeated.Keys);
        foreach (string path in snapshot.Keys)
            Assert.Equal(snapshot[path], repeated[path]);
        foreach (string project in new[] { named, CopyWithoutComments(named), first, CopyWithoutComments(first) })
        {
            Assert.Equal(original, EventProjectCompiler.Compile(project).Serialize());
            Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
            RunDotnet(["build", ProjectFile(project), "-v", "q"]);
        }
    }

    [Theory]
    [InlineData(Phantom, "ChallengeTheDarkPhantom", 97, 98, 0xAA2)]
    [InlineData(Forest, "PlantBrain", 145, 146, 0x6D7)]
    [InlineData(Kitchen, "AssignKitchenJobs", 268, 269, 0x4ABA)]
    public void EditingACutsceneMessageChangesOnlyItsOriginalOperand(string name, string file, int from, int to, int offset)
    {
        byte[] expected = ReadFixture(name);
        expected[offset] = unchecked((byte)to);
        string project = Export(name, "edit");
        string path = Path.Combine(project, "Cutscenes", file + ".cs");
        string source = File.ReadAllText(path);
        string original = $"messageId: {from})";
        Assert.Contains(original, source);
        File.WriteAllText(path, source.Replace(original, $"messageId: {to})", StringComparison.Ordinal));
        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(expected, edited.Serialize());
        string reexport = Path.Combine(_directory, "reexport");
        EventProjectExporter.Export(edited, name, reexport);
        Assert.Contains($"messageId: {to})", File.ReadAllText(Path.Combine(reexport, "Cutscenes", file + ".cs")));
    }

    [Fact]
    public async Task PhantomActionsSupportNavigationRenameAndPartialFileMoves()
    {
        string project = Export(Phantom, "navigation");
        using var workspace = new AdhocWorkspace();
        ProjectId id = ProjectId.CreateNewId();
        Solution solution = workspace.CurrentSolution.AddProject(ProjectInfo.Create(id, VersionStamp.Create(), "Phantom", "Phantom", LanguageNames.CSharp,
            compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary), metadataReferences: EventReferences(project)));
        foreach (string path in Directory.GetFiles(project, "*.cs", SearchOption.AllDirectories))
            solution = solution.AddDocument(DocumentId.CreateNewId(id), Path.GetFileName(path), SourceText.From(File.ReadAllText(path)), filePath: path);

        Project roslynProject = solution.GetProject(id)!;
        Document callback = roslynProject.Documents.Single(document => document.Name.StartsWith("06_", StringComparison.Ordinal));
        Document cutscene = roslynProject.Documents.Single(document => document.Name == "ChallengeTheDarkPhantom.cs");
        var callbackTarget = await ResolveMethod(callback, ".Challenge(this)", 1);
        var lookUp = await ResolveMethod(cutscene, ", LookUp)", 2);
        var lowerHead = await ResolveMethod(cutscene, ", LowerHead)", 2);
        Assert.All(new[] { callbackTarget, lookUp, lowerHead }, symbol => Assert.Equal(cutscene.FilePath, Assert.Single(symbol.Locations).SourceTree!.FilePath));

        solution = await Renamer.RenameSymbolAsync(solution, lookUp, new SymbolRenameOptions(), "RaiseHead");
        Document renamed = solution.GetDocument(cutscene.Id)!;
        var root = (CompilationUnitSyntax)(await renamed.GetSyntaxRootAsync())!;
        var scene = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
        MethodDeclarationSyntax action = scene.Members.OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "RaiseHead");
        File.WriteAllText(renamed.FilePath!, root.RemoveNode(action, SyntaxRemoveOptions.KeepNoTrivia)!.ToFullString());
        ClassDeclarationSyntax partial = scene.WithParameterList(null).WithBaseList(null).WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(action));
        File.WriteAllText(Path.Combine(project, "Cutscenes", "RaiseHead.cs"), root.ReplaceNode(scene, partial).NormalizeWhitespace().ToFullString());
        Assert.Equal(ReadFixture(Phantom), EventProjectCompiler.Compile(project).Serialize());
        RunDotnet(["build", ProjectFile(project), "-v", "q"]);
    }

    private static async Task<IMethodSymbol> ResolveMethod(Document document, string call, int methodOffset)
    {
        int position = (await document.GetTextAsync()).ToString().IndexOf(call, StringComparison.Ordinal);
        Assert.True(position >= 0);
        return Assert.IsAssignableFrom<IMethodSymbol>(await SymbolFinder.FindSymbolAtPositionAsync(document, position + methodOffset));
    }

    [Fact]
    public void CutsceneReceiversWithUnverifiedEvaluationAreRejectedByBothCompilers()
    {
        string project = Export(Phantom, "receivers");
        string callback = Directory.GetFiles(project, "06_*.cs").Single();
        string baseline = File.ReadAllText(callback);
        Assert.Contains("ctx.ChallengeTheDarkPhantom.Challenge(this)", baseline);
        foreach (string receiver in new[] { "GetCutscene()", "OtherCutscene" })
        {
            string source = baseline.Replace("ctx.ChallengeTheDarkPhantom.Challenge(this)", $"{receiver}.Challenge(this)", StringComparison.Ordinal);
            File.WriteAllText(callback, source.Insert(source.LastIndexOf('}'), """
                private ChallengeTheDarkPhantom GetCutscene()
                {
                    Globals.ScenarioCounter = 7;
                    return ctx.ChallengeTheDarkPhantom;
                }
                private ChallengeTheDarkPhantom OtherCutscene => GetCutscene();
                """));
            AssertBothReject(project, "registered cutscene instance");
        }
        File.WriteAllText(callback, baseline);
        string scene = Path.Combine(project, "Cutscenes", "ChallengeTheDarkPhantom.cs");
        string content = File.ReadAllText(scene).Replace(", LookUp)", ", GetCutscene().LookUp)", StringComparison.Ordinal);
        File.WriteAllText(scene, content.Insert(content.LastIndexOf('}'), """
            private ChallengeTheDarkPhantom GetCutscene()
            {
                Globals.ScenarioCounter = 7;
                return this;
            }
            """));
        AssertBothReject(project, "deferred cutscene action");
    }

    [Fact]
    public void CutsceneEntryHasOneAuthoritativeBinding()
    {
        string project = Export(Phantom, "bindings");
        string path = Path.Combine(project, "Scene.cs");
        string content = File.ReadAllText(path);
        Assert.DoesNotContain("[SceneCutscene]", content);
        Assert.Equal(ReadFixture(Phantom), EventProjectCompiler.Compile(project).Serialize());
        content = content.Replace("Scene = new Scene(this);", "Scene = new Scene(this); Other = new ChallengeTheDarkPhantom(this);", StringComparison.Ordinal)
            .Replace("    internal Location", "    internal ChallengeTheDarkPhantom Other { get; }\n    internal Location", StringComparison.Ordinal);
        File.WriteAllText(path, content);
        AssertBothReject(project, "unique get-only cutscene property");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void EditedCardHandlersCanBeOpenedAgain(int decks)
    {
        string project = Export(Phantom, "decks");
        string path = Path.Combine(project, "Cutscenes", "ChallengeTheDarkPhantom.cs");
        string source = File.ReadAllText(path);
        const string first = "await Inventory.TetraMaster(cardDeck: 122);";
        const string second = "await Inventory.TetraMaster(cardDeck: 123);";
        Assert.Contains(first, source);
        string replacement = decks switch
        {
            0 => "",
            1 => first,
            2 => first + "\n" + second,
            _ => $"if (Globals.ScenarioCounter > 7) {{ {first} }} else {{ {second} }}"
        };
        File.WriteAllText(path, source.Replace(first, replacement, StringComparison.Ordinal));
        EVContainer edited = EventProjectCompiler.Compile(project);
        string reopened = Path.Combine(_directory, "reopened");
        EventProjectExporter.Export(edited, Phantom, reopened, useCutsceneNames: false);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(reopened));
        RunDotnet(["build", ProjectFile(reopened), "-v", "q"]);
    }

    private static void AssertBothReject(string project, string message)
    {
        RunDotnet(["build", ProjectFile(project), "-v", "q"]);
        Assert.Contains(message, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
        Assert.Contains(message, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project)).Message);
    }

    [Fact]
    public void ComputedDialogueArgumentsCannotBeReorderedSilently()
    {
        string project = Export(Phantom, "dialogue-order");
        string path = Path.Combine(project, "Cutscenes", "ChallengeTheDarkPhantom.cs");
        string source = File.ReadAllText(path);
        const string original = "new DialogueLine(windowId: 0, flags: 4, messageId: 96)";
        Assert.Contains(original, source);
        File.WriteAllText(path, source.Replace(original,
            "new DialogueLine(messageId: Globals.ScenarioCounter++, flags: 4, windowId: Globals.ScenarioCounter)", StringComparison.Ordinal));
        AssertBothReject(project, "preserve evaluation order");
    }

    [Fact]
    public void RelocatingCargoPhaseStoragePreservesTheDiscoveredActions()
    {
        string project = Export(Cargo, "before-relocation", useNames: false);
        string path = Path.Combine(project, "Event.cs");
        string source = File.ReadAllText(path);
        const string oldAddress = "FieldOffset(24)]";
        Assert.Contains(oldAddress, source);
        File.WriteAllText(path, source.Replace(oldAddress, "FieldOffset(60)]", StringComparison.Ordinal));
        EVContainer relocated = EventProjectCompiler.Compile(project);
        string reopened = Path.Combine(_directory, "relocated");
        EventProjectExporter.Export(relocated, Cargo, reopened, useCutsceneNames: false, allowNativePhaseDispatch: true);
        string cutscene = Directory.GetFiles(Path.Combine(reopened, "Cutscenes"), "*.cs")
            .Select(File.ReadAllText).Single(code => code.Contains("messageId: 39)", StringComparison.Ordinal) && code.Contains("Engine.Menu", StringComparison.Ordinal));
        Assert.Contains("messageId: 41)", cutscene);
        Assert.Contains("FieldOffset(60)", File.ReadAllText(Path.Combine(reopened, "Event.cs")));
        Assert.Equal(relocated.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Theory]
    [InlineData("EVT_ALEX1_TS_MEET_0", "messageId: 53)", "03_")]
    [InlineData("EVT_ALEX1_TS_STAGE_1A", "messageId:", "08_")]
    [InlineData("EVT_EFOREST1_EF_EXT_0", "Field.ChangeMap", "00_")]
    [InlineData("EVT_ALEX1_TS_OPENING", "VisualEffects.ControlCinematic", "00_")]
    public void AdditionalFieldEpisodesAreDiscoveredWithoutNames(string name, string action, string owner)
    {
        string project = Export(name, "episode", useNames: false);
        Assert.Contains(Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs"), file => File.ReadAllText(file).Contains(action, StringComparison.Ordinal));
        string[] outline = File.ReadAllLines(Path.Combine(project, "CutsceneOutline.tsv"));
        Assert.Contains(outline, row => row.Contains($"\textracted\t{owner}", StringComparison.Ordinal));
        Assert.Equal(ReadFixture(name), EventProjectCompiler.Compile(CopyWithoutComments(project)).Serialize());
    }

    [Fact]
    public void HeldOutPalaceDialogueUsesOneSceneAndAnOrdinaryGreetingRemainsLocal()
    {
        const string palace = "EVT_BURMECIA_PALACE_0";
        string story = Export(palace, "held-out-palace", useNames: false, allowNativePhaseDispatch: true);
        string scene = Directory.GetFiles(Path.Combine(story, "Cutscenes"), "*.cs").Select(File.ReadAllText)
            .Single(code => code.Contains("messageId: 196)", StringComparison.Ordinal));
        Assert.Contains("messageId: 200)", scene);
        Assert.Contains("messageId: 210)", scene);
        Assert.DoesNotContain("EVT_BURMECIA_SQUARE_0", scene);
        Assert.Equal(ReadFixture(palace), EventProjectCompiler.Compile(CopyWithoutComments(story)).Serialize());

        const string treno = "EVT_TRENO2_TR_WBH_0";
        string ambient = Export(treno, "held-out-conversation", useNames: false);
        string local = Directory.GetFiles(ambient, "05_*.cs").Single();
        Assert.Contains("messageId: 284)", File.ReadAllText(local));
        Assert.Equal(ReadFixture(treno), EventProjectCompiler.Compile(CopyWithoutComments(ambient)).Serialize());
    }

    [Fact]
    public void DiagnosticMemoryIncludesTheCoordinatorAndCannotChangeEitherCompiler()
    {
        byte[] original = ReadFixture(Cargo);
        string project = Export(Cargo, "diagnostics");
        Assert.False(File.Exists(Path.Combine(project, "CutsceneMemory.tsv")));
        string reports = Path.Combine(_directory, "reports");
        EventProjectExporter.DiagnoseCutsceneMemory(EVContainer.Read(new MemoryStream(original)), Cargo, reports);
        string report = Directory.GetFiles(reports).Single();
        Assert.Contains(File.ReadAllLines(report), row => row.StartsWith("3\t1\tMap\t-1\t28\t80\tC0\t", StringComparison.Ordinal));
        File.WriteAllText(Path.Combine(project, "CutsceneMemory.tsv"), "not a table");
        File.WriteAllText(Path.Combine(project, "CutsceneOutline.tsv"), "not a table either");
        Assert.Equal(original, EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void LocalCatalogGroupsExistingPhasesAndReportsStaleEntries()
    {
        var catalog = new CutsceneCatalog(
        [
            new(Kitchen, 36, 1, 3, "ChooseTheHelpers", "KitchenStory", "FBCECEDC61E78E00CA88DB67"),
            new(Kitchen, 7, 1, 10, "MoogleCallsOut", "KitchenStory", "E3777608657656A8C6B9F477"),
            new(Kitchen, 36, 1, 4, "AssignTheJobs", "KitchenStory", "7E08826086BF8731D686BB15",
                Roles: new Dictionary<string, string> { ["Byte_292"] = "Angler" }),
            new(Kitchen, 36, 1, 999, "Stale", Fragment: "0123456789ABCDEF01234567")
        ]);
        string project = Export(Kitchen, "catalog", useNames: false, catalog);
        string source = File.ReadAllText(Path.Combine(project, "Cutscenes", "KitchenStory.cs"));
        Assert.Contains("ChooseTheHelpers", source);
        Assert.Contains("MoogleCallsOut", source);
        Assert.Contains("cast.Resolve(jobs.Angler)", source);
        Assert.DoesNotContain("Stale", source);
        Assert.Contains("Stale or ambiguous catalog entry", File.ReadAllText(Path.Combine(project, "CutsceneOutline.tsv")));
        Assert.Equal(ReadFixture(Kitchen), EventProjectCompiler.Compile(CopyWithoutComments(project)).Serialize());
    }

    [Theory]
    [InlineData(4, 0, 100)]
    [InlineData(5, 1, 99)]
    [InlineData(5, 0, 98)]
    public void PhantomRunsOnItsOriginalOwnersAndAllowsReentry(int cards, int choice, int result)
    {
        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(Export(Phantom, "trace")));
        var location = new byte[80];
        location[19] = 0x80;
        var host = new TraceHost(cards, choice);
        var scheduler = new EventScheduler(host, new byte[4096], location) { ControlledActorId = 8 };
        scheduler.RegisterProject(assembly);
        scheduler.CreateParticipant(6);
        scheduler.CreateParticipant(8);
        for (int index = 0; index < 10; index++)
            scheduler.Tick();
        for (int attempt = 0; attempt < 2; attempt++)
        {
            host.Commands.Clear();
            Assert.True(scheduler.TryTrigger(6, 8, 0));
            for (int index = 0; index < 100; index++)
            {
                scheduler.Tick();
                if (scheduler.IsExternalModeActive)
                {
                    Assert.Equal(98, result);
                    Assert.Equal(1, location[20] & 1);
                    Assert.Equal(0, location[19] & 0x40);
                    Assert.DoesNotContain(host.Commands, command => command.Opcode == Opcode.WIPERGB && command.Arguments[0] == 7);
                    int commands = host.Commands.Count;
                    int frame = scheduler.Frame;
                    scheduler.Tick();
                    Assert.Equal(commands, host.Commands.Count);
                    Assert.Equal(frame, scheduler.Frame);
                    scheduler.ResumeExternalMode();
                }
            }
            Assert.Contains(host.Commands, command => command.Owner == 6 && command.Opcode == Opcode.MES && command.Arguments[2] == result);
            Assert.Contains(host.Commands, command => command.Owner == 8 && command.Opcode == Opcode.ANIM && command.Arguments[0] == Resources.ANH_MAIN_F0_ZDN_LOOK_UP_1_3628);
            Assert.Contains(host.Commands, command => command.Owner == 8 && command.Opcode == Opcode.ANIM && command.Arguments[0] == Resources.ANH_MAIN_F0_ZDN_LOOK_UP_3_3635);
            Assert.Equal(result == 98, host.Commands.Any(command => command.Owner == 6 && command.Opcode == Opcode.MINIGAME && command.Arguments[0] == 122));
            Assert.Equal(0, location[20] & 1);
            if (result == 98)
                Assert.Single(host.Commands.Where(command => command.Opcode == Opcode.WIPERGB && command.Arguments[0] == 7));
            Assert.NotEqual(0, location[19] & 0x40);
        }
    }

    [Theory]
    [InlineData(2280, 137, 180)]
    [InlineData(2300, 145, 0)]
    public void PlantBrainKeepsIndependentEntrancesAndViviDoesNotWaitForZidane(int story, int message, int delay)
    {
        string extractedProject = Export(Forest, "extracted");
        string referenceProject = Export(Forest, "reference", extract: false);
        if (story == 2280)
        {
            foreach (string path in new[] { Path.Combine(extractedProject, "Cutscenes", "PlantBrain.cs"), Path.Combine(referenceProject, "15_Zidane.cs") })
            {
                string source = File.ReadAllText(path);
                Assert.Contains("WalkTo(x: 1534, z: -416)", source);
                File.WriteAllText(path, source.Replace("WalkTo(x: 1534, z: -416)", "WalkTo(x: 1734, z: -416)", StringComparison.Ordinal));
            }
            Assert.Equal(EventProjectCompiler.Compile(referenceProject).Serialize(), EventProjectCompiler.Compile(extractedProject).Serialize());
        }
        Assembly extracted = Assembly.Load(EventProjectCompiler.CompileManaged(extractedProject));
        Assembly reference = Assembly.Load(EventProjectCompiler.CompileManaged(referenceProject));
        Assert.Equal(Run(reference), Run(extracted));

        (int Dialogue, int Encounter, int Aftermath) Run(Assembly assembly)
        {
            var globals = new byte[4096];
            var entry = Start(story, message, delay);
            int dialogue = -1;
            for (int index = 0; index < 500 && !entry.Scheduler.HasEnded; index++)
            {
                entry.Scheduler.Tick();
                if (dialogue == -1 && entry.Host.ReachedLine)
                {
                    if (story == 2300)
                        return (-1, -1, entry.Scheduler.Frame);
                    dialogue = entry.Scheduler.Frame;
                    Assert.Equal(0, entry.Location[30]);
                    Assert.Equal(1, entry.Location[31]);
                }
            }
            Assert.True(entry.Host.ReachedEncounter);
            Assert.InRange(dialogue, 0, entry.Scheduler.Frame - 1);

            // The host returns from battle to a newly initialized field with the updated story counter.
            var afterBattle = Start(2300, 145, 0);
            for (int index = 0; index < 500 && !afterBattle.Host.ReachedLine; index++)
                afterBattle.Scheduler.Tick();
            Assert.True(afterBattle.Host.ReachedLine);
            Assert.False(afterBattle.Host.ReachedEncounter);
            return (dialogue, entry.Scheduler.Frame, afterBattle.Scheduler.Frame);

            (EventScheduler Scheduler, ForestHost Host, byte[] Location) Start(int counter, int line, int slowWalk)
            {
                globals[0] = unchecked((byte)counter);
                globals[1] = (byte)(counter >> 8);
                var location = new byte[80];
                var host = new ForestHost(line, slowWalk);
                var scheduler = new EventScheduler(host, globals, location);
                scheduler.RegisterProject(assembly);
                scheduler.StartScene();
                return (scheduler, host, location);
            }
        }
    }

    [Theory]
    [InlineData(39, 5, false)]
    [InlineData(40, 15, false)]
    [InlineData(39, 5, true)]
    public void CargoRevealPreservesTheActorsAndCoordinatorPolling(int message, int wait, bool reverseTracks)
    {
        byte[] original = reverseTracks ? CargoWithComputedPollingDelay() : ReadFixture(Cargo);
        string reference = Path.Combine(_directory, "cargo-reference");
        string extracted = Path.Combine(_directory, "cargo-extracted");
        EventProjectExporter.Export(EVContainer.Read(new MemoryStream(original)), Cargo, reference, extractCutscenes: false);
        EventProjectExporter.Export(EVContainer.Read(new MemoryStream(original)), Cargo, extracted, allowNativePhaseDispatch: reverseTracks);
        Edit(Path.Combine(reference, "22_Cinna.cs"), "messageId: 39);", $"messageId: {message});");
        string sceneFile = Path.Combine(extracted, "Cutscenes", "LightTheCandle.cs");
        Edit(sceneFile, "messageId: 39);", $"messageId: {message});");
        if (reverseTracks)
        {
            const string zidane = "Track(ctx.Scene.Zidane, TurnToCinnaAndOpenNameMenu)";
            const string cinna = "Track(ctx.Scene.Cinna, CallFromTheDark)";
            Edit(sceneFile, $"{zidane}, {cinna}", $"{cinna}, {zidane}");
            Edit(sceneFile, $"messageId: {message});", $"messageId: {message});{Environment.NewLine}        return;");
        }
        foreach (string path in new[] { Path.Combine(reference, "17_Zidane.cs"), sceneFile })
        {
            string source = File.ReadAllText(path);
            string startMarker = path == sceneFile ? "EventRoutine TurnToCinnaAndOpenNameMenu(" : "Location.SceneActorsRemaining = 2;";
            int start = source.IndexOf(startMarker, StringComparison.Ordinal);
            int position = source.IndexOf("await Engine.Wait(frames: 5);", start, StringComparison.Ordinal);
            Assert.True(position >= 0);
            File.WriteAllText(path, source.Remove(position, "await Engine.Wait(frames: 5);".Length).Insert(position, $"await Engine.Wait(frames: {wait});"));
        }

        byte[] expected = EventProjectCompiler.Compile(reference).Serialize();
        Assert.Equal(expected, EventProjectCompiler.Compile(extracted).Serialize());
        if (message == 39)
            Assert.Equal(original, expected);
        else
        {
            string reexport = Path.Combine(_directory, "cargo-reexport");
            EventProjectExporter.Export(EVContainer.Read(new MemoryStream(expected)), Cargo, reexport, allowNativePhaseDispatch: true);
            Assert.Contains($"messageId: {message})", File.ReadAllText(Path.Combine(reexport, "Cutscenes", "LightTheCandle.cs")));
        }
        var baseline = Run(reference);
        var edited = Run(extracted);
        Assert.Equal(baseline.Timing, edited.Timing);
        Assert.Equal(baseline.Commands, edited.Commands);
        Assert.Equal(baseline.Polling, edited.Polling);

        ((int Menu, int MenuReturn, int Window, int Joined, int Reply) Timing, (int Frame, int Actor, Opcode Opcode, string Operands)[] Commands, (int Frame, byte Step, byte Remaining)[] Polling) Run(string project)
        {
            const int sceneStep = 24;
            const int actorsRemaining = 26;
            var location = new byte[80];
            location[sceneStep] = 3;
            var host = new CargoHost(message);
            var scheduler = new EventScheduler(host, new byte[4096], location) { ControlledActorId = 17 };
            scheduler.RegisterProject(Assembly.Load(EventProjectCompiler.CompileManaged(project)));
            scheduler.CreateParticipant(3);
            scheduler.CreateParticipant(17);
            scheduler.CreateParticipant(22);
            var polling = new List<(int Frame, byte Step, byte Remaining)>();
            int joined = -1;
            for (int tick = 0; tick < 200; tick++)
            {
                host.Frame = tick;
                scheduler.Tick();
                polling.Add((tick, location[sceneStep], location[actorsRemaining]));
                if (joined == -1 && location[sceneStep] == 4)
                {
                    joined = tick;
                    Assert.Equal(0, location[actorsRemaining]);
                }
                if (host.Reply != -1)
                {
                    Assert.True(host.Menu < host.WindowClosed);
                    Assert.True(host.MenuReturn < host.WindowClosed);
                    Assert.True(host.WindowClosed < joined);
                    Assert.True(joined < host.Reply);
                    return ((host.Menu, host.MenuReturn, host.Window, joined, host.Reply), host.Commands.ToArray(), polling.ToArray());
                }
            }
            throw new InvalidOperationException($"Cargo never reached the reply. Step {location[sceneStep]}, remaining {location[actorsRemaining]}, menu {host.Menu}, window {host.Window}.");
        }

        static void Edit(string file, string before, string after)
        {
            string source = File.ReadAllText(file);
            Assert.Contains(before, source);
            File.WriteAllText(file, source.Replace(before, after, StringComparison.Ordinal));
        }
    }

    [Theory]
    [InlineData("", "Every parallel scene track needs a participant handler arm")]
    [InlineData("await ctx.LightTheCandle.PlayCandleReveal((Zidane)(EventActor)this);", "Pass the acting participant directly to the parallel phase")]
    [InlineData("await ((SceneContext)(object)(Zidane)(object)ctx).LightTheCandle.PlayCandleReveal(this);", "Use the registered cutscene instance")]
    public void CargoParallelTracksRequireEveryActingParticipant(string replacement, string diagnostic)
    {
        string project = ExportIndependentParallelPhase("cargo-incomplete-parallel");
        string path = Path.Combine(project, "22_Cinna.cs");
        string source = File.ReadAllText(path);
        const string enter = "await ctx.LightTheCandle.PlayCandleReveal(this);";
        Assert.Contains(enter, source);
        File.WriteAllText(path, source.Replace(enter, replacement, StringComparison.Ordinal));

        Assert.Contains(diagnostic, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
        Assert.Contains(diagnostic, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project)).Message);
    }

    [Theory]
    [InlineData(", Track(ctx.Scene.Cinna, CallFromTheDark)", "", "at least two tracks")]
    [InlineData("Track(ctx.Scene.Zidane,", "Track((Zidane)(EventActor)(Cinna)(EventActor)ctx.Scene.Zidane,", "Bind each parallel track to a fixed actor")]
    [InlineData("Track(ctx.Scene.Zidane,", "Track(((Scene)(object)(Cinna)(object)ctx.Scene).Zidane,", "Bind each parallel track to a fixed actor")]
    [InlineData("await Together(", "Location.SceneStep = 7;\n        await Together(", "A parallel scene method must await Together")]
    [InlineData("messageId: 39);", "messageId: 39);\n        if (Location.SceneStep == 3)\n        {\n            return;\n        }", "has an early return")]
    [InlineData("Location.SceneActorsRemaining--;", "((Location)(object)(Zidane)(object)Location).SceneActorsRemaining--;", "The storage receiver must be a side-effect-free reference")]
    [InlineData("private Location Location => ctx.Location;", "private Location Location { get; } = null!;", "The storage receiver must be a side-effect-free reference")]
    [InlineData("private Location Location => ctx.Location;", "private Location Location { get; } = new();", "The storage receiver must be a side-effect-free reference")]
    public void CargoParallelTrackEditsCannotDiscardRuntimeEffects(string before, string after, string diagnostic)
    {
        string project = ExportIndependentParallelPhase("cargo-adversarial-parallel");
        string path = Path.Combine(project, "Cutscenes", "LightTheCandle.cs");
        string source = File.ReadAllText(path);
        Assert.Contains(before, source);
        File.WriteAllText(path, source.Replace(before, after, StringComparison.Ordinal));

        AssertBothReject(project, diagnostic);
    }

    [Fact]
    public void CargoCancelRestoresControlAndASecondInteractionCanStartTheReveal()
    {
        const int sceneStep = 24;
        const int inputArea = 38;
        const int promptState = 42;
        var location = new byte[80];
        location[sceneStep] = 1;
        location[19] = 0xC0;
        var host = new CandlePromptHost();
        var scheduler = new EventScheduler(host, new byte[4096], location) { ControlledActorId = 17 };
        scheduler.RegisterProject(Assembly.Load(EventProjectCompiler.CompileManaged(Export(Cargo, "cargo-cancel"))));
        scheduler.CreateParticipant(3);
        scheduler.CreateParticipant(17);
        for (int tick = 0; tick < 100 && !host.RestoredControl; tick++)
        {
            location[inputArea] = 1;
            scheduler.Tick();
        }
        Assert.True(host.RestoredControl);
        Assert.Equal(1, host.Offers);
        Assert.Equal(1, location[sceneStep]);
        Assert.Equal(0, location[promptState]);
        Assert.Equal(0xC0, location[19] & 0xC0);

        for (int tick = 0; tick < 100 && location[sceneStep] == 1; tick++)
        {
            location[inputArea] = 1;
            scheduler.Tick();
        }
        Assert.Equal(2, host.Offers);
        Assert.Equal(2, location[sceneStep]);
    }

    private sealed class CandlePromptHost : IEventHost
    {
        public int Offers { get; private set; }
        public bool RestoredControl { get; private set; }

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            if (objectId == 17 && opcode == Opcode.MES && (int)operands[2] == 36)
                Offers++;
            if (objectId == 17 && opcode == Opcode.UCON && Offers == 1)
                RestoredControl = true;
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => opcode switch
        {
            Excode.IsKeyPressed => 1,
            Excode.B_SYSVAR when (SystemVariable)(int)operands[0] == SystemVariable.DialogChoice => Offers == 1 ? 1 : 0,
            _ => 0
        };

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void KitchenDispatchesEachRoleToTheSelectedLiveMoogleAfterRetry(bool editCast)
    {
        int[] instances = [133, 136, 135];
        int[] castInstances = editCast ? [136, 133, 135] : instances;
        string project = CopyWithoutComments(Export(Kitchen, "kitchen-roles"));
        string path = Path.Combine(project, "Cutscenes", "AssignKitchenJobs.cs");
        string source = File.ReadAllText(path);
        Assert.Contains("cast.Resolve(jobs.Fisher), evt.KitchenMoogle.FlyToFishingSpot", source);
        Assert.Contains("evt.KitchenMoogle.TakeFlight, level: 6, instance: new InstanceUid(133)", source);
        Assert.Contains("jobs.PotatoDigger = participant.Local.Byte_3;", source);
        Assert.Contains("set => globals.Byte_292 = value;", source);
        if (editCast)
        {
            const string originalCast = "new InstanceUid(133), new InstanceUid(136), new InstanceUid(135)";
            Assert.Contains(originalCast, source);
            File.WriteAllText(path, source.Replace(originalCast, "new InstanceUid(136), new InstanceUid(133), new InstanceUid(135)", StringComparison.Ordinal));
        }
        EVContainer compiled = EventProjectCompiler.Compile(project);
        if (editCast)
        {
            var baseline = EVContainer.Read(new MemoryStream(ReadFixture(Kitchen)));
            for (int slot = 0; slot < baseline.Objects.Length; slot++)
            {
                for (int index = 0; index < baseline.Objects[slot].Scripts.Length; index++)
                {
                    var original = baseline.Objects[slot].Scripts[index];
                    var edited = compiled.Objects[slot].Scripts[index];
                    Assert.Equal(original.Id, edited.Id);
                    if (slot == 36 && original.Id == 1)
                    {
                        var body = EVInstructionBody.Decode(original.Body);
                        foreach (var request in body.Instructions.Where(instruction => instruction.Opcode == FF8.JSM.Jsm.Opcode.REQSW &&
                            instruction.Operands[1].Expression == null && instruction.Operands[1].Value is 133 or 136 &&
                            instruction.Operands[2].Value is 31 or 32 or 33))
                            request.Operands[1] = new EVOperand(request.Operands[1].Value == 133 ? 136 : 133);
                        Assert.Equal(body.Encode(), edited.Body);
                    }
                    else
                        Assert.Equal(original.Body, edited.Body);
                }
            }
            string reopened = Path.Combine(_directory, "kitchen-role-reopened");
            EventProjectExporter.Export(compiled, Kitchen, reopened, useCutsceneNames: false);
            Assert.Equal(compiled.Serialize(), EventProjectCompiler.Compile(CopyWithoutComments(reopened)).Serialize());
        }
        else
            Assert.Equal(ReadFixture(Kitchen), compiled.Serialize());
        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        for (int fisher = 0; fisher < instances.Length; fisher++)
        {
            for (int vegetableChoice = 0; vegetableChoice < 2; vegetableChoice++)
            {
                int[] remaining = Enumerable.Range(0, instances.Length).Where(choice => choice != fisher).ToArray();
                (int, int)[] assignment = [(31, castInstances[fisher]), (32, castInstances[remaining[vegetableChoice]]), (33, castInstances[remaining[1 - vegetableChoice]])];
                var location = new byte[80];
                location[29] = 3;
                var host = new KitchenHost((fisher + 1) % 3, 1 - vegetableChoice, fisher, vegetableChoice);
                var scheduler = new EventScheduler(host, new byte[4096], location);
                scheduler.RegisterProject(assembly);
                scheduler.CreateParticipant(36);
                foreach (int uid in instances)
                    scheduler.CreateParticipant(9, uid);
                for (int tick = 0; tick < 400 && host.Assignments.Count < 3; tick++)
                {
                    host.Frame = tick;
                    scheduler.Tick();
                }

                Assert.Equal(assignment, host.Assignments.OrderBy(item => item.Tag));
                Assert.Equal(6, host.Acknowledgements.Count);
                Assert.Equal(new[] { instances[fisher], instances[remaining[vegetableChoice]], instances[remaining[1 - vegetableChoice]] }, host.Acknowledgements.Skip(3));
                Assert.NotEqual(host.Acknowledgements[0], host.Acknowledgements[3]);
                Assert.True(host.NextPrompt > host.RetryPrompt);
            }
        }
    }

    [Fact]
    public void KitchenRoleAliasCannotWriteToDifferentGlobalStorage()
    {
        string project = Export(Kitchen, "kitchen-incorrect-alias");
        string path = Path.Combine(project, "Cutscenes", "AssignKitchenJobs.cs");
        string source = File.ReadAllText(path);
        Assert.Contains("set => globals.Byte_292 = value;", source);
        File.WriteAllText(path, source.Replace("set => globals.Byte_292 = value;", "set => globals.Byte_291 = value;", StringComparison.Ordinal));
        AssertBothReject(project, "A role storage alias must read and write the same declared global property");
    }

    [Theory]
    [InlineData(3, 9)]
    [InlineData(0, 23)]
    public void KitchenContinuesWhenRoleHasNoMatchingHandler(byte fisher, int definitionAtFirstUid)
    {
        string project = Export(Kitchen, "kitchen-invalid-choice");
        var host = new KitchenHost(0, 0, 0, 0);
        var location = new byte[80];
        var globals = new byte[4096];
        location[29] = 4;
        globals[292] = fisher;
        globals[291] = 1;
        globals[290] = 2;
        var scheduler = new EventScheduler(host, globals, location);
        scheduler.RegisterProject(Assembly.Load(EventProjectCompiler.CompileManaged(project)));
        scheduler.CreateParticipant(36);
        scheduler.CreateParticipant(definitionAtFirstUid, 133);
        scheduler.CreateParticipant(9, 136);
        scheduler.CreateParticipant(9, 135);
        for (int tick = 0; tick < 400 && host.Assignments.Count < 2; tick++)
        {
            host.Frame = tick;
            scheduler.Tick();
        }
        Assert.Equal(new[] { (32, 136), (33, 135) }, host.Assignments.OrderBy(item => item.Tag));
    }

    [Fact]
    public void FishingTurnsEikoTowardTheMoogleBeforeItsWindowCloses()
    {
        var location = new byte[80];
        location[29] = 10;
        var host = new KitchenHost(0, 0, 0, 0);
        var scheduler = new EventScheduler(host, new byte[4096], location);
        scheduler.RegisterProject(Assembly.Load(EventProjectCompiler.CompileManaged(Export(Kitchen, "fishing"))));
        scheduler.CreateParticipant(7);
        scheduler.CreateParticipant(36);
        for (int tick = 0; tick < 200 && host.NextCatch == -1; tick++)
        {
            host.Frame = tick;
            scheduler.Tick();
            if (host.FishingScroll != -1 && tick >= host.FishingScroll + 15)
                location[33] = 1;
        }

        Assert.True(host.CatchWindow >= 0);
        Assert.True(host.FishingScroll >= host.CatchWindow);
        Assert.InRange(host.FishingTurn, host.FishingScroll + 15, host.CatchWindow + 59);
        Assert.True(host.NextCatch > host.CatchWindow + 60);
    }

    private sealed class KitchenHost(int firstFisher, int firstVegetable, int fisher, int vegetable) : IEventHost
    {
        private int _lastMessage;
        private int _attempt;
        public int Frame { get; set; }
        public int RetryPrompt { get; private set; } = -1;
        public int NextPrompt { get; private set; } = -1;
        public int CatchWindow { get; private set; } = -1;
        public int FishingScroll { get; private set; } = -1;
        public int FishingTurn { get; private set; } = -1;
        public int NextCatch { get; private set; } = -1;
        public List<int> Acknowledgements { get; } = new();
        public List<(int Tag, int Uid)> Assignments { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            if (opcode == Opcode.MES && objectId == 36)
            {
                _lastMessage = (int)operands[2];
                if (_lastMessage == 273 && _attempt == 0)
                    RetryPrompt = Frame;
                if (_lastMessage == 267 && _attempt == 1)
                    NextPrompt = Frame;
            }
            if (opcode == Opcode.MES && (int)operands[2] == 270)
                Acknowledgements.Add(objectId);
            if (opcode == Opcode.MOVE3 && objectId is 133 or 135 or 136)
            {
                int tag = (int)operands[0] switch { -140 => 31, -805 => 32, 0 when (int)operands[1] == -230 => 33, _ => 0 };
                if (tag != 0 && !Assignments.Contains((tag, objectId)))
                    Assignments.Add((tag, objectId));
                return EventCommandFlow.Yield;
            }
            if (opcode == Opcode.MESN && objectId == 7 && (int)operands[2] == 298)
                CatchWindow = Frame;
            if (opcode == Opcode.BGSSCROLL && objectId == 36 && (int)operands[0] == 160 && (int)operands[1] == 182)
                FishingScroll = Frame;
            if (opcode == Opcode.TURNA && objectId == 36 && (int)operands[0] == 7)
                FishingTurn = Frame;
            if (opcode == Opcode.MES && objectId == 7 && (int)operands[2] == 299)
                NextCatch = Frame;
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands)
        {
            if (opcode == Excode.B_OBJSPECA && (int)operands[1] == 5)
                return (int)operands[0];
            if (opcode != Excode.B_SYSVAR || (SystemVariable)(int)operands[0] != SystemVariable.DialogChoice)
                return 0;
            return _lastMessage switch
            {
                268 => _attempt == 0 ? firstFisher : fisher,
                271 => _attempt == 0 ? firstVegetable : vegetable,
                273 => _attempt++ == 0 ? 1 : 0,
                _ => throw new InvalidOperationException($"Unexpected choice in message {_lastMessage}.")
            };
        }

        public bool IsWindowOpen(int objectId, int windowId) => objectId == 7 && windowId == 7 && Frame < CatchWindow + 60;
    }

    private sealed class CargoHost(int message) : IEventHost
    {
        public List<(int Frame, int Actor, Opcode Opcode, string Operands)> Commands { get; } = new();
        public int Frame { get; set; }
        public int Menu { get; private set; } = -1;
        public int MenuReturn { get; private set; } = -1;
        public int Window { get; private set; } = -1;
        public int WindowClosed => Window + 100;
        public int Reply { get; private set; } = -1;

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Commands.Add((Frame, objectId, opcode, string.Join(",", operands.ToArray().Select(operand => (int)operand))));
            if (opcode == Opcode.MENU)
            {
                Assert.Equal(17, objectId);
                if (Menu == -1)
                    Menu = Frame;
                if (Frame < Menu + 12)
                    return EventCommandFlow.Retry;
                MenuReturn = Frame;
            }
            if (opcode == Opcode.MES && (int)operands[0] == 3 && (int)operands[2] == message)
            {
                Assert.Equal(22, objectId);
                Window = Frame;
            }
            if (opcode == Opcode.MES && (int)operands[0] == 2 && (int)operands[2] == 41)
            {
                Assert.Equal(17, objectId);
                Reply = Frame;
            }
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;
        public bool IsWindowOpen(int objectId, int windowId) => objectId == 22 && windowId == 3 && Frame < WindowClosed;
    }

    private sealed class ForestHost(int message, int slowWalk) : IEventHost
    {
        private int _remaining = slowWalk;
        public bool ReachedLine { get; private set; }
        public bool ReachedEncounter { get; private set; }

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            if (opcode == Opcode.MOVE && objectId == 15 && (int)operands[0] == 1734 && _remaining-- > 0)
                return EventCommandFlow.Retry;
            if (opcode == Opcode.MESA && (int)operands[3] == message)
                ReachedLine = true;
            if (opcode == Opcode.ENCOUNT)
            {
                Assert.Equal(303, (int)operands[1]);
                ReachedEncounter = true;
                return EventCommandFlow.EndEvent;
            }
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;
        public bool IsWindowOpen(int objectId, int windowId) => false;
    }

    private sealed class TraceHost(int cards, int choice) : IEventHost
    {
        public List<(int Owner, Opcode Opcode, int[] Arguments)> Commands { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Commands.Add((objectId, opcode, operands.ToArray().Select(value => (int)value).ToArray()));
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => opcode == Excode.B_SYSVAR
            ? (SystemVariable)(int)operands[0] switch
            {
                SystemVariable.IsMovementControlEnabled => 1,
                SystemVariable.TetraMasterCardNumber => cards,
                SystemVariable.DialogChoice => choice,
                _ => 0
            }
            : 0;

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }

    private string Export(string name, string leaf, bool useNames = true, CutsceneCatalog? catalog = null, bool extract = true, bool allowNativePhaseDispatch = false)
    {
        string path = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(EVContainer.Read(new MemoryStream(ReadFixture(name))), name, path, useCutsceneNames: useNames, cutsceneCatalog: catalog,
            extractCutscenes: extract, allowNativePhaseDispatch: allowNativePhaseDispatch || name == Cargo);
        return path;
    }

    private string ExportIndependentParallelPhase(string leaf)
    {
        string project = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(EVContainer.Read(new MemoryStream(CargoWithComputedPollingDelay())), Cargo, project, allowNativePhaseDispatch: true);
        return project;
    }

    private static byte[] CargoWithComputedPollingDelay()
    {
        var cargo = EVContainer.Read(new MemoryStream(ReadFixture(Cargo)));
        var director = cargo.Objects[3].Scripts.Single(script => script.Id == 1);
        var body = EVInstructionBody.Decode(director.Body);
        var poll = body.Instructions.Last(step => step.Opcode == FF8.JSM.Jsm.Opcode.WAIT);
        poll.ArgumentMask = 1;
        poll.Operands = [new EVOperand([EVExpressionToken.Literal(1)])];
        director.Body = body.Encode();
        return cargo.Serialize();
    }

    private static byte[] ReadFixture(string name) => File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));

    private static SortedDictionary<string, byte[]> Snapshot(string directory) =>
        new(Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
            .ToDictionary(path => Path.GetRelativePath(directory, path), File.ReadAllBytes), StringComparer.Ordinal);

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
