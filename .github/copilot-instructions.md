# GitHub Copilot Instructions for "Throw Them (Continued)" RimWorld Mod

## Mod Overview and Purpose

"Throw Them (Continued)" is an updated version of the original mod by busted_bunny. This mod enhances the usability of grenades in combat scenarios by providing an intuitive interface for selecting and using grenades directly from a pawn's inventory, streamlining the process and reducing the need for multiple clicks to swap weapons. The purpose of this mod is to make grenades an effective and convenient tool in combat, aligning with the slogan: "MAKE GRENADES GREAT AGAIN!"

## Key Features and Systems

- **Gizmo Button for Grenades**: The mod introduces a simple gizmo (button) for each type of grenade in a pawn's inventory. These buttons are available when the pawn is drafted, allowing them to throw grenades without needing to equip them first.

- **Compatibility**: The mod is designed to work seamlessly with other mods, particularly those that allow items to be picked up as RimWorld's vanilla game lacks inventory management features. Mods like "Pick Up And Haul" or "Combat Extended" are recommended.

- **Safe Loading/Unloading**: The mod can be added or removed from existing save games without causing issues, ensuring a smooth experience when managing mod lists.

- **Error Reporting and Suggestions**: Users are encouraged to report bugs and provide feedback for further improvements. Detailed steps for reproducing bugs are appreciated.

## Coding Patterns and Conventions

- **Project Structure**: The source code is organized across multiple classes, each responsible for specific functionalities. This facilitates modular and maintainable code.
  - Example classes include `HelperClass`, `JobDriver_AttackStatic_MakeNewToils`, and `Pawn_InventoryTracker_GetGizmos`.

- **Namespace and Class Naming**: Classes are named descriptively to reflect their purpose (e.g., `StartUp`, `ThrowThem`, and `Pawn_TryGetAttackVerb`).

- **.NET Framework**: The project targets .NET Framework version 4.7.2 and 4.8. 

- **Access Modifiers**: Classes are marked `internal` where appropriate to encapsulate functionality and prevent unwanted external access, improving code safety and clarity.

## XML Integration

- While the primary mod functionality is implemented in C#, XML files may be used for defining custom game data or mod configuration. Ensure these files are properly formatted and used for appropriate tasks, such as defining gizmos or patching game data.

## Harmony Patching

- Harmony is employed to modify and extend RimWorld's existing functionality without altering the original game files. This method ensures compatibility with updates and other mods.
  - Consider patching relevant methods to integrate grenade functionality seamlessly.
  - Patches should be clearly documented and named to reflect their purpose.

## Suggestions for Copilot

- **Code Suggestions**: Copilot can assist with generating boilerplate code for new classes or methods based on existing patterns. It can help maintain consistency with naming conventions and design patterns.

- **Error Handling**: Utilize Copilot for generating common error handling patterns or logging mechanisms to streamline debugging and improve user feedback.

- **XML Support**: Copilot can suggest XML structure and syntax, especially when integrating game objects or configuring mod settings.

- **Harmony Patch Templates**: Create templates for common Harmony patches, allowing Copilot to expedite the development process by suggesting these patterns.

By adhering to these guidelines and utilizing GitHub Copilot as a tool, you can efficiently enhance and maintain the "Throw Them (Continued)" mod, contributing to a richer gameplay experience in RimWorld.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
