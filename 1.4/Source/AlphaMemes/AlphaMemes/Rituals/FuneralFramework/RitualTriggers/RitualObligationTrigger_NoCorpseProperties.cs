﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    
    public class RitualObligationTrigger_NoCorpseProperties : RitualObligationTriggerProperties
    {
        public RitualObligationTrigger_NoCorpseProperties()
        {
            triggerClass = typeof(RitualObligationTrigger_NoCorpse);

        }

        
    }


}
