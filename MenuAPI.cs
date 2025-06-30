using System;
using System.Collections.Generic;
using System.Linq;
using MenuLib.MonoBehavior;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

namespace MenuLib;

public class MenuAPI {
    public static MenuController MenuController;
    public static GameObject SettingPagePrefab;
    public static GameObject CategoryPagePrefab;
    public static GameObject CheckBoxPrefab;
    public static GameObject DropDownPrefab;
    public static GameObject InputFieldPrefab;
    public static GameObject SliderFieldPrefab;
    
    private class MenuEntry {
        public string name;
        public Action OnClick;
    }
    
    private class CategoryEntry: MenuEntry {
        public Action<Transform> OnBuildContent;
    }
    
    private static readonly List<MenuEntry> menuEntries = new ();
    private static readonly List<MenuEntry> pauseMenuEntries = new ();
    private static readonly List<CategoryEntry> categoryEntries = new ();
    
    public static void AddButtonToMenu(string name, Action onClick) {
        AddButtonToMainMenu(name, onClick);
        AddButtonToPauseMenu(name, onClick);
    }

    public static void AddButtonToMainMenu(string name, Action onClick) {
        menuEntries.Add(new MenuEntry { name = name, OnClick = onClick});
    }
    
    public static void AddButtonToPauseMenu(string name, Action onClick) {
        pauseMenuEntries.Add(new MenuEntry { name = name, OnClick = onClick});
    }
    
    public static void AddNewCategory(string name, Action onClick) {
        categoryEntries.Add(new CategoryEntry { name = name, OnClick = onClick});
    }
    
    public static void AddNewCategory(string name, Action<Transform> onBuildContent) {
        categoryEntries.Add(new CategoryEntry { name = name, OnBuildContent = onBuildContent });
    }

    internal static void BuildMainMenuButton(Transform parent) {
        foreach (var menuEntry in menuEntries) {
            CreateMainMenuButton(menuEntry.name, parent,menuEntry.OnClick);
        }
    }
    
    internal static void BuildPauseMenuButton(Transform parent) {
        foreach (var menuEntry in pauseMenuEntries) {
            CreatePauseMenuButton(menuEntry.name, parent,menuEntry.OnClick);
        }
    }

    internal static void BuildCategoryContent(Transform parent) {
        foreach (var categoryEntry in categoryEntries) {
            var needPanel = categoryEntry.OnBuildContent != null;
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

    public static Button CreateMainMenuButton(string text, Transform parent, Action onClick) {
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
    
    public static Button CreatePauseMenuButton(string text, Transform parent, Action onClick) {
        var container = parent.Find("Menu");
        var template = container.Find("OptionsButton").gameObject;
        
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
        
        return checkBoxObject.GetComponentInChildren<Toggle>();
    }

    public static Dropdown CreateDropDown<T>(string label, List<T> options, T option, T defaultOption, Transform container, 
        Action<T> onValueChanged, Func<T, string> toString = null) {
        var dropDownObject = Object.Instantiate(DropDownPrefab, container);
        var dropDownController = dropDownObject.GetComponent<DropDownController>();
        dropDownController.Initialize<T>(label, options, option, defaultOption, toString, onValueChanged);
        
        return dropDownObject.GetComponentInChildren<Dropdown>();
    }
    
    public static TMP_Dropdown CreateDropDown<T>(string label, T option, T defaultOption, Transform container, 
        Action<T> onValueChanged, Func<T, string> toString = null) where T : Enum {
        var dropDownObject = Object.Instantiate(DropDownPrefab, container);
        var dropDownController = dropDownObject.GetComponent<DropDownController>();
        var options = Enum.GetValues(typeof(T))
            .Cast<T>()
            .ToList();
        dropDownController.Initialize<T>(label, options, option, defaultOption, toString, onValueChanged);
        
        return dropDownObject.GetComponentInChildren<TMP_Dropdown>();
    }

    public static TMP_InputField CreateFloatInputField(string label, float value, float defaultValue, float maxValue, float minValue,
        Transform container, Action<float> onValueChanged) {
        var inputFieldObject = Object.Instantiate(InputFieldPrefab, container);
        var inputFieldController = inputFieldObject.GetComponent<InputFieldController>();
        var inputField = inputFieldObject.GetComponentInChildren<TMP_InputField>();
        
        inputFieldController.Initialize(label, value, defaultValue, maxValue, minValue, onValueChanged);
        
        return null;
    }
    
    public static TMP_InputField CreateIntInputField(string label, int value, int defaultValue, int maxValue, int minValue,
        Transform container, Action<int> onValueChanged) {
        var inputFieldObject = Object.Instantiate(InputFieldPrefab, container);
        var inputFieldController = inputFieldObject.GetComponent<InputFieldController>();
        var inputField = inputFieldObject.GetComponentInChildren<TMP_InputField>();
        
        inputFieldController.Initialize(label, value, defaultValue, maxValue, minValue, onValueChanged);
        
        return null;
    }
    
    public static Slider CreateFloatSliderField(string label, float value, float defaultValue, float minValue, float maxValue,
        Transform container, Action<float> onValueChanged) {
        var sliderFieldObject = Object.Instantiate(SliderFieldPrefab, container);
        var sliderFieldController = sliderFieldObject.GetComponent<SliderFieldController>();
        var slider = sliderFieldObject.GetComponentInChildren<Slider>();
        
        sliderFieldController.Initialize(label, value, defaultValue, minValue, maxValue, onValueChanged);
        
        return slider;
    }
    
    public static Slider CreateIntSliderField(string label, int value, int defaultValue, int minValue, int maxValue,
        Transform container, Action<int> onValueChanged) {
        var sliderFieldObject = Object.Instantiate(SliderFieldPrefab, container);
        var sliderFieldController = sliderFieldObject.GetComponent<SliderFieldController>();
        var slider = sliderFieldObject.GetComponentInChildren<Slider>();
        
        sliderFieldController.Initialize(label, value, defaultValue, minValue, maxValue, onValueChanged);
        
        return slider;
    }
}