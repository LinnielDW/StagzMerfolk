using RimWorld;
using Verse;

namespace StagzMerfolk;

public class IncidentWorker_ArielSummoned : IncidentWorker
{
    public virtual string letterlabeljoins => "LetterLabelArielJoins";
    public virtual string letterjoins => "LetterArielJoins";
    
    private Pawn GeneratePawn(PawnKindDef pawnKindDef)
    {
        return PawnGenerator.GeneratePawn(pawnKindDef, Faction.OfAncients);
    }
    
    public void SpawnJoiner(Map map, Pawn pawn, IntVec3 center)
    {
        GenSpawn.Spawn(pawn, center, map, WipeMode.Vanish);
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        Map map = (Map)parms.target;
        Pawn pawn = this.GeneratePawn(parms.pawnKind);
        this.SpawnJoiner(map, pawn, parms.spawnCenter);
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

    public virtual ChoiceLetter_AcceptCharmedJoiner MakeAcceptLetter(TaggedString label, TaggedString taggedString)
    {
        return (ChoiceLetter_AcceptAriel)LetterMaker.MakeLetter(label, taggedString, StagzDefOf.Stagz_AcceptAriel, null, null);
    }
}