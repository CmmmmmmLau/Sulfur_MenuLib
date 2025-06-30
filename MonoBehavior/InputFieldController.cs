using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class InputFieldController: MonoBehaviour {
    private TMP_InputField inputField;
    private TMP_Text label;
    private Button resetButton;
    
    private string defaultValue;
    
    private void Awake() {
        this.inputField = this.GetComponentInChildren<TMP_InputField>();
        this.label = this.GetComponentInChildren<TMP_Text>();
        this.resetButton = this.transform.Find("Reset").GetComponent<Button>();
    }

    public void Initialize(string label, float value, float defaultValue, float maxValue, float minValue,
        Action<float> onEndEdit) {
        this.label.text = label;
        this.inputField.text = value.ToString();
        this.defaultValue = defaultValue.ToString();
        this.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
        
        this.inputField.onEndEdit.AddListener((value) => {
            float.TryParse(value, out float result);
            
            result = float.TryParse(value, out float num)
                ? Mathf.Clamp(num, minValue, maxValue)
                : float.Parse(this.defaultValue);
            
            this.inputField.text = result.ToString();
            onEndEdit?.Invoke(result);
        });
                
        this.resetButton.onClick.AddListener(
            () => {
                inputField.text = defaultValue.ToString();
                this.inputField.onEndEdit?.Invoke(this.defaultValue);
            }
        );
    }
    
    public void Initialize(string label, int value, int defaultValue, int maxValue, int minValue,
        Action<int> onEndEdit) {
        this.label.text = label;
        this.inputField.text = value.ToString();
        this.defaultValue = defaultValue.ToString();
        this.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
        
        this.inputField.onEndEdit.AddListener((value) => {
            int.TryParse(value, out int result);
            
            result = int.TryParse(value, out int num)? Mathf.Clamp(num, minValue, maxValue): int.Parse(this.defaultValue);
            
            this.inputField.text = result.ToString();
            onEndEdit?.Invoke(result);
        });
        
        this.resetButton.onClick.AddListener(
            () => {
                inputField.text = defaultValue.ToString();
                this.inputField.onEndEdit?.Invoke(this.defaultValue);
            }
        );
    }
}