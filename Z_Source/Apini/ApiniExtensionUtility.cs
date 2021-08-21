using Verse;

namespace Apini
{
    public static class ApiniExtensionUtility
    {
        /// <summary>
        /// Simple check to determine wheth the PawnKindDef is a Apini or not.
        /// </summary>
        /// <param name="def">Supplied Def.</param>
        /// <returns>True if it is a Apini.</returns>
        public static bool IsApini(this PawnKindDef def)
        {
            return ApiniTracker.pawnkinds.Contains(def);
        }

        /// <summary>
        /// Tries to get the supplied DefModExtension.
        /// </summary>
        /// <typeparam name="T">ModExtensionType to get.</typeparam>
        /// <param name="def">Supplied Def.</param>
        /// <returns>Desired mod extension if found other null.</returns>
        public static T TryGetModExtension<T>(this Def def) where T : DefModExtension
        {
            return def.HasModExtension<T>() ? def.GetModExtension<T>() : null;
        }
    }
}