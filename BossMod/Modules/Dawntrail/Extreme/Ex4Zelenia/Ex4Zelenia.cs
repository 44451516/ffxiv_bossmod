namespace BossMod.Dawntrail.Extreme.Ex4Zelenia;

[ConfigDisplay(Parent = typeof(DawntrailConfig))]
public class Ex4ZeleniaConfig : ConfigNode
{
    [PropertyDisplay("显示神圣危险（绽放6）的AOE提示", tooltip: "此机制可以使用坦克LB或减伤忽略。如果希望小地图上更简洁，请禁用。")]
    public bool ShowHolyHazard = true;
}

class ThornedCatharsis : Components.RaidwideCast
{
    public ThornedCatharsis(BossModule module) : base(module, AID.ThornedCatharsis)
    {
        KeepOnPhaseChange = true;
    }
}

// TODO: subsequent casts don't trigger if an earlier cast kills the target
class StockBreak(BossModule module) : Components.UniformStackSpread(module, 6, 0)
{
    public int NumCasts;

    public override void OnEventIcon(Actor actor, uint iconID, ulong targetID)
    {
        if (iconID == (uint)IconID.StockBreak)
            AddStack(actor, WorldState.FutureTime(8.3f));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        switch ((AID)spell.Action.ID)
        {
            case AID.StockBreak1:
            case AID.StockBreak2:
                NumCasts++;
                break;
            case AID.StockBreak3:
                NumCasts++;
                Stacks.Clear();
                break;
        }
    }
}

class P1Explosion(BossModule module) : Components.CastTowers(module, AID.P1Explosion, 3);

class SpecterOfTheLost(BossModule module) : Components.BaitAwayTethers(module, new AOEShapeCone(48, 30.Degrees()), (uint)TetherID.SpecterOfTheLost, AID.SpecterOfTheLost);
class SpecterOfTheLostAOE(BossModule module) : Components.StandardAOEs(module, AID.SpecterOfTheLost, new AOEShapeCone(48, 30.Degrees()));
class PerfumedQuietus(BossModule module) : Components.RaidwideCast(module, AID.PerfumedQuietus);
class AlexandrianBanishII(BossModule module) : Components.StackWithIcon(module, (uint)IconID.AlexandrianBanishII, AID.AlexandrianBanishIIStack, 5, 5.8f);

[ModuleInfo(BossModuleInfo.Maturity.Verified, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1031, NameID = 13861, PlanLevel = 100)]
public class Ex4Zelenia(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsCircle(16));
