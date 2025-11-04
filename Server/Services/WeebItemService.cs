using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Mod;
using SPTarkov.Server.Core.Utils;
using WeebSights.Models;

namespace WeebSights.Services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 3)]
public class WeebItemService(ISptLogger<WeebItemService> logger, JsonUtil jsonUtil, CustomItemService  customItemService, DatabaseService databaseService)
{
    public bool TryLoadConfig(string filePath, out List<WeebItemConfig> outputObject)
    {
        var json = LoadConfig(filePath);
        if (json is null)
        {
            outputObject = [];
            return false;
        }

        outputObject = json;
        return true;
    }
    
    public List<WeebItemConfig>? LoadConfig(string filePath)
    {
        return jsonUtil.DeserializeFromFile<List<WeebItemConfig>>(filePath);
    }

    public void AddIronSightToFilters(WeebItemConfig sight)
    {
        var itemsWithSlots = databaseService.GetTemplates().Items.Where(i => i.Value.Properties?.Slots?.Count() > 0);
        foreach (var item in itemsWithSlots)
        {
            var backIronSightSlot = item.Value.Properties?.Slots?.FirstOrDefault(s => s.Name == "mod_sight_rear");
            if (backIronSightSlot == null)
            {
                continue;
            }

            var slotFilter = backIronSightSlot?.Properties?.Filters?.FirstOrDefault(f => f.Filter?.Contains(sight.CloneFromTpl) ?? false);
            if (slotFilter == null)
            {
                continue;
            }

            if (slotFilter.Filter?.Add(sight.Id) is false)
            {
                logger.Error("[Weeb Iron Sights] Failed to add filter to item " + item.Key);
                continue;
            }
            logger.Success($"[Weeb Iron Sights] Added {sight.Id} to filter on item {item.Key}");
        }
    }
    
    public IEnumerable<CreateItemResult> GenerateItems(List<WeebItemConfig> items, Dictionary<MongoId, WeebLocaleConfig> locales)
    {
        if (items.Count == 0)
        {
            yield break;
        }

        foreach (var item in items)
        {
            if (!locales.TryGetValue(item.Id, out var localeConfig))
            {
                logger.Error($"[Weeb Iron Sights] Failed to load locale for {item.Id}, using IDs for name");
                localeConfig = new WeebLocaleConfig()
                {
                    Name = item.Id,
                    Description = "FAILED TO LOAD LOCALES",
                    ShortName = "WEEBSIGHT",
                };
            }
            
            NewItemFromCloneDetails clonedItem = new()
            {
                NewId = item.Id,
                ItemTplToClone = item.CloneFromTpl,
                ParentId = "55818ac54bdc2d5b648b456e", // Ironsight
                HandbookParentId = "5b5f746686f77447ec5d7708", // CATEGORY
                HandbookPriceRoubles = item.Price,
                OverrideProperties = new TemplateItemProperties()
                {
                    Ergonomics = item.Ergonomics,
                    CreditsPrice = item.Price,
                    Prefab = new Prefab()
                    {
                        Path = item.BundlePath
                    },
                },
                Locales = new Dictionary<string, LocaleDetails>
                {
                    {
                        "en", new LocaleDetails()
                        {
                            Name = localeConfig.Name,
                            ShortName = localeConfig.ShortName,
                            Description = localeConfig.Description,
                        }
                    }
                }
            };
            var itemCreation = customItemService.CreateItemFromClone(clonedItem);
            if (itemCreation.Success is false or null)
            {
                logger.Error($"[Weeb Iron Sights] Failed to clone item {item.CloneFromTpl} into {item.Id}");
                itemCreation.Errors?.ForEach(e => logger.Critical("[Weeb Iron Sights] " + e));
                continue;
            }

            AddIronSightToFilters(item);
            yield return itemCreation;
        } 
    }
}