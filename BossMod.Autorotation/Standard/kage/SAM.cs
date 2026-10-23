using BossMod.SAM;
using FFXIVClientStructs.FFXIV.Client.Game.Gauge;
using static BossMod.AIHints;

namespace BossMod.Autorotation.kage;

public sealed class KageSAM(RotationModuleManager manager, Actor player) : KageRotation<KageSAM.Strategy>(manager, player)
{
    public struct Strategy
    {
        public Track<Targeting> Targeting;
        public Track<AOEStrategy> AOE;

        [Track("循环", MinLevel = 76, UiPriority = -5)]
        public Track<LoopStrategy> Loop;

        [Track("明镜止水", MinLevel = 50, Action = AID.MeikyoShisui)]
        public Track<OffensiveStrategy> Meikyo;

        [Track("彼岸花", MinLevel = 30, Action = AID.Higanbana)]
        public Track<HiganbanaStrategy> Higanbana;

        [Track("回返", MinLevel = 74, Actions = [AID.KaeshiSetsugekka, AID.TendoKaeshiSetsugekka, AID.KaeshiGoken, AID.TendoKaeshiGoken])]
        public Track<TsubameStrategy> Tsubame;

        [Track("爆发（意气冲天、必杀剑·闪影、明镜止水、照破）", InternalName = "Burst", MinLevel = 50)]
        public Track<OffensiveStrategy> Burst;

        [Track("意气冲天 / 奥义斩浪 / 残心", MinLevel = 68, Actions = [AID.Ikishoten, AID.OgiNamikiri, AID.Zanshin])]
        public Track<OffensiveStrategy> Ikishoten;

        [Track("必杀剑·闪影 / 必杀剑·红莲", MinLevel = 70, Actions = [AID.HissatsuSenei, AID.HissatsuGuren])]
        public Track<OffensiveStrategy> Senei;

        [Track("照破", MinLevel = 80, Action = AID.Shoha)]
        public Track<OffensiveStrategy> Shoha;

        [Track("必杀剑·震天 / 必杀剑·九天", MinLevel = 52, Actions = [AID.HissatsuShinten, AID.HissatsuKyuten])]
        public Track<KenkiStrategy> Kenki;

        [Track("必杀剑·晓天（冲刺）", MinLevel = 54, Action = AID.HissatsuGyoten)]
        public Track<DashStrategy> Dash;

        [Track("必杀剑·夜天（后撤）", MinLevel = 56, Action = AID.HissatsuYaten)]
        public Track<YatenStrategy> Yaten;

        [Track("真北", MinLevel = 50, Action = ClassShared.AID.TrueNorth)]
        public Track<TrueNorthStrategy> TrueNorth;

        [Track("燕飞", MinLevel = 15, Action = AID.Enpi)]
        public Track<EnpiStrategy> Enpi;

        [Track("默想", MinLevel = 60, Action = AID.Meditate)]
        public Track<MeditateStrategy> Meditate;

        [Track("交战")]
        public Track<EngageStrategy> Engage;

        [Track("爆发药")]
        public Track<PotionStrategy> Potion;

        [Track("起手", MinLevel = 50, UiPriority = -10)]
        public Track<OpenerStrategy> Opener;
    }

    public enum LoopStrategy
    {
        [Option("根据 Fuka 按 GCD 选择：GCD 为 2.11 秒或更慢时使用明镜止水加速循环，更快时使用标准循环")]
        Automatic,
        [Option("明镜止水加速循环（2.14 秒 GCD）")]
        MAL,
        [Option("标准循环（2.07-2.08 秒 GCD）")]
        Standard
    }

    public enum HiganbanaStrategy
    {
        [Option("每分钟随爆发刷新；仅对存活 48 秒以上的目标使用")]
        Automatic,
        [Option("尽快使用", Targets = ActionTargets.Hostile)]
        Force,
        [Option("不使用")]
        Delay
    }

    public enum TsubameStrategy
    {
        [Option("为下一次爆发保留回返雪月花；不要让其过期")]
        Automatic,
        [Option("尽快使用")]
        ASAP,
        [Option("不使用")]
        Delay
    }

    public enum KenkiStrategy
    {
        [Option("为团队增益储存，并在增益期间消耗；避免溢出")]
        Automatic,
        [Option("仅用于避免溢出")]
        Overcap,
        [Option("不使用")]
        Delay
    }

    public enum DashStrategy
    {
        [Option("衔接空隙，并在团队增益内消耗多余剑气", Targets = ActionTargets.Hostile)]
        Automatic,
        [Option("仅用于衔接空隙", Targets = ActionTargets.Hostile)]
        GapClose,
        [Option("不使用")]
        Delay
    }

    public enum YatenStrategy
    {
        [Option("落点安全时后撤躲开即将到来的 AOE，然后使用 Enhanced Enpi")]
        Automatic,
        [Option("不使用")]
        Delay
    }

    public enum TrueNorthStrategy
    {
        [Option("下一个方位错误时使用")]
        Automatic,
        [Option("不使用")]
        Delay
    }

    public enum EnpiStrategy
    {
        [Option("超出近战距离时使用", Targets = ActionTargets.Hostile)]
        Ranged,
        [Option("不使用")]
        Delay
    }

    public enum MeditateStrategy
    {
        [Option("空档期站立不动时使用")]
        Automatic,
        [Option("不使用")]
        Delay
    }

    public enum EngageStrategy
    {
        [Option("倒计时期间走入近战距离，并在开怪时到达")]
        WalkIn,
        [Option("开怪前不移动")]
        Manual
    }

    public enum PotionStrategy
    {
        [Option("不自动使用")]
        Manual,
        [Option("与团队增益一同使用")]
        AlignWithRaidBuffs,
        [Option("尽快使用")]
        Immediate
    }

    public enum OpenerStrategy
    {
        [Option("标准起手（-14 秒使用明镜止水，先用月光）")]
        Standard,
        [Option("先用花车，适用于提前转阶段的战斗")]
        KashaFirst,
        [Option("开怪前不使用明镜止水")]
        None
    }

    private enum Repeat { None, Setsugekka, TendoSetsugekka, Goken, TendoGoken }

    public static RotationModuleDefinition Definition()
    {
        return new RotationModuleDefinition("Kage SAM", "武士", "标准循环（Kage）|近战", "Kagekazu", RotationModuleQuality.Ok, BitMask.Build(Class.SAM), 100).WithStrategies<Strategy>();
    }

    private int Kenki;
    private int Meditation;
    private SenFlags Sen;
    private int SenCount;
    private float FugetsuLeft;
    private float FukaLeft;
    private float MeikyoLeft;
    private int MeikyoStacks;
    private float TendoLeft;
    private (float Left, Repeat Kind) Tsubame;
    private bool OgiRepeat;
    private float OgiLeft;
    private float ZanshinLeft;

    private bool AOEMode;
    private bool ForceST;
    private bool InMelee;
    private int NumCircleTargets;
    private int NumTenkaTargets;
    private Actor? BestLineTarget;
    private int NumLineTargets;
    private Actor? BestConeTarget;
    private Actor? DotTarget;

    private bool WorthBurst;
    private bool BossDying;
    private float SeneiPlannedIn;
    private bool IkishotenDelayed;
    private bool KenkiMakesRoom;
    private DateTime DowntimeStart;
    private DateTime ReopenUntil;

    private AID ComboLastMove => (AID)World.Client.ComboState.Action;
    private AID NextGCD => NextGCDAction.As<AID>();
    private bool HasGetsu => Sen.HasFlag(SenFlags.Getsu);
    private bool HasKa => Sen.HasFlag(SenFlags.Ka);
    private bool HasSetsu => Sen.HasFlag(SenFlags.Setsu);
    private bool HaveBuffs => FugetsuLeft > GCD + IaiCastTime && FukaLeft > GCD;
    private float IaiCastTime => Unlocked(TraitID.EnhancedIaijutsu) ? 1.3f : 1.8f;
    private float FukaGCD => ActionSpeed.GCDRounded(World.Client.PlayerStats.SkillSpeed, Math.Min(World.Client.PlayerStats.Haste, 87), Player.Level);
    private float SeneiIn => ReadyIn(AID.HissatsuSenei);
    private bool SeneiSoon => SeneiPlannedIn < 7;
    private float SeneiCooldown => Unlocked(TraitID.EnhancedHissatsu) ? 60 : 120;
    private bool SeneiJustUsed(float within) => Unlocked(AID.HissatsuSenei) && SeneiIn > SeneiCooldown - within;
    private bool MidCombo => ComboLeft > GCD && ComboLastMove is AID.Hakaze or AID.Gyofu or AID.Jinpu or AID.Shifu or AID.Fuga or AID.Fuko;
    private bool Recovering => CombatTime > 10 && (FugetsuLeft == 0 || FukaLeft == 0);
    private bool Reopening => World.CurrentTime < ReopenUntil;
    private bool MeikyoUnavailable => MeikyoLeft == 0 && (MeikyoStrat == OffensiveStrategy.Delay || ReadyIn(AID.MeikyoShisui) > GCD);
    private bool DowntimeWithin(int gcds) => DowntimeIn < GCD + GCDLength * gcds;

    private OffensiveStrategy IkishotenStrat;
    private OffensiveStrategy SeneiStrat;
    private OffensiveStrategy MeikyoStrat;
    private OffensiveStrategy ShohaStrat;

    public override void Execute(in Strategy strategy, ref Actor? primaryTarget, float estimatedAnimLockDelay, bool isMoving)
    {
        var target = SelectTarget(strategy.Targeting.Value, ref primaryTarget, estimatedAnimLockDelay, 3, strategy.AOE.Value);
        IkishotenStrat = WithBurst(strategy.Burst.Value, strategy.Ikishoten.Value);
        SeneiStrat = WithBurst(strategy.Burst.Value, strategy.Senei.Value);
        MeikyoStrat = WithBurst(strategy.Burst.Value, strategy.Meikyo.Value);
        ShohaStrat = WithBurst(strategy.Burst.Value, strategy.Shoha.Value);
        ForceST = strategy.AOE.Value == AOEStrategy.ForceST;

        var gauge = World.Client.GetGauge<SamuraiGauge>();
        Kenki = gauge.Kenki;
        Meditation = gauge.MeditationStacks;
        Sen = gauge.SenFlags;
        SenCount = (HasGetsu ? 1 : 0) + (HasKa ? 1 : 0) + (HasSetsu ? 1 : 0);
        OgiRepeat = gauge.Kaeshi == KaeshiAction.Namikiri;

        FugetsuLeft = SelfStatusLeft(SID.Fugetsu);
        FukaLeft = SelfStatusLeft(SID.Fuka);
        (MeikyoLeft, MeikyoStacks) = SelfStatusDetails(SID.MeikyoShisui);
        TendoLeft = Unlocked(AID.TendoSetsugekka) ? SelfStatusLeft(SID.Tendo) : 0;
        OgiLeft = SelfStatusLeft(SID.OgiNamikiriReady);
        ZanshinLeft = Unlocked(AID.Zanshin) ? SelfStatusLeft(SID.ZanshinReady) : 0;
        SeneiPlannedIn = SeneiStrat == OffensiveStrategy.Delay ? float.MaxValue : SeneiIn;
        IkishotenDelayed = IkishotenStrat == OffensiveStrategy.Delay;
        KenkiMakesRoom = strategy.Kenki.Value == KenkiStrategy.Automatic;
        Tsubame = ReadTsubame();

        var allowAoE = strategy.AOE.Value is AOEStrategy.AOE or AOEStrategy.ForceAOE;
        NumCircleTargets = ForceST ? 0 : Hints.NumPriorityTargetsInAOECircle(Player.Position, 5);
        NumTenkaTargets = ForceST ? 0 : Hints.NumPriorityTargetsInAOECircle(Player.Position, 8);
        AOEMode = Unlocked(AID.Fuga) && UseAOE(strategy.AOE.Value, NumCircleTargets, 3);
        InMelee = target != null && Player.DistanceToHitbox(target.Actor) <= 3;
        (BestLineTarget, NumLineTargets) = BestAOETarget(target, 10, allowAoE, (c, e) => TargetInAOERect(e, Player.Position, Player.DirectionTo(c), 10, 2));
        BestConeTarget = BestAOETarget(target, 8, allowAoE, (c, e) => TargetInAOECone(e, Player.Position, 8, Player.DirectionTo(c), 60.Degrees())).Best;

        if (UsePotion(strategy))
            Hints.ActionsToExecute.Push(ActionDefinitions.IDPotionStr, Player, ActionQueue.Priority.Medium);

        if (!Player.InCombat)
            DowntimeStart = default;

        if (World.Client.CountdownRemaining is > 0 and var countdown)
        {
            Prepull(strategy, target, countdown);
            return;
        }

        if (target == null)
        {
            if (Player.InCombat && DowntimeStart == default)
                DowntimeStart = World.CurrentTime;
            if (strategy.Meditate.Value == MeditateStrategy.Automatic && Player.InCombat && Hints.MaxCastTime > 3 && ReadyIn(AID.Meditate) <= GCD)
                PushGCD(AID.Meditate, Player, 1);
            return;
        }

        if (DowntimeStart != default)
        {
            if ((World.CurrentTime - DowntimeStart).TotalSeconds > 5)
                ReopenUntil = World.FutureTime(GCDLength * 2);
            DowntimeStart = default;
        }

        WorthBurst = TimeToKill.BurstWorthIt(Bossmods.ActiveModule, Hints, target.Actor, 10);
        BossDying = Bossmods.ActiveModule?.PrimaryActor == target.Actor && !TimeToKill.WillLive(target.Actor, 5);
        DotTarget = SelectDotTarget(strategy, target);

        GCDs(strategy, target);
        UpdatePositionals(strategy, target);
        AddGoalZone(target, allowAoE);
        if (Player.InCombat)
            OGCDs(strategy, target);
    }

    private void Prepull(in Strategy strategy, Enemy? target, float countdown)
    {
        if (target == null)
            return;
        if (strategy.Engage.Value == EngageStrategy.WalkIn && !InMelee && countdown < (Player.DistanceToHitbox(target.Actor) - 3) / 6)
            Hints.ForcedMovement = Player.DirectionTo(target.Actor).ToVec3();
        if (strategy.Opener.Value == OpenerStrategy.None || !Unlocked(AID.MeikyoShisui))
            return;
        if (MeikyoLeft == 0 && countdown < 14)
            PushGCD(AID.MeikyoShisui, Player, 10);
        if (countdown < 5 && SelfStatusLeft(ClassShared.SID.TrueNorth) == 0 && HasPositionals(target.Actor) && strategy.TrueNorth.Value == TrueNorthStrategy.Automatic)
            PushOGCD(ClassShared.AID.TrueNorth, Player, 10);
        if (MeikyoLeft > countdown && countdown < 0.76f)
            PushGCD(strategy.Opener.Value == OpenerStrategy.KashaFirst ? AID.Kasha : AID.Gekko, target.Actor, 10);
    }

    #region GCD

    private void GCDs(in Strategy strategy, Enemy target)
    {
        if (OgiRepeat)
            PushGCD(AID.KaeshiNamikiri, BestConeTarget ?? target.Actor, 85);

        if (Reopening && SenCount < 3 && MeikyoLeft == 0 && TendoLeft == 0 && WorthBurst && strategy.Meikyo.Value != OffensiveStrategy.Delay
            && Unlocked(AID.MeikyoShisui) && ReadyIn(AID.MeikyoShisui) <= GCD)
            PushAction(ActionID.MakeSpell(AID.MeikyoShisui), Player, ActionQueue.Priority.High + 90);

        UseTsubame(strategy, target);
        UseIaijutsu(strategy, target);
        UseOgi(strategy, target);

        if (MeikyoLeft > GCD && MeikyoStacks > 0)
            PushGCD(MeikyoFinisher(strategy), target.Actor, 40);

        if (AOEMode)
            ComboAoE(target);
        else
            ComboST(target);

        if (!InMelee && strategy.Enpi.Value == EnpiStrategy.Ranged && Player.DistanceToHitbox(target.Actor) <= 20)
            PushGCD(AID.Enpi, target.Actor, 5);
    }

    private (float, Repeat) ReadTsubame()
    {
        if (!Unlocked(AID.TsubameGaeshi))
            return (0, Repeat.None);
        float Left(SID sid) => SelfStatusLeft(sid);
        if (Left(SID.TendoKaeshiSetsugekka) is > 0 and var ts)
            return (ts, Repeat.TendoSetsugekka);
        if (Left(SID.TendoKaeshiGoken) is > 0 and var tg)
            return (tg, Repeat.TendoGoken);
        if (Left(SID.KaeshiGoken) is > 0 and var g)
            return (g, Repeat.Goken);
        if (Left(SID.KaeshiSetsugekka) is > 0 and var s)
            return (s, Repeat.Setsugekka);
        return (0, Repeat.None);
    }

    private void UseTsubame(in Strategy strategy, Enemy target)
    {
        if (Tsubame.Kind == Repeat.None || strategy.Tsubame.Value == TsubameStrategy.Delay || ForceST && Tsubame.Kind is Repeat.Goken or Repeat.TendoGoken)
            return;

        switch (Tsubame.Kind)
        {
            case Repeat.TendoSetsugekka:
                PushGCD(AID.TendoKaeshiSetsugekka, target.Actor, 75);
                return;
            case Repeat.TendoGoken:
                PushGCD(AID.TendoKaeshiGoken, Player, 75);
                return;
            case Repeat.Goken:
                PushGCD(AID.KaeshiGoken, Player, 75);
                return;
        }

        var use = strategy.Tsubame.Value == TsubameStrategy.ASAP
            || SenCount == 3
            || Tsubame.Left < GCD + GCDLength
            || !InBossFight || NoRaidBuffs
            || Recovering
            || RaidBuffsLeft > GCD
            || SeneiSoon
            || MeikyoLeft > 0
            || DowntimeWithin(2)
            || !TimeToKill.WillLive(target.Actor, GCD + GCDLength);
        if (use)
            PushGCD(AID.KaeshiSetsugekka, target.Actor, 75);
    }

    private void UseIaijutsu(in Strategy strategy, Enemy target)
    {
        if (!Unlocked(AID.Higanbana))
            return;

        if (SenCount == 1 && DotTarget != null && (HaveBuffs && !(HasSetsu && DowntimeWithin(3)) || strategy.Higanbana.Value == HiganbanaStrategy.Force))
            PushGCD(AID.Higanbana, DotTarget, 80);

        var setsugekka = TendoLeft > GCD ? AID.TendoSetsugekka : AID.MidareSetsugekka;
        if (!HaveBuffs)
        {
            if (!AOEMode && SenCount == 3 && Tsubame.Kind != Repeat.Setsugekka && (TendoLeft > GCD || MeikyoUnavailable))
                PushGCD(setsugekka, target.Actor, 70);
            return;
        }

        if (SenCount == 2 && Unlocked(AID.TenkaGoken) && !ForceST && (NumTenkaTargets >= 3 && AOEMode || !Unlocked(AID.MidareSetsugekka)))
            PushGCD(TendoLeft > GCD ? AID.TendoGoken : AID.TenkaGoken, Player, 70);

        if (SenCount == 3 && Tsubame.Kind != Repeat.Setsugekka && !(TendoLeft == 0 && MeikyoBurstNow(strategy)))
            PushGCD(setsugekka, target.Actor, 70);
    }

    private void UseOgi(in Strategy strategy, Enemy target)
    {
        if (OgiLeft <= GCD || IkishotenStrat == OffensiveStrategy.Delay || !HaveBuffs)
            return;
        if (MeikyoLeft > 0 && MeikyoLeft < GCD + GCDLength * (2 + MeikyoStacks))
            return;
        if (IkishotenStrat != OffensiveStrategy.Force && !WorthBurst && OgiLeft > 8)
            return;

        var dotLeft = DotLeft(target.Actor);
        var use = IkishotenStrat == OffensiveStrategy.Force
            || OgiLeft <= 8
            || dotLeft > 50
            || dotLeft > 15 && (RaidBuffsLeft > GCD || !HasRaidBuffJobs)
            || AOEMode;
        if (use)
            PushGCD(AID.OgiNamikiri, BestConeTarget ?? target.Actor, 78);
    }

    private bool MeikyoBurstNow(in Strategy strategy)
        => Unlocked(AID.TendoSetsugekka) && MeikyoLeft == 0 && MeikyoStrat != OffensiveStrategy.Delay && WorthBurst
        && ReadyIn(AID.MeikyoShisui) <= GCD && DowntimeIn >= GCDLength * 3 && (SeneiSoon || HasRaidBuffJobs && RaidBuffsLeft > GCD);

    private bool MeikyoAoE => Unlocked(AID.Oka) && NumCircleTargets >= 4;

    private AID MeikyoFinisher(in Strategy strategy)
    {
        if (AOEMode && MeikyoAoE)
            return !HasGetsu || FugetsuLeft <= FukaLeft && HasKa ? AID.Mangetsu : AID.Oka;

        if (strategy.Opener.Value == OpenerStrategy.KashaFirst && CombatTime < 10 && FukaLeft == 0 && Unlocked(AID.Kasha))
            return AID.Kasha;
        if (Unlocked(AID.Gekko) && (!HasGetsu || FugetsuLeft < GCD + GCDLength))
            return AID.Gekko;
        if (Unlocked(AID.Kasha) && (!HasKa || FukaLeft < GCD + GCDLength))
            return AID.Kasha;
        if (Unlocked(AID.Yukikaze) && !HasSetsu)
            return AID.Yukikaze;
        return FugetsuLeft <= FukaLeft || !Unlocked(AID.Kasha) ? AID.Gekko : AID.Kasha;
    }

    private void ComboST(Enemy target)
    {
        var starter = Unlocked(AID.Gyofu) ? AID.Gyofu : AID.Hakaze;
        if (ComboLeft > GCD)
        {
            if (ComboLastMove is AID.Hakaze or AID.Gyofu)
            {
                var next = AfterStarter(target.Actor);
                if (next != AID.None)
                {
                    PushGCD(next, target.Actor, next == AID.Yukikaze ? 35 : 30);
                    return;
                }
            }
            if (ComboLastMove == AID.Jinpu && Unlocked(AID.Gekko))
            {
                PushGCD(AID.Gekko, target.Actor, 35);
                return;
            }
            if (ComboLastMove == AID.Shifu && Unlocked(AID.Kasha))
            {
                PushGCD(AID.Kasha, target.Actor, 35);
                return;
            }
        }
        PushGCD(starter, target.Actor, 10);
    }

    private AID AfterStarter(Actor target)
    {
        var refreshFuka = FukaLeft <= FugetsuLeft;
        if (!Unlocked(AID.Gekko))
        {
            if (Unlocked(AID.Shifu) && (FukaLeft == 0 || FugetsuLeft > 0 && refreshFuka))
                return AID.Shifu;
            return Unlocked(AID.Jinpu) ? AID.Jinpu : AID.None;
        }

        if (Unlocked(AID.Yukikaze) && !HasSetsu && (DowntimeWithin(2) || FugetsuLeft > 7 && (FukaLeft > 7 || !Unlocked(AID.Kasha))))
            return AID.Yukikaze;

        var pos = CurrentPositional(target);
        if (Unlocked(AID.Shifu) && (FukaLeft == 0
            || Unlocked(AID.Kasha) && !HasKa && (pos is Positional.Flank or Positional.Front || HasGetsu)
            || SenCount == 3 && refreshFuka))
            return AID.Shifu;
        if (Unlocked(AID.Jinpu))
            return AID.Jinpu;
        return Unlocked(AID.Shifu) ? AID.Shifu : AID.None;
    }

    private void ComboAoE(Enemy target)
    {
        var starter = Unlocked(AID.Fuko) ? AID.Fuko : AID.Fuga;
        if (ComboLeft > GCD && ComboLastMove is AID.Fuga or AID.Fuko)
        {
            var refreshFuka = FukaLeft <= FugetsuLeft;
            if (Unlocked(AID.Oka) && (!HasKa || FukaLeft == 0 || SenCount >= 2 && refreshFuka))
            {
                PushGCD(AID.Oka, Player, 30);
                return;
            }
            if (Unlocked(AID.Mangetsu))
            {
                PushGCD(AID.Mangetsu, Player, 30);
                return;
            }
        }
        PushGCD(starter, BestConeTarget ?? target.Actor, 10);
    }

    private Actor? SelectDotTarget(in Strategy strategy, Enemy target)
    {
        if (strategy.Higanbana.Value == HiganbanaStrategy.Delay)
            return null;
        if (strategy.Higanbana.Value == HiganbanaStrategy.Force)
            return ResolveTarget(strategy.Higanbana) ?? target.Actor;
        if (BanaWanted(target, 48))
            return target.Actor;
        return Hints.PriorityTargets.Where(e => e != target && Player.DistanceToHitbox(e.Actor) <= 6 && BanaWanted(e, 30)).MaxBy(e => e.Actor.HPMP.CurHP)?.Actor;
    }

    private bool BanaWanted(Enemy e, float minLife)
    {
        if (e.ForbidDOTs || DowntimeIn < 15)
            return false;
        if (!TimeToKill.WorthDot(Bossmods.ActiveModule, e.Actor, minLife))
            return false;
        var left = DotLeft(e.Actor);
        if (left == 0)
            return true;
        if (left > 15)
            return false;
        if (left <= GCD + GCDLength)
            return true;
        if (SeneiSoon)
            return false;
        if (Unlocked(TraitID.EnhancedHissatsu))
            return SeneiJustUsed(35) || Unlocked(AID.Ikishoten) && ReadyIn(AID.Ikishoten) > 85;
        return true;
    }

    private float DotLeft(Actor target) => StatusDetails(target, SID.Higanbana, Player.InstanceID, 60).Left;

    #endregion

    #region oGCD

    private void OGCDs(in Strategy strategy, Enemy target)
    {
        UseMeikyo(strategy, target);

        if (SeneiStrat != OffensiveStrategy.Delay && Kenki >= 25 && (SeneiStrat == OffensiveStrategy.Force || (WorthBurst || BossDying) && SeneiWanted()))
        {
            var aid = !ForceST && (NumLineTargets >= 2 && Unlocked(AID.HissatsuGuren) || !Unlocked(AID.HissatsuSenei)) ? AID.HissatsuGuren : AID.HissatsuSenei;
            if (CanWeave(aid))
                PushOGCD(aid, aid == AID.HissatsuGuren ? BestLineTarget ?? target.Actor : target.Actor, 60);
        }

        if (IkishotenStrat != OffensiveStrategy.Delay && CanWeave(AID.Ikishoten) && ZanshinLeft == 0
            && (IkishotenStrat == OffensiveStrategy.Force || (Kenki <= 50 || !KenkiMakesRoom) && WorthBurst && DowntimeIn > 10 && (!Unlocked(AID.HissatsuSenei) || SeneiJustUsed(20) || SeneiPlannedIn <= GCD + GCDLength * 2)))
            PushOGCD(AID.Ikishoten, Player, 58);

        if (IkishotenStrat != OffensiveStrategy.Delay && ZanshinLeft > 0 && Kenki >= 50 && CanWeave(AID.Zanshin)
            && (ZanshinLeft <= 8 || BossDying || WorthBurst && (RaidBuffsLeft > GCD || !HasRaidBuffJobs || RaidBuffsIn > ZanshinLeft)))
            PushOGCD(AID.Zanshin, BestConeTarget ?? target.Actor, 56);

        if (ShohaStrat != OffensiveStrategy.Delay && Meditation >= 3 && CanWeave(AID.Shoha)
            && (ShohaStrat == OffensiveStrategy.Force || BossDying || GrantsMeditation(NextGCD) || RaidBuffsLeft > GCD || !HasRaidBuffJobs && !SeneiSoon))
            PushOGCD(AID.Shoha, BestLineTarget ?? target.Actor, 54);

        if (strategy.Kenki.Value != KenkiStrategy.Delay && ShouldSpendKenki(strategy))
        {
            if (NumCircleTargets >= 3 && CanWeave(AID.HissatsuKyuten))
                PushOGCD(AID.HissatsuKyuten, Player, 50);
            else if (CanWeave(AID.HissatsuShinten))
                PushOGCD(AID.HissatsuShinten, target.Actor, 50);
        }

        if (BossDying && SenCount is > 0 and < 3 && Kenki <= 100 - 10 * SenCount && CanWeave(AID.Hagakure))
            PushOGCD(AID.Hagakure, Player, 52);

        UseYaten(strategy, target);
        UseGyoten(strategy, target);
    }

    private void UseGyoten(in Strategy strategy, Enemy target)
    {
        if (strategy.Dash.Value == DashStrategy.Delay || Kenki < 10 || !CanWeave(AID.HissatsuGyoten))
            return;
        var dashTarget = ResolveTarget(strategy.Dash) ?? target.Actor;
        var dist = Player.DistanceToHitbox(dashTarget);
        if (dist is > 3 and <= 20)
        {
            if (Hints.MaxCastTime > 0 && !Hints.ForbiddenZones.Any(z => z.activation <= World.FutureTime(3) && z.shape.Check(dashTarget.Position)))
                PushOGCD(AID.HissatsuGyoten, dashTarget, 30);
            return;
        }
        if (strategy.Dash.Value == DashStrategy.Automatic && InMelee && Kenki < 25 && RaidBuffsLeft > GCD && RaidBuffsLeft < GCD + GCDLength * 2 && ZanshinLeft == 0)
            PushOGCD(AID.HissatsuGyoten, target.Actor, 30);
    }

    private void UseYaten(in Strategy strategy, Enemy target)
    {
        if (strategy.Yaten.Value == YatenStrategy.Delay || !InMelee || Kenki < 10 || !CanWeave(AID.HissatsuYaten))
            return;
        var soon = World.FutureTime(2.5f);
        if (!Hints.ForbiddenZones.Any(z => z.activation <= soon && z.shape.Check(Player.Position)))
            return;
        var away = (Player.Position - target.Actor.Position).Normalized();
        var landing = Player.Position + away * 10;
        if (!Hints.PathfindMapBounds.Contains(landing - Hints.PathfindMapCenter) || Hints.ForbiddenZones.Any(z => z.activation <= World.FutureTime(5) && z.shape.Check(landing)))
            return;
        PushOGCD(AID.HissatsuYaten, target.Actor, 75);
    }

    private void UseMeikyo(in Strategy strategy, Enemy target)
    {
        if (MeikyoStrat == OffensiveStrategy.Delay || MeikyoLeft > 0 || !CanWeave(AID.MeikyoShisui))
            return;
        if (MeikyoStrat == OffensiveStrategy.Force)
        {
            PushOGCD(AID.MeikyoShisui, Player, 70);
            return;
        }
        if (TendoLeft > 0 || MidCombo && !Reopening || DowntimeIn < GCDLength * 3 || !WorthBurst && CombatTime > 25)
            return;

        if (AOEMode)
        {
            PushOGCD(AID.MeikyoShisui, Player, 70);
            return;
        }

        var use = strategy.Opener.Value != OpenerStrategy.None && CombatTime < 25 && SenCount == 0 && Tsubame.Kind == Repeat.None && Unlocked(AID.TendoSetsugekka)
            || SenCount == 0 && DotTarget != null && DotLeft(target.Actor) <= 15
            || Recovering
            || Reopening && SenCount < 3
            || MaxChargesIn(AID.MeikyoShisui) <= GCD + GCDLength
            || !Unlocked(AID.HissatsuSenei)
            || UseMAL(strategy) && NeedAcceleration()
            || SeneiSoon
            || SenCount == 3 && MeikyoBurstNow(strategy);
        if (use)
            PushOGCD(AID.MeikyoShisui, Player, 70);
    }

    private bool UseMAL(in Strategy strategy) => Unlocked(TraitID.EnhancedMeikyoShisui) && strategy.Loop.Value switch
    {
        LoopStrategy.MAL => true,
        LoopStrategy.Standard => false,
        _ => FukaGCD >= 2.11f
    };

    private bool NeedAcceleration()
    {
        if (SenCount == 3)
            return false;
        var gcds = SenCount switch { 0 => 8, 1 => 5, _ => 3 };
        return gcds * GCDLength > SeneiPlannedIn + GCD;
    }

    private bool SeneiWanted()
    {
        if (!HasRaidBuffJobs || RaidBuffsLeft > GCD || RaidBuffsIn > 45)
            return true;
        return Recovering && MeikyoUnavailable;
    }

    private bool ShouldSpendKenki(in Strategy strategy)
    {
        if (Kenki < 25)
            return false;
        if (Kenki >= 95 || BossDying)
            return true;
        if (strategy.Kenki.Value == KenkiStrategy.Overcap)
            return Kenki >= 90;

        var reserve = (ZanshinLeft > 0 ? 50 : 0) + (SeneiSoon ? 25 : 0);
        if (ZanshinLeft == 0 && Kenki > 50 && !IkishotenDelayed && ReadyIn(AID.Ikishoten) <= GCD + GCDLength * 4 || RaidBuffsLeft > GCD || SeneiJustUsed(20))
            return Kenki - 25 >= reserve;
        return Kenki >= (HasRaidBuffJobs ? 90 : 65) && Kenki - 25 >= reserve;
    }

    private static bool GrantsMeditation(AID aid) => aid is AID.Higanbana or AID.MidareSetsugekka or AID.TenkaGoken or AID.TendoSetsugekka or AID.TendoGoken or AID.OgiNamikiri;

    #endregion

    #region Positionals

    private void UpdatePositionals(in Strategy strategy, Enemy target)
    {
        var (pos, imminent) = NextGCD switch
        {
            AID.Gekko => (Positional.Rear, true),
            AID.Kasha => (Positional.Flank, true),
            AID.Jinpu => (Positional.Rear, false),
            AID.Shifu => (Positional.Flank, false),
            _ => (Positional.Any, false)
        };
        if (AOEMode || !Unlocked(AID.Gekko))
            (pos, imminent) = (Positional.Any, false);
        RecommendPositional(target, pos, imminent, strategy.TrueNorth.Value == TrueNorthStrategy.Automatic && InMelee);
    }

    private void AddGoalZone(Enemy target, bool allowAoE)
    {
        var (_, pos, imminent, _) = Hints.RecommendedPositional;
        var single = Hints.GoalSingleTarget(target.Actor, imminent ? pos : Positional.Any, Player, World.Actors, 3);
        Hints.GoalZones.Add(allowAoE && Unlocked(AID.Fuga) ? GoalCombined(single, Hints.GoalAOECircle(5), 3) : single);
    }

    #endregion

    #region Helpers

    private bool UsePotion(in Strategy strategy) => strategy.Potion.Value switch
    {
        PotionStrategy.AlignWithRaidBuffs => PotionWithRaidBuffs,
        PotionStrategy.Immediate => true,
        _ => false
    };

    private bool Unlocked(TraitID tid) => TraitUnlocked((uint)tid);

    private void PushGCD(AID aid, Actor? target, int priority)
        => base.PushGCD(aid, target, priority, ActionDefinitions.Instance.Spell(aid)?.CastTime > 0 ? IaiCastTime : 0);

    #endregion
}
