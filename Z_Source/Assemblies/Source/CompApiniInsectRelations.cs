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
            if (dinfo.Instigator?.Faction == null || parent.Faction != Faction.OfInsects || !ApiniTracker.playerfactions.Contains(dinfo.Instigator.Faction.def) || parent.HostileTo(dinfo.Instigator)){return;}
            //Actual Check
            if (AttackProvokes(dinfo, parent))
            {
                Faction.OfInsects.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true);
            }
        }

        //Curves the range of attack to a probability of provoking the hive.
        private static bool AttackProvokes(DamageInfo dinfo, ThingWithComps parent)
        {
            if(dinfo.Category == DamageInfo.SourceCategory.Collapse){return false;}

            int attackDist = (int)Math.Ceiling(dinfo.Instigator.Position.DistanceTo(parent.Position));
            // (<=1 = 1) ∪ (>=15 = 0)
            return Rand.Chance(1f - (0.07f * attackDist)); //Rand.Chance clamps values already so there is no need to do it here.
        }
    }
}