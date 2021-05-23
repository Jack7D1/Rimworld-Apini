using System.Collections.Generic;
using Verse;

namespace Apini
{
    /// <summary>
    /// Tracks everything to do with Apinis.
    /// </summary>
    [StaticConstructorOnStartup]
    public static class ApiniTracker
    {
        /// <summary>
        /// Tracks all Apini aparrel.
        /// </summary>
        public static List<ThingDef> apparel = new List<ThingDef>();

        /// <summary>
        /// Pawn kinds that count as Apini.
        /// </summary>
        public static List<PawnKindDef> apiniKind = new List<PawnKindDef>();

        static ApiniTracker()
        {
            //Scour all ThingDefs for Apini aparrel.
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
            {
                if (def.HasModExtension<ApiniAparrelProperties>())
                {
                    apparel.Add(def);
                }
            }

            //Scour all PawnKindDefs for Apinis.
            foreach (PawnKindDef def in DefDatabase<PawnKindDef>.AllDefs)
            {
                if (def.HasModExtension<ApiniProperties>())
                {
                    apiniKind.Add(def);
                }
            }
        }
    }
}