namespace BossMod.Stormblood.Ultimate.UCOB;

[ConfigDisplay(Order = 0x200, Parent = typeof(StormbloodConfig))]
public class UCOBConfig() : ConfigNode()
{
    [PropertyDisplay("P3 快步/天降三重奏：安全点分配（假设巴哈姆特相对北方/上方，L组向左；L1/R1最接近首领）")]
    [GroupDetails(["L1", "L2", "L3", "L4", "R1", "R2", "R3", "R4"])]
    [GroupPreset("Hector: THMR", [0, 4, 1, 5, 2, 6, 3, 7])]
    [GroupPreset("LPDU: HTTH/RMMR", [1, 2, 0, 3, 5, 6, 4, 7])]
    public GroupAssignmentUnique P3QuickmarchTrioAssignments = new() { Assignments = [0, 4, 1, 5, 2, 6, 3, 7] };

    [PropertyDisplay("P3 天降三重奏：塔优先级，从奈尔开始顺时针")]
    [GroupDetails(["0", "1", "2", "3", "4", "5", "6", "7"])]
    [GroupPreset("Hector: THMR, G1 CCW, G2 CW", [7, 0, 6, 1, 5, 2, 4, 3])]
    public GroupAssignmentUnique P3HeavensfallTrioTowers = new() { Assignments = [7, 0, 6, 1, 5, 2, 4, 3] };
}
