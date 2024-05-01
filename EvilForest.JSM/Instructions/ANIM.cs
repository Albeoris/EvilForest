using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// RunAnimation
    /// Make the character play an animation.
    /// 
    /// 1st argument: animation ID.
    /// AT_ANIMATION Animation (2 bytes)
    /// ANIM = 0x040,
    /// </summary>
    internal sealed class ANIM : JsmInstruction
    {
        private readonly IConstExpression _animation;

        private ANIM(IConstExpression animation)
        {
            _animation = animation;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression animation = (IConstExpression) reader.ArgumentInt16();
            return new ANIM(animation);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            String animation = formatterContext.GetAnimation((DBAnimation.Id) _animation.Int32()).FileName;
            
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.Animate))
                .Argument("animation", animation)
                .Comment(nameof(ANIM));
        }
        
        public override String ToString()
        {
            return $"{nameof(ANIM)}({nameof(_animation)}: {_animation})";
        }
    }
}