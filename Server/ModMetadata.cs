using SPTarkov.Server.Core.Models.Spt.Mod;

namespace WeebSights;

public record ModMetadata : AbstractModMetadata
{
    public override string Name { get; init; } = "Weeb Iron Sights";
    public override string Author { get; init; } = "yuyui.moe";
    public override List<string>? Contributors { get; init; } = ["SamSWAT"];
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
        
        
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; } = "https://github.com/yuyuimoe/SamSwat-AnimeSights-4.0/";
    public override bool? IsBundleMod { get; init; } = true;
    public override string? License { get; init; } = "MIT";
    public override string ModGuid { get; init; } = "moe.yuyui.animesights-port";
}