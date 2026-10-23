using BossMod.Autorotation;
using Dalamud.Bindings.ImGui;
using System.Reflection;

namespace BossMod.Stormblood.Foray;

[AttributeUsage(AttributeTargets.Field)]
public sealed class NMAttribute(uint nameID, uint prepNameID, uint fateID = 0) : Attribute
{
    public uint NameID => nameID;
    public uint PrepNameID => prepNameID;
    public uint FateID => fateID;

    public string Label => $"{ModuleViewer.BNpcName(nameID)} ({ModuleViewer.BNpcName(prepNameID)})";
}

[ConfigDisplay(Name = "优雷卡", Parent = typeof(StormbloodConfig))]
public class EurekaConfig : ConfigNode
{
    [PropertyDisplay("启用区域模块")]
    public bool Enabled = true;

    [PropertyDisplay("寻找新怪物的最大范围")]
    [PropertySlider(20, 120)]
    public float MaxPullDistance = 30f;

    [PropertyDisplay("一次最多拉取的怪物数量（0 表示无限制）")]
    [PropertySlider(0, 30)]
    public int MaxPullCount = 10;

    [PropertyDisplay("只攻击已进入战斗的怪物")]
    public bool AssistMode;
}

public abstract class EurekaZone<NM> : ZoneModule where NM : struct, Enum
{
    protected readonly EurekaConfig _globalConfig = Service.Config.Get<EurekaConfig>();

    public readonly string Zone;

    private readonly EventSubscriptions _subscriptions;

    private static NMAttribute? GetAttr(Enum nm) => nm.GetType().GetField(nm.ToString())?.GetCustomAttribute<NMAttribute>();
    private static uint GetFateID(Enum nm) => GetAttr(nm)?.FateID ?? 0;
    private static uint GetPrepID(Enum nm) => GetAttr(nm)?.PrepNameID ?? 0;

    protected abstract NM FarmTarget { get; set; }

    protected EurekaZone(WorldState ws, string zone) : base(ws)
    {
        _subscriptions = new(ws.Client.FateInfo.Subscribe(OnFateSpawn));
        Zone = zone;
    }

    private void OnFateSpawn(ClientState.OpFateInfo fate)
    {
        if (GetFateID(FarmTarget) == fate.FateId)
            FarmTarget = default;
    }

    protected override void Dispose(bool disposing)
    {
        _subscriptions.Dispose();
        base.Dispose(disposing);
    }

    protected virtual void AddAIHints(int playerSlot, Actor player, AIHints hints) { }

    public override void CalculateAIHints(int playerSlot, Actor player, AIHints hints)
    {
        if (!_globalConfig.Enabled)
            return;

        AddAIHints(playerSlot, player, hints);

        hints.ForbiddenZones.RemoveAll(z => World.Actors.Find(z.tag) is Actor src && ShouldIgnore(src, player));

        var farmNameID = GetPrepID(FarmTarget);
        var farmMax = _globalConfig.MaxPullCount;
        var farmRange = _globalConfig.MaxPullDistance;

        if (farmMax > 0 && hints.PotentialTargets.Count(e => e.Priority >= 0) >= farmMax)
            return;

        if (farmNameID == 0)
            return;

        // only need to check "undesirable" targets, as mobs already attacking party members will be handled by autofarm
        bool canTarget(AIHints.Enemy enemy) => enemy.Priority == AIHints.Enemy.PriorityUndesirable && (!_globalConfig.AssistMode || enemy.Actor.InCombat);

        foreach (var e in hints.PotentialTargets.Where(t => t.Actor.NameID == farmNameID))
        {
            if (canTarget(e) && (e.Actor.Position - player.Position).LengthSq() <= farmRange * farmRange)
            {
                // only level 61+ wraiths spawn golde
                if (farmNameID == 8058 && e.Actor.ForayInfo.Level < 61)
                    continue;

                if (e.Actor.InCombat)
                    // someone else pulled
                    e.Priority = 0;
                else
                {
                    // we pull
                    e.ShouldBeTargeted = true;
                    e.Priority = 0;
                }
            }
        }
    }

    private bool ShouldIgnore(Actor caster, Actor player)
    {
        return caster.CastInfo != null
            && caster.CastInfo.Action.ID switch
            {
                15415 or 15416 => true,
                15449 or 15295 => caster.CastInfo.TargetID == player.InstanceID,
                _ => false,
            };
    }

    public override WindowDescription? GetWindowDescription() => _globalConfig.Enabled ? new($"VBM 自动：{Zone}###Eureka module", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.AlwaysAutoResize) : null;
    public override void DrawWindow(RotationModuleManager rmm)
    {
        Overlays.DrawPseudoTitlebar($"VBM 刷取：{Zone}", typeof(EurekaConfig), 61833, "禁用模块（并隐藏此浮层）", () =>
        {
            _globalConfig.Enabled = false;
            _globalConfig.Modified.Fire();
        });

        var modified = false;

        var tar = FarmTarget;
        if (UICombo.Enum("###prep", ref tar, t => (uint)(object)t == 0 ? "选择一个目标..." : GetAttr(t)?.Label ?? t.ToString()))
            FarmTarget = tar;

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        ImGui.SetNextItemWidth(200);
        modified |= ImGui.SliderFloat("搜索距离（码）", ref _globalConfig.MaxPullDistance, 20, 120);
        ImGui.SetNextItemWidth(200);
        modified |= ImGui.SliderInt("单次拉怪数量（0 表示无限制）", ref _globalConfig.MaxPullCount, 0, 30);
        modified |= ImGui.Checkbox("只攻击已进入战斗的怪物", ref _globalConfig.AssistMode);

        if (modified)
            _globalConfig.Modified.Fire();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        var isActive = rmm.ActiveModulesFlat.Any(m => m.Module.GetType().ToString() == "BossMod.Autorotation.MiscAI.AutoTarget");

        if (ImGui.Button(isActive ? "自动战斗：开" : "自动战斗：关"))
        {
            if (isActive)
                rmm.Clear();
            else
            {
                rmm.Activate(rmm.Database.Presets.FindPresetByName("VBM AI")!);
                rmm.Activate(rmm.Database.Presets.FindPresetByName("VBM Default")!);
            }
        }
    }
}
