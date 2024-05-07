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
        }
        private void OnSceneUnloaded(Scene scene)
        {
            SaveGame();
        }
        
        public void NewGame()
        {
            _gameData = new GameData();
        }

        private void LoadGame()
        {
            _gameData = _dataHandler.LoadData();
            if (_gameData is null)
            {
                return;
            }

            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.LoadData(_gameData);
            }
        }

        private void SaveGame()
        {
            if (_gameData is null)
            {
                return;
            }
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.SaveData(ref _gameData);
            }
            _dataHandler.SaveData(_gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }
        
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public bool HasGameData()
        {
            return _gameData is not null;
        }
        public GameData GameData => _gameData;
    }
}
