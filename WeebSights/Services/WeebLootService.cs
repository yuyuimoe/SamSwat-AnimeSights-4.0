using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;
using WeebSights.Models;

namespace WeebSights.Services;

[Injectable(TypePriority = Mod.ModLoadOrder + 4)]
public class WeebLootService(
    JsonUtil jsonUtil,
    DatabaseService databaseService,
    WeebItemService weebItemService
) : IOnLoad
{
    private const string CONFIG_PATH = "/db/loot/default.jsonc";

    public async Task OnLoad()
    {
        await Task.Run(async () =>
        {
#if DEBUG
            var watch = new Stopwatch();
            watch.Start();
#endif
            var config = await jsonUtil.DeserializeFromFileAsync<WeebLootConfig>(
                Path.Join(Mod.AssemblyLocation, CONFIG_PATH)
            );
            if (config is null)
            {
                Mod.Logger.Critical(
                    "[Weeb Iron Sights] Failed to load loot tables. Sights won't spawn in raid."
                );
                return;
            }

            RegisterStaticLoot(config);
#if DEBUG
            watch.Stop();
            Mod.Logger.Success($"[WeebSights] Loot loaded in {watch.ElapsedMilliseconds}ms");
#endif
        });
    }

    private void RegisterStaticLoot(WeebLootConfig lootConfig)
    {
        var locations = databaseService.GetTables().Locations;
        var locationDict = locations.GetDictionary().ToFrozenDictionary();
        foreach (var location in lootConfig.Locations)
        {
            var mappedKey = locations.GetMappedKey(location);
            if (mappedKey == location)
            {
                Mod.Logger.Warning(
                    $"[Weeb Iron Sights] Invalid location {location} name on preset {lootConfig.Name}"
                );
                continue;
            }

            if (!locationDict.TryGetValue(mappedKey, out var eftLocation))
            {
                Mod.Logger.Warning(
                    $"[Weeb Iron Sights] Failed to find {location} on eft, on preset {lootConfig.Name}"
                );
                continue;
            }

            var staticLoot = lootConfig
                .Containers.Select(c => new ItemDistribution
                {
                    Tpl = c.Key,
                    RelativeProbability = c.Value,
                })
                .ToImmutableList();

            foreach (var tpl in weebItemService.WeebItems.Keys)
            {
                eftLocation.StaticLoot!.AddTransformer(t =>
                {
                    t!.Add(tpl, new StaticLootDetails { ItemDistribution = staticLoot });
                    return t;
                });
#if DEBUG
                Mod.Logger.Success($"[Weeb Iron Sights] Added {tpl} to containers on {location}.");
#endif
            }
        }
    }
}
