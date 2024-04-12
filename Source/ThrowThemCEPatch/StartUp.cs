using System.Reflection;
using HarmonyLib;
using Verse;

namespace ThrowThem;

[StaticConstructorOnStartup]
public static class StartUp
{
    static StartUp()
    {
        new Harmony("Mlie.ThrowThem.CEPatch").PatchAll(Assembly.GetExecutingAssembly());
    }
}