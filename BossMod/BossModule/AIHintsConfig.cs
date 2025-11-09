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
}
