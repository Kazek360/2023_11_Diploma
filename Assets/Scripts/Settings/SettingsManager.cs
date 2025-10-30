using System;
using Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Settings
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        private SettingsFileDataHandler _dataHandler;
        
        private Localization _localization;
        public Localization Localization => _localization;

        private SettingsData _settingsData;
        public SettingsData Data
        {
            get => _settingsData;
            set => _settingsData = value;
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SettingsEvents.OnSettingSaved += SaveSettings;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SettingsEvents.OnSettingSaved -= SaveSettings;
            _localization.UnsubscribeEvents();
        }

        protected override void Awake()
        {
            base.Awake();
            _dataHandler = new SettingsFileDataHandler(Application.persistentDataPath);
            _settingsData = _dataHandler.LoadData();
            _localization = new Localization();
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (_settingsData == null)
            {
                _settingsData = new SettingsData();
                return;
            }
            SettingsEvents.LoadSettings(_settingsData);
        }

        private void SaveSettings()
        {
            _dataHandler.SaveData(_settingsData);
        }
    }
}
