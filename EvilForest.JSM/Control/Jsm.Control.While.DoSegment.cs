using System;
using System.Collections.Generic;
using System.Linq;
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
            public sealed partial class While
            {
                private sealed class DoSegment : ExecutableSegment
                {
                    public DoSegment(Int32 from, Int32 to)
                        : base(from, to)
                    {
                    }

                    public override IEnumerable<IJsmInstruction> EnumerateAllInstruction()
                    {
                        foreach (IJsmInstruction instruction in GetBodyInstructions())
                            yield return instruction;
                        yield return JmpIf;
                    }

                    public override void ToString(StringBuilder sb)
                    {
                        sb.AppendLine("do");

                        FormatBranch(sb, GetBodyInstructions());
                        
                        sb.Append("while(");
                        sb.Append(JmpIf);
                        sb.AppendLine(");");
                    }

                    public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                    {
                        sw.AppendLine("do");
                        
                        FormatBranch(sw, formatterContext, services, GetBodyInstructions());
                        
                        sw.Append("while(");
                        JmpIf.Format(sw, formatterContext, services);
                        sw.AppendLine(");");
                    }

                    public override IScriptExecuter GetExecuter()
                    {
                        throw new NotSupportedException();
                        // return new Executer(this);
                    }

                    public JMP_IF JmpIf => ((JMP_IF)_list[0]);

                    public IEnumerable<IJsmInstruction> GetBodyInstructions()
                    {
                        return _list.Skip(1);
                    }
                }
            }
        }
    }
}