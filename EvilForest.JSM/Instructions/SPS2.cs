﻿using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// RunSPSCodeSimple
    /// Run Sps code, which seems to be special model effects on the field.
    /// 
    /// 1st argument: sps ID.
    /// 2nd argument: sps code.
    /// 3rd to 5th arguments: depend on the sps code.
    ///  Load Sps (sps type)
    ///  Enable Attribute (attribute list, boolean enable/disable)
    ///  Set Position (X, -Z, Y)
    ///  Set Rotation (angle X, angle Z, angle Y)
    ///  Set Scale (scale factor)
    ///  Attach (object's entry to attach, bone number)
    ///  Set Fade (fade)
    ///  Set Animation Rate (rate)
    ///  Set Frame Rate (rate)
    ///  Set Frame (value) where the value is factored by 16 to get the frame
    ///  Set Position Offset (X, -Z, Y)
    ///  Set Depth Offset (depth)
    /// AT_USPIN Sps (1 bytes)
    /// AT_SPSCODE Code (1 bytes)
    /// AT_SPIN Parameter 1 (1 bytes)
    /// AT_SPIN Parameter 2 (2 bytes)
    /// AT_SPIN Parameter 3 (2 bytes)
    /// SPS2 = 0x0DA,
    /// </summary>
    internal sealed class SPS2 : JsmInstruction
    {
        private readonly IJsmExpression _sps;
        private readonly IConstExpression _code;
        private readonly IJsmExpression _parameter1;
        private readonly IJsmExpression _parameter2;
        private readonly IJsmExpression _parameter3;

        private SPS2(IJsmExpression sps, IConstExpression code, IJsmExpression parameter1, IJsmExpression parameter2, IJsmExpression parameter3)
        {
            _sps = sps;
            _code = code;
            _parameter1 = parameter1;
            _parameter2 = parameter2;
            _parameter3 = parameter3;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression sps = reader.ArgumentByte();
            IConstExpression code = (IConstExpression) reader.ArgumentByte();
            IJsmExpression parameter1 = reader.ArgumentByte();
            IJsmExpression parameter2 = reader.ArgumentInt16();
            IJsmExpression parameter3 = reader.ArgumentInt16();
            return new SPS2(sps, code, parameter1, parameter2, parameter3);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            SPS.Format(sw, formatterContext, services, _code, _sps, _parameter1, _parameter2, _parameter3);
        }
        
        public override String ToString()
        {
            return $"{nameof(SPS2)}({nameof(_sps)}: {_sps}, {nameof(_code)}: {_code}, {nameof(_parameter1)}: {_parameter1}, {nameof(_parameter2)}: {_parameter2}, {nameof(_parameter3)}: {_parameter3})";
        }
    }
}