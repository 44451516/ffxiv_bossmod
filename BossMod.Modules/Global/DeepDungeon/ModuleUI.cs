using BossMod.Autorotation;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;

namespace BossMod.Global.DeepDungeon;

abstract partial class AutoClear : ZoneModule
{
    private int DrawMap(Actor? player, int playerSlot)
    {
        var ix = new Minimap(Palace, player?.Rotation ?? default, DesiredRoom, Math.Max(0, playerSlot)).Draw();

        DrawBoxes(player);

        return ix;
    }

    public override WindowDescription? GetWindowDescription() => _config.EnableMinimap && !Palace.IsBossFloor ? new("VBM 深层地下城小地图##VBMDD", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.AlwaysAutoResize) : null;

    public override void DrawWindow(RotationModuleManager rmm)
    {
        DrawTraps();

        Overlays.DrawPseudoTitlebar("VBM 深层地下城地图", typeof(AutoDDConfig), 61824, "禁用小地图（并隐藏此浮层）", () =>
        {
            _config.EnableMinimap = false;
            _config.Modified.Fire();
        });

        ImGui.Text($"击杀：{Kills}");

        var player = World.Party.Player();
        var playerSlot = Array.FindIndex(Palace.Party, p => p.EntityId == player?.InstanceID);
        var targetRoom = DrawMap(player, playerSlot);
        if (targetRoom >= 0)
            DesiredRoom = targetRoom;

        using (ImRaii.Disabled(true))
            ImGui.Text("点击房间以设置目的地。");

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        if (_palantirError != null)
        {
            ImGui.Text("无法连接 Palantir，将不显示陷阱位置。");
            if (ImGui.Button("重试"))
                FilterTraps();
        }

        var maxPull = _config.MaxPull;
        ImGui.AlignTextToFramePadding();
        ImGui.Text("最大拉怪数量");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(200);
        if (ImGui.SliderInt("###pullsize", ref maxPull, 0, 15))
        {
            _config.MaxPull = maxPull;
            _config.Modified.Fire();
        }
    }
}
