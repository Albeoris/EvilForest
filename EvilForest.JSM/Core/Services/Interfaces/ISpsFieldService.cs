using System;

namespace FF8.Core
{
    public interface ISpsFieldService
    {
        void SetReference(Int32 index, Int32 referenceIndex);
        void SetAttribute(Int32 index, Int32 isSet, Int32 value);
        void SetPosition(Int32 index, Int32 x, Int32 y, Int32 z);
        void SetRotation(Int32 index, Int32 x, Int32 y, Int32 z);
        void SetScale(Int32 index, Int32 scale);
        void SetCharacter(Int32 index, Int32 characterIndex, Int32 boneIndex);
        void SetFade(Int32 index, Int32 fade);
        void SetAnimationRate(Int32 index, Int32 rate);
        void SetFrameRate(Int32 index, Int32 rate);
        void SetCurrentFrame(Int32 index, Int32 frame);
        void SetPositionOffset(Int32 index, Int32 offset);
        void SetDepthOffset(Int32 index, Int32 offset);
    }
}