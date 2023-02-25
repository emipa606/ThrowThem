using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ThrowThem;

internal class ThrowThem
{
    private static readonly List<Type> ModTypes = new List<Type>
    {
        AccessTools.TypeByName("CombatExtended.Verb_ShootCEOneUse"),
        AccessTools.TypeByName("Verb_LaunchProjectile")
    };

    [HarmonyPatch(typeof(Pawn_InventoryTracker), "GetGizmos")]
    private class GetGizmosPatch
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, ThingOwner<Thing> ___innerContainer,
            Pawn ___pawn)
        {
            foreach (var gizmo in __result)
            {
                yield return gizmo;
            }

            foreach (var thing in ___innerContainer)
            {
                var compEquippable = thing.TryGetComp<CompEquippable>();
                if (compEquippable == null || !ModTypes.Contains(compEquippable.PrimaryVerb.verbProps.verbClass))
                {
                    continue;
                }

                compEquippable.PrimaryVerb.caster = ___pawn;
                var command_VerbTarget = new Command_VerbTarget();
                var styleDef = thing.StyleDef;
                var icon = styleDef?.UIIcon;
                if (icon == null)
                {
                    icon = thing.def.uiIcon;
                }

                if (icon == null)
                {
                    icon = (Texture2D)thing.Graphic.ExtractInnerGraphicFor(thing)
                        .MatAt(thing.def.defaultPlacingRot).mainTexture;
                }

                if (icon == null)
                {
                    continue;
                }

                command_VerbTarget.defaultDesc = $"{thing.LabelCap}: {thing.def.description.CapitalizeFirst()}";
                command_VerbTarget.icon =
                    styleDef != null && styleDef.UIIcon != null ? styleDef.UIIcon : thing.def.uiIcon;
                command_VerbTarget.iconAngle = thing.def.uiIconAngle;
                command_VerbTarget.iconOffset = thing.def.uiIconOffset;
                command_VerbTarget.tutorTag = "VerbTarget";
                command_VerbTarget.verb = compEquippable.PrimaryVerb;
                command_VerbTarget.hotKey = KeyBindingDefOf.Misc4;
                if (___pawn.Faction != Faction.OfPlayer)
                {
                    command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
                }
                else
                {
                    if (___pawn.WorkTagIsDisabled(WorkTags.Violent))
                    {
                        command_VerbTarget.Disable("IsIncapableOfViolence".Translate(___pawn.LabelShort, ___pawn));
                    }
                    else if (!___pawn.drafter.Drafted)
                    {
                        command_VerbTarget.Disable("IsNotDrafted".Translate(___pawn.LabelShort, ___pawn));
                    }
                    else if (compEquippable.PrimaryVerb is Verb_LaunchProjectile)
                    {
                        var apparel = compEquippable.PrimaryVerb.FirstApparelPreventingShooting();
                        if (apparel != null)
                        {
                            command_VerbTarget.Disable("ApparelPreventsShooting"
                                .Translate(___pawn.Named("PAWN"), apparel.Named("APPAREL")).CapitalizeFirst());
                        }
                    }
                    else if (EquipmentUtility.RolePreventsFromUsing(___pawn,
                                 compEquippable.PrimaryVerb.EquipmentSource, out var reason))
                    {
                        command_VerbTarget.Disable(reason);
                    }
                }

                yield return command_VerbTarget;
            }
        }
    }

    [HarmonyPatch(typeof(Pawn), "TryGetAttackVerb")]
    private class TryGetAttackVerbPatch
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

                var modTypes = ModTypes;
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

    [HarmonyPatch(typeof(JobDriver_AttackStatic), "MakeNewToils")]
    private class AttackStaticPatch
    {
        public static void Prefix(JobDriver_AttackStatic __instance)
        {
            var pawn = __instance.pawn;
            var curJob = pawn.CurJob;
            var verbToUse = curJob.verbToUse;
            if (verbToUse?.EquipmentSource != null)
            {
                __instance.FailOnMissingItems(pawn, pawn.CurJob.verbToUse.EquipmentSource);
            }
        }
    }
}