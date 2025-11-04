using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Extensions;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;

namespace moe.yuyui.weebsights_port.Services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
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
            var item = databaseService.GetItems().FirstOrDefault(i => i.Key ==  itemResult.ItemId);
            var traderItem = GenerateItemForTrader(item);
            var traderBarter = new BarterScheme()
            {
                Count = item.Value.Properties?.CreditsPrice ?? 1,
                Template = CurrencyType.RUB.GetCurrencyTpl()
            };
            
            trader.Assort.Items.Add(traderItem);
            if (!trader.Assort.BarterScheme.TryAdd(item.Key, [[traderBarter]]))
            {
                logger.Error($"[Weeb Iron Sights] Failed to add barter for item {item.Value.Name} with ID {item.Value.Id} in MECHANIC");
            }
            logger.Success($"[Weeb Iron Sights] Added item {item.Value.Name} with ID {item.Value.Id} to MECHANIC");
        }

        return true;
    }

    private Item GenerateItemForTrader(KeyValuePair<MongoId, TemplateItem> databaseItem)
    {
        return new Item
        {
            Id = databaseItem.Key,
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