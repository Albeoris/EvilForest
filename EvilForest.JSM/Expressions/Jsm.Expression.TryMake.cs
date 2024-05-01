﻿using System;
using System.Collections.Generic;
 using Albeoris.Framework.Collections;
 using FF8.Core;
 using Memoria.EventEngine.EV;
using Memoria.EventEngine.Execution;

 namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public static void Evaluate(EVScriptMaker maker, IStack<IJsmExpression> stack)
            {
                Evaluate(maker, stack, StatelessServices.Instance);
            }

            private static void Evaluate(EVScriptMaker maker, IStack<IJsmExpression> stack, IServices services)
            {
                while (true)
                {
                    SByte opcode = maker.ReadSByte();

                    if (opcode < 0)
                    {
                        IJsmExpression value = ReadInt26(maker, opcode);
                        stack.Push(value);
                    }
                    else
                    {
                        Excode excode = (Excode) opcode;
                        switch (excode)
                        {
                            case Excode.B_OBJSPECA:
                            {
                                Int26 value = new Int26((maker.ReadByte() << 8) | maker.ReadByte(), VariableSource.Object, VariableType.Default);
                                stack.Push(new VariableExpression(value));
                                break;
                            }
                            case Excode.B_SYSLIST:
                            {
                                Int26 value = new Int26(maker.ReadByte(), VariableSource.System, VariableType.Default);
                                stack.Push(new VariableExpression(value));
                                break;
                            }
                            case Excode.B_SYSVAR:
                            {
                                Int26 value = new Int26(maker.ReadByte(), VariableSource.System, VariableType.Default);
                                stack.Push(new VariableExpression(value).Evaluate());
                                break;
                            }
                            case Excode.Const16:
                            {
                                Int32 value = maker.ReadInt16();
                                stack.Push(new ValueExpression(value, VariableType.Default));
                                break;
                            }
                            case Excode.Const26:
                            {
                                Int32 value = maker.ReadInt26();
                                stack.Push(new ValueExpression(value, VariableType.Default));
                                break;
                            }
                            case Excode.End:
                            {
                                return;
                            }
                            default:
                            {
                                Evaluate(excode, maker, stack, services);
                                break;
                            }
                        }
                    }
                }
            }

            private static IJsmExpression ReadInt26(EVScriptMaker maker, Int32 value)
            {
                Int32 x = (value & 3); // XXXX XX11
                x <<= 26;

                Int32 y = (value & 28); // XXX1 11XX
                y <<= 27;

                x |= y;

                y = maker.ReadByte();
                x |= y;

                if ((value & 32) != 0) // XX1X XXXX
                {
                    y = maker.ReadByte();
                    y <<= 8;
                    x |= y;
                }

                Int26 v = new Int26(x);

                if (v.Source == VariableSource.Const)
                    return new ValueExpression(v.Value, v.Type);

                return new VariableExpression(v);
            }

            public static void Evaluate(Excode opcode, EVScriptMaker maker, IStack<IJsmExpression> stack, IServices services)
            {
                if (!Factories.TryGetValue(opcode, out var eval))
                    throw new NotImplementedException(opcode.ToString());

                JsmExpressionEvaluator evaluator = new JsmExpressionEvaluator(maker, stack, services);
                eval(evaluator);
            }


            private delegate void EvaluateDelegate(JsmExpressionEvaluator evaluator);

            private static readonly Dictionary<Excode, EvaluateDelegate> Factories = new Dictionary<Excode, EvaluateDelegate>
            {
                {Excode.IncrementPost, Unary.IncrementPost},
                {Excode.DecrementPost, Unary.DecrementPost},
                {Excode.B_PRE_PLUS, Unary.IncrementPrefix},
                {Excode.B_PRE_MINUS, Unary.DecrementPrefix},
                {Excode.B_SINGLE_PLUS, Unary.Plus},
                {Excode.B_SINGLE_MINUS, Unary.Minus},
                {Excode.B_NOT, Unary.Not},
                {Excode.B_COMP, Unary.BitInverse},
                {Excode.Mul, Math.Mul},
                {Excode.Div, Math.Div},
                {Excode.Mod, Math.Mod},
                {Excode.Add, Math.Add},
                {Excode.Sub, Math.Sub},
                {Excode.BitLeft, Math.BitLeft},
                {Excode.BitRight, Math.BitRight},
                {Excode.LessThan, Comparison.LessStrict},
                {Excode.LessOrEquals, Comparison.LessOrEqual},
                {Excode.GreatThan, Comparison.GreatStrict},
                {Excode.GreatOrEquals, Comparison.GreatOrEqual},
                {Excode.Equivalence, Comparison.Equal},
                {Excode.NotEquivalence, Comparison.NotEqual},
                {Excode.BitAnd, Math.BitAnd},
                {Excode.BitXor, Math.BitXor},
                {Excode.BitOr, Math.BitOr},
                {Excode.BooleanAnd, BooleanAnd.Evaluate},
                {Excode.BooleanOr, BooleanOr.Evaluate},
                {Excode.Let, Let.Evaluate},
                {Excode.MulLet, Math.MulLet},
                {Excode.DivLet, Math.DivLet},
                {Excode.ModeLet, Math.ModLet},
                {Excode.AddLet, Math.AddLet},
                {Excode.SubLet, Math.SubLet},
                {Excode.BitLeftLet, Math.BitLeftLet},
                {Excode.BitRightLet, Math.BitRightLet},
                {Excode.BitAndLet, Math.BitAndLet},
                {Excode.BitXorLet, Math.BitXorLet},
                {Excode.BitOrLet, Math.BitOrLet},
                {Excode.IsKeyPressed, IsKeyPressed.Evaluate},
                {Excode.B_SIN2, Sinus.Evaluate},
                {Excode.B_COS2, Cosinus.Evaluate},
                {Excode.GetCurrentHP, GetCharacterStat.CurrentHP},
                {Excode.GetMaxHP, GetCharacterStat.MaxHP},
                {Excode.B_KEYOFF, B_KEYOFF.Evaluate},
                {Excode.B_KEY, B_KEY.Evaluate},
                {Excode.GetActorAngle, GetActorAngle.Evaluate},
                {Excode.GetActorDistance, GetActorDistance.Evaluate},
                {Excode.B_PTR, B_PTR.Evaluate},
                {Excode.B_ANGLEA, B_ANGLEA.Evaluate},
                {Excode.B_DISTANCEA, B_DISTANCEA.Evaluate},
                {Excode.B_SIN, Sinus.EvaluateMul16},
                {Excode.B_COS, Cosinus.EvaluateMul16},
                {Excode.B_HAVE_ITEM, B_HAVE_ITEM.Evaluate},
                {Excode.GetAngle, CalcAngleByTangent.Evaluate},
                {Excode.B_FRAME, B_FRAME.Evaluate},
                {Excode.IsInParty, IsInParty.Evaluate},
                {Excode.AddToParty, AddToParty.Evaluate},
                {Excode.GetCurrentMP, GetCharacterStat.CurrentMP},
                {Excode.GetMaxMP, GetCharacterStat.MaxMP},
                {Excode.B_BGIID, B_BGIID.Evaluate},
                {Excode.B_BGIFLOOR, B_BGIFLOOR.Evaluate},
            };
        }
    }
}