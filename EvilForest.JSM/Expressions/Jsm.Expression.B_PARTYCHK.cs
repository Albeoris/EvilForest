using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class IsInParty : IValueExpression
            {
                private readonly IValueExpression _characterId;

                public IsInParty(IValueExpression characterId)
                {
                    _characterId = characterId;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IValueExpression characterId = evaluator.PopValue();

                    evaluator.Push(new IsInParty(characterId));
                }

                public override String ToString()
                {
                    return $"{nameof(IsInParty)}({_characterId})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method(nameof(IsInParty))
                        .Argument("characterId", _characterId)
                        .Comment(nameof(IsInParty));
                }
            }
        }
    }
}