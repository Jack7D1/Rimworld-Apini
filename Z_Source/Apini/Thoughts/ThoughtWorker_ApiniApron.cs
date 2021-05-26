using RimWorld;
using System.Linq;
using Verse;

namespace Apini
{
    /// <summary>
    /// Thought for giving non Apinis a thought when the other pawn is a Apinie wearing a Apron.
    /// </summary>
    public class ThoughtWorker_ApiniApron : ThoughtWorker
    {
        /*protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            return ThoughtState.Inactive;
        }*/

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

            //Only affect non Apinis
            if (!pawn_is_apini && other_pawn_is_apini &&
                other.apparel.WornApparelCount > 0 &&
                other.apparel.WornApparel.Where(
                    apparel => ApiniTracker.apparel.Contains(apparel.def)
                               && apparel.def.GetModExtension<ApiniAparrelProperties>().isCute).Count() > 0)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            return false;
        }
    }
}