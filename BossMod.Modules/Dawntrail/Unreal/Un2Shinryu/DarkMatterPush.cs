namespace BossMod.Dawntrail.Unreal.Un2Shinryu;

class DarkMatterPush(BossModule module) : BossComponent(module)
{
    public override void AddGlobalHints(GlobalHints hints)
    {
        var hp = Module.PrimaryActor.HPMP;
        if (hp.MaxHP == 0 || Module.StateMachine.TimeSincePhaseEnter < 240)
            return;
        var pct = 100f * hp.CurHP / hp.MaxHP;
        if (pct > 45)
            hints.Add($"将血量压至 45% 以触发暗物质（当前 {pct:f1}%）");
    }
}
