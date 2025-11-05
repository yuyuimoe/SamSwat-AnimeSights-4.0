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

[Injectable]
public class WeebTraderService(ISptLogger<WeebTraderService> logger, DatabaseService databaseService)
{
    public bool AddToAssortFromItemClone(MongoId traderId, IEnumerable<CreateItemResult> itemCloneResults)
    {
        if (!databaseService.GetTables().Traders.TryGetValue(traderId, out var trader))
        {
            logger.Error($"[Weeb Iron Sights] Failed to find trader with ID {traderId}");
            return false;
        }
        
        foreach (var itemResult in itemCloneResults)
        {
            var item = databaseService.GetItems().First(i => i.Key == itemResult.ItemId);
            var traderItem = GenerateItemForTrader(item);
            var traderBarter = new BarterScheme()
            {
                Count = item.Value.Properties?.CreditsPrice ?? 1,
                Template = CurrencyType.RUB.GetCurrencyTpl()
            };
            
            trader.Assort.Items.Add(traderItem);
            if (!trader.Assort.BarterScheme.TryAdd(traderItem.Id, [[traderBarter]]))
            {
                logger.Error(
                    $"[Weeb Iron Sights] Failed to add barter {traderItem.Id} for item {item.Value.Name} with ID {item.Value.Id} in {trader.Base.Name}. Item will not be added");
                trader.Assort.Items.Remove(traderItem);
                continue;
            }

            if (!trader.Assort.LoyalLevelItems.TryAdd(traderItem.Id, 1))
            {
                logger.Critical(
                    $"[Weeb Iron Sight] Failed to add loyalty requirements for item {item.Value.Name} with ID {item.Value.Id} in {trader.Base.Name}. Item will not be added");
                trader.Assort.Items.Remove(traderItem);
                continue;
            }
#if DEBUG
            logger.Success($"[Weeb Iron Sights] Added item {traderItem.Id} with ID {item.Value.Id} and barter {traderItem.Id} to MECHANIC");
#endif
        }

        return true;
    }

    private Item GenerateItemForTrader(KeyValuePair<MongoId, TemplateItem> databaseItem)
    {
        return new Item
        {
            Id = new MongoId(),
            Template = databaseItem.Value.Id,
            ParentId = "hideout",
            SlotId = "hideout",
            Upd = new Upd()
            {
                UnlimitedCount = true,
                StackObjectsCount = 99999,
            }
        };
    }
}