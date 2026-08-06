using SPTarkov.Server.Core.Models.Spt.Mod;

namespace WeebSights;

public record ModMetadata : IModMetadata
{
    public string Name { get; init; } = "Weeb Iron Sights";
    public string Author { get; init; } = "yuyui.moe";
    public List<string>? Contributors { get; init; } = ["SamSWAT"];
    public SemanticVersioning.Version Version { get; init; } = new("1.4.0");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
    public bool HasPrepatcher { get; init; } = false;

    public List<string>? Incompatibilities { get; init; }

    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }

    public string? Url { get; init; } = "https://github.com/yuyuimoe/SamSwat-AnimeSights-4.0/";

    public bool? IsBundleMod { get; init; } = true;
    public string? License { get; init; } = "MIT";
    public string ModGuid { get; init; } = "moe.yuyui.animesightsport";
}
