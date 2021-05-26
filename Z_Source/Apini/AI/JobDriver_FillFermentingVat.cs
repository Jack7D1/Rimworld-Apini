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
        protected Building_FermentingVat Vat
		{
			get
			{
				return (Building_FermentingVat)job.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Thing InputThing
		{
			get
			{
				return job.GetTarget(TargetIndex.B).Thing;
			}
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Vat, job, 1, -1, null, errorOnFailed) && pawn.Reserve(InputThing, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
            AddEndCondition(delegate
            {
                if (Vat.SpaceLeftForInput > 0)
                    return JobCondition.Ongoing;
                return JobCondition.Succeeded;
            }
            );
            yield return Toils_General.DoAtomic(delegate
            {
				//Log.Message("this.Vat.SpaceLeftForInput returns: " + this.Vat.SpaceLeftForInput);
				if(Vat.SpaceLeftForInput > InputThing.stackCount)
                {
					job.count = InputThing.stackCount;
                }
				else job.count = Vat.SpaceLeftForInput;
            });
			Toil reserveThing = Toils_Reserve.Reserve(TargetIndex.B, 1);
			yield return reserveThing;
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveThing, TargetIndex.B, TargetIndex.None, false, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(200, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate
				{
					Vat.AddInput(InputThing);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
            yield break;
        }
	}
}