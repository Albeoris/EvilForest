using System.Text;
using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed class EventControlFlowFormatter
{
    private readonly EVInstructionBody _body;
    private readonly IList<EVInstruction> _instructions;
    private readonly int _instructionCount;
    private readonly Dictionary<int, int> _positions;
    private readonly List<int>[] _incoming;
    private readonly List<(int Indent, string Text, int? Label)> _lines = new();
    private readonly HashSet<int> _explicitTargets = new();
    private readonly Func<EVInstruction, string> _formatInstruction;
    private readonly Func<EVExpressionToken[], string> _formatValue;
    private readonly Func<EVOperand, string> _formatOperand;
    private readonly bool _simplify;
    private readonly Func<EVExpressionToken[], string?> _formatCondition;
    private readonly Func<EVExpressionToken[], string> _formatSwitchValue;
    private readonly Func<EVInstruction, IEnumerable<string>> _annotations;
    private readonly List<(string Key, string Attribute)> _switchLayouts = new();
    private int? _breakTarget;

    public IEnumerable<string> SwitchLayouts => _switchLayouts.Select(layout => layout.Attribute).Distinct();
    public bool HasConflictingSwitchLayouts => _switchLayouts.GroupBy(layout => layout.Key).Any(group => group.Select(layout => layout.Attribute).Distinct().Count() > 1);
    public bool UseSwitchLayouts { get; init; } = true;
    public bool HasUnstructuredTargets => _explicitTargets.Count > 0;
    public bool UseNativeJumps { get; init; }
    public bool HasRawTransfer { get; private set; }

    public bool CanWriteNativeJumps()
    {
        for (int index = 0; index < _instructionCount; index++)
        {
            EVInstruction instruction = _instructions[index];
            if (instruction.Targets.Length == 0 || instruction.Opcode == Jsm.Opcode.JMP)
                continue;
            if (instruction.Opcode is not (Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN or Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d) ||
                index == 0 || _instructions[index - 1].Opcode != Jsm.Opcode.EXPR || _incoming[index].Count != 0)
                return false;
            if (instruction.Opcode is (Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d) && instruction.Targets.Length < 2 ||
                instruction.Opcode == Jsm.Opcode.JMP_SWITCHEX && instruction.CaseValues.Distinct().Count() != instruction.CaseValues.Length)
                return false;
        }
        return true;
    }

    public EventControlFlowFormatter(EVInstructionBody body, int instructionCount, Func<EVInstruction, string> formatInstruction, Func<EVExpressionToken[], string> formatValue, Func<EVOperand, string> formatOperand, Func<EVExpressionToken[], string?> formatCondition, Func<EVExpressionToken[], string> formatSwitchValue, bool simplify, Func<EVInstruction, IEnumerable<string>> annotations)
    {
        _body = body;
        _instructions = body.Instructions;
        _instructionCount = instructionCount;
        _formatInstruction = formatInstruction;
        _formatValue = formatValue;
        _formatOperand = formatOperand;
        _simplify = simplify;
        _formatCondition = formatCondition;
        _formatSwitchValue = formatSwitchValue;
        _annotations = annotations;
        _positions = _instructions.Select((instruction, index) => (instruction.Label, index)).ToDictionary(item => item.Label, item => item.index);
        _positions.Add(body.EndLabel, _instructions.Count);
        _incoming = Enumerable.Range(0, _instructions.Count + 1).Select(_ => new List<int>()).ToArray();
        for (int index = 0; index < _instructions.Count; index++)
        {
            foreach (int target in _instructions[index].Targets)
                _incoming[_positions[target]].Add(index);
        }
    }

    public void AppendTo(StringBuilder source, bool structured)
    {
        WriteRange(0, _instructionCount, 2, structured);
        Mark(_instructionCount, 2);
        if (!UseNativeJumps && _explicitTargets.Count != 0)
        {
            foreach (int target in _explicitTargets.Order())
                source.AppendLine($"        const int L_{target:X4} = {target};");
            source.AppendLine();
        }

        foreach (var (indent, text, label) in _lines)
        {
            if (label.HasValue && !_explicitTargets.Contains(label.Value))
                continue;
            source.Append(' ', indent * 4).AppendLine(text);
        }
    }

    private void WriteRange(int start, int end, int indent, bool structured = true, bool markFirst = true)
    {
        for (int index = start; index < end;)
        {
            if (index != start || markFirst)
                Mark(index, indent);
            if (structured && TryWriteInfiniteLoop(index, end, indent, out int next))
            {
                index = next;
                continue;
            }
            if (structured && TryWriteTailLoop(index, end, indent, out next))
            {
                index = next;
                continue;
            }
            if (structured && TryWriteConditional(index, end, indent, out next))
            {
                index = next;
                continue;
            }
            if (structured && TryWriteSwitch(index, end, indent, out next))
            {
                index = next;
                continue;
            }

            if (UseNativeJumps && TryWriteNativeJump(index, end, indent, out next))
            {
                index = next;
                continue;
            }

            EVInstruction instruction = _instructions[index];
            if (instruction.WatchedObjectIp is int expectedIp)
                Line(indent, $"WatchAddress({expectedIp});");
            if (_simplify && index + 1 < end && EVInstructionSimplifier.CanShowWaitThenReturn(_body, index))
            {
                Line(indent, $"await WaitThenReturn(frames: {_formatOperand(instruction.Operands[0])}, mask: 0x{instruction.ArgumentMask:X2});");
                index += 2;
                continue;
            }

            foreach (string annotation in _annotations(instruction))
                Line(indent, $"// {annotation}");
            if (structured && instruction.Opcode == Jsm.Opcode.JMP && instruction.Targets[0] == _breakTarget)
            {
                Line(indent, "break;");
                index++;
                continue;
            }
            HasRawTransfer |= instruction.Targets.Length != 0;
            _explicitTargets.UnionWith(instruction.Targets);
            bool terminalReturn = instruction.Opcode == Jsm.Opcode.Return && (UseNativeJumps || structured && index + 1 == end);
            Line(indent, terminalReturn ? "return;" : _formatInstruction(instruction) + ";");
            index++;
        }
    }

    private bool TryWriteNativeJump(int index, int end, int indent, out int next)
    {
        next = index;
        EVInstruction instruction = _instructions[index];
        if (instruction.Opcode == Jsm.Opcode.JMP)
        {
            _explicitTargets.Add(instruction.Targets[0]);
            Line(indent, $"goto {LabelName(instruction.Targets[0])};");
            next++;
            return true;
        }
        if (instruction.Opcode != Jsm.Opcode.EXPR || index + 1 >= end)
            return false;

        EVInstruction transfer = _instructions[index + 1];
        if (transfer.Opcode is Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN)
        {
            _explicitTargets.Add(transfer.Targets[0]);
            Line(indent, $"if ({FormatCondition(instruction.Expression, transfer.Opcode == Jsm.Opcode.JMP_IFN)})");
            Line(indent + 1, $"goto {LabelName(transfer.Targets[0])};");
        }
        else if (transfer.Opcode is Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d)
        {
            bool explicitValues = transfer.Opcode == Jsm.Opcode.JMP_SWITCHEX;
            string table = explicitValues ? "Explicit" : transfer.Opcode == Jsm.Opcode.JMP_SWITCH ? "Dense" : "Wide";
            int[] values = explicitValues ? transfer.CaseValues.Select(value => (int)value).ToArray()
                : Enumerable.Range(unchecked((short)transfer.CaseValues[0]), transfer.Targets.Length - 1).ToArray();
            _explicitTargets.UnionWith(transfer.Targets);
            var sections = values.Select((value, position) => (Value: value, Target: transfer.Targets[position + 1])).GroupBy(item => item.Target).ToArray();
            string key = SwitchLayout.Key(instruction.Expression, sections.Select(group =>
                SwitchLayout.Section(group.Select(item => item.Value), false, false)).Append(SwitchLayout.Section(Array.Empty<int>(), true, false)), directDispatch: true);
            AddSwitchLayout(key, table, values, false);
            Line(indent, $"switch ({_formatSwitchValue(instruction.Expression)})");
            Line(indent, "{");
            foreach (var group in sections)
            {
                foreach (var entry in group)
                    Line(indent + 1, $"case {entry.Value}:");
                Line(indent + 2, $"goto {LabelName(group.Key)};");
            }
            Line(indent + 1, "default:");
            Line(indent + 2, $"goto {LabelName(transfer.Targets[0])};");
            Line(indent, "}");
        }
        else
        {
            return false;
        }

        next += 2;
        return true;
    }

    private string LabelName(int label)
    {
        int position = _positions[label];
        if (position == _instructions.Count)
            return "EndHandler";
        string operation = _instructions[position].Opcode.ToString();
        if (!char.IsLetter(operation[0]))
            operation = "Instruction";
        int ordinal = _incoming.Take(position + 1).Count(sources => sources.Count != 0);
        return $"ResumeAt{operation}_{ordinal}";
    }

    private bool TryWriteInfiniteLoop(int start, int limit, int indent, out int next)
    {
        next = start;
        foreach (int tail in _incoming[start].AsEnumerable().Reverse())
        {
            if (tail <= start || tail + 1 != limit || _instructions[tail].Opcode != Jsm.Opcode.JMP ||
                _incoming[tail].Count != 0 || HasEntryFromOutside(start, tail + 1))
                continue;
            Line(indent, "while (true)");
            Line(indent, "{");
            WriteLoopRange(start, tail, indent + 1, tail + 1, markFirst: false);
            Line(indent, "}");
            next = tail + 1;
            return true;
        }
        return false;
    }

    private bool TryWriteTailLoop(int start, int limit, int indent, out int next)
    {
        next = start;
        EVInstruction first = _instructions[start];
        if (first.Opcode == Jsm.Opcode.JMP)
        {
            int condition = _positions[first.Targets[0]];
            if (condition > start + 1 && condition + 1 < limit &&
                _incoming[condition].Count == 1 && IsLoopTail(start + 1, condition))
            {
                int end = condition + 2;
                if (!HasEntryFromOutside(start, end))
                {
                    Line(indent, $"for (; {FormatCondition(_instructions[condition].Expression, false)};)");
                    Line(indent, "{");
                    WriteLoopRange(start + 1, condition, indent + 1, end);
                    Line(indent, "}");
                    next = end;
                    return true;
                }
            }
        }

        foreach (int branch in _incoming[start])
        {
            int condition = branch - 1;
            if (condition <= start || branch >= limit ||
                !IsLoopTail(start, condition) || HasEntryFromOutside(start, branch + 1))
                continue;

            Line(indent, "do");
            Line(indent, "{");
            WriteLoopRange(start, condition, indent + 1, branch + 1, markFirst: false);
            Mark(condition, indent + 1);
            Line(indent, "}");
            Line(indent, $"while ({FormatCondition(_instructions[condition].Expression, false)});");
            next = branch + 1;
            return true;
        }
        return false;
    }

    private bool IsLoopTail(int entry, int condition) =>
        _instructions[condition].Opcode == Jsm.Opcode.EXPR &&
        _instructions[condition + 1].Opcode == Jsm.Opcode.JMP_IF &&
        _incoming[condition + 1].Count == 0 &&
        _positions[_instructions[condition + 1].Targets[0]] == entry;

    private bool TryWriteConditional(int start, int limit, int indent, out int next)
    {
        next = start;
        if (start + 2 >= limit || _instructions[start].Opcode != Jsm.Opcode.EXPR || _incoming[start + 1].Count != 0)
            return false;

        EVInstruction branch = _instructions[start + 1];
        if (branch.Opcode is not (Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN))
            return false;
        int alternate = _positions[branch.Targets[0]];
        if (alternate < start + 2 || alternate > limit || HasEntryFromOutside(start, alternate))
            return false;

        string test = FormatCondition(_instructions[start].Expression, branch.Opcode == Jsm.Opcode.JMP_IF);

        EVInstruction tail = _instructions[alternate - 1];
        if (alternate > start + 2 && tail.Opcode == Jsm.Opcode.JMP && _incoming[alternate - 1].Count == 0)
        {
            int tailTarget = _positions[tail.Targets[0]];
            if (tailTarget == start && alternate > start + 3)
            {
                Line(indent, $"while ({test})");
                Line(indent, "{");
                WriteLoopRange(start + 2, alternate - 1, indent + 1, alternate);
                Line(indent, "}");
                next = alternate;
                return true;
            }

            if (tailTarget > alternate && tailTarget <= limit &&
                !HasEntryFromOutside(start, tailTarget) &&
                CanSeparate(start + 2, alternate - 1, alternate, tailTarget))
            {
                Line(indent, $"if ({test})");
                WriteBlock(start + 2, alternate - 1, indent);
                Line(indent, "else");
                WriteBlock(alternate, tailTarget, indent);
                next = tailTarget;
                return true;
            }
        }

        Line(indent, $"if ({test})");
        WriteBlock(start + 2, alternate, indent);
        next = alternate;
        return true;
    }

    private string FormatCondition(EVExpressionToken[] tokens, bool inverted)
    {
        if (_formatCondition(tokens) is string condition)
            return inverted ? $"!({condition})" : condition;
        string test = $"((int)({_formatValue(tokens)})) is not 0";
        return inverted ? $"!({test})" : test;
    }

    private bool TryWriteSwitch(int start, int limit, int indent, out int next)
    {
        next = start;
        if (start + 2 >= limit || _instructions[start].Opcode != Jsm.Opcode.EXPR || _incoming[start + 1].Count != 0)
            return false;
        EVInstruction table = _instructions[start + 1];
        if (table.Opcode is not (Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d) || table.Targets.Length < 2)
            return false;

        bool isExplicit = table.Opcode == Jsm.Opcode.JMP_SWITCHEX;
        int first = isExplicit ? 0 : unchecked((short)table.CaseValues[0]);
        int[] values = isExplicit ? table.CaseValues.Select(value => (int)value).ToArray() : Enumerable.Range(first, table.Targets.Length - 1).ToArray();
        if (values.Distinct().Count() != values.Length)
            return false;

        var cases = new SortedDictionary<int, List<int>>();
        for (int index = 0; index < values.Length; index++)
        {
            int position = _positions[table.Targets[index + 1]];
            if (!cases.TryGetValue(position, out List<int>? atPosition))
            {
                atPosition = new List<int>();
                cases.Add(position, atPosition);
            }
            atPosition.Add(values[index]);
        }

        int defaultPosition = _positions[table.Targets[0]];
        int[] starts = cases.Keys.Append(defaultPosition).Distinct().Order().ToArray();
        if (starts[0] != start + 2 || starts[^1] > limit)
            return false;

        var joins = new SortedSet<int> { defaultPosition };
        for (int index = 1; index < starts.Length; index++)
        {
            EVInstruction tail = _instructions[starts[index] - 1];
            if (tail.Opcode == Jsm.Opcode.JMP)
                joins.Add(_positions[tail.Targets[0]]);
        }

        for (int index = starts[^1]; index < limit; index++)
        {
            EVInstruction candidate = _instructions[index];
            if (candidate.Opcode == Jsm.Opcode.Return)
            {
                joins.Add(index + 1);
                break;
            }
            if (candidate.Opcode == Jsm.Opcode.JMP && _positions[candidate.Targets[0]] == index + 1)
            {
                joins.Add(index + 1);
                break;
            }
        }

        foreach (int join in joins)
        {
            if (join < cases.Keys.Last() || join < defaultPosition || join > limit || HasEntryFromOutside(start, join))
                continue;
            if (join == cases.Keys.Last() && (join != defaultPosition || isExplicit))
                continue;
            int[] sections = starts.Where(position => position < join).ToArray();
            if (sections.Length == 0)
                continue;
            var endings = new SwitchEnding[sections.Length];
            bool valid = true;
            for (int index = 0; index < sections.Length; index++)
            {
                int sectionStart = sections[index];
                int sectionEnd = index + 1 == sections.Length ? join : sections[index + 1];
                if (!SectionHasSingleEntry(sectionStart, sectionEnd, start + 1))
                {
                    valid = false;
                    break;
                }

                EVInstruction last = _instructions[sectionEnd - 1];
                if (last.Opcode == Jsm.Opcode.JMP && _positions[last.Targets[0]] == join)
                    endings[index] = SwitchEnding.Jump;
                else if (last.Opcode == Jsm.Opcode.Return)
                    endings[index] = SwitchEnding.Return;
                else if (index + 1 == sections.Length)
                    endings[index] = SwitchEnding.FallThrough;
                else
                {
                    valid = false;
                    break;
                }
            }
            if (!valid)
                continue;

            WriteSwitch(table.Opcode, start, indent, cases, values, defaultPosition, sections, endings, join);
            next = join;
            return true;
        }
        return false;
    }

    private void WriteSwitch(Jsm.Opcode opcode, int start, int indent, SortedDictionary<int, List<int>> cases, int[] values, int defaultPosition, int[] sections, SwitchEnding[] endings, int join)
    {
        string encoding = opcode switch
        {
            Jsm.Opcode.JMP_SWITCH => "Dense",
            Jsm.Opcode.JMP_SWITCHEX => "Explicit",
            _ => "Wide"
        };
        EVExpressionToken[] expression = _instructions[start].Expression;
        bool fallsThrough = endings[^1] == SwitchEnding.FallThrough;
        int[] sourceOrder = cases.Where(item => item.Key < join).SelectMany(item => item.Value).ToArray();
        string selector = _formatValue(expression);
        if (UseSwitchLayouts)
        {
            string key = SwitchLayout.Key(expression, sections.Select((position, index) =>
                SwitchLayout.Section(cases.GetValueOrDefault(position) ?? new List<int>(), position == defaultPosition, endings[index] == SwitchEnding.Return)));
            AddSwitchLayout(key, encoding, values, fallsThrough);
            selector = _formatSwitchValue(expression);
        }
        else
        {
            selector = $"Select(SwitchTable.{encoding}, {selector}";
            if (fallsThrough)
                selector += ", lastCaseFallsThrough: true";
            if (!values.SequenceEqual(sourceOrder))
                selector += $", caseOrder: new int[] {{ {string.Join(", ", values)} }}";
            selector += ")";
        }
        Line(indent, $"switch ({selector})");
        Line(indent, "{");
        for (int index = 0; index < sections.Length; index++)
        {
            int position = sections[index];
            if (cases.TryGetValue(position, out List<int>? labels))
            {
                foreach (int value in labels)
                    Line(indent + 1, $"case {value}:");
            }
            if (position == defaultPosition)
                Line(indent + 1, "default:");

            int end = index + 1 == sections.Length ? join : sections[index + 1];
            SwitchEnding ending = endings[index];
            int? previousBreakTarget = _breakTarget;
            _breakTarget = LabelAt(join);
            WriteRange(position, ending == SwitchEnding.Jump ? end - 1 : end, indent + 2);
            _breakTarget = previousBreakTarget;
            if (ending == SwitchEnding.Jump)
                Mark(end - 1, indent + 2);
            if (ending != SwitchEnding.Return)
                Line(indent + 2, "break;");
        }
        Line(indent, "}");
    }

    private void AddSwitchLayout(string key, string encoding, int[] values, bool fallsThrough)
    {
        string order = string.Join(",", values);
        _switchLayouts.Add((key, $"    [SwitchLayout(\"{key}\", SwitchTable.{encoding}, \"{order}\", {fallsThrough.ToString().ToLowerInvariant()})]"));
    }

    private bool CanSeparate(int thenStart, int thenEnd, int elseStart, int elseEnd) =>
        SectionHasSingleEntry(thenStart, thenEnd, thenStart - 1) &&
        SectionHasSingleEntry(elseStart, elseEnd, thenStart - 1);

    private bool SectionHasSingleEntry(int start, int end, int dispatch)
    {
        for (int index = start; index < end; index++)
        {
            if (_incoming[index].Any(source => (source < start || source >= end) && (index != start || source != dispatch)))
                return false;
        }
        return true;
    }

    private bool HasEntryFromOutside(int start, int end)
    {
        for (int index = start + 1; index < end; index++)
        {
            if (_incoming[index].Any(source => source < start || source >= end))
                return true;
        }
        return false;
    }

    private void WriteLoopRange(int start, int end, int indent, int exit, bool markFirst = true)
    {
        int? previous = _breakTarget;
        _breakTarget = LabelAt(exit);
        WriteRange(start, end, indent, markFirst: markFirst);
        _breakTarget = previous;
    }

    private int LabelAt(int position) => position == _instructions.Count ? _body.EndLabel : _instructions[position].Label;

    private void WriteBlock(int start, int end, int indent)
    {
        Line(indent, "{");
        WriteRange(start, end, indent + 1);
        Line(indent, "}");
    }

    private void Mark(int position, int indent)
    {
        if (_incoming[position].Count == 0)
            return;
        int label = LabelAt(position);
        string text = UseNativeJumps ? $"{LabelName(label)}: ;" : $"Label(L_{label:X4});";
        _lines.Add((indent, text, label));
    }

    private void Line(int indent, string text) => _lines.Add((indent, text, null));

    private enum SwitchEnding { Jump, Return, FallThrough }
}
