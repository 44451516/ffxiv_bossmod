namespace BossMod.Autorotation.MiscAI;

public sealed class FateUtils(RotationModuleManager manager, Actor player) : RotationModule(manager, player)
{
    public override bool WantsLoSFix => true;

    public enum Track { Handin, Collect, Sync, Chocobo }
    public enum Flag { Enabled, Disabled }

    public static RotationModuleDefinition Definition()
    {
        var res = new RotationModuleDefinition("FATE 助手", "完成 FATE 的辅助工具", "AI", "xan", RotationModuleQuality.Basic, new(~0ul), 1000, 1);

        res.Define(Track.Handin).As<Flag>("Hand-in", "交付")
            .AddOption(Flag.Enabled, "FATE 道具达到 10 个以上时自动交付")
            .AddOption(Flag.Disabled, "不执行操作");

        res.Define(Track.Collect).As<Flag>("Collect", "采集")
            .AddOption(Flag.Enabled, "尝试采集 FATE 道具而不是参与战斗")
            .AddOption(Flag.Disabled, "不执行操作");

        res.Define(Track.Sync).As<AIHints.FateSync>("Sync", "等级同步")
            .AddOption(AIHints.FateSync.None, "不执行操作")
            .AddOption(AIHints.FateSync.Enable, "可用时始终启用等级同步")
            .AddOption(AIHints.FateSync.Disable, "可用时始终取消等级同步");

        res.Define(Track.Chocobo).As<Flag>("Chocobo", "陆行鸟")
            .AddOption(Flag.Enabled, "陆行鸟剩余时间少于 60 秒时重新召唤")
            .AddOption(Flag.Disabled, "不执行操作");

        return res;
    }

    public const int TurnInGoldReq = 10;

    public override void Execute(StrategyValues strategy, ref Actor? primaryTarget, float estimatedAnimLockDelay, bool isMoving)
    {
        Hints.WantFateSync = strategy.Option(Track.Sync).As<AIHints.FateSync>();

        if (!Utils.IsPlayerSyncedToFate(World))
            return;

        if (strategy.Option(Track.Chocobo).As<Flag>() == Flag.Enabled && World.Client.GetInventoryItemQuantity(ActionDefinitions.IDMiscItemGreens.ID) > 0 && World.Client.ActiveCompanion is { TimeLeft: < 60, Stabled: false })
            Hints.ActionsToExecute.Push(ActionDefinitions.IDMiscItemGreens, Player, ActionQueue.Priority.VeryHigh);

        var goal = GetGoal(strategy);
        if (goal is CollectFateGoal.HandIn)
        {
            var target = World.Actors.Find(World.Client.ActiveFate.ObjectiveNpc);
            Hints.InteractWithTarget = target;
            // if the auto generated obstacle map is bad, it'll get stuck so force movement regardless
            if (target != null && ShouldForceMovement(target))
                Hints.ForcedMovement = Player.DirectionTo(target).ToVec3(Player.PosRot.Y);
            return;
        }
        else if (goal is CollectFateGoal.Pickup)
        {
            var target = World.Actors.Where(a => a.FateID == World.Client.ActiveFate.ID && a.IsTargetable && a.Type == ActorType.EventObj).MinBy(Player.DistanceToHitbox);
            Hints.InteractWithTarget = target;
            if (target != null && ShouldForceMovement(target))
                Hints.ForcedMovement = Player.DirectionTo(target).ToVec3(Player.PosRot.Y);
            return;
        }

        if (Manager.LoSFix is { } los)
        {
            var losDelta = los.Destination - los.Origin;
            var losDist = losDelta.Length();
            var losDir = losDist > 1e-3f ? losDelta / losDist : default;

            // tight spot with high reward to get out of where we're at
            Hints.GoalZones.Add(Hints.GoalSingleTarget(los.Destination, 0.3f, 120));
            // add a penalty to current position to actually encourage moving out of it
            Hints.GoalZones.Add(p => p.InCircle(los.Origin, 1.0f) ? -25 : 0);
            // more encouragement for going towards the destination, but need to cap it or else it'll just keep going
            Hints.GoalZones.Add(p =>
            {
                var progress = WDir.Dot(p - los.Origin, losDir);
                if (progress <= 0)
                    return 0;

                var cappedProgress = MathF.Min(progress, losDist);
                var overshoot = MathF.Max(0, progress - losDist);
                return cappedProgress * 8 - overshoot * 16;
            });
        }
    }

    private CollectFateGoal GetGoal(StrategyValues strategy)
    {
        if (strategy.Option(Track.Handin).As<Flag>() != Flag.Enabled)
            return CollectFateGoal.None;

        if (Utils.GetFateItem(World.Client.ActiveFate.ID) is not (not 0 and var itemId))
            return CollectFateGoal.None;

        // already turned in enough, fate is ending, do nothing
        if (World.Client.ActiveFate.HandInCount >= TurnInGoldReq && World.Client.ActiveFate.Progress >= 100)
            return CollectFateGoal.None;

        // until fate is completed, hand in batches of 10; if other people complete the fate, we stop doing stuff
        if (World.Client.GetInventoryItemQuantity(itemId) >= TurnInGoldReq)
            return CollectFateGoal.HandIn;

        // pick up stuff
        return strategy.Option(Track.Collect).As<Flag>() == Flag.Enabled && !Player.InCombat ? CollectFateGoal.Pickup : CollectFateGoal.None;
    }

    // no path = force movement anyway
    private bool ShouldForceMovement(Actor target)
        => Hints.PathfindMapObstacles.Bitmap != null
            && (!Hints.PathfindMapBounds.Contains(target.Position - Hints.PathfindMapCenter) || !Hints.PathfindMapObstacles.HasObstacleMapLineOfSight(Hints.PathfindMapCenter, Player.Position, target.Position));

    private enum CollectFateGoal
    {
        None,
        HandIn,
        Pickup,
    }
}
