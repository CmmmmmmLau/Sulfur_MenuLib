using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MenuLib.MonoBehavior {
    public class ColourPickerController : BaseSettingItem {
        public int R, G, B, A;

        [SerializeField] private Image RImage, GImage, BImage, AImage, OutputImage;

        [SerializeField] private Slider RSlider, GSlider, BSlider, ASlider;

        [SerializeField] private TMP_InputField RInputField, GInputField, BInputField, AInputField, OutputField;

        [SerializeField] private GameObject ColourPickerContainer;

        private Texture2D RTexture, GTexture, BTexture, ATexture, OutputTexture;

        private void Awake() {
            RSlider.onValueChanged.AddListener(value => { UpdateInputField(RInputField, value); });
            GSlider.onValueChanged.AddListener(value => { UpdateInputField(GInputField, value); });
            BSlider.onValueChanged.AddListener(value => { UpdateInputField(BInputField, value); });
            ASlider.onValueChanged.AddListener(value => { UpdateInputField(AInputField, value); });

            RInputField.onEndEdit.AddListener(value => { UpdateSlider(RSlider, RInputField, value); });
            GInputField.onEndEdit.AddListener(value => { UpdateSlider(GSlider, GInputField, value); });
            BInputField.onEndEdit.AddListener(value => { UpdateSlider(BSlider, BInputField, value); });
            AInputField.onEndEdit.AddListener(value => { UpdateSlider(ASlider, AInputField, value); });

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

        private void UpdateSlider(Slider slider, TMP_InputField field, string value) {
            if (!int.TryParse(value, out int num)) {
                num = 255;
            }

            num = Mathf.Clamp(num, 0, 255);
            BInputField.SetTextWithoutNotify(num.ToString());
            slider.SetValueWithoutNotify(num);
            UpdateTexture();
        }

        private void UpdateInputField(TMP_InputField field, float value) {
            if (field.text != value.ToString()) {
                field.SetTextWithoutNotify(value.ToString());
                UpdateTexture();
            }
        }

        private void CreateTexture() {
            RTexture = new Texture2D(256, 1);
            for (int i = 0; i < 256; i++) {
                RTexture.SetPixel(i, 0, new Color(i / 255f, 0, 0, 1));
            }

            RTexture.Apply();
            RImage.sprite = Sprite.Create(RTexture, new Rect(0, 0, RTexture.width, RTexture.height), Vector2.zero);

            GTexture = new Texture2D(256, 1);
            for (int i = 0; i < 256; i++) {
                GTexture.SetPixel(i, 0, new Color(0, i / 255f, 0, 1));
            }

            GTexture.Apply();
            GImage.sprite = Sprite.Create(GTexture, new Rect(0, 0, GTexture.width, GTexture.height), Vector2.zero);

            BTexture = new Texture2D(256, 1);
            for (int i = 0; i < 256; i++) {
                BTexture.SetPixel(i, 0, new Color(0, 0, i / 255f, 1));
            }

            BTexture.Apply();
            BImage.sprite = Sprite.Create(BTexture, new Rect(0, 0, BTexture.width, BTexture.height), Vector2.zero);

            ATexture = new Texture2D(256, 1);
            for (int i = 0; i < 256; i++) {
                ATexture.SetPixel(i, 0, new Color(1, 1, 1, i / 255f));
            }

            ATexture.Apply();
            AImage.sprite = Sprite.Create(ATexture, new Rect(0, 0, ATexture.width, ATexture.height), Vector2.zero);
        }

        public void UpdateTexture() {
            R = (int) RSlider.value;
            G = (int) GSlider.value;
            B = (int) BSlider.value;
            A = (int) ASlider.value;

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
                RSlider.value = defaultColor.r * 255;
                GSlider.value = defaultColor.g * 255;
                BSlider.value = defaultColor.b * 255;
                ASlider.value = defaultColor.a * 255;
            });

            ColourPickerContainer.SetActive(false);
        }

        public override void ApplySetting() {
            throw new NotImplementedException();
        }
    }
}