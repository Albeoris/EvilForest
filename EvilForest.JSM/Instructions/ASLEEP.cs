using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetInactiveAnimation
    /// Change the animation played when inactive for a long time. The inaction time required is:
    /// First Time = 200 + 4 * Random[0, 255]
    /// Subsequent Times = 200 + 2 * Random[0, 255]
    /// 
    /// 1st argument: animation ID.
    /// AT_ANIMATION Animation (2 bytes)
    /// ASLEEP = 0x052,
    /// </summary>
    internal sealed class ASLEEP : JsmInstruction
    {
        private readonly IConstExpression _animation;

        private ASLEEP(IConstExpression animation)
        {
            _animation = animation;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression animation = (IConstExpression) reader.ArgumentInt16();
            return new ASLEEP(animation);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            String animation = formatterContext.GetAnimation((DBAnimation.Id) _animation.Int32()).FileName;
            
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetAnimation))
                .Argument(null, AnimationKind.Sleep)
                .Argument(null, animation)
                .Comment(nameof(ASLEEP));
        }
        
        public override String ToString()
        {
            return $"{nameof(ASLEEP)}({nameof(_animation)}: {_animation})";
        }
    }
}