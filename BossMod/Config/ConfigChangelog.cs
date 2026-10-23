using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using System.Reflection;

namespace BossMod;

public sealed record class VersionedField(ConfigNode Node, FieldInfo FieldInfo, Version AddedVersion)
{
    public string FieldKey => $"{Node}.{FieldInfo.Name}";
}

abstract class ChangelogNotice
{
    public abstract Version Since { get; }
    public abstract void Draw();

    protected void Bullet(string txt)
    {
        ImGui.Bullet();
        ImGui.SameLine();
        ImGui.TextUnformatted(txt);
    }
}

class AIMigrationNotice : ChangelogNotice
{
    public override Version Since => new(0, 0, 0, 289);

    public override void Draw()
    {
        var link = "https://github.com/awgil/ffxiv_bossmod/wiki/AI-Migration-guide";
        ImGui.TextUnformatted("旧版 AI 已弃用，新版 AI 已上线！");
        Bullet("旧版 AI 功能现已弃用，并会在未来某个版本中移除。");
        Bullet("替代方案更简单，也更灵活强大。");
        Bullet($"详情请查看 wiki（{link}）。");
        if (ImGui.Button("打开 wiki"))
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link) { UseShellExecute = true });
            }
            catch (Exception e)
            {
                Service.Log($"Error opening link: {e}");
            }
        }
    }
}

class MultiPresetNotice : ChangelogNotice
{
    public override Version Since => new(0, 2, 3, 0);

    public override void Draw()
    {
        ImGui.TextWrapped("现在可以同时启用多个预设。");
        Bullet("新增了一个内置预设：VBM AI。它提供与旧版 AI 功能相同的能力，会尝试躲避 AoE 并自动选择敌人目标。");
        Bullet("现有 /vbm ar 命令行为不变。例如，/vbm ar set <preset> 会启用指定预设并禁用其他所有预设。若要使用多预设功能，可以使用新的子命令 'activate'、'deactivate' 和 'togglemulti'。");
    }
}

class DashSafetyNotice : ChangelogNotice
{
    public override Version Since => new(0, 2, 5, 1);

    public override void Draw()
    {
        ImGui.TextWrapped("选项“尝试避免冲进 AoE”现在默认启用。你可以在“设置 -> 技能调整”中禁用它。");
    }
}

class AIMigrationNotice2 : ChangelogNotice
{
    public override Version Since => new(7, 5, 0, 22);

    public override void Draw()
    {
        var link = "https://github.com/awgil/ffxiv_bossmod/wiki/AI-Migration-guide";
        ImGui.TextUnformatted("旧版 AI 已被 VBM AI 取代。");
        Bullet("如果你是旧版 AI 用户，无需进行任何操作！为兼容起见，现有的 AI 界面已保留。");
        Bullet($"更多信息请参阅 {link}。");
        if (ImGui.Button("打开 wiki"))
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link) { UseShellExecute = true });
            }
            catch (Exception e)
            {
                Service.Log($"Error opening link: {e}");
            }
        }
    }
}

class SmartRotationNotice : ChangelogNotice
{
    public override Version Since => new(7, 5, 0, 22);

    public override void Draw()
    {
        ImGui.TextUnformatted("智能角色朝向");
        ImGui.SameLine();
        ImGui.TextUnformatted("现已默认启用。你可以在“设置 -> 技能调整”中禁用它。");
    }
}

class ArenaScaleNotice : ChangelogNotice
{
    public override Version Since => new(7, 5, 1, 10);

    readonly BossModuleConfig _bmc = Service.Config.Get<BossModuleConfig>();
    bool _adjusted;

    public override void Draw()
    {
        var scale = ImGuiHelpers.GlobalScale;
        ImGui.TextUnformatted("VBM 雷达现在会遵循你的 Dalamud 全局界面缩放。");
        if (scale == 1)
        {
            ImGui.TextUnformatted("你当前的全局缩放为 100%，因此不会有任何变化。");
        }
        else if (_adjusted)
        {
            ImGui.Text("设置已保存。");
        }
        else
        {
            ImGui.TextUnformatted($"你保存的雷达缩放为 {_bmc.ArenaScale:f2}，应用全局缩放后现在为 {_bmc.ArenaScale * scale:f2}。");
            ImGui.TextUnformatted($"若要让雷达大小与本次更新前保持一致，应将其设置为 {_bmc.ArenaScale / scale:f2}。");
            if (ImGui.Button("自动设置"))
            {
                _bmc.ArenaScale /= scale;
                _bmc.Modified.Fire();
                _adjusted = true;
            }
        }
    }
}

class MacroNotice : ChangelogNotice
{
    public override Version Since => new(7, 5, 1, 28);

    public override void Draw()
    {
        ImGui.Text("现在可以在宏中使用");
        ImGui.SameLine();
        using (ImRaii.PushFont(Service.UiBuilder.FontMono))
            ImGui.Text("/vbm macro");
        ImGui.SameLine();
        ImGui.Text("，以便在宏执行期间启用自定义排队。");
    }
}

class DtrNotice : ChangelogNotice
{
    public override Version Since => new(7, 5, 6, 22);

    public override void Draw()
    {
        ImGui.TextUnformatted("服务器信息栏设置已移动，现在在“设置”标签页中拥有独立的分组。");
        ImGui.TextUnformatted("现在可以单独配置要显示在信息栏中的预设。配置完成后，单击预设名称即可开启或关闭，右键单击则会打开包含更多选项的上下文菜单。");
    }
}

public class ConfigChangelogWindow : UIWindow
{
    public Version PreviousVersion
    {
        get;
        private set
        {
            field = value;
            _versionText = value.ToString();
            CalculateStuff();
        }
    }
    private readonly List<VersionedField> Fields = [];
    private readonly List<ChangelogNotice> Notices = [];

    private int StuffCount => Fields.Count + Notices.Count;

    string _versionText = "";
    string _versionErr = "";

    public ConfigChangelogWindow() : base("VBM 更新日志", false, new(400, 300))
    {
        PreviousVersion = Service.Config.PreviousAssemblyVersion;
        IsOpen = StuffCount > 0;
    }

    void CalculateStuff()
    {
        Fields.Clear();
        Notices.Clear();
        Fields.AddRange(GetAllFields().Where(f => f.AddedVersion > PreviousVersion));
        Notices.AddRange(GetNotices().Where(f => f.Since > PreviousVersion));
    }

    public override void Draw()
    {
        ImGui.AlignTextToFramePadding();
        ImGui.Text("显示以下版本之后的变更：");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(100 * ImGuiHelpers.GlobalScale);
        if (ImGui.InputText("###changes", ref _versionText))
        {
            if (Version.TryParse(_versionText, out var res))
            {
                PreviousVersion = res;
                _versionErr = "";
            }
            else
                _versionErr = "请输入版本号（由点分隔的 2 到 4 位数字）。";
        }
        ImGui.SameLine();
        if (ImGui.Button("显示全部"))
            PreviousVersion = new(0, 0, 0, 0);

        if (_versionErr.Length > 0)
            ImGui.Text(_versionErr);

        ImGui.TextUnformatted($"自版本 {PreviousVersion} 以来新增了以下配置选项：");

        ImGui.Separator();

        using var _ = ImRaii.TextWrapPos(0);

        Action? postIteration = null;

        foreach (var n in Notices)
        {
            using var id = ImRaii.PushId($"notice{n.GetType()}");
            n.Draw();
            if (ImGui.Button("确定"))
                postIteration += () => Acknowledge(n);

            ImGui.Separator();
        }

        foreach (var group in Fields.GroupBy(f => f.Node.GetType()))
        {
            ImGui.TextUnformatted(group.Key.GetCustomAttribute<ConfigDisplayAttribute>()?.Name ?? "");
            foreach (var f in group)
            {
                using var id = ImRaii.PushId($"changelog{f.FieldKey}");

                var disp = f.FieldInfo.GetCustomAttribute<PropertyDisplayAttribute>();

                ImGui.AlignTextToFramePadding();
                ImGui.Bullet();
                if (!string.IsNullOrEmpty(disp?.Tooltip))
                    UIMisc.HelpMarker(disp.Tooltip);
                ImGui.SameLine();
                ImGui.TextUnformatted(disp?.Label ?? "未知");
                ImGui.SameLine();
                if (ImGui.Button("启用"))
                    postIteration += () => SetOption(f, true);
                ImGui.SameLine();
                if (ImGui.Button("禁用"))
                    postIteration += () => SetOption(f, false);
            }
        }

        if (StuffCount == 0)
            ImGui.TextUnformatted($"自版本 {PreviousVersion} 以来没有变更。");

        postIteration?.Invoke();
    }

    private void Acknowledge(ChangelogNotice n)
    {
        Notices.Remove(n);
        if (StuffCount == 0)
            IsOpen = false;
    }

    private void SetOption(VersionedField field, bool value)
    {
        field.FieldInfo.SetValue(field.Node, value);
        Fields.Remove(field);
        if (StuffCount == 0)
            IsOpen = false;
    }

    private static IEnumerable<VersionedField> GetAllFields()
    {
        foreach (var n in Service.Config.Nodes)
        {
            var sinceNode = n.GetCustomAttribute<ConfigDisplayAttribute>()?.Since;

            foreach (var f in n.GetFields())
            {
                // i don't feel like supporting non bool fields
                if (f.FieldType != typeof(bool))
                    continue;

                if (sinceNode != null)
                    yield return new(Service.Config.Get<ConfigNode>(n), f, Version.Parse(sinceNode));
                else if (f.GetCustomAttribute<PropertyDisplayAttribute>()?.Since is string sinceVersion)
                    yield return new(Service.Config.Get<ConfigNode>(n), f, Version.Parse(sinceVersion));
            }
        }
    }

    private static IEnumerable<ChangelogNotice> GetNotices()
    {
        foreach (var t in Utils.GetDerivedTypes<ChangelogNotice>(Assembly.GetExecutingAssembly()).Where(t => !t.IsAbstract))
        {
            var inst = Activator.CreateInstance(t);
            if (inst != null)
                yield return (ChangelogNotice)inst;
        }
    }
}
