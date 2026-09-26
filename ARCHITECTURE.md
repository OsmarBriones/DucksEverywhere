# DucksEverywhere Architecture

This document describes the runtime structure, data flow, and design decisions for `DucksEverywhere`.

---

## High-Level Concept

1. **Trigger / Hook:** Level start lifecycle hook `RoundDirector.StartRoundLogic` (Postfix), executed once procedural level generation is fully finished.
2. **Authority / Networking:** Host-only authority (`SemiFunc.IsMasterClientOrSingleplayer()`). Instantiated room objects are automatically synchronized to all clients via Photon PUN 2.
3. **Outcome:** Spawns a configurable quantity of rubber ducks (default: 8) inside the truck scattered with randomized offset jitter and rotation.

---

## Main Data Flow

### 1. Plugin Initialization
- Entry point: `DucksEverywherePlugin.cs` (`BaseUnityPlugin`).
- Initializes `ConfigurationController.Initialize(Config)`.
- Applies Harmony patches using `Harmony.PatchAll()`.

### 2. Level Start Hook & Duck Spawning
- Patch: `Patches/RoundDirector_StartRoundLogic_Patch.cs` (`RoundDirector.StartRoundLogic` Postfix).
- Checks:
  - `ConfigurationController.IsEnabled`: exits if disabled.
  - `SemiFunc.RunIsLevel()`: ensures this is an active level (not main menu or shop).
  - `SemiFunc.IsMasterClientOrSingleplayer()`: ensures only host creates networked items.
- Locates anchor: `TruckSafetySpawnPoint.instance.transform.position`.
- Spawns ducks via `RepoAPI.Items.ItemProvider.TrySpawnByKey("Item Rubber Duck", spawnPos, spawnRot, out _)`.
- Uses horizontal random jitter (±0.6m) and elevation step to prevent physics collision explosions.

### 3. Shared Library Usage (RepoAPI)
- Uses `RepoAPI` submodule linked at `external/RepoAPI`.
- Explicitly compiles `ItemProvider.cs`, `ItemKeysProvider.cs`, `ItemName.cs`, and `Game/**/*.cs`.
- Follows §6 Modern C# coding standards (zero underscore/Hungarian prefixes).

---

## Key Design Decisions & Invariants

- **Standalone build:** Builds into a single self-contained DLL (`DucksEverywhere.dll`).
- **Multiplayer Safety:** Only the host/singleplayer instantiates items via `PhotonNetwork.InstantiateRoomObject` (under `ItemProvider.TrySpawnByKey`), preventing duplicate entities on client machines.
- **Configurable:** `DuckCount` (1-50, default 8) and `Enabled` (default true) stored in `BepInEx/config/com.osmar.DucksEverywhere.cfg`.

---

## Testing Strategy

Follows the 3-tier testing strategy in `external/RepoKit/REPO_MODS_METHODOLOGY.md` §8:
- **Tier 3 (In-game smoke test):** Launch the game via Steam or r2modman Debug profile, start a run, and verify that the rubber ducks appear in the truck floor and can be picked up.
