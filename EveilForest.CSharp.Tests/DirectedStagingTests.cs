using EveilForest.CSharp;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class DirectedStagingTests : IDisposable
{
    private const string Lindblum = "EVT_LIND1_CS_LB_ASD_0";
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-staging-").FullName;

    [Theory]
    [InlineData(false, false, true)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    public void TimedActorAdmissionsAndCameraCanLeadIntoDialogue(bool admissionsOnly, bool removeCamera, bool extracted)
    {
        byte[] fixture = ReadFixture(Lindblum);
        EVContainer field = EVContainer.Read(new MemoryStream(fixture));
        if (admissionsOnly)
        {
            EVContainerScript script = field.Objects[0].Scripts.Single(entry => entry.Id == 1);
            EVInstructionBody body = EVInstructionBody.Decode(script.Body);
            for (int index = 0; index < body.Instructions.Count; index++)
            {
                EVInstruction instruction = body.Instructions[index];
                if (instruction.Opcode == Jsm.Opcode.REQEW)
                {
                    body.Instructions[index] = new EVInstruction(instruction.Label, Jsm.Opcode.REQSW)
                    {
                        ArgumentMask = instruction.ArgumentMask,
                        Operands = instruction.Operands
                    };
                }
                else if (removeCamera && instruction.Opcode == Jsm.Opcode.BGSSCROLL)
                {
                    body.Instructions[index] = new EVInstruction(instruction.Label, Jsm.Opcode.NOP);
                }
            }
            script.Body = body.Encode();
        }

        string project = Path.Combine(_directory, "lindblum");
        EventProjectExporter.Export(field, Lindblum, project, useCutsceneNames: false);
        string stripped = SourceProjectTest.CopyWithoutComments(project);
        string scene = Path.Combine(stripped, "Cutscenes", "Scene_00_01_At1.cs");
        Assert.Equal(extracted, File.Exists(scene));
        if (extracted)
        {
            string source = CSharpSyntaxTree.ParseText(File.ReadAllText(scene)).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Single(method => method.Identifier.ValueText == "Phase1").ToFullString();
            AssertAdmission("GEO_MAIN_F0_ZDN_13", 13);
            AssertAdmission("GEO_MAIN_F0_VIV_14", 14);
            AssertAdmission("GEO_MAIN_F0_STN_16", 16);
            Assert.Contains("messageId: 155)", source);
            Assert.Contains("messageId: 156)", source);
            Assert.DoesNotContain("Together(", source);
            Assert.Equal(!admissionsOnly, source.Contains("RequestAndWait(", StringComparison.Ordinal) || source.Contains("RunAndWait(", StringComparison.Ordinal));

            void AssertAdmission(string actor, int slot)
            {
                string request = $"await scripts.RequestWhenReady(evt.{actor}.Script_11, level: 2);";
                string action = $"await scripts.StartWhenReady(On(evt.{actor}, Action_{slot:D2}_11), level: 2);";
                Assert.True(source.Contains(request, StringComparison.Ordinal) || source.Contains(action, StringComparison.Ordinal), $"Missing admission request for {actor}.");
            }
        }
        Assert.Equal(admissionsOnly ? field.Serialize() : fixture, EventProjectCompiler.Compile(stripped).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(stripped));
    }

    [Fact]
    public void RepeatableWaitressConversationStaysInItsCallback()
    {
        const string eventName = "EVT_TRENO2_TR_WBH_0";
        byte[] fixture = ReadFixture(eventName);
        string project = Path.Combine(_directory, "treno");
        EventProjectExporter.Export(EVContainer.Read(new MemoryStream(fixture)), eventName, project, useCutsceneNames: false);
        string stripped = SourceProjectTest.CopyWithoutComments(project);
        string waitress = Directory.GetFiles(stripped, "05_*.cs").Single();
        string source = File.ReadAllText(waitress);
        Assert.Contains("[Script(3,", source);
        Assert.DoesNotContain("ctx.Scene_05_03", source);
        Assert.Equal(fixture, EventProjectCompiler.Compile(stripped).Serialize());
    }

    private static byte[] ReadFixture(string eventName) =>
        File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
