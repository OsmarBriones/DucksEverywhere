# DucksEveryWhere

Fills the truck with rubber ducks whenever you start a match in R.E.P.O.

Only the host needs to have this mod installed — ducks are synchronized to all connected players via Photon.

## Features
- Automatically spawns rubber ducks inside the truck at the start of each level.
- Host-only authority: synchronized across multiplayer lobbies without duplicate spawns.
- Configurable duck count (default: 8) and toggle option.

## Requirements
- [BepInEx Pack for R.E.P.O.](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)

## Installation
1. Install the latest [BepInEx Pack](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/).
2. Place `DucksEveryWhere.dll` into your `BepInEx/plugins` folder (or install via r2modman / Thunderstore Mod Manager).
3. Launch the game once — the configuration file will be generated automatically inside `BepInEx/config`.

## Configuration
Settings are controlled through `BepInEx/config/com.osmar.DucksEveryWhere.cfg`:

- `Enabled` (default `true`): Enable or disable rubber duck spawning.
- `DuckCount` (default `8`, range `1-50`): Number of rubber ducks to spawn in the truck on level start.

## Credits
Developed by **com.osmar**
