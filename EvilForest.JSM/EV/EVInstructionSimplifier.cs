using System;
using System.Linq;
using FF8.JSM;

namespace Memoria.EventEngine.EV
{
    public static class EVInstructionSimplifier
    {
        public static Boolean CanShowWaitThenReturn(EVInstructionBody body, Int32 index)
        {
            if (index < 0 || index >= body.Instructions.Count - 1)
                return false;

            EVInstruction wait = body.Instructions[index];
            EVInstruction ret = body.Instructions[index + 1];
            return wait.Opcode == Jsm.Opcode.WAIT && ret.Opcode == Jsm.Opcode.Return &&
                !body.Instructions.Any(instruction => instruction.Targets.Contains(ret.Label));
        }

        public static EVInstruction[] ExpandWaitThenReturn(Int32 waitLabel, Int32 returnLabel, EVOperand frames, Byte mask)
        {
            return new[]
            {
                new EVInstruction(waitLabel, Jsm.Opcode.WAIT) { ArgumentMask = mask, Operands = new[] { frames } },
                new EVInstruction(returnLabel, Jsm.Opcode.Return)
            };
        }
    }
}
