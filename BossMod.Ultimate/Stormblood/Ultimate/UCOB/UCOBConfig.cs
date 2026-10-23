using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using System.Reflection;

namespace BossMod.Stormblood.Ultimate.UCOB;

[ConfigDisplay(Order = 0x200, Parent = typeof(StormbloodConfig))]
public class UCOBConfig() : ConfigNode()
{
    [PropertyDisplay("P1 火球 1：为坦克和治疗者添加避免分摊的提示")]
    public bool P1Fireball1LBHints = true;

    [PropertyDisplay("P3 快步/天降三重奏：安全点分配（假设巴哈姆特相对北方/上方，L组向左；L1/R1最接近首领）")]
    [GroupDetails(["L1", "L2", "L3", "L4", "R1", "R2", "R3", "R4"])]
    [GroupPreset("Hector: THMR", [0, 4, 1, 5, 2, 6, 3, 7])]
    [GroupPreset("LPDU: HTTH/RMMR", [1, 2, 0, 3, 5, 6, 4, 7])]
    public GroupAssignmentUnique P3QuickmarchTrioAssignments = new() { Assignments = [0, 4, 1, 5, 2, 6, 3, 7] };

    [PropertyDisplay("P3 天降三重奏：塔优先级，从奈尔开始顺时针")]
    [GroupDetails(["0", "1", "2", "3", "4", "5", "6", "7"])]
    [GroupPreset("Hector: THMR, G1 CCW, G2 CW", [7, 0, 6, 1, 5, 2, 4, 3])]
    public GroupAssignmentUnique P3HeavensfallTrioTowers = new() { Assignments = [7, 0, 6, 1, 5, 2, 4, 3] };

    [SectionStart("仅 AI 设置")]
    [PropertyDisplay("P1：负责承受 Plummet 的玩家（用于 LB）", renderer: typeof(RolesRenderer))]
    public BitMask P1PlummetTargets = new();
}

public class RolesRenderer : PropertyRenderer
{
    public override bool Draw(PropertyDisplayAttribute attrs, bool nested, ConfigNode node, FieldInfo member, object value, ConfigRoot root, UITree tree, WorldState ws)
    {
        var cfg = (UCOBConfig)node;

        ConfigUI.DrawHelp(attrs.Tooltip, nested);

        var modified = false;

        foreach (var _ in tree.Node(attrs.Label, false))
        {
            using (ImRaii.PushIndent())
            {
                for (var i = 2; i < 8; i++)
                {
                    var assignment = (PartyRolesConfig.Assignment)i;
                    var isChecked = cfg.P1PlummetTargets[i];
                    if (ImGui.Checkbox($"{assignment}###plummet{i}", ref isChecked))
                    {
                        if (isChecked)
                            cfg.P1PlummetTargets.Set(i);
                        else
                            cfg.P1PlummetTargets.Clear(i);
                        modified = true;
                    }
                    if (i < 7)
                        ImGui.SameLine();
                }
            }
        }

        return modified;
    }
}
