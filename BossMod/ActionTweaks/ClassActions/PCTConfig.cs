namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig), Group = "职业专属")]
class PCTConfig : ConfigNode
{
    [PropertyDisplay("将污迹与相机方向对齐")]
    public bool AlignDashToCamera = false;
}
