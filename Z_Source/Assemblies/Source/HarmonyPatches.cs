using HarmonyLib;
using System.Reflection;
using Verse;

namespace Apini
{
    [StaticConstructorOnStartup]
    class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("Apini");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    /*[HarmonyPatch(typeof(JobDriver_PlantWork))]
    [HarmonyPatch("MakeNewToils")]
    [HarmonyPatch(new Type[] {})]
    static class _Patch
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> TranspilerMethod(JobDriver_PlantWork __instance, IEnumerable<Toil> __result, MethodBase original, IEnumerable<CodeInstruction> instructions, ILGenerator ilgenerator)
        {
            //
            //Original
            //
            List<CodeInstruction> iloriginal = new List<CodeInstruction>(instructions);



            //
            //Override
            //

            return iloriginal;
        }
    }*/
}
