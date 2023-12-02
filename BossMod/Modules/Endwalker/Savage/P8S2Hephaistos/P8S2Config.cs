namespace BossMod.Endwalker.Savage.P8S2
{
    [ConfigDisplay(Order = 0x182, Parent = typeof(EndwalkerConfig))]
    public class P8S2Config : CooldownPlanningConfigNode
    {
        [PropertyDisplay("万象灰烬: T/N在右")]
        public bool LimitlessDesolationTHRight = false;

        [PropertyDisplay("High concept 1: long debuffs take S towers")]
        public bool HC1LongGoS = true;

        public P8S2Config() : base(90) { }
    }
}
