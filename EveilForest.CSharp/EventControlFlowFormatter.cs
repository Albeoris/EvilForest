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
    private readonly List<(int Indent, string Text, int? Label, int? Instruction)> _lines = new();
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
    private IReadOnlyDictionary<int, string>? _caseTargets;
    private int? _continueTarget;

    public IEnumerable<string> SwitchLayouts
    {
        get
        {
            foreach (var group in _switchLayouts.GroupBy(layout => layout.Key))
            {
                string[] layouts = group.Select(layout => layout.Attribute).ToArray();
                if (layouts.All(layout => layout == layouts[0]))
                {
                    yield return layouts[0];
                    continue;
                }
                foreach (string layout in layouts)
                    yield return layout;
            }
        }
    }
    public bool ExpandSwitchRegions { get; init; } = true;
    public Func<EVExpressionToken, int, bool>? PreserveSwitchPhase { get; init; }
    public bool HasExpandedSwitchRegions { get; private set; }
    public bool HasUnstructuredTargets => _explicitTargets.Count > 0;
    public bool UseNativeJumps { get; init; }
    public IReadOnlySet<int>? UnreachablePollingExitLabels { get; init; }
    public IReadOnlySet<int>? LinkedPollingDispatchLabels { get; init; }
    public bool HasRawTransfer { get; private set; }
    public int NativeGotoCount { get; private set; }
    public Dictionary<int, int> InstructionOffsets { get; } = new();

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

        foreach (var (indent, text, label, instruction) in _lines)
        {
            if (label.HasValue && !_explicitTargets.Contains(label.Value))
                continue;
            if (instruction.HasValue)
                InstructionOffsets.Add(source.Length + indent * 4, instruction.Value);
            source.Append(' ', indent * 4).AppendLine(text);
        }
    }

    private void WriteRange(int start, int end, int indent, bool structured = true, bool markFirst = true, int? trailingBreakTarget = null)
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
            if (UseNativeJumps && TryWriteCaseJump(index, end, indent, out next))
            {
                index = next;
                continue;
            }
            if (structured && TryWriteConditional(index, end, indent, out next))
            {
                index = next;
                continue;
            }
            if (structured && TryWriteSwitch(index, end, indent, trailingBreakTarget, out next))
            {
                index = next;
                continue;
            }

            EVInstruction instruction = _instructions[index];
            if (structured && instruction.Opcode == Jsm.Opcode.JMP && instruction.Targets[0] == _continueTarget)
            {
                Line(indent, "continue;", instruction.Label);
                index++;
                continue;
            }

            if (UseNativeJumps && TryWriteNativeJump(index, end, indent, out next))
            {
                index = next;
                continue;
            }

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
            Line(indent, terminalReturn ? "return;" : _formatInstruction(instruction) + ";", instruction.Label);
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
            NativeGotoCount++;
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
            NativeGotoCount++;
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
            string selector = _formatSwitchValue(instruction.Expression);
            Line(indent, $"switch ({selector})");
            Line(indent, "{");
            foreach (var group in sections)
            {
                foreach (var entry in group)
                    Line(indent + 1, $"case {FormatCase(selector, entry.Value)}:");
                Line(indent + 2, $"goto {LabelName(group.Key)};");
            }
            Line(indent + 1, "default:");
            Line(indent + 2, $"goto {LabelName(transfer.Targets[0])};");
            Line(indent, "}");
            NativeGotoCount += sections.Length + 1;
        }
        else
        {
            return false;
        }

        next += 2;
        return true;
    }

    private bool TryWriteCaseJump(int index, int end, int indent, out int next)
    {
        next = index;
        if (_caseTargets == null)
            return false;

        EVInstruction instruction = _instructions[index];
        if (instruction.Opcode == Jsm.Opcode.JMP && _caseTargets.TryGetValue(instruction.Targets[0], out string? destination))
        {
            Line(indent, $"goto {destination};");
            next++;
        }
        else if (instruction.Opcode == Jsm.Opcode.EXPR && index + 1 < end && _incoming[index + 1].Count == 0 &&
            _instructions[index + 1] is { Opcode: Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN } transfer && _caseTargets.TryGetValue(transfer.Targets[0], out destination))
        {
            Line(indent, $"if ({FormatCondition(instruction.Expression, transfer.Opcode == Jsm.Opcode.JMP_IFN)})");
            Line(indent + 1, $"goto {destination};");
            next += 2;
        }
        else
        {
            return false;
        }

        NativeGotoCount++;
        return true;
    }

    public string LabelName(int label)
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
            if (tail <= start || tail + 1 != limit || _instructions[tail].Opcode != Jsm.Opcode.JMP || HasEntryFromOutside(start, tail + 1))
                continue;
            Line(indent, "while (true)");
            Line(indent, "{");
            WriteLoopRange(start, tail, indent + 1, tail + 1, markFirst: false);
            Mark(tail, indent + 1);
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
            if (condition > start + 1 && condition + 1 < limit && IsLoopTail(start + 1, condition))
            {
                int end = condition + 2;
                if (!HasEntryFromOutside(start, end))
                {
                    Line(indent, $"for (; {FormatCondition(_instructions[condition].Expression, false)};)", _instructions[condition].Label);
                    Line(indent, "{");
                    WriteLoopRange(start + 1, condition, indent + 1, end, continuation: condition);
                    Mark(condition, indent + 1);
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
            WriteLoopRange(start, condition, indent + 1, branch + 1, markFirst: false, continuation: condition);
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
        if (start + 2 > limit || _instructions[start].Opcode != Jsm.Opcode.EXPR || _incoming[start + 1].Count != 0)
            return false;

        EVInstruction branch = _instructions[start + 1];
        if (branch.Opcode is not (Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN))
            return false;
        int alternate = _positions[branch.Targets[0]];
        if (alternate < start + 2 || alternate > limit || HasEntryFromOutside(start, alternate))
            return false;

        string test = FormatCondition(_instructions[start].Expression, branch.Opcode == Jsm.Opcode.JMP_IF);

        EVInstruction tail = _instructions[alternate - 1];
        if (alternate > start + 2 && tail.Opcode == Jsm.Opcode.JMP)
        {
            int tailTarget = _positions[tail.Targets[0]];
            if (tailTarget == start && alternate > start + 3 && _incoming[alternate - 1].Count == 0)
            {
                Line(indent, $"while ({test})");
                Line(indent, "{");
                WriteLoopRange(start + 2, alternate - 1, indent + 1, alternate);
                Line(indent, "}");
                next = alternate;
                return true;
            }

            if (tailTarget > alternate && tailTarget <= limit && _caseTargets?.ContainsKey(tail.Targets[0]) != true &&
                !HasEntryFromOutside(start, tailTarget) &&
                CanSeparate(start + 2, alternate, tailTarget))
            {
                // Branches inside the then arm may reach its implicit jump before leaving the arm.
                Line(indent, $"if ({test})", _instructions[start].Label);
                WriteBlock(start + 2, alternate - 1, indent, markEnd: true);
                Line(indent, "else");
                WriteBlock(alternate, tailTarget, indent);
                next = tailTarget;
                return true;
            }
        }

        Line(indent, $"if ({test})", _instructions[start].Label);
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

    private bool TryWriteSwitch(int start, int limit, int indent, int? trailingBreakTarget, out int next)
    {
        next = start;
        if (start + 2 >= limit || _instructions[start].Opcode != Jsm.Opcode.EXPR || _incoming[start + 1].Count != 0 ||
            LinkedPollingDispatchLabels?.Contains(_instructions[start].Label) == true)
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
        bool allowExtendedExits = ExpandSwitchRegions;
        bool acceptCaseEntries = allowExtendedExits && UseNativeJumps;
        bool threadDefault = allowExtendedExits && trailingBreakTarget == defaultPosition && defaultPosition > limit;
        if (threadDefault && isExplicit && cases.ContainsKey(defaultPosition))
            return false;
        if (threadDefault)
            cases.Remove(defaultPosition);
        if (cases.Count == 0)
            return false;

        int[] starts = threadDefault ? cases.Keys.ToArray() : cases.Keys.Append(defaultPosition).Distinct().Order().ToArray();
        if (starts[0] < start + 2 || starts[^1] > limit)
            return false;
        bool hasUnreachablePrefix = starts[0] > start + 2;
        if (hasUnreachablePrefix && (!allowExtendedExits || !IsUnenteredSwitchPrefix(start + 2, starts[0])))
            return false;

        var joins = new SortedSet<int> { threadDefault ? limit : defaultPosition };
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

        bool addedEnclosingJoin = allowExtendedExits && trailingBreakTarget.HasValue && joins.Add(limit);
        var candidateJoins = ExpandSwitchRegions ? joins.OrderBy(join => HasUnstructuredSwitchTail(join, start, limit, trailingBreakTarget)) : joins.AsEnumerable();
        foreach (int join in candidateJoins)
        {
            if (join < cases.Keys.Last() || join > limit || (threadDefault ? join != limit : join < defaultPosition) || HasEntryFromOutside(start, join))
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
                if (!SectionHasSingleEntry(sectionStart, sectionEnd, start + 1, acceptCaseEntries ? join : null))
                {
                    valid = false;
                    break;
                }

                EVInstruction last = _instructions[sectionEnd - 1];
                if (last.Opcode == Jsm.Opcode.JMP && _positions[last.Targets[0]] == join)
                    endings[index] = SwitchEnding.Jump;
                else if (acceptCaseEntries && last.Opcode == Jsm.Opcode.JMP && _positions[last.Targets[0]] != sectionStart &&
                    sections.Contains(_positions[last.Targets[0]]))
                    endings[index] = SwitchEnding.CaseTransfer;
                else if (allowExtendedExits && join == limit && last.Opcode == Jsm.Opcode.JMP && _positions[last.Targets[0]] == trailingBreakTarget)
                    endings[index] = SwitchEnding.ThreadedJump;
                else if (last.Opcode == Jsm.Opcode.Return)
                    endings[index] = SwitchEnding.Return;
                else if (allowExtendedExits && index + 1 < sections.Length && last.Opcode == Jsm.Opcode.MAPJUMP &&
                    (acceptCaseEntries || !_incoming[sectionEnd].Any(source => source >= sectionStart && source < sectionEnd)))
                    endings[index] = SwitchEnding.FieldTransfer;
                else if (UnreachablePollingExitLabels?.Contains(last.Label) == true)
                    endings[index] = SwitchEnding.UnreachableExit;
                else if (index + 1 == sections.Length)
                    endings[index] = SwitchEnding.FallThrough;
                else if (acceptCaseEntries && last.Opcode is not (Jsm.Opcode.JMP or Jsm.Opcode.JMP_SWITCH or Jsm.Opcode.JMP_SWITCHEX or Jsm.Opcode.rsv0d))
                    endings[index] = SwitchEnding.CaseFallThrough;
                else
                {
                    valid = false;
                    break;
                }
            }
            if (!valid)
                continue;

            HasExpandedSwitchRegions |= ExpandSwitchRegions && (hasUnreachablePrefix || threadDefault || addedEnclosingJoin && join == limit ||
                endings.Any(ending => ending is SwitchEnding.FieldTransfer or SwitchEnding.ThreadedJump or SwitchEnding.CaseTransfer or SwitchEnding.CaseFallThrough) ||
                joins.Any(earlier => earlier < join && HasUnstructuredSwitchTail(earlier, start, limit, trailingBreakTarget)));
            WriteSwitch(table.Opcode, start, indent, cases, values, defaultPosition, sections, endings, join, threadDefault, hasUnreachablePrefix);
            next = join;
            return true;
        }
        return false;
    }

    private bool IsUnenteredSwitchPrefix(int start, int end)
    {
        // The dispatch has no physical fallthrough and method entry is before it.
        for (int index = start; index < end; index++)
        {
            if (_incoming[index].Count != 0 || _instructions[index].Targets.Length != 0)
                return false;
        }
        return true;
    }

    private bool HasUnstructuredSwitchTail(int join, int start, int limit, int? trailingBreakTarget)
    {
        if (join <= start + 2 || join > limit || _instructions[join - 1].Opcode != Jsm.Opcode.JMP)
            return false;
        int target = _positions[_instructions[join - 1].Targets[0]];
        return target != join && !(join == limit && target == trailingBreakTarget);
    }

    private void WriteSwitch(Jsm.Opcode opcode, int start, int indent, SortedDictionary<int, List<int>> cases, int[] values, int defaultPosition, int[] sections, SwitchEnding[] endings, int join, bool threadDefault, bool hasUnreachablePrefix)
    {
        string encoding = opcode switch
        {
            Jsm.Opcode.JMP_SWITCH => "Dense",
            Jsm.Opcode.JMP_SWITCHEX => "Explicit",
            _ => "Wide"
        };
        EVExpressionToken[] expression = _instructions[start].Expression;
        bool fallsThrough = endings[^1] == SwitchEnding.FallThrough;
        (int[] Values, int Fallback)?[] guardedCases = FindGuardedCases(start + 1, cases, sections, defaultPosition);
        int[][] sectionCases = sections.Select((position, index) =>
        {
            IEnumerable<int> ownCases = cases.GetValueOrDefault(position) ?? [];
            IEnumerable<int> fallbackCases = guardedCases.OfType<(int[] Values, int Fallback)>().Where(guard => guard.Fallback == index).SelectMany(guard => guard.Values);
            return fallbackCases.Concat(ownCases).ToArray();
        }).ToArray();
        int mergedCase = !hasUnreachablePrefix && !threadDefault && guardedCases.All(guard => guard == null)
            ? FindMergedCase(start + 1, expression, sections, endings, cases, defaultPosition) : -1;
        if (mergedCase >= 0)
            sectionCases[mergedCase + 1] = sectionCases[mergedCase].Concat(sectionCases[mergedCase + 1]).ToArray();
        string key = SwitchLayout.Key(expression, Enumerable.Range(0, sections.Length).Where(index => index != mergedCase).Select(index =>
            SwitchLayout.Section(sectionCases[index], sections[index] == defaultPosition, endings[index] == SwitchEnding.Return)));
        int[] transferExits = Enumerable.Range(0, endings.Length).Where(index => endings[index] == SwitchEnding.FieldTransfer).ToArray();
        int[] threadedExits = Enumerable.Range(0, endings.Length).Where(index => endings[index] == SwitchEnding.ThreadedJump).ToArray();
        int[] caseFallThrough = Enumerable.Range(0, endings.Length).Where(index => index != mergedCase && endings[index] == SwitchEnding.CaseFallThrough).ToArray();
        AddSwitchLayout(key, encoding, values, fallsThrough, transferExits, threadedExits, threadDefault, caseFallThrough, hasUnreachablePrefix, mergedCase);
        string selector = _formatSwitchValue(expression);
        Line(indent, $"switch ({selector})");
        Line(indent, "{");
        IReadOnlyDictionary<int, string>? previousCaseTargets = _caseTargets;
        for (int index = 0; index < sections.Length; index++)
        {
            if (index == mergedCase)
                continue;
            int position = sections[index];
            int bodyStart = position;
            if (guardedCases[index] is { } guard)
            {
                string test = FormatCondition(_instructions[position].Expression, _instructions[position + 1].Opcode == Jsm.Opcode.JMP_IF);
                string patterns = string.Join(" or ", guard.Values.Select(value => FormatCase(selector, value)));
                Line(indent + 1, $"case {patterns} when {test}:");
                bodyStart += 2;
            }
            else
            {
                foreach (int value in sectionCases[index])
                    Line(indent + 1, $"case {FormatCase(selector, value)}:");
            }
            if (position == defaultPosition)
                Line(indent + 1, "default:");

            int end = index + 1 == sections.Length ? join : sections[index + 1];
            SwitchEnding ending = endings[index];
            int? previousBreakTarget = _breakTarget;
            _breakTarget = LabelAt(join);
            _caseTargets = null;
            if (UseNativeJumps && ExpandSwitchRegions)
            {
                var targets = new Dictionary<int, string>();
                foreach (int entry in sections)
                {
                    if (entry != position && _incoming[entry].Any(source => source >= position && source < end))
                    {
                        int label = LabelAt(entry);
                        targets.Add(label, FormatSectionTarget(label, selector, cases));
                    }
                }
                _caseTargets = targets;
            }
            bool trailingJump = ending is SwitchEnding.Jump or SwitchEnding.ThreadedJump or SwitchEnding.CaseTransfer;
            if (index == 0 && hasUnreachablePrefix)
            {
                Line(indent + 2, "if (false)");
                Line(indent + 2, "{");
                WriteRange(start + 2, position, indent + 3, structured: false);
                Line(indent + 2, "}");
            }
            if (mergedCase >= 0 && index == mergedCase + 1)
            {
                int[] prefixCases = sectionCases[mergedCase];
                string test = string.Join(" || ", prefixCases.Select(caseValue => $"{selector} == {FormatCase(selector, caseValue)}"));
                Line(indent + 2, $"if ({test})");
                WriteBlock(sections[mergedCase], position, indent + 2);
            }
            int? trailingTarget = ending switch
            {
                SwitchEnding.Jump => join,
                SwitchEnding.CaseTransfer => _positions[_instructions[end - 1].Targets[0]],
                _ => null
            };
            WriteRange(bodyStart, trailingJump ? end - 1 : end, indent + 2, trailingBreakTarget: trailingTarget);
            _breakTarget = previousBreakTarget;
            if (trailingJump)
                Mark(end - 1, indent + 2);
            if (ending is SwitchEnding.CaseTransfer or SwitchEnding.CaseFallThrough)
            {
                int target = ending == SwitchEnding.CaseTransfer ? _instructions[end - 1].Targets[0] : LabelAt(end);
                Line(indent + 2, $"goto {FormatSectionTarget(target, selector, cases)};");
                NativeGotoCount++;
            }
            else if (ending != SwitchEnding.Return)
                Line(indent + 2, "break;");
        }
        _caseTargets = previousCaseTargets;
        Line(indent, "}");
    }

    private int FindMergedCase(int dispatch, EVExpressionToken[] expression, int[] sections, SwitchEnding[] endings, SortedDictionary<int, List<int>> cases, int defaultPosition)
    {
        if (expression is not [var selector] || !StorageDeclarations.IsStorage(selector) || EVVariable.Decode(selector).Scope == EVStorageScope.Special ||
            endings.Any(ending => ending is SwitchEnding.FieldTransfer or SwitchEnding.ThreadedJump or SwitchEnding.CaseTransfer or SwitchEnding.UnreachableExit))
            return -1;

        int index = Array.IndexOf(endings, SwitchEnding.CaseFallThrough);
        if (index < 0 || Array.LastIndexOf(endings, SwitchEnding.CaseFallThrough) != index || !cases.ContainsKey(sections[index]) || sections[index] == defaultPosition)
            return -1;

        int start = sections[index];
        int end = sections[index + 1];
        if (_incoming[start].Any(source => source != dispatch) || _incoming[end].Any(source => source != dispatch && (source < start || source >= end)))
            return -1;
        if (PreserveSwitchPhase is { } preserve && (cases[start].Any(value => preserve(selector, value)) ||
            cases.TryGetValue(end, out List<int>? continuationCases) && continuationCases.Any(value => preserve(selector, value))))
            return -1;
        return _instructions.Skip(start).Take(end - start).SelectMany(instruction => instruction.Targets).All(target => _positions[target] >= start && _positions[target] <= end) ? index : -1;
    }

    private (int[] Values, int Fallback)?[] FindGuardedCases(int dispatch, SortedDictionary<int, List<int>> cases, int[] sections, int defaultPosition)
    {
        var guarded = new (int[] Values, int Fallback)?[sections.Length];
        if (!UseNativeJumps || !ExpandSwitchRegions)
            return guarded;

        for (int index = 0; index + 1 < sections.Length; index++)
        {
            int position = sections[index];
            if (position == defaultPosition || !cases.TryGetValue(position, out List<int>? values) ||
                position + 2 >= sections[index + 1] || _instructions[position].Opcode != Jsm.Opcode.EXPR ||
                _incoming[position].Any(source => source != dispatch) || _incoming[position + 1].Count != 0)
                continue;
            EVInstruction branch = _instructions[position + 1];
            if (branch.Opcode is not (Jsm.Opcode.JMP_IF or Jsm.Opcode.JMP_IFN))
                continue;
            int fallback = Array.IndexOf(sections, _positions[branch.Targets[0]], index + 1);
            if (fallback >= 0)
                guarded[index] = (values.ToArray(), fallback);
        }
        return guarded;
    }

    private void AddSwitchLayout(string key, string encoding, int[] values, bool fallsThrough, int[]? transferExits = null, int[]? threadedExits = null, bool threadDefault = false, int[]? caseFallThrough = null, bool hasUnreachablePrefix = false, int mergedCase = -1)
    {
        string order = string.Join(",", values);
        string transferExit = transferExits is { Length: > 0 } ? $", TransferExitSections = \"{string.Join(",", transferExits)}\"" : "";
        string threadedExit = threadedExits is { Length: > 0 } ? $", ThreadedExitSections = \"{string.Join(",", threadedExits)}\"" : "";
        string threadedDefault = threadDefault ? ", ThreadDefaultExit = true" : "";
        string fallThrough = caseFallThrough is { Length: > 0 } ? $", FallThroughSections = \"{string.Join(",", caseFallThrough)}\"" : "";
        string unreachablePrefix = hasUnreachablePrefix ? ", HasUnreachablePrefix = true" : "";
        string mergedSection = mergedCase >= 0 ? $", MergedCaseSection = {mergedCase}" : "";
        _switchLayouts.Add((key, $"    [SwitchLayout(\"{key}\", SwitchTable.{encoding}, \"{order}\", {fallsThrough.ToString().ToLowerInvariant()}{transferExit}{threadedExit}{threadedDefault}{fallThrough}{unreachablePrefix}{mergedSection})]"));
    }

    private static string FormatCase(string selector, int value) =>
        selector.EndsWith(".Facing.Sector8", StringComparison.Ordinal)
            ? value is >= 0 and < 8 ? $"FacingSector.Sector{value}" : $"(FacingSector){value}"
            : value.ToString(System.Globalization.CultureInfo.InvariantCulture);

    private string FormatSectionTarget(int label, string selector, SortedDictionary<int, List<int>> cases) =>
        cases.TryGetValue(_positions[label], out List<int>? values) ? $"case {FormatCase(selector, values[0])}" : "default";

    private bool CanSeparate(int thenStart, int elseStart, int elseEnd) =>
        SectionHasSingleEntry(thenStart, elseStart, thenStart - 1) &&
        SectionHasSingleEntry(elseStart, elseEnd, thenStart - 1);

    private bool SectionHasSingleEntry(int start, int end, int dispatch, int? switchEnd = null)
    {
        for (int index = start; index < end; index++)
        {
            foreach (int source in _incoming[index])
            {
                if (source >= start && source < end || index == start && source == dispatch)
                    continue;
                if (index == start && switchEnd.HasValue && source > dispatch && source < switchEnd.Value)
                    continue;
                return false;
            }
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

    private void WriteLoopRange(int start, int end, int indent, int exit, bool markFirst = true, int? continuation = null)
    {
        int? previousBreakTarget = _breakTarget;
        int? previousContinueTarget = _continueTarget;
        _breakTarget = LabelAt(exit);
        _continueTarget = continuation is int position ? LabelAt(position) : null;
        WriteRange(start, end, indent, markFirst: markFirst);
        _breakTarget = previousBreakTarget;
        _continueTarget = previousContinueTarget;
    }

    private int LabelAt(int position) => position == _instructions.Count ? _body.EndLabel : _instructions[position].Label;

    private void WriteBlock(int start, int end, int indent, bool markEnd = false)
    {
        Line(indent, "{");
        WriteRange(start, end, indent + 1);
        if (markEnd)
            Mark(end, indent + 1);
        Line(indent, "}");
    }

    private void Mark(int position, int indent)
    {
        if (_incoming[position].Count == 0)
            return;
        int label = LabelAt(position);
        string text = UseNativeJumps ? $"{LabelName(label)}: ;" : $"Label(L_{label:X4});";
        _lines.Add((indent, text, label, null));
    }

    private void Line(int indent, string text, int? instruction = null) => _lines.Add((indent, text, null, instruction));

    private enum SwitchEnding { Jump, Return, FallThrough, UnreachableExit, FieldTransfer, ThreadedJump, CaseTransfer, CaseFallThrough }
}
