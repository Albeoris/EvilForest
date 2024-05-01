﻿using System;
using System.Collections.Generic;
using System.Text;
using FF8.Core;
using FF8.JSM.Format;
using FF8.JSM.Instructions;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Control
        {
            public sealed partial class Switch
            {
                private sealed class CaseSegment : Segment
                {
                    public readonly Boolean HasBreak;
                    public readonly Int32 Value;
                    
                    public CaseSegment(Boolean hasBreak, Int32 value, Int32 @from, Int32 to)
                        : base(from, to)
                    {
                        HasBreak = hasBreak;
                        Value = value;
                    }
                    
                    public override IEnumerable<IJsmInstruction> EnumerateAllInstruction()
                    {
                        foreach (IJsmInstruction instruction in GetBodyInstructions())
                            yield return instruction;
                    }

                    public override void ToString(StringBuilder sb)
                    {
                        sb.Append("case ");
                        sb.Append(Value);
                        sb.Append(':');
                        FormatBranch(sb, GetBodyInstructions());
                    }

                    public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                    {
                        sw.Append("case ");
                        sw.Append(Value.ToString());
                        sw.Append(":");
                        FormatBody(sw, formatterContext, services, GetBodyInstructions());
                    }

                    private void FormatBody(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices executionContext, IEnumerable<IJsmInstruction> items)
                    {
                        sw.AppendLine("{");
                        sw.Indent++;

                        var state = sw.RememberState();
                        FormatItems(sw, formatterContext, executionContext, items);
                        if (!state.IsChanged)
                            sw.AppendLine("// do nothing");

                        if (HasBreak)
                            sw.AppendLine("break;");
                        
                        sw.Indent--;
                        sw.AppendLine("}");
                    }

                    public IEnumerable<IJsmInstruction> GetBodyInstructions()
                    {
                        return _list; // TODO skip last
                    }
                }
            }
        }
    }
}