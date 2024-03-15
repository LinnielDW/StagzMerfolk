using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk.HarmonyPatches;

[HarmonyPatch(typeof(GenHostility), "HostileTo", new[] { typeof(Thing), typeof(Thing) })]
public static class GenHostility_HostileTo_Patch
{
    private static void Postfix(Thing a, Thing b, ref bool __result)
    {
        if (a.Destroyed || b.Destroyed || a == b)
        {
            return;
        }

        if (a is Pawn { Faction: not null } aPawn && b is Pawn { Faction: not null } bPawn &&
            (aPawn.MentalState != null && StagzCollections.StateDefs.Contains(aPawn.MentalState.def) || bPawn.MentalState != null && StagzCollections.StateDefs.Contains(bPawn.MentalState.def))
           )
        {
            __result = aPawn.Faction == bPawn.Faction;
        }
    }
}

[HarmonyPatch(typeof(GenHostility), "HostileTo", new[] { typeof(Thing), typeof(Faction) })]
public static class GenHostility_HostileToFaction_Patch
{
    private static void Postfix(Thing t, Faction fac, ref bool __result)
    {
        if (t.Destroyed || fac == null)
        {
            return;
        }

        if (t is Pawn { Faction: not null, MentalState: not null } pawn && StagzCollections.StateDefs.Contains(pawn.MentalState.def))
        {
            __result = pawn.Faction == fac;
        }
    }
}