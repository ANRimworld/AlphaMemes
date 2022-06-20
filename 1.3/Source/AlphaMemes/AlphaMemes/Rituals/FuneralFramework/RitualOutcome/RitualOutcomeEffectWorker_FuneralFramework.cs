using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace FuneralFramework
{
    
    public class RitualOutcomeEffectWorker_FuneralFramework: RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_FuneralFramework() 
        { 
        }
        public RitualOutcomeEffectWorker_FuneralFramework(RitualOutcomeEffectDef def) : base(def)
        {
        }
        //Extends the functionality of apply extra outcome
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            //Get comp data
            comp = (RitualOutcomeComp_FuneralFramework)def.comps.Find(x => x.GetType() == typeof(RitualOutcomeComp_FuneralFramework));
            extraOutcomeDesc = null;
            Pawn corpsePawn = jobRitual.PawnWithRole("FF_RitualRoleCorpse");      
            Corpse corpse = corpsePawn.Corpse;
            ThingDef thingDef = comp.thingDefToSpawn;
            CustomThingDef(corpsePawn, totalPresence, jobRitual, outcome, ref thingDef); //Not used in base behavior
            ExtraOutcomeDesc(corpsePawn,corpse,totalPresence,jobRitual,outcome, ref extraOutcomeDesc, ref letterLookTargets);
            Thing thingToSpawn = ThingtoSpawn(corpsePawn, outcome, jobRitual, thingDef);
            ApplyOn(corpsePawn, corpse, thingToSpawn, totalPresence, jobRitual, outcome);

        }
        public virtual ThingDef CustomThingDef(Pawn corpsePawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref ThingDef thingDef)
        {
            return thingDef;
        }
        public virtual Thing ThingtoSpawn(Pawn corpsePawn,OutcomeChance outcome, LordJob_Ritual jobRitual,ThingDef thingDef)
        {

            if (thingDef == null)
            {
                return null;
            }
            int stacksize = comp.thingCount;
            Thing thingToSpawn = ThingMaker.MakeThing(thingDef);
            if (OutcomeChanceWorst(jobRitual, outcome))
            {
                stacksize = (int)Math.Round(stacksize * comp.worstOutcomeMulti);
            }
            if (outcome.BestPositiveOutcome(jobRitual))
            {

                stacksize = (int)Math.Round(stacksize * comp.bestOutcomeMulti);
            }
            if(stacksize <= 0)
            {
                return null;
            }
            thingToSpawn.stackCount = stacksize;
            return thingToSpawn;
            

        }
        public virtual void ApplyOn(Pawn corpsePawn, Corpse corpse, Thing thingToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
        {
            if (comp.stripcorpse == true)
            {
                corpse.Strip();
            }
            corpse.Destroy();
            GenPlace.TryPlaceThing(thingToSpawn, corpse.Position, corpse.Map, ThingPlaceMode.Near);

        }
        public virtual void ExtraOutcomeDesc(Pawn corpsePawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            if (OutcomeChanceWorst(jobRitual, outcome))
            {
               
                extraOutcomeDesc = "Funeral_WorstResult".Translate(jobRitual.Ritual.Label.Named("RITUAL"), corpsePawn.Name.Named("CORPSE"));
            }
            if (outcome.BestPositiveOutcome(jobRitual))
            {

                extraOutcomeDesc = "Funeral_BestResult".Translate(jobRitual.Ritual.Label.Named("RITUAL"), corpsePawn.Name.Named("CORPSE"));
            }
        }

        public bool OutcomeChanceWorst(LordJob_Ritual jobRitual, OutcomeChance outcome) //Doing opposite of Vanilla check if best
    {
        using (List<OutcomeChance>.Enumerator enumerator = jobRitual.Ritual.outcomeEffect.def.outcomeChances.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.positivityIndex < outcome.positivityIndex)
                {
                    return false;
                }
            }
        }
        return true;
        }
        public override void ExposeData()
        {

        }
        public RitualOutcomeComp_FuneralFramework comp;

    }
}
