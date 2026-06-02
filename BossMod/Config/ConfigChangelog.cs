using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using System.Diagnostics.CodeAnalysis;
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
        ImGui.TextWrapped(txt);
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
        ImGui.TextWrapped("Legacy AI has been replaced by VBM AI.");
        Bullet("If you're a legacy AI user, you don't need to do anything! The existing AI interface has been retained for compatibility.");
        Bullet($"For more information, see {link}.");
        if (ImGui.Button("Open wiki"))
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
        ImGui.TextWrapped("'Smart character orientation' has been changed to be enabled by default. You can disable it in Settings -> Action Tweaks.");
    }
}

public class ConfigChangelogWindow : UIWindow
{
    private readonly Version PreviousVersion;
    private readonly List<VersionedField> Fields = [];
    private readonly List<ChangelogNotice> Notices = [];

    private int StuffCount => Fields.Count + Notices.Count;

    public ConfigChangelogWindow() : base("VBM 更新日志", true, new(400, 300))
    {
        PreviousVersion = GetPreviousPluginVersion();
        Service.Config.AssemblyVersion = GetCurrentPluginVersion();
        if (Service.Config.AssemblyVersion != PreviousVersion)
        {
            Service.Config.Modified.Fire();
            Fields = [.. GetAllFields().Where(f => f.AddedVersion > PreviousVersion)];
            Notices = [.. GetNotices().Where(f => f.Since > PreviousVersion)];
        }

        if (StuffCount == 0)
        {
            // nothing interesting to show...
            IsOpen = false;
            Dispose();
        }
    }

    public override void Draw()
    {
        ImGui.TextUnformatted($"自版本 {PreviousVersion} 以来新增了以下配置选项：");

        ImGui.Separator();

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

                ImGui.Bullet();
                if (!string.IsNullOrEmpty(disp?.Tooltip))
                    UIMisc.HelpMarker(disp!.Tooltip);
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
        Service.Config.Modified.Fire();

        Fields.Remove(field);
        if (StuffCount == 0)
            IsOpen = false;
    }

    private static IEnumerable<VersionedField> GetAllFields()
    {
        foreach (var n in Service.Config.Nodes)
        {
            var sinceNode = n.GetType().GetCustomAttribute<ConfigDisplayAttribute>()?.Since;

            foreach (var f in n.GetType().GetFields())
            {
                // i don't feel like supporting non bool fields
                if (f.FieldType != typeof(bool))
                    continue;

                if (sinceNode != null)
                    yield return new(n, f, Version.Parse(sinceNode));
                else if (f.GetCustomAttribute<PropertyDisplayAttribute>()?.Since is string sinceVersion)
                    yield return new(n, f, Version.Parse(sinceVersion));
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

    private static Version GetCurrentPluginVersion()
    {
        return Service.IsDev ? new(999, 0, 0, 0) : Assembly.GetExecutingAssembly().GetName().Version!;
    }

    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "fuck it")]
    private static Version GetPreviousPluginVersion()
    {
        // change to a smaller value to test changelog
        return Service.IsDev ? new(999, 0, 0, 0) : Service.Config.AssemblyVersion;
    }
}
