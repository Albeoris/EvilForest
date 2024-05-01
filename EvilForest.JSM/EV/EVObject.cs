﻿using System;
using System.Linq;
using FF8.Core;
using FF8.JSM.Format;

namespace Memoria.EventEngine.EV
{
    public sealed class EVObject
    {
        public Int32 Id { get; } // SID
        public Byte VariableCount { get; }
        public Byte Flags { get; }
        public EVScript[] Scripts { get; }

        public EVObject(Int32 index, Byte variableCount, Byte flags, EVScript[] scripts)
        {
            Id = index;
            VariableCount = variableCount;
            Flags = flags;
            Scripts = scripts;
        }

        public void FormatType(ScriptWriter sw, String typeName, IScriptFormatterContext formatterContext, IServices executionContext)
        {
            sw.AppendLine($"public sealed class {typeName}");
            {
                sw.AppendLine("{");
                sw.Indent++;

                sw.ObjectWriter.BeginObject();
                if (Scripts.Length > 0)
                {
                    FormatConstructor(typeName, sw, formatterContext, executionContext);

                    foreach (var script in Scripts.Skip(1))
                    {
                        sw.AppendLine();
                        script.FormatMethod(sw, formatterContext, executionContext);
                    }
                }
                sw.ObjectWriter.EndObject();

                sw.Indent--;
                sw.AppendLine("}");
            }
        }

        private void FormatConstructor(String typeName, ScriptWriter sw, IScriptFormatterContext formatterContext, IServices executionContext)
        {
            String evtName = formatterContext.Event.FileName;
            
            sw.AppendLine($"private {nameof(IServices)} @ctx;");
            sw.AppendLine($"private {evtName} @evt;");
            sw.AppendLine();

            sw.AppendLine($"public IEnumerable<IAwaitable> Init({nameof(IServices)} executionContext, {evtName} evt)");
            {
                sw.AppendLine("{");
                sw.Indent++;

                sw.AppendLine("@ctx = executionContext;");
                sw.AppendLine("@evt = evt;");
                sw.BeginMethod();
                Scripts[0].FormatMethodBody(sw, formatterContext, executionContext);
                sw.EndMethod();

                sw.Indent--;
                sw.AppendLine("}");
            }
        }
    }
}