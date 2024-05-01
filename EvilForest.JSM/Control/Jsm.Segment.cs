using System;
using System.Collections;
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
        public abstract partial class Segment : IFormattableScript
        {
            public Int32 From { get; }
            public Int32 To { get; }

            protected readonly List<IJsmInstruction> _list = new List<IJsmInstruction>();

            public Segment(Int32 from, Int32 to)
            {
                From = from;
                To = to;
            }

            public void Add(IJsmInstruction value)
            {
                _list.Add(value);
            }

            public override String ToString()
            {
                StringBuilder sb = new StringBuilder();
                ToString(sb);
                return sb.ToString();
            }

            public virtual void ToString(StringBuilder sb)
            {
                foreach (var item in _list)
                    sb.AppendLine(item.ToString());
            }

            protected void FormatBranch(StringBuilder sb, IEnumerable items)
            {
                sb.AppendLine("{");

                Int32 pos = sb.Length;
                AppendItems(sb, items);
                if (sb.Length == pos)
                    sb.AppendLine("// do nothing");

                sb.AppendLine("}");
            }

            protected void FormatBranch(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices executionContext, IEnumerable<IJsmInstruction> items)
            {
                sw.AppendLine("{");
                sw.Indent++;

                var state = sw.RememberState();
                FormatItems(sw, formatterContext, executionContext, items);
                if (!state.IsChanged)
                {
                    sw.AppendLine("// do nothing");
                }

                sw.Indent--;
                sw.AppendLine("}");
            }

            private static void AppendItems(StringBuilder sb, IEnumerable items)
            {
                Int32 position = -1;
                JMP lastItem = null;
                foreach (var item in items)
                {
                    if (lastItem != null)
                        throw new InvalidProgramException($"Unexpected jump: {lastItem}");

                    lastItem = item as JMP;
                    position = sb.Length;

                    sb.Append('\t').AppendLine(item.ToString());
                }

                if (lastItem != null)
                    sb.Length = position;
            }

            public virtual IEnumerable<IJsmInstruction> EnumerateAllInstruction()
            {
                foreach (IJsmInstruction instruction in _list)
                {
                    // TODO: Change recursion to the loop
                    if (instruction is Segment nested)
                    {
                        foreach (IJsmInstruction nestedInstruction in nested.EnumerateAllInstruction())
                            yield return nestedInstruction;
                    }
                    else
                    {
                        yield return instruction;
                    }
                }
            }
            
            public IEnumerable<INameProvider> EnumerateNameProviders()
            {
                foreach (IJsmInstruction instruction in EnumerateAllInstruction())
                {
                    if (instruction is INameProvider nameProvider)
                        yield return nameProvider;
                }
            }

            public virtual void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
            {
                FormatItems(sw, formatterContext, services, _list);
            }

            protected static void FormatItems(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices executionContext, IEnumerable<IJsmInstruction> items)
            {
                foreach (var item in items)
                {
                    if (item is IFormattableScript formattable)
                        formattable.Format(sw, formatterContext, executionContext);
                    else
                        sw.AppendLine(item.ToString());
                }
            }
        }
    }
}