using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class DropDownController: MonoBehaviour {
    private TMP_Dropdown dropdown;
    private TMP_Text label;
    private Button resetButton;
    
    private int defaultOption;

    private void Awake() {
        this.label = this.GetComponentInChildren<TMP_Text>();
        this.resetButton = this.transform.Find("Reset").GetComponent<Button>();
        this.dropdown = this.gameObject.GetComponentInChildren<TMP_Dropdown>();
        this.dropdown.ClearOptions();

        this.dropdown.onValueChanged.RemoveAllListeners();
    }

    
    public void Initialize<T>(string label, List<T> options, T option, T defaultOption, Func<T, string> toString, Action<T> onValueChanged) {
        this.label.text = label;
        this.dropdown.AddOptions(options.Select(option => toString == null? option.ToString() : toString(option)).ToList());
        
        this.dropdown.value = options.IndexOf(option);
        this.defaultOption = options.IndexOf(defaultOption);
        dropdown.RefreshShownValue();

        this.dropdown.onValueChanged.AddListener(
            (index) => {
                var value = options[index];
                onValueChanged?.Invoke(value);
            }
        );
        
        this.resetButton.onClick.AddListener(
            () => {
                var value = options.IndexOf(defaultOption);

                dropdown.value = value;
                dropdown.RefreshShownValue();
            }
        );
    }
}