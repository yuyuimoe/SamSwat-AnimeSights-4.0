using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;

namespace WeebSights;

[Injectable(TypePriority = ModLoadOrder)]
public class Mod(ISptLogger<Mod> logger) : IOnLoad
{
    public static string AssemblyLocation { get; private set; } =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    public const int ModLoadOrder = OnLoadOrder.PostDBModLoader + 65535;

    public static ISptLogger<Mod> Logger { get; private set; }

    public Task OnLoad()
    {
        Logger = logger;

        return Task.CompletedTask;
    }
}