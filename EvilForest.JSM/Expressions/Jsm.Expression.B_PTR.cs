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
            /// Retrieve an object's UID or 0 if not found.
            /// Only useful for checking if an object exists with the UID or convert special entry IDs (250 to 255) to a valid UID.

            /// B_PTR = 95
            /// </summary>
            public sealed class B_PTR : IValueExpression
            {
                private readonly Byte _objectId;

                public B_PTR(Byte objectId)
                {
                    _objectId = objectId;
                }

                public static void Evaluate(JsmExpressionEvaluator evaluator)
                {
                    Byte objectId = evaluator.ReadByte();

                    evaluator.Push(new B_PTR(objectId));
                }

                public override String ToString()
                {
                    return $"{nameof(B_PTR)}({_objectId})";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    sw.Format(formatterContext, services)
                        .Method("GetObjectUid")
                        .Argument("id: ", _objectId)
                        .Comment(nameof(B_PTR));

                    sw.Append(ToString());
                }
            }
        }
    }
}