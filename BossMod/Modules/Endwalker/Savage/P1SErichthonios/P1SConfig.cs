namespace BossMod.Endwalker.Savage.P1SErichthonios;

[ConfigDisplay(Order = 0x110, Parent = typeof(EndwalkerConfig))]
public class P1SConfig() : ConfigNode()
{
    public enum Corner { NW, NE, SE, SW }

    [PropertyDisplay("不节制：在不对称模式下与N交换的角落")]
    public Corner IntemperanceAsymmetricalSwapCorner = Corner.NW;
}
