using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace Apini
{
    [StaticConstructorOnStartup]
    public class Building_FermentingVat : Building
    {
        public int MaxCapacity = 25;

        public float FermentationModifier = 1.0f;

        private int thingCount;

        private float progressInt;

        private Material barFilledCachedMat;

        private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

        private static readonly Color BarZeroProgressColor = new Color(0.8f, 0.8f, 0.8f);

        private static readonly Color BarFermentedColor = new Color(0.2f, 0.90f, 0.2f);

        private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));

        public override void PostMake()
        {
            base.PostMake();

            SetupValues();
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            SetupValues();
        }

        public void SetupValues()
        {
            VatProperties vatProps = def.TryGetModExtension<VatProperties>();
            if (vatProps != null)
            {
                MaxCapacity = vatProps.maxCapacity;
                FermentationModifier = vatProps.fermentationModifier;
            }
        }

        public float Progress
        {
            get => progressInt;
            set
            {
                if (value == progressInt)
                {
                    return;
                }
                progressInt = value;
                barFilledCachedMat = null;
            }
        }

        private Material BarFilledMat
        {
            get
            {
                if (barFilledCachedMat == null)
                {
                    barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(BarZeroProgressColor, BarFermentedColor, Progress));
                }
                return barFilledCachedMat;
            }
        }

        private float Temperature
        {
            get
            {
                if (base.MapHeld == null)
                {
                    Log.ErrorOnce("Tried to get a fermenting barrel temperature but MapHeld is null.", 847163513);
                    return 7f;
                }
                return base.PositionHeld.GetTemperature(base.MapHeld);
            }
        }

        public int SpaceLeftForInput
        {
            get
            {
                if (Fermented)
                {
                    return 0;
                }
                return MaxCapacity - thingCount;
            }
        }

        private bool Empty => thingCount <= 0;
        public bool Fermented => !Empty && Progress >= 1f;

        private float CurrentTempProgressSpeedFactor
        {
            get
            {
                CompProperties_TemperatureRuinable compProperties = def.GetCompProperties<CompProperties_TemperatureRuinable>();
                float temperature = Temperature;
                if (temperature < compProperties.minSafeTemperature)
                {
                    return 0.1f;
                }
                if (temperature < 7f)
                {
                    return GenMath.LerpDouble(compProperties.minSafeTemperature, 7f, 0.1f, 1f, temperature);
                }
                return 1f;
            }
        }

        private float ProgressPerTickAtCurrentTemp
        {
            get
            {
                return (1.66666666E-06f * CurrentTempProgressSpeedFactor) * FermentationModifier;
            }
        }

        private int EstimatedTicksLeft
        {
            get
            {
                return Mathf.Max(Mathf.RoundToInt((1f - Progress) / ProgressPerTickAtCurrentTemp), 0);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref thingCount, "thingCount", 0, false);
            Scribe_Values.Look<float>(ref progressInt, "progress", 0f, false);
        }

        public override void TickRare()
        {
            base.TickRare();
            if (!Empty)
            {
                Progress = Mathf.Min(Progress + 250f * ProgressPerTickAtCurrentTemp, 1f);
            }
        }

        public void AddInput(int count)
        {
            base.GetComp<CompTemperatureRuinable>().Reset();
            int num = Mathf.Min(count, MaxCapacity - thingCount);
            if (num <= 0)
            {
                return;
            }
            Progress = GenMath.WeightedAverage(0f, (float)num, Progress, (float)thingCount);
            thingCount += num;
        }

        public void AddInput(Thing input)
        {
            CompTemperatureRuinable comp = base.GetComp<CompTemperatureRuinable>();
            if (comp.Ruined)
            {
                comp.Reset();
            }
            int num = Mathf.Min(input.stackCount, MaxCapacity - thingCount);
            if (num > 0)
            {
                AddInput(num);
                input.SplitOff(num).Destroy(DestroyMode.Vanish);
            }
        }

        protected override void ReceiveCompSignal(string signal)
        {
            if (signal == "RuinedByTemperature")
            {
                Reset();
            }
        }

        private void Reset()
        {
            thingCount = 0;
            Progress = 0f;
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (stringBuilder.Length != 0)
            {
                stringBuilder.AppendLine();
            }
            CompTemperatureRuinable comp = base.GetComp<CompTemperatureRuinable>();
            VatProperties vatProps = def.TryGetModExtension<VatProperties>();
            if (!Empty)
            {
                if (!comp.Ruined)
                {
                    if (Fermented)
                    {
                        stringBuilder.AppendLine(vatProps.containsOutputTranslation.Translate(thingCount, MaxCapacity));
                    }
                    else
                    {
                        stringBuilder.AppendLine(vatProps.containsInputTranslation.Translate(thingCount, MaxCapacity));
                    }
                }
                if (Fermented)
                {
                    stringBuilder.AppendLine(vatProps.fermentedTranslation.Translate());
                }
                else
                {
                    stringBuilder.AppendLine(vatProps.fermentationProgressTranslation.Translate(Progress.ToStringPercent(), EstimatedTicksLeft.ToStringTicksToPeriod(true, false, true, true)));
                    if (CurrentTempProgressSpeedFactor != 1f)
                    {
                        stringBuilder.AppendLine(vatProps.fermentationNonIdealTranslation);
                    }
                }
            }
            if (base.MapHeld != null)
            {
                stringBuilder.AppendLine("Temperature".Translate() + ": " + Temperature.ToStringTemperature("F0"));
            }
            stringBuilder.AppendLine("IdealFermentingTemperature".Translate() + ": " + 7f.ToStringTemperature("F0") + " ~ " + comp.Props.maxSafeTemperature.ToStringTemperature("F0"));
            return stringBuilder.ToString().TrimEndNewlines();
        }

        public Thing TakeOutThing()
        {
            if (!Fermented)
            {
                Log.Warning("Tried to get beer but it's not yet fermented.");
                return null;
            }

            int stack_count_modifier = 1;

            VatProperties vatProps = def.TryGetModExtension<VatProperties>();

            Thing thing;
            if (vatProps != null)
            {
                thing = ThingMaker.MakeThing(vatProps.outputThingDef, null);
                stack_count_modifier = vatProps.inputToOutputRatio;
            }
            else
            {
                thing = ThingMaker.MakeThing(ThingDefOf.Beer, null);
            }

            thing.stackCount = thingCount / stack_count_modifier;
            Reset();
            return thing;
        }

        public override void Draw()
        {
            base.Draw();
            if (!Empty)
            {
                Vector3 drawPos = DrawPos;
                drawPos.y += 0.05f;
                drawPos.z += 0.25f;
                GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
                {
                    center = drawPos,
                    size = Building_FermentingVat.BarSize,
                    fillPercent = (float)thingCount / (float)MaxCapacity,
                    filledMat = BarFilledMat,
                    unfilledMat = Building_FermentingVat.BarUnfilledMat,
                    margin = 0.1f,
                    rotation = Rot4.North
                });
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            List<Gizmo> gizmos = new List<Gizmo>(base.GetGizmos());

            if (DebugSettings.godMode)
            {
                {
                    Command_Action debug_action = new Command_Action
                    {
                        defaultLabel = "Set progress to 1",
                        defaultDesc = "Finish fermenting.",
                        action = delegate ()
                        {
                            progressInt = 1.0f;
                        }
                    };

                    gizmos.Add(debug_action);
                }
            }

            return gizmos;
        }
    }
}