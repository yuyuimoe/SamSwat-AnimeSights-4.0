using System.Text.Json.Serialization;
using SPTarkov.Server.Core.Models.Common;

namespace moe.yuyui.weebsights_port.Models;

public record WeebLocaleConfig()
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("short_name")]
    public string ShortName { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
}