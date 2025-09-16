namespace BossMod.Dawntrail.Extreme.Ex5Necron;

class JailHands(BossModule module) : Components.AddsMulti(module, [OID.IcyHandsDPSJail, OID.IcyHandsHealerJail, OID.IcyHandsTankJail], 1)
{
    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        foreach (var a in ActiveActors)
        {
            var e = hints.FindEnemy(a);
            if (e != null)
            {
                e.Priority = 1;
            }

            if (e != null)
            {
                e.ForbidDOTs = true;
            }
        }
    }
}

class JailGrasp(BossModule module) : Components.StandardAOEs(module, AID.ChokingGraspDPSJail, new AOEShapeRect(24, 3));
class JailSlow(BossModule module) : Components.CastHint(module, AID.ChillingFingers, "Prepare to Esuna!");
class JailEnrage(BossModule module) : Components.CastHint(module, AID.SpreadingFearDPSJail, "Enrage!", true);
class JailInterrupt(BossModule module) : Components.CastInterruptHint(module, AID.SpreadingFearTankJail);
class JailBuster(BossModule module) : Components.SingleTargetCast(module, AID.ChokingGraspTankJail);
