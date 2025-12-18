namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig))]
class PLDConfig : ConfigNode
{
    [PropertyDisplay("防止在开战前过早使用'圣灵'")]
    public bool ForbidEarlyHolySpirit = true;

    [PropertyDisplay("防止在开战前过早使用'投盾'（如果未解锁圣灵）")]
    public bool ForbidEarlyShieldLob = true;

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
