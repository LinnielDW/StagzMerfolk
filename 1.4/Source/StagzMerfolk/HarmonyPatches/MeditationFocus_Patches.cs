﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(MeditationFocusDef), nameof(MeditationFocusDef.EnablingThingsExplanation))]
public class MeditationFocusDef_EnablingThingsExplanation_Patch
{
    public static void Postfix(Pawn pawn, MeditationFocusDef __instance, ref string __result)
    {
        if (!ModsConfig.RoyaltyActive) return;

        if (__instance == StagzDefOf.Stagz_Water)
        {
            if (pawn.genes != null && pawn.genes.HasGene(StagzDefOf.Stagz_RainVeil))
            {
                __result += "\n  - " + "Stagz_UnlockedByGeneRainVeil".Translate() + ".";
            }
        }
    }
}

[HarmonyPatch(typeof(MeditationFocusTypeAvailabilityCache), "PawnCanUseInt")]
public class MeditationFocusTypeAvailabilityCache_PawnCanUseInt_Patch
{
    public static void Postfix(Pawn p, MeditationFocusDef type, ref bool __result)
    {
        if (!ModsConfig.RoyaltyActive) return;

        if (type == StagzDefOf.Stagz_Water)
        {
            if (p.genes != null && p.genes.HasGene(StagzDefOf.Stagz_RainVeil))
            {
                __result = true;
            }
        }
    }
}