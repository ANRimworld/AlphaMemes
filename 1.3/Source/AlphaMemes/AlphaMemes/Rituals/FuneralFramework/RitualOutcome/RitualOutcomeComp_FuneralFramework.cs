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
        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        //spawning properties of the outcome
        

        public bool stripcorpse = false;
        public string roleToSpawnOn = "FF_RitualRoleCorpse";
        public string bestOutcomeDesc = null;
        public string worstOutcomeDesc = null;
        public List<RitualOutcome_FuneralFramework_ThingToSpawn> outcomeSpawners;



    }
}
