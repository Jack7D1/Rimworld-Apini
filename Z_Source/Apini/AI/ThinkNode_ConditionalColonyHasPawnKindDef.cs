using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Apini
{
    /// <summary>
    /// Do the faction on the map have a pawn of a certain kind in it?
    /// </summary>
    public class ThinkNode_ConditionalColonyHasPawnKindDef : ThinkNode_Conditional
    {
        /// <summary>
        /// List of viable PawnKindDefs.
        /// </summary>
        public List<PawnKindDef> pawnKindDefs;

        /// <summary>
        /// Performs a Deep Copy. Needed for save compatibility i reckon.
        /// </summary>
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            ThinkNode_ConditionalColonyHasPawnKindDef thinkNode_ConditionalPawnMatchesAnyDef = (ThinkNode_ConditionalColonyHasPawnKindDef)base.DeepCopy(resolve);
            thinkNode_ConditionalPawnMatchesAnyDef.pawnKindDefs = new List<PawnKindDef>(pawnKindDefs);
            return thinkNode_ConditionalPawnMatchesAnyDef;
        }

        /// <summary>
        /// Are our conditions satisfied?
        /// </summary>
        /// <param name="pawn">Pawn we are working with.</param>
        /// <returns>True if all conditions are met. False if not.</returns>
        protected override bool Satisfied(Pawn pawn)
        {
            if (pawnKindDefs != null && pawn != null && pawn.Map != null && pawn.Map.mapPawns != null)
            {
                foreach (Pawn p in Find.CurrentMap.mapPawns.AllPawnsSpawned)
                {
                    if (p.kindDef == pawn.kindDef)
                    {
                        //Log.Message("Match found.");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}