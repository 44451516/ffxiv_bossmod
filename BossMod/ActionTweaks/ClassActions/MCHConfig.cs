namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig), Group = "职业专属")]
public class MCHConfig : ConfigNode
{
    [PropertyDisplay("在引导火焰喷射器时暂停自动循环")]
    public bool PauseForFlamethrower = false;
}
