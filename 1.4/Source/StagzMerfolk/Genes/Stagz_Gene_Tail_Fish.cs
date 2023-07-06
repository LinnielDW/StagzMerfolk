using System.Collections.Generic;
using System.Linq;
using RimWorld;
using StagzMerfolk.HarmonyPatches;
using Verse;

namespace StagzMerfolk;

public class Stagz_Gene_Tail_Fish : Gene
{
    /*private int ticksToHeal;
    private static readonly IntRange HealingIntervalTicksRange = new IntRange(900000, 1800000);

    public override void Tick()
    {
        base.Tick();
        this.ticksToHeal--;
        if (this.ticksToHeal <= 0)
        {
            HealLegFootScar(this.LabelCap);
            this.ResetInterval();
        }
    }

    private void ResetInterval()
    {
        this.ticksToHeal = HealingIntervalTicksRange.RandomInRange;
    }

    public void HealLegFootScar(string cause)
    {
        Hediff hediff;
        if (!(from hd in pawn.health.hediffSet.hediffs
                where (hd.IsPermanent() || hd.def.chronic) && hd.Part.groups.GroupsContainsLegsOrFeet()
                select hd).TryRandomElement(out hediff))
        {
            return;
        }

        HealthUtility.Cure(hediff);
        if (PawnUtility.ShouldSendNotificationAbout(pawn))
        {
            Messages.Message("MessagePermanentWoundHealed".Translate(cause, pawn.LabelShort, hediff.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
        }
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
        if (DebugSettings.ShowDevGizmos)
        {
            yield return new Command_Action
            {
                defaultLabel = "DEV: Heal permanent tail wound",
                action = delegate()
                {
                    HealLegFootScar(this.LabelCap);
                    this.ResetInterval();
                }
            };
        }

        yield break;
    }*/


    public override void PostAdd()
    {
        base.PostAdd();
        foreach (var leg in pawn.RaceProps.body.AllParts.Where(x => x.def == BodyPartDefOf.Leg))
        {
            pawn.health.AddHediff(StagzDefOf.Stagz_Tail, leg);
        }
    }

    public override void PostRemove()
    {
        base.PostRemove();

        List<Hediff> parts = pawn.health.hediffSet.hediffs.Where(h => h.def == StagzDefOf.Stagz_Tail).ToList();

        foreach (var part in parts)
        {
            pawn.health.RestorePart(part.Part);
        }
    }
}