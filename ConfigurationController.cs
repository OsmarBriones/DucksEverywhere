using BepInEx.Configuration;

namespace DucksEverywhere;

internal enum DuckSpawnMode
{
	Both,
	TruckOnly,
	LevelOnly
}

internal sealed class ConfigurationController
{
	private static ConfigFile? configFile;
	private static ConfigEntry<bool>? enabledEntry;
	private static ConfigEntry<DuckSpawnMode>? spawnModeEntry;
	private static ConfigEntry<int>? truckDuckCountEntry;
	private static ConfigEntry<int>? levelDuckCountEntry;

	internal static bool IsEnabled => enabledEntry?.Value ?? true;
	internal static DuckSpawnMode SpawnMode => spawnModeEntry?.Value ?? DuckSpawnMode.Both;
	internal static int TruckDuckCount => truckDuckCountEntry?.Value ?? 15;
	internal static int DuckCount => TruckDuckCount;
	internal static int LevelDuckCount => levelDuckCountEntry?.Value ?? 30;

	internal static void Initialize(ConfigFile config)
	{
		configFile = config;

		enabledEntry = configFile.Bind(
			"General",
			"Enabled",
			true,
			"Enable or disable this mod."
		);

		spawnModeEntry = configFile.Bind(
			"General",
			"SpawnMode",
			DuckSpawnMode.Both,
			"Where rubber ducks should spawn: Both (truck and level), TruckOnly, or LevelOnly."
		);

		truckDuckCountEntry = configFile.Bind(
			"General",
			"TruckDuckCount",
			15,
			new ConfigDescription("Number of rubber ducks to spawn in the truck on level start.", new AcceptableValueRange<int>(1, 50))
		);

		levelDuckCountEntry = configFile.Bind(
			"General",
			"LevelDuckCount",
			30,
			new ConfigDescription("Number of rubber ducks to spawn across the facility rooms on level start.", new AcceptableValueRange<int>(1, 100))
		);

		configFile.Save();
	}

	internal static void Reload()
	{
		configFile?.Reload();
		configFile?.Save();
	}
}
