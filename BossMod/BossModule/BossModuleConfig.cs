namespace BossMod
{
    [ConfigDisplay(Name = "画图", Order = 1)]
    public class BossModuleConfig : ConfigNode
    {
        [PropertyDisplay("Arena scale factor")]
        [PropertySlider(0.1f, 10, Speed = 0.1f, Logarithmic = true)]
        public float ArenaScale = 1;

        [PropertyDisplay("启动 boss modules")]
        public bool Enable = true;

        // [PropertyDisplay("Lock movement and mouse interaction")]
        [PropertyDisplay("锁定禁止移动和鼠标交互")]
        public bool Lock = false;

        // [PropertyDisplay("Rotate map to match camera orientation")]
        [PropertyDisplay("地图跟随镜头移动")]
        public bool RotateArena = true;

        // [PropertyDisplay("Show arena border")]
        [PropertyDisplay("显示场地边界")]
        public bool ShowBorder = true;

        // [PropertyDisplay("Change arena border color if player is at risk")]
        [PropertyDisplay("玩家处于危险中，更改边界颜色为红色")]
        public bool ShowBorderRisk = true;

        // [PropertyDisplay("Show cardinal direction names")]
        [PropertyDisplay("显示东南西北")]
        public bool ShowCardinals = true;

        // [PropertyDisplay("Show waymarks on radar")]
        [PropertyDisplay("显示标点")]
        public bool ShowWaymarks = true;

        [PropertyDisplay("Show mechanics sequence and timers")]
        public bool ShowMechanicTimers = true;

        // [PropertyDisplay("Show raidwide hints")]
        [PropertyDisplay("显示副本提示[英文的]")]
        public bool ShowGlobalHints = true;

        // [PropertyDisplay("Show warnings and hints for player")]
        [PropertyDisplay("显示警告和提示[英文的]")]
        public bool ShowPlayerHints = true;

        [PropertyDisplay("Trisha mode: show radar without window")]
        public bool TrishaMode = false;

        // [PropertyDisplay("Add opaque background to the arena")]
        [PropertyDisplay("不透明背景")]
        public bool OpaqueArenaBackground = false;

        // [PropertyDisplay("Show movement hints in world")]
        [PropertyDisplay("显示运动提示")]
        public bool ShowWorldArrows = false;

        // [PropertyDisplay("Show boss module demo out of instances (useful for configuring windows)")]
        [PropertyDisplay("调试模式")]
        public bool ShowDemo = false;

        [PropertyDisplay("Show window with cooldown plan timers")]
        public bool EnableTimerWindow = false;

        // [PropertyDisplay("Always show all alive party members")]
        [PropertyDisplay("始终显示所有活着的队友")]
        public bool ShowIrrelevantPlayers = false;
    }
}
