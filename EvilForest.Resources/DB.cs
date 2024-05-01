using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Albeoris.Framework.Reflection;
using Albeoris.Framework.Strings;
using Albeoris.Framework.System;
using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace EvilForest.Resources
{
    public static class DB
    {
        private static readonly Assembly ResourceAssembly;

        public static IReadOnlyDictionary<DBEvent.Id, DBEvent> Events { get; }
        public static IReadOnlyDictionary<DBModel.Id, DBModel> Models { get; }
        public static IReadOnlyDictionary<DBAnimation.Id, DBAnimation> Animations { get; }
        public static IReadOnlyDictionary<DBMusic.Id, DBMusic> Music { get; }
        public static IReadOnlyDictionary<DBSong.Id, DBSong> Songs { get; }
        public static IReadOnlyDictionary<DBSfx.Id, DBSfx> SoundEffects { get; }
        public static IReadOnlyDictionary<DBScriptName.Id, DBScriptName> ScriptNames { get; }
        public static IReadOnlyDictionary<DBFieldMessage.FileId, IReadOnlyDictionary<DBFieldMessage.Id, DBFieldMessage>> FieldMessages { get; }

        static DB()
        {
            ResourceAssembly = Assembly.GetExecutingAssembly();

            Events = ReadDictionary<DBEvent.Id, DBEvent>("JSON/Events.json");
            Models = ReadDictionary<DBModel.Id, DBModel>("JSON/Models.json");
            Animations = ReadDictionary<DBAnimation.Id, DBAnimation>("JSON/Animation.json");
            Music = ReadDictionary<DBMusic.Id, DBMusic>("JSON/Music.json");
            Songs = ReadDictionary<DBSong.Id, DBSong>("JSON/Songs.json");
            SoundEffects = ReadDictionary<DBSfx.Id, DBSfx>("JSON/SoundEffects.json");
            ScriptNames = ReadDictionary<DBScriptName.Id, DBScriptName>("JSON/ScriptNames.json");

            FieldMessages = ReadFieldMessages();
        }

        private static IReadOnlyDictionary<DBFieldMessage.FileId, IReadOnlyDictionary<DBFieldMessage.Id, DBFieldMessage>> ReadFieldMessages()
        {
            var result = new Dictionary<DBFieldMessage.FileId, IReadOnlyDictionary<DBFieldMessage.Id, DBFieldMessage>>();

            Wildcard wildcard = new Wildcard("JSON/Text/Field/*.json", StringComparison.OrdinalIgnoreCase);
            foreach (EmbeddedResource resource in EmbeddedResource.Enumerate(ResourceAssembly, wildcard))
            {
                Int32 separator = resource.Info.FileName.LastIndexOf('.', resource.Info.FileName.Length - ".json".Length - 1);
                String messageFileName = resource.Info.FileName.Substring(separator + 1);
                DBFieldMessage.FileId messageFileId = (DBFieldMessage.FileId) Int32.Parse(messageFileName.Substring(0, 4));

                result.Add(messageFileId, ReadDictionary<DBFieldMessage.Id, DBFieldMessage>(resource));
            }

            return result;
        }

        private static IReadOnlyDictionary<TId, T> ReadDictionary<TId, T>(String resourceName) where T : IDBEntry<TId>
        {
            return ReadEntries<T>(resourceName).ToDictionary(entry => entry.EntryId);
        }
        
        private static IReadOnlyDictionary<TId, T> ReadDictionary<TId, T>(EmbeddedResource resource) where T : IDBEntry<TId>
        {
            return ReadEntries<T>(resource).ToDictionary(entry => entry.EntryId);
        }

        private static IReadOnlyList<T> ReadEntries<T>(String resourceName)
        {
            EmbeddedResource resource = new EmbeddedResource(ResourceAssembly, resourceName);
            return ReadEntries<T>(resource);
        }

        private static IReadOnlyList<T> ReadEntries<T>(EmbeddedResource resource)
        {
            using (var sr = resource.OpenText())
            {
                JsonSerializer serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};
                JsonTextReader json = new JsonTextReader(sr);
                return serializer.Deserialize<T[]>(json) ?? throw new InvalidDataException($"Failed to parse resource: {resource.Info.FileName}");
            }
        }
    }
}