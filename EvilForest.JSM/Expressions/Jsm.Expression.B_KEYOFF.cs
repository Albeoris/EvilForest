using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            /// <summary>
            /// Check if a button of the controller was released right before the current frame.
            ///
            /// B_KEYOFF = 88
            /// </summary>
            public sealed class B_KEYOFF : IValueExpression
            {
                private readonly IValueExpression _key;

                public B_KEYOFF(IValueExpression key)
                {
                    _key = key;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IValueExpression key = (IValueExpression) evaluator.Pop();

                    evaluator.Push(new B_KEYOFF(key));
                }

                public override String ToString()
                {
                    return $"{nameof(B_KEYOFF)}({_key})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Append(ToString());
                }
            }
        }
    }
}