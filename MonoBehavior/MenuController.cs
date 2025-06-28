using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;
public class MenuController: MonoBehaviour {
    [SerializeField] 
    public GameObject CategoryContainer;
    [SerializeField] 
    public GameObject SettingContainer;
    
    public Dictionary<string, List<GameObject>> CategoryItems = new ();

    private void Awake() {
        foreach (Transform child in CategoryContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
        
        foreach (Transform child in SettingContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
        
        this.gameObject.SetActive(false);
    }

    private void Start() {
        MenuAPI.settingCategoryBuilder?.Invoke(this);
    }

    public Button AddCategory(string category) {
        var categoryObject = Instantiate(MenuAPI.CategoryPagePrefab, CategoryContainer.transform);
        Plugin.Logger.LogInfo($"Added category: {category}");
        
        return categoryObject.GetComponent<Button>();
    }

    public void SwitchCategory(string category) {
        Plugin.Logger.LogInfo("Switching to category: " + category);
    }
}
