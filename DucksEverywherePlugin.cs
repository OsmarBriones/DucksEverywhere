using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace DucksEverywhere;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class DucksEverywherePlugin : BaseUnityPlugin
{
	public const string PluginGuid = "com.osmar.DucksEverywhere";
	public const string PluginName = "DucksEverywhere";
	public const string PluginVersion = "1.0.1";

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
