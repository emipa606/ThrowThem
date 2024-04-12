using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace ThrowThem;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.TryGetAttackVerb))]
internal class TryGetAttackVerbPatch
{
    public static Verb Postfix(Verb __result, Pawn __instance)
    {
        object obj;
        if (__instance == null)
        {
            obj = null;
        }
        else
        {
            var curJob = __instance.CurJob;
            obj = curJob?.GetCachedDriverDirect;
        }

        if (obj is not JobDriver_AttackStatic)
        {
            return __result;
        }

        var inventory = __instance.inventory;

        if (inventory?.innerContainer == null)
        {
            return __result;
        }

        if (__instance.CurJob.verbToUse == __result)
        {
            return __result;
        }

        foreach (var thing in __instance.inventory.innerContainer)
        {
            var compEquippable = thing.TryGetComp<CompEquippable>();
            if (compEquippable == null)
            {
                continue;
            }

            var modTypes = ThrowThem.ModTypes;
            var primaryVerb = compEquippable.PrimaryVerb;
            Type item;
            if (primaryVerb == null)
            {
                item = null;
            }
            else
            {
                var verbProps = primaryVerb.verbProps;
                item = verbProps?.verbClass;
            }

            if (modTypes.Contains(item) && __instance.CurJob.verbToUse == compEquippable.PrimaryVerb)
            {
                return compEquippable.PrimaryVerb;
            }
        }

        return __result;
    }
}