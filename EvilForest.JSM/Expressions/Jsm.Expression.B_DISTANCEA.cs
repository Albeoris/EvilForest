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
            /// A distance calculation.
            /// </summary>
            public sealed class B_DISTANCEA : IValueExpression
            {
                private readonly IJsmExpression _objectId;

                public B_DISTANCEA(IJsmExpression objectId)
                {
                    _objectId = objectId;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IJsmExpression objectId = evaluator.Pop();

                    evaluator.Push(new B_DISTANCEA(objectId));
                }

                public override String ToString()
                {
                    return $"{nameof(B_DISTANCEA)}({_objectId})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method("CalcDistance")
                        .Argument("id: ", _objectId)
                        .Comment(nameof(B_DISTANCEA));

                    sw.Append(ToString());
                }
            }
        }
    }
}