using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using System.Diagnostics;
using System.IO;

namespace BossMod;

public sealed class AboutTab(DirectoryInfo? replayDir)
{
    private readonly Color DiscordColor = Color.FromComponents(88, 101, 242);

    private string _lastErrorMessage = "";

    public void Draw()
    {
        using var wrap = ImRaii.TextWrapPos(0);

        ImGui.TextUnformatted("Boss Mod (vbm) 提供了 Boss 战雷达、自动循环、冷却规划和 AI 功能。所有模块都可以单独切换。支持信息可以在此标签页底部链接的 Discord 服务器中找到。");

        if (ImGui.Button("查看更新日志"))
            Service.Mediator.Publish(new OpenWindow(typeof(ConfigChangelogWindow)));
        ImGui.SameLine();
        ImGui.AlignTextToFramePadding();
        ImGui.TextDisabled($"版本 {typeof(AboutTab).Assembly.GetName().Version}");

        ImGui.Spacing();
        DrawSection("雷达",
        [
            "雷达提供一个屏幕窗口，包含显示玩家位置、Boss 位置、即将到来的 AoE 以及其他机制的区域小地图。",
            "很有用，因为你不需要记住技能名称的含义。",
            "你可以准确地看到自己是否会被即将到来的 AoE 击中。",
            "针对支持的 Boss 启用，在“支持的 Bosses”标签页中可见。",
        ]);
        ImGui.Spacing();
        DrawSection("自动循环",
        [
            "自动循环会尽最大可能执行完全优化的循环。",
            "前往“Autorotation Presets”标签页创建预设。",
            "每个循环模块的成熟度可以通过工具提示查看。",
            "使用该功能的指南可在项目的 GitHub wiki 上找到。",
        ]);
        ImGui.Spacing();
        DrawSection("冷却规划",
        [
            "冷却规划为支持的 Boss 创建冷却计划。",
            "在特定战斗中替代自动循环。",
            "允许你在特定时间施放特定技能。",
            "使用该功能的指南可在项目的 GitHub wiki 上找到。",
        ]);
        ImGui.Spacing();
        DrawSection("AI",
        [
            "AI 在 Boss 战中自动移动。",
            "根据 Boss 模块确定的安全区域自动移动角色，显示在雷达上。",
            "不应在任何组队内容中使用。",
            "可以与其他插件连接以自动执行整个任务。",
        ]);
        ImGui.Spacing();
        DrawSection("回放",
        [
            "回放对创建 Boss 模块、分析问题以及制作冷却计划非常有用。",
            "寻求帮助时，请确保提供回放！请注意，回放中会包含你的玩家名称！",
            "在设置中启用“显示回放管理 UI”（或启用自动录制）。",
            $"文件位于 '{HideUserFolder(replayDir)}'。",
        ]);
        ImGui.Spacing();
        ImGui.Spacing();

        using (ImRaii.PushColor(ImGuiCol.Button, DiscordColor.ABGR))
            if (ImGui.Button("Puni.sh Discord"))
                _lastErrorMessage = OpenLink("https://discord.gg/Zzrcc8kmvy");
        UIMisc.SameLineOrWrap(UIMisc.ButtonWidth("Boss Mod 仓库"));
        if (ImGui.Button("Boss Mod 仓库"))
            _lastErrorMessage = OpenLink("https://github.com/awgil/ffxiv_bossmod");
        UIMisc.SameLineOrWrap(UIMisc.ButtonWidth("Boss Mod Wiki 教程"));
        if (ImGui.Button("Boss Mod Wiki 教程"))
            _lastErrorMessage = OpenLink("https://github.com/awgil/ffxiv_bossmod/wiki");
        UIMisc.SameLineOrWrap(UIMisc.ButtonWidth("打开回放文件夹"));
        if (ImGui.Button("打开回放文件夹") && replayDir != null)
            _lastErrorMessage = OpenDirectory(replayDir);
        UIMisc.SameLineOrWrap(UIMisc.ButtonWidth("爱发电"));
        if (ImGui.Button("爱发电"))
        {
            _lastErrorMessage = OpenLink("https://afdian.com/a/a_44451516");
        }

        if (_lastErrorMessage.Length > 0)
            UIMisc.Banner(FontAwesomeIcon.ExclamationTriangle, _lastErrorMessage, UIMisc.ColorDanger);
    }

    private static void DrawSection(string title, string[] bulletPoints)
    {
        UIMisc.SectionHeader(title);
        foreach (var point in bulletPoints)
        {
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.TextUnformatted(point);
        }
    }

    private static string HideUserFolder(DirectoryInfo? dir)
    {
        var path = dir?.FullName ?? "";
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return appData.Length > 0 && path.StartsWith(appData, StringComparison.OrdinalIgnoreCase) ? "%APPDATA%" + path[appData.Length..] : path;
    }

    private string OpenLink(string link)
    {
        try
        {
            Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
            return "";
        }
        catch (Exception e)
        {
            Service.Log($"Error opening link {link}: {e}");
            return $"无法打开链接 '{link}'，请在浏览器中手动打开。";
        }
    }

    private string OpenDirectory(DirectoryInfo dir)
    {
        if (!dir.Exists)
            return $"找不到目录 '{dir}'。";

        try
        {
            Process.Start(new ProcessStartInfo(dir.FullName) { UseShellExecute = true });
            return "";
        }
        catch (Exception e)
        {
            Service.Log($"Error opening directory {dir}: {e}");
            return $"无法打开文件夹 '{dir}'，请手动打开。";
        }
    }
}
