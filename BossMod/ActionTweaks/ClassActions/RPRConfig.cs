namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig), Group = "职业专属")]
class RPRConfig : ConfigNode
{
    [PropertyDisplay("将地狱之门/地狱之槛与相机方向对齐")]
    public bool AlignDashToCamera = false;
}
