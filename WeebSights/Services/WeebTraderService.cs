using System.Collections.Frozen;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Extensions;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;

namespace WeebSights.Services;

[Injectable(TypePriority = Mod.ModLoadOrder + 5)]
public class WeebTraderService(DatabaseService databaseService, WeebItemService weebItemService) : IOnLoad
{
    private static MongoId[] TRADERS = [Traders.MECHANIC];


    public async Task OnLoad()
    {
        Task.Run(GenerateItemsAssorts);
    }

    public void GenerateItemsAssorts()
    {
        var dbTraders = databaseService.GetTraders()
            .Where(x => TRADERS.Contains(x.Key))
            .ToFrozenDictionary();
        var dbItems = databaseService.GetItems()
            .Where(x => weebItemService.WeebItems.ContainsKey(x.Key))
            .ToFrozenDictionary();

        foreach (var (traderId, trader) in dbTraders)
        foreach (var (tpl, item) in dbItems)
        {
            var traderAssort = GenerateItemForTrader(tpl);
            var traderBarter = new BarterScheme
            {
                Count = item.Properties?.CreditsPrice ?? 7896,
                Template = ItemTpl.MONEY_ROUBLES
            };
            trader.Assort.Items.Add(traderAssort);
            if (!trader.Assort.BarterScheme.TryAdd(traderAssort.Id, [[traderBarter]]))
            {
                Mod.Logger.Error(
                    $"[Weeb Iron Sights] Failed to add assort for item {tpl} on trader {trader.Base.Nickname ?? traderId}");
                trader.Assort.Items.Remove(traderAssort);
                continue;
            }

            if (!trader.Assort.LoyalLevelItems.TryAdd(traderAssort.Id, 1))
            {
                Mod.Logger.Error(
                    $"[Weeb Iron Sights] Failed to add LL for item {tpl} on trader {trader.Base.Nickname ?? traderId}");
                trader.Assort.BarterScheme.Remove(traderAssort.Id);
                trader.Assort.Items.Remove(traderAssort);
            }
#if DEBUG
            Mod.Logger.Success(
                $"[Weeb Iron Sights] Added assort {traderAssort.Id} with template {tpl} to {trader.Base.Nickname ?? traderId}");
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
            Upd = new Upd
            {
                UnlimitedCount = true,
                StackObjectsCount = 99999
            }
        };
    }
}