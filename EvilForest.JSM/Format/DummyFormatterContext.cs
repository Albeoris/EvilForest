﻿using System;
 using EvilForest.Resources;
 using Memoria.EventEngine.EV;

 namespace FF8.JSM.Format
{
    public sealed class DummyFormatterContext : IScriptFormatterContext
    {
        public DBEvent Event { get; } = new DBEvent {EntryId = (DBEvent.Id) (-1), FileName = "Dummy"};

        public DummyFormatterContext()
        {
        }

        public static IScriptFormatterContext Instance { get; } = new DummyFormatterContext();
        
        public String GetScriptName(DBScriptName.Id id)
        {
            return $"Script_{(Int32)id:D2}";
        }

        public String GetMessage(DBFieldMessage.Id messageIndex)
        {
            return $"Message_{(Int32)messageIndex:D3}";
        }

        public DBModel GetModel(DBModel.Id modelId)
        {
            String name = $"Model_{(Int32) modelId:D3}";
            return new DBModel {EntryId = modelId, Category = -1, DisplayName = name, FileName = name, Module = -1, Version = -1};
        }

        public DBAnimation GetAnimation(DBAnimation.Id animationId)
        {
            String name = $"Animation_{(Int32) animationId:D3}";
            return new DBAnimation {EntryId = animationId, FileName = name};
        }

        public DBMusic GetMusic(DBMusic.Id musicId)
        {
            return new DBMusic {DisplayName = "GetMusic_todo", EntryId = musicId, FileName = "GetMusic_todo"};
        }
        
        public DBSong GetSong(DBSong.Id songId)
        {
            return new DBSong {DisplayName = "GetSong_todo", EntryId = songId, FileName = "GetSong_todo"};
        }
        
        public DBSfx GetSfx(DBSfx.Id sfxId)
        {
            return new DBSfx {DisplayName = "GetSfx_todo", EntryId = sfxId, FileName = "GetSfx_todo"};
        }
    }
}