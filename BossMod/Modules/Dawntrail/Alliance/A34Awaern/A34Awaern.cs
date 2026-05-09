#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace BossMod.Dawntrail.Alliance.A34AwAern;

public enum OID : uint
{
    Boss = 0x4DB6, // R4.500, x1
    _Gen_Awzdei = 0x4DB7, // R2.300, x4
    Helper = 0x233C, // R0.500, x25, Helper type
}

public enum AID : uint
{
    _AutoAttack_ = 50477, // 4DB7->player, no cast, single-target
    _AutoAttack_Attack = 45307, // Boss->player, no cast, single-target
    _Weaponskill_GlacierSplitter = 50104, // Boss->self, 2.9+0.6s cast, single-target
    _Weaponskill_GlacierSplitter1 = 50105, // Helper->self, 3.5s cast, range 60 30-degree cone
    _Ability_OpticInduration = 50106, // 4DB7->self, 3.5s cast, range 60 30-degree cone
    _Ability_StaticFilament = 50487, // 4DB7->location, 4.0s cast, range 8 circle
    _Ability_AuroralWind = 50501, // Boss->self, 5.0s cast, single-target
    _Ability_AuroralWind1 = 50502, // Helper->players, 5.0s cast, range 6 circle
    _Ability_ImpactStream = 50485, // Boss->self, 4.0s cast, single-target
    _Ability_ImpactStream1 = 50486, // Helper->self, 4.0s cast, range 80 circle
}

class Awzdei(BossModule module) : Components.Adds(module, (uint)OID._Gen_Awzdei);
class GlacierSpitter(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_GlacierSplitter1, new AOEShapeCone(60, 15.Degrees()));
class OpticInduration(BossModule module) : Components.StandardAOEs(module, AID._Ability_OpticInduration, new AOEShapeCone(60, 15.Degrees()));
class StaticFilament(BossModule module) : Components.StandardAOEs(module, AID._Ability_StaticFilament, 8);
class AuroralWind(BossModule module) : Components.SpreadFromCastTargets(module, AID._Ability_AuroralWind1, 6);
class ImpactStream(BossModule module) : Components.RaidwideCast(module, AID._Ability_ImpactStream1);

class A34AwaernStates : StateMachineBuilder
{
    public A34AwaernStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Awzdei>()
            .ActivateOnEnter<GlacierSpitter>()
            .ActivateOnEnter<OpticInduration>()
            .ActivateOnEnter<StaticFilament>()
            .ActivateOnEnter<AuroralWind>()
            .ActivateOnEnter<ImpactStream>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1117, NameID = 14838, DevOnly = true)]
public class A34Awaern(WorldState ws, Actor primary) : BossModule(ws, primary, new(-720, 720), new ArenaBoundsSquare(28));
