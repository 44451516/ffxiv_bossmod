namespace BossMod.Endwalker.Savage.P4S2Hesperos;

[ConfigDisplay(Order = 0x142, Parent = typeof(EndwalkerConfig))]
public class P4S2Config() : ConfigNode()
{
    [PropertyDisplay("第四幕：逆时针1/8去吸收带有暗debuff的塔")]
    public bool Act4DarkSoakCCW = false;

    [PropertyDisplay("第四幕：逆时针3/8去打破水连线")]
    public bool Act4WaterBreakCCW = false;

    [PropertyDisplay("谢幕：DD先打破debuff")]
    public bool CurtainCallDDFirst = false;
}
