namespace BossMod.Autorotation.Utility;

public sealed class ClassSCHUtilityEx(RotationModuleManager manager, Actor player) : TypedRotationModule<ClassSCHUtilityEx.Strategy>(manager, player)
{
    public struct Strategy
    {
        [Track(Actions = [SCH.AID.Adloquium, SCH.AID.DeploymentTactics, SCH.AID.Protraction, SCH.AID.Recitation], DefaultPriority = ActionQueue.Priority.High + 300, Context = StrategyContext.Plan)]
        public Track<SpreadloStrategy> Spreadlo;

        [Track(Actions = [SCH.AID.Adloquium, SCH.AID.Succor, SCH.AID.Concitation], Context = StrategyContext.Plan)]
        public Track<ShieldStrategy> Shield;
    }

    public enum SpreadloStrategy
    {
        [Option("不使用")]
        None,
        [Option("使用鼓舞激励之策 + 展开战术", Targets = ActionTargets.Self | ActionTargets.Party, Effect = 30, MinLevel = 56)]
        Basic,
        [Option("秘策 -> Spreadlo", Targets = ActionTargets.Self | ActionTargets.Party, Effect = 30, MinLevel = 74)]
        Recite,
        [Option("生命回生法 -> 秘策 -> Spreadlo", Targets = ActionTargets.Self | ActionTargets.Party, Effect = 30, MinLevel = 86)]
        Protract
    }

    public enum ShieldStrategy
    {
        [Option("禁用")]
        Disabled,
        [Option("若匹配的队友缺少护盾则为其施加护盾；确保护盾持续到计划条目结束", Targets = ActionTargets.Self | ActionTargets.Party, DefaultPriority = ActionQueue.Priority.High + 500)]
        Enabled
    }

    public static RotationModuleDefinition Definition()
    {
        return new RotationModuleDefinition("辅助：SCH（额外）", "SCH 额外功能", "计划器辅助", "xan", RotationModuleQuality.Ok, BitMask.Build(Class.SCH), 100).WithStrategies<Strategy>();
    }

    bool HaveSwift;

    public override void Execute(in Strategy strategy, ref Actor? primaryTarget, float estimatedAnimLockDelay, bool isMoving)
    {
        HaveSwift = Player.FindStatus(ClassShared.SID.Swiftcast)?.ExpireAt > World.FutureTime(GCD);

        Shield(strategy, ref primaryTarget);
        Spreadlo(strategy, ref primaryTarget);
    }

    void Shield(in Strategy strategy, ref Actor? primaryTarget)
    {
        if (strategy.Shield.Value == ShieldStrategy.Disabled)
            return;

        // gcd shield lasts 30 seconds, if the entry is longer than that, just wait it out
        if (strategy.Shield.TrackRaw.ExpireIn > 30)
            return;

        var entryEnd = World.FutureTime(strategy.Shield.TrackRaw.ExpireIn);

        var playersEnum = strategy.Shield.TrackRaw.Target == StrategyTarget.Automatic ? World.Party.WithoutSlot() : Manager.ResolvePartyMembers(strategy.Shield.TrackRaw.Target, strategy.Shield.TrackRaw.TargetParam);

        var shieldPlayers = playersEnum.Where(p => !(p.FindStatus(SCH.SID.Galvanize, World.FutureTime(30))?.ExpireAt > entryEnd)).ToList();

        // use succor to hit multiple allies
        // TODO: option to force adlo? not sure if it would ever be practical though
        if (shieldPlayers.InRadius(Player.Position, 20).Count() > 1)
        {
            var shield = ActionUnlocked(SCH.AID.Concitation) ? SCH.AID.Concitation : SCH.AID.Succor;
            Hints.ActionsToExecute.Push(ActionID.MakeSpell(shield), Player, strategy.Shield.Priority(), castTime: HaveSwift ? 0 : 2);
            return;
        }

        foreach (var p in shieldPlayers)
            Hints.ActionsToExecute.Push(ActionID.MakeSpell(SCH.AID.Adloquium), p, strategy.Shield.Priority(), castTime: HaveSwift ? 0 : 2);
    }

    void Spreadlo(in Strategy strategy, ref Actor? primaryTarget)
    {
        var recite = false;
        var protract = false;

        if (ResolveTarget(strategy.Spreadlo) is not { } target)
        {
            if (strategy.Spreadlo.TrackRaw.Target == StrategyTarget.Automatic)
                target = Player;
            else
                return;
        }

        switch (strategy.Spreadlo.Value)
        {
            case SpreadloStrategy.None:
                return;
            case SpreadloStrategy.Recite:
                recite = true;
                break;
            case SpreadloStrategy.Protract:
                recite = protract = true;
                break;
        }

        if (recite)
            Hints.ActionsToExecute.Push(ActionID.MakeSpell(SCH.AID.Recitation), Player, ActionQueue.Priority.High);

        if (protract)
            Hints.ActionsToExecute.Push(ActionID.MakeSpell(SCH.AID.Protraction), target, ActionQueue.Priority.High);

        var canShield = true;

        if (recite)
            canShield &= Player.FindStatus(SCH.SID.Recitation, DateTime.MaxValue) != null || target.FindStatus(SCH.SID.Catalyze, DateTime.MaxValue) != null;

        if (protract)
            canShield &= target.FindStatus(SCH.SID.Protraction, DateTime.MaxValue) != null;

        if (canShield)
        {
            if (target.FindStatus(SCH.SID.Galvanize, DateTime.MaxValue) == null)
                Hints.ActionsToExecute.Push(ActionID.MakeSpell(SCH.AID.Adloquium), target, strategy.Spreadlo.Priority(), castTime: HaveSwift ? 0 : 2);
            else
                Hints.ActionsToExecute.Push(ActionID.MakeSpell(SCH.AID.DeploymentTactics), target, ActionQueue.Priority.High);
        }
    }
}
