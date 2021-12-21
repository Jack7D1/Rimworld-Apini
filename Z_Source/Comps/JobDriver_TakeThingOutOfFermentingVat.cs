using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Apini
{
    /// <summary>
    /// Job driver for emptying a fermenting vat.
    /// </summary>
    public class JobDriver_TakeThingOutOfFermentingVat : JobDriver
    {
        private const TargetIndex VatInd = TargetIndex.A;

        private const TargetIndex ThingToHaulInd = TargetIndex.B;

        private const TargetIndex StorageCellInd = TargetIndex.C;

        private const int Duration = 200;

        protected Building_FermentingVat Vat
        {
            get
            {
                return (Building_FermentingVat)base.job.GetTarget(TargetIndex.A).Thing;
            }
        }

        protected Thing VatProduct
        {
            get
            {
                return base.job.GetTarget(TargetIndex.B).Thing;
            }
        }

		public override bool Equals(object obj)
		{
			return obj is JobDriver_TakeThingOutOfFermentingVat vat &&
				   EqualityComparer<Building_FermentingVat>.Default.Equals(Vat, vat.Vat) &&
				   EqualityComparer<Thing>.Default.Equals(VatProduct, vat.VatProduct);
		}

		public override int GetHashCode()
		{
			var hashCode = -452190273;
			hashCode = hashCode * -1521134295 + EqualityComparer<Building_FermentingVat>.Default.GetHashCode(Vat);
			hashCode = hashCode * -1521134295 + EqualityComparer<Thing>.Default.GetHashCode(VatProduct);
			return hashCode;
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.TargetA, this.job, 1, 0, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Reserve.Reserve(TargetIndex.A, 1);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOn(() => !Vat.Fermented).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            yield return new Toil
            {
                initAction = delegate
                {
                    Thing thing = Vat.TakeOutThing();
                    GenPlace.TryPlaceThing(thing, pawn.Position, Map, ThingPlaceMode.Near, null);
					StoragePriority currentPriority = StoreUtility.StoragePriorityAtFor(thing.Position, thing); ;
					if (StoreUtility.TryFindBestBetterStoreCellFor(thing, pawn, Map, currentPriority, pawn.Faction, out IntVec3 c, true))
					{
						job.SetTarget(TargetIndex.C, c);
						job.SetTarget(TargetIndex.B, thing);
						job.count = thing.stackCount;
					}
					else
					{
						EndJobWith(JobCondition.Incompletable);
					}
				},
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return Toils_Reserve.Reserve(TargetIndex.B, 1);
            yield return Toils_Reserve.Reserve(TargetIndex.C, 1);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false);
            Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
            yield return carryToCell;
            yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
        }
    }
}
