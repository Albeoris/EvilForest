using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class TailLoopEditTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-tail-loop-").FullName;

    [Fact]
    public void EditsBeforeTheBackedgeRunAfterEitherBranch()
    {
        EVInstruction[] instructions =
        [
            new(0, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 0)] },
            new(1, Jsm.Opcode.JMP_IFN) { Targets = [4] },
            Marker(2, 1),
            new(3, Jsm.Opcode.JMP) { Targets = [5] },
            Marker(4, 2),
            new(5, Jsm.Opcode.JMP) { Targets = [0] }
        ];
        EVContainer original = Container(instructions);
        string project = Export(original, "source");
        string file = Directory.GetFiles(project, "00_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();
        BlockSyntax loop = Assert.IsType<BlockSyntax>(Assert.Single(syntax.DescendantNodes().OfType<WhileStatementSyntax>()).Statement);
        Assert.NotNull(Assert.IsType<IfStatementSyntax>(Assert.Single(loop.Statements)).Else);
        Assert.Empty(syntax.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        StatementSyntax edit = SyntaxFactory.ParseStatement("Messages.SetTextVariable(variableId: 0, value: 99);");
        SyntaxNode editedSource = syntax.ReplaceNode(loop, loop.AddStatements(edit, SyntaxFactory.ParseStatement("return;")));
        File.WriteAllText(file, editedSource.NormalizeWhitespace().ToFullString());
        EVInstruction[] body = EVInstructionBody.Decode(EventProjectCompiler.Compile(project).Objects[0].Scripts.Single().Body).Instructions.ToArray();
        Assert.Equal(99, body[5].Operands[1].Value);
        Assert.Equal(body[5].Label, Assert.Single(body[3].Targets));
        Assert.Equal(body[0].Label, Assert.Single(body[^1].Targets));
        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Run(assembly, 0, 0, [2, 99]);
        Run(assembly, 1, 0, [1, 99]);
    }

    [Fact]
    public void AdjacentJumpsKeepTheirDistinctEntryPointsAfterEditing()
    {
        EVInstruction[] instructions =
        [
            new(0, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 0)] },
            new(1, Jsm.Opcode.JMP_IFN) { Targets = [5] },
            new(2, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 1)] },
            new(3, Jsm.Opcode.JMP_IF) { Targets = [6] },
            Marker(4, 1),
            new(5, Jsm.Opcode.JMP) { Targets = [6] },
            new(6, Jsm.Opcode.JMP) { Targets = [7] },
            Marker(7, 2),
            new(8, Jsm.Opcode.Return)
        ];
        EVContainer original = Container(instructions);
        string project = Export(original, "source");
        string file = Directory.GetFiles(project, "00_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot().NormalizeWhitespace();
        EmptyStatementSyntax[] markers = syntax.DescendantNodes().OfType<EmptyStatementSyntax>()
            .Where(statement => statement.Parent is BlockSyntax or SwitchSectionSyntax).ToArray();
        Assert.Equal(2, markers.Length);
        Assert.Contains("PreserveEmptyJumps", syntax.ToFullString());
        File.WriteAllText(file, syntax.ToFullString());
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, edited: false);

        var block = Assert.IsType<BlockSyntax>(markers[0].Parent);
        StatementSyntax edit = SyntaxFactory.ParseStatement("Messages.SetTextVariable(variableId: 0, value: 99);");
        SyntaxNode editedSource = syntax.ReplaceNode(block, block.WithStatements(block.Statements.Insert(block.Statements.IndexOf(markers[0]) + 1, edit)));
        File.WriteAllText(file, editedSource.NormalizeWhitespace().ToFullString());
        EVInstruction[] body = EVInstructionBody.Decode(EventProjectCompiler.Compile(project).Objects[0].Scripts.Single().Body).Instructions.ToArray();
        Assert.Equal(body[5].Label, Assert.Single(body[1].Targets));
        Assert.Equal(body[7].Label, Assert.Single(body[3].Targets));
        Assert.Equal(body[6].Label, Assert.Single(body[5].Targets));
        Assert.Equal(body[8].Label, Assert.Single(body[7].Targets));
        ExpectPaths(project, edited: true);

        AttributeSyntax hint = editedSource.DescendantNodes().OfType<AttributeSyntax>().Single(attribute => attribute.Name.ToString() == "PreserveEmptyJumps");
        SyntaxNode noHint = editedSource.RemoveNode(hint.Parent!, SyntaxRemoveOptions.KeepNoTrivia)!;
        File.WriteAllText(file, noHint.NormalizeWhitespace().ToFullString());
        EVInstruction[] withoutJumps = EVInstructionBody.Decode(EventProjectCompiler.Compile(project).Objects[0].Scripts.Single().Body).Instructions.ToArray();
        Assert.Equal(body.Length - 2, withoutJumps.Length);
        ExpectPaths(project, edited: true);

        void ExpectPaths(string path, bool edited)
        {
            Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(path));
            Run(assembly, 0, 0, edited ? [99, 2] : [2]);
            Run(assembly, 1, 0, edited ? [1, 99, 2] : [1, 2]);
            Run(assembly, 1, 1, [2]);
        }
    }

    [Fact]
    public void EditingLoopWorkPreservesBothRoutesToTheTailCondition()
    {
        EVInstruction[] instructions =
        [
            new(0, Jsm.Opcode.JMP) { Targets = [9] },
            Marker(1, 1),
            new(2, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 0), new(0x7D, 0), new(0x2C, 0)] },
            new(3, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 1)] },
            new(4, Jsm.Opcode.JMP_IFN) { Targets = [7] },
            new(5, Jsm.Opcode.JMP) { Targets = [9] },
            Marker(6, 100),
            Marker(7, 2),
            Marker(8, 3),
            new(9, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 0)] },
            new(10, Jsm.Opcode.JMP_IF) { Targets = [1] },
            Marker(11, 4),
            new(12, Jsm.Opcode.Return)
        ];
        EVContainer original = Container(instructions);
        string project = Export(original, "source");
        string file = Directory.GetFiles(project, "00_*.cs").Single();
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();
        BlockSyntax loop = Assert.IsType<BlockSyntax>(Assert.Single(syntax.DescendantNodes().OfType<ForStatementSyntax>()).Statement);
        Assert.Single(loop.DescendantNodes().OfType<ContinueStatementSyntax>());
        Assert.Empty(syntax.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, edited: false);

        StatementSyntax marker = SyntaxFactory.ParseStatement("Messages.SetTextVariable(variableId: 0, value: 99);");
        syntax = syntax.ReplaceNode(loop, loop.AddStatements(marker));
        File.WriteAllText(file, syntax.NormalizeWhitespace().ToFullString());
        EVContainer edited = EventProjectCompiler.Compile(project);
        EVInstruction[] editedBody = EVInstructionBody.Decode(edited.Objects[0].Scripts.Single().Body).Instructions.ToArray();
        EVInstruction test = editedBody[^4];
        Assert.Equal(Jsm.Opcode.EXPR, test.Opcode);
        Assert.Equal(Jsm.Opcode.JMP_IF, editedBody[^3].Opcode);
        Assert.Equal(99, editedBody[^5].Operands[1].Value);
        Assert.Equal(test.Label, Assert.Single(editedBody[0].Targets));
        Assert.Equal(test.Label, Assert.Single(editedBody[5].Targets));
        Assert.Equal(editedBody[1].Label, Assert.Single(editedBody[^3].Targets));
        ExpectPaths(project, edited: true);

        string reopened = Export(edited, "reopened");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        ExpectPaths(reopened, edited: true);

    }

    [Fact]
    public void SharedCaseCallsResumeOnTheOriginalFrameFromEveryEntry()
    {
        EVInstruction[] instructions =
        [
            new(0, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 0)] },
            new(1, Jsm.Opcode.JMP_SWITCH) { CaseValues = [0], Targets = [9, 2, 4, 6] },
            Marker(2, 10),
            new(3, Jsm.Opcode.WAIT) { Operands = [new(2)] },
            Marker(4, 20),
            new(5, Jsm.Opcode.WAIT) { Operands = [new(1)] },
            Marker(6, 30),
            new(7, Jsm.Opcode.WAIT) { Operands = [new(1)] },
            new(8, Jsm.Opcode.JMP) { Targets = [10] },
            Marker(9, 90),
            Marker(10, 99),
            new(11, Jsm.Opcode.Return)
        ];
        EVContainer fixture = Container(instructions);
        string project = Export(fixture, "shared-case-timing");
        SyntaxNode syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(Directory.GetFiles(project, "00_*.cs").Single())).GetRoot();
        Assert.NotEmpty(syntax.DescendantNodes().OfType<LocalFunctionStatementSyntax>());
        Assert.Empty(syntax.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(fixture.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        CheckFrames(0, [[10], [], [20], [30], [99]]);
        CheckFrames(1, [[20], [30], [99]]);
        CheckFrames(2, [[30], [99]]);
        CheckFrames(3, [[90, 99]]);

        void CheckFrames(byte selector, int[][] expected)
        {
            var location = new byte[80];
            location[0] = selector;
            var host = new RecordingHost();
            var scheduler = new EventScheduler(host, new byte[4096], location);
            scheduler.RegisterProject(assembly);
            scheduler.StartScene();
            foreach (int[] frame in expected)
            {
                host.Values.Clear();
                scheduler.Tick();
                Assert.Equal(frame, host.Values);
            }
        }
    }

    private static EVInstruction Marker(int label, int value) => new(label, Jsm.Opcode.MESVALUE) { Operands = [new(0), new(value)] };

    private static EVContainer Container(EVInstruction[] instructions)
    {
        byte[] body = new EVInstructionBody(instructions, instructions.Length).Encode();
        var root = new EVContainerObject(0, 0, 0, 0)
        {
            BodyOrder = 0,
            Type = (byte)ParticipantKind.Thread,
            Scripts = [new EVContainerScript(0, body)]
        };
        var header = new byte[128];
        new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
        return new EVContainer(header, [root], []);
    }

    private string Export(EVContainer container, string name)
    {
        string directory = Path.Combine(_directory, name);
        EventProjectExporter.Export(container, "TailLoopFixture", directory, useCutsceneNames: false, extractCutscenes: false);
        return SourceProjectTest.CopyWithoutComments(directory);
    }

    private static void ExpectPaths(string project, bool edited)
    {
        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Run(assembly, 0, 0, [4]);
        Run(assembly, 1, 0, edited ? [1, 2, 3, 99, 4] : [1, 2, 3, 4]);
        Run(assembly, 1, 1, [1, 4]);
    }

    private static void Run(Assembly assembly, int condition, int skipRest, int[] expected)
    {
        var location = new byte[80];
        location[0] = (byte)condition;
        location[1] = (byte)skipRest;
        var host = new RecordingHost();
        var scheduler = new EventScheduler(host, new byte[4096], location);
        scheduler.RegisterProject(assembly);
        scheduler.StartScene();
        scheduler.Tick();
        Assert.Equal(expected, host.Values);
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);

    private sealed class RecordingHost : IEventHost
    {
        public List<int> Values { get; } = new();

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            Assert.Equal(Opcode.MESVALUE, opcode);
            Values.Add((int)operands[1]);
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands) => throw new InvalidOperationException();
        public bool IsWindowOpen(int objectId, int windowId) => throw new InvalidOperationException();
    }
}
