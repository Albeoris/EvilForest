using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// SetAnimationStandSpeed
    /// Change the standing animation speed.
    /// 
    /// 1st argument: random speed.
    /// 2nd argument: random speed.
    /// 3rd argument: random speed.
    /// 4th argument: random speed.
    /// AT_USPIN random speed 1 (1 bytes)
    /// AT_USPIN random speed 2 (1 bytes)
    /// AT_USPIN random speed 3 (1 bytes)
    /// AT_USPIN random speed 4 (1 bytes)
    /// IDLESPEED = 0x086,
    /// </summary>
    internal sealed class IDLESPEED : JsmInstruction
    {
        private readonly IJsmExpression _speed1;
        private readonly IJsmExpression _speed2;
        private readonly IJsmExpression _speed3;
        private readonly IJsmExpression _speed4;

        private IDLESPEED(IJsmExpression speed1, IJsmExpression speed2, IJsmExpression speed3, IJsmExpression speed4)
        {
            _speed1 = speed1;
            _speed2 = speed2;
            _speed3 = speed3;
            _speed4 = speed4;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression speed1 = reader.ArgumentByte();
            IJsmExpression speed2 = reader.ArgumentByte();
            IJsmExpression speed3 = reader.ArgumentByte();
            IJsmExpression speed4 = reader.ArgumentByte();
            return new IDLESPEED(speed1, speed2, speed3, speed4);
        }

        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetIdleSpeed))
                .Argument("speed1", _speed1)
                .Argument("speed2", _speed2)
                .Argument("speed3", _speed3)
                .Argument("speed4", _speed4)
                .Comment(nameof(IDLESPEED));
        }

        public override String ToString()
        {
            return $"{nameof(IDLESPEED)}({nameof(_speed1)}: {_speed1}, {nameof(_speed2)}: {_speed2}, {nameof(_speed3)}: {_speed3}, {nameof(_speed4)}: {_speed4})";
        }
    }
}