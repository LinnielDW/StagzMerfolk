using RimWorld;
using Verse;

namespace StagzMerfolk;

public class HediffComp_DisappearsOnLeavingWater : HediffComp
{
    private bool usedVerb = false;
    private int minDuration = 20;

    private new HediffCompProperties_DisappearsOnLeavingWater Props
    {
        get { return (HediffCompProperties_DisappearsOnLeavingWater)this.props; }
    }

    public override bool CompShouldRemove
    {
        get { return base.CompShouldRemove || !this.parent.pawn.OnWater() || usedVerb; }
    }

    public override void CompPostTick(ref float severityAdjustment)
    {
        base.CompPostTick(ref severityAdjustment);
        if (minDuration > 0)
        {
            minDuration--;
        }
    }

    public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
    {
        base.Notify_PawnUsedVerb(verb, target);

        var aVerb = verb as Verb_CastAbility;
        if (aVerb?.ability != null && aVerb.ability.def != StagzDefOf.Stagz_DeepDive)
        {
            usedVerb = true;
        }
    }
}

public class HediffCompProperties_DisappearsOnLeavingWater : HediffCompProperties
{
    public HediffCompProperties_DisappearsOnLeavingWater()
    {
        this.compClass = typeof(HediffComp_DisappearsOnLeavingWater);
    }

    // public EffecterDef casterEffect;
}