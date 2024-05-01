using System;
using EvilForest.Resources;

namespace FF8.Core
{
    public interface IAudioService
    {
        void SongPlay(String musicFileName);
        void SongLoad(String musicFileName);
        void SongSuspend(String musicFileName);
        void SongResume();
        void SongJumpPoint();
        void SongSkipPhrase(String musicFileName, Int32 phrase);
        void SongStop(String musicFileName);
        void SongStopCurrent();
        void SongVolumeChange(String musicFileName, Int32 volume,  Int32 ticks = 0);
        void SongVolumeChangeAll(Int32 volume, Int32 ticks = 0);
        void SongVolumeFade(String musicFileName, Int32 volumeFrom, Int32 volumeTo, Int32 ticks);
        void SongVolumeFadeAll(Int32 volumeFrom, Int32 volumeTo, Int32 ticks);
        void SongPitchChange(String musicFileName, Int32 pitch, Int32 ticks = 0);
        void SongPitchFade(String musicFileName, Int32 pitchFrom, Int32 pitchTo, Int32 ticks);

        void StreamPlay(String songFileName, Int32 position, Int32 reverberation);
        void StreamStop();
        void StreamVolumeChange(Int32 volume, Int32 ticks = 0);
        void StreamFmvVolumeChange(Int32 volume, Int32 ticks = 0);

        void SfxPlay(String sfxFileName, Int32 volume);
        void SfxStop(String sfxFileName, Int32 timeMsec);
        void SfxVolumeChange(String sfxFileName, Int32 volume, Int32 ticks = 0);
        void SfxVolumeChangeAll(Int32 volume, Int32 ticks = 0);
        void SfxPitchChange(String sfxFileName, Int32 pitch, Int32 ticks = 0);
        void SfxPositionChange(String sfxFileName, Int32 volume, Int32 ticks = 0);

        void SfxResidentPlay(Int32 slotIndex, String sfxFileName, Int32 volume, Int32 position, Int32 timeMsec = 0);
        void SfxResidentSuspend(Int32 slotIndex);
        void SfxResidentResume(Int32 slotIndex);
        void SfxResidentStop(Int32 slotIndex, String sfxFileName, Int32 timeMsec = 0);
        void SfxResidentStopCurrent();
        void SfxResidentVolumeChange(Int32 slotIndex, String sfxFileName, Int32 volume, Int32 timeMsec = 0);
        void SfxResidentVolumeChangeAll(Int32 volume, Int32 timeMsec = 0);
        void SfxResidentPitchChange(Int32 slotIndex, String sfxFileName, Int32 pitch, Int32 timeMsec = 0);
        void SfxResidentPositionChange(Int32 slotIndex, String sfxFileName, Int32 position, Int32 timeMsec = 0);
    }
}