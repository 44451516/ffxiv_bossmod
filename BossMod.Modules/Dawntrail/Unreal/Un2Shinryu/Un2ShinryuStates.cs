namespace BossMod.Dawntrail.Unreal.Un2Shinryu;

class Un2ShinryuStates : StateMachineBuilder
{
    public Un2ShinryuStates(BossModule module) : base(module)
    {
        SimplePhase(0, Phase1, "P1")
            .ActivateOnEnter<ArenaChanges>()
            .Raw.Update = () => Module.PrimaryActor.IsDeadOrDestroyed || Module.PrimaryActor.CastInfo?.IsSpell(AID.DarkMatter) == true;
        SimplePhase(1, Phase2, "P2")
            .Raw.Update = () => Module.PrimaryActor.IsDeadOrDestroyed || Module.FindComponent<ArenaChanges>()?.Shattered == true;
        SimplePhase(2, Phase3, "P3")
            .Raw.Update = () => Module.PrimaryActor.IsDeadOrDestroyed || Module.PrimaryActor.CastInfo?.IsSpell(AID.TidalWaveEnrage) == true;
        DeathPhase(3, Phase4);
    }

    private void Phase1(uint id)
    {
        ActorCast(id, () => Module.PrimaryActor, AID.EarthenFuryVisual, 10.2f, 10, true, "全屏伤害 + 地板破坏")
            .ActivateOnEnter<EarthenFury>()
            .ActivateOnEnter<EarthenFuryTile>()
            .ActivateOnEnter<TidalWave>()
            .ActivateOnEnter<WaterPuddles>()
            .ActivateOnEnter<Paralysis>()
            .ActivateOnEnter<BurningChains>()
            .ActivateOnEnter<Dragonflight>()
            .ActivateOnEnter<TailMarker>()
            .ActivateOnEnter<TailSlap>()
            .ActivateOnEnter<IcicleAdds>()
            .ActivateOnEnter<Spikesicle>()
            .ActivateOnEnter<SpikesicleKnockback>()
            .ActivateOnEnter<Levinbolt>()
            .ActivateOnEnter<Hypernova>()
            .ActivateOnEnter<IceStorm>()
            .ActivateOnEnter<Dragonfist>()
            .ActivateOnEnter<AkhMorn>()
            .ActivateOnEnter<AkhRhai>()
            .ActivateOnEnter<AkhRhaiBait>()
            .ActivateOnEnter<Hellfire>()
            .ActivateOnEnter<JudgmentBolt>()
            .ActivateOnEnter<EarthBreath>()
            .ActivateOnEnter<DiamondDust>()
            .ActivateOnEnter<ThinIce>()
            .ActivateOnEnter<AerialBlast>()
            .ActivateOnEnter<SuperCyclone>()
            .ActivateOnEnter<GyreCharge>()
            .ActivateOnEnter<TargetPriority>()
            .ActivateOnEnter<DarkMatterPush>()
            .SetHint(StateMachine.StateHint.Raidwide);
        Cast(id + 0x10, AID.TidalWaveVisual, 13.2f, 10, "击退")
            .SetHint(StateMachine.StateHint.Knockback);
        ActorCast(id + 0x20, LeftWing, AID.SummonIcicle, 13.1f, 4, false, "冰柱");
        ActorCastMulti(id + 0x30, RightWing, [AID.LevinboltVisual, AID.HypernovaVisual], 10.1f, 8, false, "分散／分摊");
        ComponentCondition<IceStorm>(id + 0x40, 15.4f, comp => comp.NumCasts > 0, "全屏伤害", 4)
            .SetHint(StateMachine.StateHint.Raidwide);
        Cast(id + 0x50, AID.AkhMorn, 8.9f, 4, "分摊死刑")
            .SetHint(StateMachine.StateHint.Tankbuster);
        CastMulti(id + 0x60, [AID.HellfireVisual, AID.JudgmentBoltVisual], 5.3f, 10, "全屏伤害（水圈）")
            .SetHint(StateMachine.StateHint.Raidwide);
        ActorCast(id + 0x70, RightWing, AID.LevinboltVisual, 18.2f, 8, false, "分散");
        ActorCast(id + 0x80, LeftWing, AID.IceStormVisual, 9.0f, 6, false);
        ComponentCondition<IceStorm>(id + 0x82, 0.6f, comp => comp.NumCasts > 1, "全屏伤害")
            .SetHint(StateMachine.StateHint.Raidwide);
        Cast(id + 0x90, AID.EarthBreath, 9.5f, 6, "诱导");
        Cast(id + 0xA0, AID.AkhMorn, 3.4f, 4, "分摊死刑")
            .SetHint(StateMachine.StateHint.Tankbuster);
        ActorCast(id + 0xB0, LeftWing, AID.IceStormVisual, 0, 6, false);
        ComponentCondition<IceStorm>(id + 0xB2, 0.6f, comp => comp.NumCasts > 2, "全屏伤害")
            .SetHint(StateMachine.StateHint.Raidwide);
        Cast(id + 0xC0, AID.DiamondDustVisual, 3.7f, 10, "全屏伤害 + 薄冰")
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<Dragonflight>(id + 0xD0, 17.3f, comp => comp.NumWaves > 0, "灵龙（治疗）");
        ActorCast(id + 0xE0, LeftWing, AID.SummonIcicle, 39.1f, 4, false, "冰柱");
        Cast(id + 0xF0, AID.AkhMorn, 0, 4, "分摊死刑")
            .SetHint(StateMachine.StateHint.Tankbuster);
        Cast(id + 0x100, AID.AerialBlastVisual, 9.3f, 10, "击退")
            .SetHint(StateMachine.StateHint.Knockback);
        Cast(id + 0x110, AID.EarthBreath, 21.1f, 6, "诱导");
        ComponentCondition<IceStorm>(id + 0x120, 3.7f, comp => comp.NumCasts > 3, "全屏伤害")
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<GyreCharge>(id + 0x130, 7.0f, comp => comp.Casters.Count > 0, "回旋冲锋", 3);
        ActorCast(id + 0x140, RightWing, AID.HypernovaVisual, 13.3f, 8, false, "分摊");
        Cast(id + 0x150, AID.AkhMorn, 8.4f, 4, "分摊死刑")
            .SetHint(StateMachine.StateHint.Tankbuster);
        ActorCastStart(id + 0x160, RightWing, AID.LevinboltVisual, 7.6f);
        CastStart(id + 0x170, AID.TidalWaveVisual, 4.0f);
        ActorCastEnd(id + 0x171, RightWing, 3.7f, false, "分散");
        CastEnd(id + 0x172, 6.0f, "击退")
            .SetHint(StateMachine.StateHint.Knockback);
        SimpleState(id + 0xFF0000, 100, "???");
    }

    private void Phase2(uint id)
    {
        CastEnd(id, 3, "全屏伤害")
            .ActivateOnEnter<DarkMatter>()
            .ActivateOnEnter<Touchdown>()
            .ActivateOnEnter<TouchdownSpot>()
            .ActivateOnEnter<TailSpitKnockback>()
            .ActivateOnEnter<Cocoons>()
            .ActivateOnEnter<DragonAdds>()
            .ActivateOnEnter<MeteorImpactMarker>()
            .ActivateOnEnter<CocoonMarker>()
            .ActivateOnEnter<Fireball>()
            .ActivateOnEnter<BlazingTrail>()
            .ActivateOnEnter<SpikedTail>()
            .ActivateOnEnter<DeathSentence>()
            .ActivateOnEnter<Protostar>()
            .ActivateOnEnter<TailSpit>()
            .ActivateOnEnter<TailSpitCenter>()
            .ActivateOnEnter<AetherialRay>()
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<Touchdown>(id + 0x20, 38.4f, comp => comp.NumCasts > 0, "距离衰减", 2)
            .SetHint(StateMachine.StateHint.DowntimeEnd);
        ComponentCondition<CocoonMarker>(id + 0x30, 46.5f, comp => comp.Active, "陨石点名", 3);
        Cast(id + 0x40, AID.Protostar, 31.0f, 6, "全屏伤害")
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<TailSpit>(id + 0x50, 9.4f, comp => comp.NumCasts > 0, "全屏伤害", 2)
            .SetHint(StateMachine.StateHint.Raidwide);
        Timeout(id + 0x60, 22.2f, "地板破碎");
    }

    private void Phase3(uint id)
    {
        CastStart(id, AID.TeraSlash, 18.2f)
            .ActivateOnEnter<TeraSlash>()
            .ActivateOnEnter<AtomicRay>()
            .ActivateOnEnter<AetherialRay>()
            .ActivateOnEnter<Wormwail>()
            .ActivateOnEnter<DoomPuddle>()
            .ActivateOnEnter<BenightingBreath>()
            .ActivateOnEnter<IceStormP3>()
            .ActivateOnEnter<IceStormFreeze>()
            .ActivateOnEnter<HypernovaP3>()
            .ActivateOnEnter<LevinboltP3Hint>()
            .ActivateOnEnter<LevinboltP3>();
        CastEnd(id + 1, 4, "死刑")
            .SetHint(StateMachine.StateHint.Tankbuster);
        ComponentCondition<AtomicRay>(id + 0x10, 6.2f, comp => comp.Active, "", 2);
        ComponentCondition<AtomicRay>(id + 0x11, 7.2f, comp => !comp.Active, "分摊", 2);
        ActorCast(id + 0x20, LeftWingP3, AID.IceStormVisualP3, 7.0f, 6, false);
        ComponentCondition<IceStormP3>(id + 0x22, 0.6f, comp => comp.NumCasts > 0, "全屏伤害")
            .SetHint(StateMachine.StateHint.Raidwide);
        CastMulti(id + 0x30, [AID.Wormwail, AID.BenightingBreathVisual], 6.4f, 4.5f, "环形／扇形");
        Cast(id + 0x40, AID.TeraSlash, 5.2f, 4, "死刑")
            .SetHint(StateMachine.StateHint.Tankbuster);
        ComponentCondition<Dragonflight>(id + 0x50, 4.3f, comp => comp.WaveJustSpawned, "灵龙（治疗）", 3);
        Cast(id + 0x60, AID.TeraSlash, 42.0f, 4, "死刑")
            .SetHint(StateMachine.StateHint.Tankbuster);
        ComponentCondition<AtomicRay>(id + 0x70, 6.2f, comp => comp.Active, "", 2);
        ComponentCondition<AtomicRay>(id + 0x71, 7.2f, comp => !comp.Active, "分摊", 2);
        ActorCast(id + 0x80, LeftWingP3, AID.IceStormVisualP3, 7.0f, 6, false);
        ComponentCondition<IceStormP3>(id + 0x82, 0.6f, comp => comp.NumCasts > 1, "全屏伤害")
            .SetHint(StateMachine.StateHint.Raidwide);
        CastMulti(id + 0x90, [AID.Wormwail, AID.BenightingBreathVisual], 6.4f, 4.5f, "环形／扇形");
        SimpleState(id + 0xFF0000, 100, "???");
    }

    private void Phase4(uint id)
    {
        CastEnd(id, 70, "狂暴")
            .ActivateOnEnter<Enrage>()
            .SetHint(StateMachine.StateHint.DowntimeStart);
    }

    private Actor? LeftWing() => Module.Enemies(OID.LeftWing).FirstOrDefault();
    private Actor? RightWing() => Module.Enemies(OID.RightWing).FirstOrDefault();
    private Actor? LeftWingP3() => Module.Enemies(OID.LeftWingP3).FirstOrDefault();
}
