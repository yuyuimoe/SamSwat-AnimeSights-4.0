using System.Diagnostics;
using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;
using WeebSights.Services;

namespace WeebSights;

[Injectable(TypePriority = OnLoadOrder.PostSptModLoader + 1)]
public class Mod(ISptLogger<Mod> logger,
    WeebItemService weebItemService,
    WeebTraderService weebTraderService,
    WeebLocaleService weebLocaleService): IOnLoad
{
    public Task OnLoad()
    {
        var stopWatch = Stopwatch.StartNew();
        var currentAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        logger.Info($"[Weeb Iron Sights] Loaded in {currentAssemblyPath}");
        if (!weebLocaleService.TryLoadLocales(Path.Join(currentAssemblyPath, "/db/locales/en.json"), out var locales))
        {
            logger.Error("[Weeb Iron Sights] Failed to load locales. Names might be weird");
        }
        
        if(!weebItemService.TryLoadConfig(Path.Join(currentAssemblyPath, "/db/items.jsonc"), out var items))
        {
            logger.Critical("[Weeb Iron Sights] Failed to load items. MOD WILL NOT LOAD");
            stopWatch.Stop();
            return Task.CompletedTask;
        }

        var resultItemCreation = weebItemService.GenerateItems(items, locales);
        if (!weebTraderService.AddToAssortFromItemClone(Traders.MECHANIC, resultItemCreation))
        {
            logger.Critical("[Weeb Iron Sights] Failed to add items into Mechanic. Sights wont be available for purchase");
        }

        stopWatch.Stop();
        logger.Success($"[Weeb Iron Sights] Loaded in {stopWatch.ElapsedMilliseconds} ms");
        return Task.CompletedTask;
    }
}