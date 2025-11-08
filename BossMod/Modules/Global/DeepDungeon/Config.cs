using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Lumina.Excel.Sheets;

namespace BossMod.Global.DeepDungeon;

[ConfigDisplay(Name = "自动深层迷宫（实验性）", Parent = typeof(ModuleConfig))]
public class AutoDDConfig : ConfigNode
{
    public enum ClearBehavior
    {
        [PropertyDisplay("不自动选择目标")]
        None,
        [PropertyDisplay("通道开启时停止")]
        Passage,
        [PropertyDisplay("未达到等级上限时攻击所有敌人，达到后通道开启时停止")]
        Leveling,
        [PropertyDisplay("攻击所有敌人")]
        All,
    }

    [PropertyDisplay("启用模块", tooltip: "警告：此功能处于非常实验性的阶段，很可能存在漏洞或意外行为。\n要启用当前状态下的此功能，您必须在`完全任务自动化`选项卡中激活'进行中'成熟度的模块。")]
    public bool Enable = true;
    [PropertyDisplay("启用小地图")]
    public bool EnableMinimap = true;
    [PropertyDisplay("尝试避开陷阱", tooltip: "避开源自PalacePal数据的已知陷阱位置。（无论此设置如何，由洞察宝玉揭示的陷阱始终会被避开。）")]
    public bool TrapHints = true;
    [PropertyDisplay("自动导航到通路祭坛")]
    public bool AutoPassage = true;

    [PropertyDisplay("自动怪物目标行为")]
    public ClearBehavior AutoClear = ClearBehavior.Leveling;

    [PropertyDisplay("在非首领楼层禁用持续伤害DOT技能（仅影响 VBM 自动旋转）")]
    public bool ForbidDOTs = false;

    [PropertyDisplay("暂停导航前的最大拉怪数量（0 = 战斗中不进行导航）")]
    [PropertySlider(0, 15)]
    public int MaxPull = 0;
    [PropertyDisplay("尝试利用地形躲避攻击")]
    public bool AutoLOS = false;

    [PropertyDisplay("自动导航到宝箱")]
    public bool AutoMoveTreasure = true;
    [PropertyDisplay("优先开启宝箱而非通路祭坛")]
    public bool OpenChestsFirst = false;
    [PropertyDisplay("开启金宝箱")]
    public bool GoldCoffer = true;
    [PropertyDisplay("开启银宝箱")]
    public bool SilverCoffer = true;
    [PropertyDisplay("开启铜宝箱")]
    public bool BronzeCoffer = true;

    [PropertyDisplay("在进入下一层前探索所有房间")]
    public bool FullClear = false;

    public BitMask AutoPoms;
    public BitMask AutoMagicite;
    public BitMask AutoDemiclone;

    public override void DrawCustom(UITree tree, WorldState ws)
    {
        foreach (var _ in tree.Node("自动使用魔石"))
        {
            ImGui.TextWrapped("当黄金宝箱中包含你无法携带的魔石时，会自动使用已标记高亮的魔石。");
            ImGui.TextWrapped("该功能在队伍中禁用。");

            for (var i = 1; i < (int)PomanderID.Count; i++)
                using (ImRaii.PushId($"pom{i}"))
                {
                    var row = Service.LuminaRow<DeepDungeonItem>((uint)i)!.Value;
                    var wrap = Service.Texture.GetFromGameIcon(row.Icon).GetWrapOrEmpty();
                    var scale = new Vector2(32, 32) * ImGuiHelpers.GlobalScale;
                    ImGui.Image(wrap.Handle, scale, new Vector2(0, 0), tintCol: AutoPoms[i] ? new(1, 1, 1, 1) : new(1, 1, 1, 0.4f));
                    if (ImGui.IsItemClicked())
                    {
                        AutoPoms.Toggle(i);
                        Modified.Fire();
                    }
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip(row.Name.ToString());
                    if (i % 8 > 0)
                        ImGui.SameLine();
                }

            ImGui.Text(""); // undo last sameline
        }
    }
}

