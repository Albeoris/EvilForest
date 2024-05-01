using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetRunAnimation
    /// Change the running animation.
    /// 
    /// 1st argument: animation ID.
    /// AT_ANIMATION Animation (2 bytes)
    /// ARUN = 0x035,
    /// </summary>
    internal sealed class ARUN : JsmInstruction
    {
        private readonly IConstExpression _animation;

        private ARUN(IConstExpression animation)
        {
            _animation = animation;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression animation = (IConstExpression) reader.ArgumentInt16();
            return new ARUN(animation);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            String animation = formatterContext.GetAnimation((DBAnimation.Id) _animation.Int32()).FileName;
            
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetAnimation))
                .Argument(null, AnimationKind.Run)
                .Argument(null, animation)
                .Comment(nameof(ARUN));
        }
        
        public override String ToString()
        {
            return $"{nameof(ARUN)}({nameof(_animation)}: {_animation})";
        }
    }
}