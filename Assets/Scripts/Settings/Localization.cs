
namespace Settings
{
    public class Localization
    {
        private int _languageIndex;

        public int LanguageIndex => _languageIndex;

        public Localization()
        {
            SettingsEvents.OnLanguageSelected += ChangeLanguageIndex;
            SettingsEvents.OnSettingLoaded += LoadData;
        }

        private void ChangeLanguageIndex(int value)
        {
            _languageIndex = value;
            SettingsEvents.ChangeLanguage(_languageIndex);
            SettingsManager.Instance.Data.languageIndex = _languageIndex;
            SettingsEvents.SaveSettings();
        }

        public void UnsubscribeEvents()
        {
            SettingsEvents.OnLanguageSelected -= ChangeLanguageIndex;
            SettingsEvents.OnSettingLoaded -= LoadData;
        }

        private void LoadData(SettingsData data)
        {
            _languageIndex = data.languageIndex;
            SettingsEvents.LoadLanguage(_languageIndex);
        }
    }
}
