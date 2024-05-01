﻿using System;
using FF8.Core;
using FF8.JSM.Format;
using FF8.JSM.Instructions;

 namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Expression
        {
            public sealed class Let : JsmInstruction, IValueExpression
            {
                private readonly IVariableExpression _variable;
                private readonly IValueExpression _value;

                public Let(IVariableExpression variable, IValueExpression value)
                {
                    _variable = variable;
                    _value = value;
                }

                public static void Evaluate(JsmExpressionEvaluator ev)
                {
                    IValueExpression value = ev.PopValue();
                    IVariableExpression variable = (IVariableExpression) ev.Pop();
                    ev.Push(new Let(variable, value));
                }

                public override String ToString()
                {
                    return $"{GetType().Name}({_variable} = {_value})";
                }

                public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                {
                    _variable.Format(sw, formatterContext, services);
                    sw.Append(" = ");

                    if (_value is ValueExpression value && (_variable.Type == VariableType.Bit || value.Type == VariableType.Bit))
                    {
                        if (value.Value == 0)
                            sw.Append("false");
                        else
                            sw.Append("true");
                    }
                    else
                    {
                        _value.Format(sw, formatterContext, services);
                    }
                    
                    sw.AppendLine(";");
                }
            }
        }
    }
}