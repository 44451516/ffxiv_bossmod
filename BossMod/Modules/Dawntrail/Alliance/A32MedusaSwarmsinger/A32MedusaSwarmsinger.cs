#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace BossMod.Dawntrail.Alliance.A32Medusa;

public enum OID : uint
{
    Boss = 0x4DA6, // R1.800, x1
    _Gen_Acrolith = 0x4DA4, // R3.000, x3
    _Gen_LamiaRover = 0x4DA3, // R1.100, x6
    _Gen_Acrolith1 = 0x4DB5, // R3.000, x3
    _Gen_LamiaRover1 = 0x4DB4, // R1.100, x2
    _Gen_LamiaJaeger = 0x4DA5, // R1.100, x1 (spawn during fight)
    _Gen_QutrubForayer = 0x4DA7, // R1.200, x2
    Helper = 0x233C, // R0.500, x2 (spawn during fight), Helper type
    _Gen_AssaultBhoot = 0x4DAA, // R1.170, x0 (spawn during fight)
    _Gen_NemeanLion = 0x4DA9, // R4.400, x0 (spawn during fight)
    _Gen_LamiaNo2 = 0x4DA8, // R1.650, x0 (spawn during fight)
    _Gen_MedusaSwarmsinger = 0x4DAB, // R2.250, x0 (spawn during fight)
}

public enum AID : uint
{
    _AutoAttack_Attack = 872, // 4DB5/4DAA->player/4DB1/4DB2/4DAD, no cast, single-target
    _Weaponskill_Earthshatter = 50085, // 4DA4->self, 4.0s cast, range 8 circle
    _AutoAttack_Attack1 = 870, // 4DB4/Boss/4DA7/4DA9/4DAB->player/4DAF/4DAC, no cast, single-target
    _Weaponskill_TranscendentShot = 50086, // 4DA5->self, 5.0s cast, range 60 width 5 rect
    _Weaponskill_LeapingCleave = 50087, // Boss->location, 3.5+1.5s cast, single-target
    _Weaponskill_ = 50639, // Helper->4D66, 5.0s cast, single-target
    _Weaponskill_LeapingCleave1 = 50481, // Helper->location, 5.0s cast, range 40 circle
    _AutoAttack_Attack2 = 873, // 4DA5/4DA8->player, no cast, single-target
    _Weaponskill_FeralLunge = 50088, // 4DA7->location, 2.0+2.0s cast, single-target
    _Weaponskill_FeralLunge1 = 50482, // Helper->location, 4.0s cast, range 10 circle
    _Weaponskill_WhirlingSlash = 50659, // Boss->self, 3.0s cast, range 6 circle
    _Ability_Perdition = 50094, // 4DAA->self, 4.0s cast, range 9 circle
    _Ability_Tourbillion = 50091, // 4DA9->self, 5.0s cast, range 40 width 50 rect
    _Weaponskill_PinningShot = 50089, // 4DA8->self, 7.0s cast, single-target
    _Weaponskill_PinningShot1 = 50090, // Helper->player, 7.0s cast, range 13 circle
    _Ability_FulminationKhalkeos = 50092, // 4DA9->self, 4.0s cast, range 70 circle
    _Ability_DanceToDust = 50096, // Helper->self, 5.0s cast, range 7 circle
    _Ability_DanceToDust1 = 50095, // 4DAB->self, 5.0s cast, single-target
    _Ability_DanceToDust2 = 50097, // Helper->self, no cast, range 7 circle
    _Weaponskill_RightShadowSlash = 50098, // 4DAB->self, 5.0s cast, range 60 180-degree cone
    _Ability_BellowingGrunt = 50103, // 4DAB->self, 4.0s cast, range 60 circle
    _Weaponskill_Disregard = 50100, // 4DAB->self, 4.0s cast, range 60 circle
    _Weaponskill_Disregard1 = 50101, // Helper->self, 4.0s cast, range 55 width 10 rect
    _Ability_Petrifaction = 50102, // 4DAB->self, 5.0s cast, range 60 circle
    _Weaponskill_LeftShadowSlash = 50099, // 4DAB->self, 5.0s cast, range 60 180-degree cone
}

class Earthshatter(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_Earthshatter, 8);
class TranscendentShot(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_TranscendentShot, new AOEShapeRect(60, 2.5f), maxCasts: 4);
class LeapingCleave(BossModule module) : Components.KnockbackFromCastTarget(module, AID._Weaponskill_LeapingCleave1, 22)
{
    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        foreach (var src in Sources(slot, actor))
            if (!IsImmune(slot, src.Activation))
            {
                var center = Arena.Center;
                hints.AddForbiddenZone(p =>
                {
                    var kb = (p - center).Normalized() * 22;
                    return !(p + kb).InRect(center, default(Angle), 20, 20, 25);
                }, src.Activation);
            }
    }
}
class FeralLunge(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_FeralLunge1, 10);
class WhirlingSlash(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_WhirlingSlash, 6);
class Perdition(BossModule module) : Components.StandardAOEs(module, AID._Ability_Perdition, 9);
class Tourbillion(BossModule module) : Components.StandardAOEs(module, AID._Ability_Tourbillion, new AOEShapeRect(40, 25));
class PinningShot(BossModule module) : Components.BaitAwayCast(module, AID._Weaponskill_PinningShot1, new AOEShapeCircle(13), centerAtTarget: true);
class FulminationKhalkeos(BossModule module) : Components.RaidwideCast(module, AID._Ability_FulminationKhalkeos);
class DanceToDust(BossModule module) : Components.Exaflare(module, new AOEShapeCircle(7))
{
    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID._Ability_DanceToDust)
            Lines.Add(new()
            {
                Next = caster.Position,
                Advance = caster.Rotation.ToDirection() * 8,
                NextExplosion = Module.CastFinishAt(spell),
                TimeToMove = 2,
                ExplosionsLeft = caster.Rotation.AlmostEqual(default, 0.1f) || caster.Rotation.AlmostEqual(180.Degrees(), 0.1f) ? 2 : 3,
                MaxShownExplosions = 3
            });
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID is AID._Ability_DanceToDust or AID._Ability_DanceToDust2)
        {
            var ix = Lines.FindIndex(l => l.Next.AlmostEqual(caster.Position, 1));
            if (ix >= 0)
                AdvanceLine(Lines[ix], caster.Position);
        }
    }
}
class ShadowSlash(BossModule module) : Components.GroupedAOEs(module, [AID._Weaponskill_RightShadowSlash, AID._Weaponskill_LeftShadowSlash], new AOEShapeCone(60, 90.Degrees()));
class BellowingGrunt(BossModule module) : Components.RaidwideCast(module, AID._Ability_BellowingGrunt);
class Disregard(BossModule module) : Components.RaidwideCast(module, AID._Weaponskill_Disregard);
class DisregardRect(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_Disregard1, new AOEShapeRect(55, 5), highlightImminent: true);
class Petrifaction(BossModule module) : Components.CastGaze(module, AID._Ability_Petrifaction);

class A32MedusaSwarmsingerStates : StateMachineBuilder
{
    public A32MedusaSwarmsingerStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Earthshatter>()
            .ActivateOnEnter<TranscendentShot>()
            .ActivateOnEnter<LeapingCleave>()
            .ActivateOnEnter<FeralLunge>()
            .ActivateOnEnter<WhirlingSlash>()
            .ActivateOnEnter<Perdition>()
            .ActivateOnEnter<Tourbillion>()
            .ActivateOnEnter<PinningShot>()
            .ActivateOnEnter<FulminationKhalkeos>()
            .ActivateOnEnter<DanceToDust>()
            .ActivateOnEnter<ShadowSlash>()
            .ActivateOnEnter<BellowingGrunt>()
            .ActivateOnEnter<Disregard>()
            .ActivateOnEnter<DisregardRect>()
            .ActivateOnEnter<Petrifaction>()
            .Raw.Update = () => module.Enemies(OID._Gen_MedusaSwarmsinger).Any(e => e.IsDeadOrDestroyed);
    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1117, NameID = 14834, DevOnly = true)]
public class A32MedusaSwarmsinger(WorldState ws, Actor primary) : BossModule(ws, primary, new(721, 720), new ArenaBoundsRect(25, 20))
{
    protected override bool CheckPull() => PrimaryActor.InCombat;

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actors(WorldState.Actors.Where(a => !a.IsAlly), ArenaColor.Enemy);
    }
}
