using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class ParticipantPollingWorkTests : IDisposable
{
    private const string Meeting = "EVT_ALEX1_TS_MEET_0";
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-poll-work-{Guid.NewGuid():N}");

    [Theory]
    [InlineData("EVT_ALEX1_AT_STREET_B")]
    [InlineData("EVT_ALEX1_TS_ENGIN")]
    [InlineData(Meeting)]
    [InlineData("EVT_ALEX1_TS_STAGE_1B")]
    [InlineData("EVT_ALEX3_AC_ENT_2F")]
    [InlineData("EVT_ALEX3_AC_OHAKA")]
    [InlineData("EVT_CLEYRA1_DUN_07")]
    [InlineData("EVT_CLEYRA3_CATHE_0")]
    [InlineData("EVT_ENDING_TH_0")]
    [InlineData("EVT_ENDING_TH_2")]
    [InlineData("EVT_GIZ_CAVERN_0")]
    [InlineData("EVT_GIZ_CAVERN_1")]
    public void StrictProjectsPreserveParticipantPollingWorkWithoutComments(string field)
    {
        var (original, project) = Export(field);
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void EditingUnmatchedPhaseWorkChangesOnlyTheFallbackAndCanSuspend()
    {
        var (original, project) = Export(Meeting);
        string actor = Assert.Single(Directory.GetFiles(project, "06_*.cs"));
        string source = File.ReadAllText(actor);
        Assert.Contains("PerformPhase(this, () =>", source);
        Assert.Contains("Actors.SetObjectFlags(flags: 7);", source);
        File.WriteAllText(actor, source.Replace("PerformPhase(this, () =>", "PerformPhase(this, async () =>", StringComparison.Ordinal)
            .Replace("Actors.SetObjectFlags(flags: 7);", "await Engine.Wait(frames: 2);\n                    Actors.SetObjectFlags(flags: 9);", StringComparison.Ordinal));

        var originalLoop = ReadLoop(original);
        EVContainer edited = EventProjectCompiler.Compile(project);
        var editedLoop = ReadLoop(edited);
        EVInstruction originalSwitch = originalLoop.Instructions[1];
        EVInstruction editedSwitch = editedLoop.Instructions[1];
        Assert.Equal(Jsm.Opcode.JMP_SWITCHEX, editedSwitch.Opcode);
        Assert.Equal(originalSwitch.CaseValues, editedSwitch.CaseValues);
        Assert.Equal(editedSwitch.Targets[0], editedSwitch.Targets[2]);
        Assert.Equal(originalLoop.Instructions.Where(step => step.Label < originalSwitch.Targets[0]).Select(step => step.Opcode),
            editedLoop.Instructions.Where(step => step.Label < editedSwitch.Targets[0]).Select(step => step.Opcode));
        Assert.Contains(editedLoop.Instructions, step => step.Label >= editedSwitch.Targets[0] && step.Opcode == Jsm.Opcode.WAIT && step.Operands[0].Value == 2);
        Assert.Contains(editedLoop.Instructions, step => step.Label >= editedSwitch.Targets[0] && step.Opcode == Jsm.Opcode.CFLAG && step.Operands[0].Value == 9);

        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Assert.Equal(new[] { (1, 14), (4, 9) }, Replay(assembly, 2));
        Assert.Equal(new[] { (1, 14), (2, 14), (3, 14), (4, 14), (5, 14), (6, 14) }, Replay(assembly, 6));
        string sdkOutput = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", sdkOutput, "--nologo"]);
        string sdkDll = Assert.Single(Directory.GetFiles(Path.Combine(sdkOutput, "bin"), "Event.dll", SearchOption.AllDirectories));
        Assembly sdkAssembly = Assembly.Load(File.ReadAllBytes(sdkDll));
        Assert.Equal(Replay(assembly, 2), Replay(sdkAssembly, 2));
        Assert.Equal(Replay(assembly, 6), Replay(sdkAssembly, 6));

        string reexport = Path.Combine(_directory, "reexport");
        EventProjectExporter.Export(edited, Meeting, reexport, useCutsceneNames: false);
        Assert.Contains("PerformPhase(this, async () =>", File.ReadAllText(Assert.Single(Directory.GetFiles(reexport, "06_*.cs"))));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reexport).Serialize());

        string scene = Path.Combine(project, "Cutscenes", "Scene_02_01.cs");
        string sceneSource = File.ReadAllText(scene);
        const string binding = "Role(evt.GEO_ACC_F0_TSM_06, Phase6_GEO_ACC_F0_TSM_06),";
        Assert.Contains(binding, sceneSource);
        File.WriteAllText(scene, sceneSource.Replace(binding, "", StringComparison.Ordinal));
        EVInstructionBody fallbackOnly = ReadLoop(EventProjectCompiler.Compile(project));
        Assert.DoesNotContain(fallbackOnly.Instructions, step => step.Opcode is Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX);
        Assembly fallbackAssembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Assert.Equal(Replay(assembly, 2), Replay(fallbackAssembly, 6));

        static EVInstructionBody ReadLoop(EVContainer container) => EVInstructionBody.Decode(container.Objects[6].Scripts.Single(script => script.Id == 1).Body);
    }

    [Fact]
    public void EditingOneBindingCanSplitTwoPhasesThatOriginallySharedAnArm()
    {
        var (original, project) = Export("EVT_ALEX1_AT_STREET_B");
        string scene = Path.Combine(project, "Cutscenes", "Scene_02_01.cs");
        string source = File.ReadAllText(scene);
        const string before = "play.Idle(0, Role(evt.GEO_NPC_F0_TBY_07, Phase3_GEO_NPC_F0_TBY_07)";
        const string after = "play.Idle(0, Role(evt.GEO_NPC_F0_TBY_07, OnlyAtZero)";
        Assert.Contains(before, source);
        const string newRole = """

            internal void OnlyAtZero(GEO_NPC_F0_TBY_07 participant)
            {
                Actors.SetObjectFlags(flags: 42);
            }

            """;
        string updated = source.Replace(before, after, StringComparison.Ordinal);
        File.WriteAllText(scene, updated.Insert(updated.LastIndexOf('}'), newRole));

        var originalLoop = ReadLoop(original);
        var editedLoop = ReadLoop(EventProjectCompiler.Compile(project));
        Assert.Equal(Target(originalLoop, 0), Target(originalLoop, 3));
        Assert.NotEqual(Target(editedLoop, 0), Target(editedLoop, 3));
        Assert.Contains(editedLoop.Instructions, step => step.Label == Target(editedLoop, 0) && step.Opcode == Jsm.Opcode.CFLAG && step.Operands[0].Value == 42);
        Assert.Contains(editedLoop.Instructions, step => step.Label == Target(editedLoop, 3) && step.Opcode != Jsm.Opcode.CFLAG);

        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Assert.Contains(Replay(assembly, 0, slot: 7), item => item.Flags == 42);
        Assert.DoesNotContain(Replay(assembly, 3, slot: 7), item => item.Flags == 42);

        static EVInstructionBody ReadLoop(EVContainer container) => EVInstructionBody.Decode(container.Objects[7].Scripts.Single(script => script.Id == 1).Body);

        static int Target(EVInstructionBody loop, int phase)
        {
            EVInstruction table = loop.Instructions[1];
            return table.Opcode == Jsm.Opcode.JMP_SWITCHEX
                ? table.Targets[Array.IndexOf(table.CaseValues, (ushort)phase) + 1]
                : table.Targets[phase - unchecked((short)table.CaseValues[0]) + 1];
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(256)]
    public void ALinkedRoleCannotReachAPhaseOutsideTheSelectorRange(int phase)
    {
        var (_, project) = Export("EVT_CLEYRA2_TOWN");
        string scene = Path.Combine(project, "Cutscenes", "Scene_04_01.cs");
        string source = File.ReadAllText(scene);
        Assert.Contains("thenPhase: 6", source);
        Assert.Contains("play.Together(6,", source);
        File.WriteAllText(scene, source.Replace("thenPhase: 6", $"thenPhase: {phase}", StringComparison.Ordinal)
            .Replace("play.Together(6,", $"play.Together({phase},", StringComparison.Ordinal));
        Assert.Contains("phase value that fits the map selector", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);

        string sdkOutput = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", sdkOutput, "--nologo"]);
        string sdkDll = Assert.Single(Directory.GetFiles(Path.Combine(sdkOutput, "bin"), "Event.dll", SearchOption.AllDirectories));
        Assembly sdkAssembly = Assembly.Load(File.ReadAllBytes(sdkDll));
        var scheduler = new EventScheduler(new Host(6), new byte[4096], new byte[80]);
        Assert.Contains("phase value that fits the map selector", Assert.Throws<InvalidOperationException>(() => scheduler.RegisterProject(sdkAssembly)).Message);
    }

    private static IReadOnlyList<(int Frame, int Flags)> Replay(Assembly assembly, byte phase, int slot = 6)
    {
        var map = new byte[80];
        var host = new Host(slot);
        var scheduler = new EventScheduler(host, new byte[4096], map);
        host.Frame = () => scheduler.Frame;
        scheduler.RegisterProject(assembly);
        scheduler.CreateParticipant(slot);
        scheduler.Tick();
        host.Flags.Clear();
        map[24] = phase;
        map[32] = 1;
        for (int tick = 0; tick < 6; tick++)
        {
            scheduler.Tick();
        }
        return host.Flags;
    }

    private (EVContainer Original, string Project) Export(string field)
    {
        using Stream input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", field + ".eb.bytes"));
        EVContainer original = EVContainer.Read(input);
        string project = Path.Combine(_directory, field);
        EventProjectExporter.Export(original, field, project, useCutsceneNames: false);
        return (original, SourceProjectTest.CopyWithoutComments(project));
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }

    private sealed class Host(int slot) : IEventHost
    {
        public Func<int> Frame { get; set; } = null!;
        public List<(int Frame, int Flags)> Flags { get; } = [];

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            if (objectId == slot && opcode == Opcode.CFLAG)
            {
                Flags.Add((Frame(), (int)operands[0]));
            }
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 1;

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }
}
