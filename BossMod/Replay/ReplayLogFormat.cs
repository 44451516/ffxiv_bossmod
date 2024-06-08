using BossMod;

namespace BossMod
{
    public enum ReplayLogFormat
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
}

public static class ReplayLogFormatMagic
{
    public static readonly FourCC CompressedBinary = new("BLCB"u8); // bossmod log compressed brotli
    public static readonly FourCC RawBinary = new("BLOG"u8); // bossmod log
}
