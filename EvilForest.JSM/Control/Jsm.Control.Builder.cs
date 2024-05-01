﻿using System;
using System.Collections.Generic;
using FF8.JSM.Instructions;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Control
        {
            public sealed class Builder
            {
                public static IJsmControl[] Build(List<JsmInstruction> instructions)
                {
                    return new Builder(instructions).Make();
                }

                private readonly List<JsmInstruction> _instructions;
                private readonly ProcessedJumps _processed = new ProcessedJumps();
                private readonly List<IJsmControl> _result = new List<IJsmControl>();

                private Builder(List<JsmInstruction> instructions)
                {
                    _instructions = instructions;
                }

                private Int32 _index;
                private JMP_IF _begin;

                public IJsmControl[] Make()
                {
                    for (_index = 0; _index < _instructions.Count; _index++)
                    {
                        if (TryMakeSwitch())
                            continue;
                        
                        if (TryMakeGotoOrSkip())
                            continue;

                        if (TryMakeWhile())
                            continue;

                        if (TryMakeIf())
                            continue;

                        throw new InvalidProgramException($"Cannot recognize the logical block: {_begin}");
                    }

                    return _result.ToArray();
                }

                private Boolean TryMakeGotoOrSkip()
                {
                    var instruction = _instructions[_index];
                    if (instruction is JMP @goto && _processed.TryProcess(@goto))
                    {
                        var to = @goto.Index;
                        if (_instructions[to] is JMP_IF @if && @if.Index == _index + 1)
                        {
                            // [14] = {JMP} "JMP(Index: 17)"
                            // [15] = {Let} "Let(@evt.Byte_28[6] = 1)"
                            // [16] = {WAIT} "WAIT(_frameDuration: 1)"
                            // [17] = {JMP_IF} "!(JMP_IF[Index: 15, Conditions: ( Comparison(@evt.Byte_26 > 0) )])"

                            _processed.Process(@if);
                            _result.Add(new While(_instructions, @if, _index, to));
                            return true;
                        }
                        
                        
                        var control = new Goto(_instructions, _index, @goto.Index);
                        _result.Add(control);
                        return true;
                    }

                    if (!(instruction is JMP_IF jpf))
                        return true;

                    if (!_processed.TryProcess(jpf))
                        return true;

                    _begin = jpf;
                    return false;
                }
                
                private Boolean TryMakeSwitch()
                {
                    var instruction = _instructions[_index];
                    if (!(instruction is JMP_SWITCH swtch))
                        return false;

                    Int32 endOfSwitch = swtch.DefaultCase.Index;
                    
                    // Get end of switch
                    for (Int32 i = _index; i < endOfSwitch; i++)
                    {
                        if (_instructions[i] is JMP jmp && jmp.Index > endOfSwitch)
                            endOfSwitch = jmp.Index;
                    }
                    
                    // Process break jumps
                    for (Int32 i = _index; i < endOfSwitch; i++)
                    {
                        if (_instructions[i] is JMP jmp && jmp.Index == endOfSwitch)
                            _processed.TryProcess(jmp); // Switch-case in switch-case
                    }
                    
                    var control = new Switch(_instructions, _index, endOfSwitch);
                    _result.Add(control);

                    for (Int32 i = 0; i < swtch.ValueCases.Length; i++)
                    {
                        var @case = swtch.ValueCases[i];
                        
                        Int32 to;
                        if (i < swtch.ValueCases.Length - 1)
                            to = swtch.ValueCases[i + 1].Index - 1;
                        else
                            to = endOfSwitch - 1;
                        
                        Boolean hasBreak = _instructions[to] is JMP jmp && jmp.Index == endOfSwitch;
                        control.AddCase(hasBreak, @case.Value, @case.Index, to);

                        if (swtch.DefaultCase.Index != endOfSwitch)
                            control.AddDefaultCase(swtch.DefaultCase.Index, endOfSwitch);
                    }
                        
                    return true;

                }

                private Boolean TryMakeWhile()
                {
                    if (_begin.Index > 0 && _instructions[_begin.Index - 1] is JMP jmp && jmp.Index == _index)
                    {
                        var @if = (JMP_IF) _instructions[_index];
                        _processed.TryProcess(jmp);
                        _result.Add(new While(_instructions, @if, _index, _begin.Index));
                        return true;
                    }

                    return false;
                }

                private Boolean TryMakeIf()
                {
                    JMP_IF jmpIf = _begin;

                    If control = new If(_instructions, _index, _begin.Index);
                    _result.Add(control);

                    if (_begin.Index < 1)
                    {
                        // There is no JMP instruction. Simple if {}
                        return true;
                    }
                    
                    JMP jmp = _instructions[_begin.Index - 1] as JMP;

                    if (jmp == null || _processed.IsProcessed(jmp))
                    {
                        // There is no JMP instruction. Simple if {}
                        return true;
                    }

                    if (jmp.Index == jmpIf.Index)
                    {
                        // It isn't our jump, but an nested if. If { nested if{}<-}
                        return true;
                    }

                    if (jmp.Index < jmpIf.Index)
                    {
                        // It isn't our jump, but an nested loop. If { nested while{}<-}
                        return true;
                    }

                    if (jmp.Index < _index)
                    {
                        // It isn't our jump, but an nested goto. If { nested goto l;<-}
                        return true;
                    }

                    _processed.Process(jmp);
                    AddIfElseBranches(control, jmpIf, jmp);
                    return true;
                }

                private void AddIfElseBranches(If control, JMP_IF jmpIf, JMP jmp)
                {
                    Int32 endOfBlock = jmp.Index;

                    while (true)
                    {
                        Int32 newJpfIndex = jmpIf.Index;
                        JMP_IF newJmpIf = _instructions[newJpfIndex] as JMP_IF;
                        if (newJmpIf == null || newJmpIf.Index > endOfBlock)
                        {
                            control.AddElse(jmpIf.Index, endOfBlock);
                            return;
                        }

                        JMP newJmp = _instructions[newJmpIf.Index - 1] as JMP;
                        if (newJmp == null)
                        {
                            if (newJmpIf.Index == endOfBlock)
                            {
                                // if-elseif without jmp
                                _processed.Process(newJmpIf);
                                control.AddIf(newJpfIndex, newJmpIf.Index);
                            }
                            else
                            {
                                // if-else without jmp
                                control.AddElse(jmpIf.Index, endOfBlock);
                            }

                            return;
                        }

                        // Isn't our jump
                        if (newJmp.Index != endOfBlock)
                        {
                            control.AddElse(jmpIf.Index, endOfBlock);
                            return;
                        }

                        jmpIf = newJmpIf;
                        jmp = newJmp;
                        _processed.Process(jmpIf);
                        _processed.TryProcess(jmp);

                        control.AddIf(newJpfIndex, jmpIf.Index);

                        if (jmpIf.Index == endOfBlock)
                            return;
                    }
                }
            }
        }
    }
}