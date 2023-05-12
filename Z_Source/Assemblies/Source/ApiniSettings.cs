using UnityEngine;
using Verse;
using AlienRace;
using RimWorld;

namespace Apini
{   //Mod Settings
    public class ApiniSettings : ModSettings
    {
        public static bool disableApparelRestrictions = false;
        public static bool disableBuildingRestrictions = false;
        public static bool disablePlantingRestrictions = false;
        public static bool disableTraitRestrictions = false;
        public static bool forceInsectHostility = false;
        public static bool disableSmokeleafAllergy = false;
        //public bool disableNecropini = false;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref disableApparelRestrictions, "disableApparelRestrictions");
            Scribe_Values.Look(ref disableBuildingRestrictions, "disableBuildingRestrictions");
            Scribe_Values.Look(ref disablePlantingRestrictions, "disablePlantingRestrictions");
            Scribe_Values.Look(ref disableTraitRestrictions, "disableTraitRestrictions");
            Scribe_Values.Look(ref forceInsectHostility, "forceInsectHostility");
            Scribe_Values.Look(ref disableSmokeleafAllergy, "disableSmokeleafAllergy");
            base.ExposeData();
        }
    }

    public class ApiniSettingsMenu : Mod
    {
        ApiniSettings settings;

        public ApiniSettingsMenu(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<ApiniSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Settings will take effect after restarting.");
            listingStandard.Gap();
            listingStandard.Label("Race restrictions:");
            listingStandard.CheckboxLabeled("Disable Apparel Restrictions", ref ApiniSettings.disableApparelRestrictions);
            listingStandard.CheckboxLabeled("Disable Building Restrictions", ref ApiniSettings.disableBuildingRestrictions);
            listingStandard.CheckboxLabeled("Disable Planting Restrictions", ref ApiniSettings.disablePlantingRestrictions);
            listingStandard.CheckboxLabeled("Disable Disallowed Traits", ref ApiniSettings.disableTraitRestrictions);
            listingStandard.Gap();
            listingStandard.Label("Gameplay Customization:");
            listingStandard.CheckboxLabeled("Disable Insect Diplomacy (Wild insects are always hostile to apini)", ref ApiniSettings.forceInsectHostility);
            listingStandard.CheckboxLabeled("Disable apini smokeleaf allergy.", ref ApiniSettings.disableSmokeleafAllergy);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Apini";
        }
    }

    //Settings Processor, runs once on startup and clears certain restrictions dependsing on settings.
    [StaticConstructorOnStartup]
    static class ApiniSettingsProcessor
    {
        static ApiniSettingsProcessor()
        {
            //Beelien iterator
            foreach(ThingDef_AlienRace beelien in ApiniTracker.beeliens){ 
                //Race restrictions
                if(ApiniSettings.disableApparelRestrictions){
                    beelien.alienRace.raceRestriction.apparelList.Clear();
                    beelien.alienRace.raceRestriction.onlyUseRaceRestrictedApparel = false;
                }
                if(ApiniSettings.disableBuildingRestrictions){
                    beelien.alienRace.raceRestriction.buildingList.Clear();
                }
                if(ApiniSettings.disablePlantingRestrictions){
                    beelien.alienRace.raceRestriction.plantList.Clear();
                }
                if(ApiniSettings.disableTraitRestrictions){
                    beelien.alienRace.generalSettings.disallowedTraits.Clear();
                }

                //Gameplay mods
                if(ApiniSettings.forceInsectHostility){
                    foreach(FactionRelationSettings relation in beelien.alienRace.generalSettings.factionRelations){
                        if(relation.factions.Contains(FactionDefOf.Insect)){
                            relation.goodwill.min = -100;
                            relation.goodwill.max = -100;
                            break;
                        }
                    }
                }
                if(ApiniSettings.disableSmokeleafAllergy){
                    foreach(ChemicalSettings chem in beelien.alienRace.generalSettings.chemicalSettings){
                        if(chem.chemical.defName == "Smokeleaf"){
                            chem.reactions.Clear();
                        }
                    }
                }
            }
        }
    }
}
