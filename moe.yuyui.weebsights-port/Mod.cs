using moe.yuyui.weebsights_port.Locales;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;

namespace moe.yuyui.weebsights_port;

[Injectable(TypePriority = OnLoadOrder.PostSptModLoader)]
public class Mod(ISptLogger<Mod> logger, EnglishLocale englishLocale): IOnLoad
{
    public Task OnLoad()
    {
        if (!englishLocale.LoadLocales())
        {
            logger.Error("[Weeb Iron Sights] Failed to load locales, you may see garbled text");
        }
        logger.Success("[Weeb Iron Sights] Items loaded successfully");
        return Task.CompletedTask;
    }
}