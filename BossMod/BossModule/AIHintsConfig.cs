namespace BossMod;

[ConfigDisplay(Name = "通用解算器设置", Order = 7)]
public sealed class AIHintsConfig : ConfigNode
{
    public enum DonutFallbackBehavior
    {
        [PropertyDisplay("假设AOE为圆形，无内部安全区")]
        AssumeCircle,
        [PropertyDisplay("完全忽略")]
        Ignore
    }

    [PropertyDisplay("未知环形AOE的处理方式", tooltip: "此设置仅在无模块激活时生效。")]
    public DonutFallbackBehavior DonutFallback = DonutFallbackBehavior.AssumeCircle;

    [PropertyDisplay("未知锥形AOE的猜测角度", tooltip: "此设置仅在无模块激活时生效。")]
    [PropertySlider(1, 180, Speed = 5)]
    public float ConeFallbackAngle = 180;

    public enum OmenBehavior
    {
        [PropertyDisplay("最佳推测；忽略无预警的大型圆形技能（通常为全屏攻击）")]
        Automatic,
        [PropertyDisplay("最佳推测；提示所有动作")]
        AutomaticConservative,
        [PropertyDisplay("完全忽略")]
        OmenOnly
    }

    [PropertyDisplay("无AOE指示器动作的处理方式", tooltip: "此设置仅在无模块激活时生效。")]
    public OmenBehavior OmenSetting = OmenBehavior.Automatic;
}