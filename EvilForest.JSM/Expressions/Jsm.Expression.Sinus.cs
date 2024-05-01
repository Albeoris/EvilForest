using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class Sinus : IValueExpression
            {
                private readonly IValueExpression _value;

                public Sinus(IValueExpression value)
                {
                    _value = value;
                }

                public static void EvaluateMul16(JsmExpressionEvaluator ev)
                {
                    IValueExpression value = ev.PopValue();
                    Math mulValue = new Math(value, new ValueExpression(16, VariableType.Int24), Math.Type.Mul);
                    ev.Push(new Sinus(mulValue));
                }

                public static void Evaluate(JsmExpressionEvaluator ev)
                {
                    IValueExpression value = ev.PopValue();
                    ev.Push(new Sinus(value));
                }

                public override String ToString()
                {
                    return $"Math.Sin({_value})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Append(nameof(System.Math));
                    sw.Append(".");
                    sw.Append(nameof(System.Math.Sin));
                    sw.Append("(");
                    _value.Format(sw, formatterContext, services);
                    sw.Append(")");
                }
            }
        }
    }
}