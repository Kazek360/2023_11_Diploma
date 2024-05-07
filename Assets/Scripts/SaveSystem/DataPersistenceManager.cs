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
        private static List<IDataPersistence> _dataPersistenceObjects;
        

        private void Awake()
        {
            Instance = this;
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        private void NewGame()
        {
            _gameData = new GameData();
        }

        private void LoadGame()
        {
            _gameData = _dataHandler.LoadData();
            if (_gameData is null)
            {
                NewGame();
            }

            foreach (var dataPersistenceObject in _dataPersistenceObjects)
            {
                dataPersistenceObject.LoadData(_gameData);
            }
        }

        private void SaveGame()
        {
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
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
