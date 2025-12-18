namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig))]
class SCHConfig : ConfigNode
{
    [PropertyDisplay("防止在开战前过早使用'热风'")]
    public bool ForbidEarlyBroil = true;
}
