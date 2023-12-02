namespace BossMod
{
    [ConfigDisplay(Name = "动作调整设置", Order = 4)]
    class ActionManagerConfig : ConfigNode
    {
        // TODO: 考虑将最大延迟暴露给配置；0表示“删除所有延迟”，最大值表示“禁用”
        [PropertyDisplay("从即时施法中删除额外的由滞后引起的动画锁定延迟（类似于xivalex）")]
        public bool RemoveAnimationLockDelay = false;

        [PropertyDisplay("删除额外的由帧率引起的冷却延迟")]
        public bool RemoveCooldownDelay = false;

        [PropertyDisplay("施法时防止移动")]
        public bool PreventMovingWhileCasting = false;

        [PropertyDisplay("使用动作后恢复旋转")]
        public bool RestoreRotation = false;

        public enum GroundTargetingMode
        {
            [PropertyDisplay("通过额外的点击手动选择位置（正常游戏行为）")]
            Manual,

            [PropertyDisplay("在当前鼠标位置施法")]
            AtCursor,

            [PropertyDisplay("在选定目标的位置施法")]
            AtTarget
        }
        [PropertyDisplay("地面目标技能的目标选择")]
        public GroundTargetingMode GTMode = GroundTargetingMode.Manual;
    }
}
