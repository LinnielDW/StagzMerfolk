using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(Verb_MeleeAttack), "GetDodgeChance")]
public static class Verb_MeleeAttack_GetDodgeChance_Patch
{
    private static void Postfix(LocalTargetInfo target, ref float __result)
    {
        Pawn pawn = target.Thing as Pawn;
        if (pawn != null && pawn.genes.HasGene(StagzDefOf.Stagz_KeenReflexes) && __result > 0)
        {
            __result += 0.2f;
        }
    }
}

//TODO: patch SpecialDisplayStats to show keen reflexes (similar to DarknessCombatUtility.GetStatEntriesForPawn)

[HarmonyPatch(typeof(ShotReport), "AimOnTargetChance_IgnoringPosture", MethodType.Getter)]
public static class ShotReport_AimOnTargetChance_IgnoringPosture_Patch
{
    private static void Postfix(ref float __result, ref TargetInfo ___target)
    {
        if (___target == null) return;

        var pawn = ___target.Thing as Pawn;
        if (pawn != null && pawn.genes.HasGene(StagzDefOf.Stagz_KeenReflexes))
        {
            //0.75 multiplier and max of 0.9 can be changed later
            __result = Math.Min(__result + (pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true, -1) * 0.75f), 0.9f);
        }
    }
}

// TODO: finish
/*[HarmonyPatch(typeof(ShotReport), "GetTextReadout")]
public static class ShotReport_GetTextReadout_Patch
{
    private static void Postfix(ref string __result, ref TargetInfo ___target)
    {
        if (___target == null) return;
                
        var pawn = ___target.Thing as Pawn;
        if (pawn != null && pawn.genes.HasGene(StagzDefOf.Stagz_KeenReflexes))
        {
            __result += " bla + " + (pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true, -1) * 0.75f).ToString();
        }
    }
}*/