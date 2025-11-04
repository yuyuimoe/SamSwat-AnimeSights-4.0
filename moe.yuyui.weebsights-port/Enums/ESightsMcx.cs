using moe.yuyui.weebsights_port.Interfaces;
using moe.yuyui.weebsights_port.Models;
using SPTarkov.Server.Core.Models.Common;

namespace moe.yuyui.weebsights_port.Enums;

public class ESightsMcx : IWeebSightEnum
{
    public static readonly WeebSight Fuyuumi = new("6753d006cec7fc449f055453", "sight_rear_all_sig_flip_up_anime", ItemTpl.IRONSIGHT_MCX_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight M4Sopmod = new("6753d006cec7fc449f055454", "sight_rear_all_sig_flip_up_anime2", ItemTpl.IRONSIGHT_MCX_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Animu3 = new("6753d006cec7fc449f055455", "sight_rear_all_sig_flip_up_anime3", ItemTpl.IRONSIGHT_MCX_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Animu3V2 = new("6753d006cec7fc449f055456", "sight_rear_all_sig_flip_up_anime3v2", ItemTpl.IRONSIGHT_MCX_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Anti = new("6753d006cec7fc449f055457", "sight_rear_all_sig_flip_up_anti", ItemTpl.IRONSIGHT_MCX_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Anti2 = new("6753d006cec7fc449f055458", "sight_rear_all_sig_flip_up_anti2", ItemTpl.IRONSIGHT_MCX_FLIPUP_REAR_SIGHT);
    public static readonly WeebSight Ramrem = new("6753d006cec7fc449f055459", "sight_rear_all_sig_flip_up_ramrem", ItemTpl.IRONSIGHT_MCX_FLIPUP_REAR_SIGHT);
}