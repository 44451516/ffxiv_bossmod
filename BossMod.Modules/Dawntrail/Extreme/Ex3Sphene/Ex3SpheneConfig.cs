namespace BossMod.Dawntrail.Extreme.Ex3Sphene;

[ConfigDisplay(Order = 0x030, Parent = typeof(DawntrailConfig))]
class Ex3SpheneConfig() : ConfigNode()
{
    [PropertyDisplay("绝对权威：忽略光球，一起集合")]
    public bool AbsoluteAuthorityIgnoreFlares = true;
}
