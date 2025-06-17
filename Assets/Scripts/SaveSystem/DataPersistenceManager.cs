using System;
using System.Collections.Generic;
using SaveSystem.Data;
using SaveSystem.Interfaces;
using Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class DataPersistenceManager : Singleton<DataPersistenceManager>
    {
        [Header("Data Storage Config")]
        [SerializeField] private string fileName;
        
        [Header("Development Options")]
        [SerializeField] private bool useMostRecentGameData;
        
        private FileDataHandler _dataHandler;
        
        private GameData _currentGameData;
        public GameData CurrentGameData => _currentGameData;
        
        private LoadMode _currentLoadMode = LoadMode.DirectlyFromScene;

        public LoadMode CurrentLoadMode
        {
            get => _currentLoadMode;
            set => _currentLoadMode = value;
        }

        protected override void Awake()
        {
            base.Awake();
            if (_dataHandler == null)
            {
                _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadGame();
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
            {
                return;
            }
            if (_currentLoadMode == LoadMode.BetweenScenes)
            {
                SaveGameData();
            }
        }

        public Dictionary<string, GameData> LoadAllGameData()
        {
            return _dataHandler.LoadAllData();
        }
        
        public void ChangeCurrentGameData(GameData gameData)
        {
            _currentGameData = gameData;
        }
        
        private void ChangeCurrentGameDataToMostRecent()
        {
            _currentGameData = _dataHandler.LoadMostRecentData();
        }
        private void LoadGame()
        {
            if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
            {
                ChangeCurrentGameDataToMostRecent();
                return;
            }
            switch (_currentLoadMode)
            {
                case LoadMode.NewGame:
                    _currentGameData = new GameData();
                    _currentGameData.save.gameBeginningDate = DateTime.Now.ToString("dd.MM.yy HH:mm");
                    SaveSystemEvents.InvokeOnNewGame();
                    break;
                case LoadMode.FromSaveFile:
                    SaveSystemEvents.InvokeOnFromFileLoad(CurrentGameData);
                    break;
                case LoadMode.BetweenScenes:
                    SaveSystemEvents.InvokeOnBetweenScenesLoad(CurrentGameData);
                    break;
                case LoadMode.DirectlyFromScene:
                    if (useMostRecentGameData)
                    {
                        ChangeCurrentGameDataToMostRecent();
                        SaveSystemEvents.InvokeOnFromFileLoad(CurrentGameData);
                    }
                    if (_currentGameData == null)
                    {
                        _currentGameData = new GameData();
                        _currentGameData.save.gameBeginningDate = DateTime.Now.ToString("dd.MM.yy HH:mm");
                        SaveSystemEvents.InvokeOnNewGame();
                    }
                    break;
                default:
                    _currentGameData = new GameData();
                    SaveSystemEvents.InvokeOnNewGame();
                    break;
            }
            _currentLoadMode = LoadMode.BetweenScenes;
        }
        
        private void SaveGameData()
        {
            if (_currentGameData == null || SceneManager.GetActiveScene().name.Equals("MainMenu"))
            {
                return;
            }

            //var beginDate = DateTime.Parse(_currentGameData.save.gameBeginningDate);
            var beginDate = DateTime.ParseExact(_currentGameData.save.gameBeginningDate, "dd.MM.yy HH:mm", null);
            var saveDate = DateTime.Now;
            
            var difference = saveDate - beginDate;
            _currentGameData.save.gameTime = $"{difference.Hours}:{difference.Minutes}";
            
            _currentGameData.save.lastUpdated = DateTime.Now.ToBinary();
            _currentGameData.save.saveDate = saveDate.ToString("dd.MM.yy HH:mm");
            SaveSystemEvents.InvokeOnSave(_currentGameData);
        }
        
        public void SaveGame(string profile)
        {
            SaveGameData();
            _dataHandler.SaveData(_currentGameData, profile);
        }
        
        public bool HasGameData()
        {
            return _currentGameData is not null;
        }
    }

    public enum LoadMode
    {
        DirectlyFromScene, FromSaveFile, BetweenScenes, NewGame
    }
}
