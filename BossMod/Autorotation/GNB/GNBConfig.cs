namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class GNBConfig : ConfigNode
    {
        // 在使用Keen Edge（单体）或Demon Slice（AOE）时执行最佳循环
        [PropertyDisplay("在使用Keen Edge（单体）或Demon Slice（AOE）时执行最佳循环")]
        public bool FullRotation = true;

        // 自动执行单体连击的前续动作
        [PropertyDisplay("自动执行单体连击的前续动作")]
        public bool STCombos = true;

        // 自动执行AOE连击的前续动作
        [PropertyDisplay("自动执行AOE连击的前续动作")]
        public bool AOECombos = true;

        // 智能目标选择，用于Shirk和心脏义务（优先目标，如果友好，则选鼠标悬停目标，如果友好，否则选择副坦克）
        [PropertyDisplay("智能目标选择，用于Shirk和心脏义务（优先目标，如果友好，则选鼠标悬停目标，如果友好，否则选择副坦克）")]
        public bool SmartHeartofCorundumShirkTarget = true;

        // 在鼠标悬停目标上使用挑衅（如果可用且敌对）
        [PropertyDisplay("在鼠标悬停目标上使用挑衅（如果可用且敌对）")]
        public bool ProvokeMouseover = true;

        // 禁止在预拉期间过早使用“闪电弹”
        [PropertyDisplay("禁止在预拉期间过早使用“闪电弹”")]
        public bool ForbidEarlyLightningShot = true;

        // 在“无情”期间使用两个“粗暴分裂”技能
        [PropertyDisplay("在“无情”期间使用两个“粗暴分裂”技能")]
        public bool NoMercyRoughDivide = true;

        // <= 2.47技能速度旋转
        [PropertyDisplay("<= 2.47技能速度旋转")]
        public bool Skscheck = true;

        // 在开局中提前使用“无情”（注意：这将破坏30-53级的循环）
        [PropertyDisplay("在开局中提前使用“无情”（注意：这将破坏30-53级的循环）")]
        public bool EarlyNoMercy = true;

        // 在开局中提前使用“音速破坏”（注意：这将破坏30-53级的循环）
        [PropertyDisplay("在开局中提前使用“音速破坏”（注意：这将破坏30-53级的循环）")]
        public bool EarlySonicBreak = true;
    }
}
