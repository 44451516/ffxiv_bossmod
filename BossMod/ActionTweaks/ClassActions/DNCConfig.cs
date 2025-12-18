namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig))]
class DNCConfig : ConfigNode
{
    [PropertyDisplay("将前冲步与相机方向对齐")]
    public bool AlignDashToCamera = false;
}
