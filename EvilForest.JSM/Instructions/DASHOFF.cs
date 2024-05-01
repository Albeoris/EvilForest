using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// DisableRun
    /// Make the player's character always walk.
    /// DASHOFF = 0x06A,
    /// </summary>
    internal sealed class DASHOFF : JsmInstruction
    {
        private DASHOFF()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new DASHOFF();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Input))
                .Method(nameof(IInputService.SetAlwaysWalk))
                .Argument("disableRun", true)
                .Comment(nameof(DASHOFF));
        }
        
        public override String ToString()
        {
            return $"{nameof(DASHOFF)}()";
        }
    }
}