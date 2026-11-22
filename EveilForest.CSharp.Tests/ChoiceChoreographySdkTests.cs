using System.Reflection;
using EvilForest.EventSource;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class ChoiceChoreographySdkTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-choice-sdk-{Guid.NewGuid():N}");

    [Theory]
    [InlineData("Debug")]
    [InlineData("Release")]
    public void AChoiceConsumesTheCueAndCanReplayThroughASharedRoute(string configuration)
    {
        Assembly assembly = Build(configuration, """
            await play.Choose(() => Location.Choice, play.Case(-1, Left), play.Case(0, Left), play.Case(1, Right));
            """);
        byte[] location = new byte[80];
        var scheduler = new EventScheduler(new Host(), new byte[4096], location);
        scheduler.RegisterProject(assembly);
        scheduler.StartScene();
        scheduler.Tick();
        scheduler.Tick();

        Tick(12, true, 0);
        Tick(-1, false, 0);
        Tick(-1, true, 1);
        Tick(0, true, 3);
        Tick(5, true, 0);
        Tick(1, true, 2);
        Tick(0, true, 3);
        Tick(5, true, 0);
        Tick(0, true, 1);

        void Tick(int choice, bool cue, byte expectedPhase)
        {
            location[2] = unchecked((byte)choice);
            location[1] = (byte)(cue ? 3 : 2);
            scheduler.Tick();
            Assert.Equal(expectedPhase, location[0]);
            Assert.Equal(0, location[1]);
        }
    }

    [Theory]
    [InlineData("ScriptMemory.ReadView(typeof(Location))", "ScriptMemory.WriteView(typeof(Location), value)")]
    [InlineData("ScriptMemory.Read(StorageScope.Map, StorageType.Byte, 1)", "ScriptMemory.Write(StorageScope.Map, StorageType.Byte, 1, value)")]
    public void AChoiceReadsTheDeclaredStorageBeforeClearingAnOverlappingCue(string getter, string setter)
    {
        Assembly assembly = Build("Release", "await play.Choose(() => Location.Choice, play.Case(1, Right));", $$"""
            [StorageView(StorageScope.Map, StorageType.Byte, 1)]
            public int Choice { get => {{getter}}; set => {{setter}}; }
            """);
        byte[] location = new byte[80];
        var scheduler = new EventScheduler(new Host(), new byte[4096], location);
        scheduler.RegisterProject(assembly);
        scheduler.StartScene();
        scheduler.Tick();
        scheduler.Tick();

        location[1] = 3;
        scheduler.Tick();

        Assert.Equal(2, location[0]);
        Assert.Equal(0, location[1]);
    }

    [Fact]
    public void AChoiceRejectsASideEffectingStorageGetterWithoutRunningIt()
    {
        Assembly assembly = Build("Release", "await play.Choose(() => Location.Choice, play.Case(0, Left));", """
            internal static int Reads;
            [StorageView(StorageScope.Map, StorageType.SByte, 2)]
            public int Choice
            {
                get
                {
                    Reads++;
                    return ScriptMemory.ReadView(typeof(Location));
                }
                set => ScriptMemory.WriteView(typeof(Location), value);
            }
            """);
        var scheduler = new EventScheduler(new Host(), new byte[4096], new byte[80]);
        Assert.Contains("writable integer map storage property", Assert.Throws<InvalidOperationException>(() => scheduler.RegisterProject(assembly)).Message);
        FieldInfo reads = assembly.GetType("ChoiceSdk.Location")!.GetField("Reads", BindingFlags.Static | BindingFlags.NonPublic)!;
        Assert.Equal(0, reads.GetValue(null));
    }

    [Fact]
    public void AContinuationCycleCannotRegisterWithoutAChoiceEdge()
    {
        Assembly assembly = Build("Release", "await play.ContinueWith(Left);");
        var scheduler = new EventScheduler(new Host(), new byte[4096], new byte[80]);
        Assert.Contains("forms a cycle", Assert.Throws<InvalidOperationException>(() => scheduler.RegisterProject(assembly)).Message);
    }

    [Fact]
    public void ASceneCallbackRepeatsAndRunsItsWorkBeforeTheCueAndPulseAreProcessed()
    {
        Assembly assembly = Build("Release", """
            await play.RunUntilCue(1);
            await play.RepeatFrom(Entry);
            """, attached: true);
        byte[] location = new byte[80];
        var scheduler = new EventScheduler(new Host(), new byte[4096], location);
        scheduler.RegisterProject(assembly);
        Assert.Equal(0, location[2]);
        scheduler.StartScene();
        scheduler.Tick();
        scheduler.Tick();
        int previousPolls = location[2];
        location[0] = 0;

        for (int tick = 0; tick < 3; tick++)
        {
            location[1] = 0;
            scheduler.Tick();
            Assert.Equal((tick + 1) % 2, location[0]);
            Assert.Equal(0, location[1]);
            Assert.Equal(previousPolls + tick + 1, location[2]);
        }
    }

    [Fact]
    public void FallThroughRunsAnotherPhaseInTheSameTickWithoutEnteringIt()
    {
        Assembly assembly = Build("Release", "await play.FallThrough(5, UpdateCue, 0);");
        byte[] location = new byte[80];
        var scheduler = new EventScheduler(new Host(), new byte[4096], location);
        scheduler.RegisterProject(assembly);
        scheduler.StartScene();
        scheduler.Tick();
        scheduler.Tick();

        location[0] = 5;
        scheduler.Tick();

        Assert.Equal(5, location[0]);
        Assert.Equal(2, location[1]);
        Assert.Equal(1, location[2]);
    }

    [Fact]
    public void DuplicateChoiceKeysAreRejectedBeforeScheduling()
    {
        Assembly assembly = Build("Release", "await play.Choose(() => Location.Choice, play.Case(0, Left), play.Case(0, Right));");
        var scheduler = new EventScheduler(new Host(), new byte[4096], new byte[80]);
        Assert.Contains("a unique value for each case", Assert.Throws<InvalidOperationException>(() => scheduler.RegisterProject(assembly)).Message);
    }

    [Fact]
    public void AChoiceKeyMustFitTheSelectedCell()
    {
        Assembly assembly = Build("Release", "await play.Choose(() => Location.Choice, play.Case(128, Left));");
        var scheduler = new EventScheduler(new Host(), new byte[4096], new byte[80]);
        Assert.Contains("Choice keys must fit the selected map cell", Assert.Throws<InvalidOperationException>(() => scheduler.RegisterProject(assembly)).Message);
    }

    private Assembly Build(string configuration, string selection, string choiceStorage = "public int Choice { get => Memory.Choice.Value; set => Memory.Choice.Value = value; }", bool attached = false)
    {
        Directory.CreateDirectory(_directory);
        File.Copy(typeof(EventRoutine).Assembly.Location, Path.Combine(_directory, "EvilForest.EventSource.dll"));
        File.WriteAllText(SourceProjectTest.ProjectFile(_directory), """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
              </PropertyGroup>
              <ItemGroup>
                <Reference Include="EvilForest.EventSource">
                  <HintPath>EvilForest.EventSource.dll</HintPath>
                </Reference>
              </ItemGroup>
            </Project>
            """);
        string initialize = attached ? "internal void OnInit() { }" : "internal void OnInit() => context.Scripts.StartDirector(context.Sequence.Direct);";
        string direct = attached ? "[Script(1, 1)] internal async EventRoutine Direct(MainThread participant)" : "internal async EventRoutine Direct()";
        string pollWork = attached ? "play.BeforePoll(UpdateCue);" : "";
        File.WriteAllText(Path.Combine(_directory, "Scene.cs"), $$"""
            using System.Runtime.InteropServices;
            using EvilForest.EventSource;

            [assembly: EventFormat(2)]
            [assembly: EventScene(typeof(ChoiceSdk.Context))]

            namespace ChoiceSdk;

            internal sealed class Context : EventSceneContext
            {
                public Context() => Sequence = new Sequence(this);

                internal Location Location { get; } = new();
                internal Sequence Sequence { get; }
            }

            internal sealed class Location
            {
                [StructLayout(LayoutKind.Explicit, Pack = 1)]
                private struct MemoryLayout
                {
                    [FieldOffset(0)]
                    internal ByteCell Phase;
                    [FieldOffset(1)]
                    internal BitCell<Bit0> Cue;
                    [FieldOffset(1)]
                    internal BitCell<Bit1> Pulse;
                    [FieldOffset(2)]
                    internal SByteCell Choice;
                }

                private ref MemoryLayout Memory => ref ScriptMemory.View<MemoryLayout>(StorageScope.Map);
                public int Phase { get => Memory.Phase.Value; set => Memory.Phase.Value = value; }
                public bool Cue { get => Memory.Cue.Value; set => Memory.Cue.Value = value; }
                public bool Pulse { get => Memory.Pulse.Value; set => Memory.Pulse.Value = value; }
                {{choiceStorage}}
            }

            [ObjectSlot(0, 0, 0, 0, 0, 0, 0, "", "")]
            internal sealed class MainThread(Context context) : EventThread(context)
            {
                [Script(0, 0)]
                {{initialize}}
            }

            internal sealed class Sequence(Context context) : EventCutscene
            {
                private Location Location => context.Location;

                {{direct}}
                {
                    var play = Choreography(() => Location.Phase, () => Location.Cue, () => Location.Pulse);
                    {{pollWork}}
                    await play.Routes(Entry);
                }

                private void UpdateCue()
                {
                    Location.Pulse = true;
                    Location.Cue = true;
                    Location.Choice++;
                }

                private async EventRoutine Entry(SceneChoreography play)
                {
                    await play.RunUntilCue(0);
                    {{selection}}
                }

                private async EventRoutine Left(SceneChoreography play)
                {
                    await play.RunUntilCue(1);
                    await play.ContinueWith(Shared);
                }

                private async EventRoutine Right(SceneChoreography play)
                {
                    await play.RunUntilCue(2);
                    await play.ContinueWith(Shared);
                }

                private async EventRoutine Shared(SceneChoreography play)
                {
                    await play.RunUntilCue(3);
                    await play.ContinueWith(Entry);
                }
            }
            """);
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(_directory), "-c", configuration, "--nologo"]);
        return Assembly.Load(File.ReadAllBytes(Path.Combine(_directory, "bin", configuration, "net8.0", "Event.dll")));
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }

    private sealed class Host : IEventHost
    {
        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands) => EventCommandFlow.Continue;
        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => throw new InvalidOperationException(opcode.ToString());
        public bool IsWindowOpen(int objectId, int windowId) => throw new InvalidOperationException(windowId.ToString());
    }
}
