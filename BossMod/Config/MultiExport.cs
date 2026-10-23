using BossMod.Autorotation;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using System.IO;
using System.Text;
using System.Text.Json;

namespace BossMod.Config;

public sealed class MultiExport(Dictionary<Class, PlanDatabase.PlanList> plans) : UIWindow("多重导出", true, new(300, 500), ImGuiWindowFlags.AlwaysAutoResize)
{
    readonly List<string> _selected = [];
    readonly List<Plan> _selectedPlans = [];

    public override void Draw()
    {
        _selectedPlans.Clear();

        if (ImGui.Button("全选"))
        {
            _selected.Clear();
            _selected.AddRange(plans.SelectMany(p => p.Value.Plans).Select(p => p.Guid));
        }
        ImGui.SameLine();
        if (ImGui.Button("清空"))
            _selected.Clear();

        using (ImRaii.Disabled())
            ImGui.Text($"已选择 {_selected.Count} 项");

        foreach (var (cls, plist) in plans)
        {
            foreach (var plan in plist.Plans)
            {
                var isSelected = _selected.Contains(plan.Guid);
                if (isSelected)
                    _selectedPlans.Add(plan);

                if (ImGui.Checkbox($"{cls} '{plan.Name}'", ref isSelected))
                {
                    if (isSelected)
                        _selected.Add(plan.Guid);
                    else
                        _selected.Remove(plan.Guid);
                }
            }
        }

        using (ImRaii.Disabled(_selected.Count == 0))
        {
            if (ImGui.Button("导出所选到剪贴板"))
            {
                var ser = Serialization.BuildSerializationOptions();

                using var stream = new MemoryStream();
                using (var writer = new Utf8JsonWriter(stream))
                {

                    writer.WriteStartObject();
                    writer.WriteBoolean("multi", true);
                    writer.WriteStartArray("plans");

                    foreach (var plan in _selectedPlans)
                        JsonSerializer.Serialize(writer, plan, ser);

                    writer.WriteEndArray();
                    writer.WriteEndObject();
                }

                ImGui.SetClipboardText(Encoding.UTF8.GetString(stream.GetBuffer()));
            }
        }
    }
}

public sealed class MultiImport(RotationDatabase rotationDB, List<Plan> plans) : UIWindow("多重导入", true, new(300, 500), ImGuiWindowFlags.AlwaysAutoResize)
{
    readonly List<(Plan plan, bool selected)> _plans = [.. plans.Select(p => (p, true))];

    public override void Draw()
    {
        if (ImGui.Button("全选"))
            for (var i = 0; i < _plans.Count; i++)
                _plans.Ref(i).selected = true;

        ImGui.SameLine();

        if (ImGui.Button("清空"))
            for (var i = 0; i < _plans.Count; i++)
                _plans.Ref(i).selected = false;

        var num = _plans.Count(p => p.selected);

        using (ImRaii.Disabled())
            ImGui.Text($"已选择 {num} 项");

        for (var i = 0; i < _plans.Count; i++)
        {
            var plan = _plans[i].plan;
            ImGui.Checkbox($"{plan.Class} '{plan.Name}' ({plan.Encounter.Name})", ref _plans.Ref(i).selected);
        }

        using (ImRaii.Disabled(num == 0))
            if (ImGui.Button("导入所选计划"))
            {
                var sel = _plans.Where(p => p.selected).Select(p => p.plan).ToList();

                foreach (var plan in sel)
                    rotationDB.Plans.ModifyPlan(null, plan);

                foreach (var group in sel.GroupBy(p => p.Encounter.FullName))
                    Service.ShowNotification($"已为遭遇战 {group.Key} 导入 {group.Count()} 个计划");
            }
    }
}
