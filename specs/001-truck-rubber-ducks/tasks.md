# Tasks: Truck Rubber Ducks

**Input**: Design documents from `specs/001-truck-rubber-ducks/` (`spec.md`, `plan.md`)
**Prerequisites**: `spec.md`, `plan.md`, `constitution.md`

## Phase 1: Setup & Foundational

**Purpose**: Project build configuration and cleanup

- [x] T001 Update `DucksEveryWhere.csproj` to compile required `RepoAPI` submodule files (`ItemProvider.cs`, `ItemKeysProvider.cs`, `ItemName.cs`, `Game/**/*.cs`).
- [x] T002 Remove template placeholder patch `Patches/ReloadOnLevelStart.cs`.

---

## Phase 2: User Story 2 - Configurable Duck Count and Toggle (Priority: P2)

**Goal**: Expose user configuration settings for mod toggle and duck count.

- [x] T003 [US2] Implement `ConfigurationController.cs` following §6 Modern C# standards (no `_`/`s_` prefixes, `internal sealed class`) exposing `Enabled` (bool) and `DuckCount` (int).

---

## Phase 3: User Story 1 - Spawn Rubber Ducks in Truck on Level Start (Priority: P1) 🎯 MVP

**Goal**: Host spawns 8 rubber ducks in truck on match start, replicated across Photon.

- [x] T004 [US1] Implement `Patches/EnemyDirector_Start_Patch.cs` hooking `EnemyDirector.Start` Postfix with host authority check, `TruckSafetySpawnPoint` positioning, jitter/rotation offsets, and `RepoAPI.Items.ItemProvider.TrySpawnByKey`.
- [x] T005 [US1] Update `Plugin.cs` to initialize `ConfigurationController`, set up BepInEx logging, and apply Harmony patches.

---

## Phase 4: Polish & Documentation

**Purpose**: Quality assurance, verification, and documentation updates per `RULE[user_global]`.

- [x] T006 Compile project via `dotnet build DucksEveryWhere.csproj` and ensure 0 warnings and 0 errors.
- [x] T007 [P] Update `ARCHITECTURE.md`, `README.md`, and `CHANGELOG.md` with feature architecture and release notes.
- [x] T008 Verify deployment of compiled `DucksEveryWhere.dll` to Steam and r2modman plugin directories.
