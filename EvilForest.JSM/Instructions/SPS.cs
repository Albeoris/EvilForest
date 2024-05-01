using System;
using System.Text;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// RunSPSCode
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
    /// AT_SPIN Parameter 1 (2 bytes)
    /// AT_SPIN Parameter 2 (2 bytes)
    /// AT_SPIN Parameter 3 (2 bytes)
    /// SPS = 0x0B3,
    /// </summary>
    internal sealed class SPS : JsmInstruction
    {
        private readonly IJsmExpression _sps;
        private readonly IConstExpression _code;
        private readonly IJsmExpression _parameter1;
        private readonly IJsmExpression _parameter2;
        private readonly IJsmExpression _parameter3;

        private SPS(IJsmExpression sps, IConstExpression code, IJsmExpression parameter1, IJsmExpression parameter2, IJsmExpression parameter3)
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
            IConstExpression code = (IConstExpression)reader.ArgumentByte();
            IJsmExpression parameter1 = reader.ArgumentInt16();
            IJsmExpression parameter2 = reader.ArgumentInt16();
            IJsmExpression parameter3 = reader.ArgumentInt16();
            return new SPS(sps, code, parameter1, parameter2, parameter3);
        }

        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            Format(sw, formatterContext, services, _code, _sps, _parameter1, _parameter2, _parameter3);
        }
        
        public static void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services, IConstExpression opCode, IJsmExpression sps, IJsmExpression parameter1, IJsmExpression parameter2, IJsmExpression parameter3)
        {
            OpCode code = (OpCode) opCode.Int32();
            FormatHelper.MethodFormatter formatter;
            
            switch (code)
            {
                case OpCode.SetReference:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetReference))
                        .Argument("index", sps)
                        .Argument("referenceIndex", parameter1);
                    break;
                }
                case OpCode.SetAttribute:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetAttribute))
                        .Argument("index", sps)
                        .Argument("isSet", parameter2)
                        .Argument("value", parameter1);
                    break;
                }
                case OpCode.SetPosition:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetPosition))
                        .Argument("index", sps)
                        .Argument("x", parameter1)
                        .Argument("y", parameter2)
                        .Argument("z", parameter3);
                    break;
                }
                case OpCode.SetRotation:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetRotation))
                        .Argument("index", sps)
                        .Argument("x", parameter1)
                        .Argument("y", parameter2)
                        .Argument("z", parameter3);
                    break;
                }
                case OpCode.SetScale:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetScale))
                        .Argument("index", sps)
                        .Argument("scale", parameter1);
                    break;
                }
                case OpCode.SetCharacter:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetCharacter))
                        .Argument("index", sps)
                        .Argument("characterIndex", parameter1)
                        .Argument("boneIndex", parameter2);
                    break;
                }
                case OpCode.SetFade:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetFade))
                        .Argument("index", sps)
                        .Argument("fade", parameter1);
                    break;
                }
                case OpCode.SetAnimationRate:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetAnimationRate))
                        .Argument("index", sps)
                        .Argument("rate", parameter1);
                    break;
                }
                case OpCode.SetFrameRate:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetFrameRate))
                        .Argument("index", sps)
                        .Argument("rate", parameter1);
                    break;
                }
                case OpCode.SetCurrentFrame:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetCurrentFrame))
                        .Argument("index", sps)
                        .Argument("frame", parameter1);
                    break;
                }
                case OpCode.SetPositionOffset:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetPositionOffset))
                        .Argument("index", sps)
                        .Argument("offset", parameter1);
                    break;
                }
                case OpCode.SetDepthOffset:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.SpsField))
                        .Method(nameof(ISpsFieldService.SetDepthOffset))
                        .Argument("index", sps)
                        .Argument("offset", parameter1);
                    break;
                }
                default:
                {
                    throw new NotSupportedException(code.ToString());
                }
            }

            formatter.Comment($"{nameof(SPS)}({sps}, {code}, {parameter1}, {parameter2}, {parameter3})");
        }
        
        public override String ToString()
        {
            return $"{nameof(SPS)}({nameof(_sps)}: {_sps}, {nameof(_code)}: {_code}, {nameof(_parameter1)}: {_parameter1}, {nameof(_parameter2)}: {_parameter2}, {nameof(_parameter3)}: {_parameter3})";
        }

        private enum OpCode
        {
            SetReference = 130,
            SetAttribute = 131,
            SetPosition = 135,
            SetRotation = 140,
            SetScale = 145,
            SetCharacter = 150,
            SetFade = 155,
            SetAnimationRate = 156,
            SetFrameRate = 160,
            SetCurrentFrame = 161,
            SetPositionOffset = 165,
            SetDepthOffset = 170
        }
    }
}