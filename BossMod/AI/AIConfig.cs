namespace BossMod.AI
{
    [ConfigDisplay(Name = "自动走路[实验性功能]", Order = 5)]
    class AIConfig : ConfigNode
    {
        [PropertyDisplay("启用AI")]
        public bool Enabled = false;

        [PropertyDisplay("向其他窗口广播按键")]
        public bool BroadcastToSlaves = false;
    }
}
