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

;
    [PropertyDisplay("未知锥形AOE的猜测角度", since: "0.3.0.0", tooltip: "此设置仅在无模块激活时生效。")]
    public DonutFallbackBehavior DonutFallback = DonutFallbackBehavior.AssumeCircle;

    [PropertyDisplay("Guessed angle for unknown cone AOEs", since: "0.3.0.0", tooltip: "This setting only applies when no module is active.")]
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

    [PropertyDisplay("无AOE指示器动作的处理方式",since: "0.3.0.0", tooltip: "此设置仅在无模块激活时生效。")]
    public OmenBehavior OmenSetting = OmenBehavior.Automatic;

    [PropertyDisplay("Run generic solver for 'Helper'-type actors", since: "0.3.0.0", tooltip: "This setting only applies when no module is active.\n\nHelpers are invisible enemies that are responsible for casting a majority of the mechanics in boss fights. Note that, although this allows the generic solver to support many more mechanics, it may cause unwanted results, since the generic solver cannot distinguish regular AOEs from gaze attacks, knockbacks, non-damaging casts that apply debuffs, and so on. Enable this option at your own discretion.")]
    public bool EnableHelperHints = false;
}
