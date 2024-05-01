﻿using System;
using System.Collections.Generic;
using FF8.JSM.Instructions;

 namespace FF8.JSM
{
    public static partial class Jsm
    {
        public sealed class LabelBuilder
        {
            private readonly Dictionary<Int32, List<IJumpToOpcode>> _targets = new Dictionary<Int32, List<IJumpToOpcode>>();
            private Dictionary<Int32, IndexedInstruction> _candidates = new Dictionary<Int32, IndexedInstruction>();

            private readonly UInt16 _maxCount;
            
            public Int32 AdditionalNopNumber { get; private set; }

            public LabelBuilder(UInt16 maxCount)
            {
                _maxCount = maxCount;
            }

            public void TraceInstruction(Int32 position, Int32 label, IndexedInstruction instruction)
            {
                _candidates.Add(label, instruction);

                if (instruction.Instruction is IJumpToOpcode jump)
                {
                    TraceJumpInstruction(position, jump, true);
                }
                else if (instruction.Instruction is JMP_SWITCH swtch)
                {
                    TraceJumpInstruction(position, swtch.DefaultCase, false, canJumpOutOfSwitch: true);
                    
                    foreach (var @case in swtch.ValueCases)
                        TraceJumpInstruction(position, @case, false);
                }
            }

            private void TraceJumpInstruction(Int32 position, IJumpToOpcode jump, Boolean isRelative, Boolean canJumpOutOfSwitch = false)
            {
                Int32 target = jump.Offset;
                if (isRelative)
                    target += position;
                
                if (target < 0)
                    throw new InvalidProgramException($"Trying to jump out of script ({position} -> {target}). The field \"test3.jsm\" isn't supported.");

                if (canJumpOutOfSwitch)
                {
                    if (target >= _maxCount + AdditionalNopNumber)
                        AdditionalNopNumber = target - _maxCount + 1;
                }
                else
                {
                    if (target >= _maxCount)
                        throw new InvalidProgramException($"Trying to jump out of script ({position} -> {target}).");
                }

                if (!_targets.TryGetValue(target, out var list))
                {
                    list = new List<IJumpToOpcode>();
                    _targets.Add(target, list);
                }

                list.Add(jump);
            }

            public HashSet<Int32> Commit()
            {
                HashSet<Int32> result = new HashSet<Int32>();

                foreach (var pair in _targets)
                {
                    var offset = pair.Key;
                    if (!_candidates.TryGetValue(offset, out IndexedInstruction target))
                        throw new InvalidProgramException($"Invalid jump target: {pair.Key}");

                    foreach (var jump in pair.Value)
                        jump.Index = target.Index;

                    result.Add(target.Index);
                }

                return result;
            }
        }
    }
}