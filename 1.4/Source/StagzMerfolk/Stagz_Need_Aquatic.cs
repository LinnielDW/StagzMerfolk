using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace StagzMerfolk;

public class Stagz_Need_Aquatic: Need
{		
    public Stagz_Need_Aquatic(Pawn pawn) : base(pawn)
    {
        this.threshPercents = new List<float>
        {
            0.1f
        };
    }
    
    //TODO: add bad hygiene integration
    public override void NeedInterval()
    {
        if(pawn.IsWet())
        {
            //TODO: change numbers
            this.CurLevel += 0.0075f;
        }
        else
        {
            this.CurLevel -= 0.0003f;
        }
        
        //TODO: add dehydration
        //this.CheckForStateChange();
    }
    
    public override int GUIChangeArrow
    {
        get
        {
            if (pawn.IsWet())
            {
                return 1;
            }
            return -1;
        }
    }
}