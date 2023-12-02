namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class WARConfig : ConfigNode
    {
        [PropertyDisplay("在重劈上执行最优单体循环，在超压斧上执行群体循环")]
        public bool FullRotation = true;

        [PropertyDisplay("在单体连击中执行前置动作（凶残裂, 暴风碎, 暴风斩）")]
        public bool STCombos = true;

        [PropertyDisplay("在群体连击中执行前置动作（秘银暴风）")]
        public bool AOECombos = true;

        [PropertyDisplay("智能定位退避和原初的勇猛（如果友方，则目标，否则鼠标悬停，否则选择副坦克）")]
        public bool SmartNascentFlashShirkTarget = true;

        [PropertyDisplay("如果可用并且是敌对的，对鼠标悬停使用挑衅")]
        public bool ProvokeMouseover = true;

        [PropertyDisplay("将自己作为死斗的目标")]
        public bool HolmgangSelf = true;

        [PropertyDisplay("禁止在准备阶段过早使用飞斧")]
        public bool ForbidEarlyTomahawk = true;

        [PropertyDisplay("为猛攻提供额外的时间缓冲（防止三重编织，防止轻微GCD延迟）")]
        public bool OnslaughtHeadroom = true;
    }
}
