using System;
using SaveSystem;
using SaveSystem.Data;
using Shared;
using UnityEngine.SceneManagement;

namespace Location
{
    public class SceneInfo : Singleton<SceneInfo>, IDataPersistence
    {
        public string displayedSceneName;
        public string currentScene;

        protected override void Awake()
        {
            base.Awake();
            currentScene = SceneManager.GetActiveScene().name;
        }
        
        public void InitializeOnNewGame()
        {
            
        }

        public void LoadDataFromSaveFile(GameData gameData)
        {
            
        }

        public void LoadDataBetweenScenes(GameData gameData)
        {
            
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.currentScene = currentScene;
            gameData.saveDate = DateTime.Now.ToString("dd.MM.yy HH:mm");
        }
    }
}