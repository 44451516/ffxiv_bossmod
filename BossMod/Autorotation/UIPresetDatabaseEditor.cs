using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BossMod.Autorotation;

// note: the editor assumes it's the only thing that modifies the database instance; having multiple editors or editing database externally will break things
public sealed class UIPresetDatabaseEditor(RotationDatabase rotationDB)
{
    private readonly PresetDatabase PresetDB = rotationDB.Presets;

    private int _selectedPresetIndex = -1;
    private bool _selectedPresetDefault;
    private int _pendingSelectPresetIndex = -1; // if >= 0, we want to select different preset, but current one has modifications
    private bool _pendingSelectPresetDefault;
    private Type? _selectedModuleType; // we want module selection to be persistent when changing presets
    private UIPresetEditor? _selectedPreset;

    private readonly AutorotationConfig _cfg = Service.Config.Get<AutorotationConfig>();

    private bool HaveUnsavedModifications => _selectedPreset?.Modified ?? false;

    public void Draw()
    {
        if (_pendingSelectPresetIndex >= 0)
            DrawPendingSwitch();
        DrawPresetSelector();
        if (_selectedPreset != null)
        {
            _selectedPreset.Draw();
            _selectedModuleType = _selectedPreset.SelectedModuleType ?? _selectedModuleType;
        }
        else
        {
            ImGui.TextUnformatted("选择要编辑的预设，或新建一个预设。");
        }
    }

    private void DrawPendingSwitch()
    {
        if (_pendingSelectPresetIndex < 0)
            return;
        if (!HaveUnsavedModifications)
        {
            CompleteChangeCurrentPreset();
            return;
        }

        ImGui.OpenPopup("未保存的修改"); // TODO: why do i have to do it every frame???
        bool modalOpen = true;
        using var modal = ImRaii.PopupModal("未保存的修改", ref modalOpen, ImGuiWindowFlags.AlwaysAutoResize);
        if (!modal)
            return;
        ImGui.TextUnformatted($"当前打开的预设 {_selectedPreset?.Preset.Name} 有未保存的修改。");
        ImGui.TextUnformatted("要选择新的预设，你需要先保存或放弃这些修改。");
        ImGui.TextUnformatted("要如何继续？");
        if (DrawSaveCurrentPresetButton())
        {
            SaveCurrentPreset();
            CompleteChangeCurrentPreset();
        }
        ImGui.SameLine();
        if (UIMisc.Button("另存为副本", _selectedPresetIndex < 0, "新预设不能另存为副本"))
        {
            SaveCurrentPresetAsCopy();
            CompleteChangeCurrentPreset();
        }
        ImGui.SameLine();
        if (ImGui.Button("放弃"))
        {
            CompleteChangeCurrentPreset();
        }
        ImGui.SameLine();
        if (ImGui.Button("取消") || !modalOpen)
        {
            _pendingSelectPresetIndex = -1;
        }
        if (_pendingSelectPresetIndex < 0)
            ImGui.CloseCurrentPopup();
    }

    private void DrawPresetSelector()
    {
        UIMisc.HelpMarker("""
            要开始使用自动循环，请创建一个“预设”。
            预设用于配置循环“模块”及其“策略”。
            模块是一段会评估游戏状态并生成候选技能优先级列表的代码。
            自动循环框架会在下一次可执行时机选择优先级最高的技能执行。
            每个模块还可以通过一组“策略”进一步配置，用于自定义其不同行为。
            例如，你可以创建“单体”和“AoE”预设；它们使用相同模块，但策略配置不同。
            你也可以为每个策略值分配键盘修饰键；只有按住对应修饰键时，该值才会生效。
            例如，这可以让你设置一个在按住 Shift 时延后 2 分钟爆发的预设。
            """);
        ImGui.SameLine();

        ImGui.SetNextItemWidth(200);
        using (var combo = ImRaii.Combo("预设", _selectedPreset == null ? "" : _selectedPresetIndex < 0 ? "<新建>" : (_selectedPresetDefault ? PresetDB.DefaultPresets : PresetDB.UserPresets)[_selectedPresetIndex].Name))
        {
            if (combo)
            {
                if (!_cfg.HideDefaultPreset)
                    DrawPresetListElements(true);
                DrawPresetListElements(false);
            }
        }

        ImGui.SameLine();
        if (DrawSaveCurrentPresetButton())
            SaveCurrentPreset();
        ImGui.SameLine();
        if (UIMisc.Button("另存为副本", _selectedPresetIndex < 0, "新预设不能另存为副本"))
            SaveCurrentPresetAsCopy();
        ImGui.SameLine();
        if (UIMisc.Button("还原", 0, (!HaveUnsavedModifications, "当前预设没有修改"), (_selectedPresetIndex < 0, "未选择预设")))
            RevertCurrentPreset();
        ImGui.SameLine();
        if (UIMisc.Button("新建", HaveUnsavedModifications, "当前预设已修改，请保存或放弃更改"))
            CreateNewPreset(-1, false);
        ImGui.SameLine();
        if (UIMisc.Button("复制", 0, (HaveUnsavedModifications, "当前预设已修改，请保存或放弃更改"), (_selectedPresetIndex < 0, "未选择预设")))
            CreateNewPreset(_selectedPresetIndex, _selectedPresetDefault);
        ImGui.SameLine();
        if (UIMisc.Button("删除", 0, (_selectedPresetDefault, "默认预设不能删除。如果想隐藏它，可以在“设置 -> 自动循环”中操作。"), (!ImGui.GetIO().KeyShift, "按住 Shift 删除"), (_selectedPresetIndex < 0, "未选择预设")))
            DeleteCurrentPreset();
        ImGui.SameLine();
        if (UIMisc.Button("导出", _selectedPreset == null, "未选择预设"))
            ExportToClipboard();
        ImGui.SameLine();
        if (UIMisc.Button("导入", HaveUnsavedModifications, "当前预设已修改，请保存或放弃更改"))
            ImportNewPresetFromClipboard();
    }

    private void DrawPresetListElements(bool defaultPresets)
    {
        var presets = defaultPresets ? PresetDB.DefaultPresets : PresetDB.UserPresets;
        for (int i = 0; i < presets.Count; ++i)
        {
            var preset = presets[i];
            if (ImGui.Selectable(preset.Name, _selectedPresetDefault == defaultPresets && _selectedPresetIndex == i))
            {
                _pendingSelectPresetIndex = i;
                _pendingSelectPresetDefault = defaultPresets;
            }

            if (!defaultPresets && ImGui.IsItemActive() && !ImGui.IsItemHovered())
            {
                var j = ImGui.GetMouseDragDelta().Y < 0 ? i - 1 : i + 1;
                if (j >= 0 && j < presets.Count)
                {
                    (presets[i], presets[j]) = (presets[j], presets[i]);
                    if (_selectedPresetIndex == i && _selectedPresetDefault == defaultPresets)
                        _selectedPresetIndex = j;
                    else if (_selectedPresetIndex == j && _selectedPresetDefault == defaultPresets)
                        _selectedPresetIndex = i;
                    PresetDB.Modify(-1, null);
                    ImGui.ResetMouseDragDelta();
                }
            }
        }
    }

    private bool DrawSaveCurrentPresetButton() => UIMisc.Button("保存", 0, (!HaveUnsavedModifications, "当前预设没有修改"), (_selectedPreset?.NameConflict ?? false, "当前预设名称为空，或与现有预设重名"));

    private void RevertCurrentPreset() => _selectedPreset = new(PresetDB, _selectedPresetIndex, _selectedPresetDefault, _selectedModuleType);

    private void SaveCurrentPreset()
    {
        if (!_selectedPresetDefault && _selectedPreset != null && _selectedPreset.Modified && !_selectedPreset.NameConflict)
        {
            PresetDB.Modify(_selectedPresetIndex, _selectedPreset.Preset);
            if (_selectedPresetIndex < 0)
                _selectedPresetIndex = PresetDB.UserPresets.Count - 1;
            RevertCurrentPreset();
        }
        else
        {
            Service.Log($"[PD] Save called when current preset #{_selectedPresetIndex} (default={_selectedPresetDefault}) is not modified or has bad name '{_selectedPreset?.Preset.Name}'");
        }
    }

    private void SaveCurrentPresetAsCopy()
    {
        if (_selectedPresetIndex >= 0 && _selectedPreset != null)
        {
            _selectedPreset.DetachFromSource();
            _selectedPreset.MakeNameUnique();
            _selectedPresetIndex = PresetDB.UserPresets.Count;
            _selectedPresetDefault = false;
            PresetDB.Modify(-1, _selectedPreset.Preset);
            RevertCurrentPreset();
        }
        else
        {
            Service.Log($"[PD] Save-as called when no preset is selected");
        }
    }

    private void CreateNewPreset(int referenceIndex, bool referenceDefault)
    {
        _selectedPresetIndex = -1;
        _selectedPresetDefault = false;
        _selectedPreset = new(PresetDB, referenceIndex, referenceDefault, _selectedModuleType);
        _selectedPreset.DetachFromSource();
        _selectedPreset.MakeNameUnique();
    }

    private void DeleteCurrentPreset()
    {
        if (!_selectedPresetDefault && _selectedPresetIndex >= 0)
        {
            PresetDB.Modify(_selectedPresetIndex, null);
            _selectedPresetIndex = -1;
            _selectedPreset = null;
        }
        else
        {
            Service.Log($"[PD] Delete called default or no preset is selected (index={_selectedPresetIndex}, default={_selectedPresetDefault})");
        }
    }

    private void CompleteChangeCurrentPreset()
    {
        _selectedPresetIndex = _pendingSelectPresetIndex;
        _selectedPresetDefault = _pendingSelectPresetDefault;
        _pendingSelectPresetIndex = -1;
        _pendingSelectPresetDefault = false;
        RevertCurrentPreset();
    }

    private void ExportToClipboard()
    {
        if (_selectedPreset != null)
        {
            ImGui.SetClipboardText(JsonSerializer.Serialize(_selectedPreset.Preset, Serialization.BuildSerializationOptions()));
        }
        else
        {
            Service.Log($"[PD] Export called no preset is selected");
        }
    }

    private void ImportNewPresetFromClipboard()
    {
        try
        {
            var finfo = new FileInfo("<import from clipboard>");
            var json = JsonNode.Parse(ImGui.GetClipboardText());

            // handle case where someone has posted the entire raw json for whatever reason
            if (json?.AsObject()?.TryGetPropertyValue("payload", out var obj) == true)
                json = obj;

            // let users import encounter-specific plans from here for convenience
            if (json?.AsObject()?.ContainsKey("Encounter") == true)
            {
                foreach (var conv in PlanPresetConverter.PlanSchema.Converters)
                    json = conv(json, 0, finfo);

                var plan = JsonSerializer.Deserialize<Plan>(json, Serialization.BuildSerializationOptions())!;
                plan.Guid = Guid.NewGuid().ToString();

                rotationDB.Plans.ModifyPlan(null, plan);

                Service.Notifications.AddNotification(new()
                {
                    Content = $"已导入 L{plan.Level} {plan.Class} 的计划 '{plan.Name}'"
                });

                return;
            }

            json = new JsonArray(json);

            foreach (var conv in PlanPresetConverter.PresetSchema.Converters)
                json = conv(json, 0, finfo);

            var preset = JsonSerializer.Deserialize<Preset>(json.AsArray()[0], Serialization.BuildSerializationOptions())!;
            _selectedPresetIndex = -1;
            _selectedPresetDefault = false;
            _selectedPreset = new(PresetDB, preset, _selectedModuleType);
        }
        catch (Exception ex)
        {
            Service.Logger.Warning(ex, $"Failed to parse preset");
            Service.Notifications.AddNotification(new()
            {
                Title = "导入预设时出错",
                Content = ex.Message,
                Type = Dalamud.Interface.ImGuiNotification.NotificationType.Warning
            });
        }
    }
}
