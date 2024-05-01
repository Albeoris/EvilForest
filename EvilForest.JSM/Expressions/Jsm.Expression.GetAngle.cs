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
            public sealed class CalcAngleByTangent : JsmInstruction, IValueExpression
            {
                private readonly IValueExpression _dx;
                private readonly IValueExpression _dz;

                public CalcAngleByTangent(IValueExpression dx, IValueExpression dz)
                {
                    _dx = dx;
                    _dz = dz;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IValueExpression value1 = (IValueExpression) evaluator.Pop();
                    IValueExpression value2 = (IValueExpression) evaluator.Pop();

                    evaluator.Push(new CalcAngleByTangent(value1, value2));
                }

                public override String ToString()
                {
                    return $"{nameof(CalcAngleByTangent)}({_dx}, {_dz})";
                }

                public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .StaticType(nameof(GameMath))
                        .Method(nameof(CalcAngleByTangent))
                        .Argument("dx", _dx)
                        .Argument("dz", _dz)
                        .Comment(nameof(CalcAngleByTangent));
                }
            }
        }
    }
}