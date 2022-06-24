using System;
using System.Text;
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
            

            
            //Bit convoluted but outcome stores the data to know if we can start
            RitualOutcomeComp_DataFuneralFramework data = (RitualOutcomeComp_DataFuneralFramework)parent.outcomeEffect.compDatas.Find(x => x != null ?  x.GetType() == typeof(RitualOutcomeComp_DataFuneralFramework): false);            
            StringBuilder failReasons = new StringBuilder();
            foreach (FuneralFramework_ThingToSpawn spawner in data.outcomeSpawners.Where(x=> x.stuffCount>0))
            {
                if (!spawner.CanStart())
                {                    
                    failReasons.AppendInNewLine("Funeral_NotEnoughStuff".Translate(string.Join(", ", spawner.stuffCategoryDefs.Select(x => x.label).Named("STUFF")), spawner.stuffCount.ToString().Named("COUNT")));
                }
            }

            if (failReasons.Length > 0)
            {
                return failReasons.ToString();
            }
            return true;
		}


        
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            //Use extension as this gets called before map is even created so always use defaults
            FuneralPreceptExtension extension = parent.def.GetModExtension<FuneralPreceptExtension>();            
            foreach(FuneralFramework_ThingToSpawn spawner in extension.spawners.Where(x => x.stuffCount > 0))
            {
                string stringReturn = "Funeral_XOf".Translate(spawner.stuffCount.ToString());
                stringReturn = stringReturn + "(" + string.Join(", ", spawner.stuffCategoryDefs.Select(x => x.label)) + ")"; 
                yield return stringReturn;
            }
       
            yield break;
        }

    
        
        
    }
}
