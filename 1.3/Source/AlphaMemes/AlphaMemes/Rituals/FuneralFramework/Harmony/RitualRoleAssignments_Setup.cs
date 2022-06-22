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

    [HarmonyPatch(typeof(RitualRoleAssignments))]
    [HarmonyPatch("Setup")]
    //This patch is to get the corpses in the list of selectable pawns
    public static class FuneralFramework_RitualRoleAssignments_Setup_Patch
    {
        [HarmonyPrefix]
        public static void SelectViableCorpse(RitualRoleAssignments __instance, ref List<Pawn> allPawns, ref Dictionary<string, Pawn> forcedRoles)
        {
            //Access tools
            if(__instance.Ritual == null)
            {
                return; //Not all rituals are rituals neat.
            }
            Precept_Ritual ritual = __instance.Ritual;

            if (ritual.def.HasModExtension<FuneralPreceptExtension>())
            {
                Map map = allPawns[0].Map; //Grab the map of the pawns to rebuild it
                List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
                for (int i = 0; i < list.Count; i++) 
                {
                    Pawn innerPawn = ((Corpse)list[i]).InnerPawn;
                    if (innerPawn.CanBeBuried() && (innerPawn.IsColonist||(ritual.def.GetModExtension<FuneralPreceptExtension>().allowanimals && innerPawn.Faction.IsPlayer))) 
                    {
                        ;
                        if (ritual.def.GetModExtension<FuneralPreceptExtension>().requiresMeme ? (ritual.def.requiredMemes.Any(meme => innerPawn.Ideo.memes.Contains(meme))):true)
                        {                            
                            if (!allPawns.Contains(innerPawn))
                            {                               
                                allPawns.Add(innerPawn);
                            }
                            
                        }
                    }
                }
            }

        }

    }

}
