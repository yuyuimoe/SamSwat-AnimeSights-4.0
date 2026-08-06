using System.Text.Json.Serialization;

namespace WeebSights.Models;

public record WeebLocaleConfig
{
    [JsonPropertyName("suffix")]
    public string Suffix { get; set; }

    [JsonPropertyName("short_name")]
    public string ShortName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
