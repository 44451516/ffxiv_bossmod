﻿using System.Collections.Generic;

namespace BossMod.RealmReborn.Dungeon.D08Qarn.D082TempleGuardian
{
    public enum OID : uint
    {
        Boss = 0x6DB, // x1
        GolemSoulstone = 0x7FA, // x1, Unknown type, and more spawn during fight
    };

    public enum AID : uint
    {
        AutoAttack = 872, // Boss->player, no cast
        BoulderClap = 1417, // Boss->self, 2.5s cast, range 14.2 120-degree cone aoe
        TrueGrit = 1418, // Boss->self, 3.0s cast, range 14.2 120-degree cone aoe
        Rockslide = 1419, // Boss->self, 2.5s cast, range 16.2 width 8 rect aoe
        StoneSkull = 1416, // Boss->player, no cast, random single-target
        Obliterate = 680, // Boss->self, 2.0s cast, range 6? ??? aoe
    };

    class BoulderClap : Components.SelfTargetedAOEs
    {
        public BoulderClap() : base(ActionID.MakeSpell(AID.BoulderClap), new AOEShapeCone(14.2f, 60.Degrees())) { }
    }

    class TrueGrit : Components.SelfTargetedAOEs
    {
        public TrueGrit() : base(ActionID.MakeSpell(AID.TrueGrit), new AOEShapeCone(14.2f, 60.Degrees())) { }
    }

    class Rockslide : Components.SelfTargetedAOEs
    {
        public Rockslide() : base(ActionID.MakeSpell(AID.Rockslide), new AOEShapeRect(16.2f, 4)) { }
    }

    class D082TempleGuardianStates : StateMachineBuilder
    {
        public D082TempleGuardianStates(BossModule module) : base(module)
        {
            TrivialPhase()
                .ActivateOnEnter<BoulderClap>()
                .ActivateOnEnter<TrueGrit>()
                .ActivateOnEnter<Rockslide>();
        }
    }

    public class D082TempleGuardian : BossModule
    {
        private List<Actor> _soulstone;

        public D082TempleGuardian(WorldState ws, Actor primary) : base(ws, primary, new ArenaBoundsCircle(new(50, -10), 15))
        {
            _soulstone = Enemies(OID.GolemSoulstone);
        }

        public override bool FillTargets(BossTargets targets, int pcSlot)
        {
            if (!targets.AddIfValid(_soulstone))
                targets.AddIfValid(PrimaryActor);
            return true;
        }
    }
}