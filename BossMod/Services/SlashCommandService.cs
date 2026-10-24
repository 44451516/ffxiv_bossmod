using BossMod.AI;
using BossMod.Autorotation;
using BossMod.Pathfinding;
using DalaMock.Host.Mediator;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Shell;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BossMod.Services;

internal sealed class SlashCommandService(
    ICommandManager commandManager,
    MediatorService mediator,
    ObstacleMapManager obstacles,
    AIHints hintsGlobal,
    IAmex amex,
    RotationModuleManager rotation,
    WorldState worldstateGlobal,
    BossModuleManager bossmod
) : IHostedService, IDisposable
{
    private readonly SlashCommandProvider _slashCmd = new(commandManager, "/vbm");

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        _slashCmd.SetSimpleHandler("显示 VBM 设置界面", () => mediator.Publish(new OpenSettingsWindow()));
        _slashCmd.AddSubcommand("r").SetSimpleHandler("显示/隐藏回放管理窗口", () => mediator.Publish(new ToggleWindow(typeof(ReplayManagementWindow))));
        RegisterAutorotationSlashCommands(_slashCmd.AddSubcommand("ar"));
        RegisterAISlashCommands(_slashCmd.AddSubcommand("ai"));
        _slashCmd.AddSubcommand("cfg").SetComplexHandler("<config-type> <field> [<value>]", "查询或修改配置项", args =>
        {
            var output = Service.Config.ConsoleCommand(args);
            foreach (var msg in output)
                Service.ChatGui.Print(msg);
            return true;
        });
        _slashCmd.AddSubcommand("d").SetSimpleHandler("显示调试界面", () => Service.Mediator.Publish(new OpenWindow(typeof(MainDebugWindow))));
        _slashCmd.AddSubcommand("gc").SetSimpleHandler("执行 C# 垃圾回收", () =>
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        });

        _slashCmd.AddSubcommand("clear-maps").SetSimpleHandler("清除所有生成的位图（用于寻路）", obstacles.ClearGenerated);

        _slashCmd.AddSubcommand("macro").SetSimpleHandler("在宏中启用 VBM 技能队列（默认情况下，宏中使用的技能会绕过队列）", () =>
        {
            unsafe
            {
                if (RaptureShellModule.Instance()->MacroCurrentLine >= 0)
                    amex.MacroCapture = true;
                else
                    Service.ChatMessage("只有在宏中使用此命令才会生效。");
            }
        });
        _slashCmd.AddSubcommand("macro-off").SetSimpleHandler("禁用宏中的 VBM 技能队列（若之前已启用）", () =>
        {
            unsafe
            {
                if (RaptureShellModule.Instance()->MacroCurrentLine >= 0)
                    amex.MacroCapture = false;
                else
                    Service.ChatMessage("只有在宏中使用此命令才会生效。");
            }
        });

        _slashCmd.AddSubcommand("helpme").SetSimpleHandler("收集诊断信息", HelpMe);

        _slashCmd.Register();
        _slashCmd.RegisterAlias("/vbmai", "ai"); // TODO: deprecated

        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public void Dispose()
    {
        _slashCmd.Dispose();
    }

    private void RegisterAutorotationSlashCommands(SlashCommandHandler cmd)
    {
        void SetOrToggle(Preset preset, bool toggle, bool exclusive)
        {
            if (toggle)
            {
                var verb = rotation.Presets.Contains(preset) ? "disables" : "enables";
                Service.Log($"Console: toggle {verb} preset '{preset.Name}'");
                rotation.Toggle(preset, exclusive);
            }
            else
            {
                Service.Log($"Console: set activates preset '{preset.Name}'");
                rotation.Activate(preset, exclusive);
            }
        }

        void SetOrToggleByName(ReadOnlySpan<char> presetName, bool toggle, bool exclusive)
        {
            var preset = rotation.Database.Presets.FindPresetByName(presetName);
            if (preset != null)
                SetOrToggle(preset, toggle, exclusive);
            else
                Service.ChatError($"找不到预设 '{presetName}'");
        }

        void ClearByName(ReadOnlySpan<char> presetName)
        {
            var preset = rotation.Database.Presets.FindPresetByName(presetName);
            if (preset != null)
            {
                Service.Log($"Console: unset deactivates preset '{preset.Name}'");
                rotation.Deactivate(preset);
            }
            else
                Service.ChatError($"找不到预设 '{presetName}'");
        }

        cmd.SetSimpleHandler("切换自动循环界面", () => mediator.Publish(new ToggleWindow(typeof(UIRotationWindow))));
        cmd.AddSubcommand("clear").SetSimpleHandler("清除当前预设；除非计划处于激活状态，否则自动循环不会执行任何操作", () =>
        {
            Service.Log($"Console: clearing autorotation preset(s) '{rotation.PresetNames}'");
            rotation.Clear();
        });
        cmd.AddSubcommand("disable").SetSimpleHandler("强制禁用自动循环；即使计划处于激活状态，也不会自动执行任何技能", () =>
        {
            Service.Log($"Console: force-disabling from presets '{rotation.PresetNames}'");
            rotation.SetForceDisabled();
        });
        cmd.AddSubcommand("set").SetComplexHandler("<preset>", "开始执行指定预设，并停用其他预设", preset =>
        {
            SetOrToggleByName(preset, false, true);
            return true;
        });
        var toggle = cmd.AddSubcommand("toggle");
        toggle.SetSimpleHandler("若尚未强制禁用自动循环则将其禁用，否则清除覆盖设置", () => SetOrToggle(RotationModuleManager.ForceDisable, true, true));
        toggle.SetComplexHandler("<preset>", "若指定预设未激活则开始执行，已激活则清除", preset =>
        {
            SetOrToggleByName(preset, true, true);
            return true;
        });

        cmd.AddSubcommand("activate").SetComplexHandler("<preset>", "将指定预设添加到激活列表", preset =>
        {
            SetOrToggleByName(preset, false, false);
            return true;
        });
        cmd.AddSubcommand("deactivate").SetComplexHandler("<preset>", "将指定预设从激活列表中移除", preset =>
        {
            ClearByName(preset);
            return true;
        });
        cmd.AddSubcommand("togglemulti").SetComplexHandler("<preset>", "若指定预设在激活列表中则禁用，否则启用", preset =>
        {
            SetOrToggleByName(preset, true, false);
            return true;
        });
    }

    private void RegisterAISlashCommands(SlashCommandHandler cmd)
    {
        var aiConfig = Service.Config.Get<AIConfig>();
        cmd.SetSimpleHandler("切换多开界面", () =>
        {
            aiConfig.DrawUI ^= true;
            aiConfig.Modified.Fire();
        });
        cmd.AddSubcommand("on").SetSimpleHandler("启用 AI 模式", () =>
        {
            var was = aiConfig.Enabled;
            aiConfig.Enabled = true;
            aiConfig.Modified.Fire();
            if (aiConfig.ChatConfirmation && !was)
                Service.ChatMessage("AI 已启用");
        });
        cmd.AddSubcommand("off").SetSimpleHandler("禁用 AI 模式", () =>
        {
            var was = aiConfig.Enabled;
            aiConfig.Enabled = false;
            aiConfig.Modified.Fire();
            if (aiConfig.ChatConfirmation && was)
                Service.ChatMessage("AI 已禁用");
        });
        cmd.AddSubcommand("toggle").SetSimpleHandler("切换 AI 模式", () =>
        {
            aiConfig.Enabled ^= true;
            aiConfig.Modified.Fire();
            if (aiConfig.ChatConfirmation)
                Service.ChatMessage($"AI {(aiConfig.Enabled ? "已启用" : "已禁用")}");
        });
        cmd.AddSubcommand("follow").SetComplexHandler("<name>/slot<N>", "启用多开模式，并跟随指定名称或指定位置的队伍成员", masterString =>
        {
            var ms = masterString.ToString();
            var masterSlot = masterString.StartsWith("slot", StringComparison.OrdinalIgnoreCase) ? int.Parse(masterString[4..]) - 1 : Array.FindIndex(worldstateGlobal.Party.Members, m => worldstateGlobal.Actors.Find(m.InstanceId)?.Name == ms);
            if (worldstateGlobal.Party[masterSlot] != null)
            {
                mediator.Publish(new SetFollowSlot(masterSlot));
                if (aiConfig.ChatConfirmation)
                    Service.ChatMessage($"AI 跟随位置 = {masterSlot}");
            }
            else
                Service.ChatError($"错误：在队伍中找不到 {masterString}");
            return true;
        });

        // TODO: this should really be removed, it's a weird synonym for /vbm cfg AIConfig ...
        cmd.SetComplexHandler("", "", args =>
        {
            Service.Log($"vbmai args: {args}");
            Span<Range> ranges = stackalloc Range[2];
            var numRanges = args.Split(ranges, ' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (numRanges == 1)
            {
                // toggle
                var value = Service.Config.ConsoleCommand($"AIConfig {args}");
                return bool.TryParse(value[0], out var boolValue) && Service.Config.ConsoleCommand($"AIConfig {args} {!boolValue}").Count == 0;
            }
            else if (numRanges == 2)
            {
                // set
                var value = args[ranges[1]];
                if (value.Equals("on", StringComparison.InvariantCultureIgnoreCase))
                    value = "true";
                else if (value.Equals("off", StringComparison.InvariantCultureIgnoreCase))
                    value = "false";
                return Service.Config.ConsoleCommand($"AIConfig {args[ranges[0]]} {value}").Count == 0;
            }
            return false;
        });
    }

    private void HelpMe()
    {
        StringBuilder diag = new();

        var i = 0;

        diag.AppendLine($"VBM {typeof(TickService).Assembly.GetName().Version}");
        diag.AppendLine($"current zone: {worldstateGlobal.CurrentZone}/{worldstateGlobal.CurrentCFCID}");

        var player = worldstateGlobal.Party.Player();

        if (player != null)
        {
            diag.AppendLine();
            diag.AppendLine("player:");
            diag.AppendLine($"L{player.Level} {player.Class}, HP {player.HPMP.CurHP}/{player.HPMP.MaxHP}, MP {player.HPMP.CurMP}/{player.HPMP.MaxMP}");

            diag.AppendLine();
            diag.AppendLine("statuses:");
            foreach (var sid in player.Statuses)
            {
                if (sid.ID == 0)
                    continue;
                var desc = Service.LuminaRow<Lumina.Excel.Sheets.Status>(sid.ID);
                diag.AppendLine($"status [{i++}]: {sid.ID} '{desc?.Name ?? "unknown"}' p=0x{sid.Extra:X} ({(sid.ExpireAt - worldstateGlobal.CurrentTime).TotalSeconds:f1})");
            }
        }

        diag.AppendLine();
        diag.AppendLine("autorot:");
        foreach (var m in rotation.Presets)
        {
            diag.AppendLine($"{m.Name}");
            foreach (var mod in m.Modules)
            {
                diag.AppendLine($"- {mod.Definition.DisplayName} ({mod.Type.FullName})");
                foreach (var s in mod.TransientSettings)
                {
                    var track = mod.Definition.Configs[s.Track];
                    diag.AppendLine($"  - {track.InternalName} = {track.ToDisplayString(s.Value)}");
                }
            }
        }

        diag.AppendLine();
        diag.AppendLine("modules:");
        foreach (var m in bossmod.LoadedModules)
        {
            var s = m.StateMachine.ActiveState == null ? "inactive" : "active";
            diag.AppendLine($"{m.GetType()} ({s}) (OID=0x{m.PrimaryActor.OID:X})");
        }

        if (player != null)
        {
            diag.AppendLine();
            diag.AppendLine("bitmap:");

            var (entry, _) = obstacles.Find(player.PosRot.XYZ());
            if (entry != null)
                diag.AppendLine(entry.ToString());

            var (map, rect) = hintsGlobal.PathfindMapObstacles;
            diag.AppendLine($"current bounds: {rect}");

            if (map != null)
            {
                var offX = -rect.Left;
                var offY = -rect.Top;
                var r = rect.Clamped(map.FullRect).Clamped(new(0, 0, map.Width, map.Height), offX, offY);
                var allPx = (r.Bottom - r.Top) * (r.Right - r.Left);
                var filledPx = 0;
                for (var y = r.Top; y < r.Bottom; ++y)
                    for (var x = r.Left; x < r.Right; ++x)
                        if (map[x, y])
                            filledPx++;

                diag.AppendLine($"map is {filledPx}/{allPx} filled ({filledPx / (float)allPx * 100:f2}%)");
            }
        }

        ImGui.SetClipboardText($"```{diag.ToString()}```");

        Service.ChatMessage("诊断信息已复制到剪贴板。");
    }
}
