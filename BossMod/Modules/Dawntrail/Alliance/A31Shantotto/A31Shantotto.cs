#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace BossMod.Dawntrail.Alliance.A31Shantotto;

public enum OID : uint
{
    Boss = 0x4D9F, // R4.900, x1
    Helper = 0x233C, // R0.500, x16, Helper type
    _Gen_1 = 0x4DDC, // R2.400, x2, Helper type
    _Gen_ = 0x4DDD, // R1.000, x21
}

public enum AID : uint
{
    _AutoAttack_Attack = 872, // Boss->player, no cast, single-target
    _Spell_FlarePlay = 50215, // Boss->self, 5.0s cast, range 78 circle
    _Spell_Vidohunir = 50213, // Boss->player, 5.0+1.0s cast, single-target
    _Spell_Vidohunir1 = 50214, // Helper->players, 6.0s cast, range 5 circle
    _Spell_EmpiricalResearch = 50206, // Boss->self, 3.0s cast, single-target
    _Spell_EmpiricalResearch1 = 50208, // Helper->self, 3.8s cast, range 80 width 12 rect
    _Spell_SuperiorStoneII = 50193, // Boss->self, 4.0s cast, range 60 circle
    _Spell_SuperiorStoneII1 = 50194, // Helper->self, 4.8s cast, range 21 width 13 rect
    _Spell_SuperiorStoneII2 = 51025, // Helper->self, no cast, range 21 width 13 rect
    _Spell_GroundbreakingQuake = 50195, // Boss->self, 8.0s cast, single-target
    _Spell_GroundbreakingQuake1 = 50196, // Helper->self, 9.0s cast, range 30 width 12 rect
    _Spell_PainfulPressure = 50197, // Helper->self, no cast, ???
    _Ability_ = 50192, // Boss->location, no cast, single-target
    _Spell_DiagrammaticDoorway = 50200, // Boss->self, 3.0s cast, single-target
    _Ability_DaintyStep = 50205, // Boss->location, no cast, single-target
    _Spell_CircumscribedFire = 50201, // Boss->self, 7.0s cast, range ?-70 donut
    _Spell_CircumscribedFire1 = 50202, // Boss->self, 1.0s cast, range 6-70 donut
    _Spell_LocalizedBlizzard = 50203, // Boss->self, 2.2s cast, range 10 circle
    _Spell_ThunderAndError = 50204, // Helper->players, 5.0s cast, range 5 circle
    _Spell_MeteoricRhyme = 50182, // Boss->self, 3.0s cast, single-target
    _Spell_SmallSpecimen = 50184, // Helper->location, 4.0s cast, range 6 circle
    _Spell_LargeSpecimen = 50186, // 4DDC->self, 6.0s cast, range 50 circle
    _Spell_StardustSpecimen = 50185, // Helper->players, 6.0s cast, range 6 circle
    _Spell_ = 50183, // Boss->location, no cast, range 30 circle
    _Ability_1 = 50648, // Boss->location, no cast, single-target
    _Spell_Shockwave = 50187, // Helper->self, 10.5s cast, range 48 width 60 rect
    _Ability_FallingRubble = 50191, // Helper->self, 4.5s cast, range 35 width 10 rect
    _Ability_FallingRubble1 = 50189, // Helper->self, 4.5s cast, range 12 circle
    _Ability_FallingRubble2 = 50190, // Helper->self, 4.5s cast, range 25 width 6 rect
    _Ability_FallingRubble3 = 50188, // Helper->self, 4.5s cast, range 8 circle
    _Spell_AeroDynamics = 50198, // Boss->self, 3.0s cast, single-target
    _Spell_AeroDynamics1 = 50199, // Helper->self, no cast, range 48 width 60 rect
    _Spell_1 = 50382, // Helper->self, no cast, range 48 width 60 rect
    _Spell_FinalExam = 50210, // Boss->player, 4.2+0.8s cast, single-target
    _Spell_FinalExam1 = 50211, // Helper->players, 5.0s cast, range 6 circle
    _Spell_FinalExam2 = 50212, // Helper->players, no cast, range 6 circle
}

public enum SID : uint
{
    _Gen_VulnerabilityUp = 1789, // Helper/Boss->player, extra=0x1/0x2/0x3/0x4
    _Gen_Weakness = 43, // none->player, extra=0x0
    _Gen_Transcendent = 418, // none->player, extra=0x0
    _Gen_ = 2552, // Boss->Boss/4DDC, extra=0x475
    _Gen_EasterlyWinds = 5398, // none->player/4D66, extra=0x0
    _Gen_WesterlyWinds = 5399, // none->player, extra=0x0
    _Gen_1 = 2160, // none->4D66, extra=0x3931
    _Gen_BrinkOfDeath = 44, // none->player, extra=0x0
    _Gen_DownForTheCount = 3908, // Helper->player, extra=0xEC7
}

public enum IconID : uint
{
    _Gen_Icon_m0926trg_t0a1 = 570, // player->self
    _Gen_Icon_target_ae_5m_s5_0k2 = 558, // player->self
    _Gen_Icon_com_share3_6s0p = 318, // player->self
    _Gen_Icon_com_s8count05x = 713, // player/4D66->self
    _Gen_Icon_com_share4a1 = 305, // player->self
}

public enum TetherID : uint
{
    _Gen_Tether_chn_d1088_c1v = 384, // 4DDD->4DDD
    _Gen_Tether_chn_d1088_c0v = 383, // 4DDD->4DDD
    _Gen_Tether_chn_mto_tykdn_1v = 382, // 4DDD->4DDD
}

class FlarePlay(BossModule module) : Components.RaidwideCast(module, AID._Spell_FlarePlay);
class Vidohunir(BossModule module) : Components.CastSharedTankbuster(module, AID._Spell_Vidohunir1, 5);
class EmpiricalResearch(BossModule module) : Components.StandardAOEs(module, AID._Spell_EmpiricalResearch1, new AOEShapeRect(80, 6));
class SuperiorStoneIIRaidwide(BossModule module) : Components.RaidwideCast(module, AID._Spell_SuperiorStoneII);
class SuperiorStoneIIRect(BossModule module) : Components.StandardAOEs(module, AID._Spell_SuperiorStoneII1, new AOEShapeRect(21, 6.5f));
class GroundbreakingQuake(BossModule module) : Components.StandardAOEs(module, AID._Spell_GroundbreakingQuake1, new AOEShapeRect(30, 6));
class SuperiorStoneArena(BossModule module) : Components.CastCounter(module, AID._Spell_SuperiorStoneII1)
{
    readonly RelSimplifiedComplexPolygon _rect = new(CurveApprox.Rect(new WDir(24, 0), new WDir(0, 30)));
    RelSimplifiedComplexPolygon _current = new(CurveApprox.Rect(new WDir(24, 0), new WDir(0, 30)));

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (spell.Action == WatchedAction)
        {
            NumCasts++;
            List<WDir> curve = [new WDir(-6.5f, 0).Rotate(spell.Rotation), new WDir(-6.5f, 21).Rotate(spell.Rotation), new WDir(6.5f, 21).Rotate(spell.Rotation), new WDir(6.5f, 0).Rotate(spell.Rotation)];
            var poly = new RelSimplifiedComplexPolygon(curve.Select(c => c + (caster.Position - Arena.Center)));
            _current = Arena.Bounds.Clipper.Difference(new(_current), new(poly));
            Arena.Bounds = new ArenaBoundsCustom(30, _current);
        }

        if ((AID)spell.Action.ID == AID._Spell_PainfulPressure)
        {
            _current = _rect;
            Arena.Bounds = new ArenaBoundsRect(24, 30);
        }
    }
}

class A31ShantottoTheDemonStates : StateMachineBuilder
{
    public A31ShantottoTheDemonStates(BossModule module) : base(module)
    {
        DeathPhase(0, P1);
    }

    void P1(uint id)
    {
        FlarePlay(id, 8.2f);

        Vidohunir(id + 0x10, 4.3f);

        Cast(id + 0x100, AID._Spell_EmpiricalResearch, 4, 3)
            .ActivateOnEnter<EmpiricalResearch>();
        ComponentCondition<EmpiricalResearch>(id + 0x102, 0.8f, r => r.NumCasts > 0, "Laser")
            .DeactivateOnExit<EmpiricalResearch>();

        Timeout(id + 0xFF0000, 10000, "???")
            .ActivateOnEnter<SuperiorStoneIIRaidwide>()
            .ActivateOnEnter<SuperiorStoneIIRect>()
            .ActivateOnEnter<SuperiorStoneArena>()
            .ActivateOnEnter<GroundbreakingQuake>();
    }

    void FlarePlay(uint id, float delay)
    {
        Cast(id, AID._Spell_FlarePlay, delay, 5, "Raidwide")
            .ActivateOnEnter<FlarePlay>()
            .DeactivateOnExit<FlarePlay>();
    }

    void Vidohunir(uint id, float delay)
    {
        Cast(id, AID._Spell_Vidohunir, delay, 5)
            .ActivateOnEnter<Vidohunir>();
        ComponentCondition<Vidohunir>(id + 2, 1, v => v.NumCasts > 0, "Shared tankbuster")
            .DeactivateOnExit<Vidohunir>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1117, NameID = 14778, DevOnly = true)]
public class A31ShantottoTheDemon(WorldState ws, Actor primary) : BossModule(ws, primary, new(0, -720), new ArenaBoundsRect(24, 30));
