using Dalamud.Interface.Utility.Raii;
using Dalamud.Utility;
using Dalamud.Bindings.ImGui;

namespace BossMod.Autorotation;

public sealed class UIRotationModule
{
    public static void DescribeModule(Type type, RotationModuleDefinition definition)
    {
        ImGui.TextUnformatted(definition.DisplayName);
        ImGui.TextUnformatted(definition.Description);
        ImGui.TextUnformatted($"L{definition.MinLevel}-{definition.MaxLevel} {string.Join(" ", definition.Classes.SetBits().Select(b => (Class)b))}");
        ImGui.TextUnformatted($"作者/贡献者：{definition.Author}");
        ImGui.TextUnformatted($"质量：{(int)definition.Quality}/{(int)RotationModuleQuality.Count - 1} {definition.Quality.GetAttribute<PropertyDisplayAttribute>()?.Label ?? ""}");
        using (ImRaii.Disabled())
        {
            ImGui.TextUnformatted($"Class: {type.FullName}");
            ImGui.TextUnformatted($"排序组：{definition.Order}");
        }
    }
}
