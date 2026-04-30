using System.Globalization;

namespace BossMod.Autorotation.MiscAI;

public sealed class StayCloseToPartyRole(RotationModuleManager manager, Actor player) : RotationModule(manager, player)
{
    public enum Tracks
    {
        Role,
        Range
    }

    public enum RangeDefinition
    {
        OnHitbox
    }

    public static RotationModuleDefinition Definition()
    {
        RotationModuleDefinition def = new("杂项 AI：保持在队伍角色范围内", "供 AutoDuty 预设使用的模块。", "AI", "erdelf", RotationModuleQuality.Basic, new(~0ul), 1000);

        var roleRef = def.Define(Tracks.Role).As<Role>("Role", "要靠近的角色");

        foreach (var role in Enum.GetValues<Role>())
        {
            roleRef.AddOption(role);
        }

        var rangeRef = def.Define(Tracks.Range).As<RangeDefinition>("range", "到队友的距离", renderer: typeof(FakeFloatRenderer));

        rangeRef.AddOption(RangeDefinition.OnHitbox, "停在碰撞箱边缘（+/- 1 单位）");
        for (var f = 1.1f; f <= 30f; f = MathF.Round(f + 0.1f, 1))
        {
            rangeRef.AddOption((RangeDefinition)(f * 10f - 10f), internalNameOverride: f.ToString(CultureInfo.InvariantCulture));
        }

        return def;
    }

    public override void Execute(StrategyValues strategy, ref Actor? primaryTarget, float estimatedAnimLockDelay, bool isMoving)
    {
        var role = strategy.Option(Tracks.Role).As<Role>();
        if (role != Role.None && role != Manager.Player?.Role)
        {
            var roleActor = World.Party.WithoutSlot(false, true).FirstOrDefault(a => a.Role == role);
            if (roleActor != null)
            {
                var position = roleActor.Position;
                var radius = roleActor.HitboxRadius;
                var range = strategy.Option(Tracks.Range);
                if (range.As<RangeDefinition>() == RangeDefinition.OnHitbox)
                    Hints.GoalZones.Add(p => p.InDonut(position, radius - 1, radius + 1) ? 0.5f : 0);
                else
                    Hints.GoalZones.Add(Hints.GoalSingleTarget(position, (range.Value.Option + 10f) / 10f + roleActor.HitboxRadius, 1f));
            }
        }
    }
}
