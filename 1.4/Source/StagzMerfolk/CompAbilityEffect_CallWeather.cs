using HarmonyLib;
using RimWorld;
using Verse;

namespace StagzMerfolk;

public class CompAbilityEffect_CallWeather: CompAbilityEffect
{
    private new CompProperties_AbilityCallWeather Props
    {
        get
        {
            return (CompProperties_AbilityCallWeather)this.props;
        }
    }

    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        base.Apply(target, dest);
        
        //sets weather
        this.parent.pawn.Map.weatherManager.TransitionTo(Props.weatherDef);
        //forces duration
        AccessTools.FieldRefAccess<int>(typeof(WeatherDecider),"curWeatherDuration").Invoke(this.parent.pawn.Map.weatherDecider) = Props.weatherDuration;
        
        //todo: move this to translation
        Messages.Message(this.parent.pawn.LabelShort + " has called forth a thunderstorm. It will begin shortly...", this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
        
        
        if (this.Props.casterEffect != null)
        {
            Effecter effecter = this.Props.casterEffect.SpawnAttached(this.parent.pawn, this.parent.pawn.MapHeld, 1f);
            effecter.Trigger(this.parent.pawn, null, -1);
            effecter.Cleanup();
        }
    }
}
public class CompProperties_AbilityCallWeather : CompProperties_AbilityEffect
{
    public CompProperties_AbilityCallWeather()
    {
        this.compClass = typeof(CompAbilityEffect_CallWeather);
    }		
    
    public EffecterDef casterEffect;
    public WeatherDef weatherDef;
    public int weatherDuration;
}