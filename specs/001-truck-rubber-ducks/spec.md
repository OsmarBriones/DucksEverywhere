# Feature Specification: Truck Rubber Ducks

**Feature Branch**: `001-truck-rubber-ducks`

**Created**: 2026-09-25

**Status**: Ready for Planning

**Input**: User description: "genera un mod a partir de la plantilla llamado DucksEveryWhere, que simplemente haga que el camion se llene de patos de goma al iniciar la partida, unos 8 patos"

## User Scenarios & Testing

### User Story 1 - Spawn Rubber Ducks in Truck on Level Start (Priority: P1)

As a player starting a level (either singleplayer or hosting multiplayer), when the match begins and the truck doors are ready, I want to find rubber ducks scattered around the truck floor so that the truck feels festive and filled with rubber ducks.

**Why this priority**: Core value of the mod. Without spawning ducks in the truck, the mod provides zero functionality.

**Independent Test**: Start a new run/level as host or singleplayer. Walk around the truck. Verify 8 rubber ducks are present and interactive.

**Acceptance Scenarios**:

1. **Given** the mod is enabled, **When** a playable match level starts and generation is completed (`RoundDirector.StartRoundLogic`), **Then** 8 rubber ducks (`Item Rubber Duck`) spawn on the truck floor near `TruckSafetySpawnPoint`.
2. **Given** a multiplayer lobby, **When** the host enters the level, **Then** only the host instantiates the ducks so that Photon replicates them to connected clients without duplicate entities.

---

### User Story 2 - Configurable Duck Count and Toggle (Priority: P2)

As a player or server host, I want to configure whether the mod is active and how many ducks spawn, so that I can customize the chaos or disable the mod without uninstalling.

**Why this priority**: Flexibility and standard mod polish.

**Independent Test**: Change `DuckCount` to 4 in `BepInEx/config/com.osmar.duckseverywhere.cfg`, start a level, and verify exactly 4 ducks appear.

**Acceptance Scenarios**:

1. **Given** `Enabled = false` in the configuration, **When** entering a level, **Then** 0 ducks are spawned.
2. **Given** `DuckCount = 12` in the configuration, **When** entering a level, **Then** 12 ducks are spawned with distributed jitter.

---

### Edge Cases

- **Non-level scenes**: When loading into the Main Menu, Lobby, or Shop, ducks MUST NOT be spawned (`SemiFunc.RunIsLevel()` check).
- **Client authority**: Non-host clients in multiplayer MUST NOT execute the spawn logic (`SemiFunc.IsMasterClientOrSingleplayer()`), preventing duplicate duck spam.
- **Physics collision explosion**: Spawning all 8 ducks at the exact same point would cause Unity physics to violently shoot ducks everywhere. Each duck MUST have slight position jitter (e.g. ±0.6m in X/Z, slightly elevated Y) and random yaw rotation.
- **Missing anchor**: If `TruckSafetySpawnPoint.instance` is unexpectedly null, the mod MUST log a warning and gracefully abort without throwing an unhandled exception or breaking level generation.

## Requirements

### Functional Requirements

- **FR-001**: System MUST spawn rubber duck items (`RepoAPI.Items.ItemName.RubberDuck` / `"Item Rubber Duck"`) inside the truck when a playable level starts.
- **FR-002**: Spawning MUST be restricted to the Master Client or singleplayer session (`SemiFunc.IsMasterClientOrSingleplayer()`).
- **FR-003**: System MUST verify that the current scene is an active game level (`SemiFunc.RunIsLevel()`).
- **FR-004**: Each duck MUST be positioned relative to `TruckSafetySpawnPoint.instance.transform.position` with randomized horizontal jitter and random rotation to avoid physics overlap.
- **FR-005**: System MUST provide a BepInEx configuration file (`ConfigurationController`) exposing `Enabled` (bool, default `true`) and `DuckCount` (int, default `8`, range 1-50).
- **FR-006**: System MUST use BepInEx logger (`Plugin.Log`) for all diagnostic output.

### Key Entities

- **ConfigurationController**: Manages config entries (`Enabled`, `DuckCount`).
- **RoundDirector_StartRoundLogic_Patch**: Harmony postfix patch on `RoundDirector.StartRoundLogic` triggering duck instantiation once level generation completes.

## Success Criteria

### Measurable Outcomes

- **SC-001**: On match start as host, exactly `DuckCount` (default 8) rubber ducks appear inside the truck.
- **SC-002**: Zero exceptions or warnings logged to BepInEx log regarding duck spawning under normal conditions.
- **SC-003**: When `Enabled` is set to false, 0 ducks spawn.
- **SC-004**: In multiplayer, only the host instantiates ducks and Photon replicates them to all clients.

## Assumptions

- `RepoAPI` submodule is available at `external/RepoAPI` and provides `ItemProvider.TrySpawnByKey`.
- Game assembly provides `TruckSafetySpawnPoint.instance` inside the truck.
- Target framework is `.NET Framework 4.8` (`net48`).
