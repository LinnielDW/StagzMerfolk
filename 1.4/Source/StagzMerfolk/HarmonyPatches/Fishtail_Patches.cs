using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(Apparel), "PawnCanWear")]
public static class Apparel_PawnCanWear_FishtailPatch
{
    private static bool Prefix(Pawn pawn, ref bool __result)
    {
        if (pawn.genes.HasGene(StagzDefOf.Stagz_Tail_Fish))
        {
            __result = false;
            return false;
        }

        return true;
    }
}


[HarmonyPatch(typeof(Pawn_ApparelTracker), "Wear")]
public static class Pawn_ApparelTracker_Wear_FishtailPatch
{
    private static bool Prefix(Pawn ___pawn)
    {
        if (___pawn.genes.HasGene(StagzDefOf.Stagz_Tail_Fish))
        {
            Messages.Message("StagzMerfolk_CannotWearBecauseOfTail".Translate(___pawn.LabelShort), MessageTypeDefOf.NeutralEvent);
            return false;
        }

        return true;
    }
}