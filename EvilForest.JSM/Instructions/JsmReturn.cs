using System;

namespace FF8.JSM.Instructions
{
    internal sealed class JsmReturn : JsmInstruction
    {
        public JsmReturn()
        {
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            return new JsmReturn();
        }

        public override String ToString()
        {
            return $"yield break;";
        }
    }
}