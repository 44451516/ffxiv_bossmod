using BossMod.SGE;
using FFXIVClientStructs.FFXIV.Client.Game.Gauge;
using static BossMod.AIHints;

namespace BossMod.Autorotation.kage;

public sealed class KageSGE(RotationModuleManager manager, Actor player) : KageShared<KageSGE.Strategy>(manager, player)
{
    public struct Strategy
    {
        [Track(UiPriority = 500)]
        public Track<Targeting> Targeting;
        [Track(UiPriority = 499)]
        public Track<AOEStrategy> AOE;

        [Track("均衡注药", MinLevel = 30, Actions = [AID.Eukrasia, AID.EukrasianDosis, AID.EukrasianDosisII, AID.EukrasianDosisIII, AID.EukrasianDyskrasia], UiPriority = 100)]
        public Track<DotStrategy> Dot;

        [Track("发炎", MinLevel = 26, Actions = [AID.Phlegma, AID.PhlegmaII, AID.PhlegmaIII], UiPriority = 99)]
        public Track<OffensiveStrategy> Phlegma;

        [Track("心神风息", MinLevel = 92, Action = AID.Psyche, UiPriority = 98)]
        public Track<OffensiveStrategy> Psyche;

        [Track("箭毒", MinLevel = 66, Actions = [AID.Toxikon, AID.ToxikonII], UiPriority = 97)]
        public Track<ToxikonStrategy> Toxikon;

        [Track("魂灵风息（伤害）", MinLevel = 90, Action = AID.Pneuma, UiPriority = 96)]
        public Track<OffensiveStrategy> Pneuma;

        [Track("心关", MinLevel = 4, Action = AID.Kardia, UiPriority = 95)]
        public Track<KardiaStrategy> Kardia;

        [Track("拯救", MinLevel = 35, Action = AID.Soteria, UiPriority = 94)]
        public Track<OffensiveStrategy> Soteria;

        [Track("根素", MinLevel = 74, Action = AID.Rhizomata, UiPriority = 93)]
        public Track<OffensiveStrategy> Rhizomata;

        [Track("灵橡清汁（蛇胆溢出）", MinLevel = 45, Action = AID.Druochole, UiPriority = 92)]
        public Track<DruocholeStrategy> Druochole;

        [Track("醒梦", MinLevel = 14, Action = ClassShared.AID.LucidDreaming, UiPriority = 91)]
        public Track<OffensiveStrategy> Lucid;

        [Track("神翼", MinLevel = 40, Action = AID.Icarus, UiPriority = 90.5f)]
        public Track<IcarusStrategy> Icarus;

        [Track("爆发药", Item = 1049238, UiPriority = 90)]
        public Track<PotionStrategy> Potion;

        [Track("起手", MinLevel = 30, UiPriority = -10)]
        public Track<OpenerStrategy> Opener;
    }

    public enum DotStrategy
    {
        [Option("保持所有敌人的持续伤害效果")]
        Automatic,
        [Option("仅对当前目标使用")]
        TargetOnly,
        [Option("立即刷新当前目标的持续伤害效果", Context = StrategyContext.Plan)]
        Force,
        [Option("不使用")]
        Delay
    }

    public enum ToxikonStrategy
    {
        [Option("移动中且在 2 个及以上目标上使用")]
        Automatic,
        [Option("仅在移动时使用")]
        Movement,
        [Option("尽快使用")]
        ASAP,
        [Option("不使用")]
        Delay
    }

    public enum KardiaStrategy
    {
        [Option("保持在承受伤害的坦克身上")]
        Automatic,
        [Option("不更换心关")]
        Manual,
        [Option("保持在选定的队伍成员上", Targets = ActionTargets.Party | ActionTargets.Self)]
        Specific
    }

    public enum DruocholeStrategy
    {
        [Option("满层时对生命值最低的队友使用")]
        Automatic,
        [Option("不使用")]
        Delay
    }

    public enum IcarusStrategy
    {
        [Option("不使用")]
        None,
        [Option("用于抵消击退", Targets = ActionTargets.Party | ActionTargets.Hostile)]
        Knockback
    }

    public enum PotionStrategy
    {
        [Option("不自动使用")]
        Manual,
        [Option("最优：开怪前使用，之后配合团队增益")]
        AlignWithRaidBuffs,
        [Option("尽快使用")]
        Immediate
    }

    public enum OpenerStrategy
    {
        [Option("箭毒起手")]
        Toxikon,
        [Option("魂灵风息起手", MinLevel = 90)]
        Pneuma,
        [Option("不执行开怪前及起手专用规则")]
        None
    }

    public static RotationModuleDefinition Definition()
    {
        return new RotationModuleDefinition("Kage SGE", "贤者", "标准循环（Kage）|治疗", "Kagekazu", RotationModuleQuality.Ok, BitMask.Build(Class.SGE), 100).WithStrategies<Strategy>();
    }

    private int Gall;
    private int Sting;
    private bool Eukrasia;

    private bool AOEMode;

    protected override bool UsesSpellSpeed => true;
    private bool CanCast => CastFits(ScaledCastTime(AID.Dosis));

    private OffensiveStrategy PhlegmaStrat;
    private OffensiveStrategy PsycheStrat;

    public override void Execute(in Strategy strategy, ref Actor? primaryTarget, float estimatedAnimLockDelay, bool isMoving)
    {
        var target = SelectTarget(strategy, strategy.Targeting.Value, ref primaryTarget, estimatedAnimLockDelay, 25, strategy.AOE.Value);
        PhlegmaStrat = strategy.Phlegma.Value;
        PsycheStrat = strategy.Psyche.Value;

        var gauge = World.Client.GetGauge<SageGauge>();
        Gall = gauge.Addersgall;
        Sting = gauge.Addersting;
        Eukrasia = gauge.EukrasiaActive;

        var numAround = ForceST ? 0 : Hints.NumPriorityTargetsInAOECircle(Player.Position, 5);
        AOEMode = Unlocked(AID.Dyskrasia) && UseAOE(strategy.AOE.Value, numAround, 3);

        Kardia(strategy);

        if (UsePotion(strategy))
            PushPotion(ActionDefinitions.IDPotionMnd);

        if (World.Client.CountdownRemaining is > 0 and var countdown)
        {
            var precast = strategy.Opener.Value switch
            {
                OpenerStrategy.Pneuma when GCDReady(AID.Pneuma) => AID.Pneuma,
                OpenerStrategy.Toxikon or OpenerStrategy.Pneuma when Sting > 0 && Unlocked(AID.Toxikon) && strategy.Toxikon.Value != ToxikonStrategy.Delay => AID.Toxikon,
                _ => AID.None
            };
            if (target != null && precast != AID.None && Unlocked(AID.Eukrasia))
            {
                if (!Eukrasia && countdown < 5)
                    PushGCD(AID.Eukrasia, Player, 10);
                else if (countdown < 1.5f)
                    PushGCD(precast, target.Actor, 10);
            }
            else if (target != null && strategy.Opener.Value != OpenerStrategy.None && countdown < CastTime(AID.Dosis) + 0.2f)
                PushGCD(AID.Dosis, target.Actor, 10);
            return;
        }

        if (target == null)
            return;

        var goal = Hints.GoalSingleTarget(target.Actor, Player, World.Actors, 25);
        Hints.GoalZones.Add(!AllowAoE || !Unlocked(AID.Dyskrasia) ? goal : GoalCombined(goal, Hints.GoalAOECircle(5), 3));

        GCDs(strategy, target, numAround);
        if (Player.InCombat)
            OGCDs(strategy, target);
    }

    #region GCD

    private void GCDs(in Strategy strategy, Enemy target, int numAround)
    {
        var allowDotAoE = AOEMode && strategy.Dot.Value == DotStrategy.Automatic && Unlocked(AID.EukrasianDyskrasia);
        if (Eukrasia)
        {
            if (allowDotAoE)
                PushGCD(AID.Dyskrasia, Player, 60);
            PushGCD(AID.Dosis, DotTarget(strategy, target) ?? target.Actor, 59);
            return;
        }

        if (Player.InCombat && strategy.Dot.Value != DotStrategy.Delay)
        {
            var dotAoE = allowDotAoE && Hints.PriorityTargets.Count(e => Player.DistanceToHitbox(e.Actor) <= 5 && NeedsDot(e)) >= 3;
            if (dotAoE || !AOEMode && DotTarget(strategy, target) != null)
                PushGCD(AID.Eukrasia, Player, 50);
        }

        if (ShouldPhlegma(target))
        {
            Hints.GoalZones.Add(Hints.GoalSingleTarget(target.Actor, Player, World.Actors, 6));
            if (Player.DistanceToHitbox(target.Actor) <= 6)
                PushGCD(AID.Phlegma, target.Actor, 40);
        }

        if (CanCast && Player.InCombat && GCDReady(AID.Pneuma) && (strategy.Pneuma.Value == OffensiveStrategy.Force
            || strategy.Pneuma.Value == OffensiveStrategy.Automatic && !RaidwideWithin(20) && !ForceST && Hints.NumPriorityTargetsInAOERect(Player.Position, Player.DirectionTo(target.Actor), 25, 2) >= 2))
            PushGCD(AID.Pneuma, target.Actor, 35);

        if (Sting > 0 && (strategy.Toxikon.Value == ToxikonStrategy.ASAP
            || strategy.Toxikon.Value == ToxikonStrategy.Automatic && numAround < 3 && !ForceST && Hints.NumPriorityTargetsInAOECircle(target.Actor.Position, 5) >= 2))
            PushGCD(AID.Toxikon, target.Actor, 30);

        if (AOEMode)
            PushGCD(AID.Dyskrasia, Player, 20);

        PushGCD(AID.Dosis, target.Actor, 10);

        if (!CanCast)
        {
            if (Sting > 0 && strategy.Toxikon.Value != ToxikonStrategy.Delay)
                PushGCD(AID.Toxikon, target.Actor, 9);
            if (numAround > 0)
                PushGCD(AID.Dyskrasia, Player, 8);
        }
    }

    private float DotRefresh => GCD + GCDLength + 1;

    private float DotLeft(Actor target) => MaxDotLeft(target, SID.EukrasianDosisIII, SID.EukrasianDosisII, SID.EukrasianDosis, SID.EukrasianDyskrasia);

    private bool NeedsDot(Enemy e) => DotWorthIt(e) && DotLeft(e.Actor) < DotRefresh;

    private Actor? DotTarget(in Strategy strategy, Enemy target)
    {
        if (strategy.Dot.Value == DotStrategy.Force)
            return JustUsed(AID.EukrasianDosisIII, 5) || JustUsed(AID.EukrasianDosisII, 5) || JustUsed(AID.EukrasianDosis, 5) ? null : target.Actor;
        return DotTargetFor(target, DotLeft, DotRefresh, CombatTime < 40 ? 12 : 8, strategy.Dot.Value == DotStrategy.Automatic);
    }

    private bool ShouldPhlegma(Enemy target)
    {
        if (ReadyIn(AID.Phlegma) > GCD)
            return false;
        switch (PhlegmaStrat)
        {
            case OffensiveStrategy.Force:
                return true;
            case OffensiveStrategy.Delay:
                return false;
        }
        var buffsSoon = Player.InCombat && InBossFight && RaidBuffsLeft <= GCD && RaidBuffsIn < 10;
        if (MaxChargesIn(AID.Phlegma) <= GCD + GCDLength)
            return !buffsSoon;
        if (DowntimeIn < GCD + GCDLength && MaxChargesIn(AID.Phlegma) < 30)
            return true;
        if (!Player.InCombat || target.Priority == Enemy.PriorityPointless && !TimeToKill.IsBossTier(Bossmods.ActiveModule, Hints, target.Actor))
            return false;
        if (RaidBuffsLeft > GCD || RaidBuffsIn > 9000 || !InBossFight || !ForceST && Hints.NumPriorityTargetsInAOECircle(target.Actor.Position, 5) >= 3)
            return true;
        var cooldown = ActionDefinitions.Instance.Spell(AID.Phlegma)!.Cooldown;
        return MaxChargesIn(AID.Phlegma) + cooldown <= RaidBuffsIn;
    }

    private bool MPCheckSoon => RaidwideWithin(15) || World.Party.WithoutSlot(excludeAlliance: true).Any(p => p.IsDead);

    #endregion

    #region oGCD

    private void OGCDs(in Strategy strategy, Enemy target)
    {
        if (strategy.Icarus.Value == IcarusStrategy.Knockback)
            CancelKnockback(AID.Icarus, ResolveTarget(strategy.Icarus) ?? target.Actor);

        if (PsycheStrat != OffensiveStrategy.Delay && CanWeave(AID.Psyche)
            && (PsycheStrat == OffensiveStrategy.Force || DowntimeIn > 2 && (target.Priority != Enemy.PriorityPointless || TimeToKill.IsBossTier(Bossmods.ActiveModule, Hints, target.Actor))
                && (RaidBuffsLeft > 0 || RaidBuffsIn > 10 || !InBossFight)))
            PushOGCD(AID.Psyche, target.Actor, 50);

        if ((strategy.Rhizomata.Value == OffensiveStrategy.Force || strategy.Rhizomata.Value == OffensiveStrategy.Automatic && Gall <= 1) && CanWeave(AID.Rhizomata))
            PushOGCD(AID.Rhizomata, Player, 40);

        // the gall timer stops at 3, so every second at cap is wasted
        if (strategy.Druochole.Value == DruocholeStrategy.Automatic && Gall >= 3 && CanWeave(AID.Druochole)
            && !(RaidwideWithin(15) && ReadyIn(AID.Kerachole) < 5))
        {
            var healTarget = World.Party.WithoutSlot(excludeAlliance: true).Where(p => !p.IsDead && Player.DistanceToHitbox(p) <= 30).MinBy(p => p.PendingHPRatio) ?? Player;
            PushOGCD(AID.Druochole, healTarget, 35);
        }

        if ((strategy.Soteria.Value == OffensiveStrategy.Force || strategy.Soteria.Value == OffensiveStrategy.Automatic && Player.FindStatus(SID.Kardia) != null) && CanWeave(AID.Soteria))
            PushOGCD(AID.Soteria, Player, 30);

        if ((strategy.Lucid.Value == OffensiveStrategy.Force || strategy.Lucid.Value == OffensiveStrategy.Automatic && (MP <= 6500 || MP <= 8500 && MPCheckSoon)) && CanWeave(ClassShared.AID.LucidDreaming))
            PushOGCD(ClassShared.AID.LucidDreaming, Player, 20);
    }

    private void Kardia(in Strategy strategy)
    {
        if (strategy.Kardia.Value == KardiaStrategy.Manual || ReadyIn(AID.Kardia) > 0)
            return;

        var desired = strategy.Kardia.Value == KardiaStrategy.Specific ? ResolveTarget(strategy.Kardia) : KardiaTarget();
        if (desired == null || desired.IsDead || !desired.IsTargetable || HasKardion(desired))
            return;

        Hints.ActionsToExecute.Push(ActionID.MakeSpell(AID.Kardia), desired, Player.InCombat ? ActionQueue.Priority.Low + 60 : ActionQueue.Priority.High);
    }

    private bool HasKardion(Actor a) => a.FindStatus(SID.Kardion, Player.InstanceID, World.FutureTime(1000)) != null;

    private Actor? KardiaTarget()
    {
        var party = World.Party.WithoutSlot(excludeAlliance: true).ToList();
        if (party.Count == 1)
            return Player;
        var tanks = party.Where(p => p.Role == Role.Tank && !p.IsDead).ToList();
        if (tanks.Count <= 1)
            return tanks.FirstOrDefault() ?? Player;

        var current = tanks.FirstOrDefault(HasKardion);
        int Aggro(Actor t) => Hints.PriorityTargets.Sum(e => e.Actor.TargetID != t.InstanceID ? 0 : e.Actor == Bossmods.ActiveModule?.PrimaryActor ? 10 : 1);
        var best = tanks.MaxBy(Aggro)!;
        return current != null && Aggro(best) <= Aggro(current) ? current : best;
    }

    #endregion

    #region Helpers

    private bool UsePotion(in Strategy strategy) => strategy.Potion.Value switch
    {
        PotionStrategy.AlignWithRaidBuffs => PotionWithRaidBuffs(PotionPrepull) || PotionBeforeKill,
        PotionStrategy.Immediate => true,
        _ => false
    };

    private float CastTime(AID aid) => Eukrasia && aid == AID.Dosis ? 0 : HardcastTime(aid);

    private void PushGCD(AID aid, Actor? target, int priority) => base.PushGCD(aid, target, priority, CastTime(aid));

    #endregion
}
