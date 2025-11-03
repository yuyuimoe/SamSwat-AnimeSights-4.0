using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;

namespace moe.yuyui.weebsights_port.Locales;

[Injectable(TypePriority = OnLoadOrder.PostSptModLoader + 1)]
public class EnglishLocale(
    DatabaseService databaseService,
    LocaleService localeService,
    ServerLocalisationService serverLocalisationService)
{
    public bool LoadLocales()
    {
        if (!databaseService.GetLocales().Global.TryGetValue("en", out var localeEn))
        {
            return false;
        }
        localeEn.AddTransformer(l =>
        { 
	        l.Add("6753d006cec7fc449f055445 Name", "Magpul MBUS Gen2 rear flip-up sight [Anti]");
            l.Add("6753d006cec7fc449f055445 ShortName", "MBUS RS");
            l.Add("6753d006cec7fc449f055445 Description", "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. Your personal waifu giving her neko love to support you in combat.");
            
            l.Add("6753d006cec7fc449f055446 Name", "Magpul MBUS Gen2 rear flip-up sight [Ai Fuyuumi]");
            l.Add("6753d006cec7fc449f055446 ShortName", "MBUS RS");
            l.Add("6753d006cec7fc449f055446 Description", "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul.");
            
	 	 	l.Add("6753d006cec7fc449f055447 Name", "Magpul MBUS Gen2 rear flip-up sight [GF M4 SOPMOD II]");
	 	 	l.Add("6753d006cec7fc449f055447 ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f055447 Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul.");
            
	 	 	l.Add("6753d006cec7fc449f055448 Name", "Magpul MBUS Gen2 rear flip-up sight [Animu 3]");
	 	 	l.Add("6753d006cec7fc449f055448 ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f055448 Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul.");

	 	 	l.Add("6753d006cec7fc449f055449 Name", "Magpul MBUS Gen2 rear flip-up sight [Animu 3 v2]");
	 	 	l.Add("6753d006cec7fc449f055449 ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f055449 Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul.");

	 	 	l.Add("6753d006cec7fc449f05544a Name", "Magpul MBUS Gen2 rear flip-up sight [Anti 2]");
	 	 	l.Add("6753d006cec7fc449f05544a ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f05544a Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. The personal sight used by Anti, the war Neko! Nya!!");
            
	 	 	l.Add("6753d006cec7fc449f05544b Name", "Magpul MBUS Gen2 rear flip-up sight [Ram & Rem]");
	 	 	l.Add("6753d006cec7fc449f05544b ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f05544b Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul.");

	 	 	l.Add("6753d006cec7fc449f05544c Name", "Magpul MBUS Gen2 rear flip-up sight (FDE) [Ai Fuyuumi]");
	 	 	l.Add("6753d006cec7fc449f05544c ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f05544c Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. Flat Dark Earth color.");

	 	 	l.Add("6753d006cec7fc449f05544d Name", "Magpul MBUS Gen2 rear flip-up sight (FDE) [GF M4 SOPMOD II]");
	 	 	l.Add("6753d006cec7fc449f05544d ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f05544d Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. Flat Dark Earth color.");

	 	 	l.Add("6753d006cec7fc449f05544e Name", "Magpul MBUS Gen2 rear flip-up sight (FDE) [Animu 3]");
	 	 	l.Add("6753d006cec7fc449f05544e ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f05544e Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. Flat Dark Earth color.");

	 	 	l.Add("6753d006cec7fc449f05544f Name", "Magpul MBUS Gen2 rear flip-up sight (FDE) [Animu 3 v2]");
	 	 	l.Add("6753d006cec7fc449f05544f ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f05544f Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. Flat Dark Earth color.");

	 	 	l.Add("6753d006cec7fc449f055450 Name", "Magpul MBUS Gen2 rear flip-up sight (FDE) [Anti]");
	 	 	l.Add("6753d006cec7fc449f055450 ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f055450 Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. Flat Dark Earth color. Your personal waifu giving her neko love to support you in combat.");

	 	 	l.Add("6753d006cec7fc449f055451 Name", "Magpul MBUS Gen2 rear flip-up sight (FDE) [Anti 2]");
	 	 	l.Add("6753d006cec7fc449f055451 ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f055451 Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. Flat Dark Earth color. The personal sight used by Anti, the war Neko! Nya!!");

	 	 	l.Add("6753d006cec7fc449f055452 Name", "Magpul MBUS Gen2 rear flip-up sight (FDE) [Ram & Rem]");
	 	 	l.Add("6753d006cec7fc449f055452 ShortName", "MBUS RS");
		    l.Add("6753d006cec7fc449f055452 Description",
			    "Removable flip-up rear sight MBUS Gen2, installed on the mount. Manufactured by Magpul. Flat Dark Earth color.");

		    l.Add("6753d006cec7fc449f055453 Name", "MCX flip-up rear sight [Ai Fuyuumi]");
			    l.Add("6753d006cec7fc449f055453 ShortName", "MCX RS");
			    l.Add("6753d006cec7fc449f055453 Description",
				    "Removable flip-up rear sight for MCX assault rifles, manufactured by SIG Sauer.");

			    l.Add("6753d006cec7fc449f055454 Name", "MCX flip-up rear sight [GF M4 SOPMOD II]");
			    l.Add("6753d006cec7fc449f055454 ShortName", "MCX RS");
			    l.Add("6753d006cec7fc449f055454 Description",
				    "Removable flip-up rear sight for MCX assault rifles, manufactured by SIG Sauer.");

			l.Add("6753d006cec7fc449f055455 Name", "MCX flip-up rear sight [Animu 3]");
			l.Add("6753d006cec7fc449f055455 ShortName", "MCX RS");
			l.Add("6753d006cec7fc449f055455 Description",
				"Removable flip-up rear sight for MCX assault rifles, manufactured by SIG Sauer.");

			l.Add("6753d006cec7fc449f055456 Name", "MCX flip-up rear sight [Animu 3 v2]");
			l.Add("6753d006cec7fc449f055456 ShortName", "MCX RS");
			l.Add("6753d006cec7fc449f055456 Description",
				"Removable flip-up rear sight for MCX assault rifles, manufactured by SIG Sauer.");

			l.Add("6753d006cec7fc449f055457 Name", "MCX flip-up rear sight [Anti]");
			l.Add("6753d006cec7fc449f055457 ShortName", "MCX RS");
			l.Add("6753d006cec7fc449f055457 Description",
				"Removable flip-up rear sight for MCX assault rifles, manufactured by SIG Sauer. Your personal waifu giving her neko love to support you in combat.");

			l.Add("6753d006cec7fc449f055458 Name", "MCX flip-up rear sight [Anti 2]");
			l.Add("6753d006cec7fc449f055458 ShortName", "MCX RS");
			l.Add("6753d006cec7fc449f055458 Description",
				"Removable flip-up rear sight for MCX assault rifles, manufactured by SIG Sauer. The personal sight used by Anti, the war Neko! Nya!!");

			l.Add("6753d006cec7fc449f055459 Name", "MCX flip-up rear sight [Ram & Rem]");
			l.Add("6753d006cec7fc449f055459 ShortName", "MCX RS");
			l.Add("6753d006cec7fc449f055459 Description",
				"Removable flip-up rear sight for MCX assault rifles, manufactured by SIG Sauer.");
			return l;
        });
        return true;
    }
}