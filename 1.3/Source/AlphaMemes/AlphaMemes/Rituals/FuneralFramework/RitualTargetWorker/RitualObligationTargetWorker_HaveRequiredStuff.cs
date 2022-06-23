using System;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{

    public class RitualObligationTargetWorker_HaveRequiredStuff : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_HaveRequiredStuff()
        {
        }
        public RitualObligationTargetWorker_HaveRequiredStuff(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {
                return false;
            }
            Precept_Ritual ritual = parent;
            if (ritual == null)
            {
                Log.Error("The thing you expected");
            }
            
            Map map = target.Thing.Map;
            
            if (stuffToUse != null)//if passed skip the heavy lifting
            {
                if (map.resourceCounter.GetCount(stuffToUse) >= count)
                {
                    canSpawn = true;
                    return true;
                }
            }            
          
            foreach (KeyValuePair<ThingDef, int> allCountedAmount in map.resourceCounter.AllCountedAmounts) //Slightly worried about perfomance of this since game likes to call this a lot
            {                
                if (allCountedAmount.Key.IsStuff && allCountedAmount.Key.stuffProps.CanMake(buildableDefToMake) && allCountedAmount.Value >= count)
                {
                    stuffOptions.Add(allCountedAmount.Key);                    
                }
            }
            if(stuffOptions.Count > 0)
            {
                stuffToUse = stuffOptions.RandomElement();//Yup it sucks
            }
            else
            {
                return "Funeral_NotEnoughStuff".Translate(string.Join(", ", buildableDefToMake.stuffCategories.Select(x => x.label).Named("STUFF")), count.ToString().Named("COUNT"));
            }
            canSpawn = true;
            return true;
		}

        public override void ExposeData()
        {
           //pretty sure exposing is going to explode cause only 1 obligigation
           //maybe subclasses exposed under main, but probably not important. Saving stufftouse/options would be nice to not need to check it as often but it'll have to be rechecked regardless

        }
        
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            yield return "Funeral_XOf".Translate(count.ToString());
            foreach(StuffCategoryDef stuffCategoryDef in buildableDefToMake.stuffCategories)
            {
                yield return stuffCategoryDef.label;
            }            
            yield break;
        }
        public ThingDef buildableDefToMake; //This only works with buildabledef, things w/ recipes scare me
        public ThingDef stuffToUse; //Dont have a way to select yet if ever (would need to create custom ritul start UI) use this for specific def
        List<ThingDef> stuffOptions = new List<ThingDef>(); //If I solve above this will be better
        public int count;
        public bool canSpawn = false;
        
        
    }
}
