using System;
using System.Collections.Generic;
using MenuLib.MonoBehavior;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;
using Toggle = UnityEngine.UI.Toggle;

namespace MenuLib;

public class MenuAPI {
    public delegate void MenuBuilderDelegate(Transform parent);
    public delegate void CategoryBuilderDelegate();
    public delegate void SettingBuilderDelegate(string category);
    
    public static MenuController MenuController;
    public static GameObject SettingPagePrefab;
    public static GameObject CategoryPagePrefab;
    public static GameObject CheckBoxPrefab;
    
    // internal static MenuBuilderDelegate mainMenuBuilder;
    // internal static CategoryBuilderDelegate settingCategoryBuilder;
    // internal static SettingBuilderDelegate settingBuilder;
    
    private class MenuEntry {
        public string name;
        public Action OnClick;
    }
    
    private class CategoryEntry: MenuEntry {
        public Action<Transform> OnBuildContent;
    }
    
    private static readonly List<MenuEntry> menuEntries = new ();
    private static readonly List<CategoryEntry> categoryEntries = new ();

    public static void AddButtonToMainMenu(string name, Action onClick) {
        menuEntries.Add(new MenuEntry { name = name, OnClick = onClick });
    }
    
    public static void AddNewCategory(string name, Action onClick) {
        categoryEntries.Add(new CategoryEntry { name = name, OnClick = onClick});
    }
    
    public static void AddNewCategory(string name, Action<Transform> onBuildContent) {
        categoryEntries.Add(new CategoryEntry { name = name, OnBuildContent = onBuildContent });
    }

    internal static void BuildMenuButton(Transform parent) {
        foreach (var menuEntry in menuEntries) {
            CreateMenuButton(menuEntry.name, parent,menuEntry.OnClick);
        }
    }

    internal static void BuildCategoryContent(Transform parent) {
        foreach (var categoryEntry in categoryEntries) {
            var needPanel = categoryEntry.OnBuildContent == null;
            var button = MenuController.AddCategory(categoryEntry.name, needPanel);
            button.GetComponentInChildren<TMP_Text>().text = categoryEntry.name;
        
            button.onClick = new Button.ButtonClickedEvent();
            if (!needPanel) {
                button.onClick.AddListener(() => {
                    categoryEntry.OnClick?.Invoke();
                });
            } else {
                button.onClick.AddListener(() => {
                    MenuController.SwitchCategory(categoryEntry.name);
                });
                categoryEntry.OnBuildContent?.Invoke(MenuController.CategoryPanel[categoryEntry.name].transform);
            
                MenuController.CategoryPanel[categoryEntry.name].SetActive(false);
            }
        }
    }

    public static Button CreateMenuButton(string text, Transform parent, Action onClick) {
        var template = parent.Find("ButtonsContainer/Options").gameObject;
        
        var container = parent.Find("ButtonsContainer");
        var buttonObject = Object.Instantiate(template, container);
        buttonObject.transform.SetSiblingIndex(container.childCount - 2);
        Object.Destroy(buttonObject.GetComponent<FontLocalizer>());
        
        var button = buttonObject.GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => { onClick?.Invoke(); });
        
        button.GetComponentInChildren<TMP_Text>().text = text;

        return button;
    }
    
    public static Toggle CreateCheckBox(string label, bool state, bool defaultState, Transform container, Action<bool> onToggle) {
        var checkBoxObject = Object.Instantiate(CheckBoxPrefab, container);
        var checkBoxController = checkBoxObject.GetComponent<CheckBoxController>();
        checkBoxController.Initialize(label, state, defaultState, onToggle);
        
        return checkBoxController.GetComponent<Toggle>();
    }
}