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
            /// <summary>
            /// A loop terminated by an unconditional backward JMP. This commonly
            /// wraps another control such as a switch, so it must be represented as
            /// a segment instead of being flattened to an orphaned goto.
            /// </summary>
            public sealed class InfiniteWhile : IJsmControl
            {
                private readonly InfiniteWhileSegment _segment;

                public InfiniteWhile(List<JsmInstruction> instructions, Int32 from, Int32 jumpIndex)
                {
                    _segment = new InfiniteWhileSegment(from, jumpIndex);
                }

                public IEnumerable<Segment> EnumerateSegments()
                {
                    yield return _segment;
                }

                private sealed class InfiniteWhileSegment : ExecutableSegment
                {
                    public InfiniteWhileSegment(Int32 from, Int32 to) : base(from, to)
                    {
                    }

                    public override void ToString(StringBuilder sb)
                    {
                        sb.AppendLine("while(true)");
                        FormatBranch(sb, _list);
                    }

                    public override void Format(ScriptWriter sw,
                        IScriptFormatterContext formatterContext, IServices services)
                    {
                        sw.AppendLine("while(true)");
                        FormatBranch(sw, formatterContext, services, _list);
                    }
                }
            }
        }
    }
}
