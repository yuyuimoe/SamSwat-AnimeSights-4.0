using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;
using WeebSights.Models;

namespace WeebSights.Services;

[Injectable]
public class WeebLootService(JsonUtil jsonUtil, DatabaseService databaseService, ISptLogger<WeebLootService> logger)
{
    public bool TryLoadConfig(string filePath, out WeebLootConfig outputObject)
    {
        var json = LoadConfig(filePath);
        if (json is null)
        {
            outputObject = new WeebLootConfig();
            return false;
        }

        outputObject = json;
        return true;
    }

    public WeebLootConfig? LoadConfig(string filePath)
    {
        return jsonUtil.DeserializeFromFile<WeebLootConfig>(filePath);
    }

    public void RegisterToStaticLoot(WeebLootConfig lootConfig, MongoId itemId)
    {
        foreach (var location in lootConfig.Locations)
        {
            var locations = databaseService.GetTables().Locations;

            if (!locations.GetDictionary().TryGetValue(locations.GetMappedKey(location), out var eftLocation))
            {
                logger.Warning($"[Weeb Iron Sights] Invalid location {location} on preset {lootConfig.Name}");
                continue;
            }

            var staticLoot = lootConfig.Containers.Select(c => new ItemDistribution
                { Tpl = c.Key, RelativeProbability = c.Value }).ToList();

            eftLocation.StaticLoot!.AddTransformer(t =>
            {
                t.Add(itemId, new StaticLootDetails { ItemDistribution = staticLoot });
                return t;
            });
#if DEBUG
            logger.Success($"[Weeb Iron Sights] Added {itemId} to containers on {location}.");
#endif
        }
    }
}