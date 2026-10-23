namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig), Group = "职业专属")]
class DNCConfig : ConfigNode
{
    [PropertyDisplay("将前冲步与相机方向对齐")]
    public bool AlignDashToCamera = false;
}
