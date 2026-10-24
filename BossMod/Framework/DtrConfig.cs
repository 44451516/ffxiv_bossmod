using BossMod.AI;
using BossMod.Autorotation;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;

namespace BossMod;

[ConfigDisplay(Name = "界面", Order = -10, Group = "通用")]
public sealed class DtrConfig : ConfigNode
{
    [PropertyDisplay("游戏界面隐藏时隐藏窗口")]
    public bool HideWithGameUi = true;

    public enum DtrStatus
    {
        [PropertyDisplay("禁用")]
        None,
        [PropertyDisplay("仅文字")]
        TextOnly,
        [PropertyDisplay("带图标")]
        Icon
    }

    [PageStart("服务器信息栏")]
    [PropertyDisplay("自动循环状态指示")]
    [PropertyRadio(inline: true)]
    public DtrStatus ShowAutorot = Service.Config.Get<AutorotationConfig>().ShowDTR;

    [PropertyDisplay("显示旧版 AI 状态")]
    public bool ShowAI = Service.Config.Get<AIConfig>().ShowDTR;

    [PropertyDisplay("显示性能统计")]
    public bool ShowStats = Service.Config.Get<AutorotationConfig>().ShowStatsDTR;

    public List<string> ShowPresets = ["VBM Default", "VBM AI"];

    public override void DrawCustom(string? page, UITree tree, WorldState ws, RotationDatabase? db)
    {
        if (db == null || page != "服务器信息栏")
            return;

        var modified = false;

        ConfigUI.DrawHelp("将为你在此处选择的每个预设创建一个单独的信息栏条目。点击该条目即可开启或关闭对应预设。");

        ImGui.SetNextItemWidth(300 * ImGuiHelpers.GlobalScale);

        List<string> add = [];
        List<string> remove = [];

        using (var combo = ImRaii.Combo("要显示的预设", "选择..."))
        {
            if (combo)
            {
                foreach (var p in db.Presets.AllPresets.Where(p => !ShowPresets.Contains(p.Name)))
                {
                    if (ImGui.Selectable(p.Name))
                        add.Add(p.Name);
                }
            }
        }

        ConfigUI.DrawHelp("");

        foreach (var p in ShowPresets)
        {
            ImGui.Button(p);
            if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                remove.Add(p);
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("右键单击以移除");
            ImGui.SameLine();
        }

        using (ImRaii.Disabled(ShowPresets.Count == 0))
        {
            if (UIMisc.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.SquareXmark, "全部移除"))
                remove.AddRange(ShowPresets);
        }

        if (add.Count > 0 || remove.Count > 0)
        {
            ShowPresets.RemoveAll(remove.Contains);
            ShowPresets.AddRange(add);
            modified = true;
        }

        if (modified)
            Modified.Fire();
    }
}
