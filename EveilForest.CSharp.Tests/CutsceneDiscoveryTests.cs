using FF8.JSM;
using Memoria.EventEngine.EV;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class CutsceneDiscoveryTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-actor-handoffs-").FullName;

    [Theory]
    [InlineData("EVT_DALI_F_UF_EGG_0", 119, 122, 126, false)]
    [InlineData("EVT_DALI_F_UF_EGG_0", 119, 122, 126, true)]
    [InlineData("EVT_KUINA_KM_THI_0", 312, 313, 315, false)]
    public void ActorsCanAdvanceTheirOwnDialogueSceneWithoutACoordinator(string eventName, int firstLine, int answer, int followUp, bool wideSwitch)
    {
        byte[] fixture = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));
        EVContainer field = EVContainer.Read(new MemoryStream(fixture));
        if (wideSwitch)
        {
            EVContainerScript loop = field.Objects[17].Scripts.Single(script => script.Id == 1);
            EVInstructionBody body = EVInstructionBody.Decode(loop.Body);
            EVInstruction dispatch = body.Instructions.Single(instruction => instruction.Opcode == Jsm.Opcode.JMP_SWITCHEX);
            var targets = dispatch.CaseValues.Zip(dispatch.Targets.Skip(1)).ToDictionary(pair => (int)pair.First, pair => pair.Second);
            int start = targets.Keys.Min();
            int defaultTarget = dispatch.Targets[0];
            dispatch.Targets = Enumerable.Range(start, targets.Keys.Max() - start + 1).Select(value => targets.GetValueOrDefault(value, defaultTarget))
                .Prepend(defaultTarget).ToArray();
            dispatch.CaseValues = [(ushort)start];
            dispatch.Opcode = Jsm.Opcode.rsv0d;
            loop.Body = body.Encode();
        }
        string project = Path.Combine(_directory, "project");
        EventProjectExporter.Export(field, eventName, project, useCutsceneNames: false);
        string stripped = SourceProjectTest.CopyWithoutComments(project);
        string sceneFile = Assert.Single(Directory.EnumerateFiles(Path.Combine(stripped, "Cutscenes"), "*.cs").Where(file =>
            File.ReadAllText(file).Contains($"messageId: {firstLine})", StringComparison.Ordinal)));
        string scene = File.ReadAllText(sceneFile);
        Assert.Contains($"messageId: {answer})", scene);
        Assert.Contains($"messageId: {followUp})", scene);
        if (eventName == "EVT_DALI_F_UF_EGG_0")
        {
            Assert.DoesNotContain("EVT_DALI_F_UF_MST_0", scene);
            Assert.DoesNotContain("messageId: 56)", scene);
            Assert.Contains("EVT_DALI_F_UF_MST_0", File.ReadAllText(Path.Combine(stripped, "11_Area_11.cs")));
        }
        Assert.Equal(wideSwitch ? field.Serialize() : fixture, EventProjectCompiler.Compile(stripped).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(stripped));
    }

    [Fact]
    public void MatchingInstanceOffsetsDoNotProveASharedCompletionSignal()
    {
        const string eventName = "EVT_ALEX1_TS_CARGO_0";
        using Stream fixture = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));
        EVContainer original = EVContainer.Read(fixture);
        EVContainerObject[] participants = original.Objects.ToArray();
        var sharedBit = new EVVariable(EVStorageScope.Map, EVStorageType.Bit, 28 * 8 + 7).Encode();
        var privateBit = new EVVariable(EVStorageScope.Instance, EVStorageType.Bit, 0).Encode();
        foreach (int slot in new[] { 3, 17 })
        {
            EVContainerObject participant = participants[slot];
            int index = Array.FindIndex(participant.Scripts, script => script.Id == 1);
            EVInstructionBody body = EVInstructionBody.Decode(participant.Scripts[index].Body);
            EVExpressionToken[] tokens = body.Instructions.Where(instruction => instruction.Expression != null)
                .SelectMany(instruction => instruction.Expression).ToArray();
            Assert.Contains(sharedBit, tokens);
            foreach (var instruction in body.Instructions.Where(instruction => instruction.Expression != null))
                instruction.Expression = instruction.Expression.Select(token => token.Equals(sharedBit) ? privateBit : token).ToArray();

            EVContainerScript[] scripts = participant.Scripts.ToArray();
            scripts[index] = new EVContainerScript(1, body.Encode());
            participants[slot] = new EVContainerObject(participant.Id, 1, participant.Flags, participant.Reserved)
            {
                BodyOrder = participant.BodyOrder,
                EmptyOffset = participant.EmptyOffset,
                Type = participant.Type,
                Gap = participant.Gap,
                Prefix = participant.Prefix,
                Scripts = scripts
            };
        }

        string project = Path.Combine(_directory, "private-signals");
        var modified = new EVContainer(original.Header, participants, original.Trailer);
        EventProjectExporter.Export(modified, eventName, project, useCutsceneNames: false, allowNativePhaseDispatch: true);
        string outline = File.ReadAllText(Path.Combine(project, "CutsceneOutline.tsv"));
        Assert.DoesNotContain("Phase 3 polls a signal from slot 17/1", outline);
        Assert.Equal(modified.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    [Fact]
    public void FieldEntranceGuardsKeepDialogueChaptersAndTheirExitsSeparate()
    {
        const string eventName = "EVT_TRENO2_TR_FKH_0";
        byte[] fixture = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));
        string project = Path.Combine(_directory, "entrance-chapters");
        EventProjectExporter.Export(EVContainer.Read(new MemoryStream(fixture)), eventName, project, useCutsceneNames: false);
        string stripped = SourceProjectTest.CopyWithoutComments(project);
        string firstArrival = File.ReadAllText(Path.Combine(stripped, "Cutscenes", "Scene_00_01_Entrance1.cs"));
        string flashback = File.ReadAllText(Path.Combine(stripped, "Cutscenes", "Scene_00_01_EntranceMinus1.cs"));
        string owner = File.ReadAllText(Path.Combine(stripped, "00_Object_00.cs"));
        Assert.Contains("messageId: 518)", firstArrival);
        Assert.DoesNotContain("messageId: 543)", firstArrival);
        Assert.Contains("messageId: 543)", flashback);
        Assert.DoesNotContain("messageId: 518)", flashback);
        var mainEntrance = Assert.Single(CSharpSyntaxTree.ParseText(owner).GetRoot().DescendantNodes().OfType<IfStatementSyntax>()
            .Where(branch => branch.Condition.ToString() == "Globals.FieldEntrance == 1" && branch.Statement.ToString().Contains("Scene_00_01_Entrance1.Play(this)")));
        Assert.Contains("Scene_00_01_EntranceMinus1.Play(this)", mainEntrance.Else!.Statement.ToString());
        Assert.Contains("messageId: 514)", owner);
        Assert.DoesNotContain("messageId: 514)", firstArrival + flashback);
        Assert.Equal(fixture, EventProjectCompiler.Compile(stripped).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(stripped));
    }

    [Fact]
    public void NestedFieldEntranceGuardsRetainEachAlternative()
    {
        const string eventName = "EVT_INV_IV_CXI_0";
        byte[] fixture = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));
        string project = Path.Combine(_directory, "nested-entrances");
        EventProjectExporter.Export(EVContainer.Read(new MemoryStream(fixture)), eventName, project, useCutsceneNames: false);
        string stripped = SourceProjectTest.CopyWithoutComments(project);
        int[] firstLines = [82, 90, 153];
        for (int index = 0; index < firstLines.Length; index++)
        {
            string chapter = File.ReadAllText(Path.Combine(stripped, "Cutscenes", $"Scene_00_01_EntranceMinus{index + 1}.cs"));
            Assert.Contains($"messageId: {firstLines[index]})", chapter);
            foreach (int other in firstLines.Where(message => message != firstLines[index]))
                Assert.DoesNotContain($"messageId: {other})", chapter);
        }
        Assert.Equal(fixture, EventProjectCompiler.Compile(stripped).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(stripped));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FieldEntranceStagingRequiresSpeakersOnTheSamePath(bool exclusiveSpeakers)
    {
        const string eventName = "EVT_MAGE_BV_GVY_0";
        byte[] fixture = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));
        EVContainer field = EVContainer.Read(new MemoryStream(fixture));
        if (exclusiveSpeakers)
        {
            EVContainerScript root = field.Objects[0].Scripts.Single(script => script.Id == 1);
            var entrance = new EVVariable(EVStorageScope.Global, EVStorageType.Int16, 2).Encode();
            var choice = new EVVariable(EVStorageScope.Map, EVStorageType.Bit, 18 * 8).Encode();
            IList<EVInstruction> original = EVInstructionBody.Decode(root.Body).Instructions;
            EVInstruction pause = original.First(instruction => instruction.Opcode == Jsm.Opcode.WAIT && instruction.Operands[0].Value > 0);
            EVInstruction stageVivi = original.First(instruction => instruction.Opcode == Jsm.Opcode.REQ && instruction.Operands[1].Value == 12);
            EVInstruction stageMage = original.First(instruction => instruction.Opcode == Jsm.Opcode.DTURN && instruction.Operands[0].Value == 8);
            EVInstruction mageLine = original.First(instruction => instruction.Opcode == Jsm.Opcode.MESA && instruction.Operands[3].Value == 220);
            EVInstruction viviLine = original.First(instruction => instruction.Opcode == Jsm.Opcode.MESA && instruction.Operands[3].Value == 222);
            var body = new EVInstructionBody([
                new(0, Jsm.Opcode.EXPR) { Expression = [entrance, EVExpressionToken.Literal(1), new((byte)Jsm.Excode.Equivalence)] },
                new(1, Jsm.Opcode.JMP_IFN) { Targets = [11] },
                Copy(2, pause),
                Copy(3, stageVivi),
                Copy(4, stageMage),
                new(5, Jsm.Opcode.EXPR) { Expression = [choice] },
                new(6, Jsm.Opcode.JMP_IFN) { Targets = [9] },
                Copy(7, mageLine),
                new(8, Jsm.Opcode.JMP) { Targets = [11] },
                Copy(9, viviLine),
                new(10, Jsm.Opcode.JMP) { Targets = [11] },
                new(11, Jsm.Opcode.Return)
            ], 12);
            root.Body = body.Encode();
        }

        string project = Path.Combine(_directory, "staged-entrance");
        EventProjectExporter.Export(field, eventName, project, useCutsceneNames: false);
        string stripped = SourceProjectTest.CopyWithoutComments(project);
        string scene = Path.Combine(stripped, "Cutscenes", "Scene_00_01_Entrance1.cs");
        if (exclusiveSpeakers)
        {
            Assert.False(File.Exists(scene));
            Assert.Contains("messageId: 222)", File.ReadAllText(Path.Combine(stripped, "00_Object_00.cs")));
        }
        else
        {
            string source = File.ReadAllText(scene);
            Assert.Contains("messageId: 220)", source);
            Assert.Contains("messageId: 222)", source);
            Assert.DoesNotContain("messageId: 390)", source);
            Assert.Contains("messageId: 390)", File.ReadAllText(Path.Combine(stripped, "Cutscenes", "Scene_00_01_EntranceMinus1.cs")));
        }
        Assert.Equal(exclusiveSpeakers ? field.Serialize() : fixture, EventProjectCompiler.Compile(stripped).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(stripped));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DirectedDialogueUsesRequestsAndSpeakersFromOnePath(bool exclusiveBranches)
    {
        const string eventName = "EVT_DOWNSHIP_BT_CMR_0";
        byte[] fixture = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", eventName + ".eb.bytes"));
        EVContainer field = EVContainer.Read(new MemoryStream(fixture));
        if (exclusiveBranches)
        {
            EVContainerScript root = field.Objects[0].Scripts.Single(script => script.Id == 1);
            var selector = new EVVariable(EVStorageScope.Map, EVStorageType.Byte, 32).Encode();
            var condition = new EVVariable(EVStorageScope.Map, EVStorageType.Bit, 18 * 8).Encode();
            IList<EVInstruction> original = EVInstructionBody.Decode(root.Body).Instructions;
            EVInstruction zidaneRequest = original.First(instruction => instruction.Opcode == Jsm.Opcode.REQ);
            EVInstruction bakuCompletion = original.First(instruction => instruction.Opcode == Jsm.Opcode.REQEW);
            EVInstruction bakuLine = original.First(instruction => instruction.Opcode == Jsm.Opcode.MESA && instruction.Operands[0].Value == 3);
            EVInstruction zidaneLine = original.First(instruction => instruction.Opcode == Jsm.Opcode.MESA && instruction.Operands[0].Value == 6);
            var body = new EVInstructionBody([
                new(0, Jsm.Opcode.EXPR) { Expression = [selector] },
                new(1, Jsm.Opcode.JMP_SWITCH) { CaseValues = [1], Targets = [12, 2] },
                new(2, Jsm.Opcode.EXPR) { Expression = [condition] },
                new(3, Jsm.Opcode.JMP_IFN) { Targets = [8] },
                Copy(4, zidaneRequest),
                Copy(5, bakuLine),
                Copy(6, zidaneLine),
                new(7, Jsm.Opcode.JMP) { Targets = [12] },
                Copy(8, bakuCompletion),
                Copy(9, bakuLine),
                Copy(10, zidaneLine),
                new(11, Jsm.Opcode.JMP) { Targets = [12] },
                new(12, Jsm.Opcode.Return)
            ], 13);
            root.Body = body.Encode();
        }

        string project = Path.Combine(_directory, "directed-arm");
        EventProjectExporter.Export(field, eventName, project, useCutsceneNames: false);
        string scene = Path.Combine(project, "Cutscenes", "Scene_00_01_At1.cs");
        if (exclusiveBranches)
        {
            Assert.False(File.Exists(scene));
        }
        else
        {
            string source = CSharpSyntaxTree.ParseText(File.ReadAllText(scene)).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Single(method => method.Identifier.ValueText == "Phase1").ToFullString();
            Assert.Contains("messageId: 228)", source);
            Assert.Contains("messageId: 230)", source);
            Assert.True(source.Contains("await scripts.RequestAndWait(evt.GEO_SUB_F0_BAK_03.Script_13, level: 4);", StringComparison.Ordinal) ||
                source.Contains("await scripts.RunAndWait(On(evt.GEO_SUB_F0_BAK_03, Action_03_13), level: 4);", StringComparison.Ordinal));
        }

        string stripped = SourceProjectTest.CopyWithoutComments(project);
        Assert.Equal(exclusiveBranches ? field.Serialize() : fixture, EventProjectCompiler.Compile(stripped).Serialize());

    }

    private static EVInstruction Copy(int label, EVInstruction instruction) => new(label, instruction.Opcode)
    {
        ArgumentMask = instruction.ArgumentMask,
        Operands = instruction.Operands
    };

    public void Dispose()
    {
        Directory.Delete(_directory, recursive: true);
    }
}
