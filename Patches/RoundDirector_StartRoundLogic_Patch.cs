using HarmonyLib;
using RepoAPI.Items;
using UnityEngine;

namespace DucksEveryWhere.Patches;

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

		var truckPoint = TruckSafetySpawnPoint.instance ?? Object.FindObjectOfType<TruckSafetySpawnPoint>();
		if (truckPoint == null)
		{
			DucksEveryWherePlugin.Log.LogWarning("TruckSafetySpawnPoint instance not found; skipping rubber duck spawning.");
			return;
		}

		var center = truckPoint.transform.position;
		var duckCount = ConfigurationController.DuckCount;

		DucksEveryWherePlugin.Log.LogInfo($"Spawning {duckCount} rubber ducks in the truck...");

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
				DucksEveryWherePlugin.Log.LogWarning($"Failed to spawn rubber duck index {i}.");
			}
		}

		DucksEveryWherePlugin.Log.LogInfo($"Finished spawning {duckCount} rubber ducks in the truck.");
	}
}
