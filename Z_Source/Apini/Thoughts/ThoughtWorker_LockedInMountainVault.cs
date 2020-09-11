using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Apini
{
    /// <summary>
    /// This thought causes discomfort to Apini who are under natural roof for too long.
    /// </summary>
    public class ThoughtWorker_LockedInMountainVault : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            /*PawnKindDef apiniKindDef = ApiniDefOf.ApiniPlayer;
            if (apiniKindDef == null)
            {
                return ThoughtState.Inactive;
            }*/
            if(!p.kindDef.IsApini())
            {
                return ThoughtState.Inactive;
            }
            if (p.Downed)
            {
                return ThoughtState.Inactive;
            }
            if (p.HostFaction != null)
            {
                return ThoughtState.Inactive;
            }
            if(!p.Spawned)
            {
                return ThoughtState.Inactive;
            }
            if(SurroundingRoofIsMountainPercentage(p) < 0.6f)
            {
                return ThoughtState.Inactive;
            }
            float num = (float)p.needs.mood.recentMemory.TicksSinceOutdoors / 60000f;
            if (num < 0.5f)
            {
                return ThoughtState.Inactive;
            }
            if (num < 1.5f)
            {

                return ThoughtState.ActiveAtStage(0);
            }
            return ThoughtState.ActiveAtStage(1);
        }

        public float SurroundingRoofIsMountainPercentage(Pawn p)
        {
            float tiles_mountain = 0;
            Map map = p.Map;

            for (int z = -1; z <= 1; z++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    IntVec3 roof_at = new IntVec3(p.Position.x + x, p.Position.y, p.Position.z + z);
                    if (roof_at.InBounds(map) && map.roofGrid.RoofAt(roof_at) == RoofDefOf.RoofRockThick)
                        tiles_mountain += 1f;
                }
            }

            return tiles_mountain / 9f;
        }
    }
}
