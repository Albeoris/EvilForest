using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// CreateObject
    /// Place (or replace) the 3D model on the field.
    /// 
    /// 1st and 2nd arguments: position in (X, Y) format.
    /// AT_POSITION_X PositionX (2 bytes)
    /// AT_POSITION_Y PositionY (2 bytes)
    /// POS = 0x1D
    /// </summary>
    internal sealed class POS : JsmInstruction
    {
        private readonly IJsmExpression _positionX;
        private readonly IJsmExpression _positionY;

        private POS(IJsmExpression positionX, IJsmExpression positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression positionX = reader.ArgumentInt16();
            IJsmExpression positionY = reader.ArgumentInt16();
            
            return new POS(positionX, positionY);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Actors))
                .Method(nameof(IActorService.SetPosition))
                .Argument("positionX", _positionX)
                .Argument("positionY", _positionY)
                .Comment(nameof(POS));
        }
        
        public override String ToString()
        {
            return $"{nameof(POS)}({nameof(_positionX)}: {_positionX}, {nameof(_positionY)}: {_positionY})";
        }
    }
}