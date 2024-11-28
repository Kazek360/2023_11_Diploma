using System;
using System.Collections.Generic;
using System.Linq;
using SaveSystem.Data;
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

        private static List<IDataPersistence> _dataPersistenceObjects;
        
        private LoadMode _currentLoadMode = LoadMode.DirectlyFromScene;

        public LoadMode CurrentLoadMode
        {
            get => _currentLoadMode;
            set => _currentLoadMode = value;
        }

        private new void Awake()
        {
            base.Awake();
            if (_dataHandler is null)
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
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SceneManager.GetActiveScene().name.Equals("MainMenu") || SceneManager.GetActiveScene().name.Equals("BlueMainMenu"))
            {
                _currentLoadMode = LoadMode.MainMenu;
            }
            LoadGame();
            _currentLoadMode = LoadMode.BetweenScenes;
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            if (SceneManager.GetActiveScene().name.Equals("MainMenu") || SceneManager.GetActiveScene().name.Equals("BlueMainMenu"))
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
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            switch (_currentLoadMode)
            {
                case LoadMode.MainMenu:
                    ChangeCurrentGameDataToMostRecent();
                    return;
                case LoadMode.NewGame:
                    _currentGameData = new GameData();
                    break;
                case LoadMode.DirectlyFromScene:
                    if (useMostRecentGameData)
                    {
                        ChangeCurrentGameDataToMostRecent();
                        CurrentLoadMode = LoadMode.FromSaveFile;
                    }
                    if (_currentGameData is null)
                    {
                        _currentGameData = new GameData();
                        CurrentLoadMode = LoadMode.NewGame;
                    }
                    break;
            }
            LoadGameData();
        }
        
        private void LoadGameData()
        {
            if (_currentGameData is not null)
            {
                foreach (var dataPersistenceObject in _dataPersistenceObjects)
                {
                    dataPersistenceObject.LoadData(_currentGameData);
                }
            }
        }
        
        private void SaveGameData()
        {
            if (_currentGameData is null || SceneManager.GetActiveScene().name.Equals("MainMenu") || SceneManager.GetActiveScene().name.Equals("BlueMainMenu"))
            {
                return;
            }
            _currentGameData.save.lastUpdated = DateTime.Now.ToBinary();
            _currentGameData.save.date = DateTime.Now.ToString("dd.MM.yy HH:mm");
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.SaveData(ref _currentGameData);
            }
        }
        
        public void SaveGame(string profile)
        {
            SaveGameData();
            _dataHandler.SaveData(_currentGameData, profile);
        }
        
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
        
        public bool HasGameData()
        {
            return _currentGameData is not null;
        }
    }

    public enum LoadMode
    {
        DirectlyFromScene, FromSaveFile, BetweenScenes, NewGame, MainMenu
    }
}
