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
            // pawn_EquipmentTracker = (((thing != null) ? thing.ParentHolder : null) as Pawn_EquipmentTracker);
            // flag = (pawn_EquipmentTracker != null);
            __result += 0.2f;
        }
        /*else
        {
            flag = false;
        }
        bool flag2 = flag;
        if (flag2)
        {
            __result /= pawn_EquipmentTracker.pawn.GetStatValue(VFEDefOf.VEF_RangeAttackSpeedFactor, true, -1);
        }*/
    }
}

[HarmonyPatch(typeof(ShotReport), "AimOnTargetChance_IgnoringPosture", MethodType.Getter)]
public static class ShotReport_AimOnTargetChance_IgnoringPosture_Patch
{
    private static void Postfix(ref float __result, ref TargetInfo ___target)
    {
        if (___target == null) return;
        
        var pawn = ___target.Thing as Pawn;
        if (pawn != null && pawn.genes.HasGene(StagzDefOf.Stagz_KeenReflexes))
        {
            __result += pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true, -1) * 0.75f;
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