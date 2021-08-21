using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Apini
{
    //Keeps track of generic apini types.
    [StaticConstructorOnStartup]
    public static class ApiniTracker
    {
        public static List<ThingDef> apparel = new List<ThingDef>();
        public static List<ThingDef> beeliens = new List<ThingDef> { ApiniDefOf.Apini, ApiniDefOf.Azuri };
        public static List<FactionDef> playerfactions = new List<FactionDef> { ApiniDefOf.TribalApiniPlayer, ApiniDefOf.TribalAzuriPlayer };
        public static List<PawnKindDef> pawnkinds = new List<PawnKindDef>();

        static ApiniTracker()
        {
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
                if (def.HasModExtension<ApiniAparrelProperties>())
                    apparel.Add(def);

            foreach (PawnKindDef def in DefDatabase<PawnKindDef>.AllDefs)
                if (def.race == ApiniDefOf.Apini || def.race == ApiniDefOf.Azuri)
                    pawnkinds.Add(def);
        }
    }
}