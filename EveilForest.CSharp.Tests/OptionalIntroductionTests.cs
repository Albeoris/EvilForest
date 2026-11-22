using System.Buffers.Binary;
using System.Reflection;
using EvilForest.EventSource;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;

namespace EveilForest.CSharp.Tests;

public sealed class OptionalIntroductionTests : IDisposable
{
    private const string Marsh = "EVT_KUINA_KM_SWP_1";
    private const string OtherMarsh = "EVT_KUINA_KM_SWP_0";
    private const int Invitation = 355;
    private const int Prompt = 356;
    private const int Acceptance = 358;
    private const int EditedAcceptance = 359;
    private const int Quina = 22;
    private readonly string _directory = Directory.CreateTempSubdirectory("evilforest-optional-introduction-").FullName;

    [Theory]
    [InlineData(Marsh, 22, 17)]
    [InlineData(OtherMarsh, 19, 14)]
    public void InvitationMovesTheDialogueAndBothPathsTogether(string name, int quina, int zidane)
    {
        byte[] fixture = ReadFixture(name);
        string project = SourceProjectTest.CopyWithoutComments(Export(Read(fixture), name, "extracted"));
        string scene = File.ReadAllText(FindInvitation(project, Prompt));
        Assert.Contains($"speakerUid: {quina}, windowId: 0, flags: 128, messageId: {Invitation})", scene);
        Assert.Contains($"speakerUid: {zidane}, windowId: 0, flags: 128, messageId: {Prompt})", scene);
        Assert.Contains($"messageId: {Acceptance})", scene);
        AssertRequest(scene, "KUI", quina, 11, waitForReturn: false);
        AssertRequest(scene, "ZDN", zidane, 12, waitForReturn: true);
        AssertRequest(scene, "KUI", quina, 28, waitForReturn: true);
        Assert.Contains("SystemVariable.DialogChoice", scene);
        string handler = File.ReadAllText(OwnerFile(project));
        Assert.Contains("if (Location.Byte_64 == 99)", handler);
        Assert.DoesNotContain($"messageId: {Prompt})", handler);
        Assert.Contains("messageId: 359)", handler);
        Assert.Contains("messageId: 360)", handler);
        Assert.Contains("messageId: 361)", handler);
        Assert.Contains("messageId: 362)", handler);
        Assert.DoesNotContain("messageId: 359)", scene);
        Assert.DoesNotContain("messageId: 360)", scene);
        Assert.DoesNotContain("messageId: 361)", scene);
        Assert.DoesNotContain("messageId: 362)", scene);
        Assert.Equal(fixture, EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void EditingAcceptancePreservesDeclineAndReexportsTheEditedInvitation()
    {
        byte[] fixture = ReadFixture(Marsh);
        EVContainer original = Read(fixture);
        string reference = Export(original, Marsh, "reference", extract: false);
        string project = SourceProjectTest.CopyWithoutComments(Export(original, Marsh, "extracted"));
        string path = FindInvitation(project, Prompt);
        string scene = File.ReadAllText(path);
        string acceptance = $"messageId: {Acceptance})";
        Assert.Single(scene.Split(acceptance, StringSplitOptions.None).Skip(1));
        File.WriteAllText(path, scene.Replace(acceptance, $"messageId: {EditedAcceptance})", StringComparison.Ordinal));

        EVContainer edited = EventProjectCompiler.Compile(project);
        byte[] editedBytes = edited.Serialize();
        Assert.Equal(fixture.Length, editedBytes.Length);
        Assert.Equal((Acceptance & 0xFF, EditedAcceptance & 0xFF), Assert.Single(fixture.Zip(editedBytes)
            .Where(pair => pair.First != pair.Second).Select(pair => ((int)pair.First, (int)pair.Second))));
        string reopened = Export(edited, Marsh, "reopened");
        Assert.Contains($"messageId: {EditedAcceptance})", File.ReadAllText(FindInvitation(reopened, Prompt)));
        Assert.Equal(editedBytes, EventProjectCompiler.Compile(reopened).Serialize());

        Assembly referenceAssembly = Assembly.Load(EventProjectCompiler.CompileManaged(reference));
        Assembly editedAssembly = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Assert.Equal(Trace(referenceAssembly, 1).Commands, Trace(editedAssembly, 1).Commands);
        var originalYes = Trace(referenceAssembly, 0);
        var editedYes = Trace(editedAssembly, 0);
        Assert.Equal(originalYes.Commands.Select(command => command with
        {
            Value = command.Opcode == Opcode.MESAN && command.Value == Acceptance ? EditedAcceptance : command.Value
        }), editedYes.Commands);
    }

    [Fact]
    public void EditingTheSceneChoiceChangesTheSameBranchAsEditingTheInlineChoice()
    {
        const string choice = "Engine.ReadSystem(SystemVariable.DialogChoice) == 0";
        const string invertedChoice = "Engine.ReadSystem(SystemVariable.DialogChoice) != 0";
        EVContainer field = Read(ReadFixture(Marsh));
        string reference = Export(field, Marsh, "reference", extract: false);
        string project = SourceProjectTest.CopyWithoutComments(Export(field, Marsh, "extracted"));
        string scene = FindInvitation(project, Prompt);
        Assembly original = Assembly.Load(EventProjectCompiler.CompileManaged(project));

        string sceneSource = File.ReadAllText(scene);
        int sceneChoice = sceneSource.IndexOf(choice, StringComparison.Ordinal);
        Assert.True(sceneChoice >= 0);
        File.WriteAllText(scene, sceneSource.Remove(sceneChoice, choice.Length).Insert(sceneChoice, invertedChoice));
        string referencePath = OwnerFile(reference);
        string referenceSource = File.ReadAllText(referencePath);
        int prompt = referenceSource.IndexOf($"messageId: {Prompt})", StringComparison.Ordinal);
        Assert.True(prompt >= 0);
        int inlineChoice = referenceSource.IndexOf(choice, prompt, StringComparison.Ordinal);
        Assert.True(inlineChoice > prompt);
        File.WriteAllText(referencePath, referenceSource.Remove(inlineChoice, choice.Length).Insert(inlineChoice, invertedChoice));

        EVContainer edited = EventProjectCompiler.Compile(project);
        Assert.Equal(EventProjectCompiler.Compile(reference).Serialize(), edited.Serialize());
        Assembly managedEdit = Assembly.Load(EventProjectCompiler.CompileManaged(project));
        Assert.Equal(Trace(original, 0).Commands, Trace(managedEdit, 1).Commands);
        Assert.Equal(Trace(original, 1).Commands, Trace(managedEdit, 0).Commands);
        string reopened = Export(edited, Marsh, "reopened");
        Assert.Contains(invertedChoice, File.ReadAllText(FindInvitation(reopened, Prompt)));
        Assert.Equal(edited.Serialize(), EventProjectCompiler.Compile(reopened).Serialize());
    }

    [Fact]
    public void AChoiceCallCannotDiscardAnEffectfulContextReceiver()
    {
        EVContainer field = Read(ReadFixture(Marsh));
        string project = Export(field, Marsh, "extracted");
        string sceneName = Path.GetFileNameWithoutExtension(FindInvitation(project, Prompt));
        string path = OwnerFile(project);
        string source = File.ReadAllText(path);
        string call = $"ctx.{sceneName}.AcceptsInvitation()";
        Assert.Contains(call, source);
        const string getter = """
                private SceneContext ChooseContext()
                {
                    Globals.ScenarioCounter = 7;
                    return ctx;
                }

            """;
        int init = source.IndexOf("    [Script(0, 0)]", StringComparison.Ordinal);
        Assert.True(init >= 0);
        source = source.Insert(init, getter).Replace(call, $"ChooseContext().{sceneName}.AcceptsInvitation()", StringComparison.Ordinal);
        File.WriteAllText(path, source);

        Assert.Throws<InvalidDataException>(() => EventProjectCompiler.Compile(project));
        Assert.Throws<InvalidDataException>(() => EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void ABlockedRequestRetriesAndTheSameSchedulerCanAcceptAfterDeclining()
    {
        EVContainer field = Read(ReadFixture(Marsh));
        Assembly local = Assembly.Load(EventProjectCompiler.CompileManaged(Export(field, Marsh, "reference", extract: false)));
        Assembly extracted = Assembly.Load(EventProjectCompiler.CompileManaged(Export(field, Marsh, "extracted")));
        var original = Trace(local, 1, retry: true);
        var result = Trace(extracted, 1, retry: true);
        Assert.Equal(original.Commands, result.Commands);
        Assert.Equal(new[] { 1, 0 }, result.Choices);
        int[] promptFrames = result.Commands.Where(command => command.Opcode == Opcode.MESA && command.Value == Prompt)
            .Select(command => command.Frame).ToArray();
        Assert.Equal(2, promptFrames.Length);
        Assert.True(promptFrames[0] > Trace(local, 1).Commands.Single(command => command.Opcode == Opcode.MESA && command.Value == Prompt).Frame);
        Assert.Single(result.Commands.Where(command => command.Opcode == Opcode.MESAN && command.Value == Acceptance));

        var originalActivity = Trace(local, 0, continueActivity: true);
        var continued = Trace(extracted, 0, continueActivity: true);
        Assert.Equal(originalActivity.Commands, continued.Commands);
        Assert.Equal(new[] { 0, 1 }, continued.Choices);
        Assert.Equal(new[] { Invitation, Prompt, 359, 361 }, continued.Commands.Where(command => command.Opcode == Opcode.MESA)
            .Select(command => command.Value));
    }

    [Fact]
    public void RelocatedMapStateRequestTagAndMessagesStillIdentifyTheInvitation()
    {
        const int messageShift = 260;
        const int relocatedAdmissionValue = 97;
        const ushort relocatedRequest = 100;
        EVContainer field = Read(ReadFixture(Marsh));
        EVExpressionToken gate = Map(EVStorageType.Byte, 64);
        EVExpressionToken mode = Map(EVStorageType.Bit, 613);
        EVExpressionToken relocatedGate = Map(EVStorageType.Byte, 23);
        EVExpressionToken relocatedMode = Map(EVStorageType.Bit, 337);
        int replacedRequests = 0;
        int replacedMessages = 0;
        foreach (EVContainerObject obj in field.Objects)
        {
            for (int index = 0; index < obj.Scripts.Length; index++)
            {
                EVContainerScript script = obj.Scripts[index];
                EVInstructionBody body = EVInstructionBody.Decode(script.Body);
                foreach (EVInstruction instruction in body.Instructions)
                {
                    if (instruction.Expression is { } expression)
                    {
                        bool usesGate = expression.Contains(gate);
                        instruction.Expression = expression.Select(token => usesGate && token.Equals(EVExpressionToken.Literal(99))
                            ? EVExpressionToken.Literal(relocatedAdmissionValue)
                            : Relocate(token)).ToArray();
                    }
                    for (int operand = 0; operand < instruction.Operands.Length; operand++)
                    {
                        if (instruction.Operands[operand].Expression is { } argument)
                            instruction.Operands[operand] = new EVOperand(argument.Select(Relocate).ToArray());
                    }
                    if (instruction.Opcode is Jsm.Opcode.REQ or Jsm.Opcode.REQSW or Jsm.Opcode.REQEW &&
                        instruction.Operands[1].Expression == null && instruction.Operands[1].Value == Quina &&
                        instruction.Operands[2].Expression == null && instruction.Operands[2].Value == 11)
                    {
                        instruction.Operands[2] = new EVOperand(relocatedRequest);
                        replacedRequests++;
                    }
                    if (obj.Id == 0 && script.Id == 1 && instruction.Opcode is Jsm.Opcode.MESA or Jsm.Opcode.MESAN &&
                        instruction.Operands[3].Expression == null && instruction.Operands[3].Value is Invitation or Prompt or Acceptance)
                    {
                        instruction.Operands[3] = new EVOperand(instruction.Operands[3].Value + messageShift);
                        replacedMessages++;
                    }
                }
                script.Body = body.Encode();
                if (obj.Id == Quina && script.Id == 11)
                    obj.Scripts[index] = new EVContainerScript(relocatedRequest, script.Body, script.OriginalOffset);
            }
        }
        Assert.Equal(1, replacedRequests);
        Assert.Equal(3, replacedMessages);
        string project = SourceProjectTest.CopyWithoutComments(Export(field, Marsh, "relocated"));
        Assert.Contains($"if (Location.Byte_23 == {relocatedAdmissionValue})", File.ReadAllText(OwnerFile(project)));
        string scene = File.ReadAllText(FindInvitation(project, Prompt + messageShift));
        Assert.Contains($"messageId: {Acceptance + messageShift})", scene);
        AssertRequest(scene, "KUI", Quina, relocatedRequest, waitForReturn: false);
        Assert.Contains("Location.Bit_337 = true;", scene);
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));

        EVExpressionToken Relocate(EVExpressionToken token) => token.Equals(gate) ? relocatedGate : token.Equals(mode) ? relocatedMode : token;
    }

    [Theory]
    [InlineData(Jsm.Opcode.REQSW)]
    [InlineData(Jsm.Opcode.REQEW)]
    public void ExistingStagingRequestsMayUseTheSameWaitMode(Jsm.Opcode request)
    {
        EVContainer field = Read(ReadFixture(Marsh));
        EVContainerScript loop = field.Objects[0].Scripts.Single(script => script.Id == 1);
        EVInstructionBody body = EVInstructionBody.Decode(loop.Body);
        EVInstruction[] stagingRequests = body.Instructions
            .TakeWhile(instruction => instruction.Opcode != Jsm.Opcode.MESA || instruction.Operands[3].Value != Invitation)
            .Where(instruction => instruction.Opcode is Jsm.Opcode.REQSW or Jsm.Opcode.REQEW).ToArray();
        Assert.Collection(stagingRequests,
            instruction => Assert.Equal(Jsm.Opcode.REQSW, instruction.Opcode),
            instruction => Assert.Equal(Jsm.Opcode.REQEW, instruction.Opcode));
        foreach (EVInstruction instruction in stagingRequests)
            instruction.Opcode = request;
        loop.Body = body.Encode();

        string project = SourceProjectTest.CopyWithoutComments(Export(field, Marsh, "same-request-mode"));
        string scene = File.ReadAllText(FindInvitation(project, Prompt));
        AssertRequest(scene, "KUI", Quina, 11, waitForReturn: request == Jsm.Opcode.REQEW);
        AssertRequest(scene, "ZDN", 17, 12, waitForReturn: request == Jsm.Opcode.REQEW);
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Fact]
    public void TheActivePhaseCanUseADifferentConditionWithoutMergingWithTheInvitation()
    {
        EVContainer field = Read(ReadFixture(Marsh));
        EVContainerScript loop = field.Objects[0].Scripts.Single(script => script.Id == 1);
        EVInstructionBody body = EVInstructionBody.Decode(loop.Body);
        var dialogChoice = new EVExpressionToken((byte)Jsm.Excode.B_SYSVAR, (int)SystemVariable.DialogChoice);
        EVInstruction continuation = body.Instructions
            .SkipWhile(instruction => instruction.Opcode != Jsm.Opcode.MESA || instruction.Operands[3].Value != 361)
            .First(instruction => instruction.Expression?.Contains(dialogChoice) == true);
        continuation.Expression = continuation.Expression.Select(token => token.Equals(dialogChoice) ? Map(EVStorageType.Byte, 23) : token).ToArray();
        loop.Body = body.Encode();

        string project = SourceProjectTest.CopyWithoutComments(Export(field, Marsh, "active-condition"));
        string scene = File.ReadAllText(FindInvitation(project, Prompt));
        string handler = File.ReadAllText(OwnerFile(project));
        Assert.Contains("SystemVariable.DialogChoice", scene);
        Assert.DoesNotContain("Location.Byte_23", scene);
        Assert.Contains("if (Location.Byte_23 == 0)", handler);
        Assert.Contains("messageId: 361)", handler);
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
        Assert.NotEmpty(EventProjectCompiler.CompileManaged(project));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AConversationWithoutAnExclusiveChoiceToStartStaysInItsHandler(bool hasDialogChoice)
    {
        EVContainer field = Read(ReadFixture(Marsh));
        EVContainerScript loop = field.Objects[0].Scripts.Single(script => script.Id == 1);
        EVInstructionBody body = EVInstructionBody.Decode(loop.Body);
        var instructions = body.Instructions.ToList();
        int prompt = instructions.FindIndex(instruction => instruction.Opcode == Jsm.Opcode.MESA &&
            instruction.Operands[3].Value == Prompt);
        Assert.True(prompt > 0);
        var choice = new EVExpressionToken((byte)Jsm.Excode.B_SYSVAR, (int)SystemVariable.DialogChoice);
        int condition = instructions.FindIndex(prompt + 1, instruction => instruction.Expression?.Contains(choice) == true);
        Assert.True(condition > prompt);
        if (hasDialogChoice)
        {
            EVInstruction branch = instructions[condition + 1];
            Assert.Equal(Jsm.Opcode.JMP_IFN, branch.Opcode);
            int declineLabel = Assert.Single(branch.Targets);
            int decline = instructions.FindIndex(condition + 2, instruction => instruction.Label == declineLabel);
            Assert.True(decline > condition);
            EVExpressionToken restore = Map(EVStorageType.Bit, 158);
            EVInstruction restoreOnDecline = instructions.Skip(decline).First(instruction => instruction.Expression is [var target, var value, { Opcode: (byte)Jsm.Excode.Let }] &&
                target.Equals(restore) && value.Equals(EVExpressionToken.Literal(1)));
            restoreOnDecline.Expression = restoreOnDecline.Expression.Select(token => token.Equals(restore) ? Map(EVStorageType.Bit, 613) : token).ToArray();
        }
        else
        {
            EVInstruction comparison = instructions[condition];
            comparison.Expression = comparison.Expression.Select(token => token.Equals(choice) ? Map(EVStorageType.Byte, 23) : token).ToArray();
        }
        loop.Body = body.Encode();

        string project = Export(field, Marsh, "conversation");
        Assert.DoesNotContain(SceneFiles(project), file => File.ReadAllText(file).Contains($"messageId: {Prompt})", StringComparison.Ordinal));
        Assert.Contains($"messageId: {Prompt})", File.ReadAllText(OwnerFile(project)));
        Assert.Equal(field.Serialize(), EventProjectCompiler.Compile(project).Serialize());
    }

    private static (Command[] Commands, int[] Choices) Trace(Assembly assembly, int firstChoice, bool retry = false, bool continueActivity = false)
    {
        var globals = new byte[4096];
        BinaryPrimitives.WriteInt16LittleEndian(globals.AsSpan(0), 5000);
        BinaryPrimitives.WriteInt16LittleEndian(globals.AsSpan(2), 96);
        globals[399] = 1;
        var location = new byte[80];
        var host = new MarshHost(firstChoice, retry);
        var scheduler = new EventScheduler(host, globals, location);
        host.GetFrame = () => scheduler.Frame;
        scheduler.RegisterProject(assembly);
        scheduler.StartScene();

        bool repeated = false;
        bool active = false;
        int? declineFinished = null;
        for (int tick = 0; tick < 500; tick++)
        {
            scheduler.Tick();
            // A level-4 turn occupies Quina as the introduction reaches its level-6 request.
            if (retry && tick == 15)
                Assert.True(scheduler.TryTrigger(Quina, 18, level: 4));
            bool catching = (location[76] & 0x20) != 0;
            if (host.PlayStarted && !active)
            {
                Assert.True(catching);
                Assert.Equal(Quina, scheduler.ControlledActorId);
                if (!continueActivity)
                    return (host.Commands.ToArray(), host.Choices.ToArray());
                active = true;
                host.Choice = 1;
                // The field uses this state to ask whether the player wants to keep catching frogs.
                location[64] = 1;
            }
            if (active && host.Choices.Count == 2 && catching)
                return (host.Commands.ToArray(), host.Choices.ToArray());
            if (host.DeclineRestoredMenu && declineFinished == null)
            {
                Assert.False(catching);
                Assert.NotEqual(Quina, scheduler.ControlledActorId);
                declineFinished = tick;
            }
            if (declineFinished is int restored && tick == restored + 2)
            {
                if (!retry)
                    return (host.Commands.ToArray(), host.Choices.ToArray());
                repeated = scheduler.TryTrigger(0, 1, level: 6);
                Assert.True(repeated);
                host.Choice = 0;
            }
        }
        throw new InvalidOperationException($"Invitation did not finish. Gate={location[64]}, repeat={repeated}, commands={string.Join(", ", host.Commands.TakeLast(12))}");
    }

    private sealed class MarshHost(int choice, bool busy) : IEventHost
    {
        private int _windowClosedAt;
        public Func<int> GetFrame { get; set; } = null!;
        public List<Command> Commands { get; } = new();
        public List<int> Choices { get; } = new();
        public int Choice { get; set; } = choice;
        public bool PlayStarted { get; private set; }
        public bool DeclineRestoredMenu { get; private set; }

        public EventCommandFlow Execute(int objectId, Opcode opcode, ReadOnlySpan<Operand> operands)
        {
            int frame = GetFrame();
            if (busy && objectId == Quina && opcode == Opcode.WAITTURN && frame < 80)
                return EventCommandFlow.Retry;
            if (opcode is Opcode.MESA or Opcode.MESAN && objectId == 0)
            {
                Commands.Add(new(frame, objectId, opcode, (int)operands[3]));
                _windowClosedAt = frame + 2;
            }
            if (opcode is Opcode.MENUOFF or Opcode.MENUON or Opcode.CC or Opcode.AIDLE)
            {
                Commands.Add(new(frame, objectId, opcode, operands.IsEmpty ? 0 : (int)operands[0]));
                PlayStarted |= opcode == Opcode.MENUOFF && objectId == 0 && Choices.Count > 0;
                DeclineRestoredMenu |= opcode == Opcode.MENUON && objectId == 0 && Choices.Count > 0;
            }
            return EventCommandFlow.Continue;
        }

        public int Read(int objectId, Excode opcode, ReadOnlySpan<Operand> operands)
        {
            if (opcode == Excode.IsInParty)
                return (int)operands[0] == 5 ? 1 : 0;
            if (opcode == Excode.B_OBJSPECA && (int)operands[1] == 6)
                return 7;
            if (opcode == Excode.B_SYSVAR && (int)operands[0] == (int)SystemVariable.DialogChoice)
            {
                Assert.True(GetFrame() >= _windowClosedAt);
                Choices.Add(Choice);
                return Choice;
            }
            return 0;
        }

        public bool IsWindowOpen(int objectId, int windowId) => objectId == 0 && windowId == 0 && GetFrame() < _windowClosedAt;
    }

    private sealed record Command(int Frame, int ObjectId, Opcode Opcode, int Value);

    private static EVExpressionToken Map(EVStorageType type, int offset) => new EVVariable(EVStorageScope.Map, type, offset).Encode();

    private static byte[] ReadFixture(string name) => File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", name + ".eb.bytes"));

    private static EVContainer Read(byte[] fixture) => EVContainer.Read(new MemoryStream(fixture));

    private string Export(EVContainer field, string name, string folder, bool extract = true)
    {
        string project = Path.Combine(_directory, folder);
        EventProjectExporter.Export(field, name, project, useCutsceneNames: false, extractCutscenes: extract);
        return project;
    }

    private static string OwnerFile(string project) => Path.Combine(project, "00_Object_00.cs");

    private static IEnumerable<string> SceneFiles(string project)
    {
        string folder = Path.Combine(project, "Cutscenes");
        return Directory.Exists(folder) ? Directory.EnumerateFiles(folder, "*.cs") : [];
    }

    private static string FindInvitation(string project, int prompt) => Assert.Single(SceneFiles(project)
        .Where(file => File.ReadAllText(file).Contains($"messageId: {prompt})", StringComparison.Ordinal)));

    private static void AssertRequest(string scene, string actor, int slot, int tag, bool waitForReturn)
    {
        string target = $"evt.GEO_MAIN_F0_{actor}_{slot:D2}";
        string entryCall = waitForReturn ? "RequestAndWait" : "RequestWhenReady";
        string actionCall = waitForReturn ? "RunAndWait" : "StartWhenReady";
        string direct = $".{entryCall}({target}.Script_{tag:D2}, level: 6)";
        string action = $".{actionCall}(On({target}, Action_{slot:D2}_{tag:D2}), level: 6)";
        Assert.True(scene.Contains(direct, StringComparison.Ordinal) || scene.Contains(action, StringComparison.Ordinal),
            $"Missing request for actor {slot}, tag {tag}, wait for return {waitForReturn}.");
    }

    public void Dispose()
    {
        Directory.Delete(_directory, recursive: true);
    }
}
