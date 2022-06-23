using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using System.Linq;
using RimWorld;

namespace AlphaMemes
{
    public class RitualObligationTargetWorker_FuneralThingExtended : RitualObligationTargetFilter
    {
        public RitualObligationTargetWorker_FuneralThingExtended()
        {
        }
        public RitualObligationTargetWorker_FuneralThingExtended(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
        {
            extension = def.GetModExtension<FuneralFramework_ObligationTargetExtension>();
            foreach (RitualObligationTargetFilter worker in extension.filters)
            {
                foreach(TargetInfo target in worker.GetTargets(obligation, map))
                {
                    yield return target;
                }              
            }
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {            

            extension = def.GetModExtension<FuneralFramework_ObligationTargetExtension>();

            RitualTargetUseReport temp;
            bool canUse = true;
            StringBuilder failReasons = new StringBuilder();

            foreach(RitualObligationTargetFilter worker in extension.filters)
            {
                temp = worker.CanUseTarget(target, obligation);
                if(!temp.canUse) { canUse = false; }
                if(temp.failReason != null)
                {
                    failReasons.AppendInNewLine(temp.failReason);                   
                }
            }
            if (failReasons.Length>0)
            {
                return failReasons.ToStringSafe();
            }
            else if (!canUse) { return false; }
            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {

            extension = def.GetModExtension<FuneralFramework_ObligationTargetExtension>();
            
            foreach (RitualObligationTargetFilter worker in extension.filters)
            {
                foreach(string str in worker.GetTargetInfos(obligation))
                {
                    yield return str;
                }
            }
            yield break;
        }
        FuneralFramework_ObligationTargetExtension extension;
    }
}
