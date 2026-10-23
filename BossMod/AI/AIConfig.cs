namespace BossMod.AI;

[ConfigDisplay(Name = "AI配置", Order = 9, Group = "AI与自动化")]
public sealed class AIConfig : ConfigNode
{
    public enum Slot
    {
        One,
        Two,
        Three,
        Four
    }

    // UI设置
    [PropertyDisplay("启用AI", tooltip: "免责声明：AI处于非常实验性的阶段，请自行承担风险！")]
    public bool Enabled = false;

    [PropertyDisplay("显示游戏内UI")]
    public bool DrawUI = false;

    [PropertyDisplay("目标设置为焦点")]
    public bool FocusTargetMaster = false;

    [PropertyDisplay("与主控的距离")]
    [PropertySlider(3, 25)]
    public float DistanceToMaster = 25f;

    [PropertyDisplay("即使首领模块处于活动状态也跟随主控")]
    public bool FollowDuringBoss = false;

    [PropertyDisplay("覆盖跟随距离")]
    public bool OverrideRange = false;

    [PropertyDisplay("跟随槽位距离")]
    public float MaxDistanceToSlot = 1;

    [PropertyDisplay("跟随目标")]
    public bool FollowTarget = false;

    [PropertyDisplay("跟随目标距离")]
    public float MaxDistanceToTarget = 2.6f;

    [PropertyDisplay("在活跃的boss模块期间跟随")]
    public bool FollowDuringActiveBossModule = false;

    [PropertyDisplay("在战斗中跟随")]
    public bool FollowDuringCombat = false;

    [PropertyDisplay("在非战斗状态下跟随")]
    public bool FollowOutOfCombat = false;

    [PropertyDisplay("禁止移动")]
    public bool ForbidMovement = false;

    [PropertyDisplay("禁止动作")]
    public bool ForbidActions = false;

    [PropertyDisplay("自动攻击FATE怪物", since: "0.0.0.253")]
    public bool AutoFate = true;

    [PropertyDisplay("将按键广播到其他窗口")]
    public bool BroadcastToSlaves = false;

    [PropertyDisplay("朝当前目标移动", since: "7.5.1.21")]
    public bool GoalZoneFallback = true;

    [PropertyDisplay("AI 状态变化时在聊天栏输出提示", since: "7.5.1.36")]
    public bool ChatConfirmation = false;

    // defined here for DtrConfig migration
    public bool ShowDTR = true;
}
