using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EvilForest.Resources.Enums;
using FF8.Core;

namespace FF8.JSM.Format
{
    public sealed class ScriptWriter
    {
        public Int32 Indent { get; set; }

        private StringBuilder _sb;
        private StringBuilder _sb2;

        private Boolean _newLine;
        public Boolean HasWhiteLine { get; private set; }
        public Boolean EndWithEmptyLine { get; set; }
        public EventWriter GlobalWriter { get; }
        public EventWriter EventWriter { get; }
        public ObjectWriter ObjectWriter { get; }

        public ScriptWriter(Int32 capacity = 8096)
        {
            _sb = new StringBuilder(capacity);
            GlobalWriter = new EventWriter(this);
            EventWriter = new EventWriter(this);
            ObjectWriter = new ObjectWriter(this);
        }

        public void AppendLine()
        {
            if (_newLine)
                HasWhiteLine = true;

            _sb.AppendLine();
            if (EndWithEmptyLine)
            {
                EndWithEmptyLine = false;
                _sb.AppendLine();
            }
            _newLine = true;
        }

        public void AppendLine(String text)
        {
            AppendIndent();

            _sb.AppendLine(text);
            if (EndWithEmptyLine)
            {
                EndWithEmptyLine = false;
                _sb.AppendLine();
            }
            _newLine = true;
            HasWhiteLine = (text == "{");
        }

        public void Append(String text)
        {
            AppendIndent();

            _sb.Append(text);
        }

        private void AppendIndent()
        {
            if (_newLine)
            {
                for (Int32 i = 0; i < Indent; i++)
                    _sb.Append("    ");

                _newLine = false;
                HasWhiteLine = false;
            }
        }

        public String Release()
        {
            String result = _sb.ToString();

            _sb.Clear();
            Indent = 0;
            _newLine = true;
            HasWhiteLine = false;

            return result;
        }

        public override String ToString()
        {
            return _sb.ToString();
        }

        public State RememberState()
        {
            return new State(this);
        }

        public sealed class State
        {
            private readonly ScriptWriter _sw;

            private Int32 _indent;
            private Boolean _newLine;
            private Boolean _hasEmptyLine;
            private Int32 _length;

            public State(ScriptWriter sw)
            {
                _sw = sw;

                _indent = _sw.Indent;
                _newLine = _sw._newLine;
                _length = _sw._sb.Length;
                _hasEmptyLine = _sw.HasWhiteLine;
            }

            public Boolean IsChanged => _length != _sw._sb.Length;

            public void Cancel()
            {
                _sw.Indent = _indent;
                _sw._newLine = _newLine;
                _sw.HasWhiteLine = _hasEmptyLine;
                _sw._sb.Length = _length;
            }
        }
        
        private readonly Dictionary<String, String> _localVariables = new Dictionary<String, String>();
        private Int32 _localVariablePosition = -1;

        public void BeginMethod()
        {
            _localVariables.Clear();
            _localVariablePosition = _sb.Length;
        }

        public void EndMethod()
        {
            if (_localVariables.Count < 1)
                return;

            var pairs = _localVariables.OrderBy(p => p.Key).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var pair in pairs)
            {
                var variableName = pair.Key;
                var variableDeclaration = pair.Value;

                for (Int32 i = 0; i < Indent; i++)
                    sb.Append("    ");
                sb.Append("var ");
                sb.Append(variableName);
                sb.Append(" = ");
                sb.Append(variableDeclaration);
                sb.AppendLine(";");
            }

            sb.AppendLine();

            _sb.Insert(_localVariablePosition, sb.ToString());

            _localVariables.Clear();
            _localVariablePosition = -1;
        }

        public String GetLocal(String fullName, String declaration)
        {
            String shortName = GetShortLocalVariableName(fullName);
            if (_localVariables.TryGetValue(shortName, out var dec))
            {
                if (dec != declaration)
                    throw new ArgumentException("if (dec != declaration)");
            }
            else
            {
                _localVariables.Add(shortName, declaration);
            }

            return shortName;
        }

        private static String GetShortLocalVariableName(String variableName)
        {
            for (Int32 limLength = 2; limLength < variableName.Length; limLength++)
            {
                switch (variableName[limLength])
                {
                    case 'a':
                    case 'e':
                    case 'i':
                    case 'o':
                    case 'u':
                    case 'A':
                    case 'E':
                    case 'I':
                    case 'O':
                    case 'U':
                        continue;
                }

                return "@" + Char.ToLowerInvariant(variableName[0]) + variableName.Substring(1, limLength);
            }

            return "@" + Char.ToLowerInvariant(variableName[0]) + variableName.Substring(1);
        }
        
        public void ObjectVariable(Int26 value)
        {
            String type = value.GetTypeName();
            Int32 offset = value.GetOffsetBits();
            Int32 size = value.GetTypeSizeBits();
            String longName = ObjectWriter.Allocate(type, offset, size);
            
            Append(longName);
        }

        public void EventVariable(Int26 value)
        {
            String type = value.GetTypeName();
            Int32 offset = value.GetOffsetBits();
            Int32 size = value.GetTypeSizeBits();
            String longName = EventWriter.Allocate(type, offset, size);
            
            Append("@evt.");
            Append(longName);
        }
        
        public void GlobalVariable(Int26 value)
        {
            String type = value.GetTypeName();
            Int32 offset = value.GetOffsetBits();
            Int32 size = value.GetTypeSizeBits();
            String longName = GlobalWriter.Allocate(type, offset, size);

            const String serviceName = nameof(ServiceId.Variables);
            String name = GetLocal(serviceName, $"{nameof(ServiceId)}.{serviceName}[@ctx]");
            
            Append($"{name}.");
            Append(longName);
        }
        
        public void SystemVariable(Int26 value)
        {
            SystemData systemData = (SystemData) value.Value;
            if (!Enum.IsDefined(typeof(SystemData), systemData))
                throw new NotSupportedException(systemData.ToString());
            
            const String serviceName = nameof(ServiceId.System);
            String name = GetLocal(serviceName, $"{nameof(ServiceId)}.{serviceName}[@ctx]");
            
            Append($"{name}.");
            Append(systemData.ToString());
        }

        // Dirty solution
        private readonly Dictionary<Guid, Region> _regions = new Dictionary<Guid, Region>();

        public Guid RememberRegion()
        {
            var region = new Region(_sb.Length, this);
            _regions.Add(region.Id, region);
            return region.Id;
        }
        
        public IDisposable GoToRegion(Guid regionId)
        {
            if (_sb2 != null) throw new NotSupportedException("if (_sb != null)");

            Region region = _regions[regionId];
            _sb2 = _sb;
            _sb = new StringBuilder();
            return region;
        }

        private void FlushRegion(Region region)
        {
            if (_sb2 == null) throw new NotSupportedException("if (_sb != null)");

            var sb = _sb;
            _sb = _sb2;
            _sb2 = null;

            var value = sb.ToString();
            if (value.Length > 0)
                _sb.Insert(region.Position, value);
            
            _regions.Remove(region.Id);
        }
        
        private sealed class Region : IDisposable
        {
            public Guid Id { get; } = Guid.NewGuid();
            public Int32 Position { get; }
            
            private readonly ScriptWriter _sw;

            public Region(Int32 sbLength, ScriptWriter sw)
            {
                Position = sbLength;
                _sw = sw;
            }

            public void Dispose()
            {
                _sw.FlushRegion(this);
            }
        }
    }
}