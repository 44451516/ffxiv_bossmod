using BossMod.Autorotation.xan;

namespace BossMod.Autorotation.MiscAI;

public sealed class AutoTarget(RotationModuleManager manager, Actor player) : RotationModule(manager, player)
{
    public enum Track { General, Retarget, QuestBattle, DeepDungeon, EpicEcho, Hunt, FATE, Everything, CollectFATE, MaxTargets }
    public enum GeneralStrategy { Aggressive, Passive }
    public enum RetargetStrategy { NoTarget, Hostiles, Always, Never }
    public enum Flag { Disabled, Enabled }

    public static RotationModuleDefinition Definition()
    {
        RotationModuleDefinition res = new("自动选择目标", "根据不同条件自动选择目标和拉怪的工具集合。", "AI", "veyn", RotationModuleQuality.Basic, new(~0ul), 1000, 1, RotationModuleOrder.HighLevel, CanUseWhileRoleplaying: true);

        res.Define(Track.General).As<GeneralStrategy>("General", "通用")
            .AddOption(GeneralStrategy.Aggressive, "自动优先选择目标", supportedTargets: ActionTargets.Hostile)
            .AddOption(GeneralStrategy.Passive, "不执行操作");

        res.Define(Track.Retarget).As<RetargetStrategy>("Retarget", "重新选择目标")
            .AddOption(RetargetStrategy.NoTarget, "仅在玩家没有目标时切换目标")
            .AddOption(RetargetStrategy.Hostiles, "仅在玩家当前目标不是友方时切换目标")
            .AddOption(RetargetStrategy.Always, "总是切换到优先级最高的敌人")
            .AddOption(RetargetStrategy.Never, "永不切换目标；只调整敌人的优先级");

        res.Define(Track.QuestBattle).As<Flag>("QuestBattle", "优先选择任务战斗中的 Boss", renderer: typeof(DefaultOffRenderer), uiPriority: -50)
            .AddOption(Flag.Disabled)
            .AddOption(Flag.Enabled);

        res.Define(Track.DeepDungeon).As<Flag>("DD", "优先选择深层迷宫 Boss（仅单人）", renderer: typeof(DefaultOffRenderer), uiPriority: -60)
            .AddOption(Flag.Disabled)
            .AddOption(Flag.Enabled);

        res.Define(Track.EpicEcho).As<Flag>("EE", "在解除限制的任务中优先选择所有目标", renderer: typeof(DefaultOffRenderer), uiPriority: -70)
            .AddOption(Flag.Disabled)
            .AddOption(Flag.Enabled);

        res.Define(Track.Hunt).As<Flag>("Hunt", "狩猎怪被开怪后优先选择它们", renderer: typeof(DefaultOffRenderer), uiPriority: -80)
            .AddOption(Flag.Disabled)
            .AddOption(Flag.Enabled);

        res.Define(Track.FATE).As<Flag>("FATE", "优先选择当前 FATE 中的敌人", renderer: typeof(DefaultOffRenderer), uiPriority: -90)
            .AddOption(Flag.Disabled)
            .AddOption(Flag.Enabled);

        res.Define(Track.Everything).As<Flag>("Everything", "优先选择所有目标", renderer: typeof(DefaultOffRenderer), uiPriority: -100)
            .AddOption(Flag.Disabled)
            .AddOption(Flag.Enabled);

        res.Define(Track.CollectFATE).As<Flag>("CollectFATE", "在交付型 FATE 中忽略被动敌人", renderer: typeof(DefaultOffRenderer), uiPriority: -110)
            .AddOption(Flag.Disabled)
            .AddOption(Flag.Enabled);

        res.DefineInt(Track.MaxTargets, "最大拉怪数量（0 = 不限制）", minValue: 0, maxValue: 30, uiPriority: -120);

        return res;
    }

    public override void Execute(StrategyValues strategy, ref Actor? primaryTarget, float estimatedAnimLockDelay, bool isMoving)
    {
        var generalOpt = strategy.Option(Track.General);
        var generalStrategy = generalOpt.As<GeneralStrategy>();
        if (generalStrategy == GeneralStrategy.Passive)
            return;

        var maxTargets = strategy.GetInt(Track.MaxTargets);
        var canPullMore = maxTargets == 0 || World.Actors.Count(a => a.AggroPlayer && !a.IsDead) < maxTargets;

        Actor? bestTarget = null; // non-null if we bump any priorities
        (int, float) bestTargetKey = (0, float.MinValue); // priority and negated squared distance
        void prioritize(AIHints.Enemy e, int prio)
        {
            e.Priority = prio;

            var key = (e.Priority, -(e.Actor.Position - Player.Position).LengthSq());
            if (key.CompareTo(bestTargetKey) > 0)
            {
                bestTarget = e.Actor;
                bestTargetKey = key;
            }
        }

        var allowAll = strategy.Option(Track.Everything).As<Flag>() == Flag.Enabled;

        if (strategy.Option(Track.QuestBattle).As<Flag>() == Flag.Enabled)
            allowAll |= Bossmods.LoadedModules is [{ Info.Category: BossModuleInfo.Category.Quest }];

        if (strategy.Option(Track.DeepDungeon).As<Flag>() == Flag.Enabled)
            allowAll |= Bossmods.LoadedModules is [{ Info.Category: BossModuleInfo.Category.DeepDungeon }];

        if (strategy.Option(Track.EpicEcho).As<Flag>() == Flag.Enabled)
            allowAll |= Player.Statuses.Any(s => s.ID == 2734);

        ulong huntTarget = 0;

        if (strategy.Option(Track.Hunt).As<Flag>() == Flag.Enabled && Bossmods.ActiveModule is
            {
                Info.Category: BossModuleInfo.Category.Hunt,
                PrimaryActor:
                {
                    InCombat: true,
                    HPRatio: <= 0.95f,
                    InstanceID: var i
                }
            }
        )
            huntTarget = i;

        var targetFates = strategy.Option(Track.FATE).As<Flag>() == Flag.Enabled && Utils.IsPlayerSyncedToFate(World);
        var targetFateMobs = World.Client.ActiveFate.Progress < 100;

        var turnin = Utils.GetFateItem(World.Client.ActiveFate.ID);
        if (turnin > 0)
        {
            if (strategy.Option(Track.CollectFATE).As<Flag>() == Flag.Enabled)
                targetFateMobs = false;
            else
                // keep targeting mobs until we have enough turnin items (unless we are holding 10, in which case FateUtils is probably trying to perform turnin, let's not interrupt it)
                targetFateMobs |= World.Client.ActiveFate.HandInCount < FateUtils.TurnInGoldReq && World.Client.GetInventoryItemQuantity(turnin) < FateUtils.TurnInGoldReq;
        }

        // first deal with pulling new enemies
        foreach (var target in Hints.PotentialTargets)
        {
            if (target.Actor.InstanceID == huntTarget)
            {
                prioritize(target, 0);
                continue;
            }

            if (canPullMore && allowAll && !target.Actor.IsStrikingDummy && target.Priority == AIHints.Enemy.PriorityUndesirable)
            {
                prioritize(target, 0);
                continue;
            }

            if (targetFates && target.Actor.FateID == World.Client.ActiveFate.ID)
            {
                if (target.Actor.NameID is 6737 or 6738)
                {
                    prioritize(target, 1);
                    continue;
                }
                if (targetFateMobs && canPullMore)
                {
                    prioritize(target, 0);
                    continue;
                }
            }

            // add all other targets to potential targets list (e.g. if modules modify out-of-combat mob priority)
            if (target.Priority >= 0)
                prioritize(target, target.Priority);
        }

        // prioritizer yielded no results meaning there are no targets to pick, do nothing
        if (bestTarget == null)
            return;

        Hints.PotentialTargets.SortByReverse(x => x.Priority);
        Hints.HighestPotentialTargetPriority = Math.Max(0, Hints.PotentialTargets[0].Priority);

        var retargetStrategy = strategy.Option(Track.Retarget).As<RetargetStrategy>();
        if (retargetStrategy == RetargetStrategy.Never)
            return;

        var currentTarget = World.Actors.Find(Player.TargetID);

        var changeTarget = retargetStrategy switch
        {
            RetargetStrategy.Hostiles => currentTarget == null || !currentTarget.IsAlly,
            RetargetStrategy.NoTarget => currentTarget == null,
            _ => true
        };

        // if we have target to switch to, do that
        if (changeTarget)
            primaryTarget = Hints.ForcedTarget = bestTarget;
    }
}
