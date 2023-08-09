using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StagzMerfolk;

public class CompSpawnerFromThingSetOnWater : ThingComp //CompSpawner
{
    private int ticksUntilSpawn;

    public CompProperties_SpawnerFromThingSetOnWater Props
    {
        get { return (CompProperties_SpawnerFromThingSetOnWater)props; }
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if (!respawningAfterLoad)
        {
            ticksUntilSpawn = Props.ticksToSpawn;
        }
    }

    public override void CompTick()
    {
        TickInterval(1);
    }

    public override void CompTickRare()
    {
        TickInterval(250);
    }

    private void TickInterval(int interval)
    {
        if (!parent.Spawned)
        {
            return;
        }

        if (parent is Pawn p && !p.OnWater())
        {
            return;
        }

        ticksUntilSpawn -= interval;
        CheckShouldSpawn();
    }

    private void CheckShouldSpawn()
    {
        if (ticksUntilSpawn <= 0)
        {
            ticksUntilSpawn = Props.ticksToSpawn;
            TryDoSpawn();
        }
    }

    public bool TryDoSpawn()
    {
        List<Thing> loot = Props.thingSetMakerDef.root.Generate();
        Log.Message(loot.ToStringSafeEnumerable());

        foreach (var thing in loot)
        {
            GenSpawn.Spawn(thing, parent.Position, parent.Map);
        }
        
        // Messages.Message("MessageCompSpawnerSpawnedItem".Translate(this.PropsSpawner.thingToSpawn.LabelCap), thing, MessageTypeDefOf.PositiveEvent, true);
        return true;
    }
}

public class CompProperties_SpawnerFromThingSetOnWater : CompProperties
{
    public CompProperties_SpawnerFromThingSetOnWater()
    {
        compClass = typeof(CompSpawnerFromThingSetOnWater);
    }

    public ThingSetMakerDef thingSetMakerDef;
    public int ticksToSpawn;
}