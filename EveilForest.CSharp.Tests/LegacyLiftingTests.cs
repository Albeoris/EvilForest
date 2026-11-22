using Memoria.EventEngine.EV;
using Xunit;
using Xunit.Abstractions;

namespace EveilForest.CSharp.Tests;

public sealed class LegacyLiftingTests(ITestOutputHelper output)
{
    [Fact]
    public void FlatDecodingPreservesScriptsThatTheLegacyLifterRejects()
    {
        string directory = Path.Combine(AppContext.BaseDirectory, "TestData");
        string[] names =
        {
            "EVT_CHOCO_CH_FGD_0", "EVT_CHOCO_CH_FST_0", "EVT_CHOCO_CH_HLG_0", "EVT_DALI_V_DL_FWM",
            "EVT_EVA1_IF_CST_0", "EVT_GATE_N_NG_BDA_0", "EVT_GATE_N_NG_BDA_1", "EVT_GATE_N_NG_TRA_1",
            "EVT_KUWAN_QH_LNI_0", "EVT_LIND1_TN_LB_LDH_0", "EVT_OEIL_UV_DEP_0", "EVT_PINA_PR_PW2_0",
            "EVT_PINA_PR_PW3_0", "EVT_SARI1_MS_ENT_0", "EVT_SARI2_MS_ENT_1", "EVT_SHRINE_ES_ENT_1"
        };
        foreach (string name in names)
        {
            bool rejected = false;
            using var file = File.OpenRead(Path.Combine(directory, name + ".eb.bytes"));
            EVContainer evt = EVContainer.Read(file);
            foreach (EVContainerObject obj in evt.Objects)
            {
                foreach (EVContainerScript script in obj.Scripts)
                {
                    try
                    {
                        EVFileReader.ParseScript(script.Body);
                    }
                    catch (Exception ex)
                    {
                        EVInstructionBody flat = EVInstructionBody.Decode(script.Body);
                        Assert.Equal(script.Body, flat.Encode());
                        rejected = true;
                        const string jumpFailure = "Invalid jump target: ";
                        if (ex.Message.StartsWith(jumpFailure, StringComparison.Ordinal))
                        {
                            int target = int.Parse(ex.Message.Substring(jumpFailure.Length));
                            EVInstruction instruction = Assert.Single(flat.Instructions.Where(instruction => instruction.Label == target));
                            Assert.Contains(flat.Instructions, source => source.Targets.Contains(target));
                            string bytes = Convert.ToHexString(script.Body.AsSpan(target, instruction.OriginalLength));
                            output.WriteLine($"{name}, object {obj.Id}, script {script.Id}, file offset 0x{script.OriginalOffset + target:X}, body offset 0x{target:X}: lost {instruction.Opcode} target {bytes}");
                        }
                        else
                        {
                            int offset = Assert.IsType<int>(ex.Data["ScriptOffset"]);
                            EVInstruction instruction = flat.Instructions.Last(item => item.Label < offset);
                            string bytes = Convert.ToHexString(script.Body.AsSpan(instruction.Label, instruction.OriginalLength));
                            output.WriteLine($"{name}, object {obj.Id}, script {script.Id}, file offset 0x{script.OriginalOffset + instruction.Label:X}, body offset 0x{instruction.Label:X}: {ex.GetType().Name} {ex.Message}, {bytes}");
                        }
                    }
                }
            }
            Assert.True(rejected, $"Expected a legacy lifting failure for {name}.");
        }
    }

    [Fact]
    public void AJumpToTheFirstOfTwoExpressionsPreservesTheTarget()
    {
        byte[] bytes = Convert.FromHexString("01010004057D01007F05D4007D01002C7F04");
        EVInstructionBody body = EVInstructionBody.Decode(bytes);
        Assert.Equal(4, Assert.Single(body.Instructions[0].Targets));
        Assert.Equal(0x05, (int)body.Instructions.Single(item => item.Label == 4).Opcode);
        Assert.Equal(bytes, body.Encode());
        Assert.Throws<InvalidProgramException>(() => EVFileReader.ParseScript(bytes));
    }
    [Fact]
    public void BooleanAndAssignmentTokenSequenceRemainsAvailableWhenLiftingFails()
    {
        byte[] bytes = Convert.FromHexString("05D11E7D000020D200277D00002C7F04");
        Assert.Throws<InvalidCastException>(() => EVFileReader.ParseScript(bytes));
        Assert.Equal(bytes, EVInstructionBody.Decode(bytes).Encode());
    }
}
