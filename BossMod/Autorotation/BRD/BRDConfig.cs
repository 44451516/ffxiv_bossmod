namespace BossMod;

[ConfigDisplay(Parent = typeof(AutorotationConfig))]
class BRDConfig : ConfigNode
{
    // 在使用Heavy Shot（单体）或Quick Nock（AOE）时执行最佳循环
    [PropertyDisplay("在使用Heavy Shot（单体）或Quick Nock（AOE）时执行最佳循环")]
    public bool FullRotation = true;

    // 智能目标选择，用于Warden's Paean（优先目标，如果友好，则选鼠标悬停目标，如果友好，否则选择随机带有可解除debuff的队员，如果没有，则选择自己）
    [PropertyDisplay("智能目标选择，用于Warden's Paean（优先目标，如果友好，则选鼠标悬停目标，如果友好，否则选择随机带有可解除debuff的队员，如果没有，则选择自己）")]
    public bool SmartWardensPaeanTarget = true;
}
