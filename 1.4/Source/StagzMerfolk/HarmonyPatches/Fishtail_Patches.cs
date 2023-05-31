using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

public static class TailHelpers
{
    public static BodyPartGroupDef[] LegsOrFeetGroups = new[] { BodyPartGroupDefOf.Legs, StagzDefOf.Feet };

    public static bool GroupsContainsLegsOrFeet(this IEnumerable<BodyPartGroupDef> bodyPartGroups)
    {
        return bodyPartGroups.Intersect(LegsOrFeetGroups).Any();
    }
    public static bool GroupContainsLegsOrFeet(this BodyPartGroupDef bodyPartGroup)
    {
        return GroupsContainsLegsOrFeet(new[] {bodyPartGroup});
    }
}

[HarmonyPatch(typeof(ApparelProperties), "PawnCanWear", new Type[] { typeof(Pawn), typeof(bool) })]
public static class ApparelProperties_PawnCanWear_FishtailPatch
{
    private static bool Prefix(Pawn pawn, ref bool __result, ApparelProperties __instance)
    {
        // Log.Message(__instance);
        if (pawn.genes.HasGene(StagzDefOf.Stagz_Gene_Tail_Fish) && __instance.bodyPartGroups.GroupsContainsLegsOrFeet())
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
        if (___pawn.genes.HasGene(StagzDefOf.Stagz_Gene_Tail_Fish) && newApparel.def.apparel.bodyPartGroups.GroupsContainsLegsOrFeet())
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
        if (__result.genes != null && __result.genes.HasGene(StagzDefOf.Stagz_Gene_Tail_Fish))
        {
            for (int i = __result.apparel.WornApparel.Count - 1; i >= 0; i--)
            {
                var apparel = __result.apparel.WornApparel[i];
                if (apparel.def.apparel.bodyPartGroups.GroupsContainsLegsOrFeet())
                {
                    __result.apparel.Remove(apparel);
                }
            }
        }
    }
}