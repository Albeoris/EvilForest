using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using EvilForest.Resources.Enums;
using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed class SourceExpression(Jsm.Excode opcode, string service, string method, params string[] parameters)
{
    public Jsm.Excode Opcode { get; } = opcode;
    public string Service { get; } = service;
    public string Method { get; } = method;
    public string[] Parameters { get; } = parameters;
    public bool Statement { get; init; }
    public ExpressionPayload Payload { get; init; }
    private ValueKind Result => Statement ? ValueKind.Operand : Opcode == Jsm.Excode.B_NOT ? ValueKind.Bool : ValueKind.Int;

    private static readonly SourceExpression[] Operations =
    {
        new(Jsm.Excode.IncrementPost, "Variables", "PostIncrement", "target") { Statement = true },
        new(Jsm.Excode.DecrementPost, "Variables", "PostDecrement", "target") { Statement = true },
        new(Jsm.Excode.B_PRE_PLUS, "Variables", "PreIncrement", "target") { Statement = true },
        new(Jsm.Excode.B_PRE_MINUS, "Variables", "PreDecrement", "target") { Statement = true },
        new(Jsm.Excode.Let, "Variables", "Set", "target", "value") { Statement = true },
        new(Jsm.Excode.MulLet, "Variables", "MultiplyBy", "target", "value") { Statement = true },
        new(Jsm.Excode.DivLet, "Variables", "DivideBy", "target", "value") { Statement = true },
        new(Jsm.Excode.ModeLet, "Variables", "KeepRemainder", "target", "divisor") { Statement = true },
        new(Jsm.Excode.AddLet, "Variables", "AddTo", "target", "value") { Statement = true },
        new(Jsm.Excode.SubLet, "Variables", "SubtractFrom", "target", "value") { Statement = true },
        new(Jsm.Excode.BitLeftLet, "Variables", "ShiftLeft", "target", "bits") { Statement = true },
        new(Jsm.Excode.BitRightLet, "Variables", "ShiftRight", "target", "bits") { Statement = true },
        new(Jsm.Excode.BitAndLet, "Variables", "KeepBits", "target", "mask") { Statement = true },
        new(Jsm.Excode.BitXorLet, "Variables", "ToggleBits", "target", "mask") { Statement = true },
        new(Jsm.Excode.BitOrLet, "Variables", "SetBits", "target", "mask") { Statement = true },
        new(Jsm.Excode.B_SINGLE_PLUS, "Values", "ReuseLastCalculation", "value"),
        new(Jsm.Excode.B_SINGLE_MINUS, "Values", "Negate", "value"),
        new(Jsm.Excode.B_NOT, "Values", "NotPositive", "value"),
        new(Jsm.Excode.B_COMP, "Values", "InvertBits", "value"),
        new(Jsm.Excode.Mul, "Values", "Multiply", "left", "right"),
        new(Jsm.Excode.Div, "Values", "Divide", "dividend", "divisor"),
        new(Jsm.Excode.Mod, "Values", "Remainder", "dividend", "divisor"),
        new(Jsm.Excode.Add, "Values", "Add", "left", "right"),
        new(Jsm.Excode.Sub, "Values", "Subtract", "left", "right"),
        new(Jsm.Excode.BitLeft, "Values", "ShiftLeft", "value", "bits"),
        new(Jsm.Excode.BitRight, "Values", "ShiftRight", "value", "bits"),
        new(Jsm.Excode.LessThan, "Values", "LessThan", "left", "right"),
        new(Jsm.Excode.GreatThan, "Values", "GreaterThan", "left", "right"),
        new(Jsm.Excode.LessOrEquals, "Values", "AtMost", "left", "right"),
        new(Jsm.Excode.GreatOrEquals, "Values", "AtLeast", "left", "right"),
        new(Jsm.Excode.Equivalence, "Values", "Equal", "left", "right"),
        new(Jsm.Excode.NotEquivalence, "Values", "NotEqual", "left", "right"),
        new(Jsm.Excode.BitAnd, "Values", "BitAnd", "left", "right"),
        new(Jsm.Excode.BitXor, "Values", "BitXor", "left", "right"),
        new(Jsm.Excode.BitOr, "Values", "BitOr", "left", "right"),
        new(Jsm.Excode.BooleanAnd, "Values", "And", "left", "right"),
        new(Jsm.Excode.BooleanOr, "Values", "Or", "left", "right"),
        new(Jsm.Excode.IsKeyPressed, "Input", "IsKeyPressed", "buttons"),
        new(Jsm.Excode.B_KEYOFF, "Input", "IsKeyReleased", "buttons"),
        new(Jsm.Excode.B_KEY, "Input", "IsKeyHeld", "buttons"),
        new(Jsm.Excode.B_SIN2, "Values", "Sin", "angle"),
        new(Jsm.Excode.B_COS2, "Values", "Cos", "angle"),
        new(Jsm.Excode.B_SIN, "Values", "SinScaledAngle", "angle"),
        new(Jsm.Excode.B_COS, "Values", "CosScaledAngle", "angle"),
        new(Jsm.Excode.GetActorAngle, "Actors", "AngleToPosition", "x", "z"),
        new(Jsm.Excode.GetActorDistance, "Actors", "DistanceToPosition", "x", "z"),
        new(Jsm.Excode.B_ANGLEA, "Actors", "AngleToActor", "targetUid"),
        new(Jsm.Excode.B_DISTANCEA, "Actors", "DistanceToActor", "targetUid"),
        new(Jsm.Excode.GetAngle, "Values", "AngleOfVector", "x", "z"),
        new(Jsm.Excode.B_HAVE_ITEM, "Inventory", "Count", "itemId"),
        new(Jsm.Excode.B_BAFRAME, "Regions", "GetWalkmeshAnimationFrame", "animationId"),
        new(Jsm.Excode.B_FRAME, "Animations", "GetLength", "animation"),
        new(Jsm.Excode.IsInParty, "Party", "Contains", "character"),
        new(Jsm.Excode.AddToParty, "Party", "TryAdd", "character"),
        new(Jsm.Excode.GetCurrentHP, "Party", "CurrentHp", "character"),
        new(Jsm.Excode.GetMaxHP, "Party", "MaximumHp", "character"),
        new(Jsm.Excode.GetCurrentMP, "Party", "CurrentMp", "character"),
        new(Jsm.Excode.GetMaxMP, "Party", "MaximumMp", "character"),
        new(Jsm.Excode.B_BGIID, "Regions", "TriangleOf", "actorUid"),
        new(Jsm.Excode.B_BGIFLOOR, "Regions", "FloorOf", "actorUid"),
        new(Jsm.Excode.B_SPS, "VisualEffects", "QuerySprite", "effect", "property"),
        new(Jsm.Excode.B_PTR, "Actors", "ResolveUid", "targetUid") { Payload = ExpressionPayload.Byte },
        new(Jsm.Excode.B_MEMBER, "Actors", "ReadMemberProperty", "property") { Payload = ExpressionPayload.Byte },
        new(Jsm.Excode.B_OBJSPECA, "Actors", "ReadProperty", "targetUid", "property") { Payload = ExpressionPayload.ObjectProperty },
        new(Jsm.Excode.B_SYSLIST, "Engine", "ReadSystemList", "slot") { Payload = ExpressionPayload.Byte },
        new(Jsm.Excode.B_SYSVAR, "Engine", "ReadSystem", "variable") { Payload = ExpressionPayload.SystemVariable }
    };

    private static readonly IReadOnlyDictionary<Jsm.Excode, SourceExpression> ByOpcode = Operations.ToDictionary(operation => operation.Opcode);

    public static bool TryGet(Jsm.Excode opcode, [NotNullWhen(true)] out SourceExpression? operation) =>
        ByOpcode.TryGetValue(opcode, out operation);

    private enum ValueKind { Operand, Int, Bool }

    private readonly record struct Printed(string Text, ValueKind Kind, int? Literal = null, bool Storage = false, bool Computed = false)
    {
        public Printed AsFlag() => Literal is 0 or 1 ? new Printed(Literal == 0 ? "false" : "true", ValueKind.Bool) : this;
    }

    public static bool TryFormat(IReadOnlyList<EVExpressionToken> tokens, StorageDeclarations storage, bool statement, [NotNullWhen(true)] out string? result) =>
        TryFormat(tokens, storage, statement, out result, out _, asOperand: true);

    public static string? FormatInteger(IReadOnlyList<EVExpressionToken> tokens, StorageDeclarations storage)
    {
        if (!TryFormat(tokens, storage, statement: false, out string? text, out ValueKind kind, asOperand: false))
            return null;
        return kind == ValueKind.Int ? text : kind == ValueKind.Bool ? $"(({text}) ? 1 : 0)" : $"(int)({text})";
    }

    public static bool TryFormatCondition(IReadOnlyList<EVExpressionToken> tokens, StorageDeclarations storage, [NotNullWhen(true)] out string? result)
    {
        if (!TryFormat(tokens, storage, statement: false, out string? value, out ValueKind kind, asOperand: false) || kind == ValueKind.Operand)
        {
            result = null;
            return false;
        }
        if (value.StartsWith('(') && value.EndsWith(')'))
            value = value[1..^1];
        result = kind == ValueKind.Bool ? value : tokens.Count == 1 && tokens[0].Opcode == (byte)Jsm.Excode.Const16 ? (tokens[0].Value == 0 ? "false" : tokens[0].Value == 1 ? "true" : $"({value}) is not 0") : $"({value}) is not 0";
        return true;
    }

    private static bool TryFormat(IReadOnlyList<EVExpressionToken> tokens, StorageDeclarations storage, bool statement, [NotNullWhen(true)] out string? result, out ValueKind kind, bool asOperand)
    {
        var stack = new Stack<Printed>();
        kind = ValueKind.Operand;
        for (int index = 0; index < tokens.Count; index++)
        {
            EVExpressionToken token = tokens[index];
            if (token.Opcode == (byte)Jsm.Excode.Const16)
            {
                stack.Push(new Printed(Number(token.Value), ValueKind.Int, token.Value, Computed: true));
                continue;
            }
            if (token.Opcode == (byte)Jsm.Excode.Const26)
            {
                int highBits = (int)((uint)token.Value >> 26);
                string data = highBits == 0 ? "" : $", highBits: {highBits}";
                stack.Push(new Printed($"Wide({Number(token.Value << 6 >> 6)}{data})", ValueKind.Int, Computed: true));
                continue;
            }
            if (StorageDeclarations.IsStorage(token))
            {
                EVVariable variable = EVVariable.Decode(token);
                stack.Push(new Printed(storage.Qualify(token), variable.UsesBitAddress ? ValueKind.Bool : ValueKind.Int, Storage: true));
                continue;
            }
            if (!TryGet((Jsm.Excode)token.Opcode, out SourceExpression? operation))
            {
                result = null;
                return false;
            }
            if (operation.Payload != ExpressionPayload.None)
            {
                stack.Push(new Printed($"{operation.Service}.{operation.Method}({FormatPayload(operation.Payload, token.Value)})", operation.Result, Computed: operation.Opcode == Jsm.Excode.B_SYSVAR));
                continue;
            }
            // The VM unary-plus opcode pushes the last calculation again. In the
            // observed sine pattern only the copied value is consumed after this.
            if (operation.Opcode == Jsm.Excode.B_SINGLE_PLUS &&
                (index == 0 || tokens[index - 1].Opcode != (byte)Jsm.Excode.B_SIN || stack.Count != 1))
            {
                result = null;
                return false;
            }
            if (stack.Count < operation.Parameters.Length)
            {
                result = null;
                return false;
            }

            var arguments = new Printed[operation.Parameters.Length];
            for (int argument = arguments.Length - 1; argument >= 0; argument--)
                arguments[argument] = stack.Pop();
            string name = operation.Service == "Values" ? operation.Method : $"{operation.Service}.{operation.Method}";
            if (operation.Opcode == Jsm.Excode.Let && arguments[0].Computed)
            {
                string discarded = $"Values.DiscardAssignment({arguments[0].Text}, {arguments[1].Text})";
                if (statement && index == tokens.Count - 1 && stack.Count == 0)
                {
                    result = discarded;
                    return true;
                }
                stack.Push(new Printed(discarded, ValueKind.Int, Computed: true));
                continue;
            }
            if (statement && index == tokens.Count - 1 && operation.Statement && stack.Count == 0)
            {
                result = Assignment(operation.Opcode, arguments) ??
                    $"{name}({string.Join(", ", operation.Parameters.Zip(arguments, (parameter, value) => $"{parameter}: {value.Text}"))})";
                return true;
            }
            if (operation.Statement && arguments[0].Storage &&
                (arguments[0].Kind == ValueKind.Int || operation.Opcode == Jsm.Excode.Let && (arguments[1].Kind == ValueKind.Bool || arguments[1].Literal is 0 or 1)) &&
                Assignment(operation.Opcode, arguments) is string assignment)
            {
                stack.Push(new Printed($"({assignment})", arguments[0].Kind));
                continue;
            }
            Printed? native = PrintOperator(operation.Opcode, arguments);
            stack.Push(native ?? new Printed($"{name}({string.Join(", ", arguments.Select(argument => argument.Text))})", operation.Result));
        }

        if (statement || stack.Count != 1)
        {
            result = null;
            return false;
        }
        Printed value = stack.Pop();
        kind = value.Kind;
        result = asOperand && value.Literal.HasValue ? $"Value({value.Text})" : value.Text;
        return true;
    }

    private static Printed? PrintOperator(Jsm.Excode opcode, Printed[] arguments)
    {
        if (arguments.Length == 1 && arguments[0].Kind == ValueKind.Int && opcode is Jsm.Excode.B_SINGLE_MINUS or Jsm.Excode.B_COMP)
        {
            string symbol = opcode == Jsm.Excode.B_SINGLE_MINUS ? "-" : "~";
            return new Printed($"({symbol}({arguments[0].Text}))", ValueKind.Int);
        }
        if (arguments.Length != 2)
            return null;

        var spec = NativeExpression.Binary.FirstOrDefault(entry => entry.Opcode == opcode);
        if (spec.Symbol == null)
            return null;
        Printed left = arguments[0];
        Printed right = arguments[1];
        if (opcode is Jsm.Excode.Equivalence or Jsm.Excode.NotEquivalence)
        {
            if (left.Kind == ValueKind.Bool)
                right = right.AsFlag();
            if (right.Kind == ValueKind.Bool)
                left = left.AsFlag();
        }
        ValueKind operandType = opcode is Jsm.Excode.BooleanAnd or Jsm.Excode.BooleanOr ? ValueKind.Bool : ValueKind.Int;
        bool flagEquality = (opcode is Jsm.Excode.Equivalence or Jsm.Excode.NotEquivalence) && left.Kind == ValueKind.Bool && right.Kind == ValueKind.Bool;
        if (!flagEquality)
        {
            left = ConvertValue(left, operandType);
            right = ConvertValue(right, operandType);
        }
        if (!flagEquality && (left.Kind != operandType || right.Kind != operandType))
            return null;
        return new Printed($"({left.Text} {spec.Symbol} {right.Text})", spec.BooleanResult ? ValueKind.Bool : ValueKind.Int, Computed: true);
    }

    private static string? Assignment(Jsm.Excode opcode, Printed[] arguments)
    {
        if (!arguments[0].Storage)
            return null;
        Printed target = arguments[0];
        if (arguments.Length == 1 && target.Kind == ValueKind.Int)
        {
            return opcode switch
            {
                Jsm.Excode.IncrementPost => $"{target.Text}++",
                Jsm.Excode.DecrementPost => $"{target.Text}--",
                Jsm.Excode.B_PRE_PLUS => $"++{target.Text}",
                Jsm.Excode.B_PRE_MINUS => $"--{target.Text}",
                _ => null
            };
        }
        if (arguments.Length != 2)
            return null;
        Printed right = ConvertValue(arguments[1], target.Kind);
        if (right.Kind != target.Kind)
            return null;
        if (opcode == Jsm.Excode.Let)
            return $"{target.Text} = {right.Text}";
        string? symbol = NativeExpression.Compound.FirstOrDefault(item => item.Opcode == opcode).Symbol;
        return target.Kind == ValueKind.Int && symbol != null ? $"{target.Text} {symbol} {right.Text}" : null;
    }

    private static Printed ConvertValue(Printed value, ValueKind kind)
    {
        if (kind == ValueKind.Bool && value.Kind == ValueKind.Int)
        {
            if (value.Literal is 0 or 1)
                return value.AsFlag();
            return new Printed($"(({value.Text}) is not 0)", ValueKind.Bool);
        }
        if (kind == ValueKind.Int && value.Kind == ValueKind.Bool)
            return new Printed($"(({value.Text}) ? 1 : 0)", ValueKind.Int);
        return value;
    }

    private static string FormatPayload(ExpressionPayload payload, int value) => payload switch
    {
        ExpressionPayload.ObjectProperty => $"targetUid: {value & 0xFF}, property: {value >> 8}",
        ExpressionPayload.SystemVariable when Enum.IsDefined((SystemData)value) => $"SystemVariable.{(SystemData)value}",
        ExpressionPayload.SystemVariable => $"(SystemVariable){value}",
        _ => Number(value)
    };

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);

    public static void AppendDeclarations(StringBuilder source)
    {
        foreach (var group in Operations.GroupBy(operation => operation.Service))
        {
            source.AppendLine();
            source.AppendLine($"public static partial class {group.Key}");
            source.AppendLine("{");
            foreach (SourceExpression operation in group)
            {
                string attribute = $"[ExpressionInstruction(Excode.{operation.Opcode})]";
                string type = operation.Payload switch
                {
                    ExpressionPayload.SystemVariable => "SystemVariable",
                    ExpressionPayload.None => "Operand",
                    _ => "byte"
                };
                string parameters = string.Join(", ", operation.Parameters.Select(parameter => $"{type} {parameter}"));
                source.AppendLine($"    {attribute}");
                string result = operation.Result == ValueKind.Int ? "int" : operation.Result == ValueKind.Bool ? "bool" : "Operand";
                string invocation = $"EventScheduler.Read(Excode.{operation.Opcode}";
                foreach (string parameter in operation.Parameters)
                    invocation += type == "SystemVariable" ? $", (int){parameter}" : $", {parameter}";
                invocation += operation.Result == ValueKind.Bool ? ") != 0" : ")";
                if (operation.Opcode == Jsm.Excode.B_SINGLE_PLUS)
                    invocation = "(int)value";
                source.AppendLine($"    public static {result} {operation.Method}({parameters}) => {invocation};");

            }
            source.AppendLine("}");
        }
    }
}

internal enum ExpressionPayload { None, Byte, ObjectProperty, SystemVariable }
