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
        
        this.resetButton.onClick.AddListener(
            () => {
                inputField.text = this.defaultValue;
                this.inputField.onEndEdit?.Invoke(this.defaultValue);
            }
        );
    }

    public void Initialize(string label, float value, float defaultValue, float maxValue, float minValue,
        Action<float> onEndEdit) {
        
        this.DoSetup(label, value.ToString(), defaultValue.ToString(), TMP_InputField.ContentType.DecimalNumber);
        
        this.inputField.onEndEdit.AddListener((value) => {
            float.TryParse(value, out float result);
            
            result = float.TryParse(value, out float num)
                ? Mathf.Clamp(num, minValue, maxValue)
                : float.Parse(this.defaultValue);
            
            this.inputField.text = result.ToString();
            onEndEdit?.Invoke(result);
        });
    }
    
    public void Initialize(string label, int value, int defaultValue, int maxValue, int minValue,
        Action<int> onEndEdit) {

        this.DoSetup(label, value.ToString(), defaultValue.ToString(), TMP_InputField.ContentType.IntegerNumber);
        
        this.inputField.onEndEdit.AddListener((value) => {
            int.TryParse(value, out int result);
            
            result = int.TryParse(value, out int num)? Mathf.Clamp(num, minValue, maxValue): int.Parse(this.defaultValue);
            
            this.inputField.text = result.ToString();
            onEndEdit?.Invoke(result);
        });
    }
    
    private void DoSetup(string label, string value, string defaultValue, TMP_InputField.ContentType contentType){
        this.label.text = label;
        this.inputField.text = value;
        this.defaultValue = defaultValue;
        this.inputField.contentType = contentType;
    }
}