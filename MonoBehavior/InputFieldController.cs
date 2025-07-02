using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class InputFieldController: BaseSettingItem {
    private TMP_InputField inputField;
    
    private string defaultValue;
    private string currentValue;
    private string pendingValue;
    
    protected override void Awake() {
        this.inputField = this.GetComponentInChildren<TMP_InputField>();
        
        this.resetButton.onClick.AddListener(
            () => {
                inputField.text = this.defaultValue;
                this.pendingValue = this.defaultValue;
            }
        );
        
        base.Awake();
    }

    public void Initialize(string label, float value, float defaultValue, float maxValue, float minValue,
        Action<float> onEndEdit) {
        
        this.DoSetup(label, value.ToString(), defaultValue.ToString(), TMP_InputField.ContentType.DecimalNumber);
        
        this.inputField.onEndEdit.AddListener((value) => {
            float.TryParse(value, out float result);
            
            result = float.TryParse(value, out float num)
                ? Mathf.Clamp(num, minValue, maxValue)
                : float.Parse(this.defaultValue);
            
            this.inputField.SetTextWithoutNotify(result.ToString());
            this.pendingValue = result.ToString();
            
            menuController.RegisterDeferredSetting(this);
        });
        
        this.applyHandler = () => {
            this.currentValue = this.pendingValue;
            
            if (float.TryParse(this.pendingValue, out float result)) {
                onEndEdit?.Invoke(result);
            } else {
                Debug.LogWarning("Invalid input value: " + this.pendingValue);
            }
        };
    }
    
    public void Initialize(string label, int value, int defaultValue, int maxValue, int minValue,
        Action<int> onEndEdit) {

        this.DoSetup(label, value.ToString(), defaultValue.ToString(), TMP_InputField.ContentType.IntegerNumber);
        
        this.inputField.onEndEdit.AddListener((value) => {
            int.TryParse(value, out int result);
            
            result = int.TryParse(value, out int num)? Mathf.Clamp(num, minValue, maxValue): int.Parse(this.defaultValue);
            
            this.inputField.SetTextWithoutNotify(result.ToString());
            this.pendingValue = result.ToString();
            
            menuController.RegisterDeferredSetting(this);
        });
        
        this.applyHandler = () => {
            this.currentValue = this.pendingValue;
            
            if (int.TryParse(this.pendingValue, out int result)) {
                onEndEdit?.Invoke(result);
            } else {
                Debug.LogWarning("Invalid input value: " + this.pendingValue);
            }
        };
    }
    
    private void DoSetup(string label, string value, string defaultValue, TMP_InputField.ContentType contentType){
        this.label.text = label;
        this.inputField.text = value;
        this.defaultValue = defaultValue;
        this.inputField.contentType = contentType;
    }
    
    public override void ApplySetting() {
        applyHandler?.Invoke();
        base.ApplySetting();
    }
}