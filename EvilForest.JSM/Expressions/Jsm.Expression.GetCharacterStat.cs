using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class GetCharacterStat : IValueExpression
            {
                private readonly IValueExpression _characterId;
                private readonly Type _type;

                public GetCharacterStat(IValueExpression characterId, Type type)
                {
                    _characterId = characterId;
                    _type = type;
                }

                public static void CurrentHP(JsmExpressionEvaluator ev) => Evaluate(ev, Type.CurrentHP);
                public static void MaxHP(JsmExpressionEvaluator ev) => Evaluate(ev, Type.MaxHP);
                public static void CurrentMP(JsmExpressionEvaluator ev) => Evaluate(ev, Type.CurrentMP);
                public static void MaxMP(JsmExpressionEvaluator ev) => Evaluate(ev, Type.MaxMP);

                public static void Evaluate(JsmExpressionEvaluator ev, Type type)
                {
                    IValueExpression characterId = ev.PopValue();

                    ev.Push(new GetCharacterStat(characterId, type));
                }

                public override String ToString()
                {
                    return $"{GetType().Name}.{_type} {_characterId}";
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    switch (_type)
                    {
                        case Type.CurrentHP:
                            sw.Append("GetCharacterCurrentHP(");
                            _characterId.Format(sw, formatterContext, services);
                            sw.Append(")");
                            break;
                        case Type.MaxHP:
                            sw.Append("GetCharacterMaxHP(");
                            _characterId.Format(sw, formatterContext, services);
                            sw.Append(")");
                            break;
                        case Type.CurrentMP:
                            sw.Append("GetCharacterCurrentMP(");
                            _characterId.Format(sw, formatterContext, services);
                            sw.Append(")");
                            break;
                        case Type.MaxMP:
                            sw.Append("GetCharacterMaxMP(");
                            _characterId.Format(sw, formatterContext, services);
                            sw.Append(")");
                            break;
                        default:
                            throw new NotSupportedException(_type.ToString());
                    }
                }

                public enum Type
                {
                    CurrentHP = Excode.GetCurrentHP,
                    MaxHP = Excode.GetMaxHP,
                    CurrentMP = Excode.GetCurrentMP,
                    MaxMP = Excode.GetMaxMP,
                }
            }
        }
    }
}