using System;
using System.IO;
using SaveSystem.Data;
using UnityEngine;
using Newtonsoft.Json;

namespace SaveSystem
{
    public class FileDataHandler
    {
        private string _dataPath = "";
        private string _dataFileName = "";
        private string _fullPath = "";

        public FileDataHandler(string dataPath, string dataFileName)
        {
            _dataPath = dataPath;
            _dataFileName = dataFileName;
            _fullPath = Path.Combine(_dataPath, _dataFileName);
        }

        public GameData LoadData()
        {
            GameData loadedData = null;
            if(File.Exists(_fullPath))
            {
                try
                {
                    var dataToLoad = "";
                    using(var stream = new FileStream(_fullPath, FileMode.Open))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occured when trying to load data from a file: {_fullPath} \n {e}");
                }
            }

            return loadedData;
        }

        public void SaveData(GameData data)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_fullPath));
                var dataToStore = JsonConvert.SerializeObject(data);
                using(var stream = new FileStream(_fullPath, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to save data into a file: {_fullPath} \n {e}");
            }
        }
    }

}
