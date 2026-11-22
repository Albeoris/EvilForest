using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;
using Opcode = EvilForest.EventSource.Opcode;

namespace EveilForest.CSharp.Tests;

public sealed class SceneDirectorBindingTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-director-binding-{Guid.NewGuid():N}");

    [Fact]
    public void AuthoredDirectorsAllocateFreeSlotsAndRunInCompiledAndSdkAssemblies()
    {
        WriteProject();
        EVContainer compiled = EventProjectCompiler.Compile(_directory);
        Assert.Equal(4, compiled.Objects.Length);
        Assert.Equal(new[] { 2, 1 }, Starts(compiled));
        Assert.Equal(new ushort[] { 0, 1 }, compiled.Objects[1].Scripts.Select(script => script.Id));
        Assert.Equal(new ushort[] { 0, 1 }, compiled.Objects[2].Scripts.Select(script => script.Id));
        Assert.Equal(compiled.Serialize(), EVContainer.Read(new MemoryStream(compiled.Serialize())).Serialize());

        AssertTrace(Assembly.Load(EventProjectCompiler.CompileManaged(_directory)), "4.1.51");
        foreach (string configuration in new[] { "Debug", "Release" })
        {
            SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(_directory), "-c", configuration, "--nologo"]);
            AssertTrace(Assembly.Load(File.ReadAllBytes(Path.Combine(_directory, "bin", configuration, "net8.0", "Event.dll"))), "4.1.51");
        }

        string source = Path.Combine(_directory, "Scene.cs");
        File.WriteAllText(source, File.ReadAllText(source).Replace("AddItem(51, 1)", "AddItem(57, 1)", StringComparison.Ordinal));
        EVInstructionBody changed = EVInstructionBody.Decode(EventProjectCompiler.Compile(_directory).Objects[1].Scripts[1].Body);
        Assert.Contains(changed.Instructions, instruction => instruction.Opcode == Jsm.Opcode.ITEMADD && instruction.Operands[0].Value == 57);
        AssertTrace(Assembly.Load(EventProjectCompiler.CompileManaged(_directory)), "4.1.57");
    }

    [Fact]
    public void AnExplicitLegacyMarkerStillBindsTheSameScene()
    {
        WriteProject();
        byte[] implicitBinding = EventProjectCompiler.Compile(_directory).Serialize();
        string path = Path.Combine(_directory, "Scene.cs");
        File.WriteAllText(path, File.ReadAllText(path).Replace("internal AlphaScene Alpha", "[SceneCutscene] internal AlphaScene Alpha", StringComparison.Ordinal));
        Assert.Equal(implicitBinding, EventProjectCompiler.Compile(_directory).Serialize());
        AssertTrace(Assembly.Load(EventProjectCompiler.CompileManaged(_directory)), "4.1.51");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(_directory), "--nologo"]);
        AssertTrace(Assembly.Load(File.ReadAllBytes(Path.Combine(_directory, "bin", "Debug", "net8.0", "Event.dll"))), "4.1.51");
    }

    [Theory]
    [InlineData("Alpha = new AlphaScene(this);", "Alpha = new AlphaScene(this); Another = new AlphaScene(this);", "internal AlphaScene Alpha { get; }", "internal AlphaScene Alpha { get; } internal AlphaScene Another { get; }")]
    [InlineData("Alpha = new AlphaScene(this);", "", "internal AlphaScene Alpha { get; }", "internal AlphaScene Alpha => new AlphaScene(this);")]
    [InlineData("Alpha = new AlphaScene(this);", "Alpha = CreateAlpha();", "internal AlphaScene Alpha { get; }", "internal AlphaScene Alpha { get; } private AlphaScene CreateAlpha() => new AlphaScene(this);")]
    [InlineData("Alpha = new AlphaScene(this);", "Alpha = new AlphaScene(new Context(false));", "public Context()", "private Context(bool detached) { } public Context()")]
    public void AnImplicitBindingMustDeclareOneCutsceneConstructedWithTheCurrentContext(string constructor, string constructorEdit, string property, string propertyEdit)
    {
        WriteProject();
        string path = Path.Combine(_directory, "Scene.cs");
        File.WriteAllText(path, File.ReadAllText(path).Replace(constructor, constructorEdit, StringComparison.Ordinal).Replace(property, propertyEdit, StringComparison.Ordinal));

        Assert.Contains("unique get-only cutscene property", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(_directory)).Message);
        Assert.Contains("unique get-only cutscene property", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(_directory)).Message);
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(_directory), "--nologo"]);
        var scheduler = new EventScheduler(new RecordingHost(), new byte[2048], new byte[80]);
        var assembly = Assembly.Load(File.ReadAllBytes(Path.Combine(_directory, "bin", "Debug", "net8.0", "Event.dll")));
        Assert.Contains("unique get-only cutscene property", Assert.Throws<InvalidOperationException>(() => scheduler.RegisterProject(assembly)).Message);
    }

    [Fact]
    public void OptionalPinsReserveSlotsBeforeAllocatingAnUnpinnedDirector()
    {
        WriteProject(betaHint: "[OriginalDirectorSlot(1)]");

        EVContainer compiled = EventProjectCompiler.Compile(_directory);
        Assert.Equal(new[] { 1, 2 }, Starts(compiled));
    }

    [Fact]
    public void ADirectorPinCannotReplaceAnExistingSlot()
    {
        WriteProject(betaHint: "[OriginalDirectorSlot(3)]");

        var diagnostic = Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(_directory));
        Assert.Contains("Invalid or duplicate director slot 3", diagnostic.Message);
    }

    [Fact]
    public void RemovingTheLastStartReclaimsDirectorBodiesAndPreservesOtherSlotIds()
    {
        WriteProject(betaHint: "[OriginalDirectorSlot(2)]", laterBodyOrder: 3);
        Assert.Equal(3, EventProjectCompiler.Compile(_directory).Objects[3].BodyOrder);
        string path = Path.Combine(_directory, "Scene.cs");
        string source = File.ReadAllText(path);
        string removed = source.Replace("scripts.StartDirector(ctx.Alpha.Direct);", "", StringComparison.Ordinal)
            .Replace("scripts.StartDirector(ctx.Beta.Direct);", "", StringComparison.Ordinal);
        File.WriteAllText(path, removed);

        EVContainer compiled = EventProjectCompiler.Compile(_directory);
        Assert.Empty(Starts(compiled));
        Assert.All(compiled.Objects[1..3], obj => Assert.Null(obj.BodyOrder));
        Assert.Equal(1, compiled.Objects[3].BodyOrder);
        Assert.Equal(compiled.Serialize(), EVContainer.Read(new MemoryStream(compiled.Serialize())).Serialize());

        File.WriteAllText(path, removed.Replace("await Engine.Wait(1);", "await Engine.Wait(1); Scripts.StartThread(objectSlot: 2, uniqueId: 0);", StringComparison.Ordinal));
        Assert.Contains("NEW references missing object slot 2", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(_directory)).Message);
    }

    [Fact]
    public void ADetachedSceneReceiverIsRejectedByTheCompilerAndTheScheduler()
    {
        WriteProject(betaReceiver: "new BetaScene(ctx)");

        var diagnostic = Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(_directory));
        Assert.Contains("registered cutscene instance", diagnostic.Message);
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(_directory), "--nologo"]);
        var scheduler = new EventScheduler(new RecordingHost(), new byte[2048], new byte[80]);
        scheduler.RegisterProject(Assembly.Load(File.ReadAllBytes(Path.Combine(_directory, "bin", "Debug", "net8.0", "Event.dll"))));
        scheduler.StartScene();
        scheduler.Tick();
        var failure = Assert.Throws<InvalidOperationException>(scheduler.Tick);
        Assert.Contains("registered cutscene instance", failure.Message);
    }

    private static int[] Starts(EVContainer container) =>
        EVInstructionBody.Decode(container.Objects[0].Scripts[0].Body).Instructions
            .Where(instruction => instruction.Opcode == Jsm.Opcode.NEW)
            .Select(instruction => instruction.Operands[0].Value).ToArray();

    private static void AssertTrace(Assembly project, string alphaEnd)
    {
        var host = new RecordingHost();
        var scheduler = new EventScheduler(host, new byte[2048], new byte[80]);
        host.Scheduler = scheduler;
        scheduler.RegisterProject(project);
        scheduler.StartScene();
        for (int frame = 0; frame < 8; frame++)
            scheduler.Tick();
        Assert.Equal(new[] { "1.0.2.0", "1.0.1.0", "3.2.60", "3.1.50", alphaEnd, "5.2.61" }, host.Trace);
    }

    private void WriteProject(string betaHint = "", string betaReceiver = "ctx.Beta", int laterBodyOrder = 1)
    {
        Directory.CreateDirectory(_directory);
        Directory.CreateDirectory(Path.Combine(_directory, "lib"));
        File.Copy(typeof(EventRoutine).Assembly.Location, Path.Combine(_directory, "lib", "EvilForest.EventSource.dll"));
        File.WriteAllText(SourceProjectTest.ProjectFile(_directory), """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
              </PropertyGroup>
              <ItemGroup>
                <Reference Include="EvilForest.EventSource">
                  <HintPath>lib/EvilForest.EventSource.dll</HintPath>
                </Reference>
              </ItemGroup>
            </Project>
            """);
        File.WriteAllText(Path.Combine(_directory, "Scene.cs"), $$"""
            using EvilForest.EventSource;
            [assembly: EventFormat(2)]
            [assembly: EventScene(typeof(DirectorTest.Context))]

            namespace DirectorTest;

            internal sealed class Context : EventSceneContext
            {
                public Context()
                {
                    Alpha = new AlphaScene(this);
                    Beta = new BetaScene(this);
                    Ignored = new AardvarkScene(this);
                }

                internal AlphaScene Alpha { get; }
                internal BetaScene Beta { get; }
                internal AardvarkScene Ignored { get; }

                internal SceneDirector UnusedMethodGroup() => Ignored.Direct;
            }

            [ObjectSlot(0, 0, 0, 0, 0, 0, 0, "", "")]
            internal sealed class MainThread : EventThread
            {
                internal MainThread(Context context) : base(context) { }
                private Context ctx => (Context)Context;

                [Script(0, 0)]
                internal async EventRoutine OnInit()
                {
                    var scripts = ctx.Scripts;
                    await Engine.Wait(1);
                    scripts.StartDirector({{betaReceiver}}.Direct);
                    scripts.StartDirector(ctx.Alpha.Direct);
                    return;
                }
            }

            [ObjectSlot(3, 0, 0, 0, {{laterBodyOrder}}, 0, 0, "", "")]
            internal static class AlreadyAllocated;

            internal sealed class AlphaScene(EventSceneContext context) : EventCutscene
            {
                internal async EventRoutine Direct()
                {
                    Inventory.AddItem(50, 1);
                    await Engine.Wait(1);
                    Inventory.AddItem(51, 1);
                }
            }

            internal sealed class BetaScene(Context context) : EventCutscene
            {
                {{betaHint}}
                internal async EventRoutine Direct()
                {
                    Inventory.AddItem(60, 1);
                    await Engine.Wait(2);
                    Inventory.AddItem(61, 1);
                    return;
                }
            }

            internal sealed class AardvarkScene(Context context) : EventCutscene
            {
                internal async EventRoutine Direct()
                {
                    await Engine.Wait(1);
                    Inventory.AddItem(99, 1);
                }
            }
            """);
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }

    private sealed class RecordingHost : IEventHost
    {
        public EventScheduler Scheduler { get; set; } = null!;
        public List<string> Trace { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            if (opcode == Opcode.NEW)
                Trace.Add($"{Scheduler.Frame}.{objectId}.{(int)operands[0]}.{(int)operands[1]}");
            if (opcode == Opcode.ITEMADD)
                Trace.Add($"{Scheduler.Frame}.{objectId}.{(int)operands[0]}");
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => throw new InvalidOperationException(opcode.ToString());
        public bool IsWindowOpen(int objectId, int windowId) => throw new InvalidOperationException(windowId.ToString());
    }
}
