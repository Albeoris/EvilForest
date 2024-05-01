using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// DisableMenu
    /// Disable menu access by the player.
    /// MENUOFF = 0x0AB,
    /// </summary>
    internal sealed class MENUOFF : JsmInstruction
    {
        private MENUOFF()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new MENUOFF();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Input))
                .Method(nameof(IInputService.SetMenuControl))
                .Argument("isEnabled", false)
                .Comment(nameof(MENUOFF));
        }
        
        public override String ToString()
        {
            return $"{nameof(MENUOFF)}()";
        }
    }
}