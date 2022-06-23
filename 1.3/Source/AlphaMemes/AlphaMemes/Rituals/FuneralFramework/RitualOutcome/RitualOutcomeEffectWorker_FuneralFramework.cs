using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AlphaMemes
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
            bool worstOutcome = OutcomeChanceWorst(jobRitual, outcome);
            bool bestOutcome = outcome.BestPositiveOutcome(jobRitual);
            extraOutcomeDesc = null;            
            Pawn pawn = jobRitual.PawnWithRole(comp.roleToSpawnOn);
            Corpse corpse = jobRitual.assignments.AllPawns.First(x => x.Dead).Corpse;//Only one corpse       
            ExtraOutcomeDesc(pawn, corpse,totalPresence,jobRitual,outcome, ref extraOutcomeDesc, ref letterLookTargets);
            List<Thing> thingsToSpawn = new List<Thing>();
            foreach(RitualOutcome_FuneralFramework_ThingToSpawn getThing in comp.outcomeSpawners)
            {
                Thing thingToSpawn = getThing.GetThingToSpawn(jobRitual, bestOutcome, worstOutcome, pawn, corpse);
                if(thingToSpawn != null)
                {
                    thingsToSpawn.Add(thingToSpawn);
                }
            }
            
            ApplyOn(pawn, corpse, thingsToSpawn, totalPresence, jobRitual, outcome);

        }


        public virtual void ApplyOn(Pawn pawn, Corpse corpse, List<Thing> thingsToSpawn, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome)
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
            if (thingsToSpawn.Count > 0)
            {
                foreach (Thing thingToSpawn in thingsToSpawn)
                {
                    GenPlace.TryPlaceThing(thingToSpawn, cell, jobRitual.Map, ThingPlaceMode.Near);
                }                
            }
            corpse.Destroy();
           
            

        }
        public virtual void ExtraOutcomeDesc(Pawn pawn, Corpse corpse, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, ref string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            if (OutcomeChanceWorst(jobRitual, outcome) && comp.worstOutcomeDesc != null)
            {
               
                extraOutcomeDesc = comp.worstOutcomeDesc.Translate(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"),pawn.Name.Named("SPAWNPAWN"));
            }
            if (outcome.BestPositiveOutcome(jobRitual))
            {
                extraOutcomeDesc = comp.bestOutcomeDesc.Translate(jobRitual.Ritual.Label.Named("RITUAL"), corpse.InnerPawn.Name.Named("CORPSE"), pawn.Name.Named("SPAWNPAWN"));
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
