using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetLeftAnimation
    /// Change the left turning animation.
    /// 
    /// 1st argument: animation ID.
    /// AT_ANIMATION Animation (2 bytes)
    /// ATURNL = 0x07A,
    /// </summary>
    internal sealed class ATURNL : JsmInstruction
    {
        private readonly IConstExpression _animation;

        private ATURNL(IConstExpression animation)
        {
            _animation = animation;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression animation = (IConstExpression) reader.ArgumentInt16();
            return new ATURNL(animation);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            String animation = formatterContext.GetAnimation((DBAnimation.Id) _animation.Int32()).FileName;
            
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetAnimation))
                .Argument(null, AnimationKind.TurnLeft)
                .Argument(null, animation)
                .Comment(nameof(ATURNL));
        }
        
        public override String ToString()
        {
            return $"{nameof(ATURNL)}({nameof(_animation)}: {_animation})";
        }
    }
}