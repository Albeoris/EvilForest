using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class OrderedSwitchLayoutTests : IDisposable
{
    private const string EventName = "EVT_LIND1_TN_LB_STN_0";
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-ordered-switch-layout-").FullName;

    [Fact]
    public void EditingOneOfTwoIdenticalSwitchShapesPreservesBothEncodings()
    {
        using var stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", EventName + ".eb.bytes"));
        EVContainer original = EVContainer.Read(stream);
        string project = Export(original, "source");
        string file = Directory.GetFiles(project, "16_*.cs").Single();
        SyntaxNode root = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();
        MethodDeclarationSyntax handler = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "OnInit");
        SwitchStatementSyntax[] switches = handler.DescendantNodes().OfType<SwitchStatementSyntax>().ToArray();
        Assert.Equal(3, switches.Length);
        Assert.All(switches, selection => Assert.Equal("Location.Byte_59", selection.Expression.ToString()));
        Assert.Empty(handler.DescendantNodes().OfType<GotoStatementSyntax>());
        Assert.Equal(original.Serialize(), EventProjectCompiler.Compile(project).Serialize());

        var firstCase = switches[0].Sections[0];
        Assert.Equal("case 1:", firstCase.Labels[0].ToString());
        StatementSyntax assignment = firstCase.Statements[0];
        SyntaxNode edited = root.ReplaceNode(assignment, SyntaxFactory.ParseStatement("Location.Int16_0 = -1613;").WithTriviaFrom(assignment));
        File.WriteAllText(file, edited.ToFullString());

        EVContainerScript script = original.Objects[16].Scripts.Single(candidate => candidate.Id == 0);
        EVInstructionBody body = EVInstructionBody.Decode(script.Body);
        EVInstruction table = body.Instructions.First(instruction => instruction.Opcode == Jsm.Opcode.JMP_SWITCH);
        Assert.Equal((ushort)1, Assert.Single(table.CaseValues));
        EVInstruction firstAssignment = body.Instructions.Single(instruction => instruction.Label == table.Targets[1]);
        Assert.Equal(EVExpressionToken.Literal(-1614), firstAssignment.Expression[1]);
        firstAssignment.Expression[1] = EVExpressionToken.Literal(-1613);
        script.Body = body.Encode();

        EVContainer rebuilt = EventProjectCompiler.Compile(project);
        Assert.Equal(original.Serialize(), rebuilt.Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
        string reopened = Export(rebuilt, "reopened");
        Assert.Equal(rebuilt.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    private string Export(EVContainer container, string name)
    {
        string project = Path.Combine(_directory, name);
        EventProjectExporter.Export(container, EventName, project, useCutsceneNames: false);
        return SourceProjectTest.CopyWithoutComments(project);
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
