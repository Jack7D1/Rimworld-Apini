using RimWorld;
using Verse;
using Verse.AI;

namespace Apini
{
    /// <summary>
    /// Workgiver giving out jobs to empty any fermenting vats which need it.
    /// </summary>
    public class WorkGiver_TakeThingOutOfFermenter : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                WorkgiverFermentingVatProperties defProps = def.TryGetModExtension<WorkgiverFermentingVatProperties>();
                if (defProps != null)
                {
                    return ThingRequest.ForDef(defProps.outputThingDef);
                }

                return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
            }
        }

        public JobDef JobDefToUse
        {
            get
            {
                WorkgiverFermentingVatProperties defProps = def.TryGetModExtension<WorkgiverFermentingVatProperties>();
                if (defProps != null)
                {
                    return defProps.jobDef;
                }

                return DefDatabase<JobDef>.GetNamed("TakeThinggOutOfFermentingVat");
            }
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced)
        {
            Building_FermentingVat building_FermentingVat = t as Building_FermentingVat;

            WorkgiverFermentingVatProperties defProps = def.TryGetModExtension<WorkgiverFermentingVatProperties>();
            if (defProps != null)
            {
                if (building_FermentingVat.def != defProps.outputThingDef)
                    return false;
            }

            return building_FermentingVat != null && building_FermentingVat.Fermented && !t.IsBurning() && !t.IsForbidden(pawn) && pawn.CanReserveAndReach(t, PathEndMode.Touch, pawn.NormalMaxDanger(), 1);
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced)
        {
            return JobMaker.MakeJob(JobDefToUse, t);
        }
    }
}