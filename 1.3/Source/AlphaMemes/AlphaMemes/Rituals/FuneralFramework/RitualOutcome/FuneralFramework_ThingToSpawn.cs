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
    public class FuneralFramework_ThingToSpawn
    {

        //spawning properties of the outcome

        public ThingDef thingDefToSpawn = null;
        public ThingDef stuffToUse = null;  //
        public ThingDef stuffDefToSpawn = null;
        public int thingCount = 0;
        public int stuffCount = 0;
        public float bestOutcomeMulti = 1;
        public float worstOutcomeMulti = 1;
        public List<ThingDef> stuffOptions = new List<ThingDef>(); 
        public List<StuffCategoryDef> stuffCategoryDefs;
        public Dictionary<ThingDef, int> thingsRequired = new Dictionary<ThingDef, int>();//For specific things needed to start a ritual
     
        
        public virtual bool CanStartStuff(bool recheck = true)
        {
            Map map = Find.CurrentMap;
            if (stuffDefToSpawn != null && recheck)
            {
                recheck = map.resourceCounter.GetCount(stuffDefToSpawn) <= stuffCount;
                if (!recheck)
                {
                    recheck = !stuffOptions.Any(x => map.resourceCounter.GetCount(x) >= stuffCount);
                }              
            }


            if (recheck)
            {
                if(stuffCategoryDefs.Count>0 || stuffToUse != null)
                {
                    FindStuffForThing(recheck);
                    foreach (ThingDef thing in stuffOptions)
                    {
                        if (map.resourceCounter.GetCount(thing) >= stuffCount)
                        {
                            return true;
                        }
                    }
                    return false;
                }

            }
            return true;
            
        }
        //Maybe need to use listerthings as not all things are in resource. But if I have a ritual where I come across that might do something different as I fear perfomance
        //Can use target gets called seemingly every tick when selecting a building with gizmos 
        public virtual AcceptanceReport CanStartThings()
        {
            Map map = Find.CurrentMap;
            StringBuilder missing = new StringBuilder();
            if (thingsRequired.Count > 0)
            {
                foreach (KeyValuePair<ThingDef, int> thing in thingsRequired)
                {
                    if (map.resourceCounter.GetCount(thing.Key) <= thing.Value) 
                    {
                        missing.AppendLine(thing.Key.LabelCap + ": " + thing.Value);                        
                    }
                }                
            }
            if(missing.Length > 0)
            {
                return missing.ToString();
            }
            return true;
        }
        public virtual void FindStuffForThing(bool recheck)
        {//
            
            if (stuffToUse != null)
            {

                stuffDefToSpawn = stuffToUse;
                return;
            }
            if(stuffOptions.Count>0 && !recheck)//Obligation filters get called a lot so dont check resource list unless we have to
            {
                return;
            }
            stuffOptions.Clear();

            foreach (KeyValuePair<ThingDef, int> allCountedAmount in Find.CurrentMap.resourceCounter.AllCountedAmounts) 
            {
                if (allCountedAmount.Key.IsStuff && stuffCategoryDefs.Any(x=> allCountedAmount.Key.stuffProps.categories.Contains(x)) && allCountedAmount.Value >= stuffCount)
                {
                    stuffOptions.Add(allCountedAmount.Key);
                }
            }
            if(stuffOptions.Count() > 0)
            {
                stuffDefToSpawn = stuffOptions.RandomElement();
                
            }            
            
        }
        public virtual void DestroyThingsUsed(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome)
        {
            RitualBehaviorWorker_FuneralFramework behaivor = (RitualBehaviorWorker_FuneralFramework)ritual.Ritual.behavior;            
            foreach (Thing thing in ritual.usedThings) //Handles removing things used to spawn 
            {
                if (thing.def == behaivor.stuffToUse)
                {
                    stuffDefToSpawn = thing.def;
                    thing.DeSpawn(DestroyMode.Vanish);
                }
                if (thingsRequired?.ContainsKey(thing.def)?? false)
                {
                    thing.DeSpawn(DestroyMode.Vanish);
                }
            }
        }

        public virtual Thing GetThingToSpawn(LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome,Pawn pawnToSpawnOn, Corpse corpse)
        {
         
            ThingDef thingDef = CustomThingDefLogic(ritual,pawnToSpawnOn,corpse);
            thingDef = thingDef == null ? thingDefToSpawn : thingDef;
            if (thingDef == null)
            {
                return null;               
            }            
            int stacksize = thingCount;
            DestroyThingsUsed(ritual, bestOutcome, worstOutcome);//Separated this out so it can be used without having to spawn things
            ThingDef stuff = stuffDefToSpawn;
            if (thingDef.MadeFromStuff ? !GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined).Contains(stuff) :false)
            {
                stuff = GenStuff.DefaultStuffFor(thingDefToSpawn);
            }
            Thing thingToSpawn = ThingMaker.MakeThing(thingDef, stuff);
            initComps(thingToSpawn, corpse, ritual,bestOutcome,worstOutcome);
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

        public virtual void initComps(Thing thing,Corpse corpse, LordJob_Ritual ritual, bool bestOutcome, bool worstOutcome)//Probably going to need more
        {
            Comp_CorpseContainer comp = thing.TryGetComp<Comp_CorpseContainer>();
            if(comp != null)
            {
                comp.InitComp_CorpseContainer(corpse);
            }
                

        }
    }
}
