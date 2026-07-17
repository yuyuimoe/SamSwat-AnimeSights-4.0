using System.Diagnostics;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Services;
using WeebSights.Services;

namespace WeebSights;

[Injectable]
public class LateLoad(DatabaseService databaseService, WeebItemService weebItemService) : IOnLoad
{
    public async Task OnLoad()
    {
        await Task.Run(() =>
        {
#if DEBUG
            var watch = new Stopwatch();
            watch.Start();
#endif

            AddIronSightToFilters();

#if DEBUG
            watch.Stop();
            Mod.Logger.Success($"[WeebSights] Loaded late load in {watch.ElapsedMilliseconds}ms");
#endif
        });
    }

    private void AddIronSightToFilters()
    {
        var slots = databaseService
            .GetTemplates()
            .Items.Where(i => i.Value.Properties?.Slots?.Count() > 0)
            .Select(i => i.Value.Properties?.Slots?.FirstOrDefault(s => s.Name == "mod_sight_rear"))
            .Where(x => x is not null);

        foreach (var (tpl, parent) in weebItemService.WeebItemsCloneFrom)
        {
            var compatibleSlotsFilter = slots
                .Select(s =>
                    s?.Properties?.Filters?.FirstOrDefault(f => f.Filter?.Contains(parent) ?? false)
                )
                .Where(s => s is not null);

            foreach (var slot in compatibleSlotsFilter)
            {
                if (slot!.Filter?.Add(tpl) is null or false)
                {
                    Mod.Logger.Error($"[Weeb Iron Sights] Failed to add filter to item {tpl}");
                    continue;
                }
#if DEBUG
                Mod.Logger.Success($"[Weeb Iron Sights] Added filter on item {tpl}");
#endif
            }
        }
    }
}
