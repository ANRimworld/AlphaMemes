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

    [HarmonyPatch(typeof(Ideo))]
    [HarmonyPatch("AddPrecept")]
    //This patch is to prevent the game from adding 2 funerals
    //Im sure there's a better spot/way for this but i cant find it T_T since IdeoFoundation Canadd doesnt feel like being used for rituals
    public static class FuneralFramework_Ideo_AddPrecept
    {

        [HarmonyPrefix]
        //Prefix is for a vanilla bug with required memes and Ideo reformation
        public static bool Prefix(Ideo __instance, Precept precept, ref RitualPatternDef fillWith, ref bool init)
        {

            if (precept.def.issue.defName != "Ritual")
            {
                return true;
            }
            
            //Fix for vanilla bug, adding arguments for rituals that dont get passed during reform when they are need
            if (fillWith == null)
            {
                Precept_Ritual ritual;
                if ((ritual = (precept as Precept_Ritual)) != null)
                {
                    init = true;
                    fillWith = ritual.def.ritualPatternBase;
                }
            }
            //Determination Logic for multiple funerals
            if (precept.def.HasModExtension<FuneralPreceptExtension>())
            {
                if (precept.def == InternalDefOf.AM_FuneralNoCorpse)
                {
                    precept.def.GetModExtension<FuneralPreceptExtension>().SetNoCorpseFuneralDefName(__instance, precept.def);
                }
                //This will add a 2nd funeral, have to decide who wins
                bool replaceRituals = precept.def.GetModExtension<FuneralPreceptExtension>().replaceConflictRituals;
                List<PreceptDef> preceptConflicts = precept.def.GetModExtension<FuneralPreceptExtension>().PreceptConflicts(__instance);
                if (!replaceRituals && preceptConflicts.Count > 0)
                {
                    return false; //There's a conflict and we aren't replacing so dont add
                }
                foreach (PreceptDef p in preceptConflicts)
                {
                    __instance.RemovePrecept(__instance.GetPrecept(p));//Remove Conflicts
                }

            }
            return true;

        }
        //Moved it all to prefix with thought of dont add a precept just to immediately remove it
        //Pretty sure there's no situation where it can cause compatability issues I hope
        //[HarmonyPostfix]
        /*        public static void Postfix(Ideo __instance, Precept precept)
                {
                    if (precept.def.issue.defName != "Ritual") return;


                    if (precept.def.HasModExtension<FuneralPreceptExtension>())
                    {
                        if (precept.def == InternalDefOf.FF_FuneralNoCorpse)
                        {
                            precept.def.GetModExtension<FuneralPreceptExtension>().SetNoCorpseFuneralDefName(__instance, precept.def);
                        }
                        //This will add a 2nd funeral, have to decide who wins
                        bool replaceRituals = precept.def.GetModExtension<FuneralPreceptExtension>().replaceConflictRituals;
                        List<PreceptDef> preceptConflicts = precept.def.GetModExtension<FuneralPreceptExtension>().PreceptConflicts(__instance);
                        if (!replaceRituals && preceptConflicts.Count > 0)
                        {
                            __instance.RemovePrecept(__instance.GetPrecept(precept.def)); //There's a conflict and we aren't replacing so dont add
                        }
                        foreach (PreceptDef p in preceptConflicts)
                        {
                            __instance.RemovePrecept(__instance.GetPrecept(p));//Remove Conflicts
                        }
                        return;
                    }

                }*/
    }
}


