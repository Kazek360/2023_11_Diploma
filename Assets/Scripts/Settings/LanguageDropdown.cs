using TMPro;
using UnityEngine;

namespace Settings
{
    public class LanguageDropdown : MonoBehaviour
    {
        private TMP_Dropdown _dropdown;
        
        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            _dropdown.onValueChanged.AddListener(ChangeValue);
        }

        private void ChangeValue(int value)
        {
            SettingsEvents.ChangeLanguage(value);
        }
    }
}
