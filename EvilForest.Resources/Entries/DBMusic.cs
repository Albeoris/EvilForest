using System;
using Newtonsoft.Json;

namespace EvilForest.Resources
{
    public sealed class DBMusic : IDBEntry<DBMusic.Id>
    {
        [JsonProperty("id")] public Id EntryId { get; set; }
        [JsonProperty("fileName")] public String FileName { get; set; } = null!;
        [JsonProperty("assetPath")] public String AssetPath { get; set; } = null!;
        [JsonProperty("displayName")] public String DisplayName { get; set; } = null!;
        
        public enum Id
        {
        }
    }
}