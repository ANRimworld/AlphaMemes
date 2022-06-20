using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace FuneralFramework
{
    public class RitualOutcomeComp_FuneralFramework : RitualOutcomeComp
    {
        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        //spawning properties of the outcome
        public ThingDef thingDefToSpawn = null;
        public int thingCount = 0;
        public float bestOutcomeMulti = 1;
        public float worstOutcomeMulti = 1;
        public bool stripcorpse = false;


    }
}
