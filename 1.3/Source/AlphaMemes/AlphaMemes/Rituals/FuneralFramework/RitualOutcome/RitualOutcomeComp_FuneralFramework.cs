using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_FuneralFramework : RitualOutcomeComp
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return new RitualOutcomeComp_DataFuneralFramework();
        }
        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        //spawning properties of the outcome
        public void fillData(RitualOutcomeComp_Data comp)
        {
            RitualOutcomeComp_DataFuneralFramework data = (RitualOutcomeComp_DataFuneralFramework)comp;
            data.outcomeSpawners = outcomeSpawners;
        }

        public bool stripcorpse = false;
        public string roleToSpawnOn = "FF_RitualRoleCorpse";
        public string bestOutcomeDesc = null;
        public string worstOutcomeDesc = null;
        public List<RitualOutcome_FuneralFramework_ThingToSpawn> outcomeSpawners;



    }
}
