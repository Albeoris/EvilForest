using System;
using System.Collections.Generic;
using System.Linq;

namespace FF8.JSM.Format
{
    internal sealed class VariableList
    {
        public String Name { get; }
        public String Type { get; }
        public Int32 Offset { get; }
        public Int32 Size { get; }
        public Boolean IsFlag { get; set; }

        private VariableList(String name, String type, Int32 offset, Int32 size, Boolean isFlag)
        {
            Name = name;
            Type = type;
            Offset = offset;
            Size = size;
            IsFlag = isFlag;
        }

        public static String Allocate(String type, Int32 offsetBits, Int32 size, Dictionary<Int32, VariableList> dic)
        {
            Int32 offset = offsetBits;
            Int32 bitNumber = offset % 8;
            Boolean isFlag = size == 1;

            if (isFlag)
                offset = offset / 8 * 8;
            else if (bitNumber != 0)
                throw new NotSupportedException("if (bitNumber != 0)");

            if (dic.TryGetValue(offset, out var variable))
            {
                if (isFlag)
                {
                    variable.IsFlag = true;
                    // if (variable.Size != 8) throw new NotSupportedException("if (variable.Size != 8)");
                    return $"{variable.Name}[{bitNumber}]";
                }

                if (variable.Type != type) throw new ArgumentException($"There is another event variable with {nameof(variable.Name)} [{variable.Name}] and different {nameof(type)} [{variable.Type}]. Expected: [{type}]", nameof(type));
                if (variable.Offset != offset) throw new ArgumentException($"There is another event variable with {nameof(variable.Name)} [{variable.Name}]and different {nameof(offset)} [{variable.Offset}]. Expected: [{offset}]", nameof(offset));
                if (variable.Size != size) throw new ArgumentException($"There is another event variable with {nameof(variable.Name)} [{variable.Name}] and different {nameof(size)} [{variable.Size}]. Expected: [{size}]", nameof(size));
                return variable.Name;
            }

            variable = dic.Values.FirstOrDefault(v => v.Offset <= offset && v.Offset + v.Size - 1 >= offset);
            if (variable != null)
            {
                if (isFlag)
                {
                    bitNumber = offsetBits - variable.Offset;
                    variable.IsFlag = true;
                    return $"{variable.Name}[{bitNumber}]";
                }
                
                throw new ArgumentException($"There is another event variable with {nameof(variable.Name)} [{variable.Name}]. Offset: {variable.Offset}", nameof(offset));
            }

            if (isFlag)
            {
                size = 8;
                type = "Byte";
            }

            String name = $"{type}_{offset / 8}";
            variable = new VariableList(name, type, offset, size, isFlag);
            dic.Add(offset, variable);

            return isFlag
                ? $"{variable.Name}[{bitNumber}]"
                : variable.Name;
        }
    }
}