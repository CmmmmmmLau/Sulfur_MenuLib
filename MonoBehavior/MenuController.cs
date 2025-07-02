using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;
public class MenuController: MonoBehaviour {
    [SerializeField] 
    public GameObject CategoryContainer;
    [SerializeField] 
    public GameObject SettingContainer;
    [SerializeField]
    private Button applyButton;

    public Dictionary<string, GameObject> CategoryPanel = new ();
    private GameObject currentCategoryPanel;
    
    private List<IDeferredSetting> deferredSettings = new List<IDeferredSetting>();

    private void Awake() {
        foreach (Transform child in CategoryContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
        
        foreach (Transform child in SettingContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
        
        applyButton.onClick.AddListener(ApplyDeferredSetting);
        applyButton.enabled = false;
        
        this.gameObject.SetActive(false);
    }

    private void Start() {
        MenuAPI.BuildCategoryContent(this.SettingContainer.transform);
        
        if (this.CategoryPanel.Count > 0) {
            var firstCategoryName = new List<string>(CategoryPanel.Keys)[0];
            SwitchCategory(firstCategoryName);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            this.gameObject.SetActive(false);
        }
    }

    public Button AddCategory(string category, bool needPanel = true) {
        var categoryObject = Instantiate(MenuAPI.CategoryPagePrefab, CategoryContainer.transform);
        Plugin.Logger.LogInfo($"Added category: {category}");

        if (needPanel) {
            var panel = new GameObject("Category Content");
            panel.transform.SetParent(SettingContainer.transform);
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 6;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.childScaleHeight = false;
            layout.childScaleWidth = false;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            CategoryPanel[category] = panel;
        }
        
        return categoryObject.GetComponent<Button>();
    }

    public void SwitchCategory(string category) {
        Plugin.Logger.LogInfo("Switching to category: " + category);
        currentCategoryPanel?.SetActive(false);
        if (CategoryPanel.TryGetValue(category, out var panel)) {
            currentCategoryPanel = panel;
            currentCategoryPanel.SetActive(true);
        } else {
            Plugin.Logger.LogWarning($"Category '{category}' not found.");
        }
    }
    
    public void RegisterDeferredSetting(IDeferredSetting setting) {
        if (!deferredSettings.Contains(setting)) {
            deferredSettings.Add(setting);
        }
        
        applyButton.enabled = deferredSettings.Count > 0;
    }

    public void ApplyDeferredSetting() {
        foreach (var setting in deferredSettings) {
            setting.ApplySetting();
        }
        deferredSettings.Clear();
    }
}
