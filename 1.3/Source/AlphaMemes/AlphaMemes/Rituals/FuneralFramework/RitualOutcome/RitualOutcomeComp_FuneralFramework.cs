using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_FuneralFramework : RitualOutcomeComp_Quality
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return new RitualOutcomeComp_DataFuneralFramework();
        }
        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            return 0f;
        }
        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        //spawning properties of the outcome
        public void fillData(RitualOutcomeComp_Data comp, Precept_Ritual ritual)
        {
            if (!ritual.def.HasModExtension<FuneralPreceptExtension>())
            {
                return;
            }
            RitualOutcomeComp_DataFuneralFramework data = (RitualOutcomeComp_DataFuneralFramework)comp;
            data.outcomeSpawners = ritual.def.GetModExtension<FuneralPreceptExtension>().spawners;
            data.ritualdef = ritual.def;

        }

        




    }
}
