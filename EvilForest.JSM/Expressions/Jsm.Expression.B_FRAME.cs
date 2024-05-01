using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class B_FRAME : IValueExpression
            {
                private readonly IValueExpression _animationIndex;

                public B_FRAME(IValueExpression animationIndex)
                {
                    _animationIndex = animationIndex;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IValueExpression animationIndex = evaluator.PopValue();

                    evaluator.Push(new B_FRAME(animationIndex));
                }

                public override String ToString()
                {
                    return $"{nameof(B_FRAME)}({_animationIndex})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method("GetCharacterAnimationFrame")
                        .Argument("animationIndex", _animationIndex)
                        .Comment(nameof(B_FRAME));
                }
            }
        }
    }
}