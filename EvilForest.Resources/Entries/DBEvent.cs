using System;
using Newtonsoft.Json;

namespace EvilForest.Resources
{
    public sealed class DBEvent : IDBEntry<DBEvent.Id>
    {
        [JsonProperty("id")] public Id EntryId { get; set; }
        [JsonProperty("fileName")] public String FileName { get; set; } = null!;
        [JsonProperty("field")] public FieldDescriptor? Field { get; set; }

        public sealed class FieldDescriptor
        {
            [JsonProperty("messageFileId")] public DBFieldMessage.FileId MessageFileId { get; set; }
            [JsonProperty("messageFileName")] public String MessageFileName { get; set; } = null!;
        }
        
        public enum Id
        {
        }
    }
}