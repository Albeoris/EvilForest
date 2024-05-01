using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// WaitTurn
    /// Wait until the character has turned.
    /// WAITTURN = 0x050,
    /// </summary>
    internal sealed class WAITTURN : JsmInstruction
    {
        private WAITTURN()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new WAITTURN();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .YieldReturn()
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.WaitTurnComplete))
                .Comment(nameof(WAITTURN));
        }
        
        public override String ToString()
        {
            return $"{nameof(WAITTURN)}()";
        }
    }
}