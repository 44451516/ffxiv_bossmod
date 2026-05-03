#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace BossMod.Dawntrail.Extreme.Ex8Enuo;

public enum OID : uint
{
    _Gen_YawningVoid = 0x4DC3, // R1.000, x2
    _Gen_ = 0x4DB8, // R5.000, x2
    _Gen_YawningVoid1 = 0x4DC2, // R1.000, x2
    Helper = 0x233C, // R0.500, x28, Helper type
    Boss = 0x4DC1, // R6.000, x1
    _Gen_Void = 0x4BFA, // R2.000, x0 (spawn during fight), Helper type
    _Gen_Void1 = 0x4BF7, // R1.000, x0 (spawn during fight), Helper type
    _Gen_Void2 = 0x4DC5, // R0.850, x0 (spawn during fight)
    _Gen_Void3 = 0x4DC6, // R1.750, x0 (spawn during fight)
    _Gen_Void4 = 0x4DC4, // R0.850, x0 (spawn during fight), Helper type
    _Gen_AggressiveShadow = 0x4DC9, // R5.000, x0 (spawn during fight)
    _Gen_SoothingShadow = 0x4DCA, // R5.000, x0 (spawn during fight)
    _Gen_LoomingShadow = 0x4DC7, // R12.500, x0 (spawn during fight)
    _Gen_ProtectiveShadow = 0x4DC8, // R5.000, x0 (spawn during fight)
    _Gen_BeaconInTheDark = 0x4DCB, // R5.000, x0 (spawn during fight)
    _Gen_Void5 = 0x4EB5, // R0.850, x0 (spawn during fight), Helper type
}

public enum AID : uint
{
    _AutoAttack_ = 49937, // Boss->player, no cast, single-target
    _Spell_Meteorain = 50049, // Boss->self, 5.0s cast, range 40 circle
    _Ability_ = 49927, // Boss->location, no cast, single-target
    _Weaponskill_NaughtGrows = 49975, // Boss->self, 7.0+1.0s cast, single-target
    _Weaponskill_NaughtGrows1 = 49977, // _Gen_YawningVoid1->self, 8.0s cast, range 40 circle
    _Weaponskill_ReturnToNothing = 49983, // _Gen_Void1->location, no cast, width 6 rect charge
    _Weaponskill_NaughtWakes = 49973, // Boss->self, 2.0+1.0s cast, single-target
    _Weaponskill_ = 49974, // _Gen_YawningVoid1->location, no cast, single-target
    _Spell_Meltdown = 50040, // Boss->self, 4.0+1.0s cast, range 40 circle
    _Spell_Meltdown1 = 50041, // Helper->location, 4.5s cast, range 5 circle
    _Spell_Meltdown2 = 50042, // Helper->players, 5.5s cast, range 5 circle
    _Weaponskill_AiryEmptiness = 50032, // Boss->self, 4.0+1.0s cast, single-target
    _Weaponskill_AiryEmptiness1 = 50034, // Helper->self, no cast, range 60 ?-degree cone
    _Weaponskill_NaughtGrows2 = 49978, // _Gen_YawningVoid1->self, 8.0s cast, range 40-60 donut
    _Weaponskill_GreatReturnToNothing = 49984, // _Gen_Void->location, no cast, width 6 rect charge
    _Weaponskill_GazeOfTheVoid = 50002, // Boss->self, 6.0+1.0s cast, single-target
    _Weaponskill_GazeOfTheVoid1 = 50005, // Helper->self, 7.0s cast, range 40 ?-degree cone
    _Weaponskill_GazeOfTheVoid2 = 50003, // Helper->self, 7.0s cast, single-target
    _Weaponskill_ViolentBurst = 50007, // _Gen_Void3->self, no cast, range 6 circle
    _Weaponskill_GazeOfTheVoid3 = 50004, // Helper->self, 7.0s cast, single-target
    _Weaponskill_BigBurst = 50008, // _Gen_Void2->self, no cast, range 60 circle
    _Weaponskill_BigBurst1 = 50009, // _Gen_Void3->self, no cast, range 60 circle
    _Weaponskill_Burst = 50006, // _Gen_Void2->self, no cast, range 5 circle
    _Weaponskill_Vacuum = 49994, // Boss->self, 2.0+1.0s cast, single-target
    _Weaponskill_SilentTorrent = 49997, // _Gen_Void4->location, 3.5s cast, single-target
    _Weaponskill_SilentTorrent1 = 49996, // _Gen_Void4->location, 3.5s cast, single-target
    _Weaponskill_SilentTorrent2 = 49995, // _Gen_Void4->location, 3.5s cast, single-target
    _Weaponskill_SilentTorrent3 = 50000, // Helper->self, 4.0s cast, range ?-19 donut
    _Weaponskill_SilentTorrent4 = 49999, // Helper->self, 4.0s cast, range ?-19 donut
    _Weaponskill_SilentTorrent5 = 49998, // Helper->self, 4.0s cast, range ?-19 donut
    _Weaponskill_Vacuum1 = 50001, // _Gen_Void4->self, 1.5s cast, range 7 circle
    _Weaponskill_DenseEmptiness = 50033, // Boss->self, 4.0+1.0s cast, single-target
    _Weaponskill_DenseEmptiness1 = 50035, // Helper->self, no cast, range 60 ?-degree cone
    _Spell_DeepFreeze = 50043, // Boss->self, 5.0+1.0s cast, range 40 circle
    _Spell_DeepFreeze1 = 50044, // Helper->players, 6.0s cast, range 40 circle
    _Weaponskill_AllForNaught = 50010, // Boss->self, 5.0s cast, single-target
    _Weaponskill_LoomingEmptiness = 50011, // _Gen_LoomingShadow->self, 5.0s cast, single-target
    _Weaponskill_LoomingEmptiness1 = 49982, // Helper->self, 6.0s cast, range 100 circle
    _Weaponskill_LoomingEmptiness2 = 49369, // Helper->self, 6.0s cast, range 8 circle
    _Weaponskill_VoidalTurbulence = 50036, // _Gen_LoomingShadow->self, 6.0+1.0s cast, single-target
    _Weaponskill_EmptyShadow = 50013, // Helper->self, 7.0s cast, range 6 circle
    _Weaponskill_VoidalTurbulence1 = 50038, // Helper->self, no cast, range 60 ?-degree cone
    _Weaponskill_1 = 50012, // _Gen_SoothingShadow/_Gen_AggressiveShadow/_Gen_ProtectiveShadow->self, no cast, single-target
    _Weaponskill_BigBurst2 = 50014, // Helper->self, no cast, range 60 circle
    _AutoAttack_1 = 50753, // _Gen_SoothingShadow->player, no cast, single-target
    _AutoAttack_2 = 50752, // _Gen_AggressiveShadow->player, no cast, single-target
    _AutoAttack_3 = 50751, // _Gen_ProtectiveShadow->player, no cast, single-target
    _AutoAttack_4 = 50016, // _Gen_LoomingShadow->player, no cast, single-target
    _Weaponskill_2 = 50015, // Helper->player, no cast, single-target
    _Weaponskill_DrainTouch = 50018, // _Gen_ProtectiveShadow->self, 5.0s cast, single-target
    _Weaponskill_DrainTouch1 = 50019, // Helper->player, no cast, single-target
    _Weaponskill_3 = 49938, // _Gen_AggressiveShadow/_Gen_SoothingShadow/_Gen_ProtectiveShadow/_Gen_LoomingShadow->self, no cast, single-target
    _Weaponskill_DemonEye = 50022, // _Gen_AggressiveShadow->self, 4.0+1.0s cast, single-target
    _Weaponskill_DemonEye1 = 50023, // Helper->self, 5.0s cast, range 20 circle
    _Weaponskill_CurseOfTheFlesh = 50024, // _Gen_SoothingShadow->self, 2.0+1.0s cast, single-target
    _Weaponskill_CurseOfTheFlesh1 = 50025, // Helper->player, 3.0s cast, single-target
    _Weaponskill_WeightOfNothing = 50021, // Helper->player, 5.0s cast, range 100 width 8 rect
    _Weaponskill_WeightOfNothing1 = 50020, // _Gen_ProtectiveShadow->self, 4.0+1.0s cast, single-target
    _Weaponskill_Nothingness = 50017, // _Gen_AggressiveShadow/_Gen_SoothingShadow/_Gen_ProtectiveShadow->self, 3.0s cast, range 100 width 4 rect
    _Weaponskill_LightlessWorld = 50029, // Boss->self, 10.0s cast, single-target
    _Weaponskill_LightlessWorld1 = 50030, // Helper->self, no cast, range 40 circle
    _Weaponskill_LightlessWorld2 = 50031, // Helper->self, no cast, range 40 circle
    _Weaponskill_Almagest = 49972, // Boss->self, 5.0s cast, range 40 circle
    _Weaponskill_NaughtGrows3 = 49976, // Boss->self, 7.0+1.0s cast, single-target
    _Weaponskill_NaughtGrows4 = 49979, // Helper->self, 8.0s cast, range 12 circle
    _Weaponskill_PassageOfNaught = 49985, // _Gen_YawningVoid1->self, 7.0s cast, range 80 width 16 rect
    _Weaponskill_PassageOfNaught1 = 49987, // Helper->self, 6.0s cast, range 80 width 16 rect
    _Weaponskill_PassageOfNaught2 = 49986, // _Gen_YawningVoid->self, 6.0s cast, range 80 width 16 rect
    _Spell_ShroudedHoly = 50045, // Boss->self, 5.0+1.0s cast, single-target
    _Spell_ShroudedHoly1 = 50046, // Helper->players, 6.0s cast, range 6 circle
    _Weaponskill_NaughtGrows5 = 49980, // Helper->self, 8.0s cast, range 6-40 donut
    _Weaponskill_DimensionZero = 50047, // Boss->self, 5.0s cast, single-target
    _Weaponskill_DimensionZero1 = 50048, // Boss->self, no cast, range 60 width 8 rect
    _Weaponskill_NaughtHunts = 49992, // Boss->self, 6.0+1.0s cast, single-target
    _Weaponskill_EndlessChase = 48475, // _Gen_Void5->self, 6.0s cast, range 6 circle
    _Weaponskill_EndlessChase1 = 49993, // _Gen_Void5->location, no cast, range 6 circle
    _Weaponskill_SelfDestruct = 50799, // _Gen_AggressiveShadow->self, 5.0s cast, range 60 circle
}

public enum SID : uint
{
    _Gen_MagicVulnerabilityUp = 2941, // _Gen_Void1/Helper/_Gen_Void/_Gen_Void3/_Gen_Void2->player, extra=0x0
    _Gen_ChainsOfCondemnation = 4562, // Boss->player, extra=0x0
    _Gen_Weakness = 43, // none->player, extra=0x0
    _Gen_Transcendent = 418, // none->player, extra=0x0
    _Gen_VulnerabilityUp = 1789, // Helper/_Gen_YawningVoid1/_Gen_YawningVoid/_Gen_Void4/_Gen_Void5->player, extra=0x1/0x2/0x3/0x4/0x5/0x6/0x8
    _Gen_ = 2234, // none->_Gen_Void2/_Gen_Void3, extra=0x58/0x4B
    _Gen_BrinkOfDeath = 44, // none->player, extra=0x0
    _Gen_DeepFreeze = 4150, // Boss->player, extra=0x0
    _Gen_FreezingUp = 3523, // Boss->player, extra=0x0
    _Gen_SustainedDamage = 4149, // _Gen_Void2->player, extra=0x1/0x2
    _Gen_DirectionalDisregard = 3808, // none->Boss, extra=0x0
    _Gen_1 = 2056, // none->_Gen_SoothingShadow/_Gen_ProtectiveShadow/_Gen_AggressiveShadow/_Gen_LoomingShadow, extra=0x46B
    _Gen_Unbecoming = 4882, // none->player, extra=0x0
    _Gen_DarkResistanceDownII = 3323, // Helper->player, extra=0x0
    _Gen_DamageDown = 2911, // Helper->player, extra=0x0
    _Gen_GauntletTaken = 5357, // none->player, extra=0x0
    _Gen_GauntletThrown = 5365, // none->_Gen_ProtectiveShadow, extra=0x0
    _Gen_GauntletTaken1 = 5362, // none->player, extra=0x0
    _Gen_GauntletTaken2 = 5363, // none->player, extra=0x0
    _Gen_GauntletTaken3 = 5364, // none->player, extra=0x0
    _Gen_GauntletThrown1 = 5372, // none->_Gen_AggressiveShadow, extra=0x0
    _Gen_GauntletThrown2 = 5371, // none->_Gen_AggressiveShadow, extra=0x0
    _Gen_GauntletThrown3 = 5370, // none->_Gen_AggressiveShadow, extra=0x0
    _Gen_Petrification = 3007, // Helper->player, extra=0x0
    _Gen_GauntletTaken4 = 5359, // none->player, extra=0x0
    _Gen_GauntletTaken5 = 5360, // none->player, extra=0x0
    _Gen_GauntletTaken6 = 5358, // none->player, extra=0x0
    _Gen_GauntletThrown4 = 5366, // none->_Gen_ProtectiveShadow, extra=0x0
    _Gen_GauntletThrown5 = 5367, // none->_Gen_SoothingShadow, extra=0x0
    _Gen_GauntletThrown6 = 5368, // none->_Gen_SoothingShadow, extra=0x0
    _Gen_GauntletTaken7 = 5361, // none->player, extra=0x0
    _Gen_GauntletThrown7 = 5369, // none->_Gen_AggressiveShadow, extra=0x0
    _Gen_QuantumEntanglement = 4884, // none->player, extra=0x0
    _Gen_QuantumNullification = 4883, // none->_Gen_SoothingShadow, extra=0x0
    _Gen_Disease = 3943, // Helper->player, extra=0x32
    _Gen_InEvent = 1268, // none->player, extra=0x0
}

public enum IconID : uint
{
    _Gen_Icon_z5fd_loc01_t0a1 = 701, // Boss->player, small ball
    _Gen_Icon_z5fd_loc02_t0a1 = 702, // Boss->player, big ball
    _Gen_Icon_m0742trg_b1t1 = 327, // player->self
    _Gen_Icon_com_trg07_0a1 = 721, // player->self
    _Gen_Icon_tank_laser_5sec_lockon_c0a1 = 471, // player->self
    _Gen_Icon_com_share3_6s0p = 318, // player->self
    _Gen_Icon_share_laser_5s_small_c0a1 = 719, // Boss->player
    _Gen_Icon_com_trg06_0v = 172, // player->self
}

public enum TetherID : uint
{
    _Gen_Tether_chn_z5fd19_0a1 = 430, // player->Boss, just a line?
    _Gen_Tether_chn_z5fd09_0a1 = 393, // _Gen_->Boss, circle, target -> source
    _Gen_Tether_chn_z5fd11_0a1 = 395, // _Gen_->Boss, donut, target -> source
    _Gen_Tether_chn_z5fd16_0a1 = 406, // _Gen_Void2/_Gen_Void3->Boss, purple clock (slow)
    _Gen_Tether_chn_z5fd17_0a1 = 407, // _Gen_Void2/_Gen_Void3->Boss, yellow clock (fast)
    _Gen_Tether_chn_tergetfix1f = 284, // _Gen_ProtectiveShadow/_Gen_AggressiveShadow/_Gen_SoothingShadow->player, another line
    _Gen_Tether_chn_z5fd10_0a1 = 394, // _Gen_->Boss, circle, source -> target
    _Gen_Tether_chn_z5fd12_0a1 = 396, // _Gen_->Boss, donut, source -> target
    _Gen_Tether_chn_z5fd14_0a1 = 404, // _Gen_Void5->player, black line
    _Gen_Tether_chn_z5fd15_0a1 = 405, // player->player, chaser pass
}

class Meteorain(BossModule module) : Components.RaidwideCast(module, AID._Spell_Meteorain);
class NaughtGrowsCounter(BossModule module) : Components.CastCounterMulti(module, [AID._Weaponskill_NaughtGrows1, AID._Weaponskill_NaughtGrows2]);
class NaughtGrowsDonut(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_NaughtGrows2, new AOEShapeDonut(40, 60));
class NaughtGrowsCircle(BossModule module) : Components.StandardAOEs(module, AID._Weaponskill_NaughtGrows1, 40);

// 21.34 -> 30.46
class ReturnToNothing(BossModule module) : Components.UntelegraphedBait(module)
{
    public override void OnEventIcon(Actor actor, uint iconID, ulong targetID)
    {
        switch ((IconID)iconID)
        {
            case IconID._Gen_Icon_z5fd_loc01_t0a1:
                CurrentBaits.Add(new(default, BitMask.Build(Raid.FindSlot(targetID)), new AOEShapeRect(15, 3), WorldState.FutureTime(9.1f), stackSize: 4, count: 2));
                break;
            case IconID._Gen_Icon_z5fd_loc02_t0a1:
                CurrentBaits.Add(new(default, BitMask.Build(Raid.FindSlot(targetID)), new AOEShapeRect(15, 3), WorldState.FutureTime(9.1f), stackSize: 8, count: 1));
                break;
        }
    }

    public override void Update()
    {
        var remove = new BitMask();
        for (var i = 0; i < CurrentBaits.Count; i++)
        {
            var (_, target) = Raid.WithSlot().IncludedInMask(CurrentBaits[i].Targets).First();
            if (target.IsDead)
            {
                remove.Set(i);
                continue;
            }
            var angle = Module.PrimaryActor.AngleTo(target);
            var src = target.Position + angle.ToDirection() * 7;
            var gap = (target.Position - Module.PrimaryActor.Position).Length();
            ref var bait = ref CurrentBaits.Ref(i);
            bait.Origin = src;
            bait.Shape = new AOEShapeRect(7 + gap, 3);
        }
        foreach (var bit in remove.SetBits().Reverse())
            CurrentBaits.RemoveAt(bit);

        base.Update();
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID is AID._Weaponskill_GreatReturnToNothing or AID._Weaponskill_ReturnToNothing)
        {
            CurrentBaits.Clear();
            NumCasts++;
        }
    }
}

class ChainsOfCondemnation(BossModule module) : Components.StayMove(module)
{
    public bool Active { get; private set; }
    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID == SID._Gen_ChainsOfCondemnation && Raid.TryFindSlot(actor, out var slot))
        {
            Active = true;
            SetState(slot, new(Requirement.NoMove, WorldState.CurrentTime));
        }
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID == SID._Gen_ChainsOfCondemnation && Raid.TryFindSlot(actor, out var slot))
            ClearState(slot);
    }
}
class MeltdownBaited(BossModule module) : Components.StandardAOEs(module, AID._Spell_Meltdown1, 5);
class MeltdownSpread(BossModule module) : Components.SpreadFromCastTargets(module, AID._Spell_Meltdown2, 5);

class Emptiness(BossModule module) : Components.UntelegraphedBait(module)
{
    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID._Weaponskill_AiryEmptiness)
        {
            foreach (var (i, p) in Raid.WithSlot().OrderByDescending(d => d.Item2.Class.IsSupport()).Take(4))
                CurrentBaits.Add(new(Module.PrimaryActor.Position, BitMask.Build(i), new AOEShapeCone(60, 30.Degrees()), Module.CastFinishAt(spell, 1), count: 4, stackSize: 2));
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        switch ((AID)spell.Action.ID)
        {
            case AID._Weaponskill_AiryEmptiness1:
                NumCasts++;
                CurrentBaits.Clear();
                break;
        }
    }
}

class Ex8EnuoStates : StateMachineBuilder
{
    public Ex8EnuoStates(BossModule module) : base(module)
    {
        DeathPhase(0, P1);
    }

    void P1(uint id)
    {
        Meteorain(id, 9.2f);
        NaughtGrows(id + 0x100, 7.5f);
        Cast(id + 0x150, AID._Weaponskill_NaughtWakes, 5.2f, 2);
        Meltdown(id + 0x200, 2.2f);

        Timeout(id + 0xFF0000, 10000, "???")
            .ActivateOnEnter<Emptiness>();
    }

    void Meteorain(uint id, float delay)
    {
        Cast(id, AID._Spell_Meteorain, delay, 5, "Raidwide")
            .ActivateOnEnter<Meteorain>()
            .DeactivateOnExit<Meteorain>();
    }

    void NaughtGrows(uint id, float delay)
    {
        CastStartMulti(id, [AID._Weaponskill_NaughtGrows, AID._Weaponskill_NaughtGrows3], delay)
            .ActivateOnEnter<NaughtGrowsCounter>()
            .ActivateOnEnter<NaughtGrowsDonut>()
            .ActivateOnEnter<NaughtGrowsCircle>()
            .ActivateOnEnter<ReturnToNothing>();

        ComponentCondition<NaughtGrowsCounter>(id + 1, 8, n => n.NumCasts > 0, "In/out")
            .DeactivateOnExit<NaughtGrowsCounter>()
            .DeactivateOnExit<NaughtGrowsDonut>()
            .DeactivateOnExit<NaughtGrowsCircle>();

        ComponentCondition<ReturnToNothing>(id + 2, 0.9f, r => r.NumCasts > 0, "Wild charge")
            .DeactivateOnExit<ReturnToNothing>();
    }

    void Meltdown(uint id, float delay)
    {
        Cast(id, AID._Spell_Meltdown, delay, 4)
            .ActivateOnEnter<ChainsOfCondemnation>()
            .ActivateOnEnter<MeltdownBaited>()
            .ActivateOnEnter<MeltdownSpread>();

        ComponentCondition<ChainsOfCondemnation>(id + 0x10, 1, c => c.Active, "Stop moving");
        ComponentCondition<MeltdownBaited>(id + 0x11, 4.5f, m => m.NumCasts > 0, "Puddles")
            .DeactivateOnExit<MeltdownBaited>();
        ComponentCondition<MeltdownSpread>(id + 0x12, 1, m => m.NumFinishedSpreads > 0, "Spreads")
            .DeactivateOnExit<MeltdownSpread>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1116, NameID = 14749, DevOnly = true)]
public class Ex8Enuo(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsCircle(20));
