#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace BossMod.Dawntrail.Alliance.A35Promathia;

public enum OID : uint
{
    Boss = 0x4DEE, // R8.000, x1
    Helper = 0x233C, // R0.500, x30 (spawn during fight), Helper type
    _Gen_LinkOfPromathia = 0x4DEF, // R2.240, x10
    _Gen_EmptyThinker = 0x4DF3, // R2.300, x0 (spawn during fight)
    _Gen_EmptyWeeper = 0x4DF2, // R2.000, x0 (spawn during fight)
    _Gen_EmptyWanderer = 0x4DF1, // R1.200, x0 (spawn during fight)
    _Gen_MemoryReceptacle = 0x4DF0, // R2.040, x0 (spawn during fight)
}

public enum AID : uint
{
    _AutoAttack_Attack = 45308, // Boss->player, no cast, single-target
    _Ability_ = 50342, // Boss->location, no cast, single-target
    _Ability_EmptySalvation = 50317, // Boss->location, 5.0s cast, range 25 circle
    _Ability_FleetingEternity = 50318, // Boss->self, 3.5+1.0s cast, single-target
    _Ability_Explosion = 50320, // Helper->self, 5.0s cast, range 16 circle
    _Ability_WheelOfImpregnability = 50321, // Boss->self, 2.0+1.0s cast, single-target
    _Ability_WheelOfImpregnability1 = 50323, // Helper->self, no cast, range 13 circle
    _Ability_BastionOfTwilight = 50322, // Boss->self, 2.0+1.0s cast, single-target
    _Ability_PestilentPenance = 50330, // Boss->self, 6.4+0.6s cast, single-target
    _Ability_BastionOfTwilight1 = 50324, // Helper->self, no cast, range ?-50 donut
    _Ability_PestilentPenance1 = 50331, // Helper->self, 7.0s cast, range 50 width 50 rect
    _Ability_FleetingEternity1 = 50319, // Boss->self, 3.5+1.0s cast, single-target
    _Spell_Comet = 50337, // Boss->self, 4.5+0.5s cast, single-target
    _Spell_Comet1 = 50338, // Helper->players, 0.5s cast, range 6 circle
    _Ability_1 = 50345, // Helper->4D66, 1.0s cast, single-target
    _Ability_FalseGenesis = 50343, // Boss->self, 9.5+0.5s cast, single-target
    _Ability_FalseGenesis1 = 50344, // Helper->self, 0.5s cast, range 25 circle
    _Ability_WindsOfPromyvion = 50352, // 4DF3->self, 3.9+0.6s cast, single-target
    _Ability_WindsOfPromyvion1 = 50353, // Helper->self, 4.5s cast, range 16 width 6 rect
    _Ability_WindsOfPromyvion2 = 50460, // 4DF3->self, no cast, single-target
    _Ability_WindsOfPromyvion3 = 50354, // Helper->self, 0.6s cast, range 16 width 6 rect
    _Ability_EmptyBeleaguer = 50351, // 4DF1->self, 6.0s cast, range 6 circle
    _Ability_AuroralDrape = 50355, // 4DF2->self, 7.0s cast, range 7 width 7 rect
    _Ability_EmptySeed = 50349, // 4DF0->self, 5.0s cast, range 10 circle
    _Ability_DeadlyRebirth = 50346, // Boss->self, 5.7+1.3s cast, single-target
    _Ability_DeadlyRebirth1 = 50348, // Helper->self, 1.3s cast, range 50 circle
    _Ability_DeadlyRebirth2 = 50694, // Boss->self, 8.0+2.0s cast, single-target
    _Ability_DeadlyRebirth3 = 50347, // Helper->self, 2.0s cast, range 50 circle
    _Ability_EarthboundHeaven = 50333, // Boss->self, 2.0+1.0s cast, single-target
    _Ability_MalevolentBlessing = 50327, // Boss->self, 5.7+0.8s cast, single-target
    _Ability_MalevolentBlessing1 = 50328, // Helper->self, 6.5s cast, range 40 23-degree cone
    _Ability_MalevolentBlessing2 = 50329, // Helper->self, 6.5s cast, range 50 width 50 rect
    _Ability_PestilentPenance2 = 50332, // 4DEF->self, 7.5s cast, range 50 width 5 rect
    _Ability_InfernalDeliverance = 50334, // Boss->self, 5.5+1.5s cast, single-target
    _Ability_InfernalDeliverance1 = 50335, // Helper->self, 7.0s cast, range 4 circle
    _Ability_InfernalDeliverance2 = 50565, // Helper->self, 5.0s cast, range 8 circle
    _Spell_Meteor = 50339, // Boss->self, 4.5+0.5s cast, single-target
    _Spell_Meteor1 = 50340, // Helper->players, 0.5s cast, range 6 circle
    _Spell_Meteor2 = 50341, // Helper->player, 0.5s cast, range 6 circle
    _Ability_MalevolentBlessing3 = 50326, // Boss->self, 5.7+0.8s cast, single-target
}

public enum SID : uint
{
    _Gen_ = 2552, // none->Boss, extra=0x48D/0x458/0x457
    _Gen_Heavy = 1796, // none->player, extra=0x32
    _Gen_VulnerabilityUp = 1789, // Helper/4DF2/4DF1/4DEF->player, extra=0x1/0x2/0x3/0x4/0x5/0x6/0x7/0x8
    _Gen_Weakness = 43, // none->player, extra=0x0
    _Gen_Transcendent = 418, // none->player, extra=0x0
    _Gen_BrinkOfDeath = 44, // none->player, extra=0x0
    _Gen_DirectionalDisregard = 3808, // none->Boss, extra=0x0
    _Gen_1 = 2056, // none->4DF2, extra=0x498
    _Gen_SystemLock = 2578, // none->player, extra=0x0
    _Gen_Invincibility = 1570, // none->player, extra=0x0
    _Gen_2 = 2160, // none->4D66, extra=0x3931
    _Gen_3 = 2273, // Boss->Boss, extra=0x226
    _Gen_DownForTheCount = 3908, // Helper->player/4D66, extra=0xEC7
}

public enum IconID : uint
{
    _Gen_Icon_m1001_lockon_c0w = 687, // Boss->self
    _Gen_Icon_m1001_lockon_c1w = 688, // Boss->self
    _Gen_Icon_tank_lockonae_6m_5s_01t = 344, // player->self
    _Gen_Icon_m1001_turning_right01w = 689, // 4DF3->self
    _Gen_Icon_m1001_turning_left01w = 690, // 4DF3->self
    _Gen_Icon_loc06sp_05ak1 = 466, // player->self
}

public enum TetherID : uint
{
    _Gen_Tether_chm_m1001_01w = 427, // Boss->4D66
    _Gen_Tether_chn_nomal01f = 12, // 4DF0->4D66
}

class A35PromathiaStates : StateMachineBuilder
{
    public A35PromathiaStates(BossModule module) : base(module)
    {
        DeathPhase(0, SinglePhase);
    }

    private void SinglePhase(uint id)
    {
        SimpleState(id + 0xFF0000, 10000, "???");
    }

    //private void XXX(uint id, float delay)
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1117, NameID = 14779, DevOnly = true)]
public class A35Promathia(WorldState ws, Actor primary) : BossModule(ws, primary, new(-820, -820), new ArenaBoundsCircle(25));

