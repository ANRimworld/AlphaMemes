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
        public ThingDef stuffDefToSpawn = null;
        public int thingCount = 0;
        public float bestOutcomeMulti = 1;
        public float worstOutcomeMulti = 1;
        public bool stripcorpse = false;
        public string roleToSpawnOn = "FF_RitualRoleCorpse";
        public string extraDefDescription = null;
        public string extraDefLabel = null;

        public void ExtraDescription(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, Thing thingToSpawn)
        {
            //bad idea def isnt unique to instace of thing. Which makes sense but yeah sad face.
            //Think I need to make it art to do what I want to do here
            //Leaving alone for now
/*            if(extraDefLabel!= null)
            {
                thingToSpawn.def.label = extraDefLabel.Formatted(pawn.Name.Named("PAWN"), corpse.InnerPawn.Name.Named("CORPSE"), thingToSpawn.def.label.Named("DEFLABEL"), thingToSpawn.def.description.Named("DEFDESC"));
            }
            if(extraDefDescription != null)
            {
                thingToSpawn.def.description = extraDefDescription.Formatted(pawn.Name.Named("PAWN"), corpse.InnerPawn.Name.Named("CORPSE"), thingToSpawn.def.label.Named("DEFLABEL"), thingToSpawn.def.description.Named("DEFDESC"));
            }*/
        }

    }
}
