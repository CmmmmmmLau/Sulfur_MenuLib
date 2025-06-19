using System;
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

    public static MenuManager MenuManager;
    
    internal static MenuBuilderDelegate mainMenuBuilder;

    public static void AddButtonToMainMenu(MenuBuilderDelegate menuBuilder) {
        mainMenuBuilder += menuBuilder;
    }

    public static GameObject CreateMenuButton(string text, Transform parent, Action onClick) {
        var template = parent.Find("ButtonsContainer/Options").gameObject;
        
        var container = parent.Find("ButtonsContainer");
        var buttonObject = Object.Instantiate(template, container);
        buttonObject.transform.SetSiblingIndex(container.childCount - 2);
        Object.Destroy(buttonObject.GetComponent<FontLocalizer>());
        
        var button = buttonObject.GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => { onClick?.Invoke(); });
        
        button.GetComponentInChildren<TMP_Text>().text = text;

        return buttonObject;
    }
}