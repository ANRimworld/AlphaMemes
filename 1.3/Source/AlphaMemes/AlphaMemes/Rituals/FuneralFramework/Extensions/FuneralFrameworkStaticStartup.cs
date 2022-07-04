using System;
using RimWorld;
using UnityEngine;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace AlphaMemes
{
    [StaticConstructorOnStartup]
    public static class FuneralFrameWork_StaticStartup
    {

        static FuneralFrameWork_StaticStartup()
        {
            foreach(PreceptDef precept in DefDatabase<PreceptDef>.AllDefsListForReading.Where(x => x.HasModExtension<FuneralPreceptExtension>())){
                funeralDefs.Add(precept);
            }
            VFEPLoaded = ModsConfig.IsActive("OskarPotocki.VFE.Pirates");
        }

        
        public static List<PreceptDef> funeralDefs = new List<PreceptDef>();
        public static bool VFEPLoaded;

    }



}
