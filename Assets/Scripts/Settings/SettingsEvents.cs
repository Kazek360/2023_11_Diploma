using System;

namespace Settings
{
    public static class SettingsEvents
    {
        public static event Action<SettingsData> OnSettingLoaded;
        public static void LoadSettings(SettingsData data)
        {
            OnSettingLoaded?.Invoke(data);
        }
        public static event Action OnSettingSaved;
        public static void SaveSettings()
        {
            OnSettingSaved?.Invoke();
        }
        
        //Events related to game localization
        public static event Action<int> OnLanguageSelected;
        public static void ChangeLanguageIndex(int languageIndex)
        {
            OnLanguageSelected?.Invoke(languageIndex);
        }
        public static event Action<int> OnLanguageChanged;
        public static void ChangeLanguage(int languageIndex)
        {
            OnLanguageChanged?.Invoke(languageIndex);
        }
        public static event Action<int> OnLanguageLoaded;
        public static void LoadLanguage(int languageIndex)
        {
            OnLanguageLoaded?.Invoke(languageIndex);
        }
    }
}
