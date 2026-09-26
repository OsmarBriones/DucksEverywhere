# DucksEverywhere

Spawns rubber ducks inside the starting truck and across facility rooms whenever you start a match in R.E.P.O.

**Only Host, clients don't need it.** Connected players see and interact with ducks automatically.

## Features
- Automatically spawns rubber ducks on level start.
- **Multiple spawn locations**: choose between the truck, throughout facility rooms, or both!
- **Only Host, clients don't need it**: ducks synchronize across all players automatically with no extra install required for clients.
- Highly configurable.

## Requirements
- [BepInEx Pack for R.E.P.O.](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)

## Installation
1. Install the latest [BepInEx Pack](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/).
2. Place `DucksEverywhere.dll` into your `BepInEx/plugins` folder (or install via r2modman / Thunderstore Mod Manager).
3. Launch the game once — the configuration file will be generated automatically inside `BepInEx/config`.

## Configuration
Settings are controlled through `BepInEx/config/com.osmar.DucksEverywhere.cfg`:

- `Enabled` (default `true`): Enable or disable rubber duck spawning.
- `SpawnMode` (default `Both`): Where rubber ducks should spawn: `Both`, `TruckOnly`, or `LevelOnly`.
- `TruckDuckCount` (default `15`, range `1-50`): Number of rubber ducks to spawn in the truck when truck spawning is active.
- `LevelDuckCount` (default `30`, range `1-100`): Number of rubber ducks to spawn across facility rooms when level spawning is active.

## Issues & Bug Reports
Please do **not** contact the developer directly or personally for bug reports or feature requests.

The official way to report issues, suggest improvements, or submit feedback is by opening an issue on the official GitHub repository:
👉 [GitHub Issues](https://github.com/OsmarBriones/DucksEverywhere/issues)

## Credits
Developed by **Osmar Briones**
