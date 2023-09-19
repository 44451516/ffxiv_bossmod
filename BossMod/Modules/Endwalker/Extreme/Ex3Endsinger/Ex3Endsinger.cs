
namespace BossMod.Endwalker.Extreme.Ex3Endsigner
{
    // raidwide is slightly delayed
    class Elegeia : Components.CastCounter
    {
        public Elegeia() : base(ActionID.MakeSpell(AID.Elegeia)) { }
    }

    class Telomania : Components.CastCounter
    {
        public Telomania() : base(ActionID.MakeSpell(AID.TelomaniaLast)) { }
    }

    class UltimateFate : Components.CastCounter
    {
        public UltimateFate() : base(ActionID.MakeSpell(AID.EnrageAOE)) { }
    }

    // TODO: proper tankbuster component...
    class Hubris : Components.CastCounter
    {
        public Hubris() : base(ActionID.MakeSpell(AID.HubrisAOE)) { }
    }

    // TODO: proper stacks component
    class Eironeia : Components.CastCounter
    {
        public Eironeia() : base(ActionID.MakeSpell(AID.EironeiaAOE)) { }
    }

    [ConfigDisplay(Order = 0x030, Parent = typeof(EndwalkerConfig))]
    public class Ex3EndsingerConfig : CooldownPlanningConfigNode
    {
        public Ex3EndsingerConfig() : base(90) { }
    }

    
    // public class Ex3EndsingerStates : StateMachineBuilder
    // {
    //     public Ex3EndsingerStates(BossModule module) : base(module)
    //     {
    //         // TODO: reconsider
    //         TrivialPhase()
    //             .ActivateOnEnter<GoldenTenet>()
    //             .ActivateOnEnter<StygianTenet>()
    //             .ActivateOnEnter<FlamesOfTheDead>()
    //             .ActivateOnEnter<LivingHeat>()
    //             .ActivateOnEnter<HeavensTrial>()
    //             .ActivateOnEnter<DeepestPit>()
    //             .ActivateOnEnter<OnceAboveEverBelow>()
    //             .ActivateOnEnter<HellOfFireFront>()
    //             .ActivateOnEnter<HellOfFireBack>()
    //             .ActivateOnEnter<WaywardSoul>()
    //             .ActivateOnEnter<FortuneFlux>()
    //             .ActivateOnEnter<Twingaze>();
    //     }
    // }
    //
    public class Ex3Endsinger : BossModule
    {
        public Ex3Endsinger(WorldState ws, Actor primary) : base(ws, primary, new ArenaBoundsCircle(new(100, 100), 20))
        {
            
            
        }
    }
}
