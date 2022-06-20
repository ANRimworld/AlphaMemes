using HarmonyLib;
using System.Reflection;
using Verse;

namespace FuneralFramework
{ 
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.FuneralFramework");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }


    }
    
}
