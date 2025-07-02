using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior {
    public class MenuController: MonoBehaviour {
        [SerializeField] 
        private GameObject CategoryContainer;
        [SerializeField] 
        private GameObject SettingContainer;
        [SerializeField]
        private Button applyButton;
    }
}