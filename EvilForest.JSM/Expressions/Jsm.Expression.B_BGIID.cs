using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class B_BGIID : IValueExpression
            {
                private readonly IValueExpression _characterId;

                public B_BGIID(IValueExpression characterId)
                {
                    _characterId = characterId;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IValueExpression characterId = evaluator.PopValue();

                    evaluator.Push(new B_BGIID(characterId));
                }

                public override String ToString()
                {
                    return $"{nameof(B_BGIID)}({_characterId})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method("BGI_charGetTri")
                        .Argument("characterId", _characterId)
                        .Comment(nameof(B_BGIID));
                }
            }
        }
    }
}