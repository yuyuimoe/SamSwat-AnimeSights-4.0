using System.Reflection;
using Microsoft.Extensions.Logging;
using moe.yuyui.weebsights_port.Enums;
using moe.yuyui.weebsights_port.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Extensions;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Mod;
using SPTarkov.Server.Core.Utils;

namespace moe.yuyui.weebsights_port.Assorts;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class MechanicAssort(ISptLogger<MechanicAssort>logger, JsonUtil jsonUtil, DatabaseService databaseService, CustomItemService customItemService)
{
    public bool InjectAssortFromItemClone(IEnumerable<CreateItemResult> itemCloneResults)
    {
        if (!databaseService.GetTables().Traders.TryGetValue(Traders.MECHANIC, out var mechanic))
        {
            logger.Error("[Weeb Iron Sights] Failed to find MECHANIC trader");
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
            
            mechanic.Assort.Items.Add(traderItem);
            if (!mechanic.Assort.BarterScheme.TryAdd(item.Key, [[traderBarter]]))
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