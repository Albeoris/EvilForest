using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class ChoreographyRoleExitTests : IDisposable
{
    private const string Town = "EVT_CLEYRA3_TOWN";
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-role-exit-{Guid.NewGuid():N}");

    [Fact]
    public void AnEarlyRoleReturnReachesTheSharedPollAfterEditingTheRole()
    {
        var (original, project) = Export("EVT_GIZ_TO_WORLD");
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        string path = Path.Combine(project, "Cutscenes", "Scene_01_01.cs");
        string source = File.ReadAllText(path);
        var role = Assert.Single(CSharpSyntaxTree.ParseText(source).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(method => method.Identifier.ValueText.StartsWith("Phase0_") && method.ParameterList.Parameters.Any(parameter => parameter.Type?.ToString() == "GEO_MAIN_F0_ZDN_14")));
        var early = Assert.Single(role.Body!.DescendantNodes().OfType<ReturnStatementSyntax>().Where(exit => exit.Parent != role.Body));
        File.WriteAllText(path, source.Insert(early.SpanStart, "await Engine.Wait(frames: 217);\n            "));

        EVContainer edited = EventProjectCompiler.Compile(project);
        var code = EVInstructionBody.Decode(edited.Objects[14].Scripts.Single(script => script.Id == 1).Body).Instructions;
        EVInstruction inserted = Assert.Single(code.Where(step => step.Opcode == Jsm.Opcode.WAIT && step.Operands[0].Value == 217));
        EVInstruction exit = code[code.IndexOf(inserted) + 1];
        Assert.Equal(Jsm.Opcode.JMP, exit.Opcode);
        EVInstruction poll = Assert.Single(code.Where(step => step.Label == Assert.Single(exit.Targets)));
        Assert.Equal(Jsm.Opcode.WAIT, poll.Opcode);
        Assert.Equal(1, poll.Operands[0].Value);
        Assert.Equal(Jsm.Opcode.JMP, code[code.IndexOf(poll) + 1].Opcode);
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData(Town, 5)]
    [InlineData("EVT_CLEYRA3_VISTA", 15)]
    [InlineData("EVT_CLEYRA3_WIND", 4)]
    public void StrictChoreographyPreservesTheNativeSelfDeleteLayout(string field, int actorSlot)
    {
        var (original, project) = Export(field);
        Assert.Contains("Perform(this)", File.ReadAllText(ActorSource(project, actorSlot)));
        FindDeleteAction(project, actorSlot);
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData("Scripts.DeleteObject(uid: 255);", true, false)]
    [InlineData("Scripts.DeleteObject(uid: 101);", false, true)]
    [InlineData("await Engine.Wait(frames: 1);", false, false)]
    [InlineData("if (Globals.ScenarioCounter == 1) { Scripts.DeleteObject(uid: 255); }", false, false)]
    public void OnlyAnUnconditionalSelfDeleteUsesTheOriginalPhysicalCaseBoundary(string ending, bool deletesActor, bool deletesOther)
    {
        var (_, project) = Export(Town);
        var (path, action) = FindDeleteAction(project, 5);
        StatementSyntax last = action.Body!.Statements.Last();
        Assert.Equal("Scripts.DeleteObject(uid: 255);", last.ToString());
        SyntaxNode root = action.SyntaxTree.GetRoot();
        File.WriteAllText(path, root.ReplaceNode(last, SyntaxFactory.ParseStatement(ending).WithTriviaFrom(last)).ToFullString());

        EVInstructionBody body = CompileActorLoop(project, 5);
        EVInstruction table = body.Instructions.Single(step => step.Opcode == Jsm.Opcode.JMP_SWITCHEX);
        int caseStart = CaseTarget(table, 13);
        int nextCase = CaseTarget(table, 15);
        EVInstruction[] phase = body.Instructions.Where(step => step.Label >= caseStart && step.Label < nextCase).ToArray();
        if (deletesActor)
        {
            Assert.Equal(Jsm.Opcode.DELETE, phase[^1].Opcode);
            Assert.Equal(255, Assert.Single(phase[^1].Operands).Value);
        }
        else
        {
            Assert.Equal(Jsm.Opcode.JMP, phase[^1].Opcode);
            Assert.Equal(table.Targets[0], Assert.Single(phase[^1].Targets));
            if (deletesOther)
                Assert.Equal(101, Assert.Single(phase[^2].Operands).Value);
            if (ending.StartsWith("if ", StringComparison.Ordinal))
                Assert.Contains(phase, step => step.Opcode == Jsm.Opcode.JMP_IFN && step.Targets.Contains(phase[^1].Label));
        }

        Assembly managed = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        var (scheduler, location, _) = StartActor(managed, 5, 13);
        Tick(scheduler, 15);
        Assert.Equal(3, location[26]);
        Assert.Equal(!deletesActor, scheduler.TryTrigger(100, 1, 6));
        Assert.Equal(!deletesOther, scheduler.TryTrigger(101, 1, 6));

        var (neighbor, neighborLocation, _) = StartActor(managed, 5, 15);
        Tick(neighbor, 3);
        Assert.Equal(5, neighborLocation[26]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void EditedSuspendingLoopResumesPollingBeforeRunningAnotherRole(bool keepsLooping)
    {
        var (_, project) = Export(Town);
        var (path, action) = FindHintedAction(project, 12, method => method.Body!.Statements.LastOrDefault() is ForStatementSyntax);
        var loop = Assert.IsType<ForStatementSyntax>(action.Body!.Statements.Last());
        var condition = SyntaxFactory.LiteralExpression(keepsLooping ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
        SyntaxNode root = action.SyntaxTree.GetRoot();
        File.WriteAllText(path, root.ReplaceNode(loop.Condition!, condition.WithTriviaFrom(loop.Condition!)).ToFullString());

        EVInstructionBody body = CompileActorLoop(project, 12);
        EVInstruction table = body.Instructions.Single(step => step.Opcode == Jsm.Opcode.JMP_SWITCHEX);
        int start = CaseTarget(table, 13);
        int adjacent = CaseTarget(table, 17);
        EVInstruction exit = body.Instructions.Last(step => step.Label < adjacent);
        if (keepsLooping)
        {
            Assert.Equal(Jsm.Opcode.JMP_IF, exit.Opcode);
            Assert.InRange(Assert.Single(exit.Targets), start, exit.Label - 1);
        }
        else
        {
            Assert.Equal(Jsm.Opcode.JMP, exit.Opcode);
            Assert.Equal(table.Targets[0], Assert.Single(exit.Targets));
        }

        Assembly managed = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        var (scheduler, location, host) = StartActor(managed, 12, 13);
        Tick(scheduler, 12);
        Assert.Contains((Opcode.MOVE, -798), host.Commands);
        Assert.DoesNotContain((Opcode.MOVE, -803), host.Commands);

        location[24] = 17;
        Tick(scheduler, 6);
        Assert.Equal(!keepsLooping, host.Commands.Contains((Opcode.MOVE, -803)));
    }

    private static void Tick(EventScheduler scheduler, int count)
    {
        for (int index = 0; index < count; index++)
            scheduler.Tick();
    }

    private static EVInstructionBody CompileActorLoop(string project, int slot)
    {
        EVContainer compiled = EventProjectCompiler.Compile(project);
        EVContainerScript script = compiled.Objects[slot].Scripts.Single(entry => entry.Id == 1);
        return EVInstructionBody.Decode(script.Body);
    }

    private static (EventScheduler Scheduler, byte[] Location, Host Host) StartActor(Assembly managed, int slot, byte phase)
    {
        var location = new byte[80];
        location[24] = phase;
        var host = new Host();
        var scheduler = new EventScheduler(host, new byte[4096], location);
        scheduler.RegisterProject(managed);
        scheduler.CreateParticipant(slot, 100);
        scheduler.AddObject(101, 0, new Dictionary<int, Func<EventRoutine>> { [1] = static () => EventRoutine.Run(static () => { }) });
        return (scheduler, location, host);
    }

    private static int CaseTarget(EVInstruction table, ushort phase)
    {
        int index = Array.IndexOf(table.CaseValues, phase);
        Assert.True(index >= 0);
        return table.Targets[index + 1];
    }

    private static string ActorSource(string project, int slot) => Assert.Single(Directory.GetFiles(project, $"{slot:D2}_*.cs"));

    private static (string Path, MethodDeclarationSyntax Method) FindDeleteAction(string project, int slot) =>
        FindHintedAction(project, slot, method => method.Body!.Statements.LastOrDefault()?.ToString() == "Scripts.DeleteObject(uid: 255);");

    private static (string Path, MethodDeclarationSyntax Method) FindHintedAction(string project, int slot, Func<MethodDeclarationSyntax, bool> matches)
    {
        string actor = Assert.Single(Parse(ActorSource(project, slot)).DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Where(type => type.AttributeLists.SelectMany(list => list.Attributes).Any(attribute => attribute.Name.ToString() == "ObjectSlot"))).Identifier.ValueText;
        return Assert.Single(Directory.GetFiles(Path.Combine(project, "Cutscenes"), "*.cs").SelectMany(path => Parse(path).DescendantNodes()
            .OfType<MethodDeclarationSyntax>().Where(method => method.ParameterList.Parameters is [var parameter] && parameter.Type?.ToString() == actor &&
                method.AttributeLists.SelectMany(list => list.Attributes).Any(attribute => attribute.Name.ToString() == "OmitUnreachablePollingExit") && matches(method))
            .Select(method => (path, method))));
    }

    private (EVContainer Original, string Project) Export(string field)
    {
        using Stream input = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", field + ".eb.bytes"));
        EVContainer original = EVContainer.Read(input);
        string project = Path.Combine(_directory, field);
        EventProjectExporter.Export(original, field, project, useCutsceneNames: false);
        return (original, SourceProjectTest.CopyWithoutComments(project));
    }

    private static SyntaxNode Parse(string path) => CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }

    private sealed class Host : IEventHost
    {
        public List<(Opcode Operation, int FirstOperand)> Commands { get; } = [];

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Commands.Add((opcode, operands.IsEmpty ? 0 : (int)operands[0]));
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => 0;

        public bool IsWindowOpen(int objectId, int windowId) => false;
    }
}
