using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    //Not Actually a real obligation trigger gets called via harmony patch
     //moved logic here so it can be called via mod extensions allowing flexibility
     //check if I need to expose this stuff. 
    public class RitualObiligationTrigger_Animals 
    {
        public bool bondedAnimals = true;
        public bool veneratedAnimals = true;
        public bool namedanimals = false;
        public bool allAnimals = false;//Probably a bad idea to turn this on but its an option

        public virtual bool CanAddPawn(Pawn pawn, Ideo ideo, Precept_Ritual ritual, out Pawn target2)
        {
            target2 = null;
            bool flag = false;
            bool bonded = TrainableUtility.GetAllColonistBondsFor(pawn).Any(x => x.Ideo == ideo);
            bool venerated = ideo.VeneratedAnimals.Any(x => x == pawn.def);
            bool named = !pawn.Name.Numerical;
            if (bonded && bondedAnimals)
            {
                target2 = TrainableUtility.GetAllColonistBondsFor(pawn).FirstOrDefault(x => x.Ideo == ideo);
                flag = true;
            }
            if ((venerated && veneratedAnimals) || namedanimals || allAnimals)
            {
                flag = true;
            }
            return flag;

        }
        



    }


}
