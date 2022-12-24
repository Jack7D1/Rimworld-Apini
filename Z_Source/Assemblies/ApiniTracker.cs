using RimWorld;
using System.Collections.Generic;
using Verse;
using AlienRace;

namespace Apini
{
    //Keeps track of generic apini types.
    [StaticConstructorOnStartup]
    public static class ApiniTracker
    {
        public static List<ThingDef> apparel = new List<ThingDef>();
        public static List<ThingDef_AlienRace> beeliens = new List<ThingDef_AlienRace> { ApiniDefOf.Apini, ApiniDefOf.Azuri, ApiniDefOf.Necropini };
        public static List<FactionDef> playerfactions = new List<FactionDef> { ApiniDefOf.TribalApiniPlayer, ApiniDefOf.TribalAzuriPlayer, ApiniDefOf.TribalNpiniPlayer };
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