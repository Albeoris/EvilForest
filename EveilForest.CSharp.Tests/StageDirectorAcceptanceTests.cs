using System.Reflection;
using EvilForest.EventSource;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;
using Xunit.Abstractions;

namespace EveilForest.CSharp.Tests;

public sealed class StageDirectorAcceptanceTests(ITestOutputHelper output) : IDisposable
{
    private const string Stage = "EVT_ALEX1_TS_STAGE_1A";
    private const string Kitchen = "EVT_SARI1_MS_KTN_0";
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-stage-director-{Guid.NewGuid():N}");
    private static readonly (int Slot, int[] Phases)[] Cast =
    [
        (8, [3, 7, 8]),
        (9, [3, 7, 8]),
        (10, [3, 7, 8]),
        (11, [0, 1, 2, 3]),
        (13, [2, 3, 8]),
        (18, [2, 3, 6, 8]),
        (19, [3, 5, 8]),
        (20, [2, 3, 4, 8])
    ];

    [Fact]
    public void StageDirectorOwnsItsCastAndMatchesTheOriginalBytesAndScheduler()
    {
        EVContainer original = Fixture(Stage);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, Stage, "extracted"));
        DirectorScene scene = FindDirector(project);
        AssertCast(project, scene, Enumerable.Range(0, 10));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        StageTrace reference = Trace(Export(original, Stage, "flat", extract: false));
        StageTrace actual = Trace(project);
        AssertStageTrace(actual);
        AssertEquivalent(reference, actual);

        Assert.Contains(scene.Method.AttributeLists.SelectMany(list => list.Attributes), attribute => attribute.Name.ToString() == "OriginalDirectorSlot");
        string withoutHints = scene.Method.WithAttributeLists(default).ToFullString();
        Edit(scene.Path, scene.Method, scene.Method.ToFullString(), withoutHints);
        AssertEquivalent(reference, Trace(project));
    }

    [Fact]
    public void ReturningEarlyFromARoleRetriesOnTheNextTickWithoutSendingItsCue()
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string storage = Path.Combine(flat, "Event.cs");
        var storageRoot = Parse(storage);
        var location = storageRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().Single(type => type.Identifier.ValueText == "Location");
        var counter = SyntaxFactory.ParseMemberDeclaration("""
            [StorageView(StorageScope.Map, StorageType.Byte, 62)]
            public int Attempts
            {
                get => ScriptMemory.Read(StorageScope.Map, StorageType.Byte, 62);
                set => ScriptMemory.Write(StorageScope.Map, StorageType.Byte, 62, value);
            }
            """)!;
        File.WriteAllText(storage, storageRoot.ReplaceNode(location, location.AddMembers(counter)).NormalizeWhitespace().ToFullString());

        string marcus = Participant(flat, 19);
        var phase = PhaseSwitch(marcus).Sections.Single(section => PhaseNumber(section) == 5);
        var guard = SyntaxFactory.ParseStatement("""
            if (Location.Attempts < 3)
            {
                Location.Attempts++;
                break;
            }
            """);
        Edit(marcus, phase, phase.ToFullString(), phase.WithStatements(phase.Statements.Insert(0, guard)).NormalizeWhitespace().ToFullString());
        EVContainer variant = EventProjectCompiler.Compile(flat);
        string strict = SourceProjectTest.CopyWithoutComments(Export(variant, "EVT_RETRY_ROLE", "strict"));
        Assert.Equal(variant.Serialize(), EventProjectCompiler.Compile(strict).Serialize());
        var scene = FindDirector(strict);
        var declaration = Assert.Single(StageRows(scene).Where(row => RowNumber(row) == 5));
        Assert.Equal("RunUntilCue", Member(declaration));
        var action = PhaseMember(strict, scene, 19, 5);
        Assert.Contains(action.DescendantNodes(), node => node is ReturnStatementSyntax);

        var reference = Replay(Assembly.Load(EventProjectCompiler.CompileManaged(flat)));
        AssertEquivalent(reference, Replay(Assembly.Load(EventProjectCompiler.CompileManaged(strict))));
        string artifacts = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(strict), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));
        AssertEquivalent(reference, Replay(Assembly.Load(File.ReadAllBytes(dll))));

        static StageTrace Replay(Assembly assembly)
        {
            var samples = new List<(byte Phase, byte Attempts, bool Cue)>();
            StageTrace trace = Trace(assembly, beforeTick: memory => samples.Add((memory[24], memory[62], (memory[28] & 128) != 0)));
            int first = samples.FindIndex(sample => sample.Attempts == 1);
            Assert.True(first >= 0);
            Assert.Equal(new (byte, byte, bool)[] { (5, 1, false), (5, 2, false), (5, 3, false) }, samples.Skip(first).Take(3));
            Assert.Contains(trace.Commands, command => command.Operation == Opcode.ENCOUNT);
            return trace;
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ADirectorWithoutSuspensionPreservesItsEntryShape(bool standalone)
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string path = Participant(flat, 4);
        if (!standalone)
            Edit(path, Method(path, "OnInit"), "return;", "Location.Bit_230 = true;\n        return;");

        CompilationUnitSyntax unit = Parse(path);
        var handler = unit.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "OnLoop");
        var loop = Assert.IsType<WhileStatementSyntax>(Assert.Single(handler.Body!.Statements));
        var body = Assert.IsType<BlockSyntax>(loop.Statement);
        ExpressionStatementSyntax[] waits = body.DescendantNodes().OfType<ExpressionStatementSyntax>()
            .Where(statement => statement.Expression is AwaitExpressionSyntax).ToArray();
        Assert.Equal(2, waits.Length);
        body = body.RemoveNodes(waits, SyntaxRemoveOptions.KeepNoTrivia)!;
        var statements = body.Statements.Insert(0, SyntaxFactory.ParseStatement("Location.Byte_24 = 9;")).Add(SyntaxFactory.ReturnStatement());
        var edited = handler.WithModifiers(SyntaxFactory.TokenList(handler.Modifiers.Where(token => !token.IsKind(SyntaxKind.AsyncKeyword))))
            .WithReturnType(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword))).WithBody(body.WithStatements(statements));
        File.WriteAllText(path, unit.ReplaceNode(handler, edited).NormalizeWhitespace().ToFullString());

        EVContainer variant = EventProjectCompiler.Compile(flat);
        string reopened = Export(variant, "EVT_NONSUSPENDING_STAGE_DIRECTOR", "reopened", allowNative: true);
        string scenePath = Assert.Single(Directory.GetFiles(Path.Combine(reopened, "Cutscenes"), "*.cs"));
        MethodDeclarationSyntax direct = Method(scenePath, "Direct");
        Assert.DoesNotContain(direct.DescendantNodes(), node => node is AwaitExpressionSyntax);
        if (standalone)
        {
            Assert.Empty(direct.ParameterList.Parameters);
            Assert.Equal("EventRoutine", direct.ReturnType.ToString());
            Assert.Contains(Invocations(reopened), call => Member(call) == "StartDirector");
        }
        else
        {
            Assert.Single(direct.ParameterList.Parameters);
            Assert.Equal("void", direct.ReturnType.ToString());
            Assert.Contains("Location.Bit_230 = true;", Method(Participant(reopened, 4), "OnInit").ToFullString());
        }
        Assert.Equal(variant.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        StageTrace reference = Trace(flat);
        Assert.Contains(reference.Commands, command => command.Owner == 4 && command.Operation == Opcode.MAPJUMP);
        AssertEquivalent(reference, Trace(reopened));
    }

    [Fact]
    public void RemovingOneRoleInTheDirectorResizesTheJoinAndChangesBothExecutions()
    {
        EVContainer original = Fixture(Stage);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, "EVT_RECAST_PLAY", "editable"));
        string flat = SourceProjectTest.CopyWithoutComments(Export(original, Stage, "flat", extract: false));
        DirectorScene scene = FindDirector(project);
        var row = Assert.Single(StageRows(scene).Where(call => RowNumber(call) == 3));
        string action = PhaseMember(project, scene, 20, 3).Identifier.ValueText;
        var removedRole = Assert.Single(row.ArgumentList.Arguments.Where(argument => argument.Expression is InvocationExpressionSyntax role &&
            Member(role) == "Role" && role.ArgumentList.Arguments.Last().Expression.ToString() == action));
        var edited = row.WithArgumentList(row.ArgumentList.WithArguments(row.ArgumentList.Arguments.Remove(removedRole)));
        Edit(scene.Path, row, row.ToFullString(), edited.ToFullString());

        string blank = Participant(flat, 20);
        var dispatch = PhaseSwitch(blank);
        var section = Assert.Single(dispatch.Sections.Where(arm => PhaseNumber(arm) == 3));
        Edit(blank, dispatch, section.ToFullString(), "");
        string camera = Participant(flat, 11);
        Edit(camera, PhaseSwitch(camera).Sections.Single(arm => PhaseNumber(arm) == 3), "Location.Byte_26 = 8;", "Location.Byte_26 = 7;");

        byte[] expected = EventProjectCompiler.Compile(flat).Serialize();
        Assert.False(original.Serialize().SequenceEqual(expected));
        Assert.Equal(expected, EventProjectCompiler.Compile(project).Serialize());
        StageTrace trace = Trace(flat);
        Assert.Contains(trace.Commands, command => command.Operation == Opcode.ENCOUNT);
        Assert.Contains(trace.Barrier, sample => sample.Count > 0);
        AssertEquivalent(trace, Trace(project));
        var artifacts = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));
        AssertEquivalent(trace, Trace(Assembly.Load(File.ReadAllBytes(dll))));
    }

    [Fact]
    public void AnIdleDirectorCanLeaveAnActorsRoleActive()
    {
        const string field = "EVT_ALEX3_AT_INN";
        EVContainer original = Fixture(field);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "idle"));
        DirectorScene scene = FindDirector(project);
        var idle = Assert.Single(scene.Method.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "Idle"));
        Assert.Equal(0, RowNumber(idle));
        Assert.Single(idle.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "Role"));
        Assert.DoesNotContain(scene.Method.DescendantNodes(), node => node is SwitchStatementSyntax);
        var loop = Method(Participant(project, 5), "OnLoop");
        Assert.Equal("Perform", Member(Assert.Single(loop.DescendantNodes().OfType<InvocationExpressionSyntax>())));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Fact]
    public void AParticipantOnlyEntrancePreservesTheActorsEditablePhaseWithoutAddingADirectorCase()
    {
        const string field = "EVT_ALEX1_AT_STREET_A";
        var original = Fixture(field);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "entrance"));
        string scene = Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs").Single(path => File.ReadAllText(path).Contains("play.Idle(8,"));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        var role = Assert.Single(Method(scene, "FromPhase8").DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "Role"));
        var body = Method(scene, role.ArgumentList.Arguments[1].Expression.ToString());
        var write = Assert.Single(body.DescendantNodes().OfType<AssignmentExpressionSyntax>());
        Edit(scene, write, "true", "false");
        var edited = EventProjectCompiler.Compile(project);
        Assert.Equal(original.Objects[0].Scripts.Single(script => script.Id == 1).Body, edited.Objects[0].Scripts.Single(script => script.Id == 1).Body);
        Assert.NotEqual(original.Objects[19].Scripts.Single(script => script.Id == 1).Body, edited.Objects[19].Scripts.Single(script => script.Id == 1).Body);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void AnInteractionThatChangesPhaseRunsOnlyWhenRequested()
    {
        const string field = "EVT_ALEX1_AT_BACK_STR";
        var original = Fixture(field);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "interaction"));
        string flat = Export(original, field, "flat-interaction", extract: false);
        string bridge = Member(Assert.Single(Method(Participant(project, 3), "OnSpeak").DescendantNodes().OfType<InvocationExpressionSyntax>()));
        string scene = FindDirector(project).Path;
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.DoesNotContain(Parse(scene).DescendantNodes().OfType<InvocationExpressionSyntax>(), call => Member(call) == "Role" && call.ArgumentList.Arguments.Any(arg => arg.Expression.ToString() == bridge));
        foreach (var (path, method, selector) in new[] { (scene, bridge, "Location.CutscenePhase"), (Participant(flat, 3), "OnSpeak", "Location.Byte_24") })
        {
            var body = Method(path, method).Body!;
            Edit(path, body.Statements[0], body.Statements[0].ToString(), $"{selector} = 77;{Environment.NewLine}" + body.Statements[0]);
        }
        Assert.Equal(EventProjectCompiler.Compile(flat).Serialize(), EventProjectCompiler.Compile(project).Serialize());

        foreach (var source in new[] { flat, project })
        {
            var location = new byte[80];
            var scheduler = new EventScheduler(new StageHost(), new byte[4096], location);
            scheduler.RegisterProject(Assembly.Load(EventProjectCompiler.CompileManaged(source)));
            scheduler.CreateParticipant(3);
            location[24] = 15;
            for (int i = 0; i < 8; i++)
                scheduler.Tick();
            Assert.Equal(15, location[24]);
            Assert.True(scheduler.TryTrigger(3, 3, 0));
            scheduler.Tick();
            Assert.Equal(77, location[24]);
        }
    }

    [Fact]
    public void RetargetingAParticipantContinuationRunsTheEditedRoleInTheSameTick()
    {
        const string field = "EVT_CLEYRA2_TOWN";
        string project = SourceProjectTest.CopyWithoutComments(Export(Fixture(field), field, "linked"));
        string path = FindDirector(project).Path;
        var link = Assert.Single(Parse(path).DescendantNodes().OfType<InvocationExpressionSyntax>(), call => Member(call) == "Role" &&
            call.ArgumentList.Arguments.Last().NameColon?.Name.Identifier.ValueText == "thenPhase");
        var actor = link.ArgumentList.Arguments[0].Expression.ToString();
        Edit(path, link, "thenPhase: 6", "thenPhase: 7");
        MarkRole(5, "Location.Byte_16 = 50;", "await Engine.Wait(frames: 2);", "Location.Byte_16 = 51;");
        MarkRole(7, "Location.Byte_17 = 7;");

        EVContainer modified = EventProjectCompiler.Compile(project);
        string decoded = Export(modified, field, "decoded-links", extract: false);
        var expected = Replay(Assembly.Load(EventProjectCompiler.CompileManaged(decoded)));
        Assert.Contains(expected, sample => sample == (5, 50, 0));
        Assert.Contains(expected, sample => sample == (5, 51, 7));
        Assert.DoesNotContain(expected, sample => sample.Item2 == 51 && sample.Item3 == 0);
        Assert.Equal(expected, Replay(Assembly.Load(EventProjectCompiler.CompileManaged(project))));
        string artifacts = Path.Combine(_directory, "linked-sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));
        Assert.Equal(expected, Replay(Assembly.Load(File.ReadAllBytes(dll))));

        string valid = File.ReadAllText(path);
        foreach (int target in new[] { 5, 99 })
        {
            File.WriteAllText(path, valid.Replace("thenPhase: 7", $"thenPhase: {target}", StringComparison.Ordinal));
            Assert.Contains("A role continuation must reach a declared phase", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
            Assert.Contains("A role continuation must reach a declared phase", Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project)).Message);
        }

        void MarkRole(int phase, params string[] statements)
        {
            var row = Assert.Single(Parse(path).DescendantNodes().OfType<InvocationExpressionSyntax>(), call => call.Expression is MemberAccessExpressionSyntax
                { Expression: IdentifierNameSyntax { Identifier.ValueText: "play" } } && call.ArgumentList.Arguments.FirstOrDefault()?.Expression.ToString() == phase.ToString());
            var role = Assert.Single(row.DescendantNodes().OfType<InvocationExpressionSyntax>(), call => Member(call) == "Role" && call.ArgumentList.Arguments[0].Expression.ToString() == actor);
            var method = Method(path, role.ArgumentList.Arguments[1].Expression.ToString());
            string source = File.ReadAllText(path);
            var edited = SyntaxFactory.Block(statements.Select(statement => SyntaxFactory.ParseStatement(statement))).NormalizeWhitespace();
            File.WriteAllText(path, source.Remove(method.Body!.SpanStart, method.Body.Span.Length).Insert(method.Body.SpanStart, edited.ToFullString()));
        }

        static List<(int, int, int)> Replay(Assembly assembly)
        {
            var location = new byte[80];
            var scheduler = new EventScheduler(new StageHost(), new byte[4096], location);
            scheduler.RegisterProject(assembly);
            scheduler.CreateParticipant(6);
            location[24] = 5;
            var trace = new List<(int, int, int)>();
            for (int tick = 0; tick < 8; tick++)
            {
                scheduler.Tick();
                trace.Add((location[24], location[16], location[17]));
            }
            return trace;
        }
    }

    [Fact]
    public void AnOutOfBandPhaseWriteIsRejectedBeforeRunningTheScene()
    {
        string project = Export(Fixture(Stage), Stage, "invalid-director");
        DirectorScene scene = FindDirector(project);
        var first = StageRows(scene).First().Ancestors().OfType<ExpressionStatementSyntax>().First();
        Edit(scene.Path, first, first.ToString(), "Location.CutscenePhase = 3;" + Environment.NewLine + first);
        Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project));
        Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project));

        string artifacts = Path.Combine(_directory, "invalid-sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));
        var scheduler = new EventScheduler(new StageHost(), new byte[4096], new byte[80]);
        Assert.Throws<InvalidOperationException>(() => scheduler.RegisterProject(Assembly.Load(File.ReadAllBytes(dll))));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ARoleTransitionCanReachAnotherArmBeforeTheDirectorResumes(bool guarded)
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string zidane = Participant(flat, 13);
        SwitchSectionSyntax release = PhaseSwitch(zidane).Sections.Single(section => PhaseNumber(section) == 8);
        Edit(zidane, release, "Location.Bit_231 = true;", "Location.Byte_24 = 10;");

        CompilationUnitSyntax unit = Parse(zidane);
        SwitchStatementSyntax dispatch = Assert.Single(unit.DescendantNodes().OfType<SwitchStatementSyntax>());
        if (guarded)
        {
            var body = (BlockSyntax)dispatch.Parent!;
            int index = body.Statements.IndexOf(dispatch);
            var guard = SyntaxFactory.ParseStatement("if (Location.Byte_24 == 10) { Location.Byte_24 = 9; }");
            unit = unit.ReplaceNode(body, body.WithStatements(body.Statements.Insert(index + 1, guard)));
        }
        else
        {
            var section = SyntaxFactory.SwitchSection(
                SyntaxFactory.SingletonList<SwitchLabelSyntax>(SyntaxFactory.CaseSwitchLabel(
                    SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(10)))),
                SyntaxFactory.List([SyntaxFactory.ParseStatement("Location.Byte_24 = 9;"), SyntaxFactory.BreakStatement()]));
            unit = unit.ReplaceNode(dispatch, dispatch.WithSections(dispatch.Sections.Add(section)));
        }
        File.WriteAllText(zidane, unit.NormalizeWhitespace().ToFullString());

        EVContainer variant = EventProjectCompiler.Compile(flat);
        string reopened = Export(variant, "EVT_STAGE_ACTOR_TRANSITION", "reopened", allowNative: true);
        DirectorScene director = FindDirector(reopened);
        string[] outline = File.ReadAllLines(Path.Combine(reopened, "CutsceneOutline.tsv"));
        int role = Array.FindIndex(outline, row => row.StartsWith("13\t1\t", StringComparison.Ordinal) && row.Contains("(phase 10)\t\textracted", StringComparison.Ordinal));
        int terminal = Array.FindIndex(outline, row => row.StartsWith("4\t1\t", StringComparison.Ordinal) && row.Contains("(phase 9)", StringComparison.Ordinal));
        Assert.True(role >= 0 && role < terminal);
        var continuation = Assert.Single(Parse(director.Path).DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(method => method.Identifier.ValueText.StartsWith("Phase10_", StringComparison.Ordinal)));
        Assert.Contains("Location.CutscenePhase = 9;", continuation.ToFullString());
        Assert.Equal(variant.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        StageTrace reference = Trace(flat);
        if (!guarded)
            Assert.Contains(reference.Transitions, transition => transition.Phase == 10);
        AssertEquivalent(reference, Trace(reopened));
    }

    [Fact]
    public void DirectorAndParticipantEditsMatchFlatSourceAndStayEditableOnReexport()
    {
        EVContainer original = Fixture(Stage);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, Stage, "extracted"));
        string flat = SourceProjectTest.CopyWithoutComments(Export(original, Stage, "flat", extract: false));
        DirectorScene scene = FindDirector(project);
        Edit(scene.Path, FinishMethod(scene), "battle: 338", "battle: 339");
        Edit(scene.Path, PhaseMember(project, scene, 13, 8), "frames: 30", "frames: 39");
        Edit(Participant(flat, 4), Method(Participant(flat, 4), "OnLoop"), "battle: 338", "battle: 339");
        Edit(Participant(flat, 13), PhaseSwitch(Participant(flat, 13)).Sections.Single(section => PhaseNumber(section) == 8), "frames: 30", "frames: 39");

        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.False(original.Serialize().SequenceEqual(edited.Serialize()));
        Assert.Equal(EventProjectCompiler.Compile(flat).Serialize(), edited.Serialize());
        StageTrace actual = Trace(project);
        AssertEquivalent(Trace(flat), actual);
        StageTrace reference = Trace(Export(original, Stage, "original", extract: false));
        Assert.Equal(9, actual.Commands.Single(command => command.Operation == Opcode.MESN && command.Arguments.EndsWith(",94")).Frame
            - reference.Commands.Single(command => command.Operation == Opcode.MESN && command.Arguments.EndsWith(",94")).Frame);
        Assert.Equal("0,339", actual.Commands.Single(command => command.Operation == Opcode.ENCOUNT).Arguments);

        string reopened = Export(edited, Stage, "reopened");
        DirectorScene reopenedScene = FindDirector(reopened);
        Assert.Contains("battle: 339", FinishMethod(reopenedScene).ToFullString());
        Assert.Contains("frames: 39", PhaseMember(reopened, reopenedScene, 13, 8).ToFullString());
        AssertEquivalent(actual, Trace(reopened));
    }

    [Fact]
    public void ARelocatedSelectorAndReorderedTransitionsRediscoverTheSameCast()
    {
        EVContainer original = Fixture(Stage);
        string flat = Export(original, Stage, "flat", extract: false);
        string storage = Path.Combine(flat, "Event.cs");
        Edit(storage, Parse(storage), "FieldOffset(24)", "FieldOffset(39)");
        string director = Participant(flat, 4);
        Edit(director, PhaseSwitch(director).Sections.Single(section => PhaseNumber(section) == 4), "= 5;", "= 6;");
        Edit(director, PhaseSwitch(director).Sections.Single(section => PhaseNumber(section) == 6), "= 7;", "= 5;");
        Edit(director, PhaseSwitch(director).Sections.Single(section => PhaseNumber(section) == 5), "= 6;", "= 7;");

        EVContainer edited = EventProjectCompiler.Compile(flat);
        string reopened = Export(edited, "EVT_HELD_OUT_STAGE_VARIANT", "reopened");
        DirectorScene scene = FindDirector(reopened);
        int[] phaseOrder = [0, 1, 2, 3, 4, 6, 5, 7, 8, 9];
        AssertCast(reopened, scene, phaseOrder);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        StageTrace reference = Trace(flat, phaseAddress: 39);
        StageTrace actual = Trace(reopened, phaseAddress: 39);
        AssertEquivalent(reference, actual);
        Assert.Equal(phaseOrder, actual.Transitions.Select(transition => (int)transition.Phase));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void TwoEntrancesShareOneEditableContinuationInBothCompilers(int initialPhase)
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string coordinator = Participant(flat, 4);
        Edit(coordinator, PhaseSwitch(coordinator).Sections.Single(section => PhaseNumber(section) == 5), "Location.Byte_24 = 6;", "Location.Byte_24 = 8;");
        if (initialPhase != 0)
        {
            string main = Participant(flat, 0);
            Edit(main, Method(main, "OnInit"), "Location.Byte_24 = 0;", $"Location.Byte_24 = {initialPhase};");
        }

        EVContainer original = EventProjectCompiler.Compile(flat);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, "EVT_ROUTED_PLAY", "strict"));
        DirectorScene scene = FindDirector(project);
        var routes = Assert.Single(scene.Method.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "Routes"));
        Assert.Equal(2, routes.ArgumentList.Arguments.Count);
        var entrances = routes.ArgumentList.Arguments.Select(arg => Method(scene.Path, Assert.IsType<IdentifierNameSyntax>(arg.Expression).Identifier.ValueText)).ToArray();
        string[] tails = entrances.Select(entry => Assert.Single(entry.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "ContinueWith"))
            .ArgumentList.Arguments.Single().Expression.ToString()).ToArray();
        Assert.Equal(tails[0], tails[1]);
        var continuation = Method(scene.Path, tails[0]);
        Assert.Contains(continuation.DescendantNodes().OfType<InvocationExpressionSyntax>(), call => Member(call) == "Until" && RowNumber(call) == 8);
        Assert.DoesNotContain(entrances.Append(continuation).SelectMany(method => method.DescendantNodes()), node => node is SwitchStatementSyntax or GotoStatementSyntax);
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        StageTrace before = Trace(flat);
        AssertEquivalent(before, Trace(project));
        Assert.Equal(initialPhase == 6, before.Transitions.Any(transition => transition.Phase == 6));

        var sharedActor = Parse(scene.Path).DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.ToString().Contains("messageId: 94"));
        Edit(scene.Path, sharedActor, "frames: 30", "frames: 39");
        string zidane = Participant(flat, 13);
        Edit(zidane, PhaseSwitch(zidane).Sections.Single(section => PhaseNumber(section) == 8), "frames: 30", "frames: 39");
        Assert.Equal(EventProjectCompiler.Compile(flat).Serialize(), EventProjectCompiler.Compile(project).Serialize());
        StageTrace after = Trace(flat);
        Assert.Equal(9, DialogueFrame(after) - DialogueFrame(before));
        AssertEquivalent(after, Trace(project));
        string artifacts = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));
        AssertEquivalent(after, Trace(Assembly.Load(File.ReadAllBytes(dll))));

        if (initialPhase == 0)
        {
            var firstRoute = entrances.Single(method => method.DescendantNodes().OfType<InvocationExpressionSyntax>().Any(call => Member(call) == "Together" && RowNumber(call) == 0));
            var secondRoute = entrances.Single(method => method != firstRoute);
            var join = firstRoute.DescendantNodes().OfType<InvocationExpressionSyntax>().Single(call => Member(call) == "ContinueWith");
            Edit(scene.Path, join, tails[0], secondRoute.Identifier.ValueText);
            Edit(coordinator, PhaseSwitch(coordinator).Sections.Single(section => PhaseNumber(section) == 5), "Location.Byte_24 = 8;", "Location.Byte_24 = 6;");
            EVContainer rewired = EventProjectCompiler.Compile(project);
            Assert.Equal(EventProjectCompiler.Compile(flat).Serialize(), rewired.Serialize());
            StageTrace longerPath = Trace(flat);
            Assert.Contains(longerPath.Transitions, transition => transition.Phase == 6);
            AssertEquivalent(longerPath, Trace(project));
            var reopened = FindDirector(Export(rewired, "EVT_REWIRED_PLAY", "reopened"));
            Assert.DoesNotContain(reopened.Method.DescendantNodes().OfType<InvocationExpressionSyntax>(), call => Member(call) == "Routes");
        }

        static int DialogueFrame(StageTrace trace) => trace.Commands.Single(command => command.Operation == Opcode.MESN && command.Arguments.EndsWith(",94")).Frame;
    }

    [Theory]
    [InlineData("EVT_ALEX1_TS_CARGO_3", 2, 1)]
    [InlineData("EVT_ALEX2_AC_SUMMON_A", 0, 2)]
    public void PublishedRoutesShowTheWholeDirectorBeforeTheCast(string field, int continuations, int endings)
    {
        EVContainer original = Fixture(field);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, field, "routes"));
        DirectorScene scene = FindDirector(project);
        var methods = Assert.IsType<ClassDeclarationSyntax>(scene.Method.Parent).Members.OfType<MethodDeclarationSyntax>().ToArray();
        var directorMethods = methods.Skip(1).TakeWhile(method => method.ParameterList.Parameters is [{ Type: IdentifierNameSyntax { Identifier.ValueText: "SceneChoreography" } }]).ToArray();
        Assert.Equal(scene.Method.Identifier.ValueText, methods[0].Identifier.ValueText);
        var routes = Assert.Single(scene.Method.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "Routes"));
        var rows = directorMethods.SelectMany(method => method.DescendantNodes().OfType<InvocationExpressionSyntax>()).ToArray();
        var joins = rows.Where(call => Member(call) == "ContinueWith").ToArray();
        Assert.Equal(continuations, joins.Length);
        var definitions = routes.ArgumentList.Arguments.Concat(joins.SelectMany(call => call.ArgumentList.Arguments)).Select(arg => arg.Expression.ToString()).Distinct().Order().ToArray();
        Assert.Equal(definitions, directorMethods.Select(method => method.Identifier.ValueText).Order());
        Assert.Equal(endings, rows.Count(call => Member(call) == "Idle"));
        if (continuations != 0)
        {
            var first = Assert.Single(rows.Where(call => Member(call) == "Together" && RowNumber(call) == 1));
            Assert.Single(first.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "Barrier"));
            Assert.Equal(2, first.DescendantNodes().OfType<InvocationExpressionSyntax>().Count(call => Member(call) == "Role"));
        }
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AChoiceConsumesTheCueOnAnUnmatchedKeyAndHonorsCaseEdits(bool revisit)
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string coordinator = Participant(flat, 4);
        int firstDestination = revisit ? 4 : 6;
        string choice = $$"""
            switch (Location.Int16_34)
            {
                case 1:
                    Location.Byte_24 = {{firstDestination}};
                    break;
                case 2:
                    Location.Byte_24 = 8;
                    break;
            }
            """;
        Edit(coordinator, PhaseSwitch(coordinator).Sections.Single(section => PhaseNumber(section) == 5), "Location.Byte_24 = 6;", choice);
        EVContainer original = EventProjectCompiler.Compile(flat);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, "EVT_HELD_OUT_CHOICE", "strict"));
        DirectorScene scene = FindDirector(project);
        var choose = Assert.Single(Invocations(scene.Path).Where(call => Member(call) == "Choose"));
        var cases = choose.ArgumentList.Arguments.Skip(1).Select(argument => Assert.IsType<InvocationExpressionSyntax>(argument.Expression)).ToArray();
        Assert.Equal(new[] { 1, 2 }, cases.Select(RowNumber));
        Assert.All(cases, arm => Assert.IsType<IdentifierNameSyntax>(arm.ArgumentList.Arguments[1].Expression));
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        var reference = Exercise(Assembly.Load(EventProjectCompiler.CompileManaged(flat)));
        AssertEquivalent(reference, Exercise(Assembly.Load(EventProjectCompiler.CompileManaged(project))));
        Assert.Contains(reference.Commands, command => command.Operation == Opcode.ENCOUNT);
        Assert.Equal(revisit ? 2 : 1, reference.Transitions.Count(transition => transition.Phase == 5));

        string artifacts = Path.Combine(_directory, "sdk");
        SourceProjectTest.RunDotnet(["build", SourceProjectTest.ProjectFile(project), "-c", "Release", "--artifacts-path", artifacts, "--nologo"]);
        string dll = Assert.Single(Directory.GetFiles(Path.Combine(artifacts, "bin"), "Event.dll", SearchOption.AllDirectories));
        AssertEquivalent(reference, Exercise(Assembly.Load(File.ReadAllBytes(dll))));

        Edit(scene.Path, cases[0], "Case(1,", "Case(3,");
        var switchInCoordinator = Method(coordinator, "OnLoop").DescendantNodes().OfType<SwitchStatementSyntax>().Single(statement => statement.Expression.ToString() == "Location.Int16_34");
        Edit(coordinator, switchInCoordinator, "case 1:", "case 3:");
        byte[] edited = EventProjectCompiler.Compile(flat).Serialize();
        Assert.False(original.Serialize().SequenceEqual(edited));
        Assert.Equal(edited, EventProjectCompiler.Compile(project).Serialize());
        AssertEquivalent(Exercise(Assembly.Load(EventProjectCompiler.CompileManaged(flat)), matchingKey: 3), Exercise(Assembly.Load(EventProjectCompiler.CompileManaged(project)), matchingKey: 3));

        StageTrace Exercise(Assembly assembly, byte matchingKey = 1)
        {
            int signalCount = 0;
            bool checkConsumed = false;
            StageTrace trace = Trace(assembly, beforeTick: location =>
            {
                if (checkConsumed)
                {
                    Assert.Equal(5, location[24]);
                    Assert.Equal(0, location[28] & 128);
                    location[28] |= 128;
                    checkConsumed = false;
                }
                if (location[24] != 5 || (location[28] & 128) == 0)
                    return;
                signalCount++;
                location[34] = signalCount switch
                {
                    1 => (byte)99,
                    2 => matchingKey,
                    _ => (byte)2
                };
                location[35] = 0;
                checkConsumed = signalCount == 1;
            });
            Assert.Equal(revisit ? 3 : 2, signalCount);
            return trace;
        }
    }

    [Fact]
    public void AConvergingPhaseIsPlacedAfterBothPredecessors()
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string director = Participant(flat, 4);
        string branch = """
            if (Location.Bit_230 == true)
            {
                Location.Byte_24 = 4;
            }
            else
            {
                Location.Byte_24 = 5;
            }
            """;
        Edit(director, PhaseSwitch(director).Sections.Single(section => PhaseNumber(section) == 3), "Location.Byte_24 = 4;", branch);
        Edit(director, PhaseSwitch(director).Sections.Single(section => PhaseNumber(section) == 4), "= 5;", "= 6;");
        EVContainer edited = EventProjectCompiler.Compile(flat);
        var failure = Assert.Throws<InvalidDataException>(() => Export(edited, "EVT_CONVERGING_SCENE", "strict-diamond"));
        Assert.Contains("EVT_CONVERGING_SCENE", failure.Message);
        Assert.Contains("cannot export one choreography script", failure.Message);
        Assert.False(Directory.Exists(Path.Combine(_directory, "strict-diamond")));
        string reopened = Export(edited, "EVT_CONVERGING_SCENE", "diamond", allowNative: true);
        DirectorScene scene = FindDirector(reopened);
        int left = PhaseMember(reopened, scene, 20, 4).SpanStart;
        int right = PhaseMember(reopened, scene, 19, 5).SpanStart;
        int join = PhaseMember(reopened, scene, 18, 6).SpanStart;
        Assert.True(left < join && right < join);
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void AnAmbiguousRoleInARecognizedMachineAbortsExportWithItsLocation()
    {
        EVContainer modified = Fixture(Stage);
        EVContainerObject zidane = modified.Objects[13];
        EVContainerScript loop = Assert.Single(zidane.Scripts.Where(script => script.Id == 1));
        zidane.Scripts = [.. zidane.Scripts, new EVContainerScript(loop.Id, loop.Body.ToArray())];
        using var input = new MemoryStream(modified.Serialize());
        EVContainer reopened = EVContainer.Read(input);
        const string name = "EVT_AMBIGUOUS_DIRECTOR_VARIANT";
        InvalidDataException error = Assert.Throws<InvalidDataException>(() => Export(reopened, name, "ambiguous"));
        Assert.Contains(name, error.Message);
        Assert.Contains("slot 13, tag 1, phase 2", error.Message);
        Assert.Contains("ambiguous dispatch arm or handler", error.Message);
        Assert.False(Directory.Exists(Path.Combine(_directory, "ambiguous")));
    }

    [Theory]
    [InlineData("Location.Byte_24 == 8")]
    [InlineData("8 == Location.Byte_24")]
    public void APhaseGuardOutsideTheSwitchIsStillEditableInTheScene(string condition)
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string actor = Participant(flat, 8);
        string original = File.ReadAllText(actor);
        SwitchStatementSyntax dispatch = PhaseSwitch(actor);
        SwitchSectionSyntax phase = Assert.Single(dispatch.Sections.Where(section => PhaseNumber(section) == 8));
        string guard = $"{Environment.NewLine}            if ({condition}){Environment.NewLine}            {{{Environment.NewLine}" +
            string.Concat(phase.Statements.SkipLast(1).Select(statement => statement.ToFullString())) + "            }";
        File.WriteAllText(actor, original.Insert(dispatch.Span.End, guard).Remove(phase.FullSpan.Start, phase.FullSpan.Length));

        EVContainer variant = EventProjectCompiler.Compile(flat);
        string project = Export(variant, "EVT_STAGE_GUARD_VARIANT", "guard", allowNative: true);
        DirectorScene scene = FindDirector(project);
        Assert.Contains("x: -156, z: -8582", GuardedPhaseMember(project, scene, 8, 8).ToFullString());
        Assert.DoesNotContain("x: -156, z: -8582", Method(Participant(project, 8), "OnLoop").ToFullString());
        Assert.Equal(variant.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        AssertEquivalent(Trace(flat), Trace(project));
    }

    [Fact]
    public void TwoSeparateDispatchArmsForOneRoleAndPhaseFailExplicitly()
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string actor = Participant(flat, 8);
        const string selector = "switch (Location.Byte_24)";
        Edit(actor, Method(actor, "OnLoop"), selector, """
            if (Location.Byte_24 == 8)
            {
                Actions.TurnInstant(angle: 41);
            }
            """ + Environment.NewLine + selector);

        EVContainer variant = EventProjectCompiler.Compile(flat);
        const string name = "EVT_STAGE_SPLIT_ROLE";
        InvalidDataException error = Assert.Throws<InvalidDataException>(() => Export(variant, name, "overlap"));
        Assert.Contains(name, error.Message);
        Assert.Contains("slot 8, tag 1, phase 8", error.Message);
        Assert.Contains("overlapping role arms", error.Message);
        Assert.False(Directory.Exists(Path.Combine(_directory, "overlap")));
    }

    [Fact]
    public void ASelectorCheckInsideAnOwnedRoleRemainsPartOfThatRole()
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        SwitchSectionSyntax phase = Assert.Single(PhaseSwitch(Participant(flat, 13)).Sections.Where(section => PhaseNumber(section) == 8));
        Edit(Participant(flat, 13), phase, "await Engine.Wait(frames: 30);", """
            if (Location.Byte_24 == 8)
            {
                await Engine.Wait(frames: 30);
            }
            """);

        EVContainer variant = EventProjectCompiler.Compile(flat);
        string project = Export(variant, "EVT_STAGE_OWNED_GUARD", "guarded");
        DirectorScene scene = FindDirector(project);
        Assert.Contains("await Engine.Wait(frames: 30);", PhaseMember(project, scene, 13, 8).ToFullString());
        Assert.Equal(variant.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Fact]
    public void AnIfOnlyRoleThatSignalsTheDirectorIsEditableInTheScene()
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string thread = Participant(flat, 2);
        Edit(thread, Method(thread, "OnLoop"), "for (; Location.Int16_32 == 1;)", """
            if (Location.Byte_24 == 8)
            {
                Location.Bit_231 = true;
            }
            for (; Location.Int16_32 == 1;)
            """);

        EVContainer variant = EventProjectCompiler.Compile(flat);
        string project = Export(variant, "EVT_STAGE_NEW_SIGNALER", "signaler");
        DirectorScene scene = FindDirector(project);
        Assert.Contains("AdvanceCutscenePhaseRequested = true;", GuardedPhaseMember(project, scene, 2, 8).ToFullString());
        Assert.Equal(variant.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Fact]
    public void TheSecondStageMapOwnsTheSoldiersGuardedDeparture()
    {
        const string secondStage = "EVT_ALEX1_TS_STAGE_1B";
        EVContainer original = Fixture(secondStage);
        string project = Export(original, secondStage, "second-stage");
        DirectorScene scene = FindDirector(project);
        string depart = GuardedPhaseMember(project, scene, 8, 1).ToFullString();
        Assert.Contains("await participant.Actions.WalkTo(x: -1848, z: -8200);", depart);
        Assert.Contains("Scripts.DeleteObject(uid: 255);", depart);
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Fact]
    public void CargoKeepsTheIndependentCandleHintInItsOwnLoop()
    {
        const string cargo = "EVT_ALEX1_TS_CARGO_0";
        EVContainer original = Fixture(cargo);
        string project = Export(original, cargo, "cargo", allowNative: true);
        Assert.Contains("messageId: 37)", Method(Participant(project, 12), "OnLoop").ToFullString());
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Fact]
    public void AComputedPhaseTransitionCannotSilentlySplitADirector()
    {
        string flat = Export(Fixture(Stage), Stage, "flat", extract: false);
        string director = Participant(flat, 4);
        Edit(director, PhaseSwitch(director).Sections.Single(section => PhaseNumber(section) == 6), "= 7;", "= Globals.ScenarioCounter + 7;");

        EVContainer variant = EventProjectCompiler.Compile(flat);
        const string name = "EVT_STAGE_COMPUTED_TRANSITION";
        InvalidDataException error = Assert.Throws<InvalidDataException>(() => Export(variant, name, "computed"));
        Assert.Contains(name, error.Message);
        Assert.Contains("slot 4, tag 1, phase 6", error.Message);
        Assert.Contains("without a resolved destination", error.Message);
        Assert.False(Directory.Exists(Path.Combine(_directory, "computed")));
    }

    [Fact]
    public void KitchenKeepsTheWindowHandoffInItsExistingActorLoops()
    {
        EVContainer original = Fixture(Kitchen);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, Kitchen, "kitchen"));
        Assert.DoesNotContain(Invocations(project), call => Member(call) == "StartDirector");
        string fishing = Assert.Single(Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs")
            .Where(path => File.ReadAllText(path).Contains("messageId: 298)", StringComparison.Ordinal)));
        MethodDeclarationSyntax[] methods = Parse(fishing).DescendantNodes().OfType<MethodDeclarationSyntax>().ToArray();
        MethodDeclarationSyntax announce = Assert.Single(methods.Where(method => method.ToFullString().Contains("messageId: 298)", StringComparison.Ordinal)));
        MethodDeclarationSyntax react = Assert.Single(methods.Where(method => method.ToFullString().Contains("await participant.Actions.WaitForTurn();", StringComparison.Ordinal)));
        Assert.Contains("await Messages.Show(windowId: 7, flags: 128, messageId: 298);", announce.ToFullString());
        Assert.Contains("await Engine.Wait(frames: 1);", react.ToFullString());
        Assert.Contains(Invocations(Participant(project, 7)), call => Member(call) == announce.Identifier.ValueText && call.Parent is AwaitExpressionSyntax);
        Assert.Contains(Invocations(Participant(project, 36)), call => Member(call) == react.Identifier.ValueText && call.Parent is AwaitExpressionSyntax);
        foreach (int slot in new[] { 7, 36 })
            Assert.Contains("await Engine.Wait(frames: 1);", Method(Participant(project, slot), "OnLoop").ToFullString());
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    private void AssertStageTrace(StageTrace trace)
    {
        Assert.Equal(new[] { 0, 53, 114, 159, 172, 253, 324, 450, 662, 824 }, trace.Transitions.Select(transition => transition.Frame));
        Assert.Equal(Enumerable.Range(0, 10).Select(phase => (byte)phase), trace.Transitions.Select(transition => transition.Phase));
        output.WriteLine("Stage transitions " + string.Join(", ", trace.Transitions.Select(transition => $"{transition.Phase}@{transition.Frame}")));
        int phaseThree = trace.Transitions.Single(transition => transition.Phase == 3).Frame;
        int phaseFour = trace.Transitions.Single(transition => transition.Phase == 4).Frame;
        Assert.Contains(trace.Barrier, sample => sample.Frame > phaseThree && sample.Frame < phaseFour && sample.Count > 0);
        Assert.Equal(0, trace.Barrier.Last(sample => sample.Frame < phaseFour).Count);

        int phaseEight = trace.Transitions.Single(transition => transition.Phase == 8).Frame;
        int phaseNine = trace.Transitions.Single(transition => transition.Phase == 9).Frame;
        Assert.Equal(new[] { 8, 9, 10, 13, 19 }, trace.Commands
            .Where(command => command.Frame == phaseEight && command.Operation == Opcode.CHRSCALE)
            .Select(command => command.Owner).Order());
        Assert.Contains(trace.Commands, command => command.Frame > phaseEight && command.Frame < phaseNine && command.Owner == 19 && command.Operation == Opcode.FLDSND1);
        Assert.Equal(new[] { false, false, true }, trace.LiveParticipants);
        Command encounter = Assert.Single(trace.Commands.Where(command => command.Operation == Opcode.ENCOUNT));
        Command field = Assert.Single(trace.Commands.Where(command => command.Operation == Opcode.MAPJUMP));
        Assert.Equal((phaseNine + 6, 4, "0,338"), (encounter.Frame, encounter.Owner, encounter.Arguments));
        Assert.Equal((encounter.Frame, 4), (field.Frame, field.Owner));
        output.WriteLine($"Phase 3 barrier cleared by frame {phaseFour}. Phase 8 starts five placements on frame {phaseEight}. Battle and field changes arrive from slot 4 on frame {encounter.Frame}.");
    }

    private static void AssertCast(string project, DirectorScene scene, IEnumerable<int> phaseOrder)
    {
        var rows = StageRows(scene).ToArray();
        Assert.Equal(phaseOrder, rows.Select(RowNumber));
        Assert.All(rows, row => Assert.IsType<AwaitExpressionSyntax>(row.Parent));
        Assert.DoesNotContain(scene.Method.DescendantNodes(), node => node is SwitchStatementSyntax or WhileStatementSyntax);
        Assert.Equal(new[] { 2, 3, 7 }, rows.Where(row => row.ArgumentList.Arguments.Any(argument =>
            argument.Expression is InvocationExpressionSyntax call && Member(call) == "Barrier")).Select(RowNumber).Order());
        Assert.Equal("Until", Member(rows.Single(row => RowNumber(row) == 8)));
        Assert.Equal("Finish", Member(rows.Last()));
        Assert.Equal(27, rows.SelectMany(row => row.DescendantNodes().OfType<InvocationExpressionSyntax>()).Count(call => Member(call) == "Role"));
        Assert.Empty(Directory.GetFiles(project, "04_*.cs"));
        var owners = new Dictionary<(int Phase, int Slot), string>();
        foreach ((int slot, int[] phases) in Cast)
        {
            var loop = Method(Participant(project, slot), "OnLoop");
            Assert.Single(loop.Body!.Statements);
            Assert.Equal("Perform", Member(Assert.Single(loop.DescendantNodes().OfType<InvocationExpressionSyntax>())));
            foreach (int phase in phases)
            {
                MethodDeclarationSyntax member = PhaseMember(project, scene, slot, phase);
                owners.Add((phase, slot), member.Identifier.ValueText);
                Assert.DoesNotContain("AdvanceCutscenePhaseRequested", member.ToFullString());
                Assert.DoesNotContain("Location.Byte_26", member.ToFullString());
                if (phase == 8 && slot is 18 or 20)
                    Assert.Contains("Scripts.DeleteObject(uid: 255);", member.ToFullString());
                if (phase == 8 && slot is 8 or 9 or 10 or 19)
                {
                    Assert.Contains("await Engine.Wait(frames: 1);", member.ToFullString());
                    Assert.Contains(member.DescendantNodes(), node => node is ForStatementSyntax { Condition: LiteralExpressionSyntax condition } &&
                        condition.IsKind(SyntaxKind.TrueLiteralExpression) || node is WhileStatementSyntax { Condition: LiteralExpressionSyntax test } &&
                        test.IsKind(SyntaxKind.TrueLiteralExpression));
                }
            }
        }
        Assert.Equal(37, owners.Count + 10);
        var declarationOrder = Parse(scene.Path).DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Select(method => method.Identifier.ValueText).Where(owners.Values.Contains);
        Assert.Equal(phaseOrder.SelectMany(phase => owners.Where(owner => owner.Key.Phase == phase)
            .OrderBy(owner => owner.Key.Slot).Select(owner => owner.Value)), declarationOrder);
    }

    private static StageTrace Trace(string project, int phaseAddress = 24)
        => Trace(Assembly.Load(EventProjectCompiler.CompileManaged(project)), phaseAddress);

    private static StageTrace Trace(Assembly assembly, int phaseAddress = 24, Action<byte[]>? beforeTick = null)
    {
        var host = new StageHost();
        var location = new byte[80];
        var scheduler = new EventScheduler(host, new byte[4096], location);
        scheduler.RegisterProject(assembly);
        scheduler.StartScene();
        var transitions = new List<(int Frame, byte Phase)> { (0, 0) };
        var barrier = new List<(int Frame, byte Count)>();
        for (int tick = 0; tick < 2000 && !host.Commands.Any(command => command.Operation == Opcode.MAPJUMP); tick++)
        {
            host.Frame = tick;
            beforeTick?.Invoke(location);
            scheduler.Tick();
            byte phase = location[phaseAddress];
            if (phase != transitions[^1].Phase)
                transitions.Add((tick, phase));
            if (phase == 3)
                barrier.Add((tick, location[26]));
        }
        bool[] liveParticipants = [scheduler.TryTrigger(18, 0, 1), scheduler.TryTrigger(20, 0, 1), scheduler.TryTrigger(8, 0, 1)];
        return new StageTrace(transitions, barrier, host.Commands, liveParticipants);
    }

    private static void AssertEquivalent(StageTrace expected, StageTrace actual)
    {
        Assert.Equal(expected.Transitions, actual.Transitions);
        Assert.Equal(expected.Barrier, actual.Barrier);
        Assert.Equal(expected.Commands, actual.Commands);
        Assert.Equal(expected.LiveParticipants, actual.LiveParticipants);
    }

    private string Export(EVContainer field, string name, string leaf, bool extract = true, bool allowNative = false)
    {
        string project = Path.Combine(_directory, leaf);
        EventProjectExporter.Export(field, name, project, useCutsceneNames: false, extractCutscenes: extract, allowNativePhaseDispatch: allowNative);
        return project;
    }

    private static EVContainer Fixture(string name)
    {
        using Stream input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
        return EVContainer.Read(input);
    }

    private static DirectorScene FindDirector(string project)
    {
        InvocationExpressionSyntax start = Assert.Single(Invocations(project).Where(call => Member(call) == "StartDirector"));
        Assert.Equal("OnInit", Assert.Single(start.Ancestors().OfType<MethodDeclarationSyntax>()).Identifier.ValueText);
        var destination = Assert.IsType<MemberAccessExpressionSyntax>(Assert.Single(start.ArgumentList.Arguments).Expression);
        string methodName = destination.Name.Identifier.ValueText;
        string path = Assert.Single(Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs")
            .Where(path => Parse(path).DescendantNodes().OfType<MethodDeclarationSyntax>().Any(method => method.Identifier.ValueText == methodName)));
        return new DirectorScene(path, Method(path, methodName));
    }

    private static MethodDeclarationSyntax PhaseMember(string project, DirectorScene scene, int slot, int phase)
    {
        if (StageRows(scene).SingleOrDefault(row => RowNumber(row) == phase) is { } row)
        {
            string participant = Parse(Participant(project, slot)).DescendantNodes().OfType<ClassDeclarationSyntax>().First().Identifier.ValueText;
            var role = Assert.Single(row.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "Role" &&
                call.ArgumentList.Arguments[0].Expression is MemberAccessExpressionSyntax receiver && receiver.Name.Identifier.ValueText == participant));
            return Method(scene.Path, ((IdentifierNameSyntax)role.ArgumentList.Arguments[1].Expression).Identifier.ValueText);
        }
        SwitchSectionSyntax section = Assert.Single(PhaseSwitch(Participant(project, slot)).Sections.Where(section => PhaseNumber(section) == phase));
        var members = Parse(scene.Path).DescendantNodes().OfType<MethodDeclarationSyntax>().ToDictionary(method => method.Identifier.ValueText);
        var call = Assert.Single(section.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => members.ContainsKey(Member(call))));
        Assert.IsType<AwaitExpressionSyntax>(call.Parent);
        return members[Member(call)];
    }

    private static IEnumerable<InvocationExpressionSyntax> StageRows(DirectorScene scene) => scene.Method.DescendantNodes().OfType<InvocationExpressionSyntax>()
        .Where(call => Member(call) is "Together" or "Until" or "RunUntilCue" or "Finish");

    private static int RowNumber(InvocationExpressionSyntax row) => (int)((LiteralExpressionSyntax)row.ArgumentList.Arguments[0].Expression).Token.Value!;

    private static MethodDeclarationSyntax FinishMethod(DirectorScene scene)
    {
        var finish = Assert.Single(StageRows(scene).Where(call => Member(call) == "Finish"));
        return Method(scene.Path, ((IdentifierNameSyntax)finish.ArgumentList.Arguments[1].Expression).Identifier.ValueText);
    }

    private static MethodDeclarationSyntax GuardedPhaseMember(string project, DirectorScene scene, int slot, int phase)
    {
        string path = Participant(project, slot);
        MethodDeclarationSyntax handler = Method(path, "OnLoop");
        if (handler.DescendantNodes().OfType<InvocationExpressionSyntax>().Any(call => Member(call) == "PerformPhase"))
        {
            string participant = Parse(path).DescendantNodes().OfType<ClassDeclarationSyntax>().First().Identifier.ValueText;
            var row = Assert.Single(Invocations(scene.Path).Where(call => Member(call) is "Together" or "Until" or "RunUntilCue" or "Idle" &&
                RowNumber(call) == phase));
            var role = Assert.Single(row.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => Member(call) == "Role" &&
                call.ArgumentList.Arguments[0].Expression is MemberAccessExpressionSyntax receiver && receiver.Name.Identifier.ValueText == participant));
            return Method(scene.Path, ((IdentifierNameSyntax)role.ArgumentList.Arguments[1].Expression).Identifier.ValueText);
        }
        var members = Parse(scene.Path).DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(method => method.Identifier.ValueText.StartsWith($"Phase{phase}_", StringComparison.Ordinal))
            .ToDictionary(method => method.Identifier.ValueText);
        var call = Assert.Single(handler.DescendantNodes().OfType<InvocationExpressionSyntax>()
            .Where(call => members.ContainsKey(Member(call)) && call.Ancestors().OfType<IfStatementSyntax>().Any()));
        return members[Member(call)];
    }

    private static IEnumerable<InvocationExpressionSyntax> Invocations(string path)
    {
        if (File.Exists(path))
            return Parse(path).DescendantNodes().OfType<InvocationExpressionSyntax>();
        return Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories).SelectMany(Invocations);
    }

    private static string Member(InvocationExpressionSyntax call) => call.Expression is MemberAccessExpressionSyntax member ? member.Name.Identifier.ValueText : call.Expression.ToString();

    private static int PhaseNumber(SwitchSectionSyntax section) => section.Labels.OfType<CaseSwitchLabelSyntax>()
        .Select(label => (int)((LiteralExpressionSyntax)label.Value).Token.Value!).DefaultIfEmpty(-1).Single();

    private static SwitchStatementSyntax PhaseSwitch(string path) => Assert.Single(Method(path, "OnLoop").DescendantNodes().OfType<SwitchStatementSyntax>());

    private static MethodDeclarationSyntax Method(string path, string name) => Assert.Single(Parse(path).DescendantNodes().OfType<MethodDeclarationSyntax>()
        .Where(method => method.Identifier.ValueText == name));

    private static CompilationUnitSyntax Parse(string path) => CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetCompilationUnitRoot();

    private static string Participant(string project, int slot) => Assert.Single(Directory.GetFiles(project, $"{slot:D2}_*.cs"));

    private static void Edit(string path, SyntaxNode node, string before, string after)
    {
        string source = File.ReadAllText(path);
        string body = node.ToFullString();
        Assert.Equal(2, body.Split(before, StringSplitOptions.None).Length);
        File.WriteAllText(path, source.Remove(node.FullSpan.Start, node.FullSpan.Length)
            .Insert(node.FullSpan.Start, body.Replace(before, after, StringComparison.Ordinal)));
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }

    [Theory]
    [InlineData("cycle", "must not form a cycle")]
    [InlineData("duplicate phase", "Declare each choreography phase once")]
    [InlineData("wrong receiver", "Use an ordinary async cutscene method group")]
    [InlineData("direct call", "Reference a choreography route only through Routes or ContinueWith")]
    public void InvalidRouteDeclarationsAreRejectedByBothCompilers(string edit, string diagnostic)
    {
        string project = Export(Fixture(Stage), Stage, "invalid-route");
        DirectorScene scene = FindDirector(project);
        var owner = Assert.IsType<ClassDeclarationSyntax>(scene.Method.Parent);
        var setup = Assert.Single(scene.Method.Body!.Statements.OfType<LocalDeclarationStatementSyntax>()
            .Where(statement => statement.Declaration.Variables.Any(variable => variable.Identifier.ValueText == "play")));
        string firstEntry = edit == "wrong receiver" ? $"new {owner.Identifier}(ctx).RouteOne" : "RouteOne";
        var director = scene.Method.WithBody(SyntaxFactory.Block(setup, SyntaxFactory.ParseStatement($"await play.Routes({firstEntry}, RouteTwo);")));
        string secondBody = edit switch
        {
            "cycle" => "await play.RunUntilCue(1); await play.ContinueWith(RouteOne);",
            "duplicate phase" => "await play.Idle(0);",
            _ => "await play.Idle(1);"
        };
        var routes = new List<MemberDeclarationSyntax>
        {
            SyntaxFactory.ParseMemberDeclaration("private async EventRoutine RouteOne(SceneChoreography play) { await play.RunUntilCue(0); await play.ContinueWith(RouteTwo); }")!,
            SyntaxFactory.ParseMemberDeclaration($"private async EventRoutine RouteTwo(SceneChoreography play) {{ {secondBody} }}")!
        };
        if (edit == "direct call")
            routes.Add(SyntaxFactory.ParseMemberDeclaration("private EventRoutine Misuse(SceneChoreography play) => RouteOne(play);")!);
        var modified = owner.ReplaceNode(scene.Method, director).AddMembers(routes.ToArray());
        File.WriteAllText(scene.Path, owner.SyntaxTree.GetRoot().ReplaceNode(owner, modified).NormalizeWhitespace().ToFullString());

        Assert.Contains(diagnostic, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project)).Message);
        Assert.Contains(diagnostic, Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project)).Message);
    }

    private sealed record DirectorScene(string Path, MethodDeclarationSyntax Method);
    private sealed record Command(int Frame, int Owner, Opcode Operation, string Arguments);
    private sealed record StageTrace(List<(int Frame, byte Phase)> Transitions, List<(int Frame, byte Count)> Barrier, List<Command> Commands, bool[] LiveParticipants);

    private sealed class StageHost : IEventHost
    {
        public int Frame { get; set; }
        public List<Command> Commands { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Commands.Add(new Command(Frame, objectId, opcode, string.Join(",", operands.ToArray().Select(operand => (int)operand))));
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }
}
