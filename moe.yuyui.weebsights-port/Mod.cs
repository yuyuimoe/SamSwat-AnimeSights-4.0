using System.Diagnostics;
using moe.yuyui.weebsights_port.Assorts;
using moe.yuyui.weebsights_port.Enums;
using moe.yuyui.weebsights_port.Items;
using moe.yuyui.weebsights_port.Locales;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;

namespace moe.yuyui.weebsights_port;

[Injectable(TypePriority = OnLoadOrder.PostSptModLoader + 1)]
public class Mod(ISptLogger<Mod> logger,
    MechanicAssort mechanicAssort,
    ItemGenerator itemGenerator): IOnLoad
{
    public Task OnLoad()
    {
        var stopWatch = Stopwatch.StartNew();
        var sightsMbus = itemGenerator.GenerateWeebSights<ESightsMbus>();
        var sightsMcx = itemGenerator.GenerateWeebSights<ESightsMcx>();
        if (!mechanicAssort.InjectAssortFromItemClone(sightsMbus))
        {
            logger.Critical("[Weeb Iron Sights] Failed to inject MBUS Sights");
        }


        if (!mechanicAssort.InjectAssortFromItemClone(sightsMcx))
        {
            logger.Critical("[Weeb Iron Sights] Failed to inject MCX Sights");
        }

        stopWatch.Stop();
        logger.Success($"[Weeb Iron Sights] Loaded in {stopWatch.ElapsedMilliseconds} ms");
        return Task.CompletedTask;
    }
}