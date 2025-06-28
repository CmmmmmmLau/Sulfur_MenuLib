using System;
using MenuLib.MonoBehavior;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace MenuLib;

public class MenuAPI {
    public delegate void MenuBuilderDelegate(Transform parent);
    public delegate void CategoryBuilderDelegate(MenuController parent);
    public static MenuController menuController;
    public static GameObject SettingPagePrefab;
    public static GameObject CategoryPagePrefab;
    public static GameObject CheckBoxPrefab;
    
    internal static MenuBuilderDelegate mainMenuBuilder;
    internal static CategoryBuilderDelegate settingCategoryBuilder;

    public static void AddButtonToMainMenu(MenuBuilderDelegate menuBuilder) {
        mainMenuBuilder += menuBuilder;
    }

    public static void AddNewCategory(CategoryBuilderDelegate categoryBuilder) {
        settingCategoryBuilder += categoryBuilder;
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
    
    public static Button CreateNewCategoryButton(string text, MenuController parent, Action onClick) {
        var button = parent.AddCategory(text);
        button.GetComponentInChildren<TMP_Text>().text = text;
        
        button.onClick = new Button.ButtonClickedEvent();
        if (onClick != null) {
            button.onClick.AddListener(() => {
                onClick?.Invoke();
            });
        } else {
            button.onClick.AddListener(() => {
                parent.SwitchCategory(text);
            });
        }

        return button;
    }
}