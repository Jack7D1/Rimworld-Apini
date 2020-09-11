using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Apini
{
    /// <summary>
    /// This thought cause happiness to Apinis wearing blue hued apparel.
    /// </summary>
    public class ThoughtWorker_WearingBlueApparel : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            //PawnKindDef apiniKindDef = ApiniDefOf.ApiniPlayer;
            /*if (apiniKindDef == null)
            {
                return ThoughtState.Inactive;
            }*/
            if(!p.kindDef.IsApini())
            {
                return ThoughtState.Inactive;
            }
            if (p.HostFaction != null)
            {
                return ThoughtState.Inactive;
            }
            if(!PawnIsWearingBlueApparel(p))
            {
                return ThoughtState.Inactive;
            }

            return ThoughtState.ActiveAtStage(0);
        }

        /// <summary>
        /// Checks whether a pawn wears blue apparel.
        /// </summary>
        /// <param name="p">Pawn to check.</param>
        /// <returns>True if a blue hued apparel was found. False if not.</returns>
        public bool PawnIsWearingBlueApparel(Pawn p)
        {
            foreach(Apparel apparel in p.apparel.WornApparel)
            {
                float H = 0f;
                float S = 0f;
                float V = 0f;

                if(apparel.DrawColor != null)
                    Color.RGBToHSV(apparel.DrawColor, out H, out S, out V);
                else if(apparel.Stuff != null && apparel.Stuff.stuffProps != null)
                    Color.RGBToHSV(apparel.Stuff.stuffProps.color, out H, out S, out V);

                //0 to 240 scale.
                //0.0 to 1.0
                if (H >= (120f / 240f) && H <= (160f / 240f) &&
                    S >= (20f / 240f) && S <= (240f / 240f) &&
                    V >= (50f / 240f) && V <= (230f / 240f))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
