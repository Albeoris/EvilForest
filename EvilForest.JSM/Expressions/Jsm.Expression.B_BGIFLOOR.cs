using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class B_BGIFLOOR : IValueExpression
            {
                private readonly IValueExpression _characterId;

                public B_BGIFLOOR(IValueExpression characterId)
                {
                    _characterId = characterId;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IValueExpression characterId = evaluator.PopValue();

                    evaluator.Push(new B_BGIFLOOR(characterId));
                }

                public override String ToString()
                {
                    return $"{nameof(B_BGIFLOOR)}({_characterId})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method("BGI_charGetFloor")
                        .Argument("characterId", _characterId)
                        .Comment(nameof(B_BGIFLOOR));
                }
            }
        }
    }
}