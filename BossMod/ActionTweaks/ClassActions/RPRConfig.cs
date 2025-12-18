namespace BossMod;

[ConfigDisplay(Parent = typeof(ActionTweaksConfig))]
class RPRConfig : ConfigNode
{
    [PropertyDisplay("禁止在开战前过早使用勾刃")]
    public bool ForbidEarlyHarpe = true;

    [PropertyDisplay("将地狱之门/地狱之槛与相机方向对齐")]
    public bool AlignDashToCamera = false;
}
