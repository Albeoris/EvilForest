using System;
using Newtonsoft.Json;

namespace EvilForest.Resources
{
    public sealed class DBModel : IDBEntry<DBModel.Id>
    {
        [JsonProperty("id")] public Id EntryId { get; set; }
        [JsonProperty("category")] public Int32 Category { get; set; }
        [JsonProperty("module")] public Int32 Module { get; set; }
        [JsonProperty("version")] public Int32 Version { get; set; }
        [JsonProperty("fileName")] public String FileName { get; set; } = null!;
        [JsonProperty("displayName")] public String DisplayName { get; set; } = null!;
        
        public enum Id
        {
        }
    }
}