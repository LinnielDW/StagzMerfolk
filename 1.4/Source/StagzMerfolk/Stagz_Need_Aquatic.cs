using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

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
        
        if (this.CurLevel < 0.1f)
        {
            //TODO: finalize dehydration tick severity increase
            HealthUtility.AdjustSeverity(this.pawn, StagzDefOf.Stagz_Dehydration, 0.0075f);
        }
        else
        {
            var dehydrationHediff = pawn.health.hediffSet.GetFirstHediffOfDef(StagzDefOf.Stagz_Dehydration);
            if (dehydrationHediff != null)
            {
                dehydrationHediff.Severity -= 0.15f;
            }
        }
        
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
    public void Hydrate(float val)
    {
        this.CurLevel = Mathf.Min(CurLevel + val, 1f);
    }
}