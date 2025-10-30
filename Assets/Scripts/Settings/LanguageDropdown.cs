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

        private void OnEnable()
        {
            SettingsEvents.OnLanguageLoaded += ChangeValueWithoutNotify;
        }

        private void OnDisable()
        {
            SettingsEvents.OnLanguageLoaded -= ChangeValueWithoutNotify;
        }

        private void ChangeValue(int value)
        {
            SettingsEvents.ChangeLanguageIndex(value);
        }

        private void ChangeValueWithoutNotify(int value)
        {
            _dropdown.SetValueWithoutNotify(value);
        }
    }
}
