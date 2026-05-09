#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace BossMod.Dawntrail.Alliance.A33AlexanderResurrected;

public enum OID : uint
{
    Helper = 0x233C, // R0.500, x43-44, Helper type
    Boss = 0x4D5C, // R8.250, x1
    _Gen_GordiusSystem = 0x4D5D, // R4.005, x0 (spawn during fight)
}

public enum AID : uint
{
    _AutoAttack_ = 50180, // Boss->player, no cast, single-target
    _Spell_BanishgaIV = 50161, // Boss->self, 5.0s cast, range 80 circle
    _Ability_ = 50123, // Boss->location, no cast, single-target
    _Weaponskill_DivineArrow = 50124, // Boss->self, 10.0s cast, single-target
    _Weaponskill_DivineArrow1 = 50126, // Boss->self, no cast, single-target
    _Weaponskill_DivineArrow2 = 50130, // Helper->self, 1.0s cast, range 45 90-degree cone
    _Weaponskill_DivineArrow3 = 50478, // Helper->self, no cast, range 45 90-degree cone
    _Weaponskill_DivineArrow4 = 50131, // Helper->self, 9.5s cast, range 10 circle
    _Weaponskill_DivineArrow5 = 50132, // Helper->self, 11.5s cast, range 10-23 donut
    _Weaponskill_DivineArrow6 = 50133, // Helper->self, 13.5s cast, range 23-36 donut
    _Spell_BanishgaIV1 = 50163, // Helper->players, 5.0s cast, range 6 circle
    _Spell_HolyII = 50165, // Helper/player->location, 5.0s cast, range 6 circle
    _Weaponskill_ImpartialRuling = 50145, // Boss->self, 6.3+0.7s cast, single-target
    _Weaponskill_ImpartialRuling1 = 50146, // Helper->self, 7.0s cast, range 50 180-degree cone
    _Weaponskill_ImpartialRuling2 = 50147, // Helper->self, 10.0s cast, range 50 180-degree cone
    _Weaponskill_CanonizeCoordinates = 50139, // Boss->self, no cast, single-target
    _Weaponskill_RadiantSacrament = 50140, // Boss->self, 4.0s cast, single-target
    _Weaponskill_RadiantSacrament1 = 50141, // Helper/player->self, no cast, range 10 width 10 rect
    _Weaponskill_DivineSpear = 50142, // Boss->self, 4.0+1.0s cast, single-target
    _Weaponskill_DivineSpear1 = 50143, // Helper->self, 8.0s cast, ???
    _Spell_MegaHoly = 50157, // Boss->self, 6.5+0.5s cast, single-target
    _Spell_MegaHoly1 = 50158, // Helper->players, 7.0s cast, range 6 circle
    _Spell_MegaHoly2 = 50159, // Helper->players, no cast, range 6 circle
    _Weaponskill_SacredAssembly = 50148, // Boss->self, 3.0+1.0s cast, single-target
    _Weaponskill_Activate = 50219, // Helper->self, 5.0s cast, range 3 circle
    _Weaponskill_PerfectDefense = 50149, // Boss->self, 6.0s cast, single-target
    _Weaponskill_KarmicShielding = 50686, // 4D5D->self, 4.0s cast, single-target
    _Spell_Repay = 50166, // Helper->player, no cast, single-target
    _Weaponskill_HolyFlame = 50150, // Helper->location, 3.0s cast, range 5 circle
    _Weaponskill_Shock = 50152, // 4D5D->self, 4.0s cast, range 7 circle
    _Weaponskill_CircuitShock = 50169, // 4D5D->self, 4.0s cast, range ?-18 donut
    _Weaponskill_DivineJudgment = 50153, // Boss->self, 10.0s cast, range 50 circle
    _Weaponskill_DivineArrow7 = 50125, // Boss->self, 10.0s cast, single-target
    _Weaponskill_DivineArrow8 = 50128, // Boss->self, no cast, single-target
    _Weaponskill_DivineArrow9 = 50137, // Helper->self, 3.5s cast, range 60 width 10 rect
    _Weaponskill_DivineArrow10 = 50138, // Helper/player->self, 5.5s cast, range 60 width 10 rect
    _Weaponskill_Reinforcements = 50154, // Boss->self, 2.0+1.0s cast, single-target
    _Weaponskill_Electrify = 50156, // 4D5D->self, 1.0s cast, range 18 circle
    _Spell_DivineBolt = 50160, // Boss->self/player, 5.0s cast, range 60 width 6 rect
    _Spell_DivineBolt1 = 50849, // Helper->self, no cast, range 60 width 6 rect
    _Weaponskill_ImpartialRuling3 = 50144, // Boss->self, 6.3+0.7s cast, single-target
    _Weaponskill_DivineArrow11 = 50127, // Boss->self, no cast, single-target
    _Weaponskill_DivineArrow12 = 50129, // Boss->self, no cast, single-target
}

public enum SID : uint
{
    _Gen_VulnerabilityUp = 1789, // Helper/4D5D->player, extra=0x1/0x2/0x3/0x4/0x5/0x6/0x8
    _Gen_Weakness = 43, // none->player, extra=0x0
    _Gen_Transcendent = 418, // none->player, extra=0x0
    _Gen_BrinkOfDeath = 44, // none->player, extra=0x0
    _Gen_PerfectDefense = 5376, // Boss->Boss, extra=0x0
    _Gen_PerfectDefense1 = 5377, // none->4D5D, extra=0x0
    _Gen_ = 2766, // none->4D5D, extra=0xF
    _Gen_MeatAndMead = 360, // none->player, extra=0xA
    _Gen_ProperCare = 362, // none->player, extra=0x14
    _Gen_Invincibility = 1570, // none->player, extra=0x0
}

// 25.06 -> 38.35
// 18 casts, 20 deg rotation per cast, ~0.66s per rotation, 13.3s delay from icon -> first aoe
// 01 = cw, 02 = ccw
public enum IconID : uint
{
    _Gen_Icon_m1002_north01_c0g = 691, // Boss->self
    _Gen_Icon_m1002_south01_c0g = 692, // Boss->self
    _Gen_Icon_m1002_east01_c0g = 693, // Boss->self
    _Gen_Icon_m1002_west01_c0g = 694, // Boss->self
    _Gen_Icon_m1002_north02_c0g = 695, // Boss->self
    _Gen_Icon_m1002_south02_c0g = 696, // Boss->self
    _Gen_Icon_m1002_east02_c0g = 697, // Boss->self
    _Gen_Icon_m1002_west02_c0g = 698, // Boss->self
    _Gen_Icon_m1002_lf_rs_c0g = 699, // Boss->self
    _Gen_Icon_m1002_ls_rf_c0g = 700, // Boss->self
    _Gen_Icon_target_ae_shine_s5x = 215, // player->self
    _Gen_Icon_com_share6m7s_1v = 590, // player->self
    _Gen_Icon_tank_laser_5sec_lockon_c0a1 = 471, // player->self
}

public enum TetherID : uint
{
    _Gen_Tether_chn_m1002_c0g = 414, // 4D5D->Boss
    _Gen_Tether_chn_m1002_inst_c0g = 428, // 4D5D->Boss
}

class BanishgaRaidwide(BossModule module) : Components.RaidwideCast(module, AID._Spell_BanishgaIV);

class DivineArrow(BossModule module) : Components.GenericRotatingAOE(module)
{
    public override void OnEventIcon(Actor actor, uint iconID, ulong targetID)
    {
        Angle starting;
        Angle advance;

        switch ((IconID)iconID)
        {
            case IconID._Gen_Icon_m1002_north01_c0g:
                starting = 180.Degrees();
                advance = -20.Degrees();
                break;
            case IconID._Gen_Icon_m1002_south01_c0g:
                starting = default;
                advance = -20.Degrees();
                break;
            case IconID._Gen_Icon_m1002_east01_c0g:
                starting = 90.Degrees();
                advance = -20.Degrees();
                break;
            case IconID._Gen_Icon_m1002_west01_c0g:
                starting = -90.Degrees();
                advance = -20.Degrees();
                break;
            case IconID._Gen_Icon_m1002_north02_c0g:
                starting = 180.Degrees();
                advance = 20.Degrees();
                break;
            case IconID._Gen_Icon_m1002_south02_c0g:
                starting = default;
                advance = 20.Degrees();
                break;
            case IconID._Gen_Icon_m1002_east02_c0g:
                starting = 90.Degrees();
                advance = 20.Degrees();
                break;
            case IconID._Gen_Icon_m1002_west02_c0g:
                starting = -90.Degrees();
                advance = 20.Degrees();
                break;
            default:
                return;
        }

        Sequences.Add(new(new AOEShapeCone(45, 45.Degrees()), actor.Position, starting, advance, WorldState.FutureTime(13.3f), 0.67f, 18, MaxShownAOEs: 9));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID is AID._Weaponskill_DivineArrow2 or AID._Weaponskill_DivineArrow3)
        {
            NumCasts++;
            AdvanceSequence(caster.Position, spell.Rotation, WorldState.CurrentTime);
        }
    }
}

class DivineArrowEarthquake(BossModule module) : Components.ConcentricAOEs(module, [new AOEShapeCircle(10), new AOEShapeDonut(10, 23), new AOEShapeDonut(23, 36)])
{
    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID._Weaponskill_DivineArrow4)
            AddSequence(caster.Position, Module.CastFinishAt(spell));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        var order = (AID)spell.Action.ID switch
        {
            AID._Weaponskill_DivineArrow4 => 0,
            AID._Weaponskill_DivineArrow5 => 1,
            AID._Weaponskill_DivineArrow6 => 2,
            _ => -1
        };
        AdvanceSequence(order, caster.Position, WorldState.FutureTime(2));
    }
}
class DivineArrowRect(BossModule module) : Components.GroupedAOEs(module, [AID._Weaponskill_DivineArrow9, AID._Weaponskill_DivineArrow10], new AOEShapeRect(60, 5), maxCasts: 5);

class HolyII(BossModule module) : Components.StandardAOEs(module, AID._Spell_HolyII, 6);
class BanishgaSpread(BossModule module) : Components.SpreadFromCastTargets(module, AID._Spell_BanishgaIV1, 6);

class ImpartialRuling(BossModule module) : Components.GroupedAOEs(module, [AID._Weaponskill_ImpartialRuling1, AID._Weaponskill_ImpartialRuling2], new AOEShapeCone(50, 90.Degrees()), highlightImminent: true);

// 0x14 -> 0x2C
// 8.1s delay from appearance to activation
class RadiantSacrament(BossModule module) : Components.GenericAOEs(module, AID._Weaponskill_RadiantSacrament1)
{
    readonly List<AOEInstance> _predicted = [];

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        DateTime imm = default;

        foreach (var p in _predicted.Take(17))
        {
            if (imm == default)
                imm = p.Activation;

            yield return p with { Color = p.Activation < imm.AddSeconds(0.2f) ? ArenaColor.Danger : ArenaColor.AOE };
        }
    }

    public override void OnMapEffect(byte index, uint state)
    {
        if (index is >= 0x14 and <= 0x2C && state == 0x00020001)
        {
            var ix = index - 0x14;
            var row = ix % 5;
            var col = ix / 5;
            var wd = new WDir(10 * col, 10 * row) + (Arena.Center - new WDir(20, 20));
            _predicted.Add(new(new AOEShapeRect(5, 5, 5), wd, default, WorldState.FutureTime(8.1f)));
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (spell.Action == WatchedAction)
        {
            NumCasts++;
            _predicted.RemoveAll(p => p.Origin.AlmostEqual(caster.Position, 1));
        }
    }
}

// divine spear:
// 31: N
// 32: W
// 33: E
// 34: S
// 35/36: N
// 37/38: S
// 00080004: turn -90
// 00100004: turn 90
// 00200004: turn -135
// 00400004: turn 135
// 36.00080004: N -> NE
// 36.00100004: N -> NW
// 36.00200004: N -> E
// 36.00400004: N -> W
class DivineSpear(BossModule module) : Components.GenericAOEs(module, AID._Weaponskill_DivineSpear1)
{
    readonly List<AOEInstance> _predicted = [];
    static readonly AOEShapeTriCone Triangle = new(25, 45.Degrees());

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _predicted;

    public override void OnMapEffect(byte index, uint state)
    {
        Angle fromBoss;

        switch (index)
        {
            case 0x31:
            case 0x35:
            case 0x36:
                fromBoss = 180.Degrees();
                break;
            case 0x32:
                fromBoss = -90.Degrees();
                break;
            case 0x33:
                fromBoss = 90.Degrees();
                break;
            case 0x34:
            case 0x37:
            case 0x38:
                fromBoss = default;
                break;
            default:
                return;
        }

        switch (state)
        {
            case 0x00080004:
                _predicted.Add(new(Triangle, Arena.Center + fromBoss.ToDirection() * 25, fromBoss - 135.Degrees(), WorldState.FutureTime(8)));
                break;
            case 0x00100004:
                _predicted.Add(new(Triangle, Arena.Center + fromBoss.ToDirection() * 25, fromBoss + 135.Degrees(), WorldState.FutureTime(8)));
                break;
            case 0x00200004:
                _predicted.Add(new(Triangle, Arena.Center, fromBoss - 45.Degrees(), WorldState.FutureTime(8)));
                break;
            case 0x00400004:
                _predicted.Add(new(Triangle, Arena.Center, fromBoss + 45.Degrees(), WorldState.FutureTime(8)));
                break;
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (spell.Action == WatchedAction)
        {
            NumCasts++;
            if (_predicted.Count > 0)
                _predicted.RemoveAt(0);
        }
    }
}

class MegaHoly(BossModule module) : Components.StackWithCastTargets(module, AID._Spell_MegaHoly1, 6)
{
    int _numCasts;

    public override void OnCastFinished(Actor caster, ActorCastInfo spell) { }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID is AID._Spell_MegaHoly1 or AID._Spell_MegaHoly2)
        {
            _numCasts++;
            if (_numCasts >= 3)
            {
                Stacks.Clear();
                NumFinishedStacks++;
            }
        }
    }
}

class Activate(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_Activate, 3);
class GordiusSystem(BossModule module) : Components.Adds(module, (uint)OID._Gen_GordiusSystem, 1, true);
class PerfectDefense(BossModule module) : Components.InvincibleStatus(module, (uint)SID._Gen_PerfectDefense1, priority: AIHints.Enemy.PriorityForbidden);
class HolyFlame(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_HolyFlame, 5);
class Shock(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_Shock, 7);
class CircuitShock(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_CircuitShock, new AOEShapeDonut(7, 18));

class DivineJudgment(BossModule module) : Components.RaidwideCast(module, AID._Weaponskill_DivineJudgment);

class Electrify(BossModule module) : Components.GenericAOEs(module, AID._Weaponskill_Electrify)
{
    readonly List<AOEInstance> _predicted = [];

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _predicted.Take(2);

    public override void OnTethered(Actor source, ActorTetherInfo tether)
    {
        if ((TetherID)tether.ID == TetherID._Gen_Tether_chn_m1002_inst_c0g)
            _predicted.Add(new(new AOEShapeCircle(18), source.Position, default, WorldState.FutureTime(9)));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (spell.Action == WatchedAction)
        {
            NumCasts++;
            _predicted.RemoveAll(p => p.Origin.AlmostEqual(caster.Position, 1));
        }
    }
}

class DivineBolt(BossModule module) : Components.BaitAwayIcon(module, new AOEShapeRect(60, 3), (uint)IconID._Gen_Icon_tank_laser_5sec_lockon_c0a1, AID._Spell_DivineBolt1, 5.1f);

class A33AlexanderResurrectedStates : StateMachineBuilder
{
    public A33AlexanderResurrectedStates(BossModule module) : base(module)
    {
        DeathPhase(0, SinglePhase)
            .ActivateOnEnter<BanishgaRaidwide>()
            .ActivateOnEnter<DivineArrow>()
            .ActivateOnEnter<DivineArrowEarthquake>()
            .ActivateOnEnter<DivineArrowRect>()
            .ActivateOnEnter<HolyII>()
            .ActivateOnEnter<BanishgaSpread>()
            .ActivateOnEnter<ImpartialRuling>()
            .ActivateOnEnter<RadiantSacrament>()
            .ActivateOnEnter<DivineSpear>()
            .ActivateOnEnter<MegaHoly>()
            .ActivateOnEnter<Activate>()
            .ActivateOnEnter<GordiusSystem>()
            .ActivateOnEnter<PerfectDefense>()
            .ActivateOnEnter<HolyFlame>()
            .ActivateOnEnter<Shock>()
            .ActivateOnEnter<CircuitShock>()
            .ActivateOnEnter<DivineJudgment>()
            .ActivateOnEnter<Electrify>()
            .ActivateOnEnter<DivineBolt>();
    }

    private void SinglePhase(uint id)
    {
        SimpleState(id + 0xFF0000, 10000, "???");
    }

    //private void XXX(uint id, float delay)
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1117, NameID = 14529, DevOnly = true)]
public class A33AlexanderResurrected(WorldState ws, Actor primary) : BossModule(ws, primary, new(0, 360), new ArenaBoundsSquare(25));

