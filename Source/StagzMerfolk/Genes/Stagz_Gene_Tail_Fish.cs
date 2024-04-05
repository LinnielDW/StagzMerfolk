using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace StagzMerfolk;

public class Stagz_Gene_Tail_Fish : Gene
{
    public override void PostAdd()
    {
        base.PostAdd();

        foreach (var leg in pawn.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Leg))
        {
            pawn.health.RestorePart(leg, null, false);
            pawn.health.AddHediff(StagzDefOf.Stagz_Tail, leg);
        }
    }

    public override void PostRemove()
    {
        base.PostRemove();

        List<Hediff> parts = pawn.health.hediffSet.hediffs
            .Where(h => h.def == StagzDefOf.Stagz_Tail).ToList();

        foreach (var part in parts)
        {
            pawn.health.RestorePart(part.Part);
        }
    }
}