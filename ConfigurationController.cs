using BepInEx.Configuration;

namespace DucksEveryWhere;

internal sealed class ConfigurationController
{
	private static ConfigFile? configFile;
	private static ConfigEntry<bool>? enabledEntry;
	private static ConfigEntry<int>? duckCountEntry;

	internal static bool IsEnabled => enabledEntry?.Value ?? true;
	internal static int DuckCount => duckCountEntry?.Value ?? 8;

	internal static void Initialize(ConfigFile config)
	{
		configFile = config;

		enabledEntry = configFile.Bind(
			"General",
			"Enabled",
			true,
			"Enable or disable this mod."
		);

		duckCountEntry = configFile.Bind(
			"General",
			"DuckCount",
			8,
			new ConfigDescription("Number of rubber ducks to spawn in the truck on level start.", new AcceptableValueRange<int>(1, 50))
		);

		configFile.Save();
	}

	internal static void Reload()
	{
		configFile?.Reload();
		configFile?.Save();
	}
}
