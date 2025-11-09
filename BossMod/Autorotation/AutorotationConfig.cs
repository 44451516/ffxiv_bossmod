namespace BossMod.Autorotation;

[ConfigDisplay(Name = "自动循环", Order = 5)]
public sealed class AutorotationConfig : ConfigNode
{
    [PropertyDisplay("显示游戏内UI")]
    public bool ShowUI = false;

    public enum DtrStatus
    {
        [PropertyDisplay("禁用")]
        None,

        [PropertyDisplay("仅文本")]
        TextOnly,

        [PropertyDisplay("带图标")]
        Icon
    }


    [PropertyDisplay("在服务器信息栏中显示自动循环预设")]
    public DtrStatus ShowDTR = DtrStatus.None;

    [PropertyDisplay("在服务器信息栏中显示性能统计数据")]
    public bool ShowStatsDTR = false;

    [PropertyDisplay ("隐藏内置预设", tooltip: "如果您已创建自己的预设且不再需要内置的默认预设，此选项将阻止它们在自动旋转和预设编辑器窗口中显示。", since: "0.0.0.253")]
    public bool HideDefaultPreset = false;

    [PropertyDisplay("在游戏中显示位置提示", tooltip: "显示位置技能提示，指示移动到目标的侧面或后方")]
    public bool SuggestHealerAI = true;

    [PropertyDisplay("显示朝向提示", tooltip: "显示技能朝向相关提示，指示你移动至目标的侧面或背面")]
    public bool ShowPositionals = false;

    [PropertyDisplay("退出战斗时自动重新启用被强制禁用的自动循环")]
    public bool ClearPresetOnCombatEnd = false;

    [PropertyDisplay ("死亡时自动禁用自动循环", since: "0.4.4.1")]
    public bool ClearPresetOnDeath = true;

    [PropertyDisplay ("若引诱陷阱触发则自动禁用自动循环", tooltip: "仅适用于深层地下城", since: "0.4.4.1")]
    public bool ClearPresetOnLuring = false;

    [PropertyDisplay ("脱离战斗时自动重新启用被强制禁用的自动循环")]
    public bool ClearForceDisableOnCombatEnd = true;

    [PropertyDisplay("提前开怪阈值", tooltip: "如果有人在倒计时超过此值时进入boss战斗，则视为抢怪，自动循环将被强制禁用")]
    [PropertySlider(0, 30, Speed = 1)]
    public float EarlyPullThreshold = 1.5f;
}
