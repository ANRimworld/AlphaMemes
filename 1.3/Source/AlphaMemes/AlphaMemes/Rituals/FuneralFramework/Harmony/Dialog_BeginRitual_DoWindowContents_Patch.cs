﻿using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;


namespace AlphaMemes
{

    [HarmonyPatch(typeof(Dialog_BeginRitual))]
    [HarmonyPatch("DoWindowContents")]

    //This patch adds stuff selection
    public static class Dialog_BeginRitual_DoWindowContents_Patch
    {
        public static void Postfix(Dialog_BeginRitual __instance, Rect inRect)
        {
            //I need things and they are all private =(
            Precept_Ritual ritual = (Precept_Ritual)AccessTools.Field(typeof(Dialog_BeginRitual), "ritual").GetValue(__instance);
            if (!ritual.def.HasModExtension<FuneralPreceptExtension>())
            {
                return;
            }
            //Do we need stuff
            if (ritual.outcomeEffect.def.HasModExtension<OutcomeEffectExtension>())
            {
                OutcomeEffectExtension data = ritual.outcomeEffect.def.GetModExtension<OutcomeEffectExtension>();
                Dictionary<Thing, int> stuffOptions = new Dictionary<Thing, int>();
                RitualBehaviorWorker_FuneralFramework behavior = (RitualBehaviorWorker_FuneralFramework)ritual.behavior;
                
                foreach (FuneralFramework_ThingToSpawn spawner in data.outcomeSpawners)
                {
                    
                    foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.Where(x => spawner.stuffOptions.Contains(x.def)))
                    {
                        stuffOptions.Add(thing, spawner.stuffCount);
                        if(behavior.stuffToUse == null)//Doing this because I cant easily make a selection mandatory so if they dont select its just one of the options
                        {
                            behavior.stuffToUse = thing.def;
                            behavior.stuffCount = spawner.stuffCount;
                        }

                    }
                }

                var selectStuffToUse = new Rect(inRect.xMax - buttonDimensions.x, inRect.yMax - 76f - buttonDimensions.y, buttonDimensions.x, buttonDimensions.y);
                
                DrawButton(selectStuffToUse, behavior.stuffCount.ToString() + " " + behavior.stuffToUse.label, delegate
                {
                    var floatOptions = new List<FloatMenuOption>();
                    foreach(KeyValuePair<Thing, int> option in stuffOptions)
                    {
                        floatOptions.Add(new FloatMenuOption(option.Value.ToString() + " " + option.Key.LabelCapNoCount, delegate
                        {
                            behavior.stuffToUse = option.Key.def;
                            behavior.stuffCount = option.Value;
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(floatOptions));
                });
            }
               

        }
        private static void DrawButton(Rect rect, string label, Action action)
        {
            if (Widgets.ButtonText(rect, label))
            {
                action();
            }

        }


        private static Vector2 buttonDimensions = new Vector2(200f, 24f);
    }


}
