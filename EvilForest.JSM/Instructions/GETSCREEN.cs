using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// 0xA9
    /// Unknown Opcode.
    /// AT_ENTRY Unknown (1 bytes)
    /// GETSCREEN = 0x0A9,
    /// </summary>
    internal sealed class GETSCREEN : JsmInstruction
    {
        private readonly IJsmExpression _objectIndex;

        private GETSCREEN(IJsmExpression objectIndex)
        {
            _objectIndex = objectIndex;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression objectIndex = reader.ArgumentByte();
            return new GETSCREEN(objectIndex);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.System))
                .Method(nameof(ISystemService.PullActorScreenPosition))
                .Argument("objectIndex", _objectIndex)
                .Comment(nameof(GETSCREEN));
        }
        
        public override String ToString()
        {
            return $"{nameof(GETSCREEN)}({nameof(_objectIndex)}: {_objectIndex})";
        }
    }
}