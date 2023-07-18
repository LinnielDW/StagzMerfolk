using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace StagzMerfolk;

public class IncidentWorker_ArielSummoned : IncidentWorker
{
    public virtual string letterlabeljoins => "LetterLabelArielJoins";
    public virtual string letterjoins => "LetterArielJoins";

    private Pawn GeneratePawn(PawnKindDef pawnKindDef)
    {
        return PawnGenerator.GeneratePawn(pawnKindDef, Faction.OfAncients);
    }

    private void SpawnJoiner(Map map, Pawn pawn, IntVec3 center)
    {
        GenSpawn.Spawn(pawn, center, map, WipeMode.Vanish);
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        //generate and spawn pawn
        Map map = (Map)parms.target;
        Pawn pawn = this.GeneratePawn(parms.pawnKind);
        this.SpawnJoiner(map, pawn, parms.spawnCenter);

        //give pawn the 'visit colony' job
        VisitColony(parms, pawn, map);

        //give pawn hediff when needed
        if (this.def.pawnHediff != null)
        {
            pawn.health.AddHediff(this.def.pawnHediff);
        }

        foreach (var _ in parms.controllerPawn.health.hediffSet.GetInjuriesTendable())
        {
            Medicine medicine = (Medicine)GenSpawn.Spawn(ThingDefOf.MedicineHerbal,parms.spawnCenter, map);
            TendUtility.DoTend(pawn,parms.controllerPawn, medicine);
        }

        TaggedString label = letterlabeljoins.Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
        TaggedString taggedString = letterjoins.Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
        PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref label, pawn);

        var letter = MakeAcceptLetter(label, taggedString);
        letter.asker = pawn;
        letter.lookTargets = new LookTargets(pawn);
        letter.requiresAliveAsker = true;
        letter.StartTimeout(60000);

        Find.LetterStack.ReceiveLetter(letter, null);
        return true;
    }

    private void VisitColony(IncidentParms parms, Pawn pawn, Map map)
    {
        var list = new List<Pawn>() { pawn };
        RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out var chillSpot);
        var lordJobVisitColony = new LordJob_VisitColony(parms.faction, chillSpot, 60000);
        LordMaker.MakeNewLord(parms.faction, lordJobVisitColony, map, list);
    }

    public virtual ChoiceLetter_AcceptCharmedJoiner MakeAcceptLetter(TaggedString label, TaggedString taggedString)
    {
        return (ChoiceLetter_AcceptAriel)LetterMaker.MakeLetter(label, taggedString, StagzDefOf.Stagz_AcceptAriel, null, null);
    }
}