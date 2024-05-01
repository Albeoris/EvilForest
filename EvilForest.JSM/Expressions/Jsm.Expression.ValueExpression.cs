using System;
using System.Globalization;
using FF8.Core;
using FF8.JSM.Format;
using Memoria.EventEngine.Execution;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class ValueExpression : IConstExpression
            {
                public Int64 Value { get; }
                public VariableType Type { get; }

                public static ValueExpression Create(Int64 value) => new ValueExpression(value, VariableType.Int24);
                public static ValueExpression Boolean(Boolean value) => new ValueExpression(value ? 1 : 0, VariableType.Int24);

                public ValueExpression(Int64 value, VariableType type)
                {
                    Value = value;
                    Type = type;

                    if (Type < VariableType.Default || type > VariableType.UInt16)
                        throw new ArgumentOutOfRangeException($"Type {type} isn't supported.", nameof(type));
                }

                public void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    switch (Type)
                    {
                        case VariableType.Default:
                            // sw.Append("(Int64)");
                            break;
                        case VariableType.Bit:
                            sw.Append("(Boolean)");
                            break;
                        case VariableType.Int24:
                            if (Value < Int32.MinValue || Value > Int32.MaxValue)
                                sw.Append("(Int32)");
                            break;
                        case VariableType.UInt24:
                            if (Value < UInt32.MinValue || Value > UInt32.MaxValue)
                                sw.Append("(UInt32)");
                            break;
                        case VariableType.SByte:
                            if (Value < SByte.MinValue || Value > SByte.MaxValue)
                                sw.Append("(SByte)");
                            break;
                        case VariableType.Byte:
                            if (Value < Byte.MinValue || Value > Byte.MaxValue)
                                sw.Append("(Byte)");
                            break;
                        case VariableType.Int16:
                            if (Value < Int16.MinValue || Value > Int16.MaxValue)
                                sw.Append("(Int16)");
                            break;
                        case VariableType.UInt16:
                            if (Value < UInt16.MinValue || Value > UInt16.MaxValue)
                                sw.Append("(UInt16)");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    sw.Append(Value.ToString(CultureInfo.InvariantCulture));
                }

                public IJsmExpression Evaluate(IServices services)
                {
                    return this;
                }

                public Int64 Calculate(IServices services)
                {
                    return Value;
                }

                public ILogicalExpression LogicalInverse()
                {
                    return new ValueExpression(Value == 0 ? 1 : 0, VariableType.Default);
                }

                public override String ToString()
                {
                    ScriptWriter sw = new ScriptWriter(capacity: 16);
                    Format(sw, DummyFormatterContext.Instance, StatelessServices.Instance);
                    return sw.Release();
                }
            }
        }
    }
}