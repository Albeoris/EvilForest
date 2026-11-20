using System;
using System.Collections.Generic;

namespace Memoria.EventEngine.EV
{
    public readonly struct EVAddressCheckpoint
    {
        public Int32 ExpectedObjectIp { get; }
        public Int32 ScriptIp { get; }

        public EVAddressCheckpoint(Int32 expectedObjectIp, Int32 scriptIp)
        {
            ExpectedObjectIp = expectedObjectIp;
            ScriptIp = scriptIp;
        }

        public static void MarkOriginal(String eventName, EVContainerObject obj, Int32 index, EVInstructionBody body)
        {
            // EventEngine.DoEventCode checks actor.ip after reading the MSPEED operand on map 658.
            if (!eventName.Equals("EVT_KUINA_KM_SWP_2", StringComparison.OrdinalIgnoreCase) || obj.Id != 18 || obj.Scripts[index].Id != 19)
                return;

            Int32 scriptStart = GetScriptStart(obj, index);
            foreach (EVInstruction instruction in body.Instructions)
            {
                if (instruction.Opcode == FF8.JSM.Jsm.Opcode.MSPEED && scriptStart + instruction.Label + instruction.OriginalLength == 323)
                    instruction.WatchedObjectIp = 323;
            }
        }

        public static IEnumerable<String> GetChanges(EVContainer container)
        {
            foreach (EVContainerObject obj in container.Objects)
            {
                Int32 start = GetScriptStart(obj, 0);
                foreach (EVContainerScript script in obj.Scripts)
                {
                    foreach (EVAddressCheckpoint check in script.AddressChecks)
                    {
                        Int32 ip = start + check.ScriptIp;
                        if (ip != check.ExpectedObjectIp)
                            yield return $"Object {obj.Id}, script {script.Id}: address-sensitive MSPEED moved from object IP {check.ExpectedObjectIp} to {ip}.";
                    }
                    start += script.Body.Length;
                }
            }
        }

        private static Int32 GetScriptStart(EVContainerObject obj, Int32 index)
        {
            Int32 start = EVContainer.ObjectHeaderSize + obj.Scripts.Length * EVContainer.ScriptEntrySize + obj.Prefix.Length;
            for (Int32 i = 0; i < index; i++)
                start += obj.Scripts[i].Body.Length;
            return start;
        }
    }
}
