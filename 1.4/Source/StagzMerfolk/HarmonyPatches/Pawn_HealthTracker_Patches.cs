using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
public static class Pawn_HealthTracker_Patches
{
    private static readonly float spawnChance = 1f;

    private static void Postfix(ref Pawn ___pawn)
    {
        if (!___pawn.Spawned)
        {
            return;
        }

        var mapTemp = ___pawn.Map;
        if (Rand.Chance(spawnChance) && mapTemp != null && ___pawn.GetRegion().Cells.Any(c => mapTemp.terrainGrid.TerrainAt(c).IsWater))
        {
            var incidentParams = StorytellerUtility.DefaultParmsNow(StagzDefOf.Stagz_ArielSummoned.category, mapTemp);
            incidentParams.controllerPawn = ___pawn;
            incidentParams.spawnCenter = ___pawn.Position;

            if (StagzDefOf.Stagz_ArielSummoned.Worker.CanFireNow(incidentParams) && StagzDefOf.Stagz_ArielSummoned.Worker.TryExecute(incidentParams))
            {
                incidentParams.target.StoryState.lastFireTicks[StagzDefOf.Stagz_ArielSummoned] = Find.TickManager.TicksGame;
            }
        }
    }
}