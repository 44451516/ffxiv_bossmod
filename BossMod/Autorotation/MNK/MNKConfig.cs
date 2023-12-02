namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class MNKConfig : ConfigNode
    {
        [PropertyDisplay("在Bootshine（单体）或Arm of the Destroyer（群体）上执行最优循环")]
        public bool FullRotation = true;

        [PropertyDisplay("在Four-point Fury上执行特定形态的群体GCD")]
        public bool AOECombos = true;
    }
}
