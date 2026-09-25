# Implementation Plan: Truck Rubber Ducks

**Branch**: `001-truck-rubber-ducks` | **Date**: 2026-09-25 | **Spec**: [specs/001-truck-rubber-ducks/spec.md](file:///c:/Users/PRIDE%20CERBERO/Documents/Osmar/vida/proyectos/REPO_Mods/DucksEveryWhere/specs/001-truck-rubber-ducks/spec.md)

**Input**: Feature specification from `specs/001-truck-rubber-ducks/spec.md`

## Summary

Implement the `DucksEveryWhere` mod for R.E.P.O. to spawn 8 (configurable) rubber duck items inside the truck when starting a level. Spawning is performed solely by the host (`SemiFunc.IsMasterClientOrSingleplayer()`) to replicate across Photon without duplicates. Positioning uses `TruckSafetySpawnPoint.instance.transform.position` with randomized offset jitter and rotation to prevent physics clipping.

## Technical Context

**Language/Version**: C# 12 / `.NET Framework 4.8` (`net48`)
**Primary Dependencies**:
- BepInEx 5.4.2402
- HarmonyX 2.12.0
- Unity 2022.3.x engine assemblies (UnityEngine.CoreModule, UnityEngine.PhysicsModule)
- `Assembly-CSharp.dll` (Game binary)
- `RepoAPI` (`external/RepoAPI`) - specifically `ItemProvider`, `ItemKeysProvider`, `ItemName`, `Game/`
**Target Platform**: Windows 64-bit / R.E.P.O.
**Project Type**: BepInEx Game Mod (`.dll`)
**Constraints**:
- Host-only spawn authority
- Strict modern C# standard (§6: zero Hungarian / underscore / `s_` prefixes)
- Zero compiler warnings (`TreatWarningsAsErrors = true`)
- Scoping: `internal` by default, only `Plugin` class is `public`

## Constitution Check

*GATE: Must pass before proceeding to tasks.*

- [x] **I. RepoKit Alignment**: Fully aligned with `REPO_MODS_WORKSPACE.md` and `REPO_MODS_METHODOLOGY.md`.
- [x] **II. Host-Only Authority**: Uses `SemiFunc.RunIsLevel()` and `SemiFunc.IsMasterClientOrSingleplayer()`.
- [x] **III. Modern C# & Zero Legacy Prefixes**:
  - No leading underscores (`config`, `enabledEntry`, `duckCountEntry`).
  - No static prefixes (`instance`, `log`).
  - Scoping: `internal sealed class ConfigurationController`, `[HarmonyPatch] internal static class RoundDirector_StartRoundLogic_Patch`.
- [x] **IV. Shared Code Hygiene (RepoAPI)**: Uses `RepoAPI.Items.ItemProvider.TrySpawnByKey` via submodule compilation (`<Compile Include>`), no duplicated code.
- [x] **V. 3-Tier Testing**: Tier 3 smoke testing via Steam / r2modman debug profile.

## Project Structure

### Documentation (this feature)

```text
specs/001-truck-rubber-ducks/
├── spec.md              # Requirements and user scenarios
├── plan.md              # This technical plan
└── tasks.md             # Ordered task breakdown
```

### Source Code

```text
DucksEveryWhere/
├── DucksEveryWhere.csproj                     # Updated with RepoAPI compilation items
├── DucksEveryWherePlugin.cs                  # BepInPlugin entry point, initializes config & Harmony
├── ConfigurationController.cs                # Configuration wrapper for Enabled & DuckCount
├── Patches/
│   └── RoundDirector_StartRoundLogic_Patch.cs # Harmony postfix on RoundDirector.StartRoundLogic
└── ARCHITECTURE.md                           # Updated architectural documentation
```

## Complexity Tracking

No violations. Standard minimal architecture following RepoKit patterns.
