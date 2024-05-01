using System;
using System.Text;
using EvilForest.Resources;
using FF8.Core;
using FF8.JSM.Format;

namespace FF8.JSM.Instructions
{
    /// <summary>
    /// RunSoundCode
    /// Same as RunSoundCode3( code, music, 0, 0, 0 ).
    /// AT_SOUNDCODE Code (2 bytes)
    /// AT_SOUND Sound (2 bytes)
    /// FLDSND0 = 0x0C5,
    /// </summary>
    internal sealed class FLDSND0 : JsmInstruction
    {
        private readonly IConstExpression _code;
        private readonly IConstExpression _sound;

        private FLDSND0(IConstExpression code, IConstExpression sound)
        {
            _code = code;
            _sound = sound;
        }

        public static JsmInstruction Create(JsmInstructionReader reader)
        {
            IConstExpression code = (IConstExpression) reader.ArgumentInt16();
            IConstExpression sound = (IConstExpression) reader.ArgumentInt16();
            return new FLDSND0(code, sound);
        }

        public override void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            Format(sw, formatterContext, services, _code, _sound.Int32(), null, null, null);
        }

        internal static void Format(ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services, IConstExpression argExpr, Int32 soundId, IJsmExpression a1, IJsmExpression a2, IJsmExpression a3)
        {
            Int32 arg = argExpr.Int32() & 65535;
            ParameterChanger(arg, ref soundId, a1, a2, a3, sw, formatterContext, services);

            OpCode code = (OpCode) (arg & 65471);
            Int32 slotIndex = (arg >> 6) & 1;

            FormatHelper.MethodFormatter formatter;

            switch (code)
            {
                case OpCode.SongPlay:
                {
                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongPlay))
                        .Argument(null, music.FileName);
                    break;
                }
                case OpCode.SongLoad:
                {
                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongLoad))
                        .Argument(null, music.FileName);
                    break;
                }
                case OpCode.SongSuspend:
                {
                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongSuspend))
                        .Argument(null, music.FileName);
                    break;
                }
                case OpCode.SongResume:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongResume));
                    break;
                }
                case OpCode.SongJumpPoint:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongJumpPoint));
                    break;
                }
                case OpCode.SongSkipPhrase:
                {
                    IJsmExpression phrase = a1 ?? throw new ArgumentNullException(nameof(a1));
                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongSkipPhrase))
                        .Argument(null, music.FileName)
                        .Argument(nameof(phrase), phrase);
                    break;
                }
                case OpCode.SongStop:
                {
                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongStop))
                        .Argument(null, music.FileName);
                    break;
                }
                case OpCode.SongStopCurrent:
                {
                    if (soundId != -1) throw new ArgumentOutOfRangeException($"soundId({soundId}) != -1", nameof(soundId));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongStopCurrent));
                    break;
                }
                case OpCode.SongVol:
                {
                    IJsmExpression volume = a1 ?? throw new ArgumentNullException(nameof(a1));
                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongVolumeChange))
                        .Argument(null, music.FileName)
                        .Argument(nameof(volume), volume);
                    break;
                }
                case OpCode.SongVolIntpl:
                {
                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression volume = a2 ?? throw new ArgumentNullException(nameof(a2));

                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongVolumeChange))
                        .Argument(null, music.FileName)
                        .Argument(nameof(volume), volume)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SongVolIntplAll:
                {
                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression volume = a2 ?? throw new ArgumentNullException(nameof(a2));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongVolumeChangeAll))
                        .Argument(nameof(volume), volume)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SongVolFade:
                {
                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression volumeFrom = a2 ?? throw new ArgumentNullException(nameof(a2));
                    IJsmExpression volumeTo = a3 ?? throw new ArgumentNullException(nameof(a3));

                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongVolumeFade))
                        .Argument(null, music.FileName)
                        .Argument(nameof(volumeFrom), volumeFrom)
                        .Argument(nameof(volumeTo), volumeTo)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SongVolFadeAll:
                {
                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression volumeFrom = a2 ?? throw new ArgumentNullException(nameof(a2));
                    IJsmExpression volumeTo = a3 ?? throw new ArgumentNullException(nameof(a3));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongVolumeFadeAll))
                        .Argument(nameof(volumeFrom), volumeFrom)
                        .Argument(nameof(volumeTo), volumeTo)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SongPitch:
                {
                    IJsmExpression pitch = a1 ?? throw new ArgumentNullException(nameof(a1));

                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongPitchChange))
                        .Argument(null, music.FileName)
                        .Argument(nameof(pitch), pitch);
                    break;
                }
                case OpCode.SongPitchIntpl:
                {
                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression pitch = a2 ?? throw new ArgumentNullException(nameof(a2));

                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongPitchChange))
                        .Argument(null, music.FileName)
                        .Argument(nameof(pitch), pitch)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SongPitchFade:
                {
                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression pitchFrom = a2 ?? throw new ArgumentNullException(nameof(a2));
                    IJsmExpression pitchTo = a3 ?? throw new ArgumentNullException(nameof(a3));

                    DBMusic music = formatterContext.GetMusic((DBMusic.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(music.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SongPitchFade))
                        .Argument(null, music.FileName)
                        .Argument(nameof(pitchFrom), pitchFrom)
                        .Argument(nameof(pitchTo), pitchTo)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.StreamPlay:
                {
                    IJsmExpression position = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression reverberation = a2 ?? throw new ArgumentNullException(nameof(a2));

                    DBSong song = formatterContext.GetSong((DBSong.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(song.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.StreamPlay))
                        .Argument(null, song.FileName)
                        .Argument(nameof(position), position)
                        .Argument(nameof(reverberation), reverberation);
                    break;
                }
                case OpCode.StreamStop:
                {
                    if (soundId != -1) throw new ArgumentOutOfRangeException($"soundId({soundId}) != -1", nameof(soundId));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.StreamStop));
                    break;
                }
                case OpCode.StreamVol:
                {
                    if (soundId != -1) throw new ArgumentOutOfRangeException($"soundId({soundId}) != -1", nameof(soundId));

                    IJsmExpression volume = a1 ?? throw new ArgumentNullException(nameof(a1));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.StreamVolumeChange))
                        .Argument(nameof(volume), volume);
                    break;
                }
                case OpCode.StreamFmvVol:
                {
                    if (soundId != -1) throw new ArgumentOutOfRangeException($"soundId({soundId}) != -1", nameof(soundId));

                    IJsmExpression volume = a1 ?? throw new ArgumentNullException(nameof(a1));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.StreamFmvVolumeChange))
                        .Argument(nameof(volume), volume);
                    break;
                }
                case OpCode.SfxPlay:
                {
                    IJsmExpression volume = a3 ?? throw new ArgumentNullException(nameof(a3));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxPlay))
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(volume), volume);
                    break;
                }
                case OpCode.SfxStop:
                {
                    IJsmExpression timeMsec = a1 ?? throw new ArgumentNullException(nameof(a1));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxStop))
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(timeMsec), timeMsec);
                    break;
                }
                case OpCode.SfxVol:
                {
                    IJsmExpression volume = a2 ?? throw new ArgumentNullException(nameof(a2));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxVolumeChange))
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(volume), volume);
                    break;
                }
                case OpCode.SfxVolIntpl:
                {
                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression volume = a2 ?? throw new ArgumentNullException(nameof(a2));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxVolumeChange))
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(volume), volume)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SfxVolAll:
                {
                    if (soundId != -1) throw new ArgumentOutOfRangeException($"soundId({soundId}) != -1", nameof(soundId));

                    IJsmExpression volume = a1 ?? throw new ArgumentNullException(nameof(a1));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxVolumeChangeAll))
                        .Argument(nameof(volume), volume);
                    break;
                }
                case OpCode.SfxVolIntplAll:
                {
                    if (soundId != -1) throw new ArgumentOutOfRangeException($"soundId({soundId}) != -1", nameof(soundId));

                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression volume = a2 ?? throw new ArgumentNullException(nameof(a2));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxVolumeChangeAll))
                        .Argument(nameof(volume), volume)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SfxPitch:
                {
                    IJsmExpression pitch = a2 ?? throw new ArgumentNullException(nameof(a2));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxPitchChange))
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(pitch), pitch);
                    break;
                }
                case OpCode.SfxPosIntpl:
                {
                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression position = a2 ?? throw new ArgumentNullException(nameof(a2));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxPositionChange))
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(position), position)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SfxResidentPlay:
                {
                    IJsmExpression timeMsec = a1 ?? throw new ArgumentNullException(nameof(a1));
                    IJsmExpression position = a2 ?? throw new ArgumentNullException(nameof(a2));
                    IJsmExpression volume = a3 ?? throw new ArgumentNullException(nameof(a3));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentPlay))
                        .Argument(nameof(slotIndex), slotIndex)
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(volume), volume)
                        .Argument(nameof(position), position)
                        .Argument(nameof(timeMsec), timeMsec);
                    break;
                }
                case OpCode.SfxResidentSuspend:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentSuspend))
                        .Argument(nameof(slotIndex), slotIndex);
                    break;
                }
                case OpCode.SfxResidentRestore:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentResume))
                        .Argument(nameof(slotIndex), slotIndex);
                    break;
                }
                case OpCode.SfxResidentStop:
                {
                    IJsmExpression timeMsec = a1 ?? throw new ArgumentNullException(nameof(a1));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentStop))
                        .Argument(nameof(slotIndex), slotIndex)
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(timeMsec), timeMsec);
                    break;
                }
                case OpCode.SfxResidentStopCurrent:
                {
                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentStopCurrent));
                    break;
                }
                case OpCode.SfxResidentVol:
                {
                    IJsmExpression volume = a2 ?? throw new ArgumentNullException(nameof(a2));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentVolumeChange))
                        .Argument(nameof(slotIndex), slotIndex)
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(volume), volume);
                    break;
                }
                case OpCode.SfxResidentVolIntpl:
                {
                    IJsmExpression ticks = a2 ?? throw new ArgumentNullException(nameof(a2));
                    IJsmExpression volume = a3 ?? throw new ArgumentNullException(nameof(a3));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentVolumeChange))
                        .Argument(nameof(slotIndex), slotIndex)
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(volume), volume)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SfxResidentVolAll:
                {
                    if (soundId != -1) throw new ArgumentOutOfRangeException($"soundId({soundId}) != -1", nameof(soundId));

                    IJsmExpression volume = a1 ?? throw new ArgumentNullException(nameof(a1));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentVolumeChangeAll))
                        .Argument(nameof(volume), volume);
                    break;
                }
                case OpCode.SfxResidentVolIntplAll:
                {
                    if (soundId != -1) throw new ArgumentOutOfRangeException($"soundId({soundId}) != -1", nameof(soundId));

                    IJsmExpression ticks = a1 ?? throw new ArgumentNullException(nameof(a2));
                    IJsmExpression volume = a2 ?? throw new ArgumentNullException(nameof(a3));

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentVolumeChangeAll))
                        .Argument(nameof(volume), volume)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SfxResPitchIntpl:
                {
                    IJsmExpression ticks = a2 ?? throw new ArgumentNullException(nameof(a2));
                    IJsmExpression pitch = a3 ?? throw new ArgumentNullException(nameof(a3));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentPitchChange))
                        .Argument(nameof(slotIndex), slotIndex)
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(pitch), pitch)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SfxResPosIntpl:
                {
                    IJsmExpression ticks = a2 ?? throw new ArgumentNullException(nameof(a2));
                    IJsmExpression position = a3 ?? throw new ArgumentNullException(nameof(a3));

                    DBSfx sfx = formatterContext.GetSfx((DBSfx.Id) soundId);
                    sw.AppendLine();
                    sw.Append("// ");
                    sw.AppendLine(sfx.DisplayName);

                    formatter = sw.Format(formatterContext, services)
                        .Service(nameof(ServiceId.Audio))
                        .Method(nameof(IAudioService.SfxResidentPositionChange))
                        .Argument(nameof(slotIndex), slotIndex)
                        .Argument(null, sfx.FileName)
                        .Argument(nameof(position), position)
                        .Argument(nameof(ticks), ticks);
                    break;
                }
                case OpCode.SfxNull:
                {
                    // Just ignore it.
                    return;
                }
                default:
                {
                    throw new NotSupportedException(code.ToString());
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("FLDSND(op: ").Append(arg);
            sb.Append(", sound: ").Append(soundId);
            if (a1 != null) sb.Append(", a1: ").Append(a1);
            if (a2 != null) sb.Append(", a2: ").Append(a2);
            if (a3 != null) sb.Append(", a3: ").Append(a3);
            sb.Append(")");

            formatter.Comment(sb.ToString());
        }

        private static void ParameterChanger(Int32 arg, ref Int32 soundId, IJsmExpression arg1, IJsmExpression arg2, IJsmExpression arg3, ScriptWriter sw, IScriptFormatterContext formatterContext, IServices services)
        {
            Int32 value = (arg >> 12) & 3;

            if (value == 0)
            {
                soundId = soundId switch
                {
                    93 => 5,
                    137 => 10,
                    78 => 22,
                    _ => soundId
                };
            }

            if (value == 1)
            {
                switch (soundId)
                {
                    case 3067:
                    case 3068:
                        soundId = 1748;
                        break;
                    case 293:
                        soundId = 633;
                        break;
                    case 1545:
                        soundId = 58;
                        break;
                    case 679:
                        soundId = 9010197;
                        break;
                    case 1230:
                    case 1231:
                        var expr = new Jsm.Expression.ValueExpression(arg, Jsm.Expression.VariableType.Int24);
                        Format(sw, formatterContext, services, expr, 9000023, arg1, arg2, arg3);
                        Format(sw, formatterContext, services, expr, 9000024, arg1, arg2, arg3);
                        Format(sw, formatterContext, services, expr, 9000025, arg1, arg2, arg3);
                        break;
                    case 681:
                        soundId = 3092;
                        break;
                    case 672:
                        soundId = 9010138;
                        break;
                    case 678:
                        soundId = 725;
                        break;
                    case 3084:
                        soundId = 370;
                        break;
                }
            }
        }

        public override String ToString()
        {
            return $"{nameof(FLDSND0)}({nameof(_code)}: {_code}, {nameof(_sound)}: {_sound})";
        }

        private enum OpCode
        {
            SongPlay = 0,
            SongLoad = 1792,
            SongSuspend = 2048,
            SongResume = 2304,
            SongJumpPoint = 2566,
            SongSkipPhrase = 16903,

            SongStop = 256,
            SongStopCurrent = 265,

            SongVol = 16897,
            SongVolFade = 50177,
            SongVolFadeAll = 51969,
            SongVolIntpl = 33537,
            SongVolIntplAll = 34305,

            SongPitch = 16899,
            SongPitchIntpl = 33539,
            SongPitchFade = 50179,

            StreamPlay = 40960,
            StreamStop = 8448,
            StreamVol = 25089,

            SfxPlay = 53248,
            SfxStop = 20736,

            SfxVol = 37377,
            SfxVolIntpl = 54017,
            SfxVolAll = 21761,
            SfxVolIntplAll = 38401,

            SfxPitch = 37379,

            SfxResidentStopCurrent = 4489,
            SfxResidentSuspend = 6272,
            SfxResidentRestore = 6528,
            SfxResidentStop = 20864,
            SfxResidentVolAll = 21889,
            SfxResidentVol = 37505,
            SfxResidentVolIntplAll = 38529,
            SfxResidentPlay = 53376,
            SfxResidentVolIntpl = 54145,

            // ================
            // Not implemented
            // ================

            Sync = 3072,
            InstrLoad = 30464,

            SongNull = 520,

            SongTempo = 16898,
            SongTempoPitch = 16900,
            SongTempoPitchFade = 1028,
            SongTempoPitchIntpl = 33540,
            SongTempoFade = 50178,
            SongTempoIntpl = 33538,

            StreamNull = 8712,
            StreamPos = 25093,
            StreamFmvVol = 25098,
            StreamReverb = 25100,

            SfxNull = 4616,

            SfxPitchAll = 21763,
            SfxPitchIntpl = 54019,
            SfxPitchIntplAll = 38403,

            SfxPos = 37381,
            SfxPosAll = 21765,
            SfxPosIntpl = 54021,
            SfxPosIntplAll = 38405,

            SfxResPitch = 37507,
            SfxResPitchAll = 21891,
            SfxResPitchIntpl = 54147,
            SfxResPitchIntplAll = 38531,

            SfxResPos = 37509,
            SfxResPosAll = 21893,
            SfxResPosIntpl = 54149,
            SfxResPosIntplAll = 38533,
        }
    }
}