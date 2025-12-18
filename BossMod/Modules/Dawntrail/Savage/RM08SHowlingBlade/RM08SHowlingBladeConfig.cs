namespace BossMod.Dawntrail.Savage.RM08SHowlingBlade;

[ConfigDisplay(Order = 0x300, Parent = typeof(DawntrailConfig))]
public class RM08SHowlingBladeConfig : ConfigNode
{
    public enum ReignStrategy
    {
        [PropertyDisplay("显示当前职责的两个安全点")]
        Any,
        [PropertyDisplay("假设从竞技场中心看向首领时G1左，G2右")]
        Standard,
        [PropertyDisplay("假设从竞技场中心看向首领时G1右，G2左")]
        Inverse,
        [PropertyDisplay("无")]
        Disabled
    }

    [PropertyDisplay("革命统治位置提示")]
    public ReignStrategy ReignHints = ReignStrategy.Standard;

    public enum TerrestrialRageStrategy
    {
        [PropertyDisplay("无提示")]
        None,
        [PropertyDisplay("时钟 - 集合标记去N/NE安全点，分散调整")]
        Clock,
        // Toxic, DN TODO
    }

    [PropertyDisplay("大地之怒")]
    public TerrestrialRageStrategy TRHints = TerrestrialRageStrategy.None;

    [PropertyDisplay("召唤月光：在小地图上高亮第一和第三个安全象限（即\"象限月光\"）")]
    public bool QuadMoonlightHints = false;

    public enum LamentTowerStrategy
    {
        [PropertyDisplay("无塔提示")]
        None,
        [PropertyDisplay("Rinon - 治疗S，长坦克SW，长坦克伙伴SE，长治疗伙伴N，短坦克和伙伴N")]
        Rinon,
    }

    [PropertyDisplay("孤狼的哀叹")]
    public LamentTowerStrategy TowerHints = LamentTowerStrategy.Rinon;

    [PropertyDisplay("风牙/石牙时钟位置", tooltip: "仅由AI使用")]
    [GroupDetails(["N", "NE", "E", "SE", "S", "SW", "W", "NW"])]
    [GroupPreset("Default", [0, 4, 6, 2, 5, 3, 7, 1])]
    public GroupAssignmentUnique WindfangStonefangSpots = new() { Assignments = [0, 4, 6, 2, 5, 3, 7, 1] };
}
