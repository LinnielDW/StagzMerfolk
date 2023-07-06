using RimWorld;
using Verse;

namespace StagzMerfolk;

public class FocusStrengthOffset_Powered : FocusStrengthOffset
{
    public override string GetExplanation(Thing parent)
    {
        if (this.CanApply(parent, null))
        {
            return "StatsReport_Lit".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
        }

        return this.GetExplanationAbstract(null);
    }

    public override string GetExplanationAbstract(ThingDef def = null)
    {
        return "StatsReport_Lit".Translate() + ": " + this.offset.ToStringWithSign("0%");
    }

    public override float GetOffset(Thing parent, Pawn user = null)
    {
        return this.offset;
    }

    public override bool CanApply(Thing parent, Pawn user = null)
    {
        return BuildingPoweredHelper.BuildingPowered(parent) && base.CanApply(parent, user);
    }
}