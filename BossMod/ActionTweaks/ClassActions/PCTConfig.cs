namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig))]
class PCTConfig : ConfigNode
{
    [PropertyDisplay("将污迹与相机方向对齐")]
    public bool AlignDashToCamera = false;
}
