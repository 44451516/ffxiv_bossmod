namespace BossMod;

[ConfigDisplay(Parent = typeof(AutorotationConfig))]
class WHMConfig : ConfigNode
{
    [PropertyDisplay("在Glare（单体伤害）、Holy（群体伤害）、Cure1（单体治疗）和Medica1（群体治疗）上执行最优循环")]
    public bool FullRotation = true;

    [PropertyDisplay("尝试施放复活时，如果可能的话自动应用迅速咏唱和薄空")]
    public bool SwiftFreeRaise = true;

    [PropertyDisplay("对友方法术使用鼠标悬停定位")]
    public bool MouseoverFriendly = true;

    [PropertyDisplay("如果友方，则目标/鼠标悬停，否则选择附近受伤玩家最多的队伍成员")]
    public bool SmartCure3Target = true;

    [PropertyDisplay("永远不要超过血百合上限：如果需要，使用Misery而不是Solace/Rapture")]
    public bool NeverOvercapBloodLilies = false;
}
