using System;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// WaitWindow
    /// Wait until the window is closed.
    /// 
    /// 1st argument: window ID determined at its creation.
    /// AT_USPIN Window ID (1 bytes)
    /// WAITMES = 0x054,
    /// </summary>
    internal sealed class WAITMES : JsmInstruction
    {
        private readonly IJsmExpression _windowId;

        private WAITMES(IJsmExpression windowId)
        {
            _windowId = windowId;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IJsmExpression windowId = reader.ArgumentByte();
            return new WAITMES(windowId);
        }
        
        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            sw.Format(formatterContext, services)
                .YieldReturn()
                .Service(nameof(ServiceId.Messages))
                .Method(nameof(IMessageService.Wait))
                .Argument(nameof(_windowId), _windowId)
                .Comment(nameof(WAITMES));
        }

        public override String ToString()
        {
            return $"{nameof(WAITMES)}({nameof(_windowId)}: {_windowId})";
        }
    }
}