using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Settings
{
    public class SettingsFileDataHandler
    {
        private string _dataPath;
        private string _dataFileName;
        
        public SettingsFileDataHandler(string dataPath)
        {
            _dataPath = dataPath;
            _dataFileName = "settings";
        }
        
        public SettingsData LoadData()
        {
            var fullPath = Path.Combine(_dataPath, _dataFileName);
            SettingsData loadedData = null;
            if(File.Exists(fullPath))
            {
                try
                {
                    var dataToLoad = "";
                    using(var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    loadedData = JsonConvert.DeserializeObject<SettingsData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occured when trying to load data from a file: {fullPath} \n {e}");
                }
            }

            return loadedData;
        }
        
        public void SaveData(SettingsData data)
        {
            var fullPath = Path.Combine(_dataPath, _dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                var dataToStore = JsonConvert.SerializeObject(data);
                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to save data into a file: {fullPath} \n {e}");
            }
        }
        
    }
}