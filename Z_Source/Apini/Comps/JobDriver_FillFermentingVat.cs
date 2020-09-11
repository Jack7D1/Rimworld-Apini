using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Apini
{
	/// <summary>
	/// Job driver for filling a fermenting vat.
	/// </summary>
	public class JobDriver_FillFermentingVat : JobDriver
	{
		private const TargetIndex VatInd = TargetIndex.A;

		private const TargetIndex InputInd = TargetIndex.B;

		private const int Duration = 200;

		protected Building_FermentingVat Vat
		{
			get
			{
				return (Building_FermentingVat)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.TargetA, this.job, 1, 0, null, errorOnFailed);
		}

		protected Thing InputThing
		{
			get
			{
				return base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1);
			Toil reserveThing = Toils_Reserve.Reserve(TargetIndex.B, 1);
			yield return reserveThing;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveThing, TargetIndex.B, TargetIndex.None, false, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate
				{
					Vat.AddInput(InputThing);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}