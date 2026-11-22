using System;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM;
using FF8.JSM.Format;

namespace Memoria.EventEngine.EV
{
    public sealed class EVScript
    {
        public Int32 Id;
        private readonly Lazy<Jsm.ExecutableSegment> _segment;
        public Jsm.ExecutableSegment Segment => _segment.Value;
        public EVInstructionBody Instructions { get; }

        public EVScript(UInt16 id, Jsm.ExecutableSegment segment, EVInstructionBody instructions = null)
        {
            Id = id;
            _segment = new Lazy<Jsm.ExecutableSegment>(() => segment);
            Instructions = instructions;
        }

        public EVScript(UInt16 id, EVInstructionBody instructions)
        {
            Id = id;
            Instructions = instructions;
            _segment = new Lazy<Jsm.ExecutableSegment>(() => EVFileReader.ParseScript(instructions.Encode()));
        }
        
        public void FormatMethod(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices executionContext)
        {
            String methodName = GetMethodName(formatterContext);

            sw.AppendLine($"public IEnumerable<IAwaitable> {methodName}()");
            {
                sw.AppendLine("{");
                sw.Indent++;

                sw.BeginMethod();
                FormatMethodBody(sw, formatterContext, executionContext);
                sw.EndMethod();

                sw.Indent--;
                sw.AppendLine("}");
            }
        }

        public void FormatMethodBody(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices executionContext)
        {
            Segment.Format(sw, formatterContext, executionContext);
        }

        private String GetMethodName(IScriptFormatterContext formatterContext)
        {
            String methodName = formatterContext.GetScriptName((DBScriptName.Id) Id);
            if (Char.IsLower(methodName[0]))
                methodName = Char.ToUpperInvariant(methodName[0]) + methodName.Substring(1);
            return methodName;
        }
    }
}
