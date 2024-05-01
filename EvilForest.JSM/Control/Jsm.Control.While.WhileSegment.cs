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
                private sealed class WhileSegment : ExecutableSegment
                {
                    public WhileSegment(Int32 from, Int32 to)
                        : base(from, to)
                    {
                    }
                    
                    public override IEnumerable<IJsmInstruction> EnumerateAllInstruction()
                    {
                        yield return JmpIf;
                        foreach (IJsmInstruction instruction in GetBodyInstructions())
                            yield return instruction;
                    }

                    public override void ToString(StringBuilder sb)
                    {
                        sb.Append("while(");
                        sb.Append(JmpIf);
                        sb.AppendLine(")");

                        FormatBranch(sb, GetBodyInstructions());
                    }

                    public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
                    {
                        sw.Append("while(");
                        JmpIf.Format(sw, formatterContext, services);
                        sw.AppendLine(")");
                        FormatBranch(sw, formatterContext, services, GetBodyInstructions());
                    }

                    public override IScriptExecuter GetExecuter()
                    {
                        return new Executer(this);
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