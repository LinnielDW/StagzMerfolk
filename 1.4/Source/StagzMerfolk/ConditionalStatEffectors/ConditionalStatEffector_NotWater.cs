using RimWorld;
using Verse;

namespace StagzMerfolk;

public class ConditionalStatEffector_NotWater : ConditionalStatAffecter
{
    public override bool Applies(StatRequest req)
    {
        //Check if biotech is active
        if (!ModsConfig.BiotechActive)
        {
            return false;
        }

        //Check if request is valid (request is for a pawn and pawn is on a map
        var pawn = req.Thing as Pawn;
        if (!req.HasThing || pawn?.Map == null) return false;

        //Check if pawn is in water or in rain
        return !pawn.OnWater();
    }

    public override string Label
    {
        get { return "StagzMerfolk_ConditionalStatEffector_NotWater".Translate(); }
    }
}