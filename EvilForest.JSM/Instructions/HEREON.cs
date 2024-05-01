using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// ShowHereIcon
    /// Show the Here icon over player's chatacter.
    /// 
    /// 1st argument: display type (0 to hide, 3 to show unconditionally)
    /// AT_SPIN Show (1 bytes)
    /// HEREON = 0x0EF,
    /// </summary>
    internal sealed class HEREON : JsmInstruction
    {
        private readonly IJsmExpression _isShown;

        private HEREON(IJsmExpression isShown)
        {
            _isShown = isShown;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression show = reader.ArgumentByte();
            return new HEREON(show);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Gui))
                .Method(nameof(IGuiService.SetPlayerHereIcon))
                .Argument("isShown", _isShown)
                .Comment(nameof(HEREON));
        }
        
        public override String ToString()
        {
            return $"{nameof(HEREON)}({nameof(_isShown)}: {_isShown})";
        }
    }
}