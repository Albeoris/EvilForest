using System;
using Newtonsoft.Json;

namespace EvilForest.Resources
{
    public sealed class DBScriptName : IDBEntry<DBScriptName.Id>
    {
        [JsonProperty("id")] public Id EntryId { get; set; }
        [JsonProperty("name")] public String Name { get; set; } = null!;
        
        public enum Id
        {
        }
    }
}