namespace BossMod.Endwalker.Savage.P8S2;

[ConfigDisplay(Order = 0x182, Parent = typeof(EndwalkerConfig))]
public class P8S2Config() : ConfigNode()
{
    [PropertyDisplay("无限荒芜：坦克/治疗使用右侧")]
    public bool LimitlessDesolationTHRight = false;

    [PropertyDisplay("高概念1：长debuff占据S塔")]
    public bool HC1LongGoS = true;
}
