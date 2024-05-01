using System;

namespace FF8.JSM.Instructions
{
    internal sealed class NOP : JsmInstruction
    {
        public NOP()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new NOP();
        }

        public override String ToString()
        {
            return $"{nameof(NOP)}()";
        }
    }
}