using Verse;

namespace Apini
{
    /// <summary>
    /// Essentially tags a piece of aparrel as belonging to Apini. Also give it extra traits.
    /// </summary>
    public class ApiniAparrelProperties : DefModExtension
    {
        /// <summary>
        /// Give social bonuses to non-Apinis.
        /// </summary>
        public bool isCute = false;
    }
}