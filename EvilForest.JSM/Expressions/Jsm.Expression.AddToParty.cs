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
            public sealed class AddToParty : JsmInstruction, IValueExpression
            {
                private readonly IValueExpression _characterId;

                public AddToParty(IValueExpression characterId)
                {
                    _characterId = characterId;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IValueExpression characterId = (IValueExpression) evaluator.Pop();

                    evaluator.Push(new AddToParty(characterId));
                }

                public override String ToString()
                {
                    return $"{nameof(AddToParty)}({_characterId})";
                }

                public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method(nameof(AddToParty))
                        .Argument("characterId", _characterId)
                        .Comment(nameof(AddToParty));
                }
            }
        }
    }
}