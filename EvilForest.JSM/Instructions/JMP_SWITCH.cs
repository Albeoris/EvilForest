﻿using System;
using System.Collections.Generic;
using System.Linq;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    internal sealed class JMP_SWITCH : JsmInstruction, IFormattableScript
    {
        public IJsmExpression Condition { get; }
        public Case[] ValueCases { get; }
        public Case DefaultCase { get; }

        public JMP_SWITCH(IJsmExpression condition, Case[] valueCases, Case defaultCase)
        {
            Condition = condition;
            ValueCases = valueCases;
            DefaultCase = defaultCase;
        }

        public static JsmInstruction Create(JsmInstructionReader reader, Boolean isEx)
        {
            IJsmExpression condition = reader.Pop();
            Int32 offset = reader.CurrentOffset;
            Int32 startValue = 0;
            Byte caseNumber = reader.ReadByte();
            if (!isEx)
            {
                Byte valueL = reader.ReadByte();
                Byte valueH = reader.ReadByte();
                startValue += valueL | (valueH << 8);
            }
            else
            {
                offset += 3;
            }

            Case defaultCase = ReadCase(reader, offset, false);
            List<Case> cases = new List<Case>(caseNumber);
            for (Int32 i = 0; i < caseNumber; i++)
            {
                Case item = ReadCase(reader, offset, isEx);
                if (!isEx)
                    item.Value = startValue + i;
                cases.Add(item);
            }

            return new JMP_SWITCH(condition, cases.OrderBy(c=>c.Offset).ToArray(), defaultCase);
        }

        private static Case ReadCase(JsmInstructionReader reader, Int32 offset, Boolean isEx)
        {
            var item = new Case();

            if (isEx)
            {
                Byte valueL = reader.ReadByte();
                Byte valueH = reader.ReadByte();
                item.Value = valueL | (valueH << 8);
            }

            var jumpL = reader.ReadByte();
            var jumpH = reader.ReadByte();
            var jump = jumpL + (jumpH << 8);

            item.Offset = jump + offset;

            return item;
        }

        public sealed class Case : IJumpToOpcode
        {
            public Int32 Offset { get; set; }
            public Int32 Index { get; set; }
            public Int32 Value { get; set; }
        }

        private Int32 _index = -1;

        public Int32 Index
        {
            get
            {
                if (_index == -1)
                    throw new ArgumentException($"{nameof(JMP_SWITCH)} instruction isn't indexed yet.", nameof(Index));
                return _index;
            }
            set
            {
                // if (_index != -1)
                //    throw new ArgumentException($"{nameof(JMP_SWITCH)} instruction has already been indexed: {_index}.", nameof(Index));
                _index = value;
            }
        }

        public override String ToString()
        {
            // TODO
            return nameof(JMP_SWITCH);
        }

        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            IJsmExpression expression = Condition;
            expression.Format(sw, formatterContext, services);
        }
    }
}