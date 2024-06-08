namespace BossMod;

[ConfigDisplay(Name = "录像设置", Order = 0)]
public class ReplayManagementConfig : ConfigNode
{
    [PropertyDisplay("显示录像管理界面")]
    public bool ShowUI = false;

    [PropertyDisplay("副本开始自动录像")]
    public bool AutoRecord = false;

    [PropertyDisplay("副本结束自动录像")]
    public bool AutoStop = false;

    [PropertyDisplay("最大录像数量")]
    [PropertySlider(0, 1000)]
    public int MaxReplays = 0;

    [PropertyDisplay("在录像中记录和存储服务器数据包")]
    public bool RecordServerPackets = false;

    // [PropertyDisplay("Dump server packets into dalamud.log")]
    [PropertyDisplay("在录像中存储服务器数据包")]
    public bool DumpServerPackets = false;

    // [PropertyDisplay("Store client packets in the replay")]
    // public bool DumpClientPackets = false;

    [PropertyDisplay("格式化录像 log")]
    public ReplayLogFormat WorldLogFormat = ReplayLogFormat.BinaryCompressed;
}
