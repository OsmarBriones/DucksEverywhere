using HarmonyLib;
using System;

namespace DucksEveryWhere.Patches
{
	[HarmonyPatch(typeof(EnemyDirector), "Start")]
	internal class ReloadOnLevelStart
	{
		static void Postfix()
		{
			if (!SemiFunc.RunIsLevel()) return;
			ConfigurationController.Reload();
		}
	}
}
