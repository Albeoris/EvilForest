using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// EnableRun
    /// Allow the player's character to run.
    /// DASHON = 0x0F0,
    /// </summary>
    internal sealed class DASHON : JsmInstruction
    {
        private DASHON()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new DASHON();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Input))
                .Method(nameof(IInputService.SetAlwaysWalk))
                .Argument("disableRun", false)
                .Comment(nameof(DASHON));
        }
        
        public override String ToString()
        {
            return $"{nameof(DASHON)}()";
        }
    }
}