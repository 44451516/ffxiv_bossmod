namespace BossMod.Endwalker.Ultimate.TOP;

[ConfigDisplay(Order = 0x210, Parent = typeof(EndwalkerConfig))]
public class TOPConfig() : ConfigNode()
{
    [PropertyDisplay("P1 程序循环：分配（G1 从 NW 顺时针，G2 默认逆时针，冲突时'较低'编号灵活调整）")]
    [GroupDetails(["G1 prio1", "G1 prio2", "G1 prio3", "G1 prio4", "G2 prio1", "G2 prio2", "G2 prio3", "G2 prio4"])]
    [GroupPreset("LPDU (global): M1>M2>MT>OT>R1>R2>H1>H2", [5, 4, 1, 0, 7, 6, 3, 2])]
    [GroupPreset("NA (snake prio): TMRH, group 2 north", [4, 0, 7, 3, 5, 1, 6, 2])]
    public GroupAssignmentUnique P1ProgramLoopAssignments = new() { Assignments = [5, 4, 1, 0, 7, 6, 3, 2] };

    [PropertyDisplay("P1 程序循环：使用全局优先级 - 将 G1 视为比 G2 编号更低（因此 G1 更可能灵活调整）")]
    public bool P1ProgramLoopGlobalPriority = true;

    [PropertyDisplay("P1 万全神：分配（G1 北，G2 南，默认顺时针调整，冲突时'较低'编号灵活调整）")]
    [GroupDetails(["G1 prio1", "G1 prio2", "G1 prio3", "G1 prio4", "G2 prio1", "G2 prio2", "G2 prio3", "G2 prio4"])]
    [GroupPreset("LPDU (light parties): flex T>M>R", [0, 4, 3, 7, 1, 5, 2, 6])]
    [GroupPreset("NA (snake prio): TMRH, group 2 north", [4, 0, 7, 3, 5, 1, 6, 2])]
    public GroupAssignmentUnique P1PantokratorAssignments = new() { Assignments = [0, 4, 3, 7, 1, 5, 2, 6] };

    [PropertyDisplay("P1 万全神：队伍位置")]
    [PropertyCombo("North/South", "NE/SW")]
    public bool P1PantokratorNESW = false;

    [PropertyDisplay("P1 万全神：使用全局优先级 - 将 G1 视为比 G2 编号更低（因此 G1 更可能灵活调整）")]
    public bool P1PantokratorGlobalPriority = false;

    [PropertyDisplay("P2 团队协同：分配（G1 左，G2 右（如果看向眼睛），冲突时'较低'编号灵活调整）")]
    [GroupDetails(["G1 prio1", "G1 prio2", "G1 prio3", "G1 prio4", "G2 prio1", "G2 prio2", "G2 prio3", "G2 prio4"])]
    [GroupPreset("LPDU (light parties): flex R>M>H", [3, 7, 2, 6, 1, 5, 0, 4])]
    [GroupPreset("NA (HRMT conga)", [0, 4, 3, 7, 1, 5, 2, 6])]
    public GroupAssignmentUnique P2PartySynergyAssignments = new() { Assignments = [3, 7, 2, 6, 1, 5, 0, 4] };

    [PropertyDisplay("P2 团队协同：使用全局优先级 - 将 G1 视为比 G2 编号更低（因此 G1 更可能灵活调整）")]
    public bool P2PartySynergyGlobalPriority = false;

    [PropertyDisplay("P2 团队协同：远程故障（远距离连线）的 G2 顺序")]
    [PropertyCombo("GPOB (only B and G swap) (NA)", "GOPB (reverse order) (LPDU)")]
    public bool P2PartySynergyG2ReverseAll = true;

    [PropertyDisplay("P2 团队协同：如果两个堆叠在同一组，则交换优先级")]
    [PropertyCombo("Northernmost pair (NA)", "Southernmost pair (LPDU)")]
    public bool P2PartySynergyStackSwapSouth = true;

    [PropertyDisplay("P3 中场：分散/集合位置分配，从西到东")]
    [GroupDetails(["1", "2", "3", "4", "5", "6", "7", "8"])]
    [GroupPreset("LPDU (RMTH HTMR)", [2, 5, 3, 4, 1, 6, 0, 7])]
    [GroupPreset("NA (HRMT conga)", [3, 4, 0, 7, 2, 5, 1, 6])]
    public GroupAssignmentUnique P3IntermissionAssignments = new() { Assignments = [2, 5, 3, 4, 1, 6, 0, 7] };

    [PropertyDisplay("P3 中场：分散/集合位置")]
    [PropertyCombo("Stacks S, spreads N", "Stacks N, spreads S")]
    public bool P3IntermissionStacksNorth = true;

    [PropertyDisplay("P3 监视器：优先级，从北到南")]
    [GroupDetails(["1", "2", "3", "4", "5", "6", "7", "8"])]
    [GroupPreset("LPDU (HTMR)", [2, 3, 0, 1, 4, 5, 6, 7])]
    [GroupPreset("NA (HRMT conga)", [3, 4, 0, 7, 2, 5, 1, 6])]
    public GroupAssignmentUnique P3MonitorsAssignments = new() { Assignments = [2, 3, 0, 1, 4, 5, 6, 7] };

    [PropertyDisplay("P3 监视器：安全侧监视器位置")]
    [PropertyCombo("North (LPDU)", "South (Aether)")]
    public bool P3LastMonitorSouth = false;

    [PropertyDisplay("P3 监视器：在机制解决前自动面向正确方向", tooltip: "此功能需要启用 设置 -> 动作调整 -> 智能角色朝向。")]
    public bool P3MonitorForbiddenDirections = true;

    [PropertyDisplay("P4 波动炮：优先级，从北到南（假设南方灵活调整）")]
    [GroupDetails(["W1", "E1", "W2", "E2", "W3", "E3", "W4", "E4"])]
    [GroupPreset("LPDU (TRHM)", [0, 1, 4, 5, 6, 7, 2, 3])]
    public GroupAssignmentUnique P4WaveCannonAssignments = new() { Assignments = [0, 1, 4, 5, 6, 7, 2, 3] };
}
