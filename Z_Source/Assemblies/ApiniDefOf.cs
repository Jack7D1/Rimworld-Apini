using RimWorld;
using Verse;
using AlienRace;

namespace Apini
{
    /// <summary>
    /// Convenience class for getting Defs. Courtesy of Rimworld.
    /// </summary>
    [DefOf]
    public static class ApiniDefOf
    {
        public static ThingDef_AlienRace Apini = DefDatabase<ThingDef_AlienRace>.GetNamed("Apini");

        /// <summary>
        /// Apini hive discord.
        /// </summary>
        //public static TraitDef ApiniHiveDiscord;
        /// <summary>
        /// Apini apron apparel.
        /// </summary>
        public static ThingDef ApiniApron;

        public static ConceptDef ApiniInsectRelations;
        public static ThingDef_AlienRace Azuri = DefDatabase<ThingDef_AlienRace>.GetNamed("Azuri");
        public static ThingDef_AlienRace Necropini = DefDatabase<ThingDef_AlienRace>.GetNamed("Necropini");

        public static FactionDef TribalApiniPlayer;
        public static FactionDef TribalAzuriPlayer;
        public static FactionDef TribeApini;
        public static FactionDef TribeAzuri;
        public static FactionDef TribalNpiniPlayer;
    }
}