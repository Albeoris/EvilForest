using System;

namespace FF8.Core
{
    public interface IGuiService
    {
        void SetPlayerHereIcon(Boolean isShown);
        void Fade(FadeFlags fadeFlags, Int32 durationInFrames, Int32 colorR, Int32 colorG, Int32 colorB);
    }

    public enum FadeFlags
    {
        None = 0,
        Unknown = 1,
        Sub = 2
    }
}