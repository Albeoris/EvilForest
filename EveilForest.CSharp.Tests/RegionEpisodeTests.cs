using System.Buffers.Binary;
using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class RegionEpisodeTests : IDisposable
{
    private const string Ice = "EVT_ICE_IC_MEN_0";
    private const string Wall = "EVT_SARI2_MS_EIK_1";
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-region-episode-").FullName;

    [Theory]
    [InlineData(Ice, 5, 2, 6, 11, 204, 205, 2)]
    [InlineData(Wall, 4, 3, 10, 13, 85, 86, 6)]
    public void TheCallbackKeepsAdmissionAndTheSceneOwnsBothDialogueAndStaging(string field, int region, int callback, int actor, int tag, int first, int second, int level)
    {
        EVContainer original = Read(field);
        string sceneName = $"Scene_{region:D2}_{callback:D2}";
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "extracted"));
        string scenePath = Path.Combine(project, "Cutscenes", sceneName + ".cs");
        string scene = File.ReadAllText(scenePath);
        string handler = File.ReadAllText(Participant(project, region));
        string action = File.ReadAllText(Participant(project, actor));

        Assert.Contains("Engine.ReadSystem(SystemVariable.IsMovementControlEnabled)", handler);
        Assert.Contains($"await ctx.{sceneName}.Play(this);", handler);
        Assert.DoesNotContain("Engine.DisableMove()", handler);
        Assert.DoesNotContain("RequestAndWait", handler);
        if (field == Wall)
        {
            Assert.Contains("Globals.ScenarioCounter != 6640", handler);
            Assert.DoesNotContain("Globals.ScenarioCounter", scene);
            Assert.Contains("messageId: 84)", File.ReadAllText(Participant(project, 1)));
        }
        Assert.Contains("Engine.DisableMove()", scene);
        Assert.Contains("Engine.EnableMove()", scene);
        Assert.Contains($"Action_{actor:D2}_{tag:D2}), level: {level})", scene);
        Assert.Contains($"messageId: {first})", scene);
        Assert.Contains($"messageId: {second})", scene);
        Assert.Contains("participant.Animation.WaitForCompletion()", scene);
        Assert.DoesNotContain($"[Script({tag},", action);
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        Assembly baseline = Assembly.Load(EventProjectCompiler.CompileManaged(Export(original, field, "reference", extract: false)));
        Assembly extracted = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Command[] expected = Trace(baseline, region, callback, actor, movement: true);
        Assert.Equal(new[] { first, second }, expected.Where(command => command.Opcode is Opcode.MES or Opcode.MESN).Select(command => command.Value));
        Assert.Equal(new[] { Opcode.UCOFF, Opcode.MENUOFF, Opcode.BGIMASK, Opcode.UCON, Opcode.BGIMASK, Opcode.MENUON },
            expected.Where(command => command.Owner == region).Select(command => command.Opcode));
        Assert.Equal(expected, Trace(extracted, region, callback, actor, movement: true));
        Assert.Empty(Trace(extracted, region, callback, actor, movement: false));
        if (field == Wall)
            Assert.Empty(Trace(extracted, region, callback, actor, movement: true, scenario: 6640));

        const int changed = 711;
        File.WriteAllText(scenePath, scene.Replace($"messageId: {second})", $"messageId: {changed})", StringComparison.Ordinal));
        EVContainer edited = EventProjectCompiler.Compile(project);
        EVInstructionBody emittedAction = EVInstructionBody.Decode(Script(edited, actor, tag).Body);
        Assert.Equal(new[] { first, changed }, emittedAction.Instructions.Where(step => step.Opcode is Jsm.Opcode.MES or Jsm.Opcode.MESN).Select(step => step.Operands[2].Value));
        Assert.Equal(Script(original, region, callback).Body, Script(edited, region, callback).Body);
        Assert.Equal(expected.Select(command => command.Opcode is Opcode.MES or Opcode.MESN && command.Value == second ? command with { Value = changed } : command),
            Trace(Assembly.Load(EventProjectCompiler.CompileManaged(project)), region, callback, actor, movement: true));

        string reexport = Export(edited, field, "reexport");
        Assert.Contains($"messageId: {changed})", File.ReadAllText(Path.Combine(reexport, "Cutscenes", sceneName + ".cs")));
    }

    [Fact]
    public void ARegionInspectionWithoutAnAwaitedPoseRestorationStaysOnTheActor()
    {
        EVContainer field = Read(Wall);
        EVContainerScript script = Script(field, 10, 13);
        EVInstructionBody body = EVInstructionBody.Decode(script.Body);
        body.Instructions.Last(step => step.Opcode == Jsm.Opcode.WAITANIM).Opcode = Jsm.Opcode.WAITTURN;
        script.Body = body.Encode();

        string project = Export(field, Wall, "near-miss");
        Assert.False(File.Exists(Path.Combine(project, "Cutscenes", "Scene_04_03.cs")));
        Assert.Contains("messageId: 85)", File.ReadAllText(Participant(project, 10)));
        Assert.Contains("RequestAndWait", File.ReadAllText(Participant(project, 4)));
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Fact]
    public void ASecondCallerKeepsTheActorChoreographyShared()
    {
        EVContainer field = Read(Wall);
        EVContainerObject area = field.Objects[4];
        EVContainerScript source = Script(field, 4, 3);
        EVContainerScript inRange = Script(field, 4, 2);
        area.Scripts[Array.IndexOf(area.Scripts, inRange)] = new EVContainerScript(inRange.Id, source.Body, inRange.OriginalOffset);

        string project = Export(field, Wall, "shared");
        Assert.False(File.Exists(Path.Combine(project, "Cutscenes", "Scene_04_03.cs")));
        Assert.False(File.Exists(Path.Combine(project, "Cutscenes", "Scene_04_02.cs")));
        Assert.Contains("messageId: 85)", File.ReadAllText(Participant(project, 10)));
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Fact]
    public void RelocatedAvailabilityTagAndDialogueStillIdentifyTheScene()
    {
        EVContainer field = Read(Wall);
        EVContainerScript callback = Script(field, 4, 3);
        EVInstructionBody callbackBody = EVInstructionBody.Decode(callback.Body);
        EVExpressionToken gate = new EVVariable(EVStorageScope.Map, EVStorageType.Bit, 158).Encode();
        EVExpressionToken relocatedGate = new EVVariable(EVStorageScope.Map, EVStorageType.Bit, 307).Encode();
        foreach (EVInstruction step in callbackBody.Instructions.Where(step => step.Expression?.Contains(gate) == true))
            step.Expression = step.Expression!.Select(token => token.Equals(gate) ? relocatedGate : token).ToArray();
        const ushort relocatedTag = 98;
        callbackBody.Instructions.Single(step => step.Opcode == Jsm.Opcode.REQEW).Operands[2] = new EVOperand(relocatedTag);
        callback.Body = callbackBody.Encode();

        EVContainerScript action = Script(field, 10, 13);
        EVInstructionBody actionBody = EVInstructionBody.Decode(action.Body);
        foreach (EVInstruction step in actionBody.Instructions.Where(step => step.Opcode == Jsm.Opcode.MES))
            step.Operands[2] = new EVOperand(step.Operands[2].Value + 600);
        EVContainerScript[] actor = field.Objects[10].Scripts;
        actor[Array.IndexOf(actor, action)] = new EVContainerScript(relocatedTag, actionBody.Encode(), action.OriginalOffset);

        string project = Export(field, "EVT_RELOCATED_REGION", "relocated");
        string scene = File.ReadAllText(Path.Combine(project, "Cutscenes", "Scene_04_03.cs"));
        Assert.Contains("Location.Bit_307 = false;", scene);
        Assert.Contains("Location.Bit_307 = true;", scene);
        Assert.Contains("Action_10_98), level: 6)", scene);
        Assert.Contains("messageId: 685)", scene);
        Assert.Contains("messageId: 686)", scene);
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void DelaysWithinPlaybackSetupAndRestorationSurviveReexport()
    {
        const string sceneName = "Scene_05_02.cs";
        string project = Export(Read(Ice), Ice, "with-delays");
        string scenePath = Path.Combine(project, "Cutscenes", sceneName);
        string source = File.ReadAllText(scenePath);
        source = InsertBefore(source, "Location.Bit_158 = false;", "await Engine.Wait(frames: 2);");
        source = InsertBefore(source, "Location.Bit_158 = true;", "await Engine.Wait(frames: 3);");
        source = InsertBefore(source, "await participant.Animation.WaitForCompletion();", "await Engine.Wait(frames: 1);");
        source = InsertBefore(source, "await participant.Animation.WaitForCompletion();", "await Engine.YieldFrame();");
        File.WriteAllText(scenePath, source);

        EVContainer edited = EventProjectCompiler.Compile(project);
        string reexport = SourceProjectTest.CopyWithoutComments(Export(edited, Ice, "with-delays-reexport"));
        string commands = File.ReadAllText(Path.Combine(reexport, "Cutscenes", sceneName));
        Assert.Contains("await Engine.Wait(frames: 2);", commands);
        Assert.Contains("await Engine.Wait(frames: 3);", commands);
        Assert.Contains("await Engine.YieldFrame();", commands);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reexport).Serialize());
        Assert.Equal(Trace(Assembly.Load(EventProjectCompiler.CompileManaged(project)), 5, 2, 6, movement: true),
            Trace(Assembly.Load(EventProjectCompiler.CompileManaged(reexport)), 5, 2, 6, movement: true));

        static string InsertBefore(string source, string statement, string inserted)
        {
            int position = source.IndexOf(statement, StringComparison.Ordinal);
            Assert.True(position >= 0);
            int start = source.LastIndexOf('\n', position) + 1;
            return source.Insert(start, source[start..position] + inserted + Environment.NewLine);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AStoppedAnimationOrAWaitOnAnotherPathCannotProveRestoration(bool stopAnimation)
    {
        const string sceneName = "Scene_04_03.cs";
        string project = Export(Read(Wall), Wall, "incomplete-animation");
        string scenePath = Path.Combine(project, "Cutscenes", sceneName);
        string[] lines = File.ReadAllLines(scenePath);
        int play = Array.FindLastIndex(lines, line => line.Contains("participant.Animation.Play(", StringComparison.Ordinal));
        Assert.True(play >= 0);
        string wait = lines[play + 1];
        Assert.Equal("await participant.Animation.WaitForCompletion();", wait.Trim());
        string[] replacement = stopAnimation
            ? [lines[play], "        participant.Animation.Stop();", wait]
            : ["        if (Location.Bit_144)", "        {", "    " + lines[play], "        }", "        else", "        {", "    " + wait, "        }"];
        File.WriteAllLines(scenePath, lines.Take(play).Concat(replacement).Concat(lines.Skip(play + 2)));

        EVContainer edited = EventProjectCompiler.Compile(project);
        string reexport = Export(edited, Wall, "incomplete-animation-reexport");
        Assert.False(File.Exists(Path.Combine(reexport, "Cutscenes", sceneName)));
        Assert.Contains("messageId: 86)", File.ReadAllText(Participant(reexport, 10)));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reexport).Serialize());
    }

    [Fact]
    public void AChangeToOtherActorsWalkmeshDoesNotProveAPlayerControlHandoff()
    {
        const string sceneName = "Scene_04_03.cs";
        string project = Export(Read(Wall), Wall, "other-actors-mask");
        string scenePath = Path.Combine(project, "Cutscenes", sceneName);
        string source = File.ReadAllText(scenePath);
        Assert.Contains("TriangleAttributeMask.All & ~TriangleAttributeMask.RestrictPlayer", source);
        File.WriteAllText(scenePath, source.Replace("TriangleAttributeMask.RestrictPlayer", "TriangleAttributeMask.RestrictOtherActors", StringComparison.Ordinal));

        EVContainer edited = EventProjectCompiler.Compile(project);
        string reexport = Export(edited, Wall, "other-actors-mask-reexport");
        Assert.False(File.Exists(Path.Combine(reexport, "Cutscenes", sceneName)));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reexport).Serialize());
    }

    private static Command[] Trace(Assembly assembly, int region, int callback, int actor, bool movement, int scenario = 0)
    {
        byte[] globals = new byte[4096];
        BinaryPrimitives.WriteInt16LittleEndian(globals, (short)scenario);
        byte[] location = new byte[128];
        location[19] = 0x80;
        var host = new RegionHost(region, actor, movement);
        var scheduler = new EventScheduler(host, globals, location);
        host.Frame = () => scheduler.Frame;
        scheduler.RegisterProject(assembly);
        scheduler.CreateParticipant(region);
        scheduler.CreateParticipant(actor);
        for (int frame = 0; frame < 15; frame++)
            scheduler.Tick();
        host.Commands.Clear();

        Assert.True(scheduler.TryTrigger(region, callback, level: 1));
        for (int frame = 0; frame < 40; frame++)
            scheduler.Tick();
        return host.Commands.ToArray();
    }

    private sealed record Command(int Frame, int Owner, Opcode Opcode, int Value);

    private sealed class RegionHost(int region, int actor, bool movement) : IEventHost
    {
        public List<Command> Commands { get; } = new();
        public Func<int> Frame { get; set; } = null!;

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            if (objectId == region && opcode is Opcode.UCOFF or Opcode.UCON or Opcode.MENUOFF or Opcode.MENUON or Opcode.BGIMASK ||
                objectId == actor && opcode is Opcode.MES or Opcode.MESN or Opcode.ANIM or Opcode.WAITANIM or Opcode.MOVE or Opcode.CLOSE)
                Commands.Add(new(Frame(), objectId, opcode, opcode is Opcode.MES or Opcode.MESN ? (int)operands[2] : operands.IsEmpty ? 0 : (int)operands[0]));
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) =>
            movement && opcode == Excode.B_SYSVAR && (int)operands[0] == (int)SystemVariable.IsMovementControlEnabled ? 1 : 0;

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }

    private static EVContainer Read(string name)
    {
        using FileStream source = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
        return EVContainer.Read(source);
    }

    private static EVContainerScript Script(EVContainer field, int slot, int tag) => field.Objects[slot].Scripts.Single(script => script.Id == tag);
    private static string Participant(string project, int slot) => Assert.Single(Directory.GetFiles(project, $"{slot:D2}_*.cs"));

    private string Export(EVContainer field, string name, string leaf, bool extract = true)
    {
        string path = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(field, name, path, useCutsceneNames: false, extractCutscenes: extract);
        return path;
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
