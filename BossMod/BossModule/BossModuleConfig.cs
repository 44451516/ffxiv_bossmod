namespace BossMod;

[ConfigDisplay(Name = "Boss模块和雷达", Order = 1)]
public class BossModuleConfig : ConfigNode
{
    public enum RadarCloseBehavior
    {
        [PropertyDisplay ("打开设置对话框")]
        Prompt,

        [PropertyDisplay ("隐藏雷达")]
        DisableRadar,

        [PropertyDisplay ("禁用当前模块（并隐藏雷达）")]
        DisableActiveModule,

        [PropertyDisplay ("禁用当前模块及同一类别中的所有模块")]
        DisableActiveModuleCategory
    }

    [PropertyDisplay("模块加载的最低成熟度", tooltip: "某些模块将处于‘WIP’状态，除非你更改此设置，否则不会自动加载")]
    public BossModuleInfo.Maturity MinMaturity = BossModuleInfo.Maturity.Contributed;

    [PropertyDisplay ("允许模块自动执行操作")]
    public bool AllowAutomaticActions = true;

    [PropertyDisplay ("允许模块自动与物体交互", since: "0.3.5.6")]
    public bool AllowAutomaticInteract = true;

    [PropertyDisplay ("[设置调整用]显示测试雷达和提示窗口", tooltip: "无需进入首领战斗，即可配置雷达和提示窗口，便于调试", separator: true)]
    public bool ShowDemo = false;

    [PropertyDisplay("启用雷达")]
    public bool Enable = true;

    [PropertyDisplay("关闭按钮行为")]
    public RadarCloseBehavior CloseBehavior = RadarCloseBehavior.Prompt;

    [PropertyDisplay("锁定雷达和提示窗口的移动和鼠标交互")]
    public bool Lock = false;

    [PropertyDisplay("透明的雷达窗口背景", tooltip: "移除雷达周围的黑色窗口；如果将雷达移至其他显示器，此功能将无效")]
    public bool TrishaMode = false;

    [PropertyDisplay("在雷达中为竞技场添加不透明背景")]
    public bool OpaqueArenaBackground = false;

    [PropertyDisplay("在各种雷达标记上显示轮廓和阴影")]
    public bool ShowOutlinesAndShadows = false;

    [PropertyDisplay("雷达竞技场缩放系数", tooltip: "雷达窗口内竞技场的缩放比例")]
    [PropertySlider(0.1f, 10, Speed = 0.1f, Logarithmic = true)]
    public float ArenaScale = 1;

    [PropertyDisplay("雷达元素厚度缩放因子", tooltip: "全局缩放雷达元素的轮廓厚度")]
    [PropertySlider(0.1f, 10, Speed = 0.1f, Logarithmic = true)]
    public float ThicknessScale = 1;

    [PropertyDisplay("旋转雷达以匹配相机方向")]
    public bool RotateArena = true;

    [PropertyDisplay("为旋转提供雷达额外空间", tooltip: "如果你使用了上述设置，此选项为雷达边缘提供额外空间，以避免在战斗时旋转相机时被剪切")]
    public bool AddSlackForRotations = true;

    [PropertyDisplay("在雷达中显示竞技场边框")]
    public bool ShowBorder = true;

    [PropertyDisplay("当玩家处于危险时更改竞技场边框颜色", tooltip: "当你站在可能被机制击中的位置时，将白色边框变为红色")]
    public bool ShowBorderRisk = true;

    [PropertyDisplay("在雷达中显示方位名称")]
    public bool ShowCardinals = false;

    [PropertyDisplay("方位名称字体大小")]
    [PropertySlider(0.1f, 100, Speed = 1)]
    public float CardinalsFontSize = 17;

    [PropertyDisplay("在雷达上显示标记点")]
    public bool ShowWaymarks = false;


    [PropertyDisplay("在雷达上显示标记（'攻击'、'束缚'、'忽略'和形状标记）", since: "0.4.10.0")]
    public bool ShowSigns = false;

    [PropertyDisplay("始终显示所有存活的队伍成员")]

    public bool ShowIrrelevantPlayers = false;

    [PropertyDisplay("允许在雷达上显示非组队玩家", tooltip: "此选项仅影响特定类别的内容，如冒险突袭。", depends: nameof(ShowIrrelevantPlayers))]
    public bool ShowAllPlayers = true;

    [PropertyDisplay("为雷达中无默认颜色的玩家按职责显示颜色")]
    public bool ColorPlayersBasedOnRole = false;

    [PropertyDisplay("始终显示焦点目标的队员", separator: true)]
    public bool ShowFocusTargetPlayer = false;

    [PropertyDisplay("在单独窗口中显示文字提示", tooltip: "将雷达窗口与提示窗口分离，允许你重新定位提示窗口")]
    public bool HintsInSeparateWindow = false;

    [PropertyDisplay("显示机制序列和计时提示")]
    public bool ShowMechanicTimers = true;

    [PropertyDisplay("显示团队范围提示")]
    public bool ShowGlobalHints = true;

    [PropertyDisplay("显示玩家提示和警告", separator: true)]
    public bool ShowPlayerHints = true;

    [PropertyDisplay("在游戏中显示移动提示", tooltip: "使用较少，但可以在游戏中显示箭头，指示在某些机制中移动的位置")]
    public bool ShowWorldArrows = false;

    public List<string> DisabledModules = [];
    public List<BossModuleInfo.Category> DisabledCategories = [];
}
