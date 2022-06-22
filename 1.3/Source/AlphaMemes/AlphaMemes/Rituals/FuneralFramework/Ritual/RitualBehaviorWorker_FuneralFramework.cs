using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using Verse.AI;
using HarmonyLib;
using Verse.AI.Group;
using RimWorld;


namespace AlphaMemes
{
    public class RitualBehaviorWorker_FuneralFramework : RitualBehaviorWorker
    {
        public RitualBehaviorWorker_FuneralFramework()
        {
        }
        public RitualBehaviorWorker_FuneralFramework(RitualBehaviorDef def) : base(def)
        {
        }
        //Overcomes checks on corpses
        public override void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
        {
            extension = ritual.def.GetModExtension<FuneralPreceptExtension>();
            if (CanStartRitualNow(target, ritual, null, assignments.ForcedRolesForReading) != null)
            {
                return;
            }
            if (!base.CanExecuteOn(target, obligation))
            {
                return;
            }
           
            LordJob_Ritual lordJob = (LordJob_Ritual)this.CreateLordJob(target, organizer, ritual, obligation, assignments);            
            LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, target.Map, assignments.Participants.Where(delegate (Pawn p)
            {
                bool flag = p.Dead;
                return !flag;
            }));

            FuneralFramework_PreparePawn(assignments);
            this.PostExecute(target, organizer, ritual, obligation, assignments);
            if (playerForced)
            {
                foreach (Pawn pawn in assignments.Participants)
                {
                    if (!pawn.Dead) 
                    { 
                        pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
                    }
                }
            }

        }
        public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {
            extension = ritual.def.GetModExtension<FuneralPreceptExtension>();
            if (ritual.activeObligations == null)
            {
                return null;
            }
            using (List<LordJob_Ritual>.Enumerator enumerator = Find.IdeoManager.GetActiveRituals(target.Map).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Ritual == ritual)
                    {
                        return "CantStartRitualAlreadyInProgress".Translate(ritual.Label).CapitalizeFirst();
                    }
                }
            }
            List<Thing> list = target.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
            bool flag = false;
            for (int i = 0; i < list.Count; i++)
            {
                Pawn innerPawn = ((Corpse)list[i]).InnerPawn;
                if (innerPawn.CanBeBuried() && innerPawn.IsColonist)
                { 
                    if(!extension.requiresMeme || (ritual.def.requiredMemes.Any(meme => innerPawn.Ideo.memes.Contains(meme))))
                    {
                        flag = true;
                        
                    }
                }
            }
            if (!flag)
            {
                return "Funeral_DisabledCorpseInaccessible".Translate();
            }
           
            return null; 
        }
      
        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);

            if (soundPlaying != null)
            {
                soundPlaying.Maintain();
            }
            if(effecter != null)
            {
                effecter.EffectTick(effecterpawn, ritual.selectedTarget);
            }
        }
        public override Sustainer SoundPlaying
        {
            get
            {
                return this.soundPlaying;
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
        }
        public override void Cleanup(LordJob_Ritual ritual)
        {
            if(this.soundPlaying != null)
            {
                soundPlaying.End();
                soundPlaying = null;
            }
            if(effecter != null)
            {
                effecter.Cleanup();
                effecter = null;
            }
            base.Cleanup(ritual);
        }
        protected override void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
        {
            leader = assignments.RoleForPawn(organizer);
       

        }
        public void FuneralFramework_PreparePawn(RitualRoleAssignments assignments)
        {
            foreach (Pawn participant in assignments.Participants)
            {
                if (participant.drafter != null)
                {
                    participant.drafter.Drafted = false;
                }
                if (!participant.Awake() && !participant.Dead)
                {
                    RestUtility.WakeUp(participant);
                }
            }
        }

        public Sustainer soundPlaying; //Cant override set        
        public Effecter effecter;
        public RitualRole leader;
        public Pawn effecterpawn;
        public FuneralPreceptExtension extension;

    }
}
