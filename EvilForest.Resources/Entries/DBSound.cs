using System;
using Newtonsoft.Json;

namespace EvilForest.Resources
{
    public sealed class DBSound : IDBEntry<DBSound.Id>
    {
        [JsonProperty("id")] public Id EntryId { get; set; }
        [JsonProperty("fileName")] public String FileName { get; set; } = null!;
        [JsonProperty("displayName")] public String DisplayName { get; set; } = null!;
        
        public enum Id
        {
        }
    }
}