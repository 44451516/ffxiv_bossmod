namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class PLDConfig : ConfigNode
    {
        [PropertyDisplay("在先锋剑上执行最优单体循环，在全蚀斩上执行群体循环")]
        public bool FullRotation = true;

        [PropertyDisplay("在单体连击中执行前置动作（暴乱剑，...）")]
        public bool STCombos = true;

        [PropertyDisplay("在群体连击中执行前置动作（...）")]
        public bool AOECombos = true;

        [PropertyDisplay("智能定位退避（如果友方，则目标，否则鼠标悬停，否则选择副坦克）")]
        public bool SmartShirkTarget = true;

        [PropertyDisplay("如果可用并且是敌对的，对鼠标悬停使用挑衅")]
        public bool ProvokeMouseover = true;
    }
}
