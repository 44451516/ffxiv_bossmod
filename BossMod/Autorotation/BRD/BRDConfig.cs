namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class BRDConfig : ConfigNode
    {
     
        [PropertyDisplay("在强力射击（单体）或连珠箭（群体）上执行最优循环")]
        public bool FullRotation = true;

        [PropertyDisplay("智能定位光阴神的礼赞凯歌（如果友方，则目标，否则鼠标悬停，否则选择有可解除减益的随机队伍成员，否则选择自己）")]
        public bool SmartWardensPaeanTarget = true;
    }
}
