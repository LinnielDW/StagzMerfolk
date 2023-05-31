using Verse;

namespace StagzMerfolk;

public class HediffComp_RemovedWhenGeneRemoved : HediffComp
{
    public new HediffCompProperties_RemovedWhenGeneRemoved Props
    {
        get
        {
            return (HediffCompProperties_RemovedWhenGeneRemoved)this.props;
        }
    }

    public override void CompPostPostRemoved()
    {
        base.CompPostPostRemoved();
        {
            parent.pawn.genes.RemoveGene(parent.pawn.genes.GetGene(this.Props.geneDef));
        }
    }
}

public class HediffCompProperties_RemovedWhenGeneRemoved : HediffCompProperties
{
    public GeneDef geneDef;
    
    public HediffCompProperties_RemovedWhenGeneRemoved()
    {
        this.compClass = typeof(HediffComp_RemovedWhenGeneRemoved);
    }
}