using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace FuneralFramework
{
    public class RitualObligationTargetWorker_FuneralThingNeedsPower : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_FuneralThingNeedsPower()
        {
        }
        public RitualObligationTargetWorker_FuneralThingNeedsPower(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {
                
                return false;               
            }
            
            Thing thing = target.Thing;
           
            CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null)
			{
				if (compPowerTrader.PowerNet == null || !compPowerTrader.PowerNet.HasActivePowerSource)
				{
                    return "Funeral_NeedsPower".Translate(obligation.targetA.Thing.def.label);
                    
                }				
			}
            
            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            
            yield return "Funeral_ThingInfo".Translate(obligation.targetA.Thing.def.label);
            yield break;
        }
    }
}
