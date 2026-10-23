namespace BossMod.Shadowbringers.Ultimate.TEA;

public class GroupAssignmentFourUnique : GroupAssignment
{
    public static GroupAssignmentFourUnique Default()
    {
        var r = new GroupAssignmentFourUnique();
        r[PartyRolesConfig.Assignment.M1] = 3;
        r[PartyRolesConfig.Assignment.M2] = 2;
        r[PartyRolesConfig.Assignment.R1] = 1;
        r[PartyRolesConfig.Assignment.R2] = 0;
        r[PartyRolesConfig.Assignment.MT] = r[PartyRolesConfig.Assignment.OT] = r[PartyRolesConfig.Assignment.H1] = r[PartyRolesConfig.Assignment.H2] = 4;
        return r;
    }

    public override bool Validate()
    {
        var assigned = new int[5];

        for (var i = 0; i < (int)PartyRolesConfig.Assignment.Unassigned; i++)
        {
            if (Assignments[i] >= 0)
                assigned[Assignments[i]]++;
        }

        // TODO: implement doll skip, i don't know what people do with dolls there
        //if (assigned.Sum() == 0)
        //    return true;

        return assigned[0] == 1 && assigned[1] == 1 && assigned[2] == 1 && assigned[3] == 1;
    }
}

[ConfigDisplay(Order = 0x200, Parent = typeof(ShadowbringersConfig))]
public class TEAConfig() : ConfigNode()
{
    [PropertyDisplay("P1: 人偶分配（中间龙卷风 = 相对南方）")]
    [GroupDetails(["NW", "NE", "SE", "SW", "Ignore"])]
    [GroupPreset("NA standard", [4, 4, 4, 4, 3, 2, 1, 0])]
    //[GroupPreset("Ignore dolls", [4, 4, 4, 4, 4, 4, 4, 4])]
    public GroupAssignmentFourUnique P1DollAssignments = GroupAssignmentFourUnique.Default();

    [PropertyDisplay("P1: 将其他玩家的人偶标记为禁止目标", tooltip: "需要有效的'人偶分配'配置（文本不能是黄色）。\n\nVBM 自动旋转不会对禁止目标使用技能，如果你在动作调整设置中启用了'使用自定义队列'，也会阻止你手动这样做。\n\n如有必要，你可以通过快速按两次技能来对禁止目标使用技能。")]
    public bool P1DollPullSafety = true;

    public enum P2Intermission
    {
        [PropertyDisplay("不显示任何提示")]
        None,

        [PropertyDisplay("始终显示 W->NE 提示")]
        AlwaysFirst,

        [PropertyDisplay("1/2/5/6 使用 W->NE，3/4/7/8 使用 E->SW")]
        FirstForOddPairs,
    }

    [PropertyDisplay("中场：安全点提示")]
    public P2Intermission P2IntermissionHints = P2Intermission.FirstForOddPairs;

    [PropertyDisplay("P2: 尼西配对分配")]
    [GroupDetails(["Pair 1", "Pair 2", "Pair 3", "Pair 4"])]
    [GroupPreset("Melee together", [0, 1, 2, 3, 0, 1, 2, 3])]
    [GroupPreset("DD CCW", [0, 2, 1, 3, 1, 0, 2, 3])]
    public GroupAssignmentDDSupportPairs P2NisiPairs = GroupAssignmentDDSupportPairs.DefaultMeleeTogether();
}
