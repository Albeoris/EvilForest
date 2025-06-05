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
            ["Actor.SetModel"] = Jsm.Opcode.MODEL,
            ["Actor.SetPosition"] = Jsm.Opcode.POS,
            ["Actor.SetAngle"] = Jsm.Opcode.DIRE,
            ["Actor.SetAnimation"] = Jsm.Opcode.AIDLE, // Default to AIDLE, will be determined by context
            ["Actor.SetRadius"] = Jsm.Opcode.RADIUS,
            ["Actor.SetIdleSpeed"] = Jsm.Opcode.ASPEED,
            
            // System service mappings
            ["System.Jump"] = Jsm.Opcode.JMP,
            ["System.Return"] = Jsm.Opcode.Return,
            ["System.Wait"] = Jsm.Opcode.WAIT,
            
            // Audio service mappings
            ["Audio.SongPlay"] = Jsm.Opcode.FLDSND0, 
            ["Audio.SongVolumeChange"] = Jsm.Opcode.FLDSND1,
            
            // Sps (Special effect positioning system) mappings
            ["Sps.SetReference"] = Jsm.Opcode.SPS,
            ["Sps.SetAttribute"] = Jsm.Opcode.SPS,
            ["Sps.SetPosition"] = Jsm.Opcode.SPS,
            ["Sps.SetRotation"] = Jsm.Opcode.SPS,
            ["Sps.SetScale"] = Jsm.Opcode.SPS,
            ["Sps.SetCharacter"] = Jsm.Opcode.SPS2,
            ["Sps.SetFade"] = Jsm.Opcode.SPS,
            ["Sps.SetAnimationRate"] = Jsm.Opcode.SPS,
            ["Sps.SetFrameRate"] = Jsm.Opcode.SPS,
            ["Sps.SetCurrentFrame"] = Jsm.Opcode.SPS,
            ["Sps.SetPositionOffset"] = Jsm.Opcode.SPS,
            ["Sps.SetDepthOffset"] = Jsm.Opcode.SPS,
            
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

        public static byte GetSpsOperationCode(string methodName)
        {
            return methodName switch
            {
                "SetReference" => 130,
                "SetAttribute" => 131,
                "SetPosition" => 135,
                "SetRotation" => 140,
                "SetScale" => 145,
                "SetCharacter" => 150,
                "SetFade" => 155,
                "SetAnimationRate" => 156,
                "SetFrameRate" => 160,
                "SetCurrentFrame" => 161,
                "SetPositionOffset" => 165,
                "SetDepthOffset" => 170,
                _ => 0 // Default fallback
            };
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
                Jsm.Opcode.SPS => new[]
                {
                    new ArgumentInfo("index", ArgumentType.Byte),
                    new ArgumentInfo("code", ArgumentType.Byte),
                    new ArgumentInfo("parameter1", ArgumentType.Int16),
                    new ArgumentInfo("parameter2", ArgumentType.Int16),
                    new ArgumentInfo("parameter3", ArgumentType.Int16)
                },
                Jsm.Opcode.SPS2 => new[]
                {
                    new ArgumentInfo("index", ArgumentType.Byte),
                    new ArgumentInfo("code", ArgumentType.Byte),
                    new ArgumentInfo("characterIndex", ArgumentType.Int16),
                    new ArgumentInfo("boneIndex", ArgumentType.Int16),
                    new ArgumentInfo("parameter3", ArgumentType.Int16)
                },
                Jsm.Opcode.NOP => Array.Empty<ArgumentInfo>(),
                Jsm.Opcode.MODEL => new[]
                {
                    new ArgumentInfo("modelId", ArgumentType.Int16),
                    new ArgumentInfo("height", ArgumentType.Byte)
                },
                Jsm.Opcode.POS => new[]
                {
                    new ArgumentInfo("positionX", ArgumentType.Int16),
                    new ArgumentInfo("positionY", ArgumentType.Int16)
                },
                Jsm.Opcode.DIRE => new[]
                {
                    new ArgumentInfo("angle", ArgumentType.Int16)
                },
                Jsm.Opcode.AIDLE => new[]
                {
                    new ArgumentInfo("animation", ArgumentType.Int16)
                },
                Jsm.Opcode.AWALK => new[]
                {
                    new ArgumentInfo("animation", ArgumentType.Int16)
                },
                Jsm.Opcode.ARUN => new[]
                {
                    new ArgumentInfo("animation", ArgumentType.Int16)
                },
                Jsm.Opcode.RADIUS => new[]
                {
                    new ArgumentInfo("radius", ArgumentType.Byte)
                },
                Jsm.Opcode.ASPEED => new[]
                {
                    new ArgumentInfo("speed", ArgumentType.Byte)
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