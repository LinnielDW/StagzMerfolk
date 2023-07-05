using RimWorld;
using Verse;
using Verse.AI;

namespace StagzMerfolk;

public class MentalState_VeryCharmed : MentalState_Charmed
{
    public MentalState_VeryCharmed()
    {
        this.charmChance = 1f;
    }
}

public class MentalState_Charmed : MentalState
{
    public float charmChance;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look<float>(ref this.charmChance, "charmChance", 0.2f, false);
    }

    public MentalState_Charmed()
    {
        this.charmChance = 0.2f;
    }

    public override RandomSocialMode SocialModeMax()
    {
        return RandomSocialMode.Off;
    }

    public override void PostEnd()
    {
        base.PostEnd();

        pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee);
        if (pawn.RaceProps.Humanlike && Rand.Chance(charmChance))
        {
            SendAskToJoinLetter(this.pawn);
        }
    }

    public void SendAskToJoinLetter(Pawn p)
    {
        //TODO: change letter strings
        TaggedString label = "LetterLabelWandererJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
        TaggedString taggedString = "LetterWandererJoins".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);

        var letter = (AcceptJoiner_Charmed)LetterMaker.MakeLetter(label, taggedString, StagzDefOf.Stagz_AcceptCharmedJoiner, null, null);
        letter.asker = p;
        letter.lookTargets = new LookTargets(p);
        letter.requiresAliveAsker = true;
        letter.StartTimeout(60000);

        Find.LetterStack.ReceiveLetter(letter, null);
    }
}