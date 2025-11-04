using System.Text.Json.Serialization;
using SPTarkov.Server.Core.Models.Common;

namespace moe.yuyui.weebsights_port.Models;

public record WeebItemConfig()
{
    [JsonPropertyName("id")]
    public MongoId Id { get; set; }
    
    [JsonPropertyName("clone_from_tpl")]
    public MongoId CloneFromTpl { get; set; }
    
    [JsonPropertyName("bundle_path")]
    public string BundlePath { get; set; }
    
    [JsonPropertyName("price")]
    public int Price { get; set; }
    
    [JsonPropertyName("ergonomics")]
    public int Ergonomics { get; set; }
}