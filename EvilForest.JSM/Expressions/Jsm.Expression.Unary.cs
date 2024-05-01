using System;
using FF8.Core;
using FF8.JSM.Format;
using FF8.JSM.Instructions;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class Unary : JsmInstruction, IValueExpression
            {
                private readonly IValueExpression _value;
                private readonly Type _type;

                public Unary(IValueExpression value, Type type)
                {
                    _value = value;
                    _type = type;
                }

                public static void IncrementPost(JsmExpressionEvaluator ev) => Evaluate(ev, Type.IncrementPost);
                public static void DecrementPost(JsmExpressionEvaluator ev) => Evaluate(ev, Type.DecrementPost);
                public static void IncrementPrefix(JsmExpressionEvaluator ev) => Evaluate(ev, Type.IncrementPrefix);
                public static void DecrementPrefix(JsmExpressionEvaluator ev) => Evaluate(ev, Type.DecrementPrefix);
                public static void Plus(JsmExpressionEvaluator ev) => Evaluate(ev, Type.Plus);
                public static void Minus(JsmExpressionEvaluator ev) => Evaluate(ev, Type.Minus);
                public static void Not(JsmExpressionEvaluator ev) => Evaluate(ev, Type.Not);
                public static void BitInverse(JsmExpressionEvaluator ev) => Evaluate(ev, Type.BitInverse);

                public static void Evaluate(JsmExpressionEvaluator ev, Type type)
                {
                    IValueExpression variable = (IValueExpression) ev.Pop();

                    ev.Push(new Unary(variable, type));
                }

                public override String ToString()
                {
                    return $"{GetType().Name}.{_type} {_value}";
                }

                public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    switch (_type)
                    {
                        case Type.IncrementPost:
                            _value.Format(sw, formatterContext, services);
                            sw.Append("++");
                            break;
                        case Type.DecrementPost:
                            _value.Format(sw, formatterContext, services);
                            sw.Append("--");
                            break;
                        case Type.IncrementPrefix:
                            sw.Append("++");
                            _value.Format(sw, formatterContext, services);
                            break;
                        case Type.DecrementPrefix:
                            sw.Append("--");
                            _value.Format(sw, formatterContext, services);
                            break;
                        case Type.Plus:
                            _value.Format(sw, formatterContext, services);
                            break;
                        case Type.Minus:
                            sw.Append("-(");
                            _value.Format(sw, formatterContext, services);
                            sw.Append(")");
                            break;
                        case Type.Not:
                            sw.Append("(");
                            _value.Format(sw, formatterContext, services);
                            sw.Append(" == 0 ? 1 : 0");
                            sw.Append(")");
                            break;
                        case Type.BitInverse:
                            sw.Append("~(");
                            _value.Format(sw, formatterContext, services);
                            sw.Append(")");
                            break;
                        default:
                            throw new NotSupportedException(_type.ToString());
                    }

                    if (IsStandalone)
                        sw.AppendLine(";");
                }

                public enum Type
                {
                    IncrementPost = Excode.IncrementPost,
                    DecrementPost = Excode.DecrementPost,
                    IncrementPrefix = Excode.B_PRE_PLUS,
                    DecrementPrefix = Excode.B_PRE_MINUS,
                    Plus = Excode.B_SINGLE_PLUS,
                    Minus = Excode.B_SINGLE_MINUS,
                    Not = Excode.B_NOT,
                    BitInverse = Excode.B_COMP
                }
            }
        }
    }
}