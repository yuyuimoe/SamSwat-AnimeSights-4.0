using System.Text.Json.Serialization;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Enums;

namespace WeebSights.Models;

public class WeebLootConfig
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("locations")]
    public List<string> Locations { get; set; }
    
    [JsonPropertyName("containers")]
    public Dictionary<MongoId, int> Containers { get; set; }
}