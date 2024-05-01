using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// RaiseWindows
    /// Make all the dialogs and windows display over the filters.
    /// RAISE = 0x08E,
    /// </summary>
    internal sealed class RAISE : JsmInstruction
    {
        private RAISE()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new RAISE();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Messages))
                .Method(nameof(IMessageService.RaiseAllWindows))
                .Comment(nameof(RAISE));
        }
        
        public override String ToString()
        {
            return $"{nameof(RAISE)}()";
        }
    }
}