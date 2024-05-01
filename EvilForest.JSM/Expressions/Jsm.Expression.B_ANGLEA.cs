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
            /// An angle calculation.
            /// </summary>
            public sealed class B_ANGLEA : IValueExpression
            {
                private readonly IJsmExpression _objectId;

                public B_ANGLEA(IJsmExpression objectId)
                {
                    _objectId = objectId;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IJsmExpression objectId = evaluator.Pop();

                    evaluator.Push(new B_ANGLEA(objectId));
                }

                public override String ToString()
                {
                    return $"{nameof(B_ANGLEA)}({_objectId})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method("CalcAngle")
                        .Argument("id: ", _objectId)
                        .Comment(nameof(B_ANGLEA));

                    sw.Append(ToString());
                }
            }
        }
    }
}