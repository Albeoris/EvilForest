using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// EnableMove
    /// Enable the player's movement control.
    /// UCON = 0x02E,
    /// </summary>
    internal sealed class UCON : JsmInstruction
    {
        private UCON()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new UCON();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Input))
                .Method(nameof(IInputService.SetMovementControl))
                .Argument("isEnabled", true)
                .Comment(nameof(UCON));
        }
        
        public override String ToString()
        {
            return $"{nameof(UCON)}()";
        }
    }
}