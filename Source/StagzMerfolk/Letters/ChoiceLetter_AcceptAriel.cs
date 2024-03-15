using RimWorld;
using Verse;

namespace StagzMerfolk;

public class ChoiceLetter_AcceptAriel : ChoiceLetter_AcceptCharmedJoiner
{
    public override DiaOption RejectOption()
    {
        return new DiaOption("Reject")
        {
            action = delegate
            {
                GenExplosion.DoExplosion(asker.Position, asker.Map, 4.9f, DamageDefOf.Extinguish, null, -1, -1f, 
                    SoundDefOf.Explosion_FirefoamPopper, null, null, null, ThingDefOf.Filth_FireFoam, 1f, 1, null, 
                    true, null, 0f, 1, 0f, false, null, null, null, 
                    true, 1f, 0f, true, null, 1f);
                
                asker.Kill(null);
                CompRottable comp;
                if (asker.ParentHolder is Corpse c && (comp = c.GetComp<CompRottable>()) != null)
                {
                    comp.RotProgress = (float)comp.PropsRot.TicksToDessicated;
                }
                Find.LetterStack.RemoveLetter(this);
            },
            resolveTree = true
        };
    }
}