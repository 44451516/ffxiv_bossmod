namespace BossMod.Endwalker.Ultimate.DSW1;

[ConfigDisplay(Order = 0x200, Parent = typeof(EndwalkerConfig))]
public class DSW1Config() : ConfigNode()
{
    public enum HeavensflameHints
    {
        [PropertyDisplay("不显示任何提示")]
        None,

        [PropertyDisplay("匹配标记颜色：圆圈=红色，三角=绿色，十字=蓝色，方块=紫色")]
        Waymarks,

        [PropertyDisplay("LPDU（间）方位：十字=N/S，方块=NE/SW，圆圈=E/W，三角=SE/NW")]
        LPDU,
    }

    [PropertyDisplay("天火解决提示")]
    public HeavensflameHints Heavensflame = HeavensflameHints.None;
}
