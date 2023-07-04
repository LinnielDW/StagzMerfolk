using RimWorld;
using Verse;
using Verse.AI;

namespace StagzMerfolk;

public class MentalState_Charmed : MentalState
{

    public override RandomSocialMode SocialModeMax()
    {
        return RandomSocialMode.Off;
    }

    public override void PostEnd()
    {
        base.PostEnd();
        RecruitUtility.Recruit(this.pawn, Faction.OfPlayer);
    }
}