using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Apini.Comps
{
    public class CompApiniInsectRelations : ThingComp
    {
        public override void CompTickRare()
        {
            if (ApiniTracker.playerfactions.Contains(Faction.OfPlayer.def))
            {
                List<Pawn> pawns = new List<Pawn>();
                foreach (Pawn pawn in PawnsFinder.AllMapsAndWorld_Alive)
                    if (pawn.Faction == Faction.OfInsects && !pawn.Dead)
                        pawns.Add(pawn);
                if (pawns.Count == 0 && Faction.OfInsects.HostileTo(Faction.OfPlayer))
                    Faction.OfInsects.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Neutral, false);
            }
        }

        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            //Bunch of checks to filter against redundancy or unrelated damage.
            if (dinfo.Instigator?.Faction == null || !dinfo.Instigator.Faction.IsPlayer || !ApiniTracker.playerfactions.Contains(dinfo.Instigator.Faction.def) || parent.HostileTo(dinfo.Instigator))
                return;
            int attackDist = (int)Math.Ceiling(dinfo.Instigator.Position.DistanceTo(parent.Position));
            if (AttackDistanceProvokes(attackDist) && dinfo.Category != DamageInfo.SourceCategory.Collapse)
            {
                Faction.OfInsects.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile);
            }
        }

        //Curves the range of attack to a probability of provoking the hive.
        private static bool AttackDistanceProvokes(int d)
        {
            // (<=1 = 1) ∪ (>=15 = 0)
            return Rand.Chance(1f - (0.07f * d)); //Rand.Chance clamps values already so there is no need to do it here.
        }
    }

    public class IncidentWorkerApini_HiveAngered : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms = null)
        {
            Faction.OfInsects.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, "You dun fucked up");
            return true;
        }
    }
}