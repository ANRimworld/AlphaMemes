using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_DataFuneralFramework : RitualOutcomeComp_Data
    {

        //spawning properties of the outcome        
        public PreceptDef ritualdef;
        public List<FuneralFramework_ThingToSpawn> outcomeSpawners;
        //public bool stuffProvided;

        public override void ExposeData()
        {
            base.ExposeData();                        
            Scribe_Defs.Look(ref ritualdef, "ritual");
            if(Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                outcomeSpawners = ritualdef.GetModExtension<FuneralPreceptExtension>().spawners;
            }
        }

    }
}
