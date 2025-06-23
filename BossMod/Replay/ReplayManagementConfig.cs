namespace BossMod;

public record struct ReplayMemory(string Path, bool IsOpen, DateTime PlaybackPosition);

[ConfigDisplay(Name = "录像设置", Order = 0)]
public class ReplayManagementConfig : ConfigNode
{
    [PropertyDisplay("显示录像管理界面")]
    public bool ShowUI = false;
    
    [PropertyDisplay("副本开始自动录像")]
    public bool AutoRecord = true;

    [PropertyDisplay("在职责记录器重放时自动录制", tooltip: "需要开启自动录制")]
    public bool AutoARR = true;

    [PropertyDisplay("删除前保留的最大回放数量")]
    [PropertySlider(0, 1000)]
    public int MaxReplays = 20;

    [PropertyDisplay("在录像中记录和存储服务器数据包")]
    public bool RecordServerPackets = false;

    [PropertyDisplay("在录像中存储服务器数据包")]
    public bool DumpServerPackets = false;

    [PropertyDisplay("导出到dalamud.log时忽略其他玩家的数据包")]
    public bool DumpServerPacketsPlayerOnly = false;

    [PropertyDisplay("将客户端数据包导出到dalamud.log")]
    public bool DumpClientPackets = false;

    [PropertyDisplay("格式化录像日志")]
    public ReplayLogFormat WorldLogFormat = ReplayLogFormat.BinaryCompressed;

    [PropertyDisplay("插件重载时打开之前打开的回放")]
    public bool RememberReplays;

    [PropertyDisplay("记住先前打开回放的播放位置")]
    public bool RememberReplayTimes;

    // TODO: this should not be part of the actual config! figure out where to store transient user preferences...
    public List<ReplayMemory> ReplayHistory = [];
}
