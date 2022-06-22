using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld.Planet;
using System;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("CanAddPrecept")]
    //Patch to prevent user from adding 2 funerals
    public static class FuneralFramework_IdeoUIUtility_CanAddPrecept_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref AcceptanceReport __result, PreceptDef def, RitualPatternDef pat, Ideo ideo)
        {
    
            if (def.HasModExtension<FuneralPreceptExtension>())
            {
                List<PreceptDef> preceptConflicts = def.GetModExtension<FuneralPreceptExtension>().PreceptConflicts(ideo);
                if (preceptConflicts.Any())
                {  
                    string conflicts = string.Join(", ", preceptConflicts.Select(s => s.label));
                    __result = "Funeral_ConflictingPrecepts".Translate(conflicts.Named("CONFLICTS"));
                }
            }




        }

    }


}
