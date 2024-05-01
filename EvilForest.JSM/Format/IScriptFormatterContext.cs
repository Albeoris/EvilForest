﻿using System;
 using EvilForest.Resources;
 using Memoria.EventEngine.EV;

 namespace FF8.JSM.Format
{
    public interface IScriptFormatterContext
    {
        DBEvent Event { get; }
        
        String GetObjectName(Int32 id, EVScript[] scripts);
        String GetScriptName(DBScriptName.Id id);
        String GetMessage(DBFieldMessage.Id messageIndex);
        DBModel GetModel(DBModel.Id modelId);
        DBAnimation GetAnimation(DBAnimation.Id animationId);
        DBMusic GetMusic(DBMusic.Id musicId);
        DBSong GetSong(DBSong.Id songId);
        DBSfx GetSfx(DBSfx.Id sfxId);
    }
}