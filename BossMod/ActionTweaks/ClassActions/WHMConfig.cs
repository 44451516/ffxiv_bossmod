namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig))]
class WHMConfig : ConfigNode
{
    [PropertyDisplay("将以太步与相机方向对齐")]
    public bool AlignDashToCamera = false;
}
