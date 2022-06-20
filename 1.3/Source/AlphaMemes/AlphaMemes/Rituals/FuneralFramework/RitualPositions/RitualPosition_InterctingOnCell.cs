using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace FuneralFramework
{    //simple override to make the position always facing interaction spot
    public class RitualPosition_InterctingOnCell : RitualPosition_OnInteractionCell
    {
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Thing thing = ritual.selectedTarget.Thing;
			Rot4 rotation = thing.Rotation;
			base.facing = rotation;
			return base.GetCell(spot, p, ritual);
		}
	}
}
