using System;
using System.Collections.Generic;
using System.Linq;
using SaveSystem.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("Data Storage Config")]
        [SerializeField] private string fileName;
        public static DataPersistenceManager Instance { get; private set; }
        private GameData _gameData;
        private FileDataHandler _dataHandler;
        private static List<IDataPersistence> _dataPersistenceObjects;
        private string _selectedProfileId = "";

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            _selectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();
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
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
            if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
            {
                _selectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();
            }
        }
        private void OnSceneUnloaded(Scene scene)
        {
            SaveGame("autosave");
        }

        public void ChangeSelectedProfileId(string newProfile)
        {
            _selectedProfileId = newProfile;
        }
        
        public void NewGame()
        {
            _gameData = new GameData();
            ChangeSelectedProfileId("newGame");
        }

        public void LoadGameData()
        {
            _gameData = _dataHandler.LoadData(_selectedProfileId);
        }
        
        public void LoadGame()
        {
            if (_selectedProfileId is null)
            {
                return;
            }
            if (_selectedProfileId.Equals("newGame"))
            {
                return;
            }
            _gameData = _dataHandler.LoadData(_selectedProfileId);
            if (_gameData is not null)
            {
                foreach (var dataPersistenceObject in _dataPersistenceObjects)
                {
                    dataPersistenceObject.LoadData(_gameData);
                }
            }
        }

        public void SaveGame(string profile)
        {
            if (_gameData is null || SceneManager.GetActiveScene().name.Equals("MainMenu"))
            {
                return;
            }

            _gameData.lastUpdated = DateTime.Now.ToBinary();
            _gameData.saveDate = DateTime.Now.ToString("dd.MM.yy HH:mm");
            _gameData.currentScene = SceneManager.GetActiveScene().name;
            
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.SaveData(ref _gameData);
            }
            _dataHandler.SaveData(_gameData, profile);
        }
        
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public Dictionary<string, GameData> GetAllGameData()
        {
            return _dataHandler.LoadAllData();
        }
        
        public bool HasGameData()
        {
            return _gameData is not null;
        }
        public GameData GameData => _gameData;
    }
}
