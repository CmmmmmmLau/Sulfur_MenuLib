using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MenuLib.MonoBehavior;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib;

[BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
public class Plugin : BaseUnityPlugin {
    private const string MOD_GUID = "cmmmmmm.menulib";
    private const string MOD_NAME = "MenuLib";
    private const string MOD_VERSION = "0.0.1";
    
    internal static AssetBundle ab;

    internal static new ManualLogSource Logger;

    private static void MainMenuHook(Action<MenuManager> orig, MenuManager self) {
        orig.Invoke(self);
        var menuObject = AccessTools.Field(typeof(MenuManager), "menuObject").GetValue(self) as GameObject;
        MenuAPI.BuildMainMenuButton(menuObject.transform);
    }

    private static void PauseMenuHook(Action<UIManager> orig, UIManager self) {
        orig.Invoke(self);
        MenuAPI.BuildPauseMenuButton(self.PauseMenu.transform);
    }
    
    private void Awake() {
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        new Hook(AccessTools.Method(typeof(MenuManager), "Start"), MainMenuHook);
        new Hook(AccessTools.Method(typeof(UIManager), "Awake"), PauseMenuHook);
        
        ab = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("MenuLib.AssetBundle.menulib"));
        MenuAPI.SettingPagePrefab = ab.LoadAsset<GameObject>("MenuContainer");
        MenuAPI.CheckBoxPrefab = ab.LoadAsset<GameObject>("BoolCheckBox");
        MenuAPI.CategoryPagePrefab = ab.LoadAsset<GameObject>("CategoryItem");
        MenuAPI.DropDownPrefab = ab.LoadAsset<GameObject>("EnumDropdown");
        MenuAPI.InputFieldPrefab = ab.LoadAsset<GameObject>("NumberInputField");
        MenuAPI.SliderFieldPrefab = ab.LoadAsset<GameObject>("SliderField");
        MenuAPI.ButtonPrefab = ab.LoadAsset<GameObject>("ButtonItem");
        MenuAPI.ColorPickerPrefab = ab.LoadAsset<GameObject>("ColorPicker");
        
        var menulib = new GameObject("MenuLib");
        menulib.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(menulib);
        var canvas = new GameObject("Canvas").AddComponent<Canvas>();
        canvas.gameObject.AddComponent<GraphicRaycaster>();
        canvas.transform.SetParent(menulib.transform, false);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 2;
        
        var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1f;
        scaler.referencePixelsPerUnit = 100f;
        
        var settingPage = Instantiate(MenuAPI.SettingPagePrefab, canvas.transform);
        MenuAPI.MenuController = settingPage.GetComponent<MenuController>();
        
        MenuAPI.AddButtonToMenu("Mod", () => {
            settingPage.SetActive(!settingPage.activeSelf);
        });

#if DEBUG
        Example.AddExample();
#endif
    }
}