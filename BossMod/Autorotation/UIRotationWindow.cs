using BossMod.Services;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;

namespace BossMod.Autorotation;

public sealed class UIRotationWindow : UIWindow
{
    private readonly RotationModuleManager _mgr;
    private readonly IAmex _amex;
    private readonly AutorotationConfig _config = Service.Config.Get<AutorotationConfig>();
    private readonly EventSubscriptions _subscriptions;

    public UIRotationWindow(RotationModuleManager mgr, IAmex amex) : base("自动循环", false, new(400, 400), ImGuiWindowFlags.NoFocusOnAppearing)
    {
        _mgr = mgr;
        _amex = amex;
        _subscriptions = new
        (
            _config.Modified.ExecuteAndSubscribe(() => IsOpen = _config.ShowUI)
        );
        RespectCloseHotkey = false;
        TitleBarButtons.Add(new() { Icon = FontAwesomeIcon.Cog, IconOffset = new(1), Click = _ => Service.Mediator.Publish(new OpenSettingsWindow("Autorotation Presets")) });
    }

    protected override void Dispose(bool disposing)
    {
        _subscriptions.Dispose();
        base.Dispose(disposing);
    }

    public void SetVisible(bool vis)
    {
        if (_config.ShowUI != vis)
        {
            _config.ShowUI = vis;
            _config.Modified.Fire();
        }
    }

    public override void PreOpenCheck()
    {
        DrawPositional();
    }

    public override bool DrawConditions() => _mgr.WorldState.Party.Player() != null;

    public override void Draw()
    {
        var player = _mgr.Player;
        if (player == null)
            return;

        DrawRotationSelector(_mgr, true);

        if (_mgr.IsForceDisabled)
            UIMisc.Banner(FontAwesomeIcon.Ban, "自动循环已禁用。再次点击“已禁用”或选择一个预设即可重新启用。", UIMisc.ColorDanger);

        var dups = _mgr.DuplicateModules.ToList();
        if (dups.Count > 0)
            UIMisc.Banner(FontAwesomeIcon.ExclamationTriangle, $"你启用了同一模块的多个副本（{string.Join(", ", dups.Select(d => d.Name))}）。每种模块只会执行一个，这通常不是你想要的结果。", UIMisc.ColorWarning);

        var activeModule = _mgr.Bossmods.ActiveModule;
        if (activeModule != null)
        {
            DrawPlanSelector(activeModule, player);
            DrawUpcoming(activeModule);
        }

        DrawActiveModules();
        DrawNextActions();
    }

    private void DrawPlanSelector(BossModule activeModule, Actor player)
    {
        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("冷却计划：");
        ImGui.SameLine();
        if (!(activeModule.Info?.PlanLevel > 0))
        {
            ImGui.TextDisabled("此模块不支持冷却计划");
            return;
        }

        var plans = _mgr.Database.Plans.GetPlans(activeModule.GetType(), player.Class);
        var newSel = UIPlanDatabaseEditor.DrawPlanCombo(plans, plans.SelectedIndex, "###root");
        if (newSel != plans.SelectedIndex)
        {
            plans.SelectedIndex = newSel;
            _mgr.Database.Plans.ModifyManifest(activeModule.GetType(), player.Class);
        }

        ImGui.SameLine();
        if (ImGui.Button(plans.SelectedIndex >= 0 ? "编辑" : "新建"))
        {
            if (plans.SelectedIndex < 0)
            {
                var plan = new Plan($"新建 {plans.Plans.Count + 1}", activeModule.GetType()) { Guid = Guid.NewGuid().ToString(), Class = player.Class, Level = activeModule.Info.PlanLevel };
                plans.SelectedIndex = plans.Plans.Count;
                _mgr.Database.Plans.ModifyPlan(null, plan);
            }
            UIPlanDatabaseEditor.StartPlanEditor(_mgr.Database.Plans, plans.Plans[plans.SelectedIndex], activeModule.StateMachine);
        }

        if (newSel >= 0 && _mgr.Presets.Count > 0)
            UIMisc.Banner(FontAwesomeIcon.ExclamationTriangle, "已启用预设，它会完全覆盖冷却计划。", UIMisc.ColorWarning);
    }

    private void DrawUpcoming(BossModule module)
    {
        var now = _mgr.WorldState.CurrentTime;
        var sm = module.StateMachine;
        var timelineValid = sm.ActiveState != null && sm.TimeSinceTransition - sm.ActiveState.Duration < 3;

        (float seconds, int targets)? next(AIHints.PredictedDamageType type, StateMachine.StateHint flag)
        {
            var at = DateTime.MaxValue;
            var targets = 0;
            foreach (var d in _mgr.Hints.PredictedDamage.Where(d => d.Type == type))
            {
                if (d.Activation < at)
                    at = d.Activation;
                targets = Math.Max(targets, d.Players.NumSetBits());
            }
            if (at == DateTime.MaxValue && timelineValid && flag != StateMachine.StateHint.None)
                at = sm.NextTransitionWithFlag(flag);
            var seconds = (float)(at - now).TotalSeconds;
            return at == DateTime.MaxValue || seconds > 60 ? null : (Math.Max(0, seconds), targets);
        }

        var upcoming = new List<(string label, float seconds)>();
        if (next(AIHints.PredictedDamageType.Raidwide, StateMachine.StateHint.Raidwide) is var (rw, _))
            upcoming.Add(("全屏伤害", rw));
        if (next(AIHints.PredictedDamageType.Tankbuster, StateMachine.StateHint.Tankbuster) is var (tb, tanks))
            upcoming.Add((tanks > 1 ? $"坦克死刑 x{tanks}" : "坦克死刑", tb));
        if (next(AIHints.PredictedDamageType.Shared, StateMachine.StateHint.None) is var (st, _))
            upcoming.Add(("分摊", st));

        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("即将到来：");
        if (upcoming.Count == 0)
        {
            ImGui.SameLine();
            ImGui.TextDisabled("未来一分钟内没有事件");
            return;
        }
        foreach (var (label, seconds) in upcoming.OrderBy(u => u.seconds))
        {
            var text = $"{label} {seconds:f1}s";
            UIMisc.SameLineOrWrap(ImGui.CalcTextSize(text).X + 10 * ImGuiHelpers.GlobalScale);
            UIMisc.Badge(text, seconds < 3 ? UIMisc.ColorDanger : seconds < 8 ? UIMisc.ColorWarning : UIMisc.ColorInfo);
        }
    }

    private void DrawActiveModules()
    {
        ImGui.TextUnformatted("模块：");

        if (_mgr.Presets.Any(p => p.Modules.Any(m => m.TransientSettings.Count > 0)))
        {
            ImGui.SameLine();
            using (ImRaii.PushColor(ImGuiCol.Text, UIMisc.ColorGood))
                UIMisc.IconText(FontAwesomeIcon.BoltLightning);
            if (ImGui.IsItemHovered())
            {
                using var tooltip = ImRaii.Tooltip();
                ImGui.TextUnformatted("临时策略：");
                foreach (var p in _mgr.Presets)
                {
                    foreach (var m in p.Modules.Where(m => m.TransientSettings.Count > 0))
                    {
                        ImGui.TextUnformatted($"> {m.Type.FullName}");
                        using var indent = ImRaii.PushIndent();
                        foreach (var s in m.TransientSettings)
                        {
                            var track = m.Definition.Configs[s.Track];
                            ImGui.TextUnformatted($"{track.InternalName} = {track.ToDisplayString(s.Value)}");
                        }
                    }
                }
            }
        }

        ImGui.SameLine();
        var modules = _mgr.ToString();
        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(ImGuiCol.TextDisabled)))
            ImGui.TextWrapped(modules.Length > 0 ? modules : "无");
    }

    private void DrawNextActions()
    {
        UIMisc.SectionHeader("接下来的技能");
        var raidBuffs = _mgr.Bossmods.RaidCooldowns.NextDamageBuffIn2() switch
        {
            null => "",
            <= 0 => " · 团辅已就绪",
            var t => $" · {t:f1} 秒后团辅就绪"
        };
        ImGui.TextDisabled($"GCD {_mgr.WorldState.Client.Cooldowns[ActionDefinitions.GCDGroup].Remaining:f2} · 动画锁定 {_amex.EffectiveAnimationLock:f2}+{_amex.AnimationLockDelayEstimate:f2} · 连击 {_amex.ComboTimeLeft:f1}{raidBuffs}");

        var entries = _mgr.Hints.ActionsToExecute.Entries;
        if (entries.Count == 0)
        {
            ImGui.TextDisabled("队列为空");
            return;
        }

        using var table = ImRaii.Table("queue", 3, ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.SizingStretchProp);
        if (!table)
            return;
        ImGui.TableSetupColumn("技能", ImGuiTableColumnFlags.WidthStretch, 3);
        ImGui.TableSetupColumn("优先级", ImGuiTableColumnFlags.WidthStretch, 1);
        ImGui.TableSetupColumn("目标", ImGuiTableColumnFlags.WidthStretch, 2);
        ImGui.TableHeadersRow();
        var iconSize = new Vector2(ImGui.GetTextLineHeight());
        var first = true;
        foreach (var a in entries.OrderByDescending(e => e.Priority))
        {
            ImGui.TableNextRow();
            if (first)
                ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, (UIMisc.ColorGood & 0x00ffffff) | 0x30000000);
            first = false;
            ImGui.TableNextColumn();
            var action = _amex.AdjustAction(a.Action);
            var icon = action.Type == ActionType.Spell ? Service.LuminaRow<Lumina.Excel.Sheets.Action>(action.ID)?.Icon ?? 0 : 0;
            if (icon != 0)
            {
                UIMisc.Image(Service.Texture?.GetFromGameIcon(icon), iconSize);
                ImGui.SameLine();
            }
            ImGui.TextUnformatted(action.Name());
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip(action == a.Action ? a.Action.ToString() : $"{action} (已排队为 {a.Action.Name()})");
            ImGui.TableNextColumn();
            ImGui.TextUnformatted($"{a.Priority:f0}");
            ImGui.TableNextColumn();
            if (a.Target == null || a.Target == _mgr.Player)
                ImGui.TextDisabled(a.Target == null ? "-" : "自身");
            else
                ImGui.TextUnformatted(a.Target.Name);
        }
    }

    public override void OnClose() => SetVisible(false);

    public static bool DrawRotationSelector(RotationModuleManager mgr, bool wrap = false)
    {
        var dtrConfig = Service.Config.Get<DtrConfig>();

        var modified = false;
        if (mgr.Player == null)
            return modified;

        using var padding = ImRaii.PushStyle(ImGuiStyleVar.FramePadding, new Vector2(4, 1) * ImGuiHelpers.GlobalScale);
        using var spacing = ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, new Vector2(3, 3) * ImGuiHelpers.GlobalScale);
        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("预设：");

        void nextButton(string label)
        {
            if (wrap)
                UIMisc.SameLineOrWrap(UIMisc.ButtonWidth(label));
            else
                ImGui.SameLine();
        }

        nextButton("已禁用");
        if (UIMisc.ToggleButton("已禁用", mgr.IsForceDisabled, 0xff000080))
        {
            if (mgr.IsForceDisabled)
                mgr.Clear();
            else
                mgr.SetForceDisabled();
            modified |= true;
        }

        foreach (var p in mgr.Database.Presets.PresetsForClass(mgr.Player.Class))
        {
            var isActive = mgr.Presets.Contains(p);
            if (!isActive && p.HiddenByDefault)
                continue;
            nextButton(p.Name);
            UIMisc.ToggleButton(p.Name, isActive);
            if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
            {
                mgr.Toggle(p);
                modified |= true;
            }
            using var ctx = ImRaii.ContextPopupItem($"###{p}");
            if (ctx)
            {
                var sel = dtrConfig.ShowPresets.Contains(p.Name);
                if (ImGui.MenuItem("在服务器信息栏中显示", sel))
                {
                    if (sel)
                        dtrConfig.ShowPresets.Remove(p.Name);
                    else
                        dtrConfig.ShowPresets.Add(p.Name);
                    dtrConfig.Modified.Fire();
                }
                if (ImGui.MenuItem("在预设编辑器中打开"))
                    Service.Mediator.Publish(new OpenPresetEditor(p.Name));
            }
        }

        return modified;
    }

    private void DrawPositional()
    {
        var pos = _mgr.Hints.RecommendedPositional;
        if (_config.ShowPositionals && pos.Target != null && !pos.Target.Omnidirectional)
        {
            var color = PositionalColor(pos.Imminent, pos.Correct);
            switch (pos.Pos)
            {
                case Positional.Flank:
                    Service.Camera.DrawWorldCone(pos.Target.PosRot.XYZ(), pos.Target.HitboxRadius + 3.5f, pos.Target.Rotation + 90.Degrees(), 45.Degrees(), color);
                    Service.Camera.DrawWorldCone(pos.Target.PosRot.XYZ(), pos.Target.HitboxRadius + 3.5f, pos.Target.Rotation - 90.Degrees(), 45.Degrees(), color);
                    break;
                case Positional.Rear:
                    Service.Camera.DrawWorldCone(pos.Target.PosRot.XYZ(), pos.Target.HitboxRadius + 3.5f, pos.Target.Rotation + 180.Degrees(), 45.Degrees(), color);
                    break;
            }
        }
    }

    private uint PositionalColor(bool imminent, bool correct) => imminent
        ? (correct ? 0xff00ff00 : 0xff0000ff)
        : (correct ? 0xffffffff : 0xff00ffff);
}
