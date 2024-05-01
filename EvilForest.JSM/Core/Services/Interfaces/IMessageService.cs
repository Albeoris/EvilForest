using System;

namespace FF8.Core
{
    public interface IMessageService
    {
        void SetDialogProgression(Int32 progression);
        void SetTextVariable(Int32 index, Int32 value);
        void RaiseAllWindows();
        void Show(Int32 messageId, Int32 windowsIndex, Int32 flags);

        IAwaitable ShowAndWait(Int32 messageId, Int32 windowsIndex, Int32 flags);
        IAwaitable Wait(Int32 windowsIndex);
    }
}