using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics;
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
using Path = System.IO.Path;

namespace WeebSights.Services;

[Injectable(InjectionType.Singleton, TypePriority = Mod.ModLoadOrder + 1)]
public class WeebItemService(
    JsonUtil jsonUtil,
    CustomItemService customItemService,
    DatabaseService databaseService
) : IOnLoad
{
    private const string ITEM_LOCATION = "/db/items.jsonc";

    private FrozenDictionary<MongoId, WeebItemConfig>? _weebItems;
    private FrozenDictionary<MongoId, MongoId>? _weebItemsCloneFrom;

    public FrozenDictionary<MongoId, MongoId> WeebItemsCloneFrom =>
        _weebItemsCloneFrom ?? throw new Exception("Items not loaded in");

    public FrozenDictionary<MongoId, WeebItemConfig> WeebItems =>
        _weebItems ?? throw new Exception("Items not loaded in");

    public async Task OnLoad()
    {
        await Task.Run(async () =>
        {
#if DEBUG
            var timer = new Stopwatch();
            timer.Start();
#endif
            var config =
                await jsonUtil.DeserializeFromFileAsync<List<WeebItemConfig>>(Path.Join(Mod.AssemblyLocation,
                    ITEM_LOCATION));

            if (config is null or { Count: < 1 })
            {
                Mod.Logger.Critical("[Weeb Item Sights] Failed to load items.");
                return;
            }

            _weebItems = config.ToFrozenDictionary(x => x.Id);
            _weebItemsCloneFrom = config.ToFrozenDictionary(x => x.Id, x => x.CloneFromTpl);

#if DEBUG
            timer.Stop();
            Mod.Logger.Info($"[WeebSights] Items loaded in {timer.ElapsedMilliseconds}ms");
#endif
        });
    }

    private void AddIronSightToFilters(WeebItemConfig sight)
    {
        var itemsWithSlots = databaseService
            .GetTemplates()
            .Items
            .Where(i => i.Value.Properties?.Slots?.Count() > 0);
        foreach (var item in itemsWithSlots)
        {
            var backIronSightSlot = item.Value.Properties?.Slots?.FirstOrDefault(s =>
                s.Name == "mod_sight_rear"
            );
            if (backIronSightSlot == null)
                continue;

            var slotFilter = backIronSightSlot?.Properties?.Filters?.FirstOrDefault(f =>
                f.Filter?.Contains(sight.CloneFromTpl) ?? false
            );
            if (slotFilter == null)
                continue;

            if (slotFilter.Filter?.Add(sight.Id) is false)
            {
                Mod.Logger.Error("[Weeb Iron Sights] Failed to add filter to item " + item.Key);
                continue;
            }
#if DEBUG
            Mod.Logger.Success($"[Weeb Iron Sights] Added {sight.Id} to filter on item {item.Key}");
#endif
        }
    }

    public IEnumerable<CreateItemResult> GenerateItems(ImmutableList<WeebItemConfig> items)
    {
        if (items.Count == 0)
            yield break;

        var langs = databaseService.GetLocales().Languages.Keys.ToHashSet();
        var cloneTpls = items.Select(x => x.CloneFromTpl).ToImmutableHashSet();

        foreach (var item in items)
        {
            NewItemFromCloneDetails clonedItem = new()
            {
                NewId = item.Id,
                ItemTplToClone = item.CloneFromTpl,
                ParentId = "55818ac54bdc2d5b648b456e", // Ironsight
                HandbookParentId = "5b5f746686f77447ec5d7708", // CATEGORY
                HandbookPriceRoubles = item.Price,
                OverrideProperties = new TemplateItemProperties
                {
                    Ergonomics = item.Ergonomics,
                    CreditsPrice = item.Price,
                    Prefab = new Prefab { Path = item.BundlePath }
                },
                Locales = new Dictionary<string, LocaleDetails>()
            };
            var itemCreation = customItemService.CreateItemFromClone(clonedItem);
            if (itemCreation.Success is false or null)
            {
                Mod.Logger.Error(
                    $"[Weeb Iron Sights] Failed to clone item {item.CloneFromTpl} into {item.Id}"
                );
                itemCreation.Errors?.ForEach(e => Mod.Logger.Critical("[Weeb Iron Sights] " + e));
                continue;
            }

            AddIronSightToFilters(item);
            yield return itemCreation;
        }
    }
}