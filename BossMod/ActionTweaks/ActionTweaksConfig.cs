namespace BossMod;

[ConfigDisplay(Name = "技能调整", Order = 4)]
public sealed class ActionTweaksConfig : ConfigNode
{
    // TODO: 考虑将最大延迟暴露给配置；0表示'移除所有延迟'，最大值表示'禁用'
    [PropertyDisplay("移除瞬发技能因延迟引起的额外动画锁定延迟（请阅读提示！）", tooltip: "请不要与XivAlexander或NoClippy一起使用——如果检测到这些工具，它应会自动禁用，但请务必先检查！")]
    public bool RemoveAnimationLockDelay = false;

    [PropertyDisplay("动画锁定最大模拟延迟（请阅读工具提示！）", tooltip: "配置使用动画锁定移除时的最大模拟延迟（以毫秒为单位）——这是必需的，且不能减少到零。将此设置为20毫秒时，在使用自动旋转时将启用三重编织。移除三重编织的最小设置为26毫秒。20毫秒的最小值已被FFLogs接受，不应对你的日志造成问题。")]
    [PropertySlider(20, 50, Speed = 0.1f)]
    public int AnimationLockDelayMax = 26;

    [PropertyDisplay("移除因帧率引起的额外冷却延迟", tooltip: "动态调整冷却和动画锁定，以确保无论帧率限制如何，队列中的动作都能立即执行")]
    public bool RemoveCooldownDelay = false;

    [PropertyDisplay("防止施法时移动", tags: ["slidecast"])]
    public bool PreventMovingWhileCasting = false;

    public enum ModifierKey
    {
        [PropertyDisplay("无")]
        None,

        [PropertyDisplay("Ctrl")]
        Ctrl,

        [PropertyDisplay("Alt")]
        Alt,

        [PropertyDisplay("Shift")]
        Shift,

        [PropertyDisplay("鼠标左键+右键")]
        M12
    }

    [PropertyDisplay("按住此键可在施法时允许移动", depends: nameof(PreventMovingWhileCasting), tags: ["slidecast"])]
    public ModifierKey MoveEscapeHatch = ModifierKey.None;

    [PropertyDisplay("当目标死亡时自动取消施法")]
    public bool CancelCastOnDeadTarget = false;

    [PropertyDisplay("当类似热病机制即将来临时禁止移动和执行动作（设置为0禁用，否则根据延迟增加阈值）。", tooltip: "设置为0可禁用此功能，否则请根据您的网络延迟增加阈值。")]
    [PropertySlider(0, 10, Speed = 0.01f)]
    public float PyreticThreshold = 1.0f;

    [PropertyDisplay("自动错误定向：如果正常运动和错误定向之间的角度大于此阈值，则防止在错误定向下运动（设置为 180 可关闭此功能）。")]
    [PropertySlider(0, 180)]
    public float MisdirectionThreshold = 180;


    [PropertyDisplay("对鼠标悬停的目标使用技能")]
    public bool PreferMouseover = false;

    [PropertyDisplay("智能技能目标选择", tooltip: "如果通常的目标（鼠标悬停/主要目标）不适合使用某个技能，则自动选择下一个最佳目标（例如为副坦使用Shirk）")]
    public bool SmartTargets = true;

    [PropertyDisplay("为手动按下的技能使用自定义队列", tooltip: "此设置可以更好地与自动循环结合，并防止在自动循环过程中按下治疗技能时出现三次编织或GCD漂移的情况")]
    public bool UseManualQueue = false;

    [PropertyDisplay ("尝试防止冲入范围性技能区域",tooltip:"如果定向冲刺（如战士的[猛攻 ]）会将你带入危险区域，则阻止其自动使用。在没有模块的副本中，此功能可能无法按预期工作。\n\n 如果启用了 ' 对手动按下的动作使用自定义队列 '，此选项也将适用于手动按下的冲刺。",since:"0.0.0.290")]
    public bool DashSafety = true;

    [PropertyDisplay("将上一选项应用于所有冲刺，而非仅突进技能", tooltip: "包括后退冲刺（如武士的'燕返'）、传送（如忍者的'残影'）和固定距离冲刺（如龙骑士的'回避跳跃'）", depends: nameof(DashSafety))]
    public bool DashSafetyExtra = true;

    [PropertyDisplay("自动管理自动攻击", tooltip: "此设置可防止在倒计时期间过早开始自动攻击，在开怪时、切换目标时以及使用任何不会明确取消自动攻击的技能时自动启动自动攻击。")]
    public bool AutoAutos = false;

    [PropertyDisplay("自动下坐骑以执行技能")]
    public bool AutoDismount = true;

    public enum GroundTargetingMode
    {
        [PropertyDisplay("通过额外点击手动选择位置（正常游戏行为）")]
        Manual,

        [PropertyDisplay("在当前鼠标位置施放")]
        AtCursor,

        [PropertyDisplay("在选定目标的位置施放")]
        AtTarget
    }

    [PropertyDisplay("地面目标技能的自动目标选择")]
    public GroundTargetingMode GTMode = GroundTargetingMode.Manual;
}
