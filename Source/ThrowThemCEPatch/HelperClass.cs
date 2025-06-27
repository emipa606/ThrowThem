using CombatExtended;
using Verse;

namespace ThrowThem;

public static class HelperClass
{
    public static void RemoveOneOrDestroy(ThingWithComps thing)
    {
        if (thing.stackCount > 1)
        {
            thing.stackCount--;
        }
        else
        {
            thing.Destroy();
        }
    }

    public static bool NoPrimaryWeaponOrInventory(Verb_ShootCEOneUse instance)
    {
        return equipmentCheck(instance.ShooterPawn);
    }

    private static bool equipmentCheck(Pawn pawn)
    {
        return pawn is { equipment: null } && pawn.equipment?.Primary == null;
    }
}