using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class SliderFieldController: MonoBehaviour {
    private TMP_InputField inputField;
    private TMP_Text label;
    private Button resetButton;
    private Slider slider;
    
    private string defaultValue;

    private void Awake() {
        this.inputField = this.GetComponentInChildren<TMP_InputField>();
        this.slider = this.GetComponentInChildren<Slider>();
        this.label = this.transform.Find("Label").GetComponentInChildren<TMP_Text>();
        this.resetButton = this.transform.Find("Reset").GetComponent<Button>();
        
        this.resetButton.onClick.AddListener((() => {
            this.slider.value = float.Parse(this.defaultValue);
        }));
    }

    public void Initialize(string label, float value, float defaultValue, float minValue, float maxValue, Action<float> onValueChanged){
        this.DoSetup(label, value, defaultValue, minValue, maxValue);

        this.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
        this.slider.wholeNumbers = false;
        
        this.slider.onValueChanged.AddListener((value) => {
            float step = 0.01f;
            float stepped = Mathf.Round(value / step) * step;
            this.slider.value = stepped;
            
            this.inputField.text = stepped.ToString();
            onValueChanged?.Invoke(stepped);
        });
        
        this.inputField.onEndEdit.AddListener((value) => {
            var result = float.Parse(value);
            result = Mathf.Clamp(result, minValue, maxValue);
            
            this.inputField.text = result.ToString();
            this.slider.value = result;
        });
    }

    public void Initialize(string label, int value, int defaultValue, int minValue, int maxValue, Action<int> onValueChanged){
        this.DoSetup(label, value, defaultValue, minValue, maxValue);

        this.inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        this.slider.wholeNumbers = true;
        
        this.slider.onValueChanged.AddListener((value) => {
            this.inputField.text = value.ToString();
            onValueChanged?.Invoke((int)value);
        });
        
        this.inputField.onEndEdit.AddListener((value) => {
            var result = float.Parse(value);
            result = Mathf.Clamp(result, minValue, maxValue);
            
            this.inputField.text = result.ToString();
            this.slider.value = result;
        });
    }
    
    private void DoSetup(string label, float value, float defaultValue, float minValue, float maxValue) {
        this.label.text = label;
        this.inputField.text = value.ToString();
        this.defaultValue = defaultValue.ToString();
        this.slider.minValue = minValue;
        this.slider.maxValue = maxValue;
        this.slider.value = value;
    }
}