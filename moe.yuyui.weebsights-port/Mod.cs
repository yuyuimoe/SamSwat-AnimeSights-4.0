using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;

namespace moe.yuyui.weebsights_port;

[Injectable(TypePriority = OnLoadOrder.PostSptModLoader)]
public class Mod(ISptLogger<Mod> logger): IOnLoad
{
    public Task OnLoad()
    {
        logger.Success("[Weeb Iron Sights] Items loaded successfully");
        return Task.CompletedTask;
    }
}