using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior;

public class ColourPickerController: MonoBehaviour {
    public int R, G, B, A;

    [SerializeField] 
    private Image RImage, GImage, BImage, AImage, OutputImage;

    [SerializeField] 
    private Slider RSlider, GSlider, BSlider, ASlider;

    [SerializeField] 
    private TMP_InputField RInputField, GInputField, BInputField, AInputField, OutputField;

    [SerializeField] 
    private GameObject ColourPickerContainer;

    private Texture2D RTexture, GTexture, BTexture, ATexture, OutputTexture;
    private TMP_Text label;
    private Button resetButton;

    private void Awake() {
        this.label = this.transform.Find("Top/Label").GetComponentInChildren<TMP_Text>();
        this.resetButton = this.transform.Find("Top/Reset").GetComponent<Button>();
        
        
        RSlider.onValueChanged.AddListener(arg0 => { RInputField.text = ((int) arg0).ToString(); });
        GSlider.onValueChanged.AddListener(arg0 => { GInputField.text = ((int) arg0).ToString(); });
        BSlider.onValueChanged.AddListener(arg0 => { BInputField.text = ((int) arg0).ToString(); });
        ASlider.onValueChanged.AddListener(arg0 => { AInputField.text = ((int) arg0).ToString(); });

        RInputField.onEndEdit.AddListener((arg0 =>
            RSlider.value = int.TryParse(arg0, out int r) ? Mathf.Clamp(r, 0, 255) : RSlider.value));
        GInputField.onEndEdit.AddListener((arg0 =>
            GSlider.value = int.TryParse(arg0, out int g) ? Mathf.Clamp(g, 0, 255) : GSlider.value));
        BInputField.onEndEdit.AddListener((arg0 =>
            BSlider.value = int.TryParse(arg0, out int b) ? Mathf.Clamp(b, 0, 255) : BSlider.value));
        AInputField.onEndEdit.AddListener((arg0 =>
            ASlider.value = int.TryParse(arg0, out int a) ? Mathf.Clamp(a, 0, 255) : ASlider.value));

        RInputField.onValueChanged.AddListener((arg0 => { UpdateColor(); }));
        GInputField.onValueChanged.AddListener((arg0 => { UpdateColor(); }));
        BInputField.onValueChanged.AddListener((arg0 => { UpdateColor(); }));
        AInputField.onValueChanged.AddListener((arg0 => { UpdateColor(); }));

        OutputField.onEndEdit.AddListener(value => {
            var colour = ColorUtility.TryParseHtmlString("#" + value, out Color color) ? color : Color.white;

            RSlider.value = (int) (colour.r * 255);
            GSlider.value = (int) (colour.g * 255);
            BSlider.value = (int) (colour.b * 255);
        });

        OutputImage.GetComponent<Button>().onClick.AddListener(() => {
            ColourPickerContainer.SetActive(!ColourPickerContainer.activeSelf);
        });
        
        CreateTexture();
    }

    private void CreateTexture() {
        RTexture = new Texture2D(255, 1);
        for (int i = 0; i < 256; i++) {
            RTexture.SetPixel(i, 0, new Color(i / 255f, 0, 0, 1));
        }

        RTexture.Apply();
        RImage.sprite = Sprite.Create(RTexture, new Rect(0, 0, RTexture.width, RTexture.height), Vector2.zero);

        GTexture = new Texture2D(255, 1);
        for (int i = 0; i < 256; i++) {
            GTexture.SetPixel(i, 0, new Color(0, i / 255f, 0, 1));
        }

        GTexture.Apply();
        GImage.sprite = Sprite.Create(GTexture, new Rect(0, 0, GTexture.width, GTexture.height), Vector2.zero);

        BTexture = new Texture2D(255, 1);
        for (int i = 0; i < 256; i++) {
            BTexture.SetPixel(i, 0, new Color(0, 0, i / 255f, 1));
        }

        BTexture.Apply();
        BImage.sprite = Sprite.Create(BTexture, new Rect(0, 0, BTexture.width, BTexture.height), Vector2.zero);

        ATexture = new Texture2D(255, 1);
        for (int i = 0; i < 256; i++) {
            ATexture.SetPixel(i, 0, new Color(1, 1, 1, i / 255f));
        }

        ATexture.Apply();
        AImage.sprite = Sprite.Create(ATexture, new Rect(0, 0, ATexture.width, ATexture.height), Vector2.zero);
    }

    public void UpdateColor() {
        R = (int) RSlider.value;
        G = (int) GSlider.value;
        B = (int) BSlider.value;
        A = (int) ASlider.value;

        RInputField.text = R.ToString();
        GInputField.text = G.ToString();
        BInputField.text = B.ToString();
        AInputField.text = A.ToString();

        for (int i = 0; i < 256; i++) {
            RTexture.SetPixel(i, 0, new Color(i / 255f, G / 255f, B / 255f, 1));
        }

        RTexture.Apply();

        for (int i = 0; i < 256; i++) {
            GTexture.SetPixel(i, 0, new Color(R / 255f, i / 255f, B / 255f, 1));
        }

        GTexture.Apply();

        for (int i = 0; i < 256; i++) {
            BTexture.SetPixel(i, 0, new Color(R / 255f, G / 255f, i / 255f, 1));
        }

        BTexture.Apply();

        for (int i = 0; i < 256; i++) {
            ATexture.SetPixel(i, 0, new Color(R / 255f, G / 255f, B / 255f, i / 255f));
        }

        ATexture.Apply();

        Color color = new Color(R / 255f, G / 255f, B / 255f, A / 255f);
        OutputImage.color = color;
        OutputField.text = ColorUtility.ToHtmlStringRGB(color);
    }
    
    public void Initialize(string label, Color color, Color defaultColor, Action<Color> onColorChanged) {
        this.label.text = label;
        
        RSlider.value = color.r * 255;
        GSlider.value = color.g * 255;
        BSlider.value = color.b * 255;
        ASlider.value = color.a * 255;

        OutputField.onValueChanged.AddListener(value => {
            ColorUtility.TryParseHtmlString("#" + value, out Color color);
            onColorChanged?.Invoke(color);
        });
        
        this.resetButton.onClick.AddListener(() => {
            OutputField.text = ColorUtility.ToHtmlStringRGB(color);
        });
        
        ColourPickerContainer.SetActive(false);
    }
}