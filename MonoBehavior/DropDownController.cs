using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class DropDownController: BaseSettingItem {
    private TMP_Dropdown dropdown;
    
    private int defaultIndex;
    private int currentIndex;
    private int pendingIndex;

    protected override void Awake() {
        this.dropdown = this.gameObject.GetComponentInChildren<TMP_Dropdown>();
        this.dropdown.ClearOptions();

        this.dropdown.onValueChanged.RemoveAllListeners();
        base.Awake();
    }

    
    public void Initialize<T>(string label, List<T> options, T option, T defaultOption, Func<T, string> toString, Action<T> onValueChanged) {
        this.label.text = label;
        this.dropdown.AddOptions(options.Select(option => toString == null? option.ToString() : toString(option)).ToList());
        
        this.currentIndex = options.IndexOf(option);
        this.dropdown.value = this.currentIndex;
        
        this.defaultIndex = options.IndexOf(defaultOption);
        this.dropdown.RefreshShownValue();

        this.dropdown.onValueChanged.AddListener(
            (index) => {
                this.pendingIndex = index;
                menuController.RegisterDeferredSetting(this);
            }
        );
        
        this.resetButton.onClick.AddListener(
            () => {
                this.dropdown.value = this.defaultIndex;
                this.dropdown.RefreshShownValue();
            }
        );
        
        this.applyHandler = () => {
            this.currentIndex = this.pendingIndex;
            var value = options[this.currentIndex];
            onValueChanged?.Invoke(value);
        };
    }

    public override void ApplySetting() {
        applyHandler?.Invoke();
        base.ApplySetting();
    }
}