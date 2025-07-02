using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class CheckBoxController: BaseSettingItem{
    private Toggle toggle;
    private bool defaultState;
    private bool currentState;
    private bool pendingState;

    protected override void Awake() {
        this.toggle = this.gameObject.GetComponentInChildren<Toggle>();
        base.Awake();
    }

    public void Initialize(string labelText, bool state, bool defaultState, Action<bool> onToggle) {
        this.label.text = labelText;
        this.defaultState = defaultState;
        this.toggle.isOn = state;
        this.currentState = state;
        this.pendingState = state;
        
        this.toggle.onValueChanged.AddListener(value => {
            this.pendingState = value;
            menuController.RegisterDeferredSetting(this);
        });
        
        this.resetButton.onClick.AddListener(
            () => {
                this.toggle.isOn = this.defaultState;
            }
        );
        
        this.applyHandler = () => {
            this.currentState = this.pendingState;
            onToggle?.Invoke(this.currentState);
        };
    }

    public override void ApplySetting() {
        applyHandler?.Invoke();
        base.ApplySetting();
    }
}