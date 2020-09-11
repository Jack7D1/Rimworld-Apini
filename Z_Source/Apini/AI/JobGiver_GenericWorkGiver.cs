using System;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Apini
{
    /// <summary>
    /// A jobgiver that is so generic it can give out any one job. Meant for use in ThinkTree nodes.
    /// </summary>
    public class JobGiver_GenericWorkGiver : ThinkNode
    {
        //public bool emergency;
        public WorkGiverDef workGiverDef;

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_GenericWorkGiver jobGiver_GenericWorkGiver = (JobGiver_GenericWorkGiver)base.DeepCopy(resolve);
            jobGiver_GenericWorkGiver.workGiverDef = workGiverDef;
            return jobGiver_GenericWorkGiver;
        }

        public override float GetPriority(Pawn pawn)
        {
            return 9f;
        }

        public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
        {
            int num = -999;
            TargetInfo targetInfo = TargetInfo.Invalid;
            WorkGiver_Scanner workGiver_Scanner = null;
            if (workGiverDef != null)
            {
                WorkGiver workGiver = workGiverDef.Worker;
                if (workGiver.def.priorityInType != num && targetInfo.IsValid)
                {
                    return ThinkResult.NoJob;
                }

                if (this.PawnCanUseWorkGiver(pawn, workGiver))
                {
                    try
                    {
                        Job job2 = workGiver.NonScanJob(pawn);
                        if (job2 != null)
                        {
                            return new ThinkResult(job2, this, new JobTag?(workGiverDef.tagToGive));
                        }
                        WorkGiver_Scanner scanner = workGiver as WorkGiver_Scanner;
                        if (scanner != null)
                        {
                            if (workGiver.def.scanThings)
                            {
                                Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn) && scanner.HasJobOnThing(pawn, t, false);
                                IEnumerable<Thing> enumerable = scanner.PotentialWorkThingsGlobal(pawn);
                                Thing thing;
                                if (scanner.Prioritized)
                                {
                                    IEnumerable<Thing> enumerable2 = enumerable;
                                    if (enumerable2 == null)
                                    {
                                        enumerable2 = pawn.Map.listerThings.ThingsMatching(scanner.PotentialWorkThingRequest);
                                    }
                                    Predicate<Thing> validator = predicate;
                                    thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, enumerable2, scanner.PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, (Thing x) => scanner.GetPriority(pawn, x));
                                }
                                else
                                {
                                    Predicate<Thing> validator = predicate;
                                    bool forceGlobalSearch = enumerable != null;
                                    thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, scanner.PotentialWorkThingRequest, scanner.PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, enumerable, 0, scanner.MaxRegionsToScanBeforeGlobalSearch, forceGlobalSearch, RegionType.Set_Passable, false);
                                }
                                if (thing != null)
                                {
                                    targetInfo = thing;
                                    workGiver_Scanner = scanner;
                                }
                            }
                            if (workGiver.def.scanCells)
                            {
                                IntVec3 position = pawn.Position;
                                float num2 = 99999f;
                                float num3 = -3.40282347E+38f;
                                bool prioritized = scanner.Prioritized;
                                foreach (IntVec3 current in scanner.PotentialWorkCellsGlobal(pawn))
                                {
                                    bool flag = false;
                                    float num4 = (float)(current - position).LengthHorizontalSquared;
                                    if (prioritized)
                                    {
                                        if (!current.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, current))
                                        {
                                            float priority = scanner.GetPriority(pawn, current);
                                            if (priority > num3 || (priority == num3 && num4 < num2))
                                            {
                                                flag = true;
                                                num3 = priority;
                                            }
                                        }
                                    }
                                    else if (num4 < num2 && !current.IsForbidden(pawn) && scanner.HasJobOnCell(pawn, current))
                                    {
                                        flag = true;
                                    }
                                    if (flag)
                                    {
                                        targetInfo = new TargetInfo(current, pawn.Map, false);
                                        workGiver_Scanner = scanner;
                                        num2 = num4;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Concat(new object[]
                        {
                    pawn,
                    " threw exception in WorkGiver ",
                    workGiver.def.defName,
                    ": ",
                    ex.ToString()
                        }));
                    }
                    finally
                    {
                    }
                    if (targetInfo.IsValid)
                    {
                        //pawn.mindState.lastGivenWorkType = workGiver.def.workType;
                        Job job3;
                        if (targetInfo.HasThing)
                        {
                            job3 = workGiver_Scanner.JobOnThing(pawn, targetInfo.Thing, false);
                        }
                        else
                        {
                            job3 = workGiver_Scanner.JobOnCell(pawn, targetInfo.Cell);
                        }
                        if (job3 != null)
                        {
                            return new ThinkResult(job3, this, new JobTag?(workGiverDef.tagToGive));
                        }
                        Log.ErrorOnce(string.Concat(new object[]
                        {
                    workGiver_Scanner,
                    " provided target ",
                    targetInfo,
                    " but yielded no actual job for pawn ",
                    pawn,
                    ". The CanGiveJob and JobOnX methods may not be synchronized."
                        }), 6112651);
                    }
                    num = workGiver.def.priorityInType;
                }
            }

            return ThinkResult.NoJob;
        }

        private bool PawnCanUseWorkGiver(Pawn pawn, WorkGiver giver)
        {
            return giver.MissingRequiredCapacity(pawn) == null && !giver.ShouldSkip(pawn);
        }
    }
}