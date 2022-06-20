using System;
using System.Linq;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace FuneralFramework
{

    [StaticConstructorOnStartup]
    public static class ModCompatibilityCheck
    {
       
        static ModCompatibilityCheck()
        {
            List<ModMetaData> list = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
            foreach(ModMetaData mod in list)
            {
                if(mod.PackageId == "VanillaExpanded.VGeneticsE")
                {
                    VanillaGenetics = true;
                }
            }
        }

    public static bool VanillaGenetics;
    }



}
