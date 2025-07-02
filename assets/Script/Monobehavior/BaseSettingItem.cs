using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuLib.MonoBehavior{
    public abstract class BaseSettingItem: MonoBehaviour, IDeferredSetting {
        [SerializeField] 
        protected MenuController menuController;
        [SerializeField] 
        protected TMP_Text label;
        [SerializeField] 
        protected Button resetButton;
        protected bool isChanged = false;
        protected Action applyHandler; 
    
        public abstract void ApplySetting();
    }
}