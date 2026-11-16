using BossMod.DRK;
using FFXIVClientStructs.FFXIV.Client.Game.Gauge;
using static BossMod.AIHints;

namespace BossMod.Autorotation.akechi;

public sealed class AkechiDRK(RotationModuleManager manager, Actor player) : AkechiTools<AID, TraitID>(manager, player)
{
    public enum Track { AOE = SharedTrack.Count, Blood, MP, Carve, DeliriumCombo, Unmend, Delirium, SaltedEarth, SaltAndDarkness, LivingShadow, Shadowbringer, Disesteem }
    public enum AOEStrategy { AutoFinish, ForceSTFinish, ForceAOEFinish, AutoBreak, ForceSTBreak, ForceAOEBreak }
    public enum BloodStrategy { Automatic, OnlyBloodspiller, OnlyQuietus, ForceBest, ForceBloodspiller, ForceQuietus, Conserve, Delay }
    public enum MPStrategy { Automatic, Auto3k, Auto6k, Auto9k, AutoRefresh, Edge3k, Edge6k, Edge9k, EdgeRefresh, Flood3k, Flood6k, Flood9k, FloodRefresh, ForceEdge, ForceFlood, Delay }
    public enum CarveStrategy { Automatic, OnlyCarve, OnlyDrain, ForceCarve, ForceDrain, Delay }
    public enum DeliriumComboStrategy { Automatic, ScarletDelirum, Comeuppance, Torcleaver, Impalement, Delay }
    public enum UnmendStrategy { OpenerFar, OpenerForce, Force, Allow, Forbid }

    public static RotationModuleDefinition Definition()
    {
        var res = new RotationModuleDefinition("Akechi DRK", "标准循环模块", "标准循环（Akechi）|坦克", "Akechi", RotationModuleQuality.Good, BitMask.Build((int)Class.DRK), 100);

        res.DefineTargeting();
        res.DefineHold();
        res.DefinePotion(ActionDefinitions.IDPotionStr);

        res.Define(Track.AOE).As<AOEStrategy>("ST/AOE", "单体与AOE循环", 300)
            .AddOption(AOEStrategy.AutoFinish, "根据附近目标自动选择最佳循环，若可以则完成当前连击")
            .AddOption(AOEStrategy.ForceSTFinish, "无视附近目标，强制使用单体循环，若可以则完成当前连击")
            .AddOption(AOEStrategy.ForceAOEFinish, "无视附近目标，强制使用 AOE 循环，若可以则完成当前连击")
            .AddOption(AOEStrategy.AutoBreak, "根据附近目标自动选择最佳循环，若在连击中则打断当前连击")
            .AddOption(AOEStrategy.ForceSTBreak, "无视附近目标，强制使用单体循环，若在连击中则打断当前连击")
            .AddOption(AOEStrategy.ForceAOEBreak, "无视附近目标，强制使用 AOE 循环，若在连击中则打断当前连击")
            .AddAssociatedActions(AID.HardSlash, AID.SyphonStrike, AID.Souleater, AID.Unleash, AID.StalwartSoul);

        res.Define(Track.Blood).As<BloodStrategy>("Blood", "血溅 / 寂灭", 194)
            .AddOption(BloodStrategy.Automatic, "根据附近目标最优使用血溅或寂灭", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(BloodStrategy.OnlyBloodspiller, "无视附近目标，仅将血溅作为血量消耗技能最优使用", supportedTargets: ActionTargets.Hostile, minLevel: 62)
            .AddOption(BloodStrategy.OnlyQuietus, "无视附近目标，仅将寂灭作为血量消耗技能最优使用", supportedTargets: ActionTargets.Hostile, minLevel: 64)
            .AddOption(BloodStrategy.ForceBest, "血量达到 50 以上时尽快自动使用血溅或寂灭", supportedTargets: ActionTargets.Hostile, minLevel: 62)
            .AddOption(BloodStrategy.ForceBloodspiller, "血量达到 50 以上时尽快强制使用血溅", supportedTargets: ActionTargets.Hostile, minLevel: 62)
            .AddOption(BloodStrategy.ForceQuietus, "血量达到 50 以上时尽快强制使用寂灭", supportedTargets: ActionTargets.Hostile, minLevel: 64)
            .AddOption(BloodStrategy.Conserve, "尽量保留血溅与寂灭，仅在必要时使用", supportedTargets: ActionTargets.Hostile, minLevel: 62)
            .AddOption(BloodStrategy.Delay, "推迟使用血溅或寂灭", supportedTargets: ActionTargets.None, minLevel: 62)
            .AddAssociatedActions(AID.Bloodspiller, AID.Quietus);

        res.Define(Track.MP).As<MPStrategy>("MP", "暗黑锋 / 暗黑波动", 193)
            .AddOption(MPStrategy.Automatic, "根据附近目标最优自动使用暗黑锋或暗黑波动；1 分钟内 2 次，2 分钟内 4 次（Dark Arts 生效时为 5 次）", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.Auto3k, "根据附近目标最优自动使用暗黑锋或暗黑波动，MP 达到 3000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.Auto6k, "根据附近目标最优自动使用暗黑锋或暗黑波动，MP 达到 6000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.Auto9k, "根据附近目标最优自动使用暗黑锋或暗黑波动，MP 达到 9000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.AutoRefresh, "根据附近目标最优自动使用暗黑锋或暗黑波动，仅用于刷新 Darkside 或 MP 接近满值时使用", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.Edge3k, "无视附近目标，将暗黑锋/暗影锋用作 Darkside 刷新与 MP 消耗技能，MP 达到 3000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 40)
            .AddOption(MPStrategy.Edge6k, "无视附近目标，将暗黑锋/暗影锋用作 Darkside 刷新与 MP 消耗技能，MP 达到 6000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 40)
            .AddOption(MPStrategy.Edge9k, "无视附近目标，将暗黑锋/暗影锋用作 Darkside 刷新与 MP 消耗技能，MP 达到 9000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 40)
            .AddOption(MPStrategy.EdgeRefresh, "无视附近目标，将暗黑锋/暗影锋用作 Darkside 刷新与 MP 消耗技能，仅用于刷新 Darkside 或 MP 接近满值时使用", supportedTargets: ActionTargets.Hostile, minLevel: 40)
            .AddOption(MPStrategy.Flood3k, "无视附近目标，将暗黑波动/暗影波动用作 Darkside 刷新与 MP 消耗技能，MP 达到 3000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.Flood6k, "无视附近目标，将暗黑波动/暗影波动用作 Darkside 刷新与 MP 消耗技能，MP 达到 6000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.Flood9k, "无视附近目标，将暗黑波动/暗影波动用作 Darkside 刷新与 MP 消耗技能，MP 达到 9000 以上时使用", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.FloodRefresh, "无视附近目标，将暗黑波动/暗影波动用作 Darkside 刷新与 MP 消耗技能，仅用于刷新 Darkside 或 MP 接近满值时使用", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.ForceEdge, "MP 达到 3000 以上时尽快强制使用暗黑锋/暗影锋", supportedTargets: ActionTargets.Hostile, minLevel: 40)
            .AddOption(MPStrategy.ForceFlood, "MP 达到 3000 以上时尽快强制使用暗黑波动/暗影波动", supportedTargets: ActionTargets.Hostile, minLevel: 30)
            .AddOption(MPStrategy.Delay, "推迟使用暗黑锋或暗黑波动/暗影波动", supportedTargets: ActionTargets.None, minLevel: 40)
            .AddAssociatedActions(AID.EdgeOfDarkness, AID.EdgeOfShadow, AID.FloodOfDarkness, AID.FloodOfShadow);

        res.Define(Track.Carve).As<CarveStrategy>("C&S/AD", "精雕怒斩 / 吸血深渊", 195)
            .AddOption(CarveStrategy.Automatic, "根据附近目标自动使用精雕怒斩或吸血深渊", supportedTargets: ActionTargets.Hostile, minLevel: 56)
            .AddOption(CarveStrategy.OnlyCarve, "无视附近目标，自动使用精雕怒斩", supportedTargets: ActionTargets.Hostile, minLevel: 60)
            .AddOption(CarveStrategy.OnlyDrain, "无视附近目标，自动使用吸血深渊", supportedTargets: ActionTargets.Hostile, minLevel: 56)
            .AddOption(CarveStrategy.ForceCarve, "可用时尽快强制使用精雕怒斩", 60, 0, ActionTargets.Hostile, 60)
            .AddOption(CarveStrategy.ForceDrain, "可用时尽快强制使用吸血深渊", 60, 0, ActionTargets.Hostile, 56)
            .AddOption(CarveStrategy.Delay, "推迟使用精雕怒斩", supportedTargets: ActionTargets.None, minLevel: 56)
            .AddAssociatedActions(AID.CarveAndSpit, AID.AbyssalDrain);

        res.Define(Track.DeliriumCombo).As<DeliriumComboStrategy>("DeliCombo", "血乱连击", 196)
            .AddOption(DeliriumComboStrategy.Automatic, "自动使用血乱连击", supportedTargets: ActionTargets.Hostile, minLevel: 96)
            .AddOption(DeliriumComboStrategy.ScarletDelirum, "可用时尽快强制使用血红乱", supportedTargets: ActionTargets.Hostile, minLevel: 96)
            .AddOption(DeliriumComboStrategy.Comeuppance, "可用时尽快强制使用报应", supportedTargets: ActionTargets.Hostile, minLevel: 96)
            .AddOption(DeliriumComboStrategy.Torcleaver, "可用时尽快强制使用戮山", supportedTargets: ActionTargets.Hostile, minLevel: 96)
            .AddOption(DeliriumComboStrategy.Impalement, "可用时尽快强制使用刺穿", supportedTargets: ActionTargets.Hostile, minLevel: 96)
            .AddOption(DeliriumComboStrategy.Delay, "推迟使用血红乱连击", supportedTargets: ActionTargets.None, minLevel: 96)
            .AddAssociatedActions(AID.ScarletDelirium, AID.Comeuppance, AID.Torcleaver, AID.Impalement);

        res.Define(Track.Unmend).As<UnmendStrategy>("Ranged", "伤残", 100)
            .AddOption(UnmendStrategy.OpenerFar, "仅在开怪前且超出最大近战距离时使用伤残", supportedTargets: ActionTargets.Hostile)
            .AddOption(UnmendStrategy.OpenerForce, "仅在开怪前使用伤残，不考虑距离", supportedTargets: ActionTargets.Hostile)
            .AddOption(UnmendStrategy.Force, "无视距离强制使用伤残", supportedTargets: ActionTargets.Hostile)
            .AddOption(UnmendStrategy.Allow, "仅在超出近战距离时允许使用伤残", supportedTargets: ActionTargets.Hostile)
            .AddOption(UnmendStrategy.Forbid, "禁止使用伤残")
            .AddAssociatedActions(AID.Unmend);

        res.DefineOGCD(Track.Delirium, AID.Delirium, "Deli", "血乱", 197, 60, 15, ActionTargets.Self, 35).AddAssociatedActions(AID.BloodWeapon, AID.Delirium);
        res.DefineOGCD(Track.SaltedEarth, AID.SaltedEarth, "SE", "腐秽大地", 196, 90, 15, ActionTargets.Self, 52).AddAssociatedActions(AID.SaltedEarth);
        res.DefineOGCD(Track.SaltAndDarkness, AID.SaltAndDarkness, "S&D", "腐秽黑暗", 190, 20, 0, ActionTargets.Self, 86).AddAssociatedActions(AID.SaltAndDarkness);
        res.DefineOGCD(Track.LivingShadow, AID.LivingShadow, "LS", "掠影示现", 199, 120, 20, ActionTargets.Self, 80).AddAssociatedActions(AID.LivingShadow);
        res.DefineOGCD(Track.Shadowbringer, AID.Shadowbringer, "SB", "暗影使者", 198, 60, 0, ActionTargets.Hostile, 90).AddAssociatedActions(AID.Shadowbringer);
        res.DefineGCD(Track.Disesteem, AID.Disesteem, "Disesteem", "掠影的蔑视", 150, supportedTargets: ActionTargets.Hostile, minLevel: 100).AddAssociatedActions(AID.Disesteem);

        return res;
    }

    private DarkKnightGauge Gauge => World.Client.GetGauge<DarkKnightGauge>();
    private int Blood => Gauge.Blood;
    private bool DarkArts => Gauge.DarkArtsState > 0;
    private int DeliriumStep => Gauge.DeliriumStep;
    private (float Left, bool Active) Darkside => Gauge.DarksideTimer / 1000f is var left ? (left, left > 0.1f) : default;
    private (float CD, float Left, bool Active) Delirium => Status(DeliriumStatus, 15) is var left ? (Cooldown(BestDelirium), left, left > GCD) : default;
    private (float Left, bool Active) SE => Status(SID.SaltedEarth, 15) is var left ? (left, left > 0) : default;
    private float LSCD => Cooldown(AID.LivingShadow);
    private float ScornLeft => Status(SID.Scorn, 30);
    private bool InDeliriumCombo => DeliriumStep is 0 or 1 or 2;

    private AID BestDelirium => Unlocked(AID.Delirium) ? AID.Delirium : AID.BloodWeapon;
    private SID DeliriumStatus => Unlocked(AID.ScarletDelirium) ? SID.EnhancedDelirium : SID.Delirium;

    private AID BestEdge => Unlocked(AID.EdgeOfShadow) ? AID.EdgeOfShadow : Unlocked(AID.EdgeOfDarkness) ? AID.EdgeOfDarkness : AID.FloodOfDarkness;
    private AID BestFlood => Unlocked(AID.FloodOfShadow) ? AID.FloodOfShadow : AID.FloodOfDarkness;
    private AID BestCarve => Unlocked(AID.CarveAndSpit) ? AID.CarveAndSpit : AID.AbyssalDrain;
    private AID BestBloodSpender => WantAOE && Unlocked(AID.Quietus) ? AID.Quietus : AID.Bloodspiller;

    private bool BurstReady => !Unlocked(AID.LivingShadow) || CombatTimer >= 30 || LastComboAction is AID.Souleater;
    private bool InBurst => Delirium.CD >= 40;
    private bool RiskingBlood => LastComboAction is AID.SyphonStrike or AID.Unleash && Blood >= 80 || Delirium.CD <= GCD && Blood >= 70;
    private int IncomingMP => (HasStatus(SID.BloodWeapon) ? 600 : 0) + (LastComboAction is AID.HardSlash or AID.Unleash ? 600 : 0);
    private bool RiskingMP => MP + IncomingMP > 10000 || Darkside.Left <= 3;

    private bool WantAOE;

    private (Enemy? Best, int Targets) RectTargets;
    private (Enemy? Best, int Targets) DrainTargets;
    private Actor? RectTarget(AID action) => Unlocked(action) && RectTargets.Targets > 1 ? RectTargets.Best?.Actor : PrimaryTarget;

    public override void Execution(StrategyValues strategy, Enemy? primaryTarget)
    {
        RectTargets = GetBestTarget(PrimaryEnemy, 10, Is10yRectTarget);
        DrainTargets = GetBestTarget(PrimaryEnemy, 20, IsSplashTarget);
        WantAOE = TargetsInAOECircle(5f, 3) || strategy.Option(Track.AOE).As<AOEStrategy>() is AOEStrategy.ForceAOEFinish or AOEStrategy.ForceAOEBreak;

        if (strategy.HoldEverything())
            return;

        if (strategy.AllowBuffs())
        {
            QueueDelirium(strategy);
            QueueLivingShadow(strategy);
        }
        if (strategy.AllowCDs())
        {
            QueueSaltedEarth(strategy);
            QueueShadowbringer(strategy);
            QueueCarveOrDrain(strategy);
        }
        if (strategy.AllowGauge())
            QueueBlood(strategy);

        QueueDisesteem(strategy);
        QueueMP(strategy);
        QueueDeliriumCombo(strategy);
        QueueSaltAndDarkness(strategy);
        QueueUnmend(strategy);
        QueuePotion(strategy, ActionDefinitions.IDPotionStr, LSCD <= 4f);
        QueueCombo(strategy);

        GetNextTarget(strategy, ref primaryTarget, 3);
        GoalZoneCombined(strategy, 3, Hints.GoalAOECircle(5), AID.Unleash, 3, maximumActionRange: 20);
    }

    private void QueueCombo(StrategyValues strategy)
    {
        var track = strategy.Option(Track.AOE);
        var stTarget = SingleTargetChoice(PrimaryTarget, track);
        var (action, aoeIntent) = track.As<AOEStrategy>() switch
        {
            AOEStrategy.AutoFinish => (WantAOE ? AOEFinish : STFinish, WantAOE),
            AOEStrategy.ForceSTFinish => (STFinish, false),
            AOEStrategy.ForceAOEFinish => (AOEFinish, true),
            AOEStrategy.AutoBreak => (WantAOE ? AOEBreak : STBreak, WantAOE),
            AOEStrategy.ForceSTBreak => (STBreak, false),
            AOEStrategy.ForceAOEBreak => (AOEBreak, true),
            _ => (AID.None, false)
        };

        var isAOE = action is AID.Unleash or AID.StalwartSoul;
        if (aoeIntent && !isAOE && !In3y(stTarget))
            (action, isAOE) = (AOEBreak, true);

        var inRange = isAOE ? aoeIntent || In5y(stTarget) : In3y(stTarget);
        var target = isAOE ? Player : stTarget;
        if (target != null && inRange && CountdownRemaining is not > 0.7f)
            QueueGCD(action, target, GCDPriority.Low);
    }

    private AID STFinish => LastComboAction switch
    {
        AID.Unleash => Unlocked(AID.StalwartSoul) ? AID.StalwartSoul : AID.HardSlash,
        AID.SyphonStrike => Unlocked(AID.Souleater) ? AID.Souleater : AID.HardSlash,
        AID.HardSlash => Unlocked(AID.SyphonStrike) ? AID.SyphonStrike : AID.HardSlash,
        _ => AID.HardSlash
    };

    private AID STBreak => LastComboAction switch
    {
        AID.SyphonStrike => Unlocked(AID.Souleater) ? AID.Souleater : AID.HardSlash,
        AID.HardSlash => Unlocked(AID.SyphonStrike) ? AID.SyphonStrike : AID.HardSlash,
        _ => AID.HardSlash
    };

    private AID AOEFinish => LastComboAction switch
    {
        AID.SyphonStrike => Unlocked(AID.Souleater) ? AID.Souleater : AID.Unleash,
        AID.HardSlash => Unlocked(AID.SyphonStrike) ? AID.SyphonStrike : AID.Unleash,
        AID.Unleash => Unlocked(AID.StalwartSoul) ? AID.StalwartSoul : AID.Unleash,
        _ => AID.Unleash
    };

    private AID AOEBreak => LastComboAction is AID.Unleash && Unlocked(AID.StalwartSoul) ? AID.StalwartSoul : AID.Unleash;

    private void QueueMP(StrategyValues strategy)
    {
        var track = strategy.Option(Track.MP);
        var mp = track.As<MPStrategy>();
        var mp3k = MP >= 3000;
        var mp6k = MP >= 6000;
        var mp9k = MP >= 9000;
        var autoPriority = RiskingMP ? OGCDPriority.Forced : OGCDPriority.Low;

        var (condition, priority) = mp switch
        {
            MPStrategy.Automatic => (ShouldSpendMP(mp3k, mp6k), autoPriority),
            MPStrategy.Auto3k or MPStrategy.Flood3k => (mp3k || DarkArts, autoPriority),
            MPStrategy.Auto6k or MPStrategy.Flood6k => (mp6k || DarkArts, autoPriority),
            MPStrategy.Auto9k or MPStrategy.Flood9k => (mp9k || DarkArts, autoPriority),
            MPStrategy.Edge3k => (mp3k, autoPriority),
            MPStrategy.Edge6k => (mp6k, autoPriority),
            MPStrategy.Edge9k => (mp9k, autoPriority),
            MPStrategy.AutoRefresh or MPStrategy.EdgeRefresh or MPStrategy.FloodRefresh => (RiskingMP, autoPriority),
            MPStrategy.ForceEdge => (Unlocked(AID.EdgeOfDarkness) && mp3k || DarkArts, OGCDPriority.Forced),
            MPStrategy.ForceFlood => (Unlocked(AID.FloodOfDarkness) && mp3k || DarkArts, OGCDPriority.Forced),
            _ => (false, OGCDPriority.None)
        };

        var (rectTarget, rectTargets) = condition ? RectTargets : (null, 0);
        var floodOfShadow = Unlocked(AID.FloodOfShadow) && rectTargets >= 3;
        var floodOfDarkness = Unlocked(AID.FloodOfDarkness) && rectTargets >= 4;
        var action = mp switch
        {
            MPStrategy.Automatic or MPStrategy.Auto3k or MPStrategy.Auto6k or MPStrategy.Auto9k or MPStrategy.AutoRefresh
                => floodOfShadow ? AID.FloodOfShadow : floodOfDarkness ? AID.FloodOfDarkness : BestEdge,
            MPStrategy.Edge3k or MPStrategy.Edge6k or MPStrategy.Edge9k or MPStrategy.EdgeRefresh or MPStrategy.ForceEdge => BestEdge,
            _ => BestFlood
        };
        var target = floodOfShadow || floodOfDarkness ? rectTarget?.Actor : SingleTargetChoice(PrimaryTarget, track);
        if (condition)
            QueueOGCD(action, target, priority);
    }
    private bool ShouldSpendMP(bool mp3k, bool mp6k)
    {
        var freeDarkArts = Unlocked(AID.TheBlackestNight) && DarkArts && (InBurst || Delirium.CD >= Darkside.Left + GCD);
        var burstSpend = InBurst && (InOddWindow(AID.LivingShadow) ? mp6k : mp3k);
        return RiskingMP ? mp3k || DarkArts : !Unlocked(AID.Delirium) || freeDarkArts ? mp3k : burstSpend;
    }
    private void QueueDelirium(StrategyValues strategy)
    {
        var delirium = strategy.Option(Track.Delirium).As<OGCDStrategy>();
        var timing = Unlocked(AID.LivingShadow) ? BurstReady : CombatTimer > 0;
        if (ShouldUseOGCD(delirium, PrimaryTarget, ActionReady(BestDelirium), InCombat(PrimaryTarget) && CanWeaveIn && Darkside.Active && timing))
            QueueOGCD(BestDelirium, Player, OGCDPrio(delirium, OGCDPriority.VeryHigh));
    }
    private void QueueLivingShadow(StrategyValues strategy)
    {
        var ls = strategy.Option(Track.LivingShadow).As<OGCDStrategy>();
        if (ShouldUseOGCD(ls, PrimaryTarget, ActionReady(AID.LivingShadow), InCombat(PrimaryTarget) && CanWeaveIn && Darkside.Active))
            QueueOGCD(AID.LivingShadow, Player, OGCDPrio(ls, OGCDPriority.ExtremelyHigh));
    }
    private void QueueSaltedEarth(StrategyValues strategy)
    {
        var se = strategy.Option(Track.SaltedEarth).As<OGCDStrategy>();
        if (ShouldUseOGCD(se, PrimaryTarget, ActionReady(AID.SaltedEarth), InCombat(PrimaryTarget) && CanWeaveIn && In3y(PrimaryTarget) && Darkside.Active && BurstReady))
            QueueOGCD(AID.SaltedEarth, Player, OGCDPrio(se, OGCDPriority.AboveAverage));
    }
    private Actor? FindSaltedEarth()
    {
        Actor? found = null;
        foreach (var actor in World.Actors)
            if (found == null && actor.OID == 0x17C && actor.OwnerID == Player.InstanceID)
                found = actor;
        return found;
    }
    private void QueueSaltAndDarkness(StrategyValues strategy)
    {
        var sd = strategy.Option(Track.SaltAndDarkness).As<OGCDStrategy>();
        var ready = sd != OGCDStrategy.Delay && SE.Active && ActionReady(AID.SaltAndDarkness);
        var zone = ready ? FindSaltedEarth() : null;
        var timing = SE.Left <= 5f || zone != null && Hints.NumPriorityTargetsInAOECircle(zone.Position, 5) > 0;
        if (zone != null && ShouldUseOGCD(sd, PrimaryTarget, true, InCombat(PrimaryTarget) && timing))
            QueueOGCD(AID.SaltAndDarkness, Player, OGCDPriority.AboveAverage);
    }
    private void QueueShadowbringer(StrategyValues strategy)
    {
        var track = strategy.Option(Track.Shadowbringer);
        var sb = track.As<OGCDStrategy>();
        var ready = sb != OGCDStrategy.Delay && Unlocked(AID.Shadowbringer) && Cooldown(AID.Shadowbringer) <= 60;
        var target = ready ? AOETargetChoice(PrimaryTarget, RectTarget(AID.Shadowbringer), track, strategy) : null;
        var timing = RaidBuffsLeft > 0f || LSCD > 80 && Delirium.CD > 40;
        if (ShouldUseOGCD(sb, target, ready, InCombat(target) && CanWeaveIn && Darkside.Active && timing))
            QueueOGCD(AID.Shadowbringer, target, OGCDPrio(sb, OGCDPriority.Average));
    }
    private void QueueCarveOrDrain(StrategyValues strategy)
    {
        var track = strategy.Option(Track.Carve);
        var carve = track.As<CarveStrategy>();
        var optimal = InCombat(PrimaryTarget) && CanWeaveIn && Darkside.Active && BurstReady && InBurst;
        var forced = carve is CarveStrategy.ForceCarve or CarveStrategy.ForceDrain;
        var ready = carve != CarveStrategy.Delay && (forced || optimal) && ActionReady(AID.AbyssalDrain);

        Actor? STTarget() => SingleTargetChoice(PrimaryTarget, track);
        Actor? AOETarget() => AOETargetChoice(PrimaryTarget, DrainTargets.Best?.Actor, track, strategy);
        Actor? CarveTarget() => Unlocked(AID.CarveAndSpit) ? STTarget() : AOETarget();
        var wantDrain = ready && carve == CarveStrategy.Automatic && DrainTargets.Targets > 2;

        var (action, target, priority) = carve switch
        {
            CarveStrategy.Automatic => (wantDrain ? AID.AbyssalDrain : BestCarve, wantDrain ? AOETarget() : STTarget(), OGCDPriority.BelowAverage),
            CarveStrategy.OnlyCarve => (BestCarve, CarveTarget(), OGCDPriority.BelowAverage),
            CarveStrategy.OnlyDrain => (AID.AbyssalDrain, AOETarget(), OGCDPriority.BelowAverage),
            CarveStrategy.ForceCarve => (BestCarve, CarveTarget(), OGCDPriority.Forced),
            _ => (AID.AbyssalDrain, AOETarget(), OGCDPriority.Forced)
        };
        if (ready)
            QueueOGCD(action, target, priority);
    }

    private void QueueDisesteem(StrategyValues strategy)
    {
        var track = strategy.Option(Track.Disesteem);
        var disesteem = track.As<GCDStrategy>();
        if (disesteem == GCDStrategy.Delay || !Unlocked(AID.Disesteem) || ScornLeft <= 0.1f)
            return;

        var target = AOETargetChoice(PrimaryTarget, RectTarget(AID.Disesteem), track, strategy);
        var timing = RaidBuffsLeft > 0 || LSCD > 80 && Delirium.CD > 30 && !Delirium.Active || ScornLeft < 10f || !In3y(target);
        var priority = disesteem is GCDStrategy.Force ? GCDPriority.Forced : CombatTimer < 30 ? GCDPriority.VeryHigh : GCDPriority.AboveAverage;
        if (ShouldUseGCD(disesteem, target, true, InCombat(target) && In10y(target) && Darkside.Active && timing))
            QueueGCD(AID.Disesteem, target, priority);
    }
    private void QueueDeliriumCombo(StrategyValues strategy)
    {
        var track = strategy.Option(Track.DeliriumCombo);
        var target = SingleTargetChoice(PrimaryTarget, track);
        if (!Unlocked(AID.ScarletDelirium) || !Delirium.Active || !InCombat(target))
            return;

        var autoAction = WantAOE ? AID.Impalement : DeliriumStep switch
        {
            2 => AID.Torcleaver,
            1 => AID.Comeuppance,
            _ => AID.ScarletDelirium
        };
        var autoPriority = Delirium.Left <= 9f ? GCDPriority.Forced : GCDPriority.SlightlyHigh;

        var (condition, action, priority) = track.As<DeliriumComboStrategy>() switch
        {
            DeliriumComboStrategy.Automatic => ((WantAOE || In3y(target)) && InDeliriumCombo, autoAction, autoPriority),
            DeliriumComboStrategy.ScarletDelirum => (DeliriumStep == 0, AID.ScarletDelirium, GCDPriority.Forced),
            DeliriumComboStrategy.Comeuppance => (DeliriumStep == 1, AID.Comeuppance, GCDPriority.Forced),
            DeliriumComboStrategy.Torcleaver => (DeliriumStep == 2, AID.Torcleaver, GCDPriority.Forced),
            DeliriumComboStrategy.Impalement => (InDeliriumCombo, Unlocked(AID.Impalement) ? AID.Impalement : AID.Quietus, GCDPriority.Forced),
            _ => (false, AID.None, GCDPriority.None)
        };
        if (condition)
            QueueGCD(action, target, priority);
    }
    private void QueueBlood(StrategyValues strategy)
    {
        var track = strategy.Option(Track.Blood);
        var blood = track.As<BloodStrategy>();
        if (blood == BloodStrategy.Delay || Blood < 50 && !Delirium.Active)
            return;

        var action = blood switch
        {
            BloodStrategy.Automatic or BloodStrategy.ForceBest or BloodStrategy.Conserve => BestBloodSpender,
            BloodStrategy.OnlyBloodspiller or BloodStrategy.ForceBloodspiller => AID.Bloodspiller,
            BloodStrategy.OnlyQuietus or BloodStrategy.ForceQuietus => AID.Quietus,
            _ => AID.None
        };
        var target = WantAOE && action == AID.Quietus ? Player : SingleTargetChoice(PrimaryTarget, track);
        var canSpend = Unlocked(BestBloodSpender) && (Blood >= 50 || Delirium.Active);
        var optimal = canSpend && InCombat(target) && In3y(target) && Darkside.Active && (Delirium.CD > 40 || RiskingBlood || RaidBuffsLeft > GCD);

        var (condition, priority) = blood switch
        {
            BloodStrategy.Automatic or BloodStrategy.OnlyBloodspiller or BloodStrategy.OnlyQuietus => (optimal, GCDPriority.Average),
            BloodStrategy.ForceBest or BloodStrategy.ForceBloodspiller or BloodStrategy.ForceQuietus => (canSpend, GCDPriority.Forced),
            BloodStrategy.Conserve => (RiskingBlood || Delirium.Active, GCDPriority.Average),
            _ => (false, GCDPriority.None)
        };
        if (condition)
            QueueGCD(action, target, priority);
    }
    private void QueueUnmend(StrategyValues strategy)
    {
        var track = strategy.Option(Track.Unmend);
        var unmend = track.As<UnmendStrategy>();
        if (unmend == UnmendStrategy.Forbid)
            return;

        var target = SingleTargetChoice(PrimaryTarget, track);
        var away = target != null && !In3y(target);
        var prepull = NoPrepullWithin(0.7f);
        var disesteemInstead = Unlocked(AID.Disesteem) && ScornLeft > GCD && In10y(target);

        var (condition, priority) = unmend switch
        {
            UnmendStrategy.OpenerFar => (away && prepull, GCDPriority.Low),
            UnmendStrategy.OpenerForce => (target != null && prepull, GCDPriority.Low + 1),
            UnmendStrategy.Force => (true, GCDPriority.Low + 1),
            UnmendStrategy.Allow => ((Player.InCombat || prepull) && away && !disesteemInstead, GCDPriority.Low),
            _ => (false, GCDPriority.None)
        };
        if (condition)
            QueueGCD(AID.Unmend, target, priority);
    }
}
