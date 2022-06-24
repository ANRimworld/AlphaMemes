using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Threading.Tasks;

namespace AlphaMemes
{
    public class RitualOutcomeComp_Crafter : RitualOutcomeComp_FuneralFramework
    {
        public override RitualOutcomeComp_Data MakeData()
        {
            return new RitualOutcomeComp_DataFuneralFramework();
        }
        public override bool Applies(LordJob_Ritual ritual)
        {
            return ritual.RoleFilled(roleId);
        }

        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {
            Pawn pawn = ritual.assignments.AssignedPawns(roleId).First();
            if (pawn != null)
            {
                return pawn.skills.GetSkill(skill).Level;
            }
            return base.Count(ritual, data);
        }
        public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            int count = assignments.AssignedPawns(roleId).First().skills.GetSkill(skill).Level;
            float quality = this.curve.Evaluate((float)count);
            return new ExpectedOutcomeDesc
            {
                label = "AM_RitualCrafterOutcome".Translate(skill.LabelCap.Named("SKILL")),
                count = Mathf.Min((float)count, base.MaxValue) + " / " + base.MaxValue,
                effect = this.ExpectedOffsetDesc(true, quality),
                quality = quality,
                positive = (count > 0),
                priority = 1f

            };
        }
        public string roleId;
        public SkillDef skill;

    }
}
