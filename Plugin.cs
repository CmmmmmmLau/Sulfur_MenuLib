using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core.UI;
using UnityEngine;

namespace MenuLib;

[BepInDependency("MegaPiggy.EnumUtils", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
public class Plugin : BaseUnityPlugin {
    private const string MOD_GUID = "cmmmmmm.menulib";
    private const string MOD_NAME = "MenuLib";
    private const string MOD_VERSION = "1.0.0";

    internal static new ManualLogSource Logger;

    private static void MainMenuHook(Action<MenuManager> orig, MenuManager self) {
        orig.Invoke(self);
        MenuAPI.MenuManager = self;
        var menuObject = AccessTools.Field(typeof(MenuManager), "menuObject").GetValue(self) as GameObject;
        MenuAPI.mainMenuBuilder?.Invoke(menuObject.transform);
    }
    
    private void Awake() {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        new Hook(AccessTools.Method(typeof(MenuManager), "Start"), MainMenuHook);
        
        Example.AddButtonToMainMenu();
    }
}