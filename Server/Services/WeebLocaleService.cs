using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Utils;
using WeebSights.Models;

namespace WeebSights.Services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 4)]
public class WeebLocaleService(JsonUtil jsonUtil)
{
    public bool TryLoadLocales(string filePath, out Dictionary<MongoId, WeebLocaleConfig> outputObject)
    {
        var json = LoadLocales(filePath);
        if (json == null)
        {
            outputObject = new Dictionary<MongoId, WeebLocaleConfig>();
            return false;
        }

        outputObject = json;
        return true;
    }
    
    public Dictionary<MongoId, WeebLocaleConfig>? LoadLocales(string filePath)
    {
        return jsonUtil.DeserializeFromFile<Dictionary<MongoId, WeebLocaleConfig>>(filePath);
    }
}