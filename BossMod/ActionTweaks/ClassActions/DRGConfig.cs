namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig))]
class DRGConfig : ConfigNode
{
    // TODO: generalize to common utility
    public enum ElusiveJumpBehavior : uint
    {
        [PropertyDisplay("游戏默认（角色相对，向后）")]
        Default = 0,

        [PropertyDisplay("角色相对，向前")]
        CharacterForward = 1,

        [PropertyDisplay("相机相对，向后")]
        CameraBackward = 2,

        [PropertyDisplay("相机相对，向前")]
        CameraForward = 3,
    }

    [PropertyDisplay("后跳方向")]
    public ElusiveJumpBehavior ElusiveJump = ElusiveJumpBehavior.Default;
}
