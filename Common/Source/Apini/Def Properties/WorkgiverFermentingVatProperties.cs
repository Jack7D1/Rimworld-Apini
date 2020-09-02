using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Apini
{
    /// <summary>
    /// Properties for workgivers with fermenting vats.
    /// </summary>
    public class WorkgiverFermentingVatProperties : DefModExtension
    {
        /// <summary>
        /// Expected thing to input into the vat.
        /// </summary>
        public ThingDef inputThingDef;
        /// <summary>
        /// Expected output from the vat.
        /// </summary>
        public ThingDef outputThingDef;
        /// <summary>
        /// JobDef to use in performing the job.
        /// </summary>
        public JobDef jobDef;

        public WorkgiverFermentingVatProperties()
        {

        }
    }
}