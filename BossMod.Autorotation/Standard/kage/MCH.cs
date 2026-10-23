using BossMod.MCH;
using FFXIVClientStructs.FFXIV.Client.Game.Gauge;
using static BossMod.AIHints;

namespace BossMod.Autorotation.kage;

public sealed class KageMCH(RotationModuleManager manager, Actor player) : KageRotation<KageMCH.Strategy>(manager, player)
{
    public struct Strategy
    {
        public Track<Targeting> Targeting;
        public Track<AOEStrategy> AOE;

        [Track("超荷", MinLevel = 30, Action = AID.Hypercharge)]
        public Track<HyperchargeStrategy> Hypercharge;

        [Track("爆发（野火、枪管加热、工具、整备、后式自走人偶）", InternalName = "Burst", MinLevel = 10)]
        public Track<OffensiveStrategy> Burst;

        [Track("野火", InternalName = "WF", MinLevel = 45, Action = AID.Wildfire)]
        public Track<WildfireStrategy> Wildfire;

        [Track("枪管加热", MinLevel = 66, Action = AID.BarrelStabilizer)]
        public Track<OffensiveStrategy> Stabilizer;

        [Track("工具", Actions = [AID.Drill, AID.HotShot, AID.AirAnchor, AID.ChainSaw, AID.Excavator, AID.Bioblaster, AID.FullMetalField])]
        public Track<OffensiveStrategy> Tools;

        [Track("整备", MinLevel = 10, Action = AID.Reassemble)]
        public Track<ReassembleStrategy> Reassemble;

        [Track("后式自走人偶", MinLevel = 40, Actions = [AID.AutomatonQueen, AID.RookAutoturret, AID.QueenOverdrive, AID.RookOverdrive])]
        public Track<QueenStrategy> Queen;

        [Track("虹吸弹 / 弹射", MinLevel = 15, Actions = [AID.GaussRound, AID.Ricochet, AID.DoubleCheck, AID.Checkmate])]
        public Track<ChargeStrategy> Charges;

        [Track("火焰喷射器", MinLevel = 70, Action = AID.Flamethrower)]
        public Track<FlamethrowerStrategy> Flamethrower;

        [Track("爆发药")]
        public Track<PotionStrategy> Potion;

        [Track("起手", MinLevel = 90, UiPriority = -10)]
        public Track<OpenerStrategy> Opener;
    }

    public enum HyperchargeStrategy
    {
        [Option("在工具技能之间使用；为野火保留 Heat")]
        Automatic,
        [Option("尽快使用", Effect = 10, MinLevel = 30)]
        ASAP,
        [Option("不使用")]
        Delay
    }

    public enum WildfireStrategy
    {
        [Option("进入过热状态后使用，留给 Boss 战", Targets = ActionTargets.Hostile)]
        Automatic,
        [Option("尽快使用", Cooldown = 120, Effect = 10, Targets = ActionTargets.Hostile, MinLevel = 45)]
        Force,
        [Option("不使用")]
        Delay
    }

    public enum ReassembleStrategy
    {
        [Option("在工具技能时使用")]
        Automatic,
        [Option("在工具技能和 AOE 填充时使用")]
        Any,
        [Option("不使用")]
        Delay
    }

    public enum QueenStrategy
    {
        [Option("配合团队增益召唤；两次增益之间仅用于避免溢出")]
        Automatic,
        [Option("电能量谱达到 50 以上时召唤")]
        Fifty,
        [Option("电能量谱达到 100 时召唤")]
        Hundred,
        [Option("不使用")]
        Delay
    }

    public enum ChargeStrategy
    {
        [Option("在过热状态与团队增益中消耗；避免溢出", Targets = ActionTargets.Hostile)]
        Automatic,
        [Option("仅用于避免溢出", Targets = ActionTargets.Hostile)]
        Overcap,
        [Option("不使用")]
        Delay
    }

    public enum FlamethrowerStrategy
    {
        [Option("站立不动时在 AOE 中使用")]
        Automatic,
        [Option("不使用")]
        Delay
    }

    public enum PotionStrategy
    {
        [Option("不自动使用")]
        Manual,
        [Option("开怪前并与枪管加热一同使用")]
        AlignWithBurst,
        [Option("开怪前并与团队增益一同使用")]
        AlignWithRaidBuffs,
        [Option("尽快使用")]
        Immediate
    }

    public enum OpenerStrategy
    {
        [Option("标准起手")]
        Standard,
        [Option("提前野火起手", MinLevel = 96)]
        EarlyWildfire,
        [Option("无起手专属规则")]
        None
    }

    public static RotationModuleDefinition Definition()
    {
        return new RotationModuleDefinition("Kage MCH", "机工士", "标准循环（Kage）|远程", "Kagekazu", RotationModuleQuality.Ok, BitMask.Build(Class.MCH), 100).WithStrategies<Strategy>();
    }

    private int Heat;
    private int Battery;
    private bool Overheated;
    private bool HasMinion;

    private float ReassembleLeft;
    private float WildfireLeft;
    private float HyperchargedLeft;
    private float ExcavatorLeft;
    private float FMFLeft;

    private int NumConeTargets;
    private Actor? BestConeTarget;
    private Actor? BestSawTarget;
    private Actor? BestSplashTarget;
    private bool AOEMode;
    private bool UseBioblaster;
    private bool LowTarget;

    private OpenerStrategy OpenerMode;

    private AID ComboLastMove => (AID)World.Client.ComboState.Action;
    private AID NextGCD => NextGCDAction.As<AID>();
    private bool InOpener => OpenerMode != OpenerStrategy.None && CombatTime < 30;
    private bool StandardOpener => InOpener && OpenerMode == OpenerStrategy.Standard;
    private bool EarlyWFOpener => InOpener && OpenerMode == OpenerStrategy.EarlyWildfire;
    private bool InBurst => RaidBuffsLeft > GCD || WildfireLeft > 0;
    private float BurstIn => InBurst ? 0 : ReadyIn(AID.Wildfire);

    private WildfireStrategy WildfireStrat;
    private OffensiveStrategy StabilizerStrat;
    private OffensiveStrategy ToolsStrat;
    private ReassembleStrategy ReassembleStrat;
    private QueenStrategy QueenStrat;

    public override void Execute(in Strategy strategy, ref Actor? primaryTarget, float estimatedAnimLockDelay, bool isMoving)
    {
        var target = SelectTarget(strategy.Targeting.Value, ref primaryTarget, estimatedAnimLockDelay, 25, strategy.AOE.Value);
        WildfireStrat = strategy.Burst.Value switch { OffensiveStrategy.Delay => WildfireStrategy.Delay, OffensiveStrategy.Force => WildfireStrategy.Force, _ => strategy.Wildfire.Value };
        StabilizerStrat = WithBurst(strategy.Burst.Value, strategy.Stabilizer.Value);
        ToolsStrat = WithBurst(strategy.Burst.Value, strategy.Tools.Value);
        ReassembleStrat = strategy.Burst.Value switch { OffensiveStrategy.Delay => ReassembleStrategy.Delay, _ => strategy.Reassemble.Value };
        QueenStrat = strategy.Burst.Value switch { OffensiveStrategy.Delay => QueenStrategy.Delay, OffensiveStrategy.Force => QueenStrategy.Fifty, _ => strategy.Queen.Value };

        OpenerMode = strategy.Opener.Value switch
        {
            _ when !Unlocked(AID.ChainSaw) => OpenerStrategy.None,
            OpenerStrategy.EarlyWildfire when !Unlocked(AID.Excavator) => OpenerStrategy.None,
            var o => o
        };

        var gauge = World.Client.GetGauge<MachinistGauge>();
        Heat = gauge.Heat;
        Battery = gauge.Battery;
        Overheated = (gauge.TimerActive & 1) != 0;
        HasMinion = (gauge.TimerActive & 2) != 0;

        ReassembleLeft = SelfStatusLeft(SID.Reassembled);
        WildfireLeft = SelfStatusLeft(SID.WildfirePlayer);
        HyperchargedLeft = SelfStatusLeft(SID.Hypercharged);
        ExcavatorLeft = SelfStatusLeft(SID.ExcavatorReady);
        FMFLeft = SelfStatusLeft(SID.FullMetalMachinist);

        if (SelfStatusLeft(SID.Flamethrower) > 0)
            return;

        var allowAoE = strategy.AOE.Value is AOEStrategy.AOE or AOEStrategy.ForceAOE;
        var minAoETargets = Unlocked(AID.Scattergun) ? 3 : 2;
        (BestConeTarget, NumConeTargets) = BestAOETarget(target, 12, allowAoE, (t, e) => TargetInAOECone(e, Player.Position, 12, Player.DirectionTo(t), 60.Degrees()));
        BestSawTarget = BestAOETarget(target, 25, allowAoE, (t, e) => TargetInAOERect(e, Player.Position, Player.DirectionTo(t), 25, 2)).Best;
        BestSplashTarget = BestAOETarget(target, 25, allowAoE, (t, e) => TargetInAOECircle(e, t.Position, 5)).Best;
        AOEMode = Unlocked(AID.SpreadShot) && Player.DistanceToHitbox(BestConeTarget) <= 12 && UseAOE(strategy.AOE.Value, NumConeTargets, minAoETargets);

        if (target != null)
        {
            var goal = Hints.GoalSingleTarget(target.Actor, Player, World.Actors, 25);
            Hints.GoalZones.Add(allowAoE && Unlocked(AID.SpreadShot) ? GoalCombined(goal, Hints.GoalAOECone(BestConeTarget ?? target.Actor, 12, 60.Degrees()), minAoETargets) : goal);
        }

        if (UsePotion(strategy))
            Hints.ActionsToExecute.Push(ActionDefinitions.IDPotionDex, Player, ActionQueue.Priority.Medium);

        if (World.Client.CountdownRemaining is > 0 and var countdown)
        {
            if (countdown < 5 && ReassembleLeft == 0 && ReadyIn(AID.AirAnchor) <= 0 && ReassembleStrat != ReassembleStrategy.Delay)
                PushGCD(AID.Reassemble, Player, 50);
            if (countdown < 1.15f)
                PushGCD(BestActionUnlocked(AID.AirAnchor, AID.HotShot, AID.HeatedSplitShot, AID.SplitShot), target?.Actor, 10);
            return;
        }

        if (target == null)
            return;

        LowTarget = !TimeToKill.IsBossTier(Bossmods.ActiveModule, Hints, target.Actor) && !TimeToKill.WillLive(target.Actor, 8);
        UseBioblaster = AOEMode && Unlocked(AID.Bioblaster) && BioblasterAllowed(target);

        if (Overheated && Unlocked(AID.HeatBlast))
            OverheatGCD(target);
        else
            NormalGCD(strategy, target);

        if (Player.InCombat)
            OGCD(strategy, target);
    }

    #region GCD

    private void OverheatGCD(Enemy target)
    {
        if (AOEMode && Unlocked(AID.AutoCrossbow) && (!Unlocked(AID.BlazingShot) || NumConeTargets >= 6))
            PushGCD(AID.AutoCrossbow, BestConeTarget ?? target.Actor, 20, faceTarget: true);
        else
            PushGCD(BestActionUnlocked(AID.BlazingShot, AID.HeatBlast), target.Actor, 20);
    }

    private void NormalGCD(in Strategy strategy, Enemy target)
    {
        var tools = ToolsStrat;
        if (tools != OffensiveStrategy.Delay && target.Priority != Enemy.PriorityPointless)
        {
            var bonus = tools == OffensiveStrategy.Force ? 100 : 0;

            if (FMFLeft > GCD && UseFullMetalField())
                PushGCD(AID.FullMetalField, BestSplashTarget ?? target.Actor, 16 + bonus);

            if (ExcavatorLeft > GCD)
                PushGCD(AID.Excavator, BestSplashTarget ?? target.Actor, 15 + bonus);

            var saveTools = LowTarget && tools != OffensiveStrategy.Force;
            if (GCDReady(AID.ChainSaw) && !saveTools)
                PushGCD(AID.ChainSaw, BestSawTarget ?? target.Actor, 14 + bonus, faceTarget: true);

            if (GCDReady(AID.AirAnchor) && !UseBioblaster && !saveTools)
                PushGCD(AID.AirAnchor, target.Actor, 13 + bonus);

            if (!Unlocked(AID.AirAnchor) && GCDReady(AID.HotShot) && !saveTools)
                PushGCD(AID.HotShot, target.Actor, 13 + bonus);

            if (UseBioblaster && GCDReady(AID.Bioblaster) && !saveTools && StatusDetails(target, SID.Bioblaster, Player.InstanceID, 15).Left == 0)
                PushGCD(AID.Bioblaster, BestConeTarget ?? target.Actor, 12 + bonus, faceTarget: true);

            var drillCapped = MaxChargesIn(AID.Drill) <= GCD;
            if (!SkipDrill && GCDReady(AID.Drill) && !saveTools && (drillCapped || !HoldDrillForBurst))
                PushGCD(AID.Drill, target.Actor, (drillCapped ? 17 : 12) + bonus);
        }

        if (strategy.Flamethrower.Value == FlamethrowerStrategy.Automatic && strategy.AOE.Value is AOEStrategy.AOE or AOEStrategy.ForceAOE && NumConeTargets >= 2 && Hints.MaxCastTime >= 2 && GCDReady(AID.Flamethrower) && ReassembleLeft == 0)
            PushGCD(AID.Flamethrower, BestConeTarget ?? target.Actor, 5, faceTarget: true);

        if (AOEMode)
        {
            PushGCD(BestActionUnlocked(AID.Scattergun, AID.SpreadShot), BestConeTarget ?? target.Actor, 1, faceTarget: true);
            return;
        }

        if (ComboLeft > 0)
        {
            if (ComboLastMove is AID.SlugShot or AID.HeatedSlugShot && Unlocked(AID.CleanShot))
            {
                PushGCD(BestActionUnlocked(AID.HeatedCleanShot, AID.CleanShot), target.Actor, 1);
                return;
            }
            if (ComboLastMove is AID.SplitShot or AID.HeatedSplitShot && Unlocked(AID.SlugShot))
            {
                PushGCD(BestActionUnlocked(AID.HeatedSlugShot, AID.SlugShot), target.Actor, 1);
                return;
            }
        }
        PushGCD(BestActionUnlocked(AID.HeatedSplitShot, AID.SplitShot), target.Actor, 1);
    }

    private bool BioblasterAllowed(Enemy target)
    {
        var dir = Player.DirectionTo(BestConeTarget ?? target.Actor);
        return !Hints.PriorityTargets.Any(e => e.ForbidDOTs && TargetInAOECone(e.Actor, Player.Position, 12, dir, 60.Degrees()));
    }

    private bool SkipDrill => UseBioblaster || AOEMode && !Unlocked(AID.Bioblaster) && NumConeTargets >= 6;

    private bool HoldDrillForBurst
        => HasRaidBuffJobs && !InOpener && BurstIn > 0 && BurstIn < 20 && !BurstStarted && MaxChargesIn(AID.Drill) > BurstIn + GCDLength * 2;

    private float FortySecondToolIn => Unlocked(AID.AirAnchor) ? ReadyIn(AID.AirAnchor) : ReadyIn(AID.HotShot);

    private bool BurstStarted => Unlocked(AID.BarrelStabilizer) && ReadyIn(AID.BarrelStabilizer) > 90;

    private bool UseFullMetalField()
    {
        if (StandardOpener)
            return WildfireLeft > 0;
        if (EarlyWFOpener)
            return !Overheated && HyperchargedLeft == 0 && WildfireLeft == 0;
        return ReadyIn(AID.Wildfire) <= GCD + GCDLength || WildfireLeft > 0 || FMFLeft < GCDLength * 3;
    }

    #endregion

    #region oGCD

    private void OGCD(in Strategy strategy, Enemy target)
    {
        var dying = target.Priority == Enemy.PriorityPointless;

        if (HasMinion && (dying || !TimeToKill.WillLive(target.Actor, 2)) && QueenStrat != QueenStrategy.Delay)
            PushOGCD(BestActionUnlocked(AID.QueenOverdrive, AID.RookOverdrive), Player, 80);

        if (ShouldHypercharge(strategy, dying, target.Actor))
            PushOGCD(AID.Hypercharge, Player, 70);

        if (ShouldWildfire(strategy, dying, target.Actor))
            PushOGCD(AID.Wildfire, ResolveTarget(strategy.Wildfire) ?? target.Actor, 75, WildfireStrat == WildfireStrategy.Force ? 0 : GCD - 0.8f);

        if (ShouldStabilize(strategy, dying, target.Actor))
            PushOGCD(AID.BarrelStabilizer, Player, 60);

        if (ShouldReassemble(strategy))
            PushOGCD(AID.Reassemble, Player, 55);

        if (ShouldQueen(strategy, dying, target.Actor))
            PushOGCD(AID.RookAutoturret, Player, 57);

        if (Manager.LastCast.Data?.Action.ID is (uint)AID.AutomatonQueen or (uint)AID.RookAutoturret && World.CurrentTime - Manager.LastCast.Time < TimeSpan.FromSeconds(3)
            && Hints.PriorityTargets.Count() > 1 && Player.DistanceToHitbox(target.Actor) <= 25)
            PushOGCD(AID.LegGraze, target.Actor, 90);

        UseCharges(strategy, ResolveTarget(strategy.Charges) ?? BestSplashTarget ?? target.Actor, dying);
    }

    private bool ShouldHypercharge(in Strategy strategy, bool dying, Actor target)
    {
        if (!Unlocked(AID.Hypercharge) || Overheated || HyperchargedLeft == 0 && Heat < 50 || !CanWeave(AID.Hypercharge))
            return false;

        switch (strategy.Hypercharge.Value)
        {
            case HyperchargeStrategy.Delay:
                return false;
            case HyperchargeStrategy.ASAP:
                return true;
        }

        if (StandardOpener && WildfireLeft > 0 && FMFLeft == 0 || EarlyWFOpener && WildfireLeft > 0 && ExcavatorLeft == 0)
            return true;

        if (dying || LowTarget || ReassembleLeft > 0)
            return false;

        if (strategy.Charges.Value == ChargeStrategy.Automatic && ChargesToSpendBeforeOverheat)
            return false;

        var toolsDelayed = ToolsStrat == OffensiveStrategy.Delay;
        var drillIn = ReadyIn(AID.Wildfire) <= GCD + GCDLength ? float.MaxValue
            : UseBioblaster ? MathF.Max(ReadyIn(AID.Bioblaster), StatusDetails(target, SID.Bioblaster, Player.InstanceID, 15).Left)
            : SkipDrill ? float.MaxValue
            : ReadyIn(AID.Drill);
        var airAnchorIn = UseBioblaster && Unlocked(AID.AirAnchor) ? float.MaxValue : FortySecondToolIn;
        var toolIn = toolsDelayed ? float.MaxValue : MathF.Min(drillIn, MathF.Min(airAnchorIn, ReadyIn(AID.ChainSaw)));
        if (!toolsDelayed && (FMFLeft > 0 || ExcavatorLeft > 0) || toolIn < GCD + GCDLength * 3 + 0.5f)
            return false;

        if (DowntimeIn < GCD + GCDLength * 5)
            return false;

        if (ComboLeft is > 0 and < 7.6f)
            return false;

        if (!Unlocked(AID.Wildfire) || WildfireStrat == WildfireStrategy.Delay || !WildfireTargetWorthIt(target))
            return true;

        if (ReadyIn(AID.Wildfire) <= GCD)
            return true;

        if (HyperchargedLeft > 0)
            return ReadyIn(AID.Wildfire) > 10 || HyperchargedLeft < GCDLength * 2;

        return ReadyIn(AID.Wildfire) > GCDLength * 15 || Heat >= 95;
    }

    private bool ChargesToSpendBeforeOverheat => MaxChargesIn(AID.GaussRound) <= 30 || MaxChargesIn(AID.Ricochet) <= 30;

    private bool ShouldWildfire(in Strategy strategy, bool dying, Actor target)
    {
        if (!Unlocked(AID.Wildfire) || !CanWeave(AID.Wildfire))
            return false;
        return WildfireStrat switch
        {
            WildfireStrategy.Force => true,
            WildfireStrategy.Automatic => !dying && WildfireLeft == 0 && (Overheated || OpenerWildfire) && WildfireTargetWorthIt(target),
            _ => false
        };
    }

    private bool WildfireTargetWorthIt(Actor target) => TimeToKill.BurstWorthIt(Bossmods.ActiveModule, Hints, target, 12);

    private bool OpenerWildfire
        => StandardOpener && FMFLeft > 0 && ExcavatorLeft == 0 && !GCDReady(AID.Drill) && !GCDReady(AID.ChainSaw)
        || EarlyWFOpener && ExcavatorLeft > 0;

    private bool ShouldStabilize(in Strategy strategy, bool dying, Actor target)
    {
        if (!Unlocked(AID.BarrelStabilizer) || !CanWeave(AID.BarrelStabilizer) || FMFLeft > 0 || HyperchargedLeft > 0)
            return false;
        return StabilizerStrat switch
        {
            OffensiveStrategy.Force => true,
            OffensiveStrategy.Automatic => !dying && WildfireTargetWorthIt(target) && (!Unlocked(AID.Wildfire) || ReadyIn(AID.Wildfire) <= 20),
            _ => false
        };
    }

    private bool ShouldReassemble(in Strategy strategy)
    {
        if (ReassembleStrat == ReassembleStrategy.Delay || LowTarget || ReassembleLeft > 0 || Overheated || !CanWeave(AID.Reassemble))
            return false;

        if (StandardOpener && Unlocked(AID.Excavator))
            return NextGCD == AID.Drill && MaxChargesIn(AID.Drill) > GCD;
        if (EarlyWFOpener)
            return NextGCD == AID.ChainSaw;

        if (HasRaidBuffJobs && Unlocked(AID.Excavator))
        {
            if (NextGCD == AID.Excavator)
                return true;
            if (BurstStarted || Unlocked(AID.Wildfire) && BurstIn < 15)
                return false;
            if (MaxChargesIn(AID.Reassemble) > GCD + GCDLength)
                return false;
        }

        if (HasRaidBuffJobs && !InBurst && MaxChargesIn(AID.Reassemble) > GCD + GCDLength && MaxChargesIn(AID.Reassemble) + 55 > BurstIn)
            return false;

        var aoeFiller = AOEMode && NumConeTargets >= AoEReassembleTargets;
        return NextGCD switch
        {
            AID.Drill or AID.AirAnchor or AID.ChainSaw or AID.Excavator => !aoeFiller,
            AID.CleanShot or AID.HeatedCleanShot => !Unlocked(AID.Drill),
            AID.HotShot => !Unlocked(AID.CleanShot),
            AID.SpreadShot or AID.Scattergun => aoeFiller || ReassembleStrat == ReassembleStrategy.Any,
            _ => false
        };
    }

    private int AoEReassembleTargets => Unlocked(AID.ChainSaw) ? int.MaxValue
        : Unlocked(AID.Scattergun) ? 5
        : Unlocked(AID.AirAnchor) ? 6
        : Unlocked(AID.Bioblaster) ? 3
        : Unlocked(AID.Drill) ? 6
        : 3;

    private bool ShouldQueen(in Strategy strategy, bool dying, Actor target)
    {
        if (!Unlocked(AID.RookAutoturret) || HasMinion || Battery < 50 || dying || !CanWeave(AID.RookAutoturret))
            return false;

        return QueenStrat switch
        {
            QueenStrategy.Fifty => true,
            QueenStrategy.Hundred => Battery == 100,
            QueenStrategy.Automatic => TimeToKill.WillLive(target, 10) && AutoQueen(target),
            _ => false
        };
    }

    private const float QueenLife = 24;
    private const float QueenLeadIn = 5;
    private const float BuffWeight = 1.15f;

    private bool AutoQueen(Actor target)
    {
        var capNext = Battery + BatteryFrom(NextGCD) > 100;

        if (TimeToKill.Estimate(target) is > 8f and < 40f)
            return true;
        if (DowntimeIn < QueenLife - 6 && !capNext)
            return false;
        if (!HasRaidBuffJobs || !Unlocked(AID.Wildfire))
            return capNext || RaidBuffsLeft > GCD;
        if (!Unlocked(AID.AutomatonQueen))
            return capNext || RaidBuffsLeft > GCD || RaidBuffsIn <= 3;

        if (RaidBuffsLeft > 8 || RaidBuffsIn <= QueenLeadIn)
            return true;

        var windowIn = RaidBuffsIn - QueenLeadIn;
        var income = BatteryIncome(windowIn);
        if (windowIn < QueenLife + 1)
        {
            if (capNext)
                return true;
            if (Battery < 50 || Battery + income <= 100)
                return false;
            const float buffBonus = BuffWeight - 1;
            const float buffLength = 20;
            var inBuffsNow = Overlap(0.8f, QueenLife, RaidBuffsIn, buffLength) / QueenLife;
            var inBuffsIdeal = buffLength / QueenLife;
            return Battery * (1 + buffBonus * inBuffsNow) + income > MathF.Min(100, Battery + income) * (1 + buffBonus * inBuffsIdeal) + 5;
        }

        return capNext || SpendNow(Battery, income, 100, CombatTime < 120 ? 90 : 50);
    }

    private static bool SpendNow(float have, float income, float cap, float minSpend, float margin = 5)
    {
        if (have < minSpend || have + income <= cap)
            return false;
        return have + BuffWeight * MathF.Min(cap, income) > BuffWeight * cap + margin;
    }

    private static float Overlap(float start, float length, float buffStart, float buffLength)
        => MathF.Max(0, MathF.Min(start + length, buffStart + buffLength) - MathF.Max(start, buffStart));

    private float BatteryIncome(float within)
    {
        var gcdLength = GCDLength;
        var heat = Heat;
        var step = ComboLeft > 0 ? ComboLastMove switch { AID.HeatedSplitShot or AID.SplitShot => 1, AID.HeatedSlugShot or AID.SlugShot => 2, _ => 0 } : 0;
        var airAnchorIn = FortySecondToolIn;
        var chainSawIn = ReadyIn(AID.ChainSaw);
        var drillIn = ReadyIn(AID.Drill);
        var wildfireIn = ReadyIn(AID.Wildfire);
        var excavator = ExcavatorLeft > 0;
        var overheat = Overheated ? 3 : 0;
        var battery = 0f;

        for (var t = GCD; t <= within + 0.01f; t += gcdLength)
        {
            if (excavator && overheat == 0)
            {
                battery += 20;
                excavator = false;
            }
            else if (overheat > 0)
                overheat--;
            else if (airAnchorIn <= t + 0.01f)
            {
                battery += 20;
                airAnchorIn = t + 40;
            }
            else if (chainSawIn <= t + 0.01f)
            {
                battery += 20;
                excavator = Unlocked(AID.Excavator);
                chainSawIn = t + 60;
            }
            else if (drillIn <= t + 0.01f)
                drillIn = t + 20;
            else if (heat >= 50 && (heat >= 95 || wildfireIn - t > gcdLength * 15) && MathF.Min(airAnchorIn, MathF.Min(chainSawIn, drillIn)) - t > gcdLength * 3)
            {
                heat -= 50;
                overheat = 4;
            }
            else
            {
                heat += 5;
                if (step == 2)
                    battery += 10;
                step = (step + 1) % 3;
            }
        }
        return battery;
    }

    private static int BatteryFrom(AID action) => action switch
    {
        AID.ChainSaw or AID.AirAnchor or AID.Excavator or AID.HotShot => 20,
        AID.CleanShot or AID.HeatedCleanShot => 10,
        _ => 0
    };

    private void UseCharges(in Strategy strategy, Actor target, bool dying)
    {
        if (strategy.Charges.Value == ChargeStrategy.Delay)
            return;

        var capIn = GCD + 0.6f + (NextGCD is AID.HeatBlast or AID.BlazingShot ? 15 : 0);
        var gauss = MaxChargesIn(AID.GaussRound);
        var rico = MaxChargesIn(AID.Ricochet);
        if (gauss <= capIn && CanWeave(AID.GaussRound))
            PushOGCD(AID.GaussRound, target, gauss <= rico ? 45 : 44);
        if (rico <= capIn && CanWeave(AID.Ricochet))
            PushOGCD(AID.Ricochet, target, rico < gauss ? 45 : 44);

        if (strategy.Charges.Value != ChargeStrategy.Automatic)
            return;

        if (!Overheated && (Heat >= 50 || HyperchargedLeft > 0))
            foreach (var aid in (ReadOnlySpan<AID>)[AID.GaussRound, AID.Ricochet])
                if (MaxChargesIn(aid) <= 30 && CanWeave(aid))
                    PushOGCD(aid, target, 46);

        var spend = Overheated || WildfireLeft > 0 || dying || InOpener || RaidBuffsLeft > GCD || !HasRaidBuffJobs || !Unlocked(AID.Hypercharge);
        if (!spend)
            return;

        if (CanWeave(AID.GaussRound))
            PushOGCD(AID.GaussRound, target, gauss <= rico ? 11 : 10);
        if (CanWeave(AID.Ricochet))
            PushOGCD(AID.Ricochet, target, rico < gauss ? 11 : 10);
    }

    #endregion

    #region Helpers

    private bool UsePotion(in Strategy strategy) => strategy.Potion.Value switch
    {
        PotionStrategy.AlignWithBurst => PotionPrepull || Player.InCombat && ReadyIn(AID.BarrelStabilizer) < 6,
        PotionStrategy.AlignWithRaidBuffs => PotionWithRaidBuffs,
        PotionStrategy.Immediate => true,
        _ => false
    };

    private void PushGCD(AID aid, Actor? target, int priority, bool faceTarget = false)
        => base.PushGCD(aid, target, priority, facing: faceTarget && target != null ? Player.AngleTo(target) : null);

    #endregion
}
