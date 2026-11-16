namespace BossMod;

[ConfigDisplay(Name = "游戏内叠加显示", Order = -8, Group = "通用")]
public class OverlayConfig : ConfigNode
{
    public enum PixelBehavior
    {
        [PropertyDisplay("禁用")]
        Disabled,
        [PropertyDisplay("战斗中")]
        Combat,
        [PropertyDisplay("副本中")]
        Duty,
        [PropertyDisplay("副本中或战斗中")]
        Either,
        [PropertyDisplay("副本中且战斗中")]
        Both,
    }

    [PropertyDisplay("显示玩家判定点")]
    [PropertyRadio(true)]
    public PixelBehavior ShowPixel;

    [PropertyDisplay("指示器大小", depends: nameof(ShowPixel))]
    [PropertySlider(0, 5)]
    public float PixelSize = 2.5f;

    public enum RingBehavior
    {
        [PropertyDisplay("禁用")]
        Disabled,
        [PropertyDisplay("近战或坦克时")]
        Melee,
        [PropertyDisplay("始终")]
        Everyone
    }

    [PropertyDisplay("显示目标距离指示环")]
    [PropertyRadio(true)]
    public RingBehavior ShowTargetRing;

    public enum AutoRingBehavior
    {
        [PropertyDisplay("禁用")]
        Disabled,
        [PropertyDisplay("坦克时")]
        Tank,
        [PropertyDisplay("始终")]
        Everyone
    }

    [PropertyDisplay("显示敌人自动攻击范围", depends: nameof(ShowTargetRing))]
    [PropertyRadio(true)]
    public AutoRingBehavior ShowAutos = AutoRingBehavior.Tank;

    [PropertyDisplay("高亮当前方位区域", depends: nameof(ShowTargetRing))]
    public bool HighlightSlice = true;

    [PageStart("颜色")]
    [PropertyDisplay("判定点颜色")]
    public Color PixelColor = Color.FromRGBA(0xE200FFFF);

    [PropertyDisplay("默认指示环颜色")]
    public Color FallbackColor = Color.FromRGBA(0xFFFFFF80);

    [PropertyDisplay("指示环颜色：背面")]
    public Color RearColor = Color.FromRGBA(0x35DC8BFF);
    [PropertyDisplay("指示环颜色：侧面")]
    public Color FlankColor = Color.FromRGBA(0xCFC851FF);
    [PropertyDisplay("指示环颜色：正面")]
    public Color FrontColor = Color.FromRGBA(0xE60014FF);

    [PropertyDisplay("身位（即将判定，正确）")]
    public Color PositionalGoodNow = Color.FromRGBA(0x35DC8BFF);
    [PropertyDisplay("身位（即将判定，错误）")]
    public Color PositionalBadNow = Color.FromRGBA(0xE60014FF);
    [PropertyDisplay("身位（后续判定，正确）")]
    public Color PositionalGoodLater = Color.FromRGBA(0xFFFFFFFF);
    [PropertyDisplay("身位（后续判定，错误）")]
    public Color PositionalBadLater = Color.FromRGBA(0xCFC851FF);
}
