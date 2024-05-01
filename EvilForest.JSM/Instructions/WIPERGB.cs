using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// FadeFilter
    /// Apply a fade filter on the screen.
    /// 
    /// 1st argument: filter mode (0 for ADD, 2 for SUBTRACT).
    /// 2nd argument: fading time.
    /// 3rd argument: unknown.
    /// 4th to 6th arguments: color of the filter in (Cyan, Magenta, Yellow) format.
    /// AT_USPIN Fade In/Out (1 bytes)
    /// AT_USPIN Fading Time (1 bytes)
    /// AT_SPIN Unknown (1 bytes)
    /// AT_COLOR_CYAN ColorC (1 bytes)
    /// AT_COLOR_MAGENTA ColorM (1 bytes)
    /// AT_COLOR_YELLOW ColorY (1 bytes)
    /// WIPERGB = 0x0EC,
    /// </summary>
    internal sealed class WIPERGB : JsmInstruction
    {
        private readonly IJsmExpression _fadeInOut;
        private readonly IJsmExpression _durationInFrames;
        private readonly IJsmExpression _unknown;
        private readonly IJsmExpression _colorR;
        private readonly IJsmExpression _colorG;
        private readonly IJsmExpression _colorB;

        private WIPERGB(IJsmExpression fadeInOut, IJsmExpression durationInFrames, IJsmExpression unknown, IJsmExpression colorR, IJsmExpression colorG, IJsmExpression colorB)
        {
            _fadeInOut = fadeInOut;
            _durationInFrames = durationInFrames;
            _unknown = unknown; // unused
            _colorR = colorR;
            _colorG = colorG;
            _colorB = colorB;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression fadeInOut = reader.ArgumentByte();
            IJsmExpression fadingTime = reader.ArgumentByte();
            IJsmExpression unknown = reader.ArgumentByte(); // unused
            IJsmExpression colorR = reader.ArgumentByte();
            IJsmExpression colorG = reader.ArgumentByte();
            IJsmExpression colorB = reader.ArgumentByte();
            return new WIPERGB(fadeInOut, fadingTime, unknown, colorR, colorG, colorB);
        }

        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.Gui))
                .Method(nameof(IGuiService.Fade))
                .Enum<FadeFlags>(_fadeInOut)
                .Argument("durationInFrames", _durationInFrames)
                .Argument("colorR", _colorR)
                .Argument("colorG", _colorG)
                .Argument("colorB", _colorB)
                .Comment(nameof(WIPERGB));
        }

        public override String ToString()
        {
            return $"{nameof(WIPERGB)}({nameof(_fadeInOut)}: {_fadeInOut}, {nameof(_durationInFrames)}: {_durationInFrames}, {nameof(_unknown)}: {_unknown}, {nameof(_colorR)}: {_colorR}, {nameof(_colorG)}: {_colorG}, {nameof(_colorB)}: {_colorB})";
        }
    }
}