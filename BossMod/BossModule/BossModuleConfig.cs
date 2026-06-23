namespace BossMod;

[ConfigDisplay(Name = "BOSS模块与雷达", Order = 1)]
public class BossModuleConfig : ConfigNode
{
    public enum RadarCloseBehavior
    {
        [PropertyDisplay("打开设置窗口")]
        Prompt,
        [PropertyDisplay("隐藏雷达")]
        DisableRadar,
        [PropertyDisplay("禁用当前模块（并隐藏雷达）")]
        DisableActiveModule,
        [PropertyDisplay("禁用当前模块及同分类下所有模块")]
        DisableActiveModuleCategory
    }

    // 首领模块设置
    [PropertyDisplay("允许模块自动执行技能")]
    public bool AllowAutomaticActions = true;

    [PropertyDisplay("允许模块自动交互物体", since: "0.3.5.6")]
    public bool AllowAutomaticInteract = true;

    [PropertyDisplay("显示测试雷达与提示窗口", tooltip: "无需进入首领战即可配置雷达和提示窗口，非常实用")]
    public bool ShowDemo = false;

    [PropertyDisplay("启用未完成模块", since: "7.5.0.10", tooltip: "未完成模块存在缺陷，可能有严重漏洞，启用风险自负")]
    public bool AllowIncompleteModules = false;

    [PropertyDisplay("在探索模式副本中启用训练假人模块", since: "7.5.0.10")]
    public bool EnableDummyModule = false;

    // radar window settings
    [SectionStart]
    [PropertyDisplay("启用雷达")]
    public bool Enable = true;

    [PropertyDisplay("关闭按钮行为")]
    public RadarCloseBehavior CloseBehavior = RadarCloseBehavior.Prompt;

    [PropertyDisplay("锁定雷达与提示窗口的移动和鼠标交互")]
    public bool Lock = false;

    [PropertyDisplay("雷达窗口背景透明", tooltip: "移除雷达的黑色窗口边框；若将雷达移至其他显示器，该功能无效")]
    public bool TrishaMode = false;

    [PropertyDisplay("为雷达内的战斗区域添加不透明背景")]
    public bool OpaqueArenaBackground = false;

    [PropertyDisplay("显示雷达各类标记的轮廓与阴影")]
    public bool ShowOutlinesAndShadows = false;

    [PropertyDisplay("雷达战斗区域缩放比例", tooltip: "雷达窗口内战斗区域的缩放大小")]
    [PropertySlider(0.1f, 10, Speed = 0.1f, Logarithmic = true)]
    public float ArenaScale = 1;

    [PropertyDisplay("雷达元素线条粗细缩放比例", tooltip: "全局调整雷达元素的轮廓线条粗细")]
    [PropertySlider(0.1f, 10, Speed = 0.1f, Logarithmic = true)]
    public float ThicknessScale = 1;

    [PropertyDisplay("雷达随镜头朝向旋转")]
    public bool RotateArena = true;

    [PropertyDisplay("为旋转预留额外空间", tooltip: "启用上述设置后，为雷达侧边增加空间，避免战斗中旋转镜头时边缘被裁剪")]
    public bool AddSlackForRotations = true;

    [PropertyDisplay("显示雷达战斗区域边框")]
    public bool ShowBorder = true;

    [PropertyDisplay("玩家处于危险时变更区域边框颜色", tooltip: "当你站在易被机制命中的位置时，白色边框会变为红色")]
    public bool ShowBorderRisk = true;

    [PropertyDisplay("在雷达上显示方位名称")]
    public bool ShowCardinals = false;

    [PropertyDisplay("以不同颜色绘制 N 方位", depends: nameof(ShowCardinals), since: "7.5.1.6")]
    public bool HighlightN = false;

    [PropertyDisplay("方位名称字体大小")]
    [PropertySlider(0.1f, 100, Speed = 1)]
    public float CardinalsFontSize = 17;

    [PropertyDisplay("在雷达上显示场地标记")]
    public bool ShowWaymarks = false;

    [PropertyDisplay("在雷达上显示指令标记（攻击、束缚、无视、图形标记）", since: "0.4.10.0")]
    public bool ShowSigns = false;

    [PropertyDisplay("在雷达上显示近战范围提示", since: "7.5.1.2")]
    public bool ShowMeleeRange = false;

    [PropertyDisplay("始终显示所有活着的队友")]
    public bool ShowIrrelevantPlayers = false;

    [PropertyDisplay("允许在雷达上绘制非队友玩家", tooltip: "该选项仅对部分内容生效，如探索任务", depends: nameof(ShowIrrelevantPlayers))]
    public bool ShowAllPlayers = true;

    [PropertyDisplay("雷达中无特殊颜色标记的玩家按职业职责显示颜色")]
    public bool ColorPlayersBasedOnRole = false;

    [PropertyDisplay("始终显示焦点目标队友")]
    public bool ShowFocusTargetPlayer = false;

    // 提示窗口设置

    [SectionStart]
    [PropertyDisplay("在独立窗口显示文字提示", tooltip: "将雷达窗口与提示窗口分离，可单独调整提示窗口位置")]
    public bool HintsInSeparateWindow = false;

    [PropertyDisplay("显示机制序列与计时提示")]
    public bool ShowMechanicTimers = true;

    [PropertyDisplay("显示团队全局提示")]
    public bool ShowGlobalHints = true;

    [PropertyDisplay("显示玩家专属提示与警告")]
    public bool ShowPlayerHints = true;


    // misc. settings
    [SectionStart]
    [PropertyDisplay("在游戏场景中显示移动指引", tooltip:  "该功能使用频率不高，会在场景内显示箭头，提示特定机制下的移动位置")]
    public bool ShowWorldArrows = false;
    public List<string> DisabledModules = [];
    public List<BossModuleInfo.Category> DisabledCategories = [];
}
