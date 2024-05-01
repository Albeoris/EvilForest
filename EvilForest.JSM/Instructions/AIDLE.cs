using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetStandAnimation
    /// Change the standing animation.
    /// 
    /// 1st argument: animation ID.
    /// AT_ANIMATION Animation (2 bytes)
    /// AIDLE = 0x033,
    /// </summary>
    internal sealed class AIDLE : JsmInstruction
    {
        private readonly IConstExpression _animation;

        private AIDLE(IConstExpression animation)
        {
            _animation = animation;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression animation = (IConstExpression)reader.ArgumentInt16();
            return new AIDLE(animation);
        }

        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            String animation = formatterContext.GetAnimation((DBAnimation.Id) _animation.Int32()).FileName;
            
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetAnimation))
                .Argument(null, AnimationKind.Idle)
                .Argument(null, animation)
                .Comment(nameof(AIDLE));
        }

        public override String ToString()
        {
            return $"{nameof(AIDLE)}({nameof(_animation)}: {_animation})";
        }
    }
}