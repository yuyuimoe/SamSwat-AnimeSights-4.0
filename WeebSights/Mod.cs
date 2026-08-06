using System.Reflection;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;

namespace WeebSights;

[Injectable(TypePriority = ModLoadOrder)]
public class Mod(ISptLogger<Mod> logger) : IOnLoad
{
    public static string AssemblyLocation { get; private set; } =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    public const int ModLoadOrder = OnLoadOrder.PostLoad + 65535;

    public static ISptLogger<Mod> Logger { get; private set; }

    public Task OnLoadAsync(CancellationToken ct)
    {
        Logger = logger;

        return Task.CompletedTask;
    }
}
