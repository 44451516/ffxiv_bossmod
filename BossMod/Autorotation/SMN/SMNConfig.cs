namespace BossMod;

[ConfigDisplay(Parent = typeof(AutorotationConfig))]
class SMNConfig : ConfigNode
{
    [PropertyDisplay("在Ruin（单体）或Outburst（群体）上执行最优循环\"")]
    public bool FullRotation = true;

    [PropertyDisplay("对友方法术使用鼠标悬停定位")]
    public bool MouseoverFriendly = true;
}
