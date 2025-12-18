namespace BossMod.Endwalker.Savage.P12S1Athena;

[ConfigDisplay(Order = 0x1C1, Parent = typeof(EndwalkerConfig))]
public class P12S1AthenaConfig() : ConfigNode()
{
    public enum EngravementOfSouls1Strategy
    {
        None,

        [PropertyDisplay("支援从N顺时针，在最终位置寻找第一个匹配的人")]
        Default,
    }

    [PropertyDisplay("灵魂刻印1：解决提示")]
    public EngravementOfSouls1Strategy Engravement1Hints = EngravementOfSouls1Strategy.Default;
}
