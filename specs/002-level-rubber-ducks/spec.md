# Feature Specification: Level-Wide Rubber Ducks Spawning

**Feature Branch**: `002-level-rubber-ducks`

**Created**: 2026-09-25

**Status**: Ready for Planning

**Input**: User description: "hacer que los patos aparezcan por todo el nivel en lugar de solo al inicio del nivel... si, adelante, tambien edita los documentos y lo demas que sea relevante"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Configurable Duck Spawning Modes (Truck, Level, or Both) (Priority: P1)

As a player or lobby host, I want to configure whether rubber ducks spawn only in the truck, scattered throughout the complex/level, or in both places, so that I can explore and discover ducks across rooms and shelves instead of only in the starting truck.

**Why this priority**: Directly satisfies the user request to have ducks appear across the entire level while providing backwards compatibility for users who still prefer truck-only spawning or both simultaneously.

**Independent Test**: Set `SpawnMode = LevelOnly` or `Both` in the configuration file, start a level, and verify rubber ducks are placed on surfaces/rooms throughout the facility.

**Acceptance Scenarios**:

1. **Given** `SpawnMode = TruckOnly`, **When** the level begins, **Then** rubber ducks spawn exclusively inside the starting truck.
2. **Given** `SpawnMode = LevelOnly`, **When** the level begins, **Then** 0 ducks spawn in the truck and the configured number of level ducks spawn across generated rooms in the facility.
3. **Given** `SpawnMode = Both`, **When** the level begins, **Then** ducks spawn both inside the truck and across generated rooms in the facility.
4. **Given** a multiplayer lobby, **When** spawning ducks across the level, **Then** only the host instantiates the ducks so that Photon synchronizes them across all clients without duplicates.

---

### User Story 2 - Natural Level Placement via Room Volumes (Priority: P2)

As a player exploring the facility, I want ducks to appear in natural, believable locations (such as tables, shelves, countertops, and room floors) rather than clipped into walls or floating in unreachable voids, so that they feel like fun discoverable curiosities.

**Why this priority**: Poorly placed ducks would fall out of bounds, cause physics glitches, or be unreachable by players.

**Independent Test**: Explore 3-5 different procedural rooms in a run and verify that spawned ducks rest safely on valid room surfaces or floors without falling through the level geometry.

**Acceptance Scenarios**:

1. **Given** a generated facility with multiple rooms, **When** placing level ducks, **Then** positions are chosen from valid room object placement anchors (`ValuableVolume` or room module anchors) distributed across different rooms.
2. **Given** fewer room anchors than requested ducks, **When** placing ducks, **Then** remaining ducks are placed on available anchors or safe module centers without errors or crashes.

---

### Edge Cases

- **Facility has no valid rooms or volumes found**: If no room volumes are detected, gracefully skip or place ducks safely near room origins with floor raycasting, logging a clear warning without throwing unhandled exceptions.
- **`LevelDuckCount` is set to 0 or negative**: Clamp or handle gracefully by spawning 0 level ducks.
- **Singleplayer vs Multiplayer**: Ensure identical behavior in singleplayer and host multiplayer, with clients never attempting to instantiate items locally.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST provide a configuration option `SpawnMode` supporting `TruckOnly`, `LevelOnly`, and `Both` (defaulting to `Both` or configurable by the user).
- **FR-002**: System MUST provide a configuration option `LevelDuckCount` (integer, range 1-100, default 15) to control how many ducks spawn across the level.
- **FR-003**: System MUST retain the existing `DuckCount` configuration option (range 1-50, default 8) controlling the number of ducks spawned in the truck when truck spawning is active.
- **FR-004**: Level ducks MUST be distributed across generated rooms/modules using valid room placement anchors (`ValuableVolume`) to avoid clipping through geometry or floating in voids.
- **FR-005**: All spawning MUST execute exclusively under host authority (`SemiFunc.IsMasterClientOrSingleplayer()`) to replicate across all Photon clients without duplication.
- **FR-006**: When `Enabled = false`, NO ducks (neither truck nor level) may be spawned.

### Key Entities

- **SpawnMode**: An enumeration with values:
  - `TruckOnly`: Spawns ducks only in the truck.
  - `LevelOnly`: Spawns ducks only distributed through the facility rooms.
  - `Both`: Spawns ducks both in the truck and distributed through the facility rooms.
- **RubberDuck**: The native interactive item (`Item Rubber Duck` / `ItemName.RubberDuck`) in R.E.P.O.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: In `LevelOnly` or `Both` modes, 100% of requested level ducks (up to the number of available valid placement spots) spawn safely inside facility rooms.
- **SC-002**: Zero physics clipping explosions or out-of-bounds falls upon level start.
- **SC-003**: In multiplayer sessions, all connected clients see and can interact with the exact same ducks at the exact same coordinates.
- **SC-004**: The project compiles with 0 warnings and 0 errors under `.NET Framework 4.8`.

## Assumptions

- Ducks are spawned at level generation completion (`RoundDirector.StartRoundLogic`), ensuring all modules and volumes are loaded and positioned.
- `ValuableVolume` instances are present in procedural rooms and provide reliable 3D coordinates on tables, desks, and floors.
