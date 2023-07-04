using RimWorld;
using Verse;

namespace StagzMerfolk;

public class ConditionalStatEffector_Rain : ConditionalStatAffecter
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

        //Check if pawn is in rain
        return pawn.InRain();
    }

    public override string Label
    {
        get { return "StagzMerfolk_ConditionalStatEffector_Rain".Translate(); }
    }
}