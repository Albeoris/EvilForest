using System;
using System.Collections.Generic;
using FF8.JSM;

namespace EveilForest.CSharp
{
    internal static class InstructionMapper
    {
        private static readonly Dictionary<string, Jsm.Opcode> ServiceMethodToOpcode = new()
        {
            // Messages service mappings
            ["Messages.ShowAndWait"] = Jsm.Opcode.MES,
            ["Messages.Show"] = Jsm.Opcode.MESN,
            ["Messages.Wait"] = Jsm.Opcode.WAITMES,
        };

        public static bool TryGetOpcode(string serviceName, string methodName, out Jsm.Opcode opcode)
        {
            string key = $"{serviceName}.{methodName}";
            return ServiceMethodToOpcode.TryGetValue(key, out opcode);
        }

        public static ArgumentInfo[] GetArgumentInfo(Jsm.Opcode opcode)
        {
            return opcode switch
            {
                Jsm.Opcode.MES => new[]
                {
                    new ArgumentInfo("windowId", ArgumentType.Byte),
                    new ArgumentInfo("ui", ArgumentType.Byte),
                    new ArgumentInfo("text", ArgumentType.Int16)
                },
                Jsm.Opcode.MESN => new[]
                {
                    new ArgumentInfo("windowId", ArgumentType.Byte),
                    new ArgumentInfo("ui", ArgumentType.Byte),
                    new ArgumentInfo("text", ArgumentType.Int16)
                },
                Jsm.Opcode.WAITMES => new[]
                {
                    new ArgumentInfo("windowId", ArgumentType.Byte)
                },
                _ => throw new NotSupportedException($"Opcode {opcode} is not supported for compilation")
            };
        }
    }

    public enum ArgumentType
    {
        Byte,
        SByte,
        Int16,
        UInt16,
        Int24,
        Int32
    }

    public record ArgumentInfo(string Name, ArgumentType Type);
}