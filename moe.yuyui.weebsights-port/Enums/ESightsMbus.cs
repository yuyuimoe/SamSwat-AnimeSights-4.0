using moe.yuyui.weebsights_port.Interfaces;
using moe.yuyui.weebsights_port.Models;
using SPTarkov.Server.Core.Models.Common;

namespace moe.yuyui.weebsights_port.Enums;

public class ESightsMbus : IWeebSightEnum
{
    public static readonly WeebSight Anti = new("6753d006cec7fc449f055445", "sight_rear_all_magpul_mbus_anti", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Anti2 = new("6753d006cec7fc449f05544a", "sight_rear_all_magpul_mbus_gen2_anti2", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight FdeAnti = new("6753d006cec7fc449f055450","sight_rear_all_magpul_mbus_gen2_fde_anti", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight FdeAnti2 = new("6753d006cec7fc449f055451", "sight_rear_all_magpul_mbus_gen2_fde_anti2", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Fuyuumi = new("6753d006cec7fc449f055446", "sight_rear_all_magpul_mbus_gen2_anime", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight FdeFuyuumi = new("6753d006cec7fc449f05544c","sight_rear_all_magpul_mbus_gen2_fde_anime", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight M4Sopmod = new("6753d006cec7fc449f055447","sight_rear_all_magpul_mbus_gen2_anime2", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight FdeM4Sopmod = new("6753d006cec7fc449f05544d","sight_rear_all_magpul_mbus_gen2_fde_anime2", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Animu3 = new("6753d006cec7fc449f055448","sight_rear_all_magpul_mbus_gen2_anime3", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight FdeAnimu3 = new("6753d006cec7fc449f05544e","sight_rear_all_magpul_mbus_gen2_fde_anime3", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Animu3V2 = new("6753d006cec7fc449f055449","sight_rear_all_magpul_mbus_gen2_anime3v2", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight FdeAnimu3V2 = new("6753d006cec7fc449f05544f","sight_rear_all_magpul_mbus_gen2_fde_anime3v2", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Ramrem = new("6753d006cec7fc449f05544b","sight_rear_all_magpul_mbus_gen2_ramrem", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight FdeRamrem = new("6753d006cec7fc449f055452","sight_rear_all_magpul_mbus_gen2_fde_ramrem", ItemTpl.IRONSIGHT_MAGPUL_MBUS_GEN2_FLIPUP_REAR_SIGHT);
}