using System;
using System.Collections.Generic;
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

        public FileDataHandler(string dataPath, string dataFileName)
        {
            _dataPath = dataPath;
            _dataFileName = dataFileName;
        }

        public GameData LoadData(string profileId)
        {
            if (profileId is null)
            {
                return null;
            }
            
            var fullPath = Path.Combine(_dataPath, profileId, _dataFileName);
            GameData loadedData = null;
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
                    loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occured when trying to load data from a file: {fullPath} \n {e}");
                }
            }

            return loadedData;
        }

        public void SaveData(GameData data, string profileId)
        {
            if (profileId is null)
            {
                return;
            }
            
            var fullPath = Path.Combine(_dataPath, profileId, _dataFileName);
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
        
        public Dictionary<string, GameData> LoadAllData()
        {
            var profileDictionary = new Dictionary<string, GameData>();

            var dirInfos = new DirectoryInfo(_dataPath).EnumerateDirectories();
            foreach (var dirInfo in dirInfos)
            {
                var profileId = dirInfo.Name;
                var fullPath = Path.Combine(_dataPath, profileId, _dataFileName);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning($"Directory does not contain save data: {profileId}");
                    continue;
                }

                var profileData = LoadData(profileId);

                if (profileData is null)
                {
                    Debug.LogError($"Tried to load profile, but something went wrong/ ProfileID: {profileId}");
                }
                else
                {
                    profileDictionary.Add(profileId, profileData);
                }
            }
            
            return profileDictionary;
        }

        public GameData LoadMostRecentData()
        {
            string mostRecentProfileId = null;
            
            var profileGameData = LoadAllData();
            foreach (var pair in profileGameData)
            {
                var profileId = pair.Key;
                var gameData = pair.Value;
                if (gameData is null)
                {
                    continue;
                }

                if (mostRecentProfileId is null)
                {
                    mostRecentProfileId = profileId;
                }
                else
                {
                    var mostRecentDateTime = DateTime.FromBinary(profileGameData[mostRecentProfileId].lastUpdated);
                    var newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                    if (newDateTime > mostRecentDateTime)
                    {
                        mostRecentProfileId = profileId;
                    }
                }
            }

            return profileGameData[mostRecentProfileId];
        }
    }
}
