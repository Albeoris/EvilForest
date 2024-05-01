using System;
using System.Collections.Generic;
using System.Linq;

namespace FF8.JSM.Format
{
    public sealed class ObjectWriter
    {
        private readonly ScriptWriter _sw;
        private readonly Dictionary<Int32, VariableList> _eventVariables = new Dictionary<Int32, VariableList>();

        public ObjectWriter(ScriptWriter sw)
        {
            _sw = sw;
        }

        private Guid _region;

        public void BeginObject()
        {
            _region = _sw.RememberRegion();
            _eventVariables.Clear();
        }

        public String Allocate(String type, Int32 offsetBits, Int32 sizeBits)
        {
            return VariableList.Allocate(type, offsetBits, sizeBits, _eventVariables);
        }

        public void EndObject()
        {
            using (_sw.GoToRegion(_region))
            {
                if (_eventVariables.Count > 0)
                {
                    foreach (VariableList variable in _eventVariables.Values.OrderBy(v => v.Offset))
                    {
                        String type = variable.Type;
                        if (variable.IsFlag)
                        {
                            type = type switch
                            {
                                "Byte" => "BitByte",
                                "SByte" => "SBitByte",
                                _ => type
                            };
                        }

                        _sw.AppendLine($"public {type} {variable.Name} {{ get; set; }}");
                    }

                    _sw.AppendLine();
                }

                _eventVariables.Clear();
            }
        }
    }
}