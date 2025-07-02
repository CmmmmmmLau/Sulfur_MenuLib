using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class SliderFieldController: BaseSettingItem {
    private TMP_InputField inputField;
    private Slider slider;
    
    private string defaultValue;
    private string currentValue;
    private string pendingValue;

    protected override void Awake() {
        this.inputField = this.GetComponentInChildren<TMP_InputField>();
        this.slider = this.GetComponentInChildren<Slider>();
        
        this.resetButton.onClick.AddListener((() => {
            this.slider.value = float.Parse(this.defaultValue);
        }));
        
        base.Awake();
    }

    public void Initialize(string label, float value, float defaultValue, float minValue, float maxValue, Action<float> onValueChanged){
        this.DoSetup(label, value, defaultValue, minValue, maxValue);

        this.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
        this.slider.wholeNumbers = false;
        
        this.slider.onValueChanged.AddListener((value) => {
            float step = 0.01f;
            float stepped = Mathf.Round(value / step) * step;
            this.pendingValue = stepped.ToString();
            
            this.slider.SetValueWithoutNotify(stepped);
            this.inputField.SetTextWithoutNotify(stepped.ToString());
            
            menuController.RegisterDeferredSetting(this);
        });
        
        this.inputField.onEndEdit.AddListener((value) => {
            var result = float.Parse(value);
            result = Mathf.Clamp(result, minValue, maxValue);
            this.pendingValue = result.ToString();
            
            this.slider.SetValueWithoutNotify(result);
            this.inputField.SetTextWithoutNotify(result.ToString());
            
            menuController.RegisterDeferredSetting(this);
        });
        
        this.applyHandler = () => {
            if (float.TryParse(this.pendingValue, out float result)) {
                this.currentValue = result.ToString();
                onValueChanged?.Invoke(result);
            } else {
                Debug.LogWarning("Invalid input value: " + this.pendingValue);
            }
        };
    }

    public void Initialize(string label, int value, int defaultValue, int minValue, int maxValue, Action<int> onValueChanged){
        this.DoSetup(label, value, defaultValue, minValue, maxValue);

        this.inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        this.slider.wholeNumbers = true;
        
        this.slider.onValueChanged.AddListener((value) => {
            this.pendingValue = value.ToString();
            this.inputField.SetTextWithoutNotify(value.ToString());
            
            menuController.RegisterDeferredSetting(this);
        });
        
        this.inputField.onEndEdit.AddListener((value) => {
            var result = float.Parse(value);
            result = Mathf.Clamp(result, minValue, maxValue);
            
            this.pendingValue = result.ToString();
            
            this.slider.SetValueWithoutNotify(result);
            this.inputField.SetTextWithoutNotify(result.ToString());
            
            menuController.RegisterDeferredSetting(this);
        });
        
        this.applyHandler = () => {
            if (int.TryParse(this.pendingValue, out int result)) {
                this.currentValue = result.ToString();
                onValueChanged?.Invoke(result);
            } else {
                Debug.LogWarning("Invalid input value: " + this.inputField.text);
            }
        };
    }
    
    private void DoSetup(string label, float value, float defaultValue, float minValue, float maxValue) {
        this.label.text = label;
        this.inputField.text = value.ToString();
        this.currentValue = value.ToString();
        this.defaultValue = defaultValue.ToString();
        this.slider.minValue = minValue;
        this.slider.maxValue = maxValue;
        this.slider.value = value;
    }

    public override void ApplySetting() {
        this.applyHandler?.Invoke();
        base.ApplySetting();
    }
}