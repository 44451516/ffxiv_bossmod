namespace BossMod;

[ConfigDisplay(Name = "通用机制解算设置", Order = 7)]
public sealed class AIHintsConfig : ConfigNode
{
    public enum DonutFallbackBehavior
    {
        [PropertyDisplay("视为环形范围伤害（无内侧安全区）")]
        AssumeCircle,
        [PropertyDisplay("完全忽略")]
        Ignore
    }

    [PropertyDisplay("未知环形范围伤害的处理逻辑", since: "0.3.0.0", tooltip: "此设置仅在无首领模块激活时生效。")]
    public DonutFallbackBehavior DonutFallback = DonutFallbackBehavior.AssumeCircle;

    [PropertyDisplay("未知锥形范围伤害的推测角度", since: "0.3.0.0", tooltip: "此设置仅在无首领模块激活时生效。")]
    [PropertySlider(1, 180, Speed = 5)]
    public float ConeFallbackAngle = 180;

    public enum OmenBehavior
    {
        [PropertyDisplay("自动推测（忽略无预警大型环形伤害，此类通常为全团机制）")]
        Automatic,
        [PropertyDisplay("保守自动推测（提示所有技能的机制预警）")]
        AutomaticConservative,
        [PropertyDisplay("仅显示机制预警（忽略其他推测）")]
        OmenOnly
    }

    [PropertyDisplay("无范围指示器技能的处理逻辑", since: "0.3.0.0", tooltip: "此设置仅在无首领模块激活时生效。")]
    public OmenBehavior OmenSetting = OmenBehavior.Automatic;

    [PropertyDisplay("为「辅助型」单位启用通用机制解算", since: "0.3.0.0", tooltip: "此设置仅在无首领模块激活时生效。\n\n「辅助型」单位是指战斗中负责释放大部分首领机制的隐形敌人。请注意，启用此选项后，通用机制解算将支持更多机制类型，但可能会产生非预期结果——因为通用解算无法区分普通范围伤害、凝视攻击、击退效果、施加减益的无伤害施法等机制。请自行斟酌是否启用。")]
    public bool EnableHelperHints = false;
}
