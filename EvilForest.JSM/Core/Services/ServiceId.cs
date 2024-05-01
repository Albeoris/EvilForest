using System;
using EvilForest.Resources;
using EvilForest.Resources.Enums;

namespace FF8.Core
{
    public interface IServices
    {
        T Service<T>(ServiceId<T> id);
    }

    public abstract class ServiceId<T>
    {
        public Boolean IsSupported => false;
        public T this[IServices services] => services.Service(this);
    }

    public static class ServiceId
    {
        public static ServiceId<IVariablesService> Variables { get; } = new VariablesServiceId();
        public static ServiceId<IMessageService> Messages { get; } = new MessageServiceId();
        public static ServiceId<IActorService> Actors { get; } = new ActorServiceId();
        public static ServiceId<IAudioService> Audio { get; } = new AudioServiceId();
        public static ServiceId<IInputService> Input { get; } = new InputServiceId();
        public static ServiceId<IBgiFieldService> BgiField { get; } = new BgiFieldServiceId();
        public static ServiceId<ISpsFieldService> SpsField { get; } = new SpsFieldServiceId();
        public static ServiceId<ISystemService> System { get; } = new SystemServiceId();
        public static ServiceId<IGuiService> Gui { get; } = new GuiServiceId();

        private sealed class VariablesServiceId : ServiceId<IVariablesService>, IVariablesService
        {
            public T Get<T>(GlobalVariableId<T> id) => throw new NotSupportedException();
            public void Set<T>(GlobalVariableId<T> id, T value) => throw new NotSupportedException();
            public Int32 Get(MapVariableId id) => throw new NotSupportedException();
            public void Set(MapVariableId id, Int32 value) => throw new NotSupportedException();
            public Int32 Get(SystemVariableId id) => throw new NotSupportedException();
            public void Set(SystemVariableId id, Int32 value) => throw new NotSupportedException();
        }

        private sealed class MessageServiceId : ServiceId<IMessageService>, IMessageService
        {
            public void SetTextVariable(Int32 index, Int32 value) => throw new NotSupportedException();
            public void RaiseAllWindows() => throw new NotSupportedException();
            public void Show(Int32 messageId, Int32 windowsIndex, Int32 flags) => throw new NotSupportedException();
            public void SetDialogProgression(Int32 progression) => throw new NotSupportedException();
            public IAwaitable ShowAndWait(Int32 messageId, Int32 windowsIndex, Int32 flags) => throw new NotSupportedException();
            public IAwaitable Wait(Int32 windowsIndex) => throw new NotSupportedException();
        }

        private sealed class ActorServiceId : ServiceId<IActorService>, IActorService
        {
            public void SetModel(String modelName, Int32 height) => throw new NotSupportedException();
            public void SetPosition(Int32 x, Int32 y) => throw new NotSupportedException();
            public void SetAngle(Int32 angle) => throw new NotSupportedException();
            public void SetAnimation(AnimationKind kind, String animationId) => throw new NotSupportedException();
            public void SetRadius(Int32 walkRadius, Int32 collisionRadius, Int32 talkDistance) => throw new NotSupportedException();
            public void SetIdleSpeed(Int32 speed1, Int32 speed2, Int32 speed3, Int32 speed4) => throw new NotSupportedException();

            public void Turn(Int32 angle, Int32 speed) => throw new NotSupportedException();
            public IAwaitable WaitTurnComplete() => throw new NotSupportedException();

            public void Animate(Int32 animationId) => throw new NotSupportedException();
            IAwaitable IActorService.WaitAnimationComplete() => throw new NotSupportedException();
        }

        private sealed class AudioServiceId : ServiceId<IAudioService>, IAudioService
        {
            public void SongPlay(DBMusic.Id id) => throw new NotSupportedException();
            public void SongJumpPoint() => throw new NotSupportedException();
            public void SongSkipPhrase(String musicFileName, Int32 phrase) => throw new NotSupportedException();
            public void SongStop(String musicFileName) => throw new NotSupportedException();
            public void SongStop(DBMusic.Id id) => throw new NotSupportedException();
            public void SongStopCurrent() => throw new NotSupportedException();
            public void SongVolumeChange(String musicFileName, Int32 volume, Int32 ticks = 0) => throw new NotSupportedException();
            public void SongVolumeChangeAll(Int32 volume, Int32 ticks = 0) => throw new NotSupportedException();
            public void SongVolumeFade(String musicFileName, Int32 volumeFrom, Int32 volumeTo, Int32 ticks) => throw new NotSupportedException();
            public void SongVolumeFadeAll(Int32 volumeFrom, Int32 volumeTo, Int32 ticks) => throw new NotSupportedException();
            public void SongPitchChange(String musicFileName, Int32 pitch, Int32 ticks = 0) => throw new NotSupportedException();
            public void SongPitchFade(String musicFileName, Int32 pitchFrom, Int32 pitchTo, Int32 ticks) => throw new NotSupportedException();
            public void StreamPlay(String songFileName, Int32 position, Int32 reverberation) => throw new NotSupportedException();
            public void StreamStop() => throw new NotSupportedException();
            public void StreamVolumeChange(Int32 volume, Int32 ticks = 0) => throw new NotSupportedException();
            public void StreamFmvVolumeChange(Int32 volume, Int32 ticks = 0) => throw new NotSupportedException();
            public void SfxPlay(String sfxFileName, Int32 volume) => throw new NotSupportedException();
            public void SfxStop(String sfxFileName, Int32 timeMsec) => throw new NotSupportedException();
            public void SfxVolumeChange(String sfxFileName, Int32 volume, Int32 ticks = 0) => throw new NotSupportedException();
            public void SfxVolumeChangeAll(Int32 volume, Int32 ticks = 0) => throw new NotSupportedException();
            public void SfxPitchChange(String sfxFileName, Int32 pitch, Int32 ticks = 0) => throw new NotSupportedException();
            public void SfxPositionChange(String sfxFileName, Int32 volume, Int32 ticks = 0) => throw new NotSupportedException();
            public void SfxResidentPlay(Int32 slotIndex, String sfxFileName, Int32 volume, Int32 position, Int32 timeMsec = 0) => throw new NotSupportedException();
            public void SfxResidentSuspend(Int32 slotIndex) => throw new NotSupportedException();
            public void SfxResidentResume(Int32 slotIndex) => throw new NotSupportedException();
            public void SfxResidentStop(Int32 slotIndex, String sfxFileName, Int32 timeMsec = 0) => throw new NotSupportedException();
            public void SfxResidentStopCurrent() => throw new NotSupportedException();
            public void SfxResidentVolumeChange(Int32 slotIndex, String sfxFileName, Int32 volume, Int32 timeMsec = 0) => throw new NotSupportedException();
            public void SfxResidentVolumeChangeAll(Int32 volume, Int32 timeMsec = 0) => throw new NotSupportedException();
            public void SfxResidentPitchChange(Int32 slotIndex, String sfxFileName, Int32 pitch, Int32 timeMsec = 0) => throw new NotSupportedException();
            public void SfxResidentPositionChange(Int32 slotIndex, String sfxFileName, Int32 position, Int32 timeMsec = 0) => throw new NotSupportedException();
            public void SongSkipPhrase() => throw new NotSupportedException();
            public void SongLoad(DBMusic.Id id) => throw new NotSupportedException();
            public void SongSuspend(DBMusic.Id id) => throw new NotSupportedException();
            public void SongPlay(String musicFileName) => throw new NotSupportedException();
            public void SongLoad(String musicFileName) => throw new NotSupportedException();
            public void SongSuspend(String musicFileName) => throw new NotSupportedException();
            public void SongResume() => throw new NotSupportedException();
        }

        private sealed class InputServiceId : ServiceId<IInputService>, IInputService
        {
            public void SetControlDirection(Byte arrowsAngle, Byte analogAngle) => throw new NotSupportedException();
            public void SetMovementControl(Boolean isEnabled) => throw new NotSupportedException();
            public void SetMenuControl(Boolean isEnabled) => throw new NotSupportedException();
            public void SetAlwaysWalk(Boolean disableRun) => throw new NotSupportedException();
        }

        private sealed class BgiFieldServiceId : ServiceId<IBgiFieldService>, IBgiFieldService
        {
            public void SetWalkmeshTriangleMask(Int32 mask) => throw new NotSupportedException();
        }
        
        private sealed class SpsFieldServiceId : ServiceId<ISpsFieldService>, ISpsFieldService
        {
            public void SetReference(Int32 index, Int32 referenceIndex) => throw new NotSupportedException();
            public void SetAttribute(Int32 index, Int32 isSet, Int32 value) => throw new NotSupportedException();
            public void SetPosition(Int32 index, Int32 x, Int32 y, Int32 z) => throw new NotSupportedException();
            public void SetRotation(Int32 index, Int32 x, Int32 y, Int32 z) => throw new NotSupportedException();
            public void SetScale(Int32 index, Int32 scale) => throw new NotSupportedException();
            public void SetCharacter(Int32 index, Int32 characterIndex, Int32 boneIndex) => throw new NotSupportedException();
            public void SetFade(Int32 index, Int32 fade) => throw new NotSupportedException();
            public void SetAnimationRate(Int32 index, Int32 rate) => throw new NotSupportedException();
            public void SetFrameRate(Int32 index, Int32 rate) => throw new NotSupportedException();
            public void SetCurrentFrame(Int32 index, Int32 frame) => throw new NotSupportedException();
            public void SetPositionOffset(Int32 index, Int32 offset) => throw new NotSupportedException();
            public void SetDepthOffset(Int32 index, Int32 offset) => throw new NotSupportedException();
        }
        
        private sealed class SystemServiceId : ServiceId<ISystemService>, ISystemService
        {
            public void InitService(Int32 objectIndex, Int32 uid) => throw new NotSupportedException();
            public void InitRegion(Int32 objectIndex, Int32 uid) => throw new NotSupportedException();
            public void InitActor(Int32 objectIndex, Int32 uid) => throw new NotSupportedException();
            public void PullActorScreenPosition(Int32 objectIndex) => throw new NotSupportedException();
            public IAwaitable Wait(Int32 frameDuration) => throw new NotSupportedException();
            public Int32 this[SystemData kind] => throw new NotImplementedException();
        }
        
        private sealed class GuiServiceId : ServiceId<IGuiService>, IGuiService
        {
            public void SetPlayerHereIcon(Boolean isShown) => throw new NotSupportedException();
            public void Fade(FadeFlags fadeFlags, Int32 durationInFrames, Int32 colorR, Int32 colorG, Int32 colorB) => throw new NotSupportedException();
        }
    }
}