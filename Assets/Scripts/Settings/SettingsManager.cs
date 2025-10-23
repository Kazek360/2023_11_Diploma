using System;
using Shared;

namespace Settings
{
    public class SettingsManager : Singleton<SettingsDataHandler>
    {
        private Localization _localization;
        
        private void Start()
        {
            _localization = new Localization();
        }

        private void OnDisable()
        {
            _localization.UnsubscribeEvents();
        }
    }
}
