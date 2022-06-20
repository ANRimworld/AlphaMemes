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
            

            comp = (RitualOutcomeComp_FuneralFramework)def.comps.Find(x => x.GetType() == typeof(RitualOutcomeComp_FuneralFramework));
            extraOutcomeDesc = null;
            string extraDefDescription = comp.extraDefDescription;
            Pawn pawn = jobRitual.PawnWithRole(comp.roleToSpawnOn);
            Corpse corpse = jobRitual.assignments.AllPawns.First(x => x.Dead).Corpse;//Only one corpse
            ThingDef thingDef = comp.thingDefToSpawn;
            CustomThingDef(pawn, corpse,totalPresence, jobRitual, outcome, ref thingDef); //Not used in base behavior

            ExtraOutcomeDesc(pawn, corpse,totalPresence,jobRitual,outcome, ref extraOutcomeDesc, ref letterLookTargets);
            Thing thingToSpawn = ThingtoSpawn(pawn, corpse, totalPresence,outcome, jobRitual, thingDef, extraDefDescription);
            ApplyOn(pawn, corpse, thingToSpawn, totalPresence, jobRitual, outcome);

        }
        public virtual ThingDef CustomThingDef(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref ThingDef thingDef)
        {
            return thingDef; //Not used but hook for easy harmony patch or override
        }
        public virtual Thing ThingtoSpawn(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, OutcomeChance outcome, LordJob_Ritual jobRitual,ThingDef thingDef, string extraDefDescription)
        {

            if (thingDef == null)
            {
                return null;
            }
            int stacksize = comp.thingCount;
            ThingDef stuff = comp.stuffDefToSpawn;
            if(thingDef.MadeFromStuff & stuff == null)
            {
                stuff = GenStuff.DefaultStuffFor(thingDef);
            }
            Thing thingToSpawn = ThingMaker.MakeThing(thingDef, stuff);       
                
            //comp.ExtraDescription(pawn, corpse, totalPresence, jobRitual, outcome, thingToSpawn);                

            if (thingToSpawn is Building)
            {
                thingToSpawn = thingToSpawn.MakeMinified();
            }

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
        public virtual void ApplyOn(Pawn pawn, Corpse corpse, Thing thingToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
        {
            if (comp.stripcorpse == true)
            {
                corpse.Strip();
            }
            IntVec3 cell = pawn.Position;
            if (pawn.Dead)
            {
                cell = pawn.Corpse.Position;
            }
            if (thingToSpawn != null)
            {
                GenPlace.TryPlaceThing(thingToSpawn, cell, jobRitual.Map, ThingPlaceMode.Near);
            }
            corpse.Destroy();
           
            

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
            base.ExposeData();
        }
        public RitualOutcomeComp_FuneralFramework comp;

    }
}
