using System;

namespace FF8.Core
{
    public interface IInputService
    {
        void SetControlDirection(Byte arrowsAngle, Byte analogAngle);
        void SetMovementControl(Boolean isEnabled);
        void SetMenuControl(Boolean isEnabled);
        void SetAlwaysWalk(Boolean disableRun);
    }
}