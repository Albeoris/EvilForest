using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// InitCode
    /// Init a normal code (independant functions).
    /// 
    /// 1st argument: code entry to init.
    /// 2nd argument: Unique ID (defaulted to entry's ID if 0).
    /// AT_ENTRY Entry (1 bytes)
    /// AT_USPIN UID (1 bytes)
    /// NEW = 0x007,
    /// </summary>
    internal sealed class NEW : JsmInstruction
    {
        private readonly Byte _objectIndex;

        private readonly Byte _uid;

        private NEW(Byte objectIndex, Byte uid)
        {
            _objectIndex = objectIndex;
            _uid = uid;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            var entry = reader.Arguments();
            var uid = reader.ReadByte();
            return new NEW(entry, uid);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .Service(nameof(ServiceId.System))
                .Method(nameof(ISystemService.InitService))
                .Argument("objectIndex", _objectIndex)
                .Argument("uid", _uid)
                .Comment(nameof(NEW));
        }
        
        public override String ToString()
        {
            return $"{nameof(NEW)}({nameof(_objectIndex)}: {_objectIndex}, {nameof(_uid)}: {_uid})";
        }
    }
}