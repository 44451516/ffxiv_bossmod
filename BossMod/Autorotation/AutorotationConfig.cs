namespace BossMod
{
    [ConfigDisplay(Name = "自动循环设置 (实验性!)", Order = 5)]
    class AutorotationConfig : ConfigNode
    {
        
        [PropertyDisplay("关闭HOOK[取消需要重启游戏]")]
        public bool ActionManagerExHookEnabled = true;
        
        [PropertyDisplay("启用自动循环")]
        public bool Enabled = false;

        [PropertyDisplay("记录信息")]
        public bool Logging = false;

        [PropertyDisplay("显示游戏内GUI")]
        public bool ShowUI = false;

        [PropertyDisplay("在世界内显示移动指示")]
        public bool ShowPositionals = false;

        [PropertyDisplay("启用能影响玩家位置的技能 (例如 猛攻, 蛮荒崩裂)")]
        public bool EnableMovement = true;

        [PropertyDisplay("Sticky auto actions")]
        public bool StickyAutoActions = false;
    }
}