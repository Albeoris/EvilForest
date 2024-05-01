using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetControlDirection
    /// Set the angles for the player's movement control.
    /// 
    /// 1st argument: angle used for arrow movements.
    /// 2nd argument: angle used for analogic stick movements.
    /// AT_SPIN Arrow Angle (1 bytes)
    /// AT_SPIN Analogic Angle (1 bytes)
    /// </summary>
    internal sealed class TWIST : JsmInstruction
    {
        private readonly IJsmExpression _arrowsAngle;
        private readonly IJsmExpression _analogAngle;

        private TWIST(IJsmExpression arrowsAngle, IJsmExpression analogAngle)
        {
            _arrowsAngle = arrowsAngle;
            _analogAngle = analogAngle;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression analogAngle = reader.ArgumentByte();
            IJsmExpression digitAngle = reader.ArgumentByte();
            return new TWIST(digitAngle, analogAngle);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Input))
                .Method(nameof(IInputService.SetControlDirection))
                .Argument("arrowsAngle", _arrowsAngle)
                .Argument("analogAngle", _analogAngle)
                .Comment(nameof(TWIST));
        }

        public override String ToString()
        {
            return $"{nameof(TWIST)}({nameof(_arrowsAngle)}: {_arrowsAngle}, {nameof(_analogAngle)}: {_analogAngle})";
        }
    }
}