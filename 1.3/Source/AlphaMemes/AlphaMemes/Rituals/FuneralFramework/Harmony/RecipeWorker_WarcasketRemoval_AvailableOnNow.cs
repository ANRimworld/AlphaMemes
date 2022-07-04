using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using System;


namespace AlphaMemes
{

/*    [HarmonyPatch(typeof(Building_WarcasketFoundry))]
    [HarmonyPatch("OccupantAliveAndPresent")]
    [HarmonyPatch(MethodType.Getter)]*/
    //To get the mesh to work on dreadnought funeral
    public static class RecipeWorker_WarcasketRemoval_AvailableOnNow_Patch
    {
       /* [HarmonyPostfix]*/
        public static void Postfix(ref bool __result,Thing thing, BodyPartRecord part = null)
        {
            if (__result == false) { return; }
            if (thing is Pawn pawn && pawn.health.hediffSet.HasHediff(DefDatabase<HediffDef>.GetNamed("AM_WarCasketLifeSupport")))
            {
                __result = false;
            }
            

        }

    }


}
