using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetWalkAnimation
    /// Change the walking animation.
    /// 
    /// 1st argument: animation ID.
    /// AT_ANIMATION Animation (2 bytes)
    /// AWALK = 0x034,
    /// </summary>
    internal sealed class AWALK : JsmInstruction
    {
        private readonly IConstExpression _animation;

        private AWALK(IConstExpression animation)
        {
            _animation = animation;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression animation = (IConstExpression) reader.ArgumentInt16();
            return new AWALK(animation);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            String animation = formatterContext.GetAnimation((DBAnimation.Id) _animation.Int32()).FileName;
            
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetAnimation))
                .Argument(null, AnimationKind.Walk)
                .Argument(null, animation)
                .Comment(nameof(AWALK));
        }
        
        public override String ToString()
        {
            return $"{nameof(AWALK)}({nameof(_animation)}: {_animation})";
        }
    }
}