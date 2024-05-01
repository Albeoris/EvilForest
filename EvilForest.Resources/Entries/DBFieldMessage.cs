using System;
using Newtonsoft.Json;

namespace EvilForest.Resources
{
    public sealed class DBFieldMessage : IDBEntry<DBFieldMessage.Id>
    {
        [JsonProperty("id")] public Id EntryId { get; set; }
        [JsonProperty("text")] public String Text { get; set; } = null!;
        
        public enum Id
        {
        }
        
        public enum FileId
        {
        }
    }
}