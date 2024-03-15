using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
public static class Pawn_HealthTracker_Patches
{
    private static float spawnChance = StagzDefOf.Stagz_ArielSummoned.HasModExtension<ArielSpawnModExtension>() ? StagzDefOf.Stagz_ArielSummoned.GetModExtension<ArielSpawnModExtension>().SpawnChance : 0f;
    private static void Postfix(ref Pawn ___pawn)
    {
        if (!___pawn.Spawned)
        {
            return;
        }

        var mapTemp = ___pawn.Map;
        // if random chance AND pawn is of player faction AND pawn downed near water
        if (Rand.Chance(spawnChance) && mapTemp != null && ___pawn.Faction == Faction.OfPlayer && ___pawn.RaceProps.Humanlike && ___pawn.GetRegion().Cells.Any(c => mapTemp.terrainGrid.TerrainAt(c).IsWater))
        {
            var incidentParams = StorytellerUtility.DefaultParmsNow(StagzDefOf.Stagz_ArielSummoned.category, mapTemp);
            incidentParams.controllerPawn = ___pawn;
            incidentParams.spawnCenter = ___pawn.Position;
            incidentParams.pawnKind = StagzDefOf.Stagz_ArielSummoned.pawnKind;

            if (StagzDefOf.Stagz_ArielSummoned.Worker.CanFireNow(incidentParams) && StagzDefOf.Stagz_ArielSummoned.Worker.TryExecute(incidentParams))
            {
                incidentParams.target.StoryState.lastFireTicks[StagzDefOf.Stagz_ArielSummoned] = Find.TickManager.TicksGame;
            }
        }
    }
}