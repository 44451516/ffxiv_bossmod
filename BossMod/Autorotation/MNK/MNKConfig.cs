namespace BossMod;

[ConfigDisplay(Parent = typeof(AutorotationConfig))]
class MNKConfig : ConfigNode
{
    [PropertyDisplay("在Bootshine（单体）或Arm of the Destroyer（群体）上执行最优循环")]
    public bool FullRotation = true;


    [PropertyDisplay("Execute filler rotation (no automatic buff usage) on True Strike")]
    public bool FillerRotation = true;

    [PropertyDisplay("在Four-point Fury上执行特定形态的群体GCD")]
    public bool AOECombos = true;

    [PropertyDisplay("Automatic mouseover targeting for Thunderclap")]
    public bool SmartThunderclap = true;

    [PropertyDisplay("Delay Thunderclap if already in melee range of target")]
    public bool PreventCloseDash = true;
}
