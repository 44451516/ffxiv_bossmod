namespace BossMod
{
    [ConfigDisplay(Name = "自动循环设置 (实验性!)", Order = 4)]
    class AutorotationConfig : ConfigNode
    {
        [PropertyDisplay("启用自动循环")]
        public bool Enabled = false;

        [PropertyDisplay("记录信息")]
        public bool Logging = false;

        [PropertyDisplay("显示游戏内GUI")]
        public bool ShowUI = false;

        [PropertyDisplay("启用能影响玩家位置的技能 (例如 猛攻, 蛮荒崩裂)")]
        public bool EnableMovement = true;

        [PropertyDisplay("智能冷却排序 (当按下按钮后, 将其放到下一个OGCD槽位而不影响GCD)")]
        public bool SmartCooldownQueueing = true;

        [PropertyDisplay("咏唱时防止移动")]
        public bool PreventMovingWhileCasting = false;

        [PropertyDisplay("移除瞬发技能所带来的动画锁定 (a-la xivalex)")]
        public bool RemoveAnimationLockDelay = false;

        [PropertyDisplay("药水使用策略")]
        public CommonRotation.Strategy.PotionUse PotionUse = CommonRotation.Strategy.PotionUse.Manual;

        public enum GroundTargetingMode
        {
            [PropertyDisplay("通过额外的点击手动选择位置 (正常游戏行为)")]
            Manual,

            [PropertyDisplay("在当前鼠标位置使用")]
            AtCursor,

            [PropertyDisplay("在选中的目标使用")]
            AtTarget
        }
        [PropertyDisplay("地面目标模式的目标选择")]
        public GroundTargetingMode GTMode = GroundTargetingMode.AtCursor;
    }
}