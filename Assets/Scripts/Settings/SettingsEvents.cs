using System;

namespace Settings
{
    public static class SettingsEvents
    {
        public static event Action<int> OnLanguageChanged;
        public static void ChangeLanguage(int languageindex)
        {
            OnLanguageChanged?.Invoke(languageindex);
        }
        public static event Action<int> OnGameLocalized;
        public static void LocalizeGame(int languageindex)
        {
            OnLanguageChanged?.Invoke(languageindex);
        }
    }
}
