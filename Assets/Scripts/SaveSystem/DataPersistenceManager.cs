using System.Collections.Generic;
using System.Linq;
using SaveSystem.Data;
using UnityEngine;

namespace SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("Data Storage Config")]
        [SerializeField] private string fileName;
        public static DataPersistenceManager Instance { get; private set; }
        private GameData _gameData;
        private FileDataHandler _dataHandler;
        public static List<IDataPersistence> DataPersistenceObjects;
        

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            DataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            _gameData = _dataHandler.LoadData();
            if (_gameData is null)
            {
                NewGame();
            }

            foreach (var dataPersistenceObject in DataPersistenceObjects)
            {
                dataPersistenceObject.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            foreach (var dataPersistenceObject in DataPersistenceObjects)
            {
                dataPersistenceObject.SaveData(ref _gameData);
            }
            _dataHandler.SaveData(_gameData);
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
        
        
        private void OnApplicationQuit()
        {
            SaveGame();
        }
    }

}
