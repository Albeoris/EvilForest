using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetTextVariable
    /// Set the value of a text number or item variable.
    /// 
    /// 1st argument: text variable's "Script ID".
    /// 2nd argument: depends on which text opcode is related to the text variable.
    ///  For [VAR_NUM]: integral value.
    ///  For [VAR_ITEM]: item ID.
    ///  For [VAR_TOKEN]: token number.
    /// AT_USPIN Variable ID (1 bytes)
    /// AT_ITEM Value (2 bytes)
    /// MESVALUE = 0x066,
    /// </summary>
    internal sealed class MESVALUE : JsmInstruction
    {
        private readonly IJsmExpression _index;

        private readonly IJsmExpression _value;

        private MESVALUE(IJsmExpression index, IJsmExpression value)
        {
            _index = index;
            _value = value;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression variableId = reader.ArgumentByte();
            IJsmExpression value = reader.ArgumentInt16();
            return new MESVALUE(variableId, value);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Messages))
                .Method(nameof(IMessageService.SetTextVariable))
                .Argument("index", _index)
                .Argument("value", _value)
                .Comment(nameof(MESVALUE));
        }
        
        public override String ToString()
        {
            return $"{nameof(MESVALUE)}({nameof(_index)}: {_index}, {nameof(_value)}: {_value})";
        }
    }
}