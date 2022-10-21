using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace Apini
{   //Should consider every defname on save load, and replace old apini names with these new ones provided in the replacement map. Should not touch any defnames except those listed.
    [StaticConstructorOnStartup]
    static class BackCompatibilityInjector
    {
        static BackCompatibilityInjector()
        {
            Traverse.Create(typeof(BackCompatibility)).Field<List<BackCompatibilityConverter>>("conversionChain").Value.Add(new BackCompatibilityConverter_Apini());
        }
    }
    public class BackCompatibilityConverter_Apini : BackCompatibilityConverter
    {
        //Def Replacement map
        readonly Dictionary<string, string> defMap = new Dictionary<string, string>{
            {"EmptyVat_V", "ApiniEmptyVat_V"},
            {"EmptyVat_W", "ApiniEmptyVat_W"},
            {"EmptyVat_R", "ApiniEmptyVat_R"},
            {"EmptyVat_B", "ApiniEmptyVat_B"},
            {"EmptyVat_M", "ApiniEmptyVat_M"},
            {"EmptyVat_E", "ApiniEmptyVat_E"},
            {"EmptyVat_F", "ApiniEmptyVat_F"},
            {"EmptyHoneyCapsule", "ApiniEmptyHoneyCapsule"},
            {"TakeHoneyOutOfFermentingVat", "ApiniTakeHoneyOutOfFermentingVat"},
            {"FillVat_V", "ApiniFillVat_V"},
            {"FillVat_W", "ApiniFillVat_W"},
            {"FillVat_R", "ApiniFillVat_R"},
            {"FillVat_B", "ApiniFillVat_B"},
            {"FillVat_M", "ApiniFillVat_M"},
            {"FillVat_E", "ApiniFillVat_E"},
            {"FillVat_F", "ApiniFillVat_F"},
            {"FillHoneyCapsule", "ApiniFillHoneyCapsule"},
            {"FillHoneyVat", "ApiniFillHoneyVat"},
            {"HoneyMaker", "ApiniHoneyMaker"},
            {"BeeswaxMaking", "ApiniBeeswaxMaking"},
            {"Apini_PetBase", "ApiniPetBase"},
            {"Alien_Bee", "ApiniBeelien"},
            {"PollenBall_V", "ApiniPollenBall_V"},
            {"PollenBall_W", "ApiniPollenBall_W"},
            {"PollenBall_R", "ApiniPollenBall_R"},
            {"PollenBall_B", "ApiniPollenBall_B"},
            {"PollenBall_M", "ApiniPollenBall_M"},
            {"PollenBall_E", "ApiniPollenBall_E"},
            {"PollenBall_F", "ApiniPollenBall_F"},
            {"PollenBall", "ApiniPollenBall"},
            {"PollenBallBase", "ApiniPollenBallBase"},
            {"PollenBase", "ApiniPollenBase"},
            {"PlantVigorbloom", "ApiniPlantVigorbloom"},
            {"PlantWiserbud", "ApiniPlantWiserbud"},
            {"PlantRushthorn", "ApiniPlantRushthorn"},
            {"PlantBardsong", "ApiniPlantBardsong"},
            {"PlantMetalily", "ApiniPlantMetalily"},
            {"PlantEmberose", "ApiniPlantEmberose"},
            {"PlantFrostbell", "ApiniPlantFrostbell"},
            {"PlantDaisyApini", "ApiniPlantDaisy"},
            {"PlantLavenderApini", "ApiniPlantLavender"},
            {"Pollen", "ApiniPollen"},
            {"BaseHemolymph", "ApiniHemolymphBase"},
            {"WaxcraftBase", "ApiniWaxcraftBase"},
            {"NectarCraftBulk", "ApiniNectarCraftBulk"},
            {"NectarCraftBase", "ApiniNectarCraftBase"},
            {"Nectar_V", "ApiniNectar_V"},
            {"Nectar_W", "ApiniNectar_W"},
            {"Nectar_R", "ApiniNectar_R"},
            {"Nectar_B", "ApiniNectar_B"},
            {"Nectar_M", "ApiniNectar_M"},
            {"Nectar_E", "ApiniNectar_E"},
            {"Nectar_F", "ApiniNectar_F"},
            {"Nectar", "ApiniNectar"},
            {"NectarBase", "ApiniNectarBase"},
            {"InsectBodyPartApitech", "ApiniInsectBodyPartApitech"},
            {"InsectBodyPartApiniAdvancedBionicsTech", "ApiniInsectBodyPartAdvancedBionicsTech"},
            {"InsectBodyPartApiniBionicsTech", "ApiniInsectBodyPartBionicsTech"},
            {"InsectBodyPartApiniProstheticsTech", "ApiniInsectBodyPartProstheticsTech"},
            {"InsectBodyPartNatural", "ApiniInsectBodyPartNatural"},
            {"InsectBodyPartBase", "ApiniInsectBodyPartBase"},
            {"BruteHoneyBuff", "ApiniBruteHoneyBuff"},
            {"SmartHoneyBuff", "ApiniSmartHoneyBuff"},
            {"SwiftHoneyBuff", "ApiniSwiftHoneyBuff"},
            {"SmoothHoneyBuff", "ApiniSmoothHoneyBuff"},
            {"RoughHoneyBuff", "ApiniRoughHoneyBuff"},
            {"HotHoneyBuff", "ApiniHotHoneyBuff"},
            {"CoolHoneyBuff", "ApiniCoolHoneyBuff"},
            {"Honey_V", "ApiniHoney_V"},
            {"Honey_W", "ApiniHoney_W"},
            {"Honey_R", "ApiniHoney_R"},
            {"Honey_B", "ApiniHoney_B"},
            {"Honey_M", "ApiniHoney_M"},
            {"Honey_E", "ApiniHoney_E"},
            {"Honey_F", "ApiniHoney_F"},
            {"SpecialHoney", "ApiniSpecialHoney"},
            {"HoneyBase", "ApiniHoneyBase"},
            {"NectarCategory", "ApiniNectarCategory"},
            {"HoneyVat", "ApiniHoneyVat"},
            {"Vat_V", "ApiniVat_V"},
            {"Vat_W", "ApiniVat_W"},
            {"Vat_R", "ApiniVat_R"},
            {"Vat_B", "ApiniVat_B"},
            {"Vat_M", "ApiniVat_M"},
            {"Vat_E", "ApiniVat_E"},
            {"Vat_F", "ApiniVat_F"},
            {"VatBase", "ApiniVatBase"},
            {"HoneyCapsule", "ApiniHoneyCapsule"},
            {"BeeswaxCandle", "ApiniWaxCandle"},
            {"HoneycombBed_L", "ApiniHoneycombBed_L"},
            {"HoneycombBed_S", "ApiniHoneycombBed_S"},
            {"HoneyWall", "ApiniHoneyWall"},
            {"InsectBodyPartsApitech", "ApiniInsectBodyPartsApitech"},
            {"InsectBodyPartsArtificial", "ApiniInsectBodyPartsArtificial"},
            {"InsectBodyPartsNatural", "ApiniInsectBodyPartsNatural"},
            {"InsectBodyParts", "ApiniInsectBodyParts"},
            {"Pawn_Moobee_Death", "Moobee_Pawn_Death"},
            {"Pawn_Moobee_Wounded", "Moobee_Pawn_Wounded"},
            {"Pawn_Apini_Wounded", "Apini_Pawn_Wounded"},
            {"Pawn_Apini_Death", "Apini_Pawn_Death"},
            {"LostHive", "ApiniScenLostHive"},
            {"NamerbaseHive", "ApiniNamerbaseHive"},
            {"NamerFactionHive", "ApiniNamerFactionHive"},
            {"NamerbaseSwarm", "AzuriNamerbaseSwarm"},
            {"NamerFactionSwarm", "AzuriNamerFactionSwarm"},
            {"NamerPersonAzuri", "AzuriNamerPerson"},
            {"NamerPersonApini", "ApiniNamerPerson"},
            {"HoneyRefining", "ApiniHoneyRefining"},
            {"HoneyConditioning", "ApiniHoneyConditioning"},
            {"Enhance_Leg_Apini", "ApiniEnhance_Leg"},
            {"Enhance_Arm_Growing_Apini", "ApiniEnhance_Arm_Growing"},
            {"Enhance_Arm_Apini", "ApiniEnhance_Arm"},
            {"Enhance_Stomach_Apini", "ApiniEnhance_Stomach"},
            {"Enhance_Heart_Apini", "ApiniEnhance_Heart"},
            {"Install_Wing_Warmer_Apini", "ApiniInstall_Wing_Warmer"},
            {"Install_Wing_Breezer_Apini", "ApiniInstall_Wing_Breezer"},
            {"Install_Wing_Aerator_Apini", "ApiniInstall_Wing_Aerator"},
            {"Install_Proboscis_Enhancement_Apini", "ApiniInstall_Proboscis_Enhancement"},
            {"Install_Crusher_Mandibles_Apini", "ApiniInstall_Crusher_Mandibles"},
            {"Install_Songstress_Mandibles_Apini", "ApiniInstall_Songstress_Mandibles"},
            {"Install_Monarch_Mandibles_Apini", "ApiniInstall_Monarch_Mandibles"},
            {"Enhance_Antenna_Apini", "ApiniEnhance_Antenna"},
            {"Enhance_Eye_Apini", "ApiniEnhance_Eye"},
            {"Enhance_Brain_Apini", "ApiniEnhance_Brain"},
            {"InstallBionicAdvancedAntenna_Apini", "ApiniInstallBionicAdvancedAntenna"},
            {"InstallBionicAntenna_Apini", "ApiniInstallBionicAntenna"},
            {"InstallProstheticAntenna_Apini", "ApiniInstallProstheticAntenna"},
            {"InstallAdvancedBionicWing_Apini", "ApiniInstallAdvancedBionicWing"},
            {"InstallBionicWing_Apini", "ApiniInstallBionicWing"},
            {"InstallProstheticWing_Apini", "ApiniInstallProstheticWing"},
            {"InstallInsectLeg_Apini", "ApiniInstallInsectLeg"},
            {"InstallInsectArm_Apini", "ApiniInstallInsectArm"},
            {"InstallInsectWing_Apini", "ApiniInstallInsectWing"},
            {"InstallHoneyStomach_Apini", "ApiniInstallHoneyStomach"},
            {"InstallInsectMouth_Apini", "ApiniInstallInsectMouth"},
            {"InstallInsectHeart_Apini", "ApiniInstallInsectHeart"},
            {"InstallInsectAntenna_Apini", "ApiniInstallInsectAntenna"},
            {"InstallArchotechEye_Apini", "ApiniInstallArchotechEye"},
            {"InstallBionicEye_Apini", "ApiniInstallBionicEye"},
            {"InstallDenture_Apini", "ApiniInstallDenture"},
            {"InstallPowerClaw_Apini", "ApiniInstallPowerClaw"},
            {"InstallBionicHeart_Apini", "ApiniInstallBionicHeart"},
            {"InstallSimpleProstheticHeart_Apini", "ApiniInstallSimpleProstheticHeart"},
            {"InstallBionicStomach_Apini", "ApiniInstallBionicStomach"},
            {"InstallArchotechArm_Apini", "ApiniInstallArchotechArm_Apini"},
            {"InstallSimpleProstheticArm_Apini", "ApiniInstallSimpleProstheticArm"},
            {"InstallArchotechLeg_Apini", "ApiniInstallArchotechLeg_Apini"},
            {"InstallBionicLeg_Apini", "ApiniInstallBionicLeg"},
            {"InstallSimpleProstheticLeg_Apini", "ApiniInstallSimpleProstheticLeg"},
            {"InstallPegLeg_Apini", "ApiniInstallPegLeg"},
            {"SurgeryAugment", "ApiniSurgeryAugment"},
            {"SurgeryApini", "ApiniSurgery"},
            {"CreateReflexEnhancement_Apini", "ApiniCreateReflexEnhancement"},
            {"CreateGathererFrontLeg_Apini", "ApiniCreateGathererFrontLeg"},
            {"CreateProboscisEnhancement_Apini", "ApiniCreateProboscisEnhancement"},
            {"CreateNeuroenhancer_Apini", "ApiniCreateNeuroenhancer"},
            {"CreateSignalReceptor_Apini", "ApiniCreateSignalReceptor"},
            {"CreateOcularEnhancement_Apini", "ApiniCreateOcularEnhancement"},
            {"CreateRegenerativeEnzymes_Apini", "ApiniCreateRegenerativeEnzymes"},
            {"CreateDigestiveEnzymes_Apini", "ApiniCreateDigestiveEnzymes"},
            {"CreateWarmerImplant_Apini", "ApiniCreateWarmerImplant"},
            {"CreateBreezerImplant_Apini", "ApiniCreateBreezerImplant"},
            {"CreateAeratorImplant_Apini", "ApiniCreateAeratorImplant"},
            {"CreateMonarchMandibles_Apini", "ApiniCreateMonarchMandibles"},
            {"CreateSongstressMandibles_Apini", "ApiniCreateSongstressMandibles"},
            {"CreateCrusherMandibles_Apini", "ApiniCreateCrusherMandibles"},
            {"MakeApitechMechanitesMost", "ApiniMakeApitechMechanitesMost"},
            {"MakeApitechMechanitesMore", "ApiniMakeApitechMechanitesMore"},
            {"MakeApitechMechanites", "ApiniMakeApitechMechanites"},
            {"CreateAdvancedBionicAntenna", "ApiniCreateAdvancedBionicAntenna"},
            {"CreateBionicAntenna", "ApiniCreateBionicAntenna"},
            {"CreateProstheticAntenna", "ApiniCreateProstheticAntenna"},
            {"CreateAdvancedBionicWing", "ApiniCreateAdvancedBionicWing"},
            {"CreateBionicWing", "ApiniCreateBionicWing"},
            {"CreateProstheticWing", "ApiniCreateProstheticWing"},
            {"ApiOrganCraft", "ApitechOrganCraft"},
            {"TakeThingOutOfFermentingVat", "ApiniTakeThingOutOfFermentingVat"},
            {"FillFermentingVat", "ApiniFillFermentingVat"},
            {"EnhancedLeg_Apini", "ApiniEnhancedLeg"},
            {"GathererFrontLeg_Apini", "ApiniGathererFrontLeg"},
            {"EnhancedArm_Growing_Apini", "ApiniEnhancedArm_Growing"},
            {"ReflexEnhancement_Apini", "ApiniReflexEnhancement"},
            {"EnhancedArm_Apini", "ApiniEnhancedArm"},
            {"DigestiveEnzymes_Apini", "ApiniDigestiveEnzymes"},
            {"EnhancedStomach_Apini", "ApiniEnhancedStomach"},
            {"RegenerativeEnzymes_Apini", "ApiniRegenerativeEnzymes"},
            {"EnhancedHeart_Apini", "ApiniEnhancedHeart"},
            {"WingWarmer_Apini", "ApiniWingWarmer"},
            {"EnhancedWing_Warmer_Apini", "ApiniEnhancedWing_Warmer"},
            {"WingBreezer_Apini", "ApiniWingBreezer"},
            {"EnhancedWing_Breezer_Apini", "ApiniEnhancedWing_Breezer"},
            {"WingAerator_Apini", "ApiniWingAerator"},
            {"EnhancedWing_Aerator_Apini", "ApiniEnhancedWing_Aerator"},
            {"ProboscisEnhancement_Apini", "ApiniProboscisEnhancement"},
            {"EnhancedMouth_Eating_Apini", "ApiniEnhancedMouth_Eating"},
            {"SongstressMandibles_Apini", "ApiniSongstressMandibles"},
            {"EnhancedMouth_Songstress_Apini", "ApiniEnhancedMouth_Songstress"},
            {"CrusherMandibles_Apini", "ApiniCrusherMandibles"},
            {"EnhancedMouth_Crusher_Apini", "ApiniEnhancedMouth_Crusher"},
            {"MonarchMandibles_Apini", "ApiniMonarchMandibles"},
            {"SignalReceptor_Apini", "ApiniSignalReceptor"},
            {"EnhancedMouth_Monarch_Apini", "ApiniEnhancedMouth_Monarch"},
            {"EnhancedAntenna_Apini", "ApiniEnhancedAntenna"},
            {"OcularEnhancement_Apini", "ApiniOcularEnhancement"},
            {"EnhancedEye_Apini", "ApiniEnhancedEye"},
            {"Neuroenhancer_Apini", "ApiniNeuroenhancer"},
            {"EnhancedBrain_Apini", "ApiniEnhancedBrain"},
            {"AdvancedBionicAntenna", "ApiniAdvancedBionicAntenna"},
            {"BionicAntenna", "ApiniBionicAntenna"},
            {"ProstheticAntenna", "ApiniProstheticAntenna"},
            {"AdvancedBionicWing", "ApiniAdvancedBionicWing"},
            {"BionicWing", "ApiniBionicWing"},
            {"ProstheticWing", "ApiniProstheticWing"},
            {"InsectSting", "ApiniInsectSting"},
            {"HindLegs", "ApiniHindLegs"},
            {"Thorax", "ApiniInsectThorax"},
            {"Tarsus", "ApiniInsectTarsus"},
            {"InsectLeg", "ApiniInsectLeg"},
            {"Tarsal", "ApiniInsectTarsalClaw"},
            {"Abdomen", "ApiniInsectAbdomen"},
            {"Mandible", "ApiniInsectMandible"},
            {"InsectMouth", "ApiniInsectMouth"},
            {"InsectAntenna", "ApiniInsectAntenna"},
            {"Antenna", "ApiniInsectAntenna"},
            {"InsectHeart", "ApiniInsectHeart"},
            {"HoneyStomach", "ApiniHoneyStomach"},
            {"InsectWings", "ApiniInsectWings"},
            {"InsectWing", "ApiniInsectWing"},
            {"NecropiniMeatHoneyVat", "NpiniMeatHoneyVat"},
            {"NecropiniMeatSlurry", "NpiniMeatSlurry"},
            {"NecropiniMeatHoney", "NpiniMeatHoney"},
            {"NecropiniPawn", "NpiniPawn"},
            {"NecropiniSoldier", "NpiniSoldier"},
            {"NecropiniOverseer", "NpiniOverseer"},
            {"NecropiniTrader", "NpiniTrader"},
            {"NecropiniBasic", "NpiniBasic"},
            {"Necropini_Settings", "Npini_Settings"},
            {"NecropiniMake_Slurry", "NpiniMake_Slurry"},
            {"NecropiniMake_BulkSlurry", "NpiniMake_BulkSlurry"},
            {"BuzzApini", "ApiniBuzz"},
            {"Apini_Outcast", "ApiniOutcast"}
        };
        //End Def Replacement map

        public override bool AppliesToVersion(int majorVer, int minorVer) { return true; }

        public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
        {
            if (defMap.TryGetValue(defName, out string newDef)) { return newDef; }
            return null;
        }

        public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node) { return null; }

        public override void PostExposeData(object obj) { }
    }
}