using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class NestedSwitchExitTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-nested-switch-").FullName;

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void InsertingWorkBeforeTheOuterBreakRetargetsTheNativeInnerExits(bool inline, bool editHelper)
    {
        EVInstruction[] instructions =
        [
            new(0, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 0)] },
            new(1, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [1], Targets = [8, 2] },
            new(2, Jsm.Opcode.EXPR) { Expression = [new(0xD5, 1)] },
            new(3, Jsm.Opcode.JMP_SWITCHEX) { CaseValues = [42, 5], Targets = [8, 6, 4] },
            Marker(4, 1),
            new(5, Jsm.Opcode.JMP) { Targets = [8] },
            Marker(6, 2),
            new(7, Jsm.Opcode.JMP) { Targets = [8] },
            Marker(8, 3),
            new(9, Jsm.Opcode.Return)
        ];
        byte[] body = new EVInstructionBody(instructions, instructions.Length).Encode();
        var root = new EVContainerObject(0, 0, 0, 0)
        {
            BodyOrder = 0,
            Type = (byte)ParticipantKind.Thread,
            Scripts = [new EVContainerScript(0, body)]
        };
        var header = new byte[128];
        new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
        var original = new EVContainer(header, [root], []);
        string project = Export(original, "source");
        string file = Directory.GetFiles(project, "00_*.cs").Single();
        string script = File.ReadAllText(file);
        Assert.Contains("ThreadedExitSections = \"0\"", script);
        Assert.Contains("ThreadDefaultExit = true", script);
        Assert.DoesNotContain("goto ", script);

        SyntaxNode syntax = CSharpSyntaxTree.ParseText(script).GetRoot();
        SwitchSectionSyntax outer = OuterArm(syntax);
        if (inline)
        {
            SwitchStatementSyntax selection = Assert.IsType<SwitchStatementSyntax>(outer.Statements[0]);
            syntax = syntax.ReplaceNode(selection, SyntaxFactory.ParseStatement("DispatchInner();"));
            ClassDeclarationSyntax participant = OuterArm(syntax).FirstAncestorOrSelf<ClassDeclarationSyntax>()!;
            MemberDeclarationSyntax helper = SyntaxFactory.ParseMemberDeclaration($"private static void DispatchInner() {{ {selection} }}")!;
            syntax = syntax.ReplaceNode(participant, participant.AddMembers(helper));
            File.WriteAllText(file, syntax.NormalizeWhitespace().ToFullString());
        }
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        ExpectPaths(project, original, edited: false);

        StatementSyntax marker = SyntaxFactory.ParseStatement("Messages.SetTextVariable(variableId: 0, value: 99);");
        if (editHelper)
        {
            BlockSyntax helper = syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "DispatchInner").Body!;
            syntax = syntax.ReplaceNode(helper, helper.AddStatements(marker));
        }
        else
        {
            outer = OuterArm(syntax);
            syntax = syntax.ReplaceNode(outer, outer.WithStatements(outer.Statements.Insert(outer.Statements.Count - 1, marker)));
        }
        File.WriteAllText(file, syntax.NormalizeWhitespace().ToFullString());
        EVContainer edited = EventProjectCompiler.Compile(project);
        string reopened = Export(edited, "reopened");
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
        ExpectPaths(reopened, edited, edited: true);

        static EVInstruction Marker(int label, int value) => new(label, Jsm.Opcode.MESVALUE) { Operands = [new(0), new(value)] };
        static SwitchSectionSyntax OuterArm(SyntaxNode root) => Assert.Single(root.DescendantNodes().OfType<SwitchStatementSyntax>().First().Sections);
    }

    private string Export(EVContainer container, string name)
    {
        string project = Path.Combine(_directory, name);
        EventProjectExporter.Export(container, "NestedSwitchFixture", project, useCutsceneNames: false, extractCutscenes: false);
        return SourceProjectTest.CopyWithoutComments(project);
    }

    private static void ExpectPaths(string project, EVContainer container, bool edited)
    {
        Assembly assembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        EVInstructionBody body = EVInstructionBody.Decode(Assert.Single(container.Objects[0].Scripts).Body);
        Run(1, 5, edited ? [1, 99, 3] : [1, 3]);
        Run(1, 42, edited ? [2, 99, 3] : [2, 3]);
        Run(1, 0, edited ? [99, 3] : [3]);
        Run(0, 0, [3]);

        void Run(byte outerSelector, byte innerSelector, int[] expected)
        {
            var location = new byte[80];
            location[0] = outerSelector;
            location[1] = innerSelector;
            Assert.Equal(expected, RunNative(body, location));
            var host = new RecordingHost();
            var scheduler = new EventScheduler(host, new byte[4096], location);
            scheduler.RegisterProject(assembly);
            scheduler.StartScene();
            scheduler.Tick();
            Assert.Equal(expected, host.Values);
        }
    }

    private static int[] RunNative(EVInstructionBody body, byte[] location)
    {
        var instructions = body.Instructions.ToDictionary(instruction => instruction.Label);
        var values = new List<int>();
        int position = 0;
        ushort selector = 0;
        for (int step = 0; step < instructions.Count; step++)
        {
            EVInstruction instruction = instructions[position];
            position += instruction.OriginalLength;
            switch (instruction.Opcode)
            {
                case Jsm.Opcode.EXPR:
                    EVExpressionToken token = Assert.Single(instruction.Expression);
                    Assert.Equal(0xD5, token.Opcode);
                    selector = location[token.Value];
                    break;
                case Jsm.Opcode.JMP_SWITCHEX:
                    position = instruction.Targets[Array.IndexOf(instruction.CaseValues, selector) + 1];
                    break;
                case Jsm.Opcode.MESVALUE:
                    values.Add(instruction.Operands[1].Value);
                    break;
                case Jsm.Opcode.JMP:
                    position = Assert.Single(instruction.Targets);
                    break;
                case Jsm.Opcode.Return:
                    return values.ToArray();
                default:
                    throw new InvalidOperationException($"Unexpected test instruction {instruction.Opcode}.");
            }
        }
        throw new InvalidOperationException("The test script did not return.");
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
