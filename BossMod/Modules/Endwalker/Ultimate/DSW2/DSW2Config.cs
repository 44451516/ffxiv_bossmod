namespace BossMod.Endwalker.Ultimate.DSW2
{
    [ConfigDisplay(Order = 0x201, Parent = typeof(EndwalkerConfig))]
    public class DSW2Config : CooldownPlanningConfigNode
    {
        [PropertyDisplay("P2 二运 苍穹之阵：圣杖(charges): group assignments")]
        [GroupDetails(new string[] { "West/Across", "East/Behind" })]
        public GroupAssignmentLightParties P2SanctityGroups = GroupAssignmentLightParties.DefaultLightParties();

        [PropertyDisplay("P2 二运 苍穹之阵：圣杖(charges): groups relative to DRK (across/behind) rather than absolute (west/east)")]
        public bool P2SanctityRelative = false;

        // [PropertyDisplay("P2 二运 苍穹之阵：圣杖(charges)[国服选Melee]: role responsible for balancing groups (if not set, swap with role partner instead)")]
        [PropertyDisplay("P2 二运 苍穹之阵：圣杖(charges): role responsible for balancing groups (if not set, swap with role partner instead)")]
        public Role P2SanctitySwapRole;

        [PropertyDisplay("P2 二运 苍穹之阵：圣杖(meteors): pair assignments")]
        [GroupDetails(new string[] { "North[北]", "West[西]", "South[南]", "East[东]" })]
        public GroupAssignmentDDSupportPairs P2Sanctity2Pairs = GroupAssignmentDDSupportPairs.DefaultOneMeleePerPair();

        [PropertyDisplay("P2 二运 苍穹之阵：圣杖(meteors): prefer E/W for prey rather than N/S")]
        public bool P2Sanctity2PreferEWPrey = false;

        [PropertyDisplay("P2 二运 苍穹之阵：圣杖(meteors): swap N/E and S/W (rather than N/W and S/E) when both soakers are on wrong cardinals")]
        public bool P2Sanctity2SwapBothNE = false;

        // [PropertyDisplay("P2 二运 苍穹之阵：圣杖(meteors)[国服选这个]: take CW (rather than CCW) intercardinal as non-prey role for second towers")]
        [PropertyDisplay("P2 二运 苍穹之阵：圣杖(meteors): take CW (rather than CCW) intercardinal as non-prey role for second towers")]
        public bool P2Sanctity2NonPreyTowerCW = false;

        [PropertyDisplay("P3 four towers with counters: assignments")]
        [GroupDetails(new string[] { "西北固定 Flex", "东北固定 Flex", "东南固定 Flex", "西南固定 Flex", "西北动态 Stay", "东北动态 Stay", "东南动态 Stay", "西南动态 Stay" })]
        public GroupAssignmentUnique P3DarkdragonDiveCounterGroups = GroupAssignmentUnique.Default();

        [PropertyDisplay("P3 four towers with counters: prefer flexing to CCW tower (rather than to CW)")]
        public bool P3DarkdragonDiveCounterPreferCCWFlex = false;

        public DSW2Config() : base(90) { }
    }
}
