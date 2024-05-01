using System;
using System.Collections.Generic;
using System.Text;
using FF8.JSM.Instructions;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Control
        {
            public sealed partial class While : IJsmControl
            {
                private readonly List<JsmInstruction> _instructions;
                private readonly ExecutableSegment _segment;
                private readonly JMP_IF _if;

                internal While(List<JsmInstruction> instructions, JMP_IF @if, Int32 @from, Int32 to)
                {
                    _instructions = instructions;
                    _if = @if.Inverse();
                    if (from < to)
                    {
                        _segment = new WhileSegment(from, to);
                    }
                    else if (from > to)
                    {
                        _segment = new DoSegment(to, from);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                    _segment.Add(_if);
                }

                public override String ToString()
                {
                    StringBuilder sb = new StringBuilder();
                    
                    sb.Append("while(");
                    sb.Append(_if);
                    sb.AppendLine(")");

                    sb.AppendLine("{");
                    for (Int32 i = _segment.From + 1; i < _segment.To; i++)
                    {
                        JsmInstruction instruction = _instructions[i];
                        sb.Append('\t').AppendLine(instruction.ToString());
                    }

                    sb.AppendLine("}");

                    return base.ToString();
                }

                public IEnumerable<Segment> EnumerateSegments()
                {
                    yield return _segment;
                }
            }
        }
    }
}