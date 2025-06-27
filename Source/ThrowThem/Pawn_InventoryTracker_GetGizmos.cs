using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ThrowThem;

[HarmonyPatch(typeof(Pawn_InventoryTracker), nameof(Pawn_InventoryTracker.GetGizmos))]
internal class Pawn_InventoryTracker_GetGizmos
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
            var commandVerbTarget = new Command_VerbTarget();
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

            commandVerbTarget.defaultDesc = $"{thing.LabelCap}: {thing.def.description.CapitalizeFirst()}";
            commandVerbTarget.icon =
                styleDef != null && styleDef.UIIcon != null ? styleDef.UIIcon : thing.def.uiIcon;
            commandVerbTarget.iconAngle = thing.def.uiIconAngle;
            commandVerbTarget.iconOffset = thing.def.uiIconOffset;
            commandVerbTarget.tutorTag = "VerbTarget";
            commandVerbTarget.verb = compEquippable.PrimaryVerb;
            commandVerbTarget.hotKey = KeyBindingDefOf.Misc4;
            if (___pawn.Faction != Faction.OfPlayer)
            {
                commandVerbTarget.Disable("CannotOrderNonControlled".Translate());
            }
            else
            {
                if (___pawn.WorkTagIsDisabled(WorkTags.Violent))
                {
                    commandVerbTarget.Disable("IsIncapableOfViolence".Translate(___pawn.LabelShort, ___pawn));
                }
                else if (!___pawn.drafter.Drafted)
                {
                    commandVerbTarget.Disable("IsNotDrafted".Translate(___pawn.LabelShort, ___pawn));
                }
                else if (compEquippable.PrimaryVerb is Verb_LaunchProjectile)
                {
                    var apparel = compEquippable.PrimaryVerb.FirstApparelPreventingShooting();
                    if (apparel != null)
                    {
                        commandVerbTarget.Disable("ApparelPreventsShooting"
                            .Translate(___pawn.Named("PAWN"), apparel.Named("APPAREL")).CapitalizeFirst());
                    }
                }
                else if (EquipmentUtility.RolePreventsFromUsing(___pawn,
                             compEquippable.PrimaryVerb.EquipmentSource, out var reason))
                {
                    commandVerbTarget.Disable(reason);
                }
            }

            yield return commandVerbTarget;
        }
    }
}