namespace BossMod.Stormblood.Ultimate.UWU;

[ConfigDisplay(Order = 0x210, Parent = typeof(StormbloodConfig))]
public class UWUConfig() : ConfigNode()
{
    [PropertyDisplay("泰坦石牢优先级（近 < 远）")]
    [GroupDetails(["0", "1", "2", "3", "4", "5", "6", "7"])]
    public GroupAssignmentUnique P3GaolPriorities = GroupAssignmentUnique.Default();
}
