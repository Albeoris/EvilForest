using FF8.JSM;
using Microsoft.CodeAnalysis.Operations;

namespace EveilForest.CSharp;

internal static class NativeExpression
{
    public static readonly (Jsm.Excode Opcode, BinaryOperatorKind Kind, string Symbol, bool BooleanResult)[] Binary =
    {
        (Jsm.Excode.Mul, BinaryOperatorKind.Multiply, "*", false),
        (Jsm.Excode.Div, BinaryOperatorKind.Divide, "/", false),
        (Jsm.Excode.Mod, BinaryOperatorKind.Remainder, "%", false),
        (Jsm.Excode.Add, BinaryOperatorKind.Add, "+", false),
        (Jsm.Excode.Sub, BinaryOperatorKind.Subtract, "-", false),
        (Jsm.Excode.BitLeft, BinaryOperatorKind.LeftShift, "<<", false),
        (Jsm.Excode.BitRight, BinaryOperatorKind.RightShift, ">>", false),
        (Jsm.Excode.LessThan, BinaryOperatorKind.LessThan, "<", true),
        (Jsm.Excode.GreatThan, BinaryOperatorKind.GreaterThan, ">", true),
        (Jsm.Excode.LessOrEquals, BinaryOperatorKind.LessThanOrEqual, "<=", true),
        (Jsm.Excode.GreatOrEquals, BinaryOperatorKind.GreaterThanOrEqual, ">=", true),
        (Jsm.Excode.Equivalence, BinaryOperatorKind.Equals, "==", true),
        (Jsm.Excode.NotEquivalence, BinaryOperatorKind.NotEquals, "!=", true),
        (Jsm.Excode.BitAnd, BinaryOperatorKind.And, "&", false),
        (Jsm.Excode.BitXor, BinaryOperatorKind.ExclusiveOr, "^", false),
        (Jsm.Excode.BitOr, BinaryOperatorKind.Or, "|", false),
        (Jsm.Excode.BooleanAnd, BinaryOperatorKind.And, "&", true),
        (Jsm.Excode.BooleanOr, BinaryOperatorKind.Or, "|", true)
    };

    public static readonly (Jsm.Excode Opcode, BinaryOperatorKind Kind, string Symbol)[] Compound =
    {
        (Jsm.Excode.MulLet, BinaryOperatorKind.Multiply, "*="),
        (Jsm.Excode.DivLet, BinaryOperatorKind.Divide, "/="),
        (Jsm.Excode.ModeLet, BinaryOperatorKind.Remainder, "%="),
        (Jsm.Excode.AddLet, BinaryOperatorKind.Add, "+="),
        (Jsm.Excode.SubLet, BinaryOperatorKind.Subtract, "-="),
        (Jsm.Excode.BitLeftLet, BinaryOperatorKind.LeftShift, "<<="),
        (Jsm.Excode.BitRightLet, BinaryOperatorKind.RightShift, ">>="),
        (Jsm.Excode.BitAndLet, BinaryOperatorKind.And, "&="),
        (Jsm.Excode.BitXorLet, BinaryOperatorKind.ExclusiveOr, "^="),
        (Jsm.Excode.BitOrLet, BinaryOperatorKind.Or, "|=")
    };
}
