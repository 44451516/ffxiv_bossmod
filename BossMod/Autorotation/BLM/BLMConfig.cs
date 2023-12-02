namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class BLMConfig : ConfigNode
    {
        [PropertyDisplay("在Blizzard1（单体）或Blizzard2（群体）上执行最优旋转")]
        public bool FullRotation = true;

        [PropertyDisplay("对友方法术使用鼠标悬停目标")]
        public bool MouseoverFriendly = true;
    }
}
