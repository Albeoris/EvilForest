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
            /// Return the amount of items of an item type in the player's inventory.
            /// B_HAVE_ITEM = 100
            /// </summary>
            public sealed class B_HAVE_ITEM : IValueExpression
            {
                private readonly IValueExpression _itemId;

                public B_HAVE_ITEM(IValueExpression itemId)
                {
                    _itemId = itemId;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    IValueExpression itemId = (IValueExpression) evaluator.Pop();

                    evaluator.Push(new B_HAVE_ITEM(itemId));
                }

                public override String ToString()
                {
                    return $"{nameof(B_HAVE_ITEM)}({_itemId})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method("GetItemCount")
                        .Argument("characterId", _itemId)
                        .Comment(nameof(B_HAVE_ITEM));
                }
            }
        }
    }
}