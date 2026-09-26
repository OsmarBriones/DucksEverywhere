# Implementation Plan: Level-Wide Rubber Ducks Spawning

**Branch**: `002-level-rubber-ducks` | **Date**: 2026-09-25 | **Spec**: [specs/002-level-rubber-ducks/spec.md](spec.md)

**Input**: Feature specification from `specs/002-level-rubber-ducks/spec.md`

## Summary

Expand the `DucksEverywhere` mod to allow spawning rubber ducks across the entire facility (level) in addition to or instead of just the truck. Adds a new `SpawnMode` setting (`TruckOnly`, `LevelOnly`, `Both`) and `LevelDuckCount` setting (1-100, default 15). Spawning is host-authoritative (`SemiFunc.IsMasterClientOrSingleplayer()`) and targets room `ValuableVolume` anchors for realistic placement on tables, desks, and floors without clipping.

## Technical Context

**Language/Version**: C# 12 / `.NET Framework 4.8` (`net48`)
**Primary Dependencies**:
- BepInEx 5.4.2402
- HarmonyX 2.12.0
- Unity 2022.3.x engine assemblies (UnityEngine.CoreModule, UnityEngine.PhysicsModule)
- `Assembly-CSharp.dll` (Game binary: `RoundDirector`, `TruckSafetySpawnPoint`, `ValuableVolume`, `Module`, `SemiFunc`)
- `RepoAPI` (`external/RepoAPI`): `ItemProvider`, `ItemName`
**Target Platform**: Windows 64-bit / R.E.P.O.
**Project Type**: BepInEx Game Mod (`.dll`)
**Constraints**:
- Host-only spawn authority (`SemiFunc.IsMasterClientOrSingleplayer()`)
- Strict modern C# standard (§6: zero Hungarian / underscore / `s_` prefixes)
- Zero compiler warnings (`TreatWarningsAsErrors = true`)
- Scoping: `internal` by default, only `Plugin` class is `public`

## Constitution Check

*GATE: Must pass before proceeding to tasks.*

- [x] **I. RepoKit Alignment**: Fully aligned with `REPO_MODS_WORKSPACE.md` and `REPO_MODS_METHODOLOGY.md`.
- [x] **II. Host-Only Authority**: Checked via `SemiFunc.RunIsLevel()` and `SemiFunc.IsMasterClientOrSingleplayer()`.
- [x] **III. Modern C# & Zero Legacy Prefixes**:
  - Zero leading underscores on members (`spawnModeEntry`, `levelDuckCountEntry`, `configFile`).
  - Scoping is `internal sealed` and `internal static`.
- [x] **IV. Shared Code Hygiene (RepoAPI)**: Uses `RepoAPI.Items.ItemProvider.TrySpawnByKey` via submodule compilation.
- [x] **V. 3-Tier Testing**: Tier 3 smoke testing via Steam and r2modman debug profile.

## Project Structure

### Documentation (this feature)

```text
specs/002-level-rubber-ducks/
├── spec.md              # Requirements and user scenarios
├── checklists/
│   └── requirements.md  # Quality validation checklist
├── plan.md              # This technical plan
└── tasks.md             # Ordered task breakdown
```

### Source Code Modifications

```text
DucksEverywhere/
├── ConfigurationController.cs                # Add DuckSpawnMode enum, SpawnMode and LevelDuckCount config entries
└── Patches/
    └── RoundDirector_StartRoundLogic_Patch.cs # Add level-wide duck distribution logic via ValuableVolume anchors
```
