using Verse;

namespace Apini
{
    /// <summary>
    /// The fermenting vat properties.
    /// </summary>
    public class VatProperties : DefModExtension
    {
        /// <summary>
        /// Thing that go in to ferment.
        /// </summary>
        public ThingDef inputThingDef;
        /// <summary>
        /// Thing that go out on successful fermentation.
        /// </summary>
        public ThingDef outputThingDef;
        /// <summary>
        /// Maximum capacity of inputThingDef in the vat.
        /// </summary>
        public int maxCapacity = 25;
        /// <summary>
        /// Modifies the default fermentation time.
        /// </summary>
        public float fermentationModifier = 1.0f;
        /// <summary>
        /// How much input stuff is needed to make one output stuff.
        /// </summary>
        public int inputToOutputRatio = 1;

        //Translations
        public string containsInputTranslation = "ContainsNectar";
        public string containsOutputTranslation = "ContainsHoney";
        public string fermentedTranslation = "Fermented";
        public string fermentationProgressTranslation = "FermentationProgress";
        public string fermentationNonIdealTranslation = "FermentationBarrelOutOfIdealTemperature";

        public VatProperties()
        {

        }
    }
}