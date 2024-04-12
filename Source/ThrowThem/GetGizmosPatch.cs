using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ThrowThem;

[HarmonyPatch(typeof(Pawn_InventoryTracker), nameof(Pawn_InventoryTracker.GetGizmos))]
internal class GetGizmosPatch
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
            if (compEquippable == null || !ThrowThem.ModTypes.Contains(compEquippable.PrimaryVerb.verbProps.verbClass))
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