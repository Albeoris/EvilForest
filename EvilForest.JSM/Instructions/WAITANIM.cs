using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// WaitAnimation
    /// Wait until the current object's animation has ended.
    /// WAITANIM = 0x041,
    /// </summary>
    internal sealed class WAITANIM : JsmInstruction
    {
        private WAITANIM()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new WAITANIM();
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .YieldReturn()
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.WaitAnimationComplete))
                .Comment(nameof(WAITANIM));
        }
        
        public override String ToString()
        {
            return $"{nameof(WAITANIM)}()";
        }
    }
}