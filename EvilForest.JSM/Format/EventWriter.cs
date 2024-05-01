using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EvilForest.Resources;
using Memoria.EventEngine.EV;

namespace FF8.JSM.Format
{
    public sealed class EventWriter
    {
        private readonly ScriptWriter _sw;
        private readonly Dictionary<Int32, VariableList> _eventVariables = new Dictionary<Int32, VariableList>();

        private String _name;

        public EventWriter(ScriptWriter sw)
        {
            _sw = sw;
        }

        public void BeginEvent(String name)
        {
            _name = name;
            _eventVariables.Clear();
        }

        public String Allocate(String type, Int32 offsetBits, Int32 sizeBits)
        {
            return VariableList.Allocate(type, offsetBits, sizeBits, _eventVariables);
        }

        public void EndEvent()
        {
            _sw.AppendLine($"public sealed class {_name}");
            {
                _sw.AppendLine("{");
                _sw.Indent++;

                foreach (VariableList variable in _eventVariables.Values.OrderBy(v => v.Offset))
                {
                    String type = variable.Type;
                    if (variable.IsFlag)
                    {
                        type = type switch
                        {
                            "Byte" => "BitByte",
                            "SByte" => "SBitByte",
                            "Int32" => "BitInt32",
                            _ => throw new NotSupportedException(type)
                        };
                    }

                    _sw.AppendLine($"public {type} {variable.Name} {{ get; set; }}");
                }

                _sw.Indent--;
                _sw.AppendLine("}");
            }

            _eventVariables.Clear();
        }

        public void RememberObject(EVObject evObject, String typeName, String fileName, String result)
        {
            throw new NotImplementedException();
        }
    }
}