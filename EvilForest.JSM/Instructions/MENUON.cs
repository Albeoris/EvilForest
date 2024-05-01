using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// EnableMenu
    /// Enable menu access by the player.
    /// MENUON = 0x0AA,
    /// </summary>
    internal sealed class MENUON : JsmInstruction
    {
        private MENUON()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new MENUON();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Input))
                .Method(nameof(IInputService.SetMenuControl))
                .Argument("isEnabled", true)
                .Comment(nameof(MENUON));
        }
        
        public override String ToString()
        {
            return $"{nameof(MENUON)}()";
        }
    }
}