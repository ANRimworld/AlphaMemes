using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{   //Using this for thingdef now
    //Allows for multiple things to be spawned, and ability to create comps to do different things easier
    public class RitualOutcome_FuneralFramework_ThingToSpawn
    {

        //spawning properties of the outcome
        public ThingDef thingDefToSpawn = null;
        public ThingDef stuffDefToSpawn = null;
        public int thingCount = 0;
        public float bestOutcomeMulti = 1;
        public float worstOutcomeMulti = 1;
        

        public virtual Thing GetThingToSpawn(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome,Pawn pawnToSpawnOn, Corpse corpse)
        {
            //This shit is wonky and I have no idea if it'll work. But itd be kinda neat if it did.
            //Though I should find a better way            
            if (ritual.Ritual.obligationTargetFilter.def.HasModExtension<FuneralFramework_ObligationTargetExtension>())
            {
                foreach (RitualObligationTargetFilter filter in ritual.Ritual.obligationTargetFilter.def.GetModExtension<FuneralFramework_ObligationTargetExtension>().filters)
                {
                    if(filter is RitualObligationTargetWorker_HaveRequiredStuff) //definetly want a better way here. Might need to rethink how extension.filters is setup seperate mod extension for spawning stuff on individual def?
                    {
                        RitualObligationTargetWorker_HaveRequiredStuff stuffworker = filter as RitualObligationTargetWorker_HaveRequiredStuff;
                        if (!stuffworker.canSpawn)
                        {
                            return null;
                        }
                        stuffDefToSpawn = stuffworker.stuffToUse;
                        break;
                    }
                }
            }
            ThingDef thingDef = CustomThingDefLogic(ritual,pawnToSpawnOn,corpse);
            thingDef = thingDef == null ? thingDefToSpawn : thingDef;
            if (thingDef == null)
            {
                return null;               
            }            
            int stacksize = thingCount;
            ThingDef stuff = stuffDefToSpawn;
            if (thingDef.MadeFromStuff ? !GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined).Contains(stuff) :false)
            {
                stuff = GenStuff.DefaultStuffFor(thingDefToSpawn);
            }
            Thing thingToSpawn = ThingMaker.MakeThing(thingDef, stuff);
            initComps(thingToSpawn, corpse);
            if (thingToSpawn is Building)
            {
                thingToSpawn = thingToSpawn.MakeMinified();
            }
            if (worstOutcome)
            {
                stacksize = (int)Math.Round(stacksize * worstOutcomeMulti);
            }
            if (bestOutcome)
            {
                stacksize = (int)Math.Round(stacksize * bestOutcomeMulti);
            }
            if (stacksize <= 0)
            {
                return null;
            }
            thingToSpawn.stackCount = stacksize;
            
            return thingToSpawn;
        }
        public virtual ThingDef CustomThingDefLogic(LordJob_Ritual ritual, Pawn pawnToSpawnOn, Corpse corpse)
        {
            return null;
        }

        public virtual void initComps(Thing thing,Corpse corpse)//Probably going to need more
        {
            Comp_CorpseContainer comp = thing.TryGetComp<Comp_CorpseContainer>();
            if(comp != null)
            {
                comp.InitComp_CorpseContainer(corpse);
            }
                

        }
    }
}
