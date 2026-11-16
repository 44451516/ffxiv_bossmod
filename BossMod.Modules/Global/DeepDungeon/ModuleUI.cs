using BossMod.Autorotation;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;

namespace BossMod.Global.DeepDungeon;

abstract partial class AutoClear : ZoneModule
{
    private int DrawMap(Actor? player, int playerSlot)
    {
        var ix = new Minimap(Palace, _config, player?.Rotation ?? default, DesiredRoom.Index, Math.Max(0, playerSlot)).Draw();

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
            DesiredRoom = (targetRoom, PathfindReason.Manual);

        using (ImRaii.Disabled(true))
        {
            if (DesiredRoom.Reason == PathfindReason.Nothing)
                ImGui.Text("点击房间以设置目的地。");
            else
                ImGui.Text($"当前：{ReasonReadable(DesiredRoom.Reason)}");
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        if (Service.IsDev && MostInterestingObject is { } obj)
            Service.Camera.AddLine(player?.PosRot.XYZ() ?? default, obj.Actor.PosRot.XYZ(), ArenaColor.Danger, 2, false);

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
