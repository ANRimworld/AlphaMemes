using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace FuneralFramework
{
    [DefOf]
    public static class InternalDefOf
    {
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

        [MayRequireIdeology]
        public static JobDef FF_DeliverCorpseToCell;
        public static PreceptDef FF_FuneralNoCorpse;
    }



}
