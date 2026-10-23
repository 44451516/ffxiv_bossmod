namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig), Group = "职业专属")]
class PLDConfig : ConfigNode
{
    public enum WingsBehavior : uint
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

    [PropertyDisplay("武装戍卫方向")]
    public WingsBehavior Wings = WingsBehavior.Default;
}
