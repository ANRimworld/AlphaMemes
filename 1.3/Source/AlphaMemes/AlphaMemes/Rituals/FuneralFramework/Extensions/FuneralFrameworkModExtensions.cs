using System;
using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace FuneralFramework
{

    public class FuneralPreceptExtension : DefModExtension
    {
       
        public bool allowanimals = false; //Not working theoretical
        public bool requiresMeme = false; //allows for use for none ideo members that has same memes
        public bool isColonistFuneral = false; //Can only have 1 colonist funeral type
        public bool replaceConflictRituals = false; //Decides whether to replace vanilla funeral on generation
        public bool addNoCorpseFuneral = true;//Adds the base game funeral for no corpse (empty grave)
        public List<PreceptDef> conflictingPreceptDefs;
        public List<PreceptDef> PreceptConflicts(Ideo ideo)
        {

            List<PreceptDef> conflicts = new List <PreceptDef>();
            foreach (Precept p in ideo.PreceptsListForReading.Where<Precept>(x => x.def.HasModExtension<FuneralPreceptExtension>()))
            {
                if(isColonistFuneral && p.def.GetModExtension<FuneralPreceptExtension>().isColonistFuneral)
                {
                    conflicts.Add(p.def);
                    continue;
                }
                if (conflictingPreceptDefs.Contains<PreceptDef>(p.def))
                {
                    conflicts.Add(p.def);
                }
            }
            return conflicts;
        }
        public void SetNoCorpseFuneralDefName(Ideo ideo,PreceptDef precept)
        {
            //Used to make No Corpse Funeral have the same name as main funeral
            Precept_Ritual ritual = ideo.GetAllPreceptsOfType<Precept_Ritual>().First(
                x => x.def.HasModExtension<FuneralPreceptExtension>() ? x.def.GetModExtension<FuneralPreceptExtension>().addNoCorpseFuneral : false);
            precept.takeNameFrom = ritual.def;


        }
    }



}
