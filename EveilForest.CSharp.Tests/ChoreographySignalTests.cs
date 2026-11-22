using System.Reflection;
using EvilForest.EventSource;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class ChoreographySignalTests : IDisposable
{
    private const int PhaseAddress = 24;
    private const int CueAddress = 28;
    private const int CueMask = 128;
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-choreography-signal-{Guid.NewGuid():N}");

    [Fact]
    public void APhaseWithoutRolesConsumesAnExternalCueAtItsNormalPoll()
    {
        const string field = "EVT_ALEX1_AT_INN";
        EVContainer original = Fixture(field);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "strict"));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        InvocationExpressionSyntax row = Declaration(project, "RunUntilCue", 1);
        Assert.Single(row.ArgumentList.Arguments);

        AssertReplays(project, Export(original, field, "flat", extract: false), 12, (tick, map) =>
        {
            if (tick == 8)
                map[CueAddress] |= CueMask;
        }, result =>
        {
            Assert.Equal(new PhaseSample(1, false), result.Samples[7]);
            Assert.Equal(new PhaseSample(2, false), result.Samples[8]);
        });
    }

    [Fact]
    public void AnEditedParticipantSelectorWriteTakesEffectInsideTheRoleWithoutAPulse()
    {
        const string field = "EVT_ALEX1_TS_STAGE_BK";
        EVContainer original = Fixture(field);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "strict"));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        var setup = Assert.Single(DirectorSource(project).Root.DescendantNodes().OfType<InvocationExpressionSyntax>()
            .Where(call => call.Expression.ToString() == "Choreography"));
        Assert.Equal(2, setup.ArgumentList.Arguments.Count);

        Edit(project, method => method.Identifier.ValueText.StartsWith("Phase6_", StringComparison.Ordinal),
            "Location.CutscenePhase = 7;", "Location.CutscenePhase = 5;");
        EVContainer variant = EventProjectCompiler.Compile(project);
        Assert.NotEqual(original.Serialize(), variant.Serialize());
        string reopened = Export(variant, "EVT_EDITED_ROLE_SELECTOR", "reopened");
        Assert.Equal(variant.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());

        AssertReplays(project, Export(variant, field, "flat", extract: false), 31, (tick, map) =>
        {
            if (tick == 7)
            {
                map[PhaseAddress] = 6;
                map[CueAddress] &= unchecked((byte)~CueMask);
            }
        }, result =>
        {
            Assert.Equal(new PhaseSample(6, false), result.Samples[24]);
            Assert.Equal(new PhaseSample(5, false), result.Samples[25]);
        });
    }

    [Fact]
    public void FinishRolesRunWhileTheEditedCoordinatorWaits()
    {
        const string field = "EVT_GIZ_ENTER_1";
        EVContainer original = Fixture(field);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "strict"));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        var finish = Declaration(project, "Finish", 8);
        Assert.Equal(5, finish.ArgumentList.Arguments.Count);

        Edit(project, method => method.Identifier.ValueText == "FinishScene", "await Engine.Wait(frames: 60);", "await Engine.Wait(frames: 30);");
        EVContainer variant = EventProjectCompiler.Compile(project);
        Assert.NotEqual(original.Serialize(), variant.Serialize());
        string reopened = Export(variant, "EVT_EDITED_FINISH_ROLES", "reopened");
        Assert.Equal(variant.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());

        AssertReplays(project, Export(variant, field, "flat", extract: false), 80, (tick, map) =>
        {
            if (tick == 8)
                map[PhaseAddress] = 8;
        }, result =>
        {
            var transition = Assert.Single(result.Commands.Where(command => command.Opcode == Opcode.MAPJUMP));
            Assert.Equal(63, transition.Tick);
            Assert.Contains(result.Commands, command => command is { Tick: 18, Slot: 10, Opcode: Opcode.MOVE });
        });
    }

    [Fact]
    public void AFinishingRoleCanUseTheSameCueSendingActionAsAnEarlierPhase()
    {
        const string field = "EVT_GIZ_ENTER_1";
        string project = Export(Fixture(field), field, "strict");
        var (path, root) = DirectorSource(project);
        const string earlier = "Phase7_GEO_MAIN_F0_ZDN_09";
        const string finishing = "Phase8_GEO_MAIN_F0_ZDN_09";
        var gate = (MemberAccessExpressionSyntax)Declaration(project, "Until", 7).Expression;
        var changed = root.FindNode(gate.Span, getInnermostNodeForTie: true);
        root = root.ReplaceNode(changed, gate.WithName(SyntaxFactory.IdentifierName("RunUntilCue")));
        var action = Assert.Single(root.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Identifier.ValueText == earlier));
        root = root.ReplaceNode(action, action.WithBody(action.Body!.AddStatements(SyntaxFactory.ParseStatement("Location.AdvanceCutscenePhaseRequested = true;"))));
        var finishRole = Assert.Single(root.DescendantNodes().OfType<ArgumentSyntax>().Where(argument => argument.Expression is IdentifierNameSyntax name && name.Identifier.ValueText == finishing));
        root = root.ReplaceNode(finishRole, finishRole.WithExpression(SyntaxFactory.IdentifierName(earlier)));
        File.WriteAllText(path, root.NormalizeWhitespace().ToFullString());

        EVContainer edited = EventProjectCompiler.Compile(project);
        string reopened = SourceProjectTest.CopyWithoutComments(Export(edited, "EVT_SHARED_FINISH_ROLE", "reopened"));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        var earlierRole = Declaration(reopened, "RunUntilCue", 7);
        var laterRole = Declaration(reopened, "Finish", 8);
        Assert.Contains(earlierRole.DescendantNodes().OfType<ArgumentSyntax>(), argument => argument.Expression.ToString() == earlier);
        Assert.Contains(laterRole.DescendantNodes().OfType<ArgumentSyntax>(), argument => argument.Expression.ToString() == earlier);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(reopened));
        var scheduler = new EventScheduler(new RecordingHost(), new byte[4096], new byte[80]);
        scheduler.RegisterProject(BuildSdk(reopened));
    }

    [Fact]
    public void ABarrierRequiresARealPulse()
    {
        const string field = "EVT_ALEX1_TS_STAGE_1A";
        string project = Export(Fixture(field), field, "strict");
        var (path, root) = DirectorSource(project);
        InvocationExpressionSyntax setup = Assert.Single(root.DescendantNodes().OfType<InvocationExpressionSyntax>()
            .Where(call => call.Expression.ToString() == "Choreography"));
        File.WriteAllText(path, root.ReplaceNode(setup, setup.WithArgumentList(setup.ArgumentList.WithArguments(setup.ArgumentList.Arguments.RemoveAt(2)))).ToFullString());

        Assert.Contains("requires choreography pulse storage", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
        Assert.Contains("requires choreography pulse storage", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project)).Message);
        var scheduler = new EventScheduler(new RecordingHost(), new byte[4096], new byte[80]);
        Assert.Contains("requires choreography pulse storage", Assert.Throws<InvalidOperationException>(() => scheduler.RegisterProject(BuildSdk(project))).Message);
    }

    [Fact]
    public void ARoleSwitchCanSkipDirectlyToTheNextPoll()
    {
        const string field = "EVT_ALEX1_AC_OUT_C";
        EVContainer original = Fixture(field);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "strict"));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        AssertReplays(project, Export(original, field, "flat", extract: false), 47, (tick, map) =>
        {
            if (tick == 5)
            {
                map[PhaseAddress] = 0;
                map[25] = 99;
            }
            if (tick == 10)
                map[25] = 1;
        }, result =>
        {
            Assert.DoesNotContain(result.Commands, command => command is { Tick: >= 5 and < 40, Slot: 5 });
            Assert.Contains(result.Commands, command => command is { Tick: 40, Slot: 5, Opcode: Opcode.POS });
        });
    }

    private void AssertReplays(string project, string flat, int ticks, Action<int, byte[]> beforeTick, Action<Replay> verify)
    {
        Replay expected = Trace(Assembly.Load(EventProjectCompiler.CompileManaged(flat)), ticks, beforeTick);
        verify(expected);
        foreach (Assembly assembly in new[] { Assembly.Load(EventProjectCompiler.CompileManaged(project)), BuildSdk(project) })
        {
            Replay result = Trace(assembly, ticks, beforeTick);
            Assert.Equal(expected.Samples, result.Samples);
            Assert.Equal(expected.Commands, result.Commands);
        }
    }

    private Assembly BuildSdk(string project)
    {
        string artifacts = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));
        return Assembly.Load(File.ReadAllBytes(dll));
    }

    private static Replay Trace(Assembly assembly, int ticks, Action<int, byte[]> beforeTick)
    {
        var host = new RecordingHost();
        byte[] location = new byte[80];
        var scheduler = new EventScheduler(host, new byte[4096], location);
        scheduler.RegisterProject(assembly);
        scheduler.StartScene();
        var samples = new PhaseSample[ticks];
        for (int tick = 0; tick < ticks; tick++)
        {
            host.Tick = tick;
            beforeTick(tick, location);
            scheduler.Tick();
            samples[tick] = new PhaseSample(location[PhaseAddress], (location[CueAddress] & CueMask) != 0);
        }
        return new Replay(samples, host.Commands.ToArray());
    }

    private static void Edit(string project, Func<MethodDeclarationSyntax, bool> findMethod, string original, string edit)
    {
        var (path, root) = DirectorSource(project);
        var statement = Assert.Single(root.DescendantNodes().OfType<ExpressionStatementSyntax>().Where(node => node.ToString() == original &&
            node.FirstAncestorOrSelf<MethodDeclarationSyntax>() is { } method && findMethod(method)));
        File.WriteAllText(path, root.ReplaceNode(statement, SyntaxFactory.ParseStatement(edit).WithTriviaFrom(statement)).ToFullString());
    }

    private static InvocationExpressionSyntax Declaration(string project, string operation, int value) =>
        Assert.Single(DirectorSource(project).Root.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call =>
            call.Expression is MemberAccessExpressionSyntax member && member.Name.Identifier.ValueText == operation &&
            call.ArgumentList.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax literal && Equals(literal.Token.Value, value)));

    private static (string Path, SyntaxNode Root) DirectorSource(string project) =>
        Assert.Single(Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs")
            .Select(path => (Path: path, Root: CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot()))
            .Where(file => file.Root.DescendantNodes().OfType<MethodDeclarationSyntax>().Any(method => method.Identifier.ValueText == "Direct")));

    private string Export(EVContainer original, string field, string leaf, bool extract = true)
    {
        string project = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(original, field, project, useCutsceneNames: false, extractCutscenes: extract);
        return project;
    }

    private static EVContainer Fixture(string field)
    {
        using Stream input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", field + ".eb.bytes"));
        return EVContainer.Read(input);
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }

    private sealed record Replay(PhaseSample[] Samples, Command[] Commands);
    private sealed record PhaseSample(int Phase, bool Cue);
    private sealed record Command(int Tick, int Slot, Opcode Opcode, string Operands);

    private sealed class RecordingHost : IEventHost
    {
        public int Tick { get; set; }
        public List<Command> Commands { get; } = [];

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Commands.Add(new Command(Tick, objectId, opcode, string.Join(",", operands.ToArray().Select(operand => (int)operand))));
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }
}
