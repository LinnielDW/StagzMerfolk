using RimWorld;
using Verse;

namespace StagzMerfolk;

[DefOf]
public class StagzDefOf
{

    public static GeneDef Stagz_KeenReflexes;
    public static GeneDef Stagz_Gene_Tail_Fish;

    public static HediffDef Stagz_Dehydration;
    public static NeedDef Stagz_NeedAquatic;
    public static JobDef Stagz_HydrateAquaticJob;

    public static AbilityDef Stagz_DeepDive;

    public static BodyPartGroupDef Feet;
    public static HediffDef Stagz_Tail;

    public static JobDef Stagz_GotoWaterCell;
    public static JobDef Stagz_Wait_Hydrate;
    
    public static MentalStateDef Stagz_Charmed;
    public static MentalStateDef Stagz_VeryCharmed;

    public static LetterDef Stagz_AcceptCharmedJoiner;
}

public static class StagzCollections
{
    public static readonly MentalStateDef[] StateDefs = { StagzDefOf.Stagz_Charmed, StagzDefOf.Stagz_VeryCharmed };
}