namespace BossMod;

[ConfigDisplay(Name = "任务 / 副本全自动模式", Order = 6)]
public sealed class ZoneModuleConfig : ConfigNode
{
    [PropertyDisplay("加载区域模块所需的成熟度")]
    public BossModuleInfo.Maturity MinMaturity = BossModuleInfo.Maturity.Contributed;

    [PropertyDisplay("启用任务战斗 / 单人任务的自动执行")]
    public bool EnableQuestBattles = false;

    [PropertyDisplay("在游戏世界中绘制导航点")]
    public bool ShowWaypoints = false;

    [PropertyDisplay("导航时使用冲刺技能（速涂、回避跳跃、ETC等）")]
    public bool UseDash = true;
}
