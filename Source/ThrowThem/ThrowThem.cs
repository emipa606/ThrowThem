using System;
using System.Collections.Generic;
using HarmonyLib;

namespace ThrowThem;

internal static class ThrowThem
{
    public static readonly List<Type> ModTypes =
    [
        AccessTools.TypeByName("CombatExtended.Verb_ShootCEOneUse"),
        AccessTools.TypeByName("Verb_LaunchProjectile")
    ];
}