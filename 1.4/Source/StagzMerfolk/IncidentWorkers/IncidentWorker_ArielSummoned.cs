using RimWorld;
using Verse;

namespace StagzMerfolk;

public class IncidentWorker_ArielSummoned : IncidentWorker
{
    private Pawn GeneratePawn()
    {
        return PawnGenerator.GeneratePawn(StagzDefOf.Stagz_Ariel, Faction.OfAncients);
    }
    
    public void SpawnJoiner(Map map, Pawn pawn, IntVec3 center)
    {
        GenSpawn.Spawn(pawn, center, map, WipeMode.Vanish);
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        Map map = (Map)parms.target;
        Pawn pawn = this.GeneratePawn();
        this.SpawnJoiner(map, pawn, parms.spawnCenter);
        pawn.health.AddHediff(StagzDefOf.Stagz_Mute);

        foreach (var _ in parms.controllerPawn.health.hediffSet.GetInjuriesTendable())
        {
            Medicine medicine = (Medicine)GenSpawn.Spawn(ThingDefOf.MedicineHerbal,parms.spawnCenter, map);
            TendUtility.DoTend(pawn,parms.controllerPawn, medicine);
        }

        TaggedString label = ("LetterLabelArielJoins").Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
        TaggedString taggedString = ("LetterArielJoins").Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
        PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref label, pawn);
        
        var letter = (ChoiceLetter_AcceptAriel)LetterMaker.MakeLetter(label, taggedString, StagzDefOf.Stagz_AcceptAriel, null, null);
        letter.asker = pawn;
        letter.lookTargets = new LookTargets(pawn);
        letter.requiresAliveAsker = true;
        letter.StartTimeout(60000);

        Find.LetterStack.ReceiveLetter(letter, null);
        return true;
    }
}