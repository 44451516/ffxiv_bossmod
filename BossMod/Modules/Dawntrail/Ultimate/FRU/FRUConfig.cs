namespace BossMod.Dawntrail.Ultimate.FRU;

[ConfigDisplay(Order = 0x200, Parent = typeof(DawntrailConfig))]
public class FRUConfig() : ConfigNode()
{
    // TODO: fixed tethers option
    [PropertyDisplay("P1 信仰束缚（小队连线）：队伍分配和灵活优先级（较低编号灵活调整）")]
    [GroupDetails(["N prio 1", "N prio 2", "N prio 3", "N prio 4", "S prio 1", "S prio 2", "S prio 3", "S prio 4"])]
    [GroupPreset("Supports N, DD S", [0, 1, 2, 3, 4, 5, 6, 7])]
    [GroupPreset("G1 N, G2 S, TMRH", [0, 4, 3, 7, 1, 5, 2, 6])]
    public GroupAssignmentUnique P1BoundOfFaithAssignment = GroupAssignmentUnique.DefaultRoles();

    [PropertyDisplay("P1 信仰坠落（锥形连线）：康加舞优先级（两个没有连线且优先级较低的人加入奇数组）")]
    [GroupDetails(["1", "2", "3", "4", "5", "6", "7", "8"])]
    [GroupPreset("HHTTMMRR", [2, 3, 0, 1, 4, 5, 6, 7])]
    [GroupPreset("HRMTTMRH", [3, 4, 0, 7, 2, 5, 1, 6])]
    public GroupAssignmentUnique P1FallOfFaithAssignment = new() { Assignments = [2, 3, 0, 1, 4, 5, 6, 7] };

    [PropertyDisplay("P1 信仰坠落（锥形连线）：奇数组去W（而不是N）")]
    public bool P1FallOfFaithEW = false;

    [PropertyDisplay("P1 爆炸：塔填充优先级（较低编号去北方）")]
    [GroupDetails(["Tank N", "Tank S", "Fixed N", "Fixed Center", "Fixed S", "Flex 1", "Flex 2", "Flex 3"])]
    [GroupPreset("H1-R2-H2 fixed, M1-M2-R1 flex", [0, 1, 2, 4, 5, 6, 7, 3])]
    public GroupAssignmentUnique P1ExplosionsAssignment = new() { Assignments = [0, 1, 2, 4, 5, 6, 7, 3] };

    [PropertyDisplay("P1 爆炸：灵活职责只有在他们的自然塔是1时才填充3/4（而不是做康加舞）")]
    public bool P1ExplosionsPriorityFill;

    [PropertyDisplay("P1 爆炸：让坦克在坦克破坏上集合（使用减伤可存活，简化输出时间）")]
    public bool P1ExplosionsTankbusterCheese;

    [PropertyDisplay("P2 钻石星尘：方位分配")]
    [GroupDetails(["Support N", "Support E", "Support S", "Support W", "DD N", "DD E", "DD S", "DD W"])]
    [GroupPreset("Default", [0, 2, 3, 1, 7, 6, 4, 5])]
    public GroupAssignmentUnique P2DiamondDustCardinals = new() { Assignments = [0, 2, 3, 1, 7, 6, 4, 5] };

    [PropertyDisplay("P2 钻石星尘：支援去逆时针间方位")]
    public bool P2DiamondDustSupportsCCW;

    [PropertyDisplay("P2 钻石星尘：DD去逆时针间方位")]
    public bool P2DiamondDustDDCCW;

    [PropertyDisplay("P2 钻石星尘：击退组")]
    [GroupDetails(["G1 (CCW from N)", "G2 (CW from NE)"])]
    public GroupAssignmentLightParties P2DiamondDustKnockbacks = GroupAssignmentLightParties.DefaultLightParties();

    [PropertyDisplay("P2 光之泛滥：康加舞位置（较低编号去西方，假设顺时针旋转）")]
    [GroupDetails(["N1", "N2", "N3", "N4", "S1", "S2", "S3", "S4"])]
    [GroupPreset("HHTT/RRMM", [2, 3, 0, 1, 6, 7, 4, 5])]
    public GroupAssignmentUnique P2LightRampantAssignment = GroupAssignmentUnique.DefaultRoles();

    [PropertyDisplay("P3 终极相对论：位置分配（较低编号去NW/SW）")]
    [GroupDetails(["1", "2", "3", "4"])]
    [GroupPreset("HTTH/RMMR", [1, 2, 0, 3, 1, 2, 0, 3])]
    public GroupAssignmentDDSupportPairs P3UltimateRelativityAssignment = GroupAssignmentDDSupportPairs.DefaultMeleeTogether();

    [PropertyDisplay("P3 天启：分配（G1从N逆时针，G2从NE顺时针，冲突时'较低'编号灵活调整）")]
    [GroupDetails(["G1 prio1", "G1 prio2", "G1 prio3", "G1 prio4", "G2 prio1", "G2 prio2", "G2 prio3", "G2 prio4"])]
    [GroupPreset("TTHH/MMRR", [0, 1, 2, 3, 4, 5, 6, 7])]
    [GroupPreset("TMRH/TMRH", [0, 4, 3, 7, 1, 5, 2, 6])]
    public GroupAssignmentUnique P3ApocalypseAssignments = GroupAssignmentUnique.DefaultRoles();

    [PropertyDisplay("P3 天启：输出时间交换（仅考虑优先级1/2和3/4内的交换，假设这些是近战和远程）")]
    public bool P3ApocalypseUptime;

    [PropertyDisplay("P3 天启：忽略交换并使用初始静态位置进行分散")]
    public bool P3ApocalypseStaticSpreads;

    [PropertyDisplay("P4 暗光龙诗：分配（较低优先级保持更顺时针，最低优先级支援占据N塔）")]
    [GroupDetails(["Support prio1", "Support prio2", "Support prio3", "Support prio4", "DD prio1", "DD prio2", "DD prio3", "DD prio4"])]
    [GroupPreset("Default (healer N)", [2, 3, 0, 1, 4, 5, 6, 7])]
    public GroupAssignmentUnique P4DarklitDragonsongAssignments = new() { Assignments = [2, 3, 0, 1, 4, 5, 6, 7] };

    [PropertyDisplay("P4 结晶时间：爪子分配（较低优先级去西方）", separator: true)]
    [GroupDetails(["Prio 1", "Prio 2", "Prio 3", "Prio 4", "Prio 5", "Prio 6", "Prio 7", "Prio 8"])]
    [GroupPreset("Default HTMR", [3, 2, 1, 0, 4, 5, 6, 7])]
    public GroupAssignmentUnique P4CrystallizeTimeAssignments = new() { Assignments = [3, 2, 1, 0, 4, 5, 6, 7] };

    // ai-only settings
    [PropertyDisplay("P1 气旋破（分散）：诱饵时钟位置（支援应靠近DD以解决配对）", tooltip: "仅由AI使用")]
    [GroupDetails(["N", "NE", "E", "SE", "S", "SW", "W", "NW"])]
    [GroupPreset("Default", [0, 4, 6, 2, 5, 3, 7, 1])]
    public GroupAssignmentUnique P1CyclonicBreakSpots = new() { Assignments = [0, 4, 6, 2, 5, 3, 7, 1] };

    [PropertyDisplay("P1 气旋破（分散）：配对躲避方向", tooltip: "仅由AI使用")]
    [PropertyCombo("Supports CW, DD CCW", "Supports CCW, DD CW")]
    public bool P1CyclonicBreakStackSupportsCCW = true;

    [PropertyDisplay("P1 气旋破（分散）：支援的分散躲避方向", tooltip: "仅由AI使用")]
    [PropertyCombo("CW", "CCW")]
    public bool P1CyclonicBreakSpreadSupportsCCW;

    [PropertyDisplay("P1 气旋破（分散）：DD的分散躲避方向", tooltip: "仅由AI使用")]
    [PropertyCombo("CW", "CCW")]
    public bool P1CyclonicBreakSpreadDDCCW;

    [PropertyDisplay("P1 乌托邦天空：初始时钟位置（MT应靠近OT以解决坦克破坏）", tooltip: "仅由AI使用")]
    [GroupDetails(["N", "NE", "E", "SE", "S", "SW", "W", "NW"])]
    [GroupPreset("Default", [0, 1, 6, 2, 5, 3, 7, 4])]
    public GroupAssignmentUnique P1UtopianSkyInitialSpots = new() { Assignments = [0, 1, 6, 2, 5, 3, 7, 4] };

    [PropertyDisplay("P1 乌托邦天空：分散位置（G1从N逆时针，G2从NE顺时针）", tooltip: "仅由AI使用")]
    [GroupDetails(["G1 Close", "G1 Far Center", "G1 Far Left", "G1 Far Right", "G2 Close", "G2 Far Center", "G2 Far Left", "G2 Far Right"])]
    [GroupPreset("Default", [1, 5, 0, 4, 2, 6, 3, 7])]
    public GroupAssignmentUnique P1UtopianSkySpreadSpots = new() { Assignments = [1, 5, 0, 4, 2, 6, 3, 7] };

    [PropertyDisplay("P2 镜中镜：第一次分散的位置（从首领看向蓝镜）", tooltip: "仅由AI使用")]
    [GroupDetails(["Boss opposite right", "Boss opposite left", "Boss side right", "Boss side left", "Mirror diagonal right", "Mirror diagonal left", "Mirror wall right", "Mirror wall left"])]
    [GroupPreset("Default", [0, 1, 4, 5, 2, 3, 6, 7])]
    public GroupAssignmentUnique P2MirrorMirror1SpreadSpots = new() { Assignments = [0, 1, 4, 5, 2, 3, 6, 7] };

    [PropertyDisplay("P2 镜中镜：第二次分散的位置（看向红镜，如果两个红镜对称则假设顺时针旋转）", tooltip: "仅由AI使用")]
    [GroupDetails(["Boss wall opposite other", "Boss wall facing other", "Boss center", "Boss diagonal", "Mirror wall right", "Mirror wall left", "Mirror center right", "Mirror center left"])]
    [GroupPreset("Default", [1, 0, 6, 7, 2, 3, 4, 5])]
    public GroupAssignmentUnique P2MirrorMirror2SpreadSpots = new() { Assignments = [1, 0, 6, 7, 2, 3, 4, 5] };

    [PropertyDisplay("P2 光之泛滥后的放逐：分散时钟位置（支援应靠近DD以解决配对）", tooltip: "仅由AI使用")]
    [GroupDetails(["N", "NE", "E", "SE", "S", "SW", "W", "NW"])]
    [GroupPreset("Default", [0, 4, 6, 2, 5, 3, 7, 1])]
    public GroupAssignmentUnique P2Banish2SpreadSpots = new() { Assignments = [0, 4, 6, 2, 5, 3, 7, 1] };

    [PropertyDisplay("P2 光之泛滥后的放逐：从默认分散位置移动以解决配对的职责", tooltip: "仅由AI使用")]
    [PropertyCombo("DD", "Supports")]
    public bool P2Banish2SupportsMoveToStack = true;

    [PropertyDisplay("P2 光之泛滥后的放逐：移动以解决配对的方向", tooltip: "仅由AI使用")]
    [PropertyCombo("CW", "CCW")]
    public bool P2Banish2MoveCCWToStack = true;

    [PropertyDisplay("P2 中场：时钟位置（方位优先他们的水晶，间方位诱饵）", tooltip: "仅由AI使用")]
    [GroupDetails(["N", "NE", "E", "SE", "S", "SW", "W", "NW"])]
    [GroupPreset("Default", [0, 2, 5, 3, 4, 6, 7, 1])]
    public GroupAssignmentUnique P2IntermissionClockSpots = new() { Assignments = [0, 2, 5, 3, 4, 6, 7, 1] };

    [PropertyDisplay("P3 天启：组1的暗水1方向（组2相反）", tooltip: "仅由AI使用")]
    [PropertySlider(-180, 180)]
    public float P3ApocalypseDarkWater1ReferenceDirection = -90;

    [PropertyDisplay("P3 最暗之舞：诱饵者", tooltip: "仅由AI使用")]
    [PropertyCombo("MT", "OT")]
    public bool P3DarkestDanceOTBait;

    [PropertyDisplay("P4 忧郁之舞：诱饵者", tooltip: "仅由AI使用")]
    [PropertyCombo("MT", "OT")]
    public bool P4SomberDanceOTBait = true;

    [PropertyDisplay("P5 龙神冲：侧边分配", tooltip: "仅由AI使用")]
    [GroupDetails(["Left (looking at boss)", "Right (looking at boss)"])]
    public GroupAssignmentLightParties P5AkhMornAssignments = GroupAssignmentLightParties.DefaultLightParties();

    [PropertyDisplay("P5 极化打击：诱饵顺序", tooltip: "仅由AI使用")]
    [GroupDetails(["Left 1", "Left 2", "Left 3", "Left 4", "Right 1", "Right 2", "Right 3", "Right 4"])]
    [GroupPreset("TMRH", [0, 4, 3, 7, 1, 5, 2, 6])]
    public GroupAssignmentUnique P5PolarizingStrikesAssignments = new() { Assignments = [0, 4, 3, 7, 1, 5, 2, 6] };
}
