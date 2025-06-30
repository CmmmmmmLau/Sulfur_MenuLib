using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class ButtonController: MonoBehaviour {
    private TMP_Text label;
    private Button button;

    private void Awake() {
        this.label = this.transform.Find("Label").GetComponentInChildren<TMP_Text>();
        this.button = this.GetComponentInChildren<Button>();
    }
    
    public void Initialize(string label, Action onClick) {
        this.label.text = label;
        this.button.onClick.AddListener(() => {
            onClick?.Invoke();
        });
    }
}