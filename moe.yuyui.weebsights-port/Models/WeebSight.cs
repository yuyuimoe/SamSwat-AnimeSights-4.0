using SPTarkov.Server.Core.Models.Common;

namespace moe.yuyui.weebsights_port.Models;

public record WeebSight(MongoId Id, string Asset, MongoId? CloneFromTpl);
