using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace StagzMerfolk.CompAbility;

public class CompAbilityEffect_GiveMentalStateInArea : CompAbilityEffect
{
    private new CompProperties_AbilityGiveMentalStateInArea Props
    {
        get
        {
            return (CompProperties_AbilityGiveMentalStateInArea)this.props;
        }
    }

    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        base.Apply(target, dest);
        var things = GenRadial.RadialDistinctThingsAround(target.Cell, this.parent.pawn.Map, this.parent.def.EffectRadius, true);
        foreach (var thing in things)
        {
            if (thing is Pawn pawn && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Hearing) && pawn.psychicEntropy.PsychicSensitivity >= 0.1f && pawn.Faction != Faction.OfPlayer)
            {
                if (Rand.Chance(0.2f))
                {
                    // Log.Message("effected " + pawn.Label);
                    pawn.jobs.StopAll();
                    pawn.mindState.mentalStateHandler.TryStartMentalState(Props.mentalState);
                }
                // else
                // {
                //     Log.Message("did not effect " + pawn.Label);
                // }

            }
        }
        if (this.Props.casterEffect != null)
        {
            Effecter effecter = this.Props.casterEffect.SpawnAttached(this.parent.pawn, this.parent.pawn.MapHeld, 1f);
            effecter.Trigger(this.parent.pawn, null, -1);
            effecter.Cleanup();
        }

    }
    
    public override void DrawEffectPreview(LocalTargetInfo target)
    {
        GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red, null);
    }
    
    private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
    {
        if (target.Cell.Filled(this.parent.pawn.Map))
        {
            yield break;
        }
        foreach (IntVec3 intVec in GenRadial.RadialCellsAround(target.Cell, this.parent.def.EffectRadius, true))
        {
            if (intVec.InBounds(map) && GenSight.LineOfSightToEdges(target.Cell, intVec, map, true, null))
            {
                yield return intVec;
            }
        }
    }
}

public class CompProperties_AbilityGiveMentalStateInArea : CompProperties_AbilityEffect
{
    public CompProperties_AbilityGiveMentalStateInArea()
    {
        this.compClass = typeof(CompAbilityEffect_GiveMentalStateInArea);
    }		
    
    public EffecterDef casterEffect;
    public MentalStateDef mentalState;
}