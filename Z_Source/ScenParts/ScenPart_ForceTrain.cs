using RimWorld;
using Verse;

namespace Apini
{
    /// <summary>
    /// This ensures all starting animals are trained in Obedience and Release.
    /// </summary>
    public class ScenPart_ForceTrain : ScenPart_ForcePawn
    {
        public override void ApplyToPawn(Pawn pawn, Map map)
        {
            //Only apply to animals.
            if (pawn.RaceProps.Animal)
            {
                //Train Obedience x 1
                pawn.training.Train(TrainableDefOf.Obedience, null);

                //Train Release x 2
                pawn.training.Train(TrainableDefOf.Release, null);
                pawn.training.Train(TrainableDefOf.Release, null);
            }
        }

        /*public override string Summary(Scenario scen)
        {
            return "ApiniScenPartForceTrainSummary".Translate();
        }*/
    }
}