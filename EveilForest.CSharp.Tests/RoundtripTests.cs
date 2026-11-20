using EveilForest.CSharp;
using FF8.JSM;
using Memoria.EventEngine.EV;
using Xunit;
using Xunit.Abstractions;
using static EveilForest.CSharp.Tests.SourceProjectTest;

namespace EveilForest.CSharp.Tests;

public sealed class RoundtripTests : IDisposable
{
    private const string Cargo = "EVT_ALEX1_TS_CARGO_0";
    private static readonly string TestData = Path.Combine(AppContext.BaseDirectory, "TestData");
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"evilforest-{Guid.NewGuid():N}");
    private readonly ITestOutputHelper _output;

    public RoundtripTests(ITestOutputHelper output)
    {
        _output = output;
        Directory.CreateDirectory(_directory);
    }

    [Fact]
    public void ContainerAndInstructionCodecAccountForTheFullCorpus()
    {
        string[] files = CorpusFiles();
        var failures = new List<string>();
        foreach (string file in files)
        {
            var result = new CorpusResult(file);
            CheckBinaryStages(result);
            if (result.Failure != null)
                failures.Add(result.Failure);
        }

        Assert.Equal(818, files.Length);
        _output.WriteLine($"Container and instruction identity across {files.Length - failures.Count}/{files.Length} events.");
        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    [Fact]
    public void CargoCliBuildsAndRoundtripsWithAndWithoutComments()
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string project = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, project });
        AddCommentFixture(project);
        CheckCargoCopies(project, "baseline", bytes => Assert.Equal(File.ReadAllBytes(original), bytes));
    }

    [Theory]
    [InlineData("scale")]
    [InlineData("comparison")]
    [InlineData("vertex")]
    [InlineData("rename")]
    [InlineData("named arguments")]
    public void CargoCliEditsAffectOnlyTheRequestedOperand(string edit)
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string project = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, project });

        byte[] expected = File.ReadAllBytes(original);
        EVContainer container = ReadContainer(expected);
        const string initializer = "04_Area_04.cs";
        switch (edit)
        {
            case "comparison":
                ReplaceSource(project, "00_Object_00.cs", "if (Globals.EnvironmentSoundSlot1Id < 0)", "if (Globals.EnvironmentSoundSlot1Id >= 0)");
                EVContainerScript init = Assert.Single(container.Objects[0].Scripts.Where(script => script.Id == 0));
                int comparison = FindPattern(init.Body, new byte[] { 0x05, 0xD8, 0x0B, 0x7D, 0, 0, (byte)Jsm.Excode.LessThan, 0x7F, 0x02 }) + 6;
                expected[init.OriginalOffset + comparison] = (byte)Jsm.Excode.GreatOrEquals;
                break;
            case "scale":
                const string initialCall = "VisualEffects.SetScale(effect: 1, scale: 936, parameter2: 0, parameter3: 0);";
                ReplaceSource(project, "00_Object_00.cs", initialCall, initialCall.Replace("936", "500", StringComparison.Ordinal));
                EVContainerScript scale = Assert.Single(container.Objects[0].Scripts.Where(script => script.Id == 10));
                int offset = FindPattern(scale.Body, new byte[] { 0xB3, 0, 1, 145, 0xA8, 3, 0, 0, 0, 0 }) + 4;
                expected[scale.OriginalOffset + offset] = 0xF4;
                expected[scale.OriginalOffset + offset + 1] = 0x01;
                break;
            case "vertex":
                ReplaceSource(project, initializer, "Point(x: 210, z: 120)", "Point(x: 234, z: 120)");
                EVContainerScript region = Assert.Single(container.Objects[4].Scripts.Where(script => script.Id == 0));
                Assert.Equal(new byte[] { 0x29, 0, 4, 0, 0, 94, 1, 210, 0, 120, 0 }, region.Body.Take(11));
                expected[region.OriginalOffset + 7] = 234;
                break;
            case "rename":
                ReplaceSource(project, initializer, "InRange()", "HandleCargoArea()");
                File.Move(Path.Combine(project, initializer), Path.Combine(project, "renamed-area.cs"));
                break;
            case "named arguments":
                ReplaceSource(project, initializer, "Point(x: 210, z: 120)", "Point(z: 120, x: 210)");
                string model = "13_GEO_SUB_F2_BAK_13.cs";
                ReplaceSource(project, model, "using static EvilForest.EventSource.Vm;", "using Instructions = EvilForest.EventSource.Actors;");
                ReplaceSource(project, model, "        Actors.SetModel(model: Resources.GEO_SUB_F2_BAK, height: 112);",
                    "        Instructions.SetModel(height: 112, model: Resources.GEO_SUB_F2_BAK);");
                // The source contains other intrinsic calls. Keep their static import.
                string file = Path.Combine(project, model);
                File.WriteAllText(file, "using static EvilForest.EventSource.Vm;" + Environment.NewLine + File.ReadAllText(file));
                break;
        }

        CheckCargoCopies(project, edit, bytes =>
        {
            Assert.Equal(expected, bytes);
            EVContainer result = ReadContainer(bytes);
            Assert.Contains(result.Objects[4].Scripts, script => script.Id == 2);
            Assert.Equal(4, result.Objects[4].Scripts[0].Body[2]);
        });
    }

    [Fact]
    public void InvalidEditPreservesExistingDestinationAndAcceptsRetry()
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string project = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, project });
        const string file = "00_Object_00.cs";
        AddCommentFixture(project);
        ReplaceSource(project, file, "scale: 936", "scale: 50000");
        string stripped = CopyWithoutComments(project);
        foreach (string variant in new[] { project, stripped })
        {
            RunDotnet(new[] { "build", ProjectFile(variant), "--nologo", "-v:q" });
            string output = Path.Combine(variant, "existing.eb.bytes");
            byte[] sentinel = { 0xFE, 0xED };
            File.WriteAllBytes(output, sentinel);
            string diagnostic = RunDotnet(new[] { CliPath(), "compile", variant, output }, expectedExit: 1);
            Assert.Contains("Object 0, script 10", diagnostic);
            Assert.Contains("Value 50000 does not fit operand 3 (16 bits)", diagnostic);
            Assert.Equal(sentinel, File.ReadAllBytes(output));

            ReplaceSource(variant, file, "scale: 50000", "scale: 936");
            RunDotnet(new[] { "build", ProjectFile(variant), "--nologo", "-v:q" });
            RunDotnet(new[] { CliPath(), "compile", variant, output });
            Assert.Equal(File.ReadAllBytes(original), File.ReadAllBytes(output));
        }
    }

    [Fact]
    public void EditorScaleEditMatchesTheSourceCompilerAndReopens()
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string source = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, source });
        ReplaceSource(source, "00_Object_00.cs", "scale: 936", "scale: 500");
        string compiled = Path.Combine(_directory, "compiled.eb.bytes");
        RunDotnet(new[] { "build", ProjectFile(source), "--nologo", "-v:q" });
        RunDotnet(new[] { CliPath(), "compile", source, compiled });

        EVEditDocument editor = EVEditDocument.Open(original);
        int directoryIndex = Array.FindIndex(editor.Objects[0].Scripts, script => script.Id == 10);
        EVInstructionBody body = editor.GetBody(0, directoryIndex);
        int scaleIndex = Enumerable.Range(0, body.Instructions.Count).Single(index =>
        {
            EVInstruction instruction = body.Instructions[index];
            return instruction.Opcode == FF8.JSM.Jsm.Opcode.SPS && instruction.Operands[1].Value == 145 && instruction.Operands[2].Value == 936;
        });
        string diagnostic = Assert.Throws<InvalidDataException>(() => editor.SetImmediate(0, directoryIndex, scaleIndex, 2, 50000)).Message;
        ReplaceSource(source, "00_Object_00.cs", "scale: 500", "scale: 50000");
        string compilerError = RunDotnet(new[] { CliPath(), "compile", source, compiled }, expectedExit: 1);
        Assert.Contains(diagnostic, compilerError);
        Assert.Equal(936, editor.GetBody(0, directoryIndex).Instructions[scaleIndex].Operands[2].Value);
        editor.SetImmediate(0, directoryIndex, scaleIndex, 2, 500);
        string edited = Path.Combine(_directory, "editor.eb.bytes");
        Assert.Empty(editor.Save(edited));
        Assert.Equal(File.ReadAllBytes(compiled), File.ReadAllBytes(edited));
        Assert.Equal(500, EVEditDocument.Open(edited).GetBody(0, directoryIndex).Instructions[scaleIndex].Operands[2].Value);
    }

    [Fact]
    public void UnsupportedCallCannotBeIntroducedByAnOpcodeComment()
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string source = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, source });
        AddCommentFixture(source);
        ReplaceSource(source, "04_Area_04.cs", "        Regions.DefinePolygon(", "        EvilForest.EventSource.CommentFixture.Unsupported(); // WAIT(frames: 1)\n        Regions.DefinePolygon(");
        foreach (string variant in new[] { source, CopyWithoutComments(source) })
        {
            RunDotnet(new[] { "build", ProjectFile(variant), "--nologo", "-v:q" });
            string diagnostic = RunDotnet(new[] { CliPath(), "compile", variant, Path.Combine(variant, "invalid.eb.bytes") }, expectedExit: 1);
            Assert.Contains("Unsupported call 'EvilForest.EventSource.CommentFixture.Unsupported()'", diagnostic);
        }
    }

    [Fact]
    public void Map658ReportsAChangeToMemoriasWalkingSpeedAddress()
    {
        string original = Path.Combine(TestData, "EVT_KUINA_KM_SWP_2.eb.bytes");
        string source = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, source });
        string checkpointFile = Directory.GetFiles(source, "18_*.cs").Single();
        string before = File.ReadAllText(checkpointFile);
        Assert.Contains("WatchAddress(323);", before);
        CheckCargoCopies(source, "address checkpoint", bytes => Assert.Equal(File.ReadAllBytes(original), bytes));

        File.WriteAllText(checkpointFile, before.Replace("WatchAddress(323);", "Wait(1, 0);\n        WatchAddress(323);", StringComparison.Ordinal));
        string output = Path.Combine(_directory, "edited.eb.bytes");
        string diagnostics = RunDotnet(new[] { CliPath(), "compile", source, output });
        Assert.Contains("address-sensitive MSPEED moved from object IP 323 to 326", diagnostics);
        byte[] bytes = File.ReadAllBytes(output);
        int offset = 128 + ReadUInt16(bytes, 128 + 18 * 8);
        Assert.Equal(new byte[] { 0x22, 0, 1, 0x26, 0, 30 }, bytes.AsSpan(offset + 320, 6).ToArray());
    }

    [Fact]
    public void SimplifiedExportExpandsExactlyAndRejectsAnIncomingEdge()
    {
        string original = Path.Combine(_directory, "simplification.eb.bytes");
        var header = new byte[128];
        new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
        byte[] eligible = { 0x22, 0x80, 2, 4 };
        byte[] sharedReturn = { 1, 3, 0, 0x22, 0, 1, 4 };
        var slot = new EVContainerObject(0, 0, 0, 0)
        {
            BodyOrder = 0,
            Scripts = new[] { new EVContainerScript(0, eligible), new EVContainerScript(1, sharedReturn) }
        };
        var evt = new EVContainer(header, new[] { slot }, Array.Empty<byte>());
        EVFileWriter.Write(original, evt);
        string source = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, source, "--simplify" });
        string code = File.ReadAllText(Directory.GetFiles(source, "*_*.cs").Single());
        Assert.Contains("WaitThenReturn(frames: 2, mask: 0x80);", code);
        Assert.Contains("Engine.Wait(frames: 1);", code);
        CheckCargoCopies(source, "simplified", bytes =>
        {
            Assert.Equal(File.ReadAllBytes(original), bytes);
            EVContainer rebuilt = ReadContainer(bytes);
            Assert.Equal(WaitTrace.Run(eligible), WaitTrace.Run(rebuilt.Objects[0].Scripts[0].Body));
            Assert.Equal(WaitTrace.Run(sharedReturn), WaitTrace.Run(rebuilt.Objects[0].Scripts[1].Body));
        });
    }

    [Fact]
    public void StructuredExportPreservesIfElseWhileAndSharedSwitchTargets()
    {
        byte[] conditional = Convert.FromHexString("05D6007D0000217F02060022000101030022000222000304");
        byte[] loop = Convert.FromHexString("05D6007D0200187F020B0022000405D600047F01EAFF04");
        byte[] dense = Convert.FromHexString("05D6007F0B0201000F000900090022000501000022000604");
        byte[] explicitTable = Convert.FromHexString("05D6007F060318002A00120005000C000700120022000701060022000801000022000904");
        byte[] externalEntry = new byte[] { 1, 11, 0 }.Concat(conditional).ToArray();
        byte[] tailLoop = Convert.FromHexString("01080022000405D600047F05D6007D0200187F03EDFF04");
        byte[] postTest = tailLoop.Skip(3).ToArray();
        var scripts = new[] { conditional, loop, dense, explicitTable, externalEntry, tailLoop, postTest };
        var header = new byte[128];
        new byte[] { 0x45, 0x56, 2, 1 }.CopyTo(header, 0);
        var slot = new EVContainerObject(0, 1, 0, 0)
        {
            BodyOrder = 0,
            Scripts = scripts.Select((code, index) => new EVContainerScript((ushort)index, code)).ToArray()
        };

        string original = Path.Combine(_directory, "flow.eb.bytes");
        EVFileWriter.Write(original, new EVContainer(header, new[] { slot }, Array.Empty<byte>()));
        byte[] expected = File.ReadAllBytes(original);
        string structured = Path.Combine(_directory, "structured");
        RunDotnet(new[] { CliPath(), "export", original, structured });
        string code = File.ReadAllText(Directory.GetFiles(structured, "*_*.cs").Single());
        Assert.Contains("if (Local.Byte_0 != 0)", code);
        Assert.Contains("else", code);
        Assert.Contains("while (Local.Byte_0 < 2)", code);
        Assert.Contains("switch (Local.Byte_0)", code);
        Assert.Contains("case 1:" + Environment.NewLine + "            case 2:", code);
        Assert.Contains("SwitchTable.Explicit, \"42,5,7\"", code);
        int externalEntryStart = code.IndexOf("[Script(4, 4)]", StringComparison.Ordinal);
        int nextScript = code.IndexOf("[Script(5, 5)]", StringComparison.Ordinal);
        string externallyEnteredBody = code.Substring(externalEntryStart, nextScript - externalEntryStart);
        Assert.Contains("goto ResumeAt", externallyEnteredBody);
        Assert.DoesNotContain("Branch(", externallyEnteredBody);
        Assert.Contains("for (; Local.Byte_0 < 2;)", code);
        Assert.Contains("do" + Environment.NewLine + "        {", code);

        CheckCargoCopies(structured, "structured control flow", bytes =>
        {
            Assert.Equal(expected, bytes);
            EVContainer data = ReadContainer(bytes);
            Assert.Equal(new[] { 1, 3 }, WaitTrace.Run(data.Objects[0].Scripts[0].Body, initialValue: 1));
            Assert.Equal(new[] { 2, 3 }, WaitTrace.Run(data.Objects[0].Scripts[0].Body));
            Assert.Equal(new[] { 4, 4 }, WaitTrace.Run(data.Objects[0].Scripts[1].Body));
            Assert.Equal(new[] { 5, 6 }, WaitTrace.Run(data.Objects[0].Scripts[2].Body, initialValue: 2));
            Assert.Equal(new[] { 8, 9 }, WaitTrace.Run(data.Objects[0].Scripts[3].Body, initialValue: 7));
            Assert.Empty(WaitTrace.Run(data.Objects[0].Scripts[5].Body, initialValue: 2));
            Assert.Equal(new[] { 4 }, WaitTrace.Run(data.Objects[0].Scripts[6].Body, initialValue: 2));
        });

        string flat = Path.Combine(_directory, "flat");
        RunDotnet(new[] { CliPath(), "export", original, flat, "--flat" });
        string flatCode = File.ReadAllText(Directory.GetFiles(flat, "*_*.cs").Single());
        Assert.DoesNotContain("if (", flatCode);
        Assert.DoesNotContain("switch (", flatCode);
        Assert.Contains("Switch(Opcode.JMP_SWITCHEX", flatCode);
        CheckCargoCopies(flat, "explicit control flow", bytes => Assert.Equal(expected, bytes));
    }

    [Fact]
    public void CargoAnnotationsUseTheDescriptorBankAndNeverAffectOutput()
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        byte[] expected = File.ReadAllBytes(original);
        string project = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, project });

        string hero = Directory.GetFiles(project, "17_*.cs").Single();
        string music = Directory.GetFiles(project, "16_*.cs").Single();
        string dialogue = File.ReadAllText(hero).Replace("\r\n", "\n", StringComparison.Ordinal);
        string audio = File.ReadAllText(music).Replace("\r\n", "\n", StringComparison.Ordinal);
        Assert.Matches(@"// Sure is dark\.\.\.\n\s*await Messages\.ShowAndWait\(windowId: 2, flags: 128, messageId: 34\);", dialogue);
        Assert.Matches(@"// - Light the candle\n\s*// - Cancel\n\s*await Messages\.ShowAndWait\(windowId: 2, flags: 128, messageId: 36\);", dialogue);
        Assert.Matches(@"// Strategy Conference[^\n]*\n\s*Audio\.PlaySong\(sound: Resources\.music018\);", audio);
        Assert.DoesNotContain("{Speed", dialogue);

        CheckCargoCopies(project, "dialogue and music", bytes => Assert.Equal(expected, bytes));
        string misleading = Path.Combine(_directory, "misleading");
        RunDotnet(new[] { CliPath(), "export", original, misleading });
        ReplaceSource(misleading, Path.GetFileName(hero), "// Sure is dark...", "// Different message in another bank. WAIT(100).");
        ReplaceSource(misleading, Path.GetFileName(music), "Strategy Conference", "An unrelated title");
        CheckCargoCopies(misleading, "misleading comments", bytes => Assert.Equal(expected, bytes));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void CargoWaitCanBeDeletedOrCopiedInsideTheSwitch(int copies)
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string project = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, project });
        string hero = Directory.GetFiles(project, "17_*.cs").Single();
        string source = File.ReadAllText(hero);
        const string waitCall = "await Engine.Wait(frames: 25);";
        int call = source.IndexOf(waitCall, StringComparison.Ordinal);
        Assert.True(call >= 0);
        string edit = string.Join($"{Environment.NewLine}                    ", Enumerable.Repeat(waitCall, copies));
        File.WriteAllText(hero, source.Remove(call, waitCall.Length).Insert(call, edit));

        EVContainer expected = ReadContainer(File.ReadAllBytes(original));
        EVContainerScript script = expected.Objects[17].Scripts.Single(item => item.Id == 1);
        EVInstructionBody body = EVInstructionBody.Decode(script.Body);
        EVInstruction wait = body.Instructions.First(instruction => instruction.Opcode == Jsm.Opcode.WAIT && instruction.Operands[0].Value == 25);
        Assert.DoesNotContain(body.Instructions, instruction => instruction.Targets.Contains(wait.Label));
        if (copies == 0)
            body.Instructions.Remove(wait);
        else
            body.Instructions.Insert(body.Instructions.IndexOf(wait) + 1, new EVInstruction(-1, wait.Opcode) { ArgumentMask = wait.ArgumentMask, Operands = wait.Operands });
        script.Body = body.Encode();
        CheckCargoCopies(project, "wait edit", bytes => Assert.Equal(expected.Serialize(), bytes));
    }

    [Fact]
    public void CargoSwitchCaseCanBeAddedWithoutChangingLayoutAttributes()
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string project = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, project });
        string hero = Path.GetFileName(Directory.GetFiles(project, "17_*.cs").Single());
        ReplaceSource(project, hero, "case 103:", $"case 104:{Environment.NewLine}            case 103:");

        EVContainer baseline = ReadContainer(File.ReadAllBytes(original));
        CheckCargoCopies(project, "new switch case", bytes =>
        {
            EVContainer result = ReadContainer(bytes);
            for (int slot = 0; slot < result.Objects.Length; slot++)
            {
                foreach (EVContainerScript script in result.Objects[slot].Scripts)
                {
                    if (slot == 17 && script.Id == 0)
                        continue;
                    Assert.Equal(baseline.Objects[slot].Scripts.Single(item => item.Id == script.Id).Body, script.Body);
                }
            }

            EVInstruction dispatch = Assert.Single(EVInstructionBody.Decode(result.Objects[17].Scripts.Single(script => script.Id == 0).Body).Instructions
                .Where(instruction => instruction.Opcode == Jsm.Opcode.JMP_SWITCH));
            Assert.Equal((ushort)102, Assert.Single(dispatch.CaseValues));
            Assert.Equal(4, dispatch.Targets.Length);
            Assert.Equal(dispatch.Targets[2], dispatch.Targets[3]);
            Assert.NotEqual(dispatch.Targets[0], dispatch.Targets[3]);
        });
    }

    [Fact]
    public void AwaitCannotBeDiscardedInCargoSource()
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string project = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, project });
        string hero = Path.GetFileName(Directory.GetFiles(project, "17_*.cs").Single());
        ReplaceSource(project, hero, "await Engine.Wait(frames: 25);", "Engine.Wait(frames: 25);");
        foreach (string variant in new[] { project, CopyWithoutComments(project) })
        {
            RunDotnet(new[] { "build", ProjectFile(variant), "--nologo", "-v:q" });
            string diagnostic = RunDotnet(new[] { CliPath(), "compile", variant, Path.Combine(variant, "invalid.eb.bytes") }, expectedExit: 1);
            Assert.Contains("A suspending operation must be awaited", diagnostic);
        }
    }

    [Fact]
    public void CargoUsesNamedServicesForRequestsVisualsAudioAndMessages()
    {
        string original = Path.Combine(TestData, Cargo + ".eb.bytes");
        string project = Path.Combine(_directory, "source");
        RunDotnet(new[] { CliPath(), "export", original, project });
        string source = string.Join(Environment.NewLine, Directory.GetFiles(project, "*_*.cs").Select(File.ReadAllText));
        Assert.Contains("Scripts.RequestAndWait(level:", source);
        Assert.Contains("Actors.SetModel(model: Resources.GEO_SUB_F2_BAK", source);
        Assert.Contains("VisualEffects.SetScale(effect: 1, scale: 936", source);
        Assert.Contains("Regions.DefinePolygon(4, 0x00", source);
        Assert.Contains("Audio.PlaySong(sound:", source);
        Assert.Contains("Audio.PlayEffect(sound:", source);
        Assert.Contains("Messages.", source);
        Assert.DoesNotContain("Emit(Opcode.", source);
        Assert.Contains("Input.IsKeyPressed(Wide(", source);
        CheckCargoCopies(project, "service names", bytes => Assert.Equal(File.ReadAllBytes(original), bytes));
    }

    [Fact]
    public void StructuredIfAndCommonContinuationFollowTheVmBranchPolarity()
    {
        string source = Path.Combine(_directory, "source");
        Directory.CreateDirectory(source);
        File.WriteAllText(ProjectFile(source), """<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net8.0</TargetFramework></PropertyGroup></Project>""");
        File.WriteAllText(Path.Combine(source, "Support.cs"), """
            public sealed class TimerService
            {
                public object Wait(int frames) => new object();
            }
            public sealed class ServiceId
            {
                public static ServiceId System { get; } = new ServiceId();
                public TimerService this[object ctx] => new TimerService();
            }
            """);
        File.WriteAllText(Path.Combine(source, "00_Flow.cs"), """
            public sealed class Flow
            {
                public byte Byte_0 { get; set; }
                public byte Byte_1 { get; set; }
                public System.Collections.Generic.IEnumerable<object> Init(object ctx)
                {
                    var sys = ServiceId.System[ctx];
                    if (Byte_0 != 0)
                        yield return sys.Wait(1); // misleading: WAIT(99)
                    yield return sys.Wait(2);
                    yield break;
                }

                public System.Collections.Generic.IEnumerable<object> Script_01(object ctx)
                {
                    var sys = ServiceId.System[ctx];
                    while (Byte_0 < 2)
                    {
                        Byte_1 = 0;
                        while (Byte_1 < 2)
                        {
                            yield return sys.Wait(3);
                            Byte_1++;
                        }
                        Byte_0++;
                    }
                    yield break;
                }

                public System.Collections.Generic.IEnumerable<object> Script_02(object ctx)
                {
                    var sys = ServiceId.System[ctx];
                    switch (Byte_0)
                    {
                        case 1:
                        case 2:
                            yield return sys.Wait(4);
                            yield return sys.Wait(5);
                            break;
                        default:
                            yield return sys.Wait(6);
                            yield return sys.Wait(7);
                            break;
                    }
                    yield return sys.Wait(8);
                    yield break;
                }

                public System.Collections.Generic.IEnumerable<object> Script_03(object ctx)
                {
                    var sys = ServiceId.System[ctx];
                    if (Byte_0 != 0)
                    {
                        yield return sys.Wait(1);
                        yield return sys.Wait(2);
                    }
                    else
                    {
                        yield return sys.Wait(3);
                        yield return sys.Wait(4);
                    }
                    yield return sys.Wait(5);
                    yield break;
                }
            }
            """);
        foreach (string variant in new[] { source, CopyWithoutComments(source) })
        {
            RunDotnet(new[] { "build", ProjectFile(variant), "--nologo", "-v:q" });
            EVObject obj = Assert.Single(new CSharpEventCompiler().CompileDirectory(variant));
            byte[] Code(int id) => obj.Scripts.Single(script => script.Id == id).Instructions.Encode();
            Assert.Equal(new[] { 1, 2 }, WaitTrace.Run(Code(0), initialValue: 1));
            Assert.Equal(new[] { 2 }, WaitTrace.Run(Code(0), initialValue: 0));
            Assert.Equal(new[] { 3, 3, 3, 3 }, WaitTrace.Run(Code(1)));
            Assert.Equal(new[] { 4, 5, 8 }, WaitTrace.Run(Code(2), initialValue: 1));
            Assert.Equal(new[] { 4, 5, 8 }, WaitTrace.Run(Code(2), initialValue: 2));
            Assert.Equal(new[] { 6, 7, 8 }, WaitTrace.Run(Code(2), initialValue: 3));
            Assert.Equal(new[] { 1, 2, 5 }, WaitTrace.Run(Code(3), initialValue: 1));
            Assert.Equal(new[] { 3, 4, 5 }, WaitTrace.Run(Code(3), initialValue: 0));
        }
    }

    [Fact]
    public void FailedContainerSerializationPreservesExistingDestination()
    {
        string output = Path.Combine(_directory, "existing.eb.bytes");
        byte[] sentinel = { 0xFE, 0xED };
        File.WriteAllBytes(output, sentinel);
        byte[] original = File.ReadAllBytes(Path.Combine(TestData, Cargo + ".eb.bytes"));
        EVContainer container = ReadContainer(original);
        EVContainerScript script = container.Objects[0].Scripts[0];
        byte[] originalBody = script.Body;
        script.Body = new byte[ushort.MaxValue + 1];

        Assert.Throws<InvalidDataException>(() => EVFileWriter.Write(output, container));
        Assert.Equal(sentinel, File.ReadAllBytes(output));
        script.Body = originalBody;
        EVFileWriter.Write(output, container);
        Assert.Equal(original, File.ReadAllBytes(output));
        Assert.Empty(Directory.EnumerateFiles(_directory, "*.tmp"));
    }

    [Fact]
    public void ContainerResizesOneScriptAndRelocatesItsNeighbors()
    {
        byte[] original = File.ReadAllBytes(Path.Combine(TestData, Cargo + ".eb.bytes"));
        EVContainer container = ReadContainer(original);
        EVContainerObject obj = container.Objects[0];
        EVContainerScript first = obj.Scripts[0];
        EVContainerScript second = obj.Scripts[1];
        int objectOffset = 128 + ReadUInt16(original, 128);
        int nextObjectOffset = 128 + ReadUInt16(original, 136);
        int size = ReadUInt16(original, 130);
        first.Body = first.Body.Concat(new byte[] { 0, 0, 0 }).ToArray();

        byte[] edited = container.Serialize();
        Assert.Equal(size + 3, ReadUInt16(edited, 130));
        Assert.Equal(nextObjectOffset + 3, 128 + ReadUInt16(edited, 136));
        Assert.Equal(second.OriginalOffset + 3, objectOffset + 2 + ReadUInt16(edited, objectOffset + 8));
        Assert.Equal(second.Body, edited.Skip(second.OriginalOffset + 3).Take(second.Body.Length));
        Assert.Equal(original.Skip(nextObjectOffset), edited.Skip(nextObjectOffset + 3));
    }

    [Fact]
    [Trait("Category", "FullCorpus")]
    public void EveryProductionExportBuildsAndRecompilesTwice()
    {
        if (Environment.GetEnvironmentVariable("EVILFOREST_FULL_CORPUS") != "1")
        {
            _output.WriteLine("Set EVILFOREST_FULL_CORPUS=1 to run the 1,636 clean SDK builds.");
            return;
        }

        string[] files = CorpusFiles();
        var results = files.Select(file => new CorpusResult(file)).ToArray();
        int workers = int.TryParse(Environment.GetEnvironmentVariable("EVILFOREST_CORPUS_WORKERS"), out int parallelism)
            ? Math.Max(1, parallelism) : Math.Min(6, Environment.ProcessorCount);
        Parallel.ForEach(results, new ParallelOptions { MaxDegreeOfParallelism = workers }, RunFullRoundtrip);
        foreach (CorpusResult result in results)
            _output.WriteLine(result.ToString());

        string report = Path.Combine(_directory, "corpus.tsv");
        File.WriteAllLines(report, new[] { "event\tcontainer\tinstructions\texport\tsdk\timport\tbytes\tsdk (comment-free)\timport (comment-free)\tbytes (comment-free)\tfailure" }
            .Concat(results.Select(result => result.ToString())));
        string? reportDestination = Environment.GetEnvironmentVariable("EVILFOREST_CORPUS_REPORT");
        if (!string.IsNullOrEmpty(reportDestination))
            File.Copy(report, reportDestination, overwrite: true);

        Assert.Equal(818, results.Length);
        string[] failures = results.Where(result => result.Failure != null).Select(result => result.Failure!).ToArray();
        Assert.True(failures.Length == 0, string.Join(Environment.NewLine, failures));
    }

    private void RunFullRoundtrip(CorpusResult result)
    {
        EVContainer? container = CheckBinaryStages(result);
        if (container == null)
            return;

        string directory = Path.Combine(_directory, result.Name);
        try
        {
            RunStage(result, 2, () => EventProjectExporter.Export(container, result.Name, directory));
            string stripped = CopyWithoutComments(directory);
            CheckVariant(directory, 3);
            CheckVariant(stripped, 6);
        }
        catch (Exception ex)
        {
            result.Failure ??= $"{result.Name}: {ex.Message}";
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
            if (Directory.Exists(directory + "-stripped"))
                Directory.Delete(directory + "-stripped", recursive: true);
        }

        void CheckVariant(string project, int stage)
        {
            try
            {
                RunStage(result, stage, () => RunDotnet(new[] { "build", ProjectFile(project), "--nologo", "-v:q", "-m:1" }));
                EVContainer? compiled = null;
                RunStage(result, stage + 1, () => compiled = EventProjectCompiler.Compile(project));
                RunStage(result, stage + 2, () => CompareBytes(container, File.ReadAllBytes(result.File), compiled!.Serialize()));
            }
            catch (Exception)
            {
                // Keep the first failed stage and continue with the other variant.
            }
        }
    }

    private static EVContainer? CheckBinaryStages(CorpusResult result)
    {
        try
        {
            byte[] original = File.ReadAllBytes(result.File);
            EVContainer? container = null;
            RunStage(result, 0, () =>
            {
                container = ReadContainer(original);
                CompareBytes(container, original, container.Serialize());
            });
            RunStage(result, 1, () =>
            {
                foreach (EVContainerObject obj in container!.Objects)
                {
                    foreach (EVContainerScript script in obj.Scripts)
                    {
                        try
                        {
                            EVInstructionBody body = EVInstructionBody.Decode(script.Body);
                            body.ValidateObjectSlots(container.Objects.Length);
                            byte[] actual = body.Encode();
                            int diff = FirstDifference(script.Body, actual);
                            if (diff >= 0)
                                throw new InvalidDataException($"offset 0x{diff:X}: {DescribeDifference(script.Body, actual, diff)}");
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidDataException($"object {obj.Id}, script {script.Id}, file offset 0x{script.OriginalOffset:X}: {ex.Message}", ex);
                        }
                    }
                }
            });
            return container;
        }
        catch (Exception ex)
        {
            result.Failure ??= $"{result.Name}: {ex.Message}";
            return null;
        }
    }

    private static void RunStage(CorpusResult result, int stage, Action action)
    {
        try
        {
            action();
            result.Stages[stage] = "pass";
        }
        catch (Exception ex)
        {
            result.Stages[stage] = "fail";
            result.Failure ??= $"{result.Name}, {CorpusResult.StageNames[stage]}: {ex.Message}";
            throw;
        }
    }

    private void CheckCargoCopies(string source, string edit, Action<byte[]> check)
    {
        string stripped = CopyWithoutComments(source);
        byte[]? original = null;
        foreach (string variant in new[] { source, stripped })
        {
            RunDotnet(new[] { "build", ProjectFile(variant), "--nologo", "-v:q" });
            string output = Path.Combine(variant, "compiled.eb.bytes");
            RunDotnet(new[] { CliPath(), "compile", variant, output });
            byte[] bytes = File.ReadAllBytes(output);
            check(bytes);
            if (original != null)
                Assert.Equal(original, bytes);
            original = bytes;
            _output.WriteLine($"{edit}: {Path.GetFileName(variant)} passed SDK and EV compilation.");
        }
    }

    private static void AddCommentFixture(string source)
    {
        File.AppendAllText(Path.Combine(source, "CommentFixture.cs"), """

            #if true // WIPERGB(unknown: 255)
            /// <summary>FLDSND(op: 65535, sound: 0)</summary>
            namespace EvilForest.EventSource;
            internal static class CommentFixture
            {
                public const string Markers = "/* keep this */ and // keep this";
                public const int Boundaries = 1/**/+2;
                public static void Unsupported() { }
            }
            #endif
            """);
        ReplaceSource(source, "00_Object_00.cs", "scale: 936,", "scale: 936, /* SPS(1, SetScale, 500, 0, 0) */");
    }

    private static void CompareBytes(EVContainer originalContainer, byte[] expected, byte[] actual)
    {
        int offset = FirstDifference(expected, actual);
        if (offset < 0)
            return;

        string context = offset < 128 ? "file header" : "object bodies or padding";
        if (offset >= 128 && offset < 128 + originalContainer.Objects.Length * 8)
        {
            int entry = (offset - 128) % 8;
            string field = entry switch { < 2 => "offset", < 4 => "size", 4 => "variable count", 5 => "flags", _ => "reserved" };
            context = $"object {(offset - 128) / 8}, table field {field}";
        }
        foreach (EVContainerObject obj in originalContainer.Objects)
        {
            foreach (EVContainerScript script in obj.Scripts)
            {
                int relative = offset - script.OriginalOffset;
                if (relative < 0 || relative >= script.Body.Length)
                    continue;
                EVInstruction? instruction = EVInstructionBody.Decode(script.Body).Instructions
                    .LastOrDefault(candidate => candidate.Label <= relative);
                context = $"object {obj.Id}, script {script.Id}, instruction {instruction?.Opcode} at +0x{instruction?.Label:X}";
            }
        }
        throw new InvalidDataException($"{context}, offset 0x{offset:X}: {DescribeDifference(expected, actual, offset)}");
    }

    private static string DescribeDifference(byte[] expected, byte[] actual, int offset)
    {
        static string At(byte[] bytes, int start) => start >= bytes.Length ? "<end>" : Convert.ToHexString(bytes.AsSpan(start, Math.Min(8, bytes.Length - start)));
        return $"expected {At(expected, offset)} ({expected.Length} bytes), got {At(actual, offset)} ({actual.Length} bytes)";
    }

    private static int FirstDifference(byte[] expected, byte[] actual)
    {
        for (int i = 0; i < Math.Min(expected.Length, actual.Length); i++)
        {
            if (expected[i] != actual[i])
                return i;
        }
        return expected.Length == actual.Length ? -1 : Math.Min(expected.Length, actual.Length);
    }

    private static int FindPattern(byte[] data, byte[] pattern)
    {
        int index = data.AsSpan().IndexOf(pattern);
        Assert.True(index >= 0, $"Could not find {Convert.ToHexString(pattern)}.");
        Assert.True(data.AsSpan(index + pattern.Length).IndexOf(pattern) < 0, "Expected a unique binary match.");
        return index;
    }

    private static void ReplaceSource(string directory, string file, string oldText, string newText)
    {
        string path = Path.Combine(directory, file);
        string text = File.ReadAllText(path);
        Assert.Contains(oldText, text);
        File.WriteAllText(path, text.Replace(oldText, newText, StringComparison.Ordinal));
    }

    private static string[] CorpusFiles() => Directory.GetFiles(TestData, "*.eb.bytes", SearchOption.AllDirectories).OrderBy(file => file, StringComparer.Ordinal).ToArray();

    private static ushort ReadUInt16(byte[] data, int offset) => System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(offset, 2));

    private static EVContainer ReadContainer(byte[] data) => EVContainer.Read(new MemoryStream(data, writable: false));

    private static string CliPath() => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..",
        "EvilForest", "bin", new DirectoryInfo(AppContext.BaseDirectory).Parent!.Name, "net10.0", "EvilForest.dll"));

    public void Dispose()
    {
        if (Environment.GetEnvironmentVariable("EVILFOREST_KEEP_TEST_OUTPUT") == "1")
            _output.WriteLine($"Kept generated files in {_directory}.");
        else
            Directory.Delete(_directory, recursive: true);
    }

    private sealed class CorpusResult(string file)
    {
        public static readonly string[] StageNames = { "container", "instructions", "export", "sdk", "import", "bytes", "sdk (comment-free)", "import (comment-free)", "bytes (comment-free)" };
        public string File { get; } = file;
        public string Name { get; } = Path.GetFileName(file).Replace(".eb.bytes", "", StringComparison.Ordinal);
        public string[] Stages { get; } = Enumerable.Repeat("-", StageNames.Length).ToArray();
        public string? Failure { get; set; }

        public override string ToString() => $"{Name}\t{string.Join("\t", Stages)}\t{Failure?.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ')}";
    }
}
