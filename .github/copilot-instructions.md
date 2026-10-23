# GitHub Copilot Instructions for Throw Them (Continued) Mod

Welcome to the Throw Them (Continued) mod for RimWorld! This mod enhances combat by allowing pawns to throw grenades without having to equip them first. The mod adds a gizmo (button) on pawns for every type of grenade in their inventory, making it easier and faster to use grenades in combat. Below are detailed instructions and guidelines to help you effectively use GitHub Copilot for this mod.

## Mod Overview and Purpose

**Overview:**  
The Throw Them (Continued) mod is an update to the original `busted_bunnys` mod, which was designed to improve the grenade usage experience in RimWorld. It allows pawns to throw grenades directly from their inventory without equipping them, streamlining the combat process.

**Purpose:**  
The primary purpose of this mod is to make grenades more accessible and efficient in combat. By adding a gizmo for each grenade in a pawn's inventory, players can quickly order pawns to throw grenades without the need for multiple clicks to swap weapons. This enhances the combat experience and makes grenades a more versatile tool.

## Key Features and Systems

1. **Gizmo for Grenades:**  
   The mod adds a gizmo (button) for each grenade in a pawn's inventory. This allows players to easily order pawns to throw grenades without equipping them.

2. **Drafted Pawns:**  
   When a pawn is drafted, players can issue an order to throw a grenade. This works similarly to using an equipped weapon but without the need to swap weapons.

3. **Compatibility:**  
   The mod is compatible with any other mod, including Combat Extended, and can be added or removed midgame without issues.

4. **XML Integration:**  
   The mod uses XML definitions to integrate with RimWorld's system. The `About.xml` file specifies dependencies, while the `News.xml` file contains update information.

5. **Harmony Patching:**  
   The mod uses Harmony patching to modify vanilla RimWorld behavior. This includes prefixing and postfixing methods to add custom functionality.

## Coding Patterns and Conventions

1. **C# Files:**  
   - `HelperClass.cs`: Contains utility methods, such as `FailOnMissingItems`.
   - `Verb_ShootCEOneUse_SelfConsume.cs`: Patches methods related to Combat Extended's grenades.
   - `JobDriver_AttackStatic_MakeNewToils.cs`: Prefixes methods to modify attack behavior.
   - `Pawn_InventoryTracker_GetGizmos.cs`: Postfixes methods to add grenade gizmos to pawns.
   - `Pawn_TryGetAttackVerb.cs`: Postfixes methods to modify how pawns get attack verbs.

2. **XML Files:**  
   - `About.xml`: Contains metadata about the mod, including dependencies.
   - `News.xml`: Contains update information and features.

3. **Naming Conventions:**  
   - Use descriptive and consistent naming for classes, methods, and variables.
   - Prefix methods with `Prefix` or `Postfix` when using Harmony patching.

## XML Integration

The mod uses XML definitions to integrate with RimWorld's system. Below are the key XML files and their purposes:

### `About.xml`

xml
<About>
  <modIdentifier>Mlie.ThrowThem</modIdentifier>
  <modName>Throw Them (Continued)</modName>
  <version>1.0.0</version>
  <dependencies>
    <dependency>brrainz.harmony</dependency>
  </dependencies>
</About>


**Purpose:**  
This file contains metadata about the mod, including its identifier, name, version, and dependencies.

### `News.xml`

xml
<Defs>
  <HugsLib.UpdateFeatureDef>
    <defName>ThrowThem</defName>
    <updateText>
      <![CDATA[
      <h2>Throw Them (Continued) - Version 1.0.0</h2>
      <p>Improves grenade usage by allowing pawns to throw grenades directly from their inventory.</p>
      ]]>
    </updateText>
  </HugsLib.UpdateFeatureDef>
</Defs>


**Purpose:**  
This file contains update information and features for the mod, which is displayed to users through the HugsLib update system.

## Harmony Patching

The mod uses Harmony patching to modify vanilla RimWorld behavior. Below are examples of Harmony patches used in the mod:

### `Verb_ShootCEOneUse_SelfConsume.cs`

csharp
using HarmonyLib;
using Verse;

namespace ThrowThemCEPatch
{
    [HarmonyPatch(typeof(Verb_ShootCEOneUse_SelfConsume), "Transpiler")]
    public static class Verb_ShootCEOneUse_SelfConsume_Transpiler
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Add custom transpiler logic here
        }
    }
}


**Purpose:**  
This patch modifies the `Transpiler` method of `Verb_ShootCEOneUse_SelfConsume` to add custom functionality related to Combat Extended's grenades.

### `Pawn_InventoryTracker_GetGizmos.cs`

csharp
using HarmonyLib;
using Verse;

namespace ThrowThem
{
    [HarmonyPatch(typeof(Pawn_InventoryTracker), "GetGizmos")]
    public static class Pawn_InventoryTracker_GetGizmos_Postfix
    {
        public static void Postfix(Pawn_InventoryTracker __instance, ref IEnumerable<Gizmo> __result)
        {
            // Add custom gizmo logic here
        }
    }
}


**Purpose:**  
This patch modifies the `GetGizmos` method of `Pawn_InventoryTracker` to add grenade gizmos to pawns.

## Suggestions for Copilot

1. **Use Descriptive Naming:**  
   When using GitHub Copilot, ensure that class, method, and variable names are descriptive and consistent with the mod's coding patterns.

2. **Follow Harmony Patching Conventions:**  
   When generating Harmony patches, ensure that methods are prefixed with `Prefix` or `Postfix` to indicate their role in the patching process.

3. **Integrate with XML Definitions:**  
   When adding new features, ensure that they are integrated with the mod's XML definitions to maintain consistency and functionality.

4. **Test and Validate:**  
   After generating code with GitHub Copilot, thoroughly test the changes to ensure they work as expected and do not introduce new bugs.

By following these guidelines, you can effectively use GitHub Copilot to enhance the Throw Them (Continued) mod and improve the grenade usage experience in RimWorld.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

