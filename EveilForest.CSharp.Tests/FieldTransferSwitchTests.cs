using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class FieldTransferSwitchTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-field-transfer-{Guid.NewGuid():N}");

    [Theory]
    [InlineData("EVT_CLEYRA2_CATHE_2", 11)]
    [InlineData("EVT_CLEYRA2_INN", 11)]
    [InlineData("EVT_CLEYRA2_TOWN", 11)]
    [InlineData("EVT_CLEYRA2_VISTA", 11)]
    [InlineData("EVT_CLEYRA2_WATER", 11)]
    [InlineData("EVT_ALEX3_AT_BACK_STR", 17)]
    public void AdjacentFieldTransferArmsExportAsOrdinarySwitchSections(string field, int armCount)
    {
        (byte[] original, string source) = ExportWithoutComments(field);
        SwitchStatementSyntax selection = Assert.Single(Directory.EnumerateFiles(Path.Combine(source, "Cutscenes"), "*.cs")
            .SelectMany(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot().DescendantNodes().OfType<SwitchStatementSyntax>())
            .Where(selection => selection.Sections.Count == armCount && selection.Sections.All(EndsWithFieldTransfer)));
        Assert.DoesNotContain(selection.DescendantNodes(), node => node is GotoStatementSyntax or LabeledStatementSyntax);
        Assert.Equal(original, EventProjectCompiler.Compile(source).Serialize());
    }

    [Theory]
    [InlineData("EVT_DOWNSHIP_BT_CCP_0", "07_GEO_MAIN_F0_ZDN_07.cs", "Globals.FieldEntrance", "break;", true)]
    [InlineData("EVT_DOWNSHIP_BT_GTR_0", "15_GEO_MAIN_F0_ZDN_15.cs", "Globals.FieldEntrance", "break;", true)]
    [InlineData("EVT_EFOREST1_EF_TNT_0", "10_GEO_MAIN_F0_ZDN_10.cs", "Globals.FieldEntrance", "break;", false)]
    [InlineData("EVT_CHOCO_CH_DWL_0", "11_GEO_NPC_F0_CHD_11.cs", "Globals.Byte_191", "await Engine.Wait(frames: 12);", true)]
    public void AStructuredInnerSwitchPreservesItsContinuationAndDefault(string field, string participant, string selector, string continuation, bool hasDefault)
    {
        (byte[] original, string source) = ExportWithoutComments(field);
        var unit = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(source, participant))).GetRoot();
        SwitchStatementSyntax selection = Assert.Single(unit.DescendantNodes().OfType<SwitchStatementSyntax>().Where(selection =>
            selection.Expression.ToString() == selector && selection.Parent is SwitchSectionSyntax parent &&
            parent.Statements.Skip(parent.Statements.IndexOf(selection) + 1).FirstOrDefault()?.ToString() == continuation));

        Assert.Equal(hasDefault, selection.Sections.SelectMany(section => section.Labels).Any(label => label is DefaultSwitchLabelSyntax));
        Assert.DoesNotContain(selection.DescendantNodes(), node => node is GotoStatementSyntax or LabeledStatementSyntax);
        Assert.Equal(original, EventProjectCompiler.Compile(source).Serialize());
    }

    [Fact]
    public void ANewSwitchRegionDoesNotFlattenOtherStructuredCode()
    {
        (byte[] original, string source) = ExportWithoutComments("EVT_PINA_PR_PW3_0");
        var unit = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(source, "08_GEO_NPC_F4_JJY_08.cs"))).GetRoot();
        MethodDeclarationSyntax handler = Assert.Single(unit.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Identifier.ValueText == "OnSpeak"));

        int jumps = handler.DescendantNodes().OfType<GotoStatementSyntax>().Count();
        Assert.True(jumps <= 25, $"Expected at most 25 local jumps in the structured handler, found {jumps}.");
        Assert.Equal(original, EventProjectCompiler.Compile(source).Serialize());
    }

    private (byte[] Original, string Source) ExportWithoutComments(string field)
    {
        byte[] original = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", $"{field}.eb.bytes"));
        using var input = new MemoryStream(original);
        string project = Path.Combine(_directory, field);
        EventProjectExporter.Export(EVContainer.Read(input), field, project);
        return (original, SourceProjectTest.CopyWithoutComments(project));
    }

    private static bool EndsWithFieldTransfer(SwitchSectionSyntax section) =>
        section.Statements.Count >= 2 && section.Statements[^1] is BreakStatementSyntax &&
        section.Statements[^2] is ExpressionStatementSyntax
        {
            Expression: InvocationExpressionSyntax
            {
                Expression: MemberAccessExpressionSyntax
                {
                    Expression: IdentifierNameSyntax { Identifier.ValueText: "Field" },
                    Name.Identifier.ValueText: "ChangeMap"
                }
            }
        };

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }
}
