using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Apini
{
    /// <summary>
    /// This thought causes a vector of conflict between pawns with Hive Discord trait and Apinis.
    /// </summary>
    public class ThoughtWorker_ApiniHiveDiscord : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
        {
            bool pawn_is_apini = false;
            bool other_pawn_is_apini = false;
            //PawnKindDef apini_kind_def = ApiniDefOf.ApiniPlayer;
            //if (apini_kind_def == null)
            //    return false;
            if (pawn.kindDef.IsApini())
            {
                pawn_is_apini = true;
            }
            if (other.kindDef.IsApini())
            {
                other_pawn_is_apini = true;
            }
            if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
            {
                return false;
            }
            //TraitDef trait_def = ApiniDefOf.ApiniHiveDiscord;
            TraitDef trait_def = null;
            if (trait_def == null)
                return false;

            if (other.story.traits.HasTrait(trait_def))
            {
                if(pawn_is_apini)
                {
                    if (other_pawn_is_apini)
                        return ThoughtState.ActiveAtStage(1);
                    else
                        return ThoughtState.ActiveAtStage(0);
                }
            }

            return false;
        }
    }
}
