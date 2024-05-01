using System;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// TerminateEntry
    /// Stop the execution of an entry's code.
    ///
    /// 1st argument: entry to terminate.
    /// DELETE = 0x01C,
    /// </summary>
    internal sealed class DELETE : JsmInstruction
    {
        private readonly IJsmExpression _continue;

        private DELETE(IJsmExpression @continue)
        {
            _continue = @continue;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression entry = reader.ArgumentByte();
            return new DELETE(@entry);
        }

        public override String ToString()
        {
            return $"{nameof(DELETE)}({nameof(_continue)}: {_continue})";
        }
    }
}