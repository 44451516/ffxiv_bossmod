using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;

namespace BossMod.Autorotation.xan;

public class TargetingRenderer : TrackRenderer
{
    public override void DrawLabel(StrategyContext context, StrategyConfig config)
    {
        ImGui.Text("目标选择");
        ImGui.SameLine();
        UIMisc.HelpMarker("这些设置只影响循环模块会对什么目标使用技能。无论你选择哪一项，此模块都不会改变你的游戏内目标（硬目标）。\n\n如果需要自动改变硬目标，请使用 AI -> 自动选择目标。");
    }

    public override bool DrawValue(StrategyContext context, StrategyConfigTrack config, ref StrategyValueTrack value)
    {
        var ix = value.Option;
        var modified = false;
        var opt = (Targeting)ix;

        var forcepri = opt == Targeting.AutoPrimary;
        var trypri = forcepri || opt == Targeting.AutoTryPri;

        if (ImGui.RadioButton("使用玩家当前目标", opt == Targeting.Manual))
        {
            value.Option = 0;
            modified = true;
        }
        if (ImGui.RadioButton("自动选择最佳目标", opt != Targeting.Manual))
        {
            if (opt != Targeting.Auto)
            {
                value.Option = 1;
                modified = true;
            }
        }
        using (ImRaii.Disabled(opt == Targeting.Manual))
        {
            ImGui.Indent();
            if (ImGui.Checkbox("确保玩家当前目标会被命中", ref trypri))
            {
                value.Option = trypri ? 3 : 1;
                modified = true;
            }
            using (ImRaii.Disabled(!trypri))
            {
                if (ImGui.Checkbox("玩家没有目标时不执行操作", ref forcepri))
                {
                    value.Option = forcepri ? 2 : 3;
                    modified = true;
                }
            }
            ImGui.Unindent();
        }

        return modified;
    }
}

public class OffensiveStrategyRenderer : TrackRenderer
{
    private static readonly List<string> optionNames = ["自动", "禁用", "强制"];

    public override bool DrawValue(StrategyContext context, StrategyConfigTrack config, ref StrategyValueTrack value) => UICombo.Radio(typeof(OffensiveStrategy), ref value.Option, true, i => optionNames.BoundSafeAt(i, "")!);
}

public class DefaultOnRenderer : TrackRenderer
{
    public override bool DrawValue(StrategyContext context, StrategyConfigTrack config, ref StrategyValueTrack value)
    {
        var enabled = value.Option == 0;

        if (ImGui.Checkbox("启用", ref enabled))
        {
            value.Option = enabled ? 0 : 1;
            return true;
        }

        return false;
    }
}

public class DefaultOffRenderer : TrackRenderer
{
    public override bool DrawValue(StrategyContext context, StrategyConfigTrack config, ref StrategyValueTrack value)
    {
        var enabled = value.Option == 1;

        if (ImGui.Checkbox("启用", ref enabled))
        {
            value.Option = enabled ? 1 : 0;
            return true;
        }

        return false;
    }
}
