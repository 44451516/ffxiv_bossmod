namespace BossMod.Endwalker.Savage.P12S2PallasAthena;

[ConfigDisplay(Order = 0x1C2, Parent = typeof(EndwalkerConfig))]
public class P12S2PallasAthenaConfig() : ConfigNode()
{
    [PropertyDisplay("泛生：塔分配策略")]
    [PropertyCombo("2+0: 第一塔由短色和0不稳定吸收；然后都去北方第二塔", "2+1: 第一塔由短色和1不稳定吸收；然后都去不同的第二塔")]
    public bool PangenesisFirstStack = true;
}
