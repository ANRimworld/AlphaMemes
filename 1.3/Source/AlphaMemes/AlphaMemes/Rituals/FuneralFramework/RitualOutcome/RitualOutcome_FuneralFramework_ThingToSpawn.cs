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
        public ThingDef stuffToUse = null;  //Dont have a way to select yet if ever (would need to create custom ritul start UI) use this for specific def
        public ThingDef stuffDefToSpawn = null;
        public int thingCount = 0;
        public int stuffCount = 0;
        public float bestOutcomeMulti = 1;
        public float worstOutcomeMulti = 1;
        public List<ThingDef> stuffOptions = new List<ThingDef>(); //If I solve above this will be better
        public List<StuffCategoryDef> stuffCategoryDefs;

        public virtual ThingDef FindStuffForThing(RitualObligation obligation,bool recheck)
        {//
            if(stuffToUse != null)
            {

                stuffDefToSpawn = stuffToUse;                
                return stuffDefToSpawn;
            }
            if(stuffOptions.Count>0 && !recheck)//Obligation filters get called a lot so dont check resource list unless we have to
            {                
                return stuffDefToSpawn;
            }
            stuffOptions.Clear();

            foreach (KeyValuePair<ThingDef, int> allCountedAmount in Find.CurrentMap.resourceCounter.AllCountedAmounts) 
            {
                if (allCountedAmount.Key.IsStuff && stuffCategoryDefs.Any(x=> allCountedAmount.Key.stuffProps.categories.Contains(x)) && allCountedAmount.Value >= stuffCount)
                {
                    stuffOptions.Add(allCountedAmount.Key);
                }
            }
            stuffDefToSpawn = stuffOptions.RandomElement();
            return stuffDefToSpawn;
        }

        public virtual Thing GetThingToSpawn(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome,Pawn pawnToSpawnOn, Corpse corpse)
        {
            //This shit is wonky and I have no idea if it'll work. But itd be kinda neat if it did.
            //Though I should find a better way            

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
