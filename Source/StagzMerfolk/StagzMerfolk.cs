using System.Reflection;
using HarmonyLib;
using Verse;

namespace StagzMerfolk
{
    [StaticConstructorOnStartup]
    public static class StagzMerfolk
    {
        static StagzMerfolk()
        {
            var harmony = new Harmony("com.arquebus.rimworld.mod.stagzmerfolk");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
