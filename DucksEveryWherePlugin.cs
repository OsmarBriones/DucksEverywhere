using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace DucksEveryWhere;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class DucksEveryWherePlugin : BaseUnityPlugin
{
	public const string PluginGuid = "com.osmar.DucksEveryWhere";
	public const string PluginName = "DucksEveryWhere";
	public const string PluginVersion = "1.0.0";

	internal static ManualLogSource Log { get; private set; } = null!;
	internal Harmony? Harmony { get; private set; }

	private void Awake()
	{
		Log = base.Logger;

		// Prevent the plugin from being deleted
		gameObject.transform.parent = null;
		gameObject.hideFlags = HideFlags.HideAndDontSave;

		ConfigurationController.Initialize(Config);

		Harmony = new Harmony(PluginGuid);
		Harmony.PatchAll();

		Log.LogInfo($"{PluginName} {PluginVersion} loaded!");
	}

	internal void Unpatch()
	{
		Harmony?.UnpatchSelf();
	}
}
