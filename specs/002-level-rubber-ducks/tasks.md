# Tasks: Level-Wide Rubber Ducks Spawning

**Input**: Design documents from `specs/002-level-rubber-ducks/` (`spec.md`, `plan.md`)
**Prerequisites**: `spec.md`, `plan.md`, `constitution.md`

## Phase 1: Configuration Support (Priority: P1)

- [x] T001 Update `ConfigurationController.cs` to add `DuckSpawnMode` enum (`TruckOnly`, `LevelOnly`, `Both`), bind `SpawnMode` (default `Both`), and bind `LevelDuckCount` (default 15, range 1-100).

---

## Phase 2: Spawning Logic Implementation (Priority: P1)

- [x] T002 Update `Patches/RoundDirector_StartRoundLogic_Patch.cs` to support `DuckSpawnMode`:
  - When mode includes Truck (`TruckOnly` or `Both`), spawn ducks at `TruckSafetySpawnPoint`.
  - When mode includes Level (`LevelOnly` or `Both`), discover `ValuableVolume` room anchors, randomize/distribute positions, and spawn up to `LevelDuckCount` ducks on room surfaces.
  - Implement graceful fallback to `Module` transforms if no `ValuableVolume` instances are present.

---

## Phase 3: Build Verification & Deployment (Priority: P1)

- [x] T003 Compile project via `dotnet build DucksEverywhere.sln -c Release` and ensure 0 warnings and 0 errors.
- [x] T004 Verify deployment of compiled `DucksEverywhere.dll` to Steam and r2modman debug plugins directories, and confirm `ts_build` packaging.

---

## Phase 4: Documentation & Polish (Priority: P1)

- [x] T005 Update `README.md` with new `SpawnMode` and `LevelDuckCount` configuration settings and level-wide feature description.
- [x] T006 Update `ARCHITECTURE.md` with updated data flow, components, and design invariants.
- [x] T007 Update `CHANGELOG.md` with version 1.1.0 changes.
