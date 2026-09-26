using HarmonyLib;
using RepoAPI.Items;
using UnityEngine;

namespace DucksEverywhere.Patches;

[HarmonyPatch(typeof(RoundDirector), "StartRoundLogic")]
internal static class RoundDirector_StartRoundLogic_Patch
{
	[HarmonyPostfix]
	private static void Postfix()
	{
		if (!ConfigurationController.IsEnabled)
			return;

		if (!SemiFunc.RunIsLevel())
			return;

		if (!SemiFunc.IsMasterClientOrSingleplayer())
			return;

		var mode = ConfigurationController.SpawnMode;

		if (mode is DuckSpawnMode.TruckOnly or DuckSpawnMode.Both)
		{
			SpawnTruckDucks(ConfigurationController.TruckDuckCount);
		}

		if (mode is DuckSpawnMode.LevelOnly or DuckSpawnMode.Both)
		{
			SpawnLevelDucks(ConfigurationController.LevelDuckCount);
		}
	}

	private static void SpawnTruckDucks(int duckCount)
	{
		var truckPoint = TruckSafetySpawnPoint.instance ?? Object.FindObjectOfType<TruckSafetySpawnPoint>();
		if (truckPoint == null)
		{
			DucksEverywherePlugin.Log.LogWarning("TruckSafetySpawnPoint instance not found; skipping truck rubber duck spawning.");
			return;
		}

		var center = truckPoint.transform.position;
		DucksEverywherePlugin.Log.LogInfo($"Spawning {duckCount} rubber ducks in the truck...");

		for (var i = 0; i < duckCount; i++)
		{
			var offset = new Vector3(
				Random.Range(-0.6f, 0.6f),
				0.1f + (i * 0.05f),
				Random.Range(-0.6f, 0.6f)
			);
			var spawnPos = center + offset;
			var spawnRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

			if (!ItemProvider.TrySpawnByKey(ItemName.RubberDuck.GetGameKey(), spawnPos, spawnRot, out _))
			{
				DucksEverywherePlugin.Log.LogWarning($"Failed to spawn truck rubber duck index {i}.");
			}
		}

		DucksEverywherePlugin.Log.LogInfo($"Finished spawning {duckCount} rubber ducks in the truck.");
	}

	private static void SpawnLevelDucks(int duckCount)
	{
		if (duckCount <= 0)
			return;

		DucksEverywherePlugin.Log.LogInfo($"Spawning {duckCount} rubber ducks across facility rooms...");

		var volumes = Object.FindObjectsOfType<ValuableVolume>(includeInactive: false);
		if (volumes != null && volumes.Length > 0)
		{
			var volumeList = new System.Collections.Generic.List<ValuableVolume>(volumes);
			for (var i = volumeList.Count - 1; i > 0; i--)
			{
				var j = Random.Range(0, i + 1);
				(volumeList[i], volumeList[j]) = (volumeList[j], volumeList[i]);
			}

			for (var i = 0; i < duckCount; i++)
			{
				var targetVolume = volumeList[i % volumeList.Count];
				var spawnPos = targetVolume.transform.position + Vector3.up * 0.15f;
				if (i >= volumeList.Count)
				{
					spawnPos += new Vector3(Random.Range(-0.2f, 0.2f), 0.05f, Random.Range(-0.2f, 0.2f));
				}

				var spawnRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
				if (!ItemProvider.TrySpawnByKey(ItemName.RubberDuck.GetGameKey(), spawnPos, spawnRot, out _))
				{
					DucksEverywherePlugin.Log.LogWarning($"Failed to spawn level rubber duck index {i}.");
				}
			}

			DucksEverywherePlugin.Log.LogInfo($"Finished spawning {duckCount} rubber ducks across facility rooms.");
			return;
		}

		// Fallback to room modules if no ValuableVolumes were found
		var modules = Object.FindObjectsOfType<Module>();
		if (modules != null && modules.Length > 0)
		{
			for (var i = 0; i < duckCount; i++)
			{
				var targetModule = modules[i % modules.Length];
				var spawnPos = targetModule.transform.position + new Vector3(
					Random.Range(-1.5f, 1.5f),
					0.5f,
					Random.Range(-1.5f, 1.5f)
				);
				var spawnRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

				if (!ItemProvider.TrySpawnByKey(ItemName.RubberDuck.GetGameKey(), spawnPos, spawnRot, out _))
				{
					DucksEverywherePlugin.Log.LogWarning($"Failed to spawn fallback level rubber duck index {i}.");
				}
			}

			DucksEverywherePlugin.Log.LogInfo($"Finished spawning {duckCount} fallback level rubber ducks across room modules.");
			return;
		}

		DucksEverywherePlugin.Log.LogWarning("No ValuableVolume or Module instances found; skipping level rubber duck spawning.");
	}
}
