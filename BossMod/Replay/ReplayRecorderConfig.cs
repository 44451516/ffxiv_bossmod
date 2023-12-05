using Newtonsoft.Json;
using System.IO;

namespace BossMod
{
    [ConfigDisplay(Name = "录像设置", Order = 0)]
    public class ReplayRecorderConfig : ConfigNode
    {
        public enum LogFormat
        {
            [PropertyDisplay("压缩二进制")]
            BinaryCompressed,

            [PropertyDisplay("原始二进制")]
            BinaryUncompressed,

            [PropertyDisplay("精简文本")]
            TextCondensed,

            [PropertyDisplay("详细文本")]
            TextVerbose,
        }

        [PropertyDisplay("显示录像管理界面")]
        public bool ShowUI = false;

        [PropertyDisplay("在录像中存储服务器数据包")]
        public bool DumpServerPackets = false;

        [PropertyDisplay("在录像中存储客户器数据包")]
        public bool DumpClientPackets = false;

        [PropertyDisplay("Log format")]
        public LogFormat WorldLogFormat = LogFormat.BinaryCompressed;

        [JsonIgnore]
        public DirectoryInfo? TargetDirectory;

        [JsonIgnore]
        public string LogPrefix = "World";
    }
}
