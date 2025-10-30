
using UnityEngine;

namespace Settings
{
    public class Localization
    {
        private int _languageIndex;

        public Localization()
        {
            SettingsEvents.OnLanguageChanged += ChangeLanguageIndex;
        }

        private void ChangeLanguageIndex(int value)
        {
            _languageIndex = value;
            SettingsEvents.LocalizeGame(_languageIndex);
        }

        public void UnsubscribeEvents()
        {
            SettingsEvents.OnLanguageChanged -= ChangeLanguageIndex;
        }
    }
}
