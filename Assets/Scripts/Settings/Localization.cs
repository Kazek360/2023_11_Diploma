
using UnityEngine;

namespace Settings
{
    public class Localization
    {
        private string _language;
        private int _languageIndex;

        public Localization()
        {
            SettingsEvents.OnLanguageChanged += ChangeLanguageIndex;
        }

        private void ChangeLanguageIndex(int value)
        {
            _languageIndex = value;
            Debug.Log($"Language Index: {_languageIndex}");
            SettingsEvents.LocalizeGame(_languageIndex);
        }

        public void UnsubscribeEvents()
        {
            SettingsEvents.OnLanguageChanged -= ChangeLanguageIndex;
        }
    }
}
