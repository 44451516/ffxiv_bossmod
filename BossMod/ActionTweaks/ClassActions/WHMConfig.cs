namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig), Group = "职业专属")]
class WHMConfig : ConfigNode
{
    [PropertyDisplay("将以太步与相机方向对齐")]
    public bool AlignDashToCamera = false;
}
