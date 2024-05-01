using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetRightAnimation
    /// Change the right turning animation.
    /// 
    /// 1st argument: animation ID.
    /// AT_ANIMATION Animation (2 bytes)
    /// ATURNR = 0x07B,
    /// </summary>
    internal sealed class ATURNR : JsmInstruction
    {
        private readonly IConstExpression _animation;

        private ATURNR(IConstExpression animation)
        {
            _animation = animation;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression animation = (IConstExpression) reader.ArgumentInt16();
            return new ATURNR(animation);
        }

        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            String animation = formatterContext.GetAnimation((DBAnimation.Id) _animation.Int32()).FileName;
            
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetAnimation))
                .Argument(null, AnimationKind.TurnRight)
                .Argument(null, animation)
                .Comment(nameof(ATURNR));
        }

        public override String ToString()
        {
            return $"{nameof(ATURNR)}({nameof(_animation)}: {_animation})";
        }
    }
}