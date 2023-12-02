namespace BossMod
{
    [ConfigDisplay(Name = "录像设置", Order = 0)]
    public class ReplayManagementConfig : ConfigNode
    {
        [PropertyDisplay("显示录像管理界面")]
        public bool ShowUI = false;

        [PropertyDisplay("在录像中存储服务器数据包")]
        public bool DumpServerPackets = false;

        [PropertyDisplay("在录像中存储客户器数据包")]
        public bool DumpClientPackets = false;

        [PropertyDisplay("格式化录像 logs")]
        public ReplayLogFormat WorldLogFormat = ReplayLogFormat.BinaryCompressed;
    }
}
