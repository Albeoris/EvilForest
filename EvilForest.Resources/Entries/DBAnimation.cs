using System;
using Newtonsoft.Json;

namespace EvilForest.Resources
{
    public sealed class DBAnimation : IDBEntry<DBAnimation.Id>
    {
        [JsonProperty("id")] public Id EntryId { get; set; }
        [JsonProperty("fileName")] public String FileName { get; set; } = null!;
        
        public enum Id
        {
        }
    }
}