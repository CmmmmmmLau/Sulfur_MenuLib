using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class CheckBoxController: MonoBehaviour{
    private Toggle toggle;
    private TMP_Text label;
    private Button resetButton;
    
    private bool defaultState;
    
    Action<bool> onToggle;
    

    private void Awake() {
        this.label = this.GetComponentInChildren<TMP_Text>();
        this.resetButton = this.transform.Find("Reset").GetComponent<Button>();
        this.toggle = this.gameObject.GetComponentInChildren<Toggle>();
        
        this.toggle.onValueChanged.AddListener(
            (value) => {
                onToggle?.Invoke(value);
            }
        );

        this.resetButton.onClick.AddListener(
            () => {
                toggle.isOn = defaultState;
            }
        );
    }
    
    public void Initialize(string labelText, bool state, bool defaultState, Action<bool> onToggle) {
        this.label.text = labelText;
        this.defaultState = defaultState;
        this.onToggle = onToggle;
        this.toggle.isOn = state;
    }
}