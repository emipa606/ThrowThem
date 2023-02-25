using Verse;
using Verse.AI;

namespace ThrowThem;

public static class HelperClass
{
    public static T FailOnMissingItems<T>(this T f, Pawn x, Thing t) where T : IJobEndable
    {
        f.AddEndCondition(delegate
        {
            if (!x.equipment.Contains(t) && !x.inventory.Contains(t))
            {
                return JobCondition.Incompletable;
            }

            return JobCondition.Ongoing;
        });
        return f;
    }
}