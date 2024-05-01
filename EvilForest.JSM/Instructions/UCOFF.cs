using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// DisableMove
    /// Disable the player's movement control.
    /// UCOFF = 0x02D,
    /// </summary>
    internal sealed class UCOFF : JsmInstruction
    {
        private UCOFF()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new UCOFF();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Input))
                .Method(nameof(IInputService.SetMovementControl))
                .Argument("isEnabled", false)
                .Comment(nameof(UCON));
        }
        
        public override String ToString()
        {
            return $"{nameof(UCOFF)}()";
        }
    }
}