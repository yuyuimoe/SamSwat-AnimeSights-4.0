using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Modding.Custom;
using SPTarkov.Server.Core.Utils;
using WeebSights.Models;
using Path = System.IO.Path;

namespace WeebSights.Services;

[Injectable(InjectionType.Singleton, TypePriority = Mod.ModLoadOrder + 1)]
public class WeebItemService(
    JsonUtil jsonUtil,
    CustomItemService customItemService,
    TemplateTable templateTable
) : IOnLoad
{
    private const string ITEM_LOCATION = "/db/items.jsonc";

    private FrozenDictionary<MongoId, WeebItemConfig>? _weebItems;
    private FrozenDictionary<MongoId, MongoId>? _weebItemsCloneFrom;
    private FrozenDictionary<MongoId, string>? _weebItemCloneName;
    private FrozenDictionary<MongoId, TemplateItem>? _weebItemTemplate;

    public FrozenDictionary<MongoId, MongoId> WeebItemsCloneFrom =>
        _weebItemsCloneFrom ?? throw new Exception("Items not loaded in");

    public FrozenDictionary<MongoId, WeebItemConfig> WeebItems =>
        _weebItems ?? throw new Exception("Items not loaded in");

    public FrozenDictionary<MongoId, TemplateItem> WeebItemTemplate =>
        _weebItemTemplate ?? throw new Exception("Items not loaded in");

    public FrozenDictionary<MongoId, string>? WeebItemCloneName =>
        _weebItemCloneName ?? throw new Exception("Items not loaded in");

    public async Task OnLoadAsync(CancellationToken ct)
    {
        await Task.Run(
            async () =>
            {
#if DEBUG
                var watch = new Stopwatch();
                watch.Start();
#endif
                var config = await jsonUtil.DeserializeFromFileAsync<List<WeebItemConfig>>(
                    Path.Join(Mod.AssemblyLocation, ITEM_LOCATION),
                    ct
                );

                if (config is null or { Count: < 1 })
                {
                    Mod.Logger.Critical("[Weeb Item Sights] Failed to load items.");
                    return;
                }

                _weebItems = config.ToFrozenDictionary(x => x.Id);
                _weebItemsCloneFrom = config.ToFrozenDictionary(x => x.Id, x => x.CloneFromTpl);
                _weebItemCloneName = _weebItemsCloneFrom.ToFrozenDictionary(
                    x => x.Key,
                    x =>
                    {
                        var item = templateTable.Items[x.Value];
                        return item.Name ?? item.Id;
                    }
                );
                GenerateItems();
                _weebItemTemplate = templateTable
                    .Items.Where(x => _weebItems.ContainsKey(x.Key))
                    .ToFrozenDictionary();

#if DEBUG
                watch.Stop();
                Mod.Logger.Success($"[WeebSights] Items loaded in {watch.ElapsedMilliseconds}ms");
#endif
            },
            ct
        );
    }

    public void GenerateItems()
    {
        if (_weebItems?.Count == 0)
            return;

        foreach (var (tpl, item) in _weebItems!)
        {
            NewItemFromCloneDetails clonedItem = new()
            {
                NewId = tpl,
                ItemTplToClone = item.CloneFromTpl,
                NewItemName = string.Concat(_weebItemCloneName[tpl], "_", item.ItemName),
                ParentId = "55818ac54bdc2d5b648b456e", // Ironsight
                HandbookParentId = "5b5f746686f77447ec5d7708", // CATEGORY
                HandbookPriceRoubles = item.Price,
                FleaPriceRoubles = item.Price,
                OverrideProperties = new TemplateItemProperties
                {
                    Ergonomics = item.Ergonomics,
                    CreditsPrice = item.Price,
                    Prefab = new Prefab { Path = item.BundlePath },
                },
                Locales = new Dictionary<string, LocaleDetails>(), //Handled by WeebLocaleService
            };
            var itemCreation = customItemService.CreateItemFromClone(clonedItem);
            if (!itemCreation.Success)
            {
                Mod.Logger.Error(
                    $"[Weeb Iron Sights] Failed to clone item {item.CloneFromTpl} into {item.Id}"
                );
                itemCreation.Errors?.ForEach(e => Mod.Logger.Critical("[Weeb Iron Sights] " + e));
            }
        }
    }
}
