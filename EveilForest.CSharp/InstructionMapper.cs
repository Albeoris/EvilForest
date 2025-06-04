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
            
            // Character/Actor service mappings (examples)
            ["Actor.Move"] = Jsm.Opcode.MOVE,
            ["Actor.Turn"] = Jsm.Opcode.TURN,
            ["Actor.Wait"] = Jsm.Opcode.WAIT,
            ["Actor.Stop"] = Jsm.Opcode.STOP,
            ["Actor.Sleep"] = Jsm.Opcode.ASLEEP,
            
            // System service mappings
            ["System.Jump"] = Jsm.Opcode.JMP,
            ["System.Return"] = Jsm.Opcode.Return,
            ["System.Wait"] = Jsm.Opcode.WAIT,
            
            // Audio service mappings
            ["Audio.SongPlay"] = Jsm.Opcode.FLDSND0, 
            ["Audio.SongVolumeChange"] = Jsm.Opcode.FLDSND1,
            
            // Sps (Script positioning system?) mappings - using NOP as placeholder
            ["Sps.SetReference"] = Jsm.Opcode.NOP,
            ["Sps.SetPositionOffset"] = Jsm.Opcode.NOP,
            ["Sps.SetCharacter"] = Jsm.Opcode.NOP,
            ["Sps.SetAnimation"] = Jsm.Opcode.NOP,
            ["Sps.SetPos"] = Jsm.Opcode.NOP,
            ["Sps.SetSpeed"] = Jsm.Opcode.NOP,
            ["Sps.Move"] = Jsm.Opcode.NOP,
            ["Sps.Turn"] = Jsm.Opcode.NOP,
            ["Sps.Stop"] = Jsm.Opcode.NOP,
            ["Sps.SetVisible"] = Jsm.Opcode.NOP,
            ["Sps.SetLayer"] = Jsm.Opcode.NOP,
            
            // Variables service mappings
            ["Variables.Set"] = Jsm.Opcode.EXPR,
            ["Variables.Get"] = Jsm.Opcode.EXPR,
            
            // This service mappings (self-reference methods)
            ["This.Sleep"] = Jsm.Opcode.NOP, // Placeholder
            ["This.Stop"] = Jsm.Opcode.NOP, // Placeholder
            ["This.Move"] = Jsm.Opcode.NOP, // Placeholder
            
            // Add more mappings as needed for other common instructions
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
                Jsm.Opcode.MOVE => new[]
                {
                    new ArgumentInfo("x", ArgumentType.Int16),
                    new ArgumentInfo("y", ArgumentType.Int16),
                    new ArgumentInfo("z", ArgumentType.Int16)
                },
                Jsm.Opcode.TURN => new[]
                {
                    new ArgumentInfo("direction", ArgumentType.Byte)
                },
                Jsm.Opcode.WAIT => new[]
                {
                    new ArgumentInfo("frameDuration", ArgumentType.Byte)
                },
                Jsm.Opcode.FLDSND0 => new[]
                {
                    new ArgumentInfo("sound", ArgumentType.UInt16)
                },
                Jsm.Opcode.FLDSND1 => new[]
                {
                    new ArgumentInfo("sound", ArgumentType.UInt16),
                    new ArgumentInfo("volume", ArgumentType.Byte)
                },
                Jsm.Opcode.STOP => Array.Empty<ArgumentInfo>(),
                Jsm.Opcode.ASLEEP => new[]
                {
                    new ArgumentInfo("frames", ArgumentType.Byte)
                },
                Jsm.Opcode.JMP => new[]
                {
                    new ArgumentInfo("offset", ArgumentType.Int16)
                },
                Jsm.Opcode.Return => Array.Empty<ArgumentInfo>(),
                Jsm.Opcode.NOP => Array.Empty<ArgumentInfo>(),
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