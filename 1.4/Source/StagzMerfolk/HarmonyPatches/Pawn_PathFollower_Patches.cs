using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(Pawn_PathFollower))]
[HarmonyPatch("CostToMoveIntoCell")]
[HarmonyPatch(new Type[]
{
    typeof(Pawn),
    typeof(IntVec3)
})]
public static class Pawn_PathFollower_Patches
{
    private static void Postfix(Pawn pawn, IntVec3 c, ref int __result)
    {
        if (pawn?.genes != null && pawn.genes.GetFirstGeneOfType<Stagz_Gene_Tail_Fish>() != null && pawn.Map.terrainGrid.TerrainAt(c).IsWater)
        {
            
            // Log.Message("terrain is water: " + __result);
            //TODO: also add terrain affordance of land in there
            var movingCardinal = c.x == pawn.Position.x || c.z == pawn.Position.z;
            __result = movingCardinal ? pawn.TicksPerMoveCardinal : pawn.TicksPerMoveDiagonal;
            
            // Log.Message("after patch: " + __result);
        }
    }
}

/*
[HarmonyPatch(typeof(GenGrid))]
[HarmonyPatch("WalkableBy")]
public static class GenGrid_WalkableBy_Patches
{

    public static bool Prefix(ref bool __result, Pawn pawn, IntVec3 c)
    {
        if (pawn?.genes != null && pawn.genes.HasGene(StagzDefOf.Stagz_Tail_Fish) && pawn.Map.terrainGrid.TerrainAt(c).IsWater)
        {
            Log.Message("walkable by fishtail");
            __result = true;
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(Reachability))]
[HarmonyPatch("CanReach")]
[HarmonyPatch(new[] {typeof(IntVec3), typeof(LocalTargetInfo), typeof(PathEndMode), typeof(TraverseParms)})]
public static class Reachability_Patch
{
    public static void Postfix(TraverseParms traverseParams, LocalTargetInfo dest, ref bool __result)
    {
        if (traverseParams.pawn?.genes != null && traverseParams.pawn.genes.HasGene(StagzDefOf.Stagz_Tail_Fish) && traverseParams.pawn.Map.terrainGrid.TerrainAt(dest.Cell).IsWater)
        {
            __result = true;
            // return false;
        }

        // return true;
    }
}*/