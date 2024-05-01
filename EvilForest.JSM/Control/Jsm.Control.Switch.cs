using System;
using System.Collections.Generic;
using FF8.JSM.Instructions;

namespace FF8.JSM
{
    public static partial class Jsm
    {
        public static partial class Control
        {
            public sealed partial class Switch : IJsmControl
            {
                private readonly List<JsmInstruction> _instructions;
                private readonly SwitchSegment _segment;
                private readonly List<Segment> _cases = new List<Segment>();

                public Switch(List<JsmInstruction> instructions, Int32 from, Int32 to)
                {
                    _instructions = instructions;
                    _segment = new SwitchSegment(this, from, to, (JMP_SWITCH)instructions[from]);
                }

                public void AddCase(Boolean hasBreak, Int32 value, Int32 @from, Int32 to)
                {
                    if (from < _segment.From || to > _segment.To)
                        throw new ArgumentOutOfRangeException();
                    
                    CaseSegment caseSegment = new CaseSegment(hasBreak, value, from, to);
                    _cases.Add(caseSegment);
                }
                
                public void AddDefaultCase(Int32 from, Int32 to)
                {
                    if (from < _segment.From || to > _segment.To)
                        throw new ArgumentOutOfRangeException();
                    
                    DefaultSegment caseSegment = new DefaultSegment(from, to);
                    _cases.Add(caseSegment);
                }

                public override String ToString()
                {
                    return _segment.ToString();
                }

                public IEnumerable<Segment> EnumerateSegments()
                {
                    yield return _segment;
                    
                    foreach (var block in EnumerateCaseBlocks())
                        yield return block;
                }
                
                private IEnumerable<Segment> EnumerateCaseBlocks()
                {
                    foreach (var item in _cases)
                        yield return item;
                }
            }
        }
    }
}