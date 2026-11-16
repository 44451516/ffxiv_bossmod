namespace BossMod.Stormblood.Ultimate.UWU;

[ConfigDisplay(Order = 0x210, Parent = typeof(StormbloodConfig))]
public class UWUConfig() : ConfigNode()
{
    [PropertyDisplay("泰坦石牢优先级（近 < 远）")]
    [GroupDetails(["迦楼罗", "伊弗利特", "泰坦"])]
    [GroupPreset("H1 -> R2 -> M1", [-1, -1, 0, -1, 2, -1, -1, 1])]
    public GroupAssignment3 BeyondLimits = new() { Assignments = [-1, -1, 0, -1, 2, -1, -1, 1] };

    [PropertyDisplay("泰坦石牢优先级（近 < 远）")]
    [GroupDetails(["1", "2", "3"])]
    [GroupPreset("M1 -> M2 -> OT", [-1, 2, -1, -1, 0, 1, -1, -1])]
    public GroupAssignment3 P1Friction = new() { Assignments = [-1, 2, -1, -1, 0, 1, -1, -1] };

    [PropertyDisplay("泰坦石牢优先级（近 < 远）")]
    public bool P1FastAwaken = false;

    [PropertyDisplay("泰坦石牢优先级（近 < 远）")]
    [GroupDetails(["0", "1", "2", "3", "4", "5", "6", "7"])]
    public GroupAssignmentUnique P3GaolPriorities = GroupAssignmentUnique.Default();
}

public class GroupAssignment3 : GroupAssignment
{
    public override bool Validate()
    {
        var ass = new int[8];

        for (var i = 0; i < 8; ++i)
            if (Assignments[i] >= 0)
                ass[Assignments[i]]++;

        return ass[0] == 1 && ass[1] == 1 && ass[2] == 1;
    }
}
