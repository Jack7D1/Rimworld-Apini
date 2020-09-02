using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Apini
{
    public static class ApiniExtensionUtility
    {
        /// <summary>
        /// Tries to get the supplied DefModExtension.
        /// </summary>
        /// <typeparam name="T">ModExtensionType to get.</typeparam>
        /// <param name="def">Supplied Def.</param>
        /// <returns>Desired mod extension if found other null.</returns>
        public static T TryGetModExtension<T>(this Def def) where T : DefModExtension
        {
            if (def.HasModExtension<T>())
                return def.GetModExtension<T>();

            return null;
        }

        /// <summary>
        /// Simple check to determine wheth the PawnKindDef is a Apini or not.
        /// </summary>
        /// <param name="def">Supplied Def.</param>
        /// <returns>True if it is a Apini.</returns>
        public static bool IsApini(this PawnKindDef def)
        {
            return ApiniTracker.apiniKind.Contains(def);
        }
    }
}
