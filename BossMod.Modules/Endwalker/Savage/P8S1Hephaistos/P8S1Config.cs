namespace BossMod.Endwalker.Savage.P8S1Hephaistos;

[ConfigDisplay(Order = 0x181, Parent = typeof(EndwalkerConfig))]
public class P8S1Config() : ConfigNode()
{
    [PropertyDisplay("蛇1：分配")]
    [GroupDetails(["Prio 1 (always CW from N)", "Prio 2 (flex CW from N)", "Prio 3 (flex CCW from NW)", "Prio 4 (always CCW from NW)"])]
    [GroupPreset("PF", [0, 1, 2, 3, 0, 1, 2, 3])]
    [GroupPreset("Hector", [2, 1, 3, 0, 2, 1, 3, 0])]
    public GroupAssignmentDDSupportPairs Snake1Assignments = GroupAssignmentDDSupportPairs.DefaultMeleeTogether();

    [PropertyDisplay("蛇2：分配")]
    [GroupDetails(["G1 (NW/CCW) flex", "G2 (SE/CW) flex", "G1 (NW/CCW) fixed", "G2 (SE/CW) fixed"])]
    [GroupPreset("PF/Hector", [0, 1, 2, 3, 0, 1, 2, 3])]
    public GroupAssignmentDDSupportPairs Snake2Assignments = GroupAssignmentDDSupportPairs.DefaultMeleeTogether();

    [PropertyDisplay("蛇2：使用方位优先级（G1 N/W，PF策略）而不是排序（G1从NW第一个安全CCW，Hector策略）")]
    public bool Snake2CardinalPriorities = true;
}
