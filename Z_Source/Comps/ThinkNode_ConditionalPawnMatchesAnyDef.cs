using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Apini
{
    /// <summary>
    /// Do this pawn match any of the Things in the list?
    /// </summary>
    public class ThinkNode_ConditionalPawnMatchesAnyDef : ThinkNode_Conditional
    {
        /// <summary>
        /// List of viable PawnKindDefs.
        /// </summary>
        public List<ThingDef> defs;

        /// <summary>
        /// Performs a Deep Copy. Needed for save compatibility i reckon.
        /// </summary>
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            ThinkNode_ConditionalPawnMatchesAnyDef thinkNode_ConditionalPawnMatchesAnyDef = (ThinkNode_ConditionalPawnMatchesAnyDef)base.DeepCopy(resolve);
            thinkNode_ConditionalPawnMatchesAnyDef.defs = new List<ThingDef>(defs);
            return thinkNode_ConditionalPawnMatchesAnyDef;
        }

        /// <summary>
        /// Are our conditions satisfied?
        /// </summary>
        /// <param name="pawn">Pawn we are working with.</param>
        /// <returns>True if all conditions are met. False if not.</returns>
        protected override bool Satisfied(Pawn pawn)
        {
            if (defs != null)
                return defs.Contains(pawn.def);

            return false;
        }
    }
}
