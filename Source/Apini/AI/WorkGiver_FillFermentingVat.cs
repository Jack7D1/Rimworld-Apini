﻿using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace Apini
{
    /// <summary>
    /// Workgiver giving out jobs to fill any fermenting vats that need it.
    /// </summary>
    class WorkGiver_FillFermentingVat : WorkGiver_Scanner
    {
        private string TemperatureTrans;

        private string NoInputTrans;

        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                WorkgiverFermentingVatProperties defProps = def.TryGetModExtension<WorkgiverFermentingVatProperties>();
                if (defProps != null)
                {
                    return ThingRequest.ForDef(defProps.inputThingDef);
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

                return DefDatabase<JobDef>.GetNamed("FillFermentingVat");
            }
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public void Reset()
        {
            if(TemperatureTrans == null || TemperatureTrans == "" ||
               NoInputTrans == null || NoInputTrans == "")
            {
                {
                    TemperatureTrans = "BadTemperature".Translate().ToLower();
                    NoInputTrans = "NoNectar".Translate();
                }
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
			if (!(t is Building_FermentingVat building_FermentingVat) || building_FermentingVat.Fermented || building_FermentingVat.SpaceLeftForInput <= 0)
			{
				return false;
			}
			float temperature = building_FermentingVat.Position.GetTemperature(building_FermentingVat.Map);
            CompProperties_TemperatureRuinable compProperties = building_FermentingVat.def.GetCompProperties<CompProperties_TemperatureRuinable>();
            if (temperature < compProperties.minSafeTemperature + 2f || temperature > compProperties.maxSafeTemperature - 2f)
            {
                Reset();
                JobFailReason.Is(TemperatureTrans);
                return false;
            }
            if (t.IsForbidden(pawn) || !pawn.CanReserveAndReach(t, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), 1))
            {
                return false;
            }
            if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
            {
                return false;
            }
            if (FindInputThing(pawn, building_FermentingVat) == null)
            {
                Reset();
                JobFailReason.Is(NoInputTrans);
                return false;
            }
            WorkgiverFermentingVatProperties defProps = def.TryGetModExtension<WorkgiverFermentingVatProperties>();
            if (defProps != null)
            {
                if (building_FermentingVat.def != defProps.inputThingDef)
                    return false;
            }

            return !t.IsBurning();
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_FermentingVat building_FermentingVat = (Building_FermentingVat)t;
            Thing t2 = this.FindInputThing(pawn, building_FermentingVat);

            return JobMaker.MakeJob(JobDefToUse, t, t2);
        }

        private Thing FindInputThing(Pawn pawn, Building_FermentingVat barrel)
        {
            Predicate<Thing> validator = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false);
            ThingDef input_def = null;
            VatProperties vatProps = barrel.def.TryGetModExtension<VatProperties>();
            if (vatProps != null)
            {
                input_def = vatProps.inputThingDef;
            }
            //Log.Message("input_def: " + input_def);
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(input_def), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
        }
    }
}