using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;
using SPTarkov.Server.Core.Utils.Json;
using WeebSights.Models;

namespace WeebSights.Services;

[Injectable(InjectionType.Singleton, TypePriority = Mod.ModLoadOrder + 3)]
public class WeebLocaleService(
    JsonUtil jsonUtil,
    LocaleTable localeTable,
    WeebItemService weebItemService
) : IOnLoad
{
    private const string LOCALES_PATH = "db/locales/";

    private FrozenDictionary<
        string,
        FrozenDictionary<MongoId, WeebLocaleConfig>
    >? _localesPerLanguage;

    private FrozenDictionary<
        MongoId,
        FrozenDictionary<string, WeebLocaleConfig>
    >? _localesPerTemplate;

    private FrozenDictionary<
        MongoId,
        FrozenDictionary<string, WeebLocaleConfig>
    >? _builtLocalesByTemplate;

    public FrozenDictionary<
        string,
        FrozenDictionary<MongoId, WeebLocaleConfig>
    > LocalesPerLanguage => _localesPerLanguage ?? throw new Exception("Locales not loaded yet");

    public FrozenDictionary<
        MongoId,
        FrozenDictionary<string, WeebLocaleConfig>
    > LocalesPerTemplate => _localesPerTemplate ?? throw new Exception("Locales not loaded yet");

    public FrozenDictionary<
        MongoId,
        FrozenDictionary<string, WeebLocaleConfig>
    > BuiltLocalesByTemplate =>
        _builtLocalesByTemplate ?? throw new Exception("Locales not loaded yet");

    private Dictionary<MongoId, WeebLocaleConfig>? LoadLocales(string filePath)
    {
        return jsonUtil.DeserializeFromFile<Dictionary<MongoId, WeebLocaleConfig>>(filePath);
    }

    public async Task OnLoadAsync(CancellationToken ct)
    {
        await Task.Run(
            () =>
            {
#if DEBUG
                var watch = new Stopwatch();
                watch.Start();
#endif
                _localesPerLanguage = BuildModLocaleByLanguage();
                _localesPerTemplate = BuildModLocaleByTemplate();
                LazyLoadNewLocales();
#if DEBUG
                watch.Stop();
                Mod.Logger.Success($"[WeebSights] Locale loaded in {watch.ElapsedMilliseconds}ms");
#endif
            },
            ct
        );
    }

    private void LazyLoadNewLocales()
    {
        var keys = localeTable.Languages.Keys;
        var gameLocales = localeTable.Global;
        foreach (var lang in keys)
        {
            if (!gameLocales.TryGetValue(lang, out var lazyLoad))
                continue;

            if (!_localesPerLanguage!.TryGetValue(lang, out var locales))
            {
#if DEBUG
                Mod.Logger.Warning(
                    $"[WeebSights] No locales for language {lang}. Using defaults from parent + english"
                );
#endif
                lazyLoad.AddTransformer(localeData =>
                {
                    foreach (var (tpl, parentTpl) in weebItemService.WeebItemsCloneFrom)
                    {
                        var locale = _localesPerTemplate![tpl]["en"];
                        localeData![$"{tpl} Name"] = string.Join(
                            " ",
                            localeData[$"{parentTpl} Name"],
                            locale.Suffix
                        );
                        localeData[$"{tpl} Description"] = string.Join(
                            "\n",
                            localeData[$"{parentTpl} Description"],
                            locale.Description
                        );
                        localeData[$"{tpl} ShortName"] = string.IsNullOrWhiteSpace(locale.ShortName)
                            ? localeData[$"{parentTpl} ShortName"]
                            : locale.ShortName;
                    }

                    return localeData;
                });

                continue;
            }

            lazyLoad.AddTransformer(localeData =>
            {
                foreach (var (tpl, locale) in locales!)
                {
                    if (!weebItemService.WeebItemsCloneFrom.TryGetValue(tpl, out var parentTpl))
                    {
                        Mod.Logger.Error(
                            $"[Weeb Sights Port] Failed to find parent clone for {tpl}"
                        );
                        continue;
                    }

                    localeData![$"{tpl} Name"] = string.Join(
                        " ",
                        localeData[$"{parentTpl} Name"],
                        locale.Suffix
                    );
                    localeData[$"{tpl} Description"] = string.Join(
                        "\n",
                        localeData[$"{parentTpl} Description"],
                        locale.Description
                    );
                    localeData[$"{tpl} ShortName"] = string.IsNullOrWhiteSpace(locale.ShortName)
                        ? localeData[$"{parentTpl} ShortName"]
                        : locale.ShortName;
                }

                return localeData;
            });
        }
    }

    private FrozenDictionary<
        string,
        FrozenDictionary<MongoId, WeebLocaleConfig>
    > BuildModLocaleByLanguage()
    {
        return Directory
            .EnumerateFiles(Path.Join(Mod.AssemblyLocation, LOCALES_PATH), "*.json")
            .Select(f => (lang: Path.GetFileNameWithoutExtension(f), locale: LoadLocales(f)))
            .Where(x => x.locale is not null)
            .ToFrozenDictionary(x => x.lang, x => x.locale!.ToFrozenDictionary());
    }

    private FrozenDictionary<
        MongoId,
        FrozenDictionary<string, WeebLocaleConfig>
    > BuildModLocaleByTemplate()
    {
        return _localesPerLanguage!
            .SelectMany(ll => ll.Value.Select(wlc => (tpl: wlc.Key, lo: wlc.Value, la: ll.Key)))
            .GroupBy(x => x.tpl)
            .ToFrozenDictionary(g => g.Key, g => g.ToFrozenDictionary(k => k.la, k => k.lo));
    }
}
