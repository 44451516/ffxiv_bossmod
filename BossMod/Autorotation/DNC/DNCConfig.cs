namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class DNCConfig : ConfigNode
    {
        // 在使用Cascade（单体）或Windmill（AOE）时执行最佳循环
        [PropertyDisplay("在使用Cascade（单体）或Windmill（AOE）时执行最佳循环")]
        public bool FullRotation = true;

        // 即兴表演期间暂停自动旋转
        [PropertyDisplay("即兴表演期间暂停自动旋转")]
        public bool PauseDuringImprov = false;

        // 自动选择舞伴
        [PropertyDisplay("自动选择舞伴")]
        public bool AutoPartner = true;
    }
}
