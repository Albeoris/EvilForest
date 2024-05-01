using System;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// ATE
    /// Enable or disable ATE.
    /// 
    /// 1st argument: maybe flags (unknown format).
    /// AT_SPIN Unknown (1 bytes)
    /// AICON = 0x0D7,
    /// </summary>
    internal sealed class AICON : JsmInstruction
    {
        private readonly IJsmExpression _modeFlags;

        private AICON(IJsmExpression modeFlags)
        {
            _modeFlags = modeFlags;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression modeFlags = reader.ArgumentByte();
            return new AICON(modeFlags);
        }
        public override String ToString()
        {
            return $"{nameof(AICON)}({nameof(_modeFlags)}: {_modeFlags})";
        }
    }
}