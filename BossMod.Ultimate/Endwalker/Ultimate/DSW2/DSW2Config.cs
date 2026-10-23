namespace BossMod.Endwalker.Ultimate.DSW2;

[ConfigDisplay(Order = 0x201, Parent = typeof(EndwalkerConfig))]
public class DSW2Config() : ConfigNode()
{
    [PropertyDisplay("P2 圣域（充能）：队伍分配")]
    [GroupDetails(["West/Across", "East/Behind"])]
    [GroupPreset("Default light parties", [0, 1, 0, 1, 0, 1, 0, 1])]
    [GroupPreset("Inverted light parties", [1, 0, 1, 0, 1, 0, 1, 0])]
    public GroupAssignmentLightParties P2SanctityGroups = GroupAssignmentLightParties.DefaultLightParties();

    [PropertyDisplay("P2 圣域（充能）：队伍相对于暗骑（对面/背后）而不是绝对（西/东）")]
    public bool P2SanctityRelative = false;

    [PropertyDisplay("P2 圣域（充能）：负责平衡队伍的职责（如果未设置，则与职责伙伴交换）")]
    public Role P2SanctitySwapRole;

    [PropertyDisplay("P2 圣域（流星）：如需要自动使用击退免疫")]
    public bool P2Sanctity2AutomaticAntiKB = true;

    [PropertyDisplay("P2 圣域（流星）：配对分配")]
    [GroupDetails(["North", "East", "South", "West"])]
    [GroupPreset("MT/R1 N, OT/R2 S, H1/M1 E, H2/M2 W", [0, 2, 1, 3, 1, 3, 0, 2])]
    [GroupPreset("MT/R1 N, OT/R2 S, H1/M1 W, H2/M2 E", [0, 2, 3, 1, 3, 1, 0, 2])]
    public GroupAssignmentDDSupportPairs P2Sanctity2Pairs = GroupAssignmentDDSupportPairs.DefaultOneMeleePerPair();

    public enum P2PreyCardinals
    {
        [PropertyDisplay("始终 N/S")]
        AlwaysNS,

        [PropertyDisplay("始终 E/W")]
        AlwaysEW,

        [PropertyDisplay("N/S，除非两个猎物都从 E & W 开始")]
        PreferNS,

        [PropertyDisplay("E/W，除非两个猎物都从 N & S 开始")]
        PreferEW,
    }

    [PropertyDisplay("P2 圣域（流星）：猎物目标的优先方位")]
    public P2PreyCardinals P2Sanctity2PreyCardinals;

    [PropertyDisplay("P2 圣域（流星）：即使对于120度模式也强制优先方位（交换更简单，但移动更复杂）")]
    public bool P2Sanctity2ForcePreferredPrey = true;

    public enum P2PreySwapDirection
    {
        [PropertyDisplay("所有猎物职责顺时针旋转")]
        RotateCW,

        [PropertyDisplay("所有猎物职责逆时针旋转")]
        RotateCCW,

        [PropertyDisplay("成对：N <-> E, S <-> W")]
        PairsNE,

        [PropertyDisplay("成对：N <-> W, S <-> E")]
        PairsNW,
    }

    [PropertyDisplay("P2 圣域（流星）：如果两个猎物目标都在错误的方位上，则交换方向")]
    public P2PreySwapDirection P2Sanctity2SwapDirection;

    [PropertyDisplay("P2 圣域（流星）：猎物职责的优先外塔")]
    [PropertyCombo("CCW (leftmost, if facing outside)", "CW (rightmost, if facing outside)")]
    public bool P2Sanctity2PreferCWTowerAsPrey = true;

    public enum P2OuterTowers
    {
        [PropertyDisplay("不尝试分配外塔")]
        None,

        [PropertyDisplay("始终使用优先方向")]
        AlwaysPreferred,

        [PropertyDisplay("如果角度更好，两个猎物目标都使用共同相反方向；没有猎物目标的象限中的玩家仍使用优先方向")]
        SynchronizedTargets,

        [PropertyDisplay("如果角度更好，两个猎物目标都使用共同相反方向；所有象限中的玩家使用相同方向")]
        SynchronizedRole,

        [PropertyDisplay("猎物目标使用任何给出最佳角度的方向；没有猎物目标的象限中的玩家仍使用优先方向")]
        Individual
    }

    [PropertyDisplay("P2 圣域（流星）：外塔分配策略")]
    public P2OuterTowers P2Sanctity2OuterTowers = P2OuterTowers.Individual;

    public enum P2InnerTowers
    {
        [PropertyDisplay("不尝试分配内塔")]
        None,

        [PropertyDisplay("分配最近的无歧义内塔")]
        Closest,

        [PropertyDisplay("分配第一个未分配给更近的人的顺时针塔")]
        CW,
    }

    [PropertyDisplay("P2 圣域（流星）：内塔分配策略")]
    public P2InnerTowers P2Sanctity2InnerTowers = P2InnerTowers.CW;

    [PropertyDisplay("P2 圣域（流星）：非猎物职责第二塔的间方位")]
    [PropertyCombo("CCW", "CW")]
    public bool P2Sanctity2NonPreyTowerCW = false;

    [PropertyDisplay("P3 优雅俯冲：向西看箭头而不是向东（因此前箭头占据E位置，后箭头占据W位置）")]
    public bool P3DiveFromGraceLookWest = false;

    [PropertyDisplay("P3 枚举塔：分配")]
    [GroupDetails(["NW Flex", "NE Flex", "SE Flex", "SW Flex", "NW Stay", "NE Stay", "SE Stay", "SW Stay"])]
    [GroupPreset("LPDU", [1, 3, 6, 0, 2, 4, 5, 7])]
    [GroupPreset("LPDU but CCW", [0, 2, 5, 7, 1, 3, 4, 6])]
    [GroupPreset("NA", [1, 3, 4, 6, 0, 2, 5, 7])]
    public GroupAssignmentUnique P3DarkdragonDiveCounterGroups = GroupAssignmentUnique.Default();

    [PropertyDisplay("P3 枚举塔：优先灵活调整到逆时针塔（而不是顺时针）")]
    public bool P3DarkdragonDiveCounterPreferCCWFlex = false;

    public enum P6MortalVow
    {
        [PropertyDisplay("不假设任何顺序")]
        None,

        [PropertyDisplay("LPDU: MT->OT->M1 (M2作为后备)->R1")]
        TanksMeleeR1,

        [PropertyDisplay("LPDU: MT->OT->M1 (M2作为后备)->R2")]
        TanksMeleeR2,
    }

    [PropertyDisplay("P6 致命誓言传递顺序")]
    public P6MortalVow P6MortalVowOrder = P6MortalVow.None;
}
