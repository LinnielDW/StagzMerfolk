using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(Building_MusicalInstrument), "StopPlaying")]
public class Building_MusicalInstrument_Patches
{
    private static float spawnChance = StagzDefOf.Stagz_VirtuosoSummoned.HasModExtension<ArielSpawnModExtension>() ? StagzDefOf.Stagz_VirtuosoSummoned.GetModExtension<ArielSpawnModExtension>().SpawnChance : 0f;

    private static void Prefix(Pawn ___currentPlayer)
    {
        if (___currentPlayer == null)
        {
            return;
        }

        var mapTemp = ___currentPlayer.Map;
        if (Rand.Chance(spawnChance) && mapTemp != null)
        {
            CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => mapTemp.reachability.CanReachColony(c) && !c.Fogged(mapTemp), mapTemp, CellFinder.EdgeRoadChance_Neutral, out var cell);

            var incidentParams = StorytellerUtility.DefaultParmsNow(StagzDefOf.Stagz_VirtuosoSummoned.category, mapTemp);
            incidentParams.spawnCenter = cell;
            incidentParams.pawnKind = StagzDefOf.Stagz_Ariel;
            incidentParams.controllerPawn = ___currentPlayer;

            if (StagzDefOf.Stagz_VirtuosoSummoned.Worker.CanFireNow(incidentParams) && StagzDefOf.Stagz_VirtuosoSummoned.Worker.TryExecute(incidentParams))
            {
                incidentParams.target.StoryState.lastFireTicks[StagzDefOf.Stagz_VirtuosoSummoned] = Find.TickManager.TicksGame;
            }
        }
    }
}