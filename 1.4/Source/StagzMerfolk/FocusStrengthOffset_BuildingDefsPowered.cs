using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StagzMerfolk;

public static class BuildingPoweredHelper
{
    public static bool BuildingPowered(Thing b)
    {
        CompPowerTrader compPowerTrader = b.TryGetComp<CompPowerTrader>();
        return compPowerTrader != null && compPowerTrader.PowerOn;
    }
}

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

public class FocusStrengthOffset_BuildingDefsPowered : FocusStrengthOffset_BuildingDefs
{
    public override bool CanApply(Thing parent, Pawn user = null)
    {
        return BuildingPoweredHelper.BuildingPowered(parent) && base.CanApply(parent, user);
    }


    protected override float OffsetForBuilding(Thing b)
    {
        if (BuildingPoweredHelper.BuildingPowered(b))
        {
            return base.OffsetFor(b.def);
        }

        return 0f;
    }

    protected override int BuildingCount(Thing parent)
    {
        if (parent == null || !parent.Spawned)
        {
            return 0;
        }

        int num = 0;
        List<Thing> forCell = parent.Map.listerBuldingOfDefInProximity.GetForCell(parent.Position, this.radius, this.defs, parent);
        for (int i = 0; i < forCell.Count; i++)
        {
            Thing b = forCell[i];
            if (BuildingPoweredHelper.BuildingPowered(b))
            {
                num++;
            }
        }

        return Math.Min(num, this.maxBuildings);
    }
}