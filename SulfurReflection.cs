using System.Reflection;
using HarmonyLib;
using PerfectRandom.Sulfur.Core.UI;
using UnityEngine;

namespace MenuLib;

public static class SulfurReflection {
    public static readonly FieldInfo MENU_OBJECT = AccessTools.Field(typeof(MenuManager), "menuObject");
    public static readonly FieldInfo MENU_BUTTON_TEMPLATE = AccessTools.Field(typeof(MenuManager), "buttonPrefab");
}