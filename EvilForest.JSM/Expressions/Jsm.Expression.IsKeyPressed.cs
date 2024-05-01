using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class IsKeyPressed : IValueExpression
            {
                private readonly IJsmExpression _key;

                public IsKeyPressed(IJsmExpression key)
                {
                    _key = key;
                }

                public static void Evaluate(JsmExpressionEvaluator ev)
                {
                    IValueExpression key = ev.PopValue();
                    ev.Push(new IsKeyPressed(key));
                }

                public override String ToString()
                {
                    return $"IsKeyPressed({_key})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method(nameof(IsKeyPressed))
                        .Argument("key", _key)
                        .Comment(nameof(IsKeyPressed));
                }
            }
        }
    }
}