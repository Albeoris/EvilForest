using System.Reflection;
using EvilForest.EventSource;
using EvilForest.Resources;
using EvilForest.Resources.Enums;
using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal static class CharacterEffectSource
{
    private static readonly IReadOnlyDictionary<int, MethodInfo> Operations = typeof(ActorAnimation).GetMethods()
        .Where(method => method.IsDefined(typeof(CharacterEffectAttribute)))
        .ToDictionary(method => (int)method.GetCustomAttribute<CharacterEffectAttribute>()!.Command);

    public static string? Format(EVInstruction instruction, string receiver, Func<EVOperand, string> formatOperand)
    {
        if (instruction.Opcode != Jsm.Opcode.CHRFX || instruction.Operands[0].Expression != null ||
            !Operations.TryGetValue(instruction.Operands[0].Value, out MethodInfo? method))
            return null;

        ParameterInfo[] parameters = method.GetParameters();
        var arguments = new List<string>();
        int semanticCount = parameters.Length - 1;
        for (int index = 0; index < semanticCount; index++)
        {
            ParameterInfo parameter = parameters[index];
            EVOperand operand = instruction.Operands[index + 1];
            string text = FormatDomain(parameter.ParameterType, operand, formatOperand);
            arguments.Add($"{parameter.Name}: {text}");
        }

        var encoding = new List<string>();
        for (int index = semanticCount + 1; index < instruction.Operands.Length; index++)
        {
            EVOperand unused = instruction.Operands[index];
            if (unused.Expression != null || unused.Value != 0)
                encoding.Add($"unused{index - semanticCount}: {FormatInteger(unused, formatOperand)}");
        }
        if (instruction.ArgumentMask != SourceInstruction.InferMask(instruction.Opcode, instruction.Operands))
            encoding.Add($"mask: 0x{instruction.ArgumentMask:X2}");
        if (encoding.Count > 0)
            arguments.Add($"encoding: new CharacterEffectEncoding({string.Join(", ", encoding)})");
        return $"{receiver}.{method.Name}({string.Join(", ", arguments)})";
    }

    private static string FormatDomain(Type domain, EVOperand operand, Func<EVOperand, string> formatOperand)
    {
        string integer = FormatInteger(operand, formatOperand);
        if (domain == typeof(int))
            return integer;
        if (domain == typeof(FieldBlendMode))
            return operand.Expression == null && Enum.IsDefined((FieldBlendMode)operand.Value)
                ? $"FieldBlendMode.{(FieldBlendMode)operand.Value}" : $"(FieldBlendMode)({integer})";
        if (domain == typeof(ScriptFlag))
        {
            if (operand.Expression == null && operand.Value is 0 or 1)
                return operand.Value == 0 ? "false" : "true";
            return $"ScriptFlag.FromScript({integer})";
        }
        if (operand.Expression != null)
            return $"{domain.Name}.FromScript({integer})";
        int value = operand.Value;
        if (domain == typeof(AnimationId) && DB.Animations.TryGetValue((DBAnimation.Id)value, out DBAnimation? animation))
        {
            if (value == Resources.ANH_MAIN_F0_ZDN_RUN)
                return "Animations.Zidane.Run";
            string member = ResourceCatalog.MemberName(animation.FileName, value);
            if (HasResource("AnimationIds", member))
                return $"AnimationIds.{member}";
            integer = ResourceCatalog.Qualify(animation.FileName, value);
        }
        if (domain == typeof(SoundEffectId) && DB.SoundEffects.TryGetValue((DBSfx.Id)value, out DBSfx? sound))
        {
            string member = ResourceCatalog.SoundMember(sound.FileName, value);
            if (HasResource("Sounds", member))
                return $"Sounds.{member}";
            integer = ResourceCatalog.Qualify(sound.FileName, value);
        }
        return operand.Value is >= 0 and <= ushort.MaxValue ? $"new {domain.Name}({integer})" : $"{domain.Name}.FromScript({integer})";
    }

    private static bool HasResource(string catalog, string member) =>
        typeof(AnimationId).Assembly.GetType($"EvilForest.EventSource.{catalog}")?.GetProperty(member) != null;

    private static string FormatInteger(EVOperand operand, Func<EVOperand, string> formatOperand) =>
        operand.Expression == null ? formatOperand(operand) : $"(int)({formatOperand(operand)})";
}
