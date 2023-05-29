using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

public static class TailHelpers
{
    public static bool ApparelCoversLegs(Pawn pawn, ApparelProperties __instance)
    {
        // return __instance.CoversBodyPart(pawn.def.race.body.AllParts.All(x => x.gr == BodyPartDefOf.Leg));
        return pawn.def.race.body.AllParts.Where(part => part.IsInGroup(BodyPartGroupDefOf.Legs)).Any(record => __instance.CoversBodyPart(record));
    }

    public static bool ApparelCoversLegs(Pawn pawn, Apparel __instance)
    {
        return ApparelCoversLegs(pawn, __instance.def.apparel);
    }
}

[HarmonyPatch(typeof(ApparelProperties), "PawnCanWear", new Type[] { typeof(Pawn), typeof(bool) })]
public static class ApparelProperties_PawnCanWear_FishtailPatch
{
    private static bool Prefix(Pawn pawn, ref bool __result, ApparelProperties __instance)
    {
        // Log.Message(__instance);
        if (pawn.genes.HasGene(StagzDefOf.Stagz_Tail_Fish) && TailHelpers.ApparelCoversLegs(pawn, __instance))
        {
            __result = false;
            return false;
        }

        // Log.Message("can wear, continue");
        return true;
    }
}

[HarmonyPatch(typeof(Pawn_ApparelTracker), "Wear")]
public static class Pawn_ApparelTracker_Wear_FishtailPatch
{
    private static bool Prefix(Pawn ___pawn, Apparel newApparel)
    {
        if (___pawn.genes.HasGene(StagzDefOf.Stagz_Tail_Fish) && TailHelpers.ApparelCoversLegs(___pawn, newApparel))
        {
            // Log.Message("trying to wear: " + newApparel.LabelShort);
            Messages.Message("StagzMerfolk_CannotWearBecauseOfTail".Translate(___pawn.LabelShort), MessageTypeDefOf.NeutralEvent);
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
public static class PawnGenerator_GeneratePawn_FishtailPatch
{
    public static void Postfix(Pawn __result)
    {
        if (__result.genes != null && __result.genes.HasGene(StagzDefOf.Stagz_Tail_Fish))
        {
            for (int i = __result.apparel.WornApparel.Count - 1; i >= 0; i--)
            {
                var apparel = __result.apparel.WornApparel[i];
                if (TailHelpers.ApparelCoversLegs(__result, apparel))
                {
                    __result.apparel.Remove(apparel);
                }
            }
        }
    }
}