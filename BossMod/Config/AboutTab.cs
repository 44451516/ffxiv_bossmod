using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using System.Diagnostics;
using System.IO;

namespace BossMod;

public sealed class AboutTab(DirectoryInfo? replayDir)
{
    private readonly Color TitleColor = Color.FromComponents(255, 165, 0);
    private readonly Color SectionBgColor = Color.FromComponents(38, 38, 38);
    private readonly Color BorderColor = Color.FromComponents(178, 178, 178, 204);
    private readonly Color DiscordColor = Color.FromComponents(88, 101, 242);

    private string _lastErrorMessage = "";

    public void Draw()
    {
        using var wrap = ImRaii.TextWrapPos(0);

        ImGui.TextUnformatted("Boss Mod (vbm) 提供了boss战雷达、自动循环、冷却规划和AI功能。所有模块都可以单独切换。支持信息可以在此标签页底部链接的Discord服务器中找到。");
        ImGui.Spacing();
        DrawSection("Radar",
        [
            "雷达 提供一个屏幕窗口，包含显示玩家位置、boss位置、即将到来的AoE以及其他机制的区域小地图。",
            "很有用，因为你不需要记住技能名称的含义。",
            "你可以准确地看到自己是否会被即将到来的AoE击中。",
            "针对支持的boss启用，在'支持的boss'标签页中可见。",
        ]);
        ImGui.Spacing();
        DrawSection("Autorotation",
        [
            "自动循环 尽最大可能执行完全优化的循环。",
            "前往'自动循环预设'标签页创建预设。 ",
            "每个循环模块的成熟度可以通过工具提示查看。",
            "使用该功能的指南可在项目的GitHub wiki上找到",
        ]);
        ImGui.Spacing();
        DrawSection("CD Planner",
        [
            "冷却规划 为支持的boss创建冷却计划。",
            "在特定战斗中替代自动循环。",
            "允许你在特定时间施放特定技能。",
            "使用该功能的指南可在项目的GitHub wiki上找到。",
        ]);
        ImGui.Spacing();
        DrawSection("AI",
        [
            "AI 在boss战中自动移动。",
            "根据boss模块确定的安全区域自动移动角色，显示在雷达上。",
            "不应在任何组队内容中使用。",
            "可以与其他插件连接以自动执行整个任务。",
        ]);
        ImGui.Spacing();
        DrawSection("Replays",
        [
            "回放 对创建boss模块、分析问题以及制作冷却计划非常有用。 ",
            "寻求帮助时，请确保提供回放！请注意，回放中会包含你的玩家名称！",
            "在设置中启用 > 显示回放管理UI（或启用自动录制）。",
            $"文件位于 '{replayDir}'。",
        ]);
        ImGui.Spacing();
        ImGui.Spacing();

        using (ImRaii.PushColor(ImGuiCol.Button, DiscordColor.ABGR))
            if (ImGui.Button("Puni.sh Discord", new(180, 0)))
                _lastErrorMessage = OpenLink("https://discord.gg/Zzrcc8kmvy");
        ImGui.SameLine();
        if (ImGui.Button("Boss Mod Repository", new(180, 0)))
            _lastErrorMessage = OpenLink("https://github.com/awgil/ffxiv_bossmod");
        ImGui.SameLine();
        if (ImGui.Button("Boss Mod Wiki Tutorials", new(180, 0)))
            _lastErrorMessage = OpenLink("https://github.com/awgil/ffxiv_bossmod/wiki");
        ImGui.SameLine();
        if (ImGui.Button("Open Replay Folder", new(180, 0)) && replayDir != null)
            _lastErrorMessage = OpenDirectory(replayDir);
        ImGui.SameLine();
        if (ImGui.Button("爱发电", new(180, 0)))
        {
            _lastErrorMessage = OpenLink("https://afdian.com/a/a_44451516");
        }

        if (_lastErrorMessage.Length > 0)
        {
            using var color = ImRaii.PushColor(ImGuiCol.Text, 0xff0000ff);
            ImGui.TextUnformatted(_lastErrorMessage);
        }
    }

    private void DrawSection(string title, string[] bulletPoints)
    {
        using var colorBackground = ImRaii.PushColor(ImGuiCol.ChildBg, SectionBgColor.ABGR);
        using var colorBorder = ImRaii.PushColor(ImGuiCol.Border, BorderColor.ABGR);
        using var section = ImRaii.Child(title, new(0, 150), false, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysUseWindowPadding);
        if (!section)
            return;

        using (ImRaii.PushColor(ImGuiCol.Text, TitleColor.ABGR))
        {
            ImGui.TextUnformatted(title);
        }

        ImGui.Separator();

        foreach (var point in bulletPoints)
        {
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.TextUnformatted(point);
        }
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
            return $"Failed to open link '{link}', open it manually in the browser.";
        }
    }

    private string OpenDirectory(DirectoryInfo dir)
    {
        if (!dir.Exists)
            return $"Directory '{dir}' not found.";

        try
        {
            Process.Start(new ProcessStartInfo(dir.FullName) { UseShellExecute = true });
            return "";
        }
        catch (Exception e)
        {
            Service.Log($"Error opening directory {dir}: {e}");
            return $"Failed to open folder '{dir}', open it manually.";
        }
    }
}
