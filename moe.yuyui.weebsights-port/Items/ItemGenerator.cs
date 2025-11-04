using System.Reflection;
using Microsoft.Extensions.Logging;
using moe.yuyui.weebsights_port.Interfaces;
using moe.yuyui.weebsights_port.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services.Mod;

namespace moe.yuyui.weebsights_port.Items;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
public class ItemGenerator(ISptLogger<ItemGenerator> logger, CustomItemService customItemService)
{
    public IEnumerable<CreateItemResult> GenerateWeebSights<T>() where T : IWeebSightEnum
    {
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static ).Where(f => f.FieldType == typeof(WeebSight)).ToArray();
        foreach (var field in fields)
        {
            WeebSight? sight = field.GetValue(typeof(WeebSight)) as WeebSight;
            if (sight == null)
            {
                logger.Error($"[Weeb Iron Sights] Failed to convert field {field.Name} of type {field.FieldType} into WeebSight");
                continue;
            }

            NewItemFromCloneDetails clonedItem = new()
            {
                NewId = sight.Id,
                ItemTplToClone = sight.CloneFromTpl,
                ParentId = "55818ac54bdc2d5b648b456e", // Ironsight
                HandbookParentId = "5b5f746686f77447ec5d7708", // CATEGORY
                HandbookPriceRoubles = 4000,
                OverrideProperties = new TemplateItemProperties()
                {
                    Ergonomics = 3,
                    CreditsPrice = 4000,
                    Prefab = new Prefab()
                    {
                        Path = $"assets/content/items/mods/sights rear/{sight.Asset}.bundle"
                    },
                },
                Locales = new Dictionary<string, LocaleDetails>
                {
                    {
                        "en", new LocaleDetails()
                        {
                            Name = $"{sight.Id} TODO LOCALE LOADER FROM JSON",
                            ShortName = "WEEBSIGHT",
                            Description = "DESCRIPTION",
                        }
                    }
                }
            };
            var itemCreation = customItemService.CreateItemFromClone(clonedItem);
            if (itemCreation.Success is false or null)
            {
                logger.Error($"[Weeb Iron Sights] Failed to clone item {sight.CloneFromTpl} into {sight.Id}");
                itemCreation.Errors?.ForEach(e => logger.Critical("[Weeb Iron Sights] " + e));
                continue;
            }
            yield return itemCreation;
        }
    }
}