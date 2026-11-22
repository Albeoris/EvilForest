using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class DirectorVariantTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-director-variant-{Guid.NewGuid():N}");

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void TwoPlayerCallbacksOnOneActorDoNotMakeAFieldVariantIntoADirector(bool useNames)
    {
        const string field = "EVT_LIND1_TN_LB_STN_0";
        EVContainer fixture = Fixture(field);
        EventProjectExporter.Export(fixture, field, _directory, useCutsceneNames: useNames);

        MethodDeclarationSyntax setup = Methods(Directory.GetFiles(_directory, "16_*.cs").Single())
            .Single(method => method.Identifier.ValueText == "OnInit");
        Assert.Equal(3, setup.DescendantNodes().OfType<SwitchStatementSyntax>()
            .Count(selection => selection.Expression.ToString() == "Location.Byte_59"));
        MethodDeclarationSyntax[] attendant = Methods(Directory.GetFiles(_directory, "11_*.cs").Single()).ToArray();
        foreach (string name in new[] { "OnSpeak", "OnCard" })
        {
            var callback = Assert.Single(attendant.Where(method => method.Identifier.ValueText == name));
            Assert.Contains(callback.DescendantNodes().OfType<SwitchStatementSyntax>(),
                selection => selection.Expression.ToString() == "Location.Byte_59");
            Assert.Contains("IsMovementControlEnabled", callback.ToString());
        }

        Assert.Equal(fixture.Serialize(), EventProjectCompiler.Compile(_directory).Serialize());
    }

    [Fact]
    public void ChoreographyIncludesTheWharfsExternallySelectedRoute()
    {
        const string field = "EVT_ALEX3_AT_WHARF";
        EVContainer fixture = Fixture(field);
        EventProjectExporter.Export(fixture, field, _directory, useCutsceneNames: false);

        MethodDeclarationSyntax[] methods = Directory.GetFiles(Path.Combine(_directory, "Cutscenes"), "*.cs")
            .SelectMany(Methods).ToArray();
        foreach (string method in new[] { "Direct", "Phase3_GEO_MAIN_F0_FRJ_23", "Phase3_GEO_MAIN_F0_SLM_26" })
            Assert.Contains(methods, candidate => candidate.Identifier.ValueText == method);
        var director = Assert.Single(methods.Where(method => method.Identifier.ValueText == "Direct"));
        var routes = Assert.Single(director.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => IsCall(call, "Routes")));
        Assert.Contains(routes.ArgumentList.Arguments, argument => argument.Expression.ToString() == "FromPhase1");
        var route = Assert.Single(methods.Where(method => method.Identifier.ValueText == "FromPhase1"));
        Assert.Contains(route.DescendantNodes().OfType<InvocationExpressionSyntax>(), call => IsCall(call, "Together") && call.ArgumentList.Arguments[0].ToString() == "1");
        Assert.Contains(route.DescendantNodes().OfType<InvocationExpressionSyntax>(), call => IsCall(call, "Idle") && call.ArgumentList.Arguments[0].ToString() == "0");
        Assert.Equal(fixture.Serialize(), EventProjectCompiler.Compile(_directory).Serialize());
    }

    [Theory]
    [InlineData("EVT_ALEX1_AC_ENT_2F", "[Script(1, 1)]")]
    [InlineData("EVT_ALEX1_TS_STAGE_2B", "play.BeforePoll(")]
    [InlineData("EVT_ALEX1_TS_SWD_BTL", "await play.FallThrough(")]
    [InlineData("EVT_BURMECIA_TOWN_1", "await play.RepeatFrom(")]
    public void CoordinatorShapesRoundTripWithoutNativeDispatch(string field, string declaration)
    {
        EVContainer fixture = Fixture(field);
        EventProjectExporter.Export(fixture, field, _directory, useCutsceneNames: false);

        Assert.Contains(Directory.GetFiles(Path.Combine(_directory, "Cutscenes"), "*.cs"), file => File.ReadAllText(file).Contains(declaration, StringComparison.Ordinal));
        Assert.Equal(fixture.Serialize(), EventProjectCompiler.Compile(_directory).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(_directory));
    }

    [Fact]
    public void AnEditedFallThroughToANonadjacentArmCanBeExportedAgain()
    {
        const string field = "EVT_ALEX1_TS_SWD_BTL";
        EVContainer fixture = Fixture(field);
        EventProjectExporter.Export(fixture, field, _directory, useCutsceneNames: false);
        string path = Assert.Single(Directory.GetFiles(Path.Combine(_directory, "Cutscenes"), "*.cs"), file => File.ReadAllText(file).Contains("await play.FallThrough("));
        var syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
        var original = Assert.Single(syntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(call => IsCall(call, "FallThrough")));
        var destination = original.ArgumentList.Arguments[2].Expression;
        Assert.Equal("10", destination.ToString());
        File.WriteAllText(path, syntax.ReplaceNode(destination, SyntaxFactory.ParseExpression("11")).ToFullString());

        EVContainer edited = EventProjectCompiler.Compile(_directory);
        Assert.NotEqual(fixture.Serialize(), edited.Serialize());
        string reexport = Path.Combine(_directory, "Reexport");
        EventProjectExporter.Export(edited, field, reexport, useCutsceneNames: false);
        Assert.Contains(Directory.GetFiles(Path.Combine(reexport, "Cutscenes"), "*.cs"), file => File.ReadAllText(file).Contains("await play.FallThrough(9, RunPhase9, 11)"));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reexport).Serialize());
    }

    private static bool IsCall(InvocationExpressionSyntax call, string name) =>
        call.Expression is MemberAccessExpressionSyntax member && member.Name.Identifier.ValueText == name;

    private static EVContainer Fixture(string name)
    {
        using FileStream stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));
        return EVContainer.Read(stream);
    }

    private static IEnumerable<MethodDeclarationSyntax> Methods(string file) =>
        CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }
}
