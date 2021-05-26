using RimWorld;
using System.Linq;
using Verse;

namespace Apini
{
    /// <summary>
    /// Abstract Scenario Part for forcing stuff on pawns.
    /// </summary>
    public abstract class ScenPart_ForcePawn : ScenPart
    {
        /// <summary>
        /// Action to apply to each pawn.
        /// </summary>
        /// <param name="pawn">Pawn to apply to.</param>
        public abstract void ApplyToPawn(Pawn pawn, Map map);

        /// <summary>
        /// Apply scenario part post map generation.
        /// </summary>
        /// <param name="map">Supplied map.</param>
        public override void PostMapGenerate(Map map)
        {
            //Iterate through starting pawns.
            foreach (Pawn pawn in map.mapPawns.PawnsInFaction(Faction.OfPlayer))
            {
                ApplyToPawn(pawn, map);
            }

            //Iterate through incoming drop pods.
            foreach (Thing pod in map.listerThings.ThingsInGroup(ThingRequestGroup.ActiveDropPod))
            {
                if (pod is DropPodIncoming dropPod)
                {
                    foreach (Thing thing in dropPod.Contents.innerContainer.AsEnumerable())
                    {
                        if (thing is Pawn pawn)
                        {
                            ApplyToPawn(pawn, map);
                        }
                    }
                }
            }
        }
    }
}