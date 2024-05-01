using System;
using FF8.Core;
using FF8.JSM.Format;
using Memoria.EventEngine.Execution;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class VariableExpression : IVariableExpression
            {
                public Int26 Value { get; }
                public VariableType Type => Value.Type;

                public VariableExpression(Int26 value)
                {
                    Value = value;
                }
                
                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    switch (Value.Source)
                    {
                        case VariableSource.Global:
                            sw.GlobalVariable(Value);
                            break;
                        case VariableSource.Map:
                            sw.EventVariable(Value);
                            break;
                        case VariableSource.Instance:
                            sw.ObjectVariable(Value);
                            break;
                        case VariableSource.Object:
                            AppendCast(sw);
                            var v = Value.Value;
                            Int32 kind = v & 0xFF;
                            Int32 objectIndex = v >> 8;
                            sw.Append($"Object[{objectIndex}, {kind}]");
                            break;
                        case VariableSource.System:
                            sw.SystemVariable(Value);
                            break;
                        case VariableSource.Member:
                            AppendCast(sw);
                            sw.Append($"Member[{Value.Value}]");
                            break;
                        case VariableSource.Const:
                            AppendCast(sw);
                            sw.Append($"{Value.Value}");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                private void AppendCast(ScriptWriter sw)
                {
                    sw.Append($"({Value.GetTypeName()})");
                }

                public override String ToString()
                {
                    ScriptWriter sw = new ScriptWriter(capacity: 16);
                    Format(sw, DummyFormatterContext.Instance, StatelessServices.Instance);
                    return sw.Release();
                }

                public IJsmExpression Evaluate()
                {
                    if (Value.Source == VariableSource.Const)
                        return new ValueExpression(Value.Value, Value.Type);

                    return new Evaluated(Value);
                }

                private sealed class Evaluated : IVariableExpression
                {
                    private readonly Int26 _value;
                    public VariableType Type => _value.Type;

                    public Evaluated(Int26 value)
                    {
                        _value = value;
                    }

                    public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                    {
                        String typeName = _value.GetTypeName();
                        
                        switch (_value.Source)
                        {
                            case VariableSource.Global:
                                sw.GlobalVariable(_value);
                                break;
                            case VariableSource.Map:
                                sw.EventVariable(_value);
                                break;
                            case VariableSource.Instance:
                                sw.ObjectVariable(_value);
                                break;
                            case VariableSource.Object:
                                var v = _value.Value;
                                Int32 kind = v & 0xFF;
                                Int32 objectIndex = v >> 8;
                                sw.Append($"({typeName})");
                                sw.Append($"Object[{objectIndex}, {kind}]");
                                break;
                            case VariableSource.System:
                                sw.SystemVariable(_value);
                                break;
                            case VariableSource.Member:
                                sw.Append($"({typeName})");
                                sw.Append($"Member[{_value.Value}]");
                                break;
                            case VariableSource.Const:
                                sw.Append($"({typeName})");
                                sw.Append($"{_value.Value}");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }
    }
}