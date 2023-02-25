using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using CombatExtended;
using HarmonyLib;
using Verse;

namespace ThrowThem;

[HarmonyPatch(typeof(Verb_ShootCEOneUse), "SelfConsume")]
public static class CombatExtendedPatch
{
    private static readonly MethodInfo ShooterPawn = AccessTools.Method(typeof(Verb_LaunchProjectileCE), "ShooterPawn");

    private static readonly MethodInfo NoPrimaryWeaponOrInventory =
        AccessTools.Method(typeof(HelperClass), "NoPrimaryWeaponOrInventory");

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var first = false;
        var second = false;
        var codes = new List<CodeInstruction>(instructions);
        var codetofind = AccessTools.Method(typeof(Thing), "Destroy");
        for (var i = 0; i < codes.Count; i++)
        {
            if (i < codes.Count - 1 && codes[i + 1].opcode == OpCodes.Callvirt &&
                codes[i + 1].operand as MethodInfo == codetofind)
            {
                yield return new CodeInstruction(OpCodes.Nop);
                first = true;
            }
            else if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand as MethodInfo == codetofind)
            {
                yield return new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(HelperClass), "RemoveOneOrDestroy"));
            }
            else if (i < codes.Count - 5 && codes[i].opcode == OpCodes.Nop && codes[i + 1].opcode == OpCodes.Nop &&
                     codes[i + 2].opcode == OpCodes.Ldloc_0)
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(codes[i].labels);
                yield return new CodeInstruction(OpCodes.Call, NoPrimaryWeaponOrInventory);
                yield return codes[i + 3];
                second = true;
            }
            else
            {
                yield return codes[i];
            }
        }

        if (!first)
        {
            Log.Error("Throw Them first transpiler failed, please send log to bug reports");
        }

        if (!second)
        {
            Log.Error("Throw Them second transpiler failed, please send log to bug reports");
        }
    }
}