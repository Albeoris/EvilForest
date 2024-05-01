﻿using System;
using System.Linq;
using FF8.Core;
using FF8.JSM.Format;
using FF8.JSM.Instructions;

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

        public String GetObjectName(IScriptFormatterContext context)
        {
            foreach (EVScript script in Scripts)
            {
                if (script.Id == 0)
                {
                    foreach (INameProvider instruction in script.Segment.EnumerateNameProviders())
                    {
                        if (instruction.TryGetAsciiName(context, out String displayName))
                            return $"{displayName}_{Id:D2}";
                    }
                }

                if (Id != 0 && script.Id == 1) // OnLoop
                {
                    foreach (IJsmInstruction instruction in script.Segment.EnumerateAllInstruction())
                    {
                        if (instruction is BGLCOLOR)
                            return $"Background_{Id:D2}";
                    }
                }
            }

            return $"ObjectId_{Id:D2}";
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