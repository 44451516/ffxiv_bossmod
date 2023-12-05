namespace BossMod
{
    // [ConfigDisplay(Name = "[调试用的]General settings", Order = 0)]
    [ConfigDisplay(Name = "本插件免费[20231205_CN6.45]", Order = 0)]
    public class GeneralConfig : ConfigNode
    {
        [PropertyDisplay("开发者用的")]
        public bool DumpWorldStateEvents = false;

        [PropertyDisplay("开发者用的")]
        public bool DumpServerPackets = false;

        [PropertyDisplay("开发者用的")]
        public bool DumpClientPackets = false;
    }
}
