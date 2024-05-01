using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        /// <summary>
        /// Check if a button of the controller is currently pressed.
        ///
        /// B_KEY = 89
        /// </summary>
        public sealed class B_KEY : IValueExpression
        {
            private readonly IValueExpression _key;

            public B_KEY(IValueExpression key)
            {
                _key = key;
            }

            public static void Evaluate(JsmExpressionEvaluator evaluator)
            {
                IValueExpression key = (IValueExpression) evaluator.Pop();

                evaluator.Push(new B_KEY(key));
            }

            public override String ToString()
            {
                return $"{nameof(B_KEY)}({_key})";
            }

            public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
            {
                sw.Append(ToString());
            }
        }
    }
}