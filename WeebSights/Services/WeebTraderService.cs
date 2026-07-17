using System.Collections.Frozen;
using System.Diagnostics;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Services;

namespace WeebSights.Services;

[Injectable(TypePriority = Mod.ModLoadOrder + 5)]
public class WeebTraderService(DatabaseService databaseService, WeebItemService weebItemService)
    : IOnLoad
{
    private static readonly MongoId[] AllowedTraders = [Traders.MECHANIC];
    private FrozenDictionary<MongoId, Trader>? _allowedTradersInstances;

    public async Task OnLoad()
    {
        await Task.Run(() =>
        {
#if DEBUG
            var watch = new Stopwatch();
            watch.Start();
#endif
            _allowedTradersInstances = databaseService
                .GetTraders()
                .Where(x => AllowedTraders.Contains(x.Key))
                .ToFrozenDictionary();

            GenerateItemsAssorts();
#if DEBUG
            watch.Stop();
            Mod.Logger.Success($"[WeebSights] Trader loaded in {watch.ElapsedMilliseconds}ms");
#endif
        });
    }

    private void GenerateItemsAssorts()
    {
        foreach (var (traderId, trader) in _allowedTradersInstances!)
        foreach (var (tpl, item) in weebItemService.WeebItemTemplate)
        {
            var traderAssort = GenerateItemForTrader(tpl);
            var traderBarter = new BarterScheme
            {
                Count = item.Properties?.CreditsPrice ?? 7896,
                Template = ItemTpl.MONEY_ROUBLES,
            };
            trader.Assort.Items.Add(traderAssort);
            if (
                !trader.Assort.BarterScheme.TryAdd(
                    traderAssort.Id,
                    [
                        [traderBarter],
                    ]
                )
            )
            {
                Mod.Logger.Error(
                    $"[Weeb Iron Sights] Failed to add assort for item {tpl} on trader {trader.Base.Nickname ?? traderId}"
                );
                trader.Assort.Items.Remove(traderAssort);
                continue;
            }

            if (!trader.Assort.LoyalLevelItems.TryAdd(traderAssort.Id, 1))
            {
                Mod.Logger.Error(
                    $"[Weeb Iron Sights] Failed to add LL for item {tpl} on trader {trader.Base.Nickname ?? traderId}"
                );
                trader.Assort.BarterScheme.Remove(traderAssort.Id);
                trader.Assort.Items.Remove(traderAssort);
            }
#if DEBUG
            Mod.Logger.Success(
                $"[Weeb Iron Sights] Added assort {traderAssort.Id} with template {tpl} to {trader.Base.Nickname ?? traderId}"
            );
#endif
        }
    }

    private Item GenerateItemForTrader(MongoId tpl)
    {
        return new Item
        {
            Id = new MongoId(),
            Template = tpl,
            ParentId = "hideout",
            SlotId = "hideout",
            Upd = new Upd { UnlimitedCount = true, StackObjectsCount = 99999 },
        };
    }
}
