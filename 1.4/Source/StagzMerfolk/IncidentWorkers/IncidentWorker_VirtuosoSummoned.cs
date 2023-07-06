using Verse;

namespace StagzMerfolk;

public class IncidentWorker_VirtuosoSummoned : IncidentWorker_ArielSummoned
{
    public override string letterlabeljoins =>  "LetterLabelVirtuosoJoins";
    public override string letterjoins => "LetterVirtuosoJoins";
    
    public override ChoiceLetter_AcceptCharmedJoiner MakeAcceptLetter(TaggedString label, TaggedString taggedString)
    {
        return (ChoiceLetter_AcceptCharmedJoiner)LetterMaker.MakeLetter(label, taggedString, StagzDefOf.Stagz_AcceptCharmedJoiner, null, null);
    }
}