using System;
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
                private sealed class SwitchSegment : ExecutableSegment
                {
                    private readonly Switch _aggregator;
                    private readonly JMP_SWITCH _switch;

                    public SwitchSegment(Switch aggregator, Int32 from, Int32 to, JMP_SWITCH @switch)
                        : base(from, to)
                    {
                        _aggregator = aggregator;
                        _switch = @switch;
                    }
                    
                    public override IEnumerable<IJsmInstruction> EnumerateAllInstruction()
                    {
                        yield return _switch;
                        foreach (Segment caseBlock in _aggregator.EnumerateCaseBlocks())
                        {
                            foreach (IJsmInstruction instruction in caseBlock.EnumerateAllInstruction())
                                yield return instruction;
                        }
                    }

                    public override void ToString(StringBuilder sb)
                    {
                        sb.Append("switch(");
                        sb.Append(_switch);
                        sb.AppendLine(")");

                        foreach (var item in _aggregator.EnumerateCaseBlocks())
                            item.ToString(sb);
                    }

                    public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                    {
                        sw.Append("switch(");
                        _switch.Format(sw, formatterContext, services);
                        sw.AppendLine(")");
                        
                        sw.AppendLine("{");
                        sw.Indent++;

                        foreach (var item in _aggregator.EnumerateCaseBlocks())
                            item.Format(sw, formatterContext, services);
                        
                        sw.Indent--;
                        sw.AppendLine("}");
                    }

                    public override IScriptExecuter GetExecuter()
                    {
                        return new Executer(this);
                    }

                    public JMP_SWITCH JmpSwitch => ((JMP_SWITCH)_list[0]);
                }
            }
        }
    }
}