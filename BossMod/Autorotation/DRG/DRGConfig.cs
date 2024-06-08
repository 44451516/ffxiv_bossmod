namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class DRGConfig : ConfigNode
    {
        [PropertyDisplay("在True Thrust（单体）或Doom Spike（群体）上执行最优循环")]
        public bool FullRotation = true;

        [PropertyDisplay("智能定位Dragon Sight（如果友方，则目标，否则鼠标悬停，否则选择按职业排名最高的玩家）")]
        public bool SmartDragonSightTarget = true;


        // TODO: generalize to common utility
        public enum ElusiveJumpBehavior : uint
        {
            [PropertyDisplay("Game default (character-relative, backwards)")]
            Default = 0,

            [PropertyDisplay("Character-relative, forwards")]
            CharacterForward = 1,

            [PropertyDisplay("Camera-relative, backwards")]
            CameraBackward = 2,

            [PropertyDisplay("Camera-relative, forwards")]
            CameraForward = 3,
        }

        [PropertyDisplay("Elusive Jump direction")]
        public ElusiveJumpBehavior ElusiveJump = ElusiveJumpBehavior.Default;
    }

}
