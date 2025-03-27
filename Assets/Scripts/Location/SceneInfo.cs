using System;
using SaveSystem;
using SaveSystem.Data;
using SaveSystem.Interfaces;
using Shared;
using UnityEngine.SceneManagement;

namespace Location
{
    public class SceneInfo : Singleton<SceneInfo>, ISavable
    {
        public string displayedSceneName;
        [NonSerialized] public string currentScene;

        protected override void Awake()
        {
            base.Awake();
            currentScene = SceneManager.GetActiveScene().name;
        }
        
        private void OnEnable()
        {
            SaveSystemEvents.OnSave += SaveData;
        }
        private void OnDisable()
        {
            SaveSystemEvents.OnSave -= SaveData;
        }
        
        public void SaveData(GameData gameData)
        {
            gameData.scene.name = currentScene;
            gameData.scene.displayedName = displayedSceneName;
        }
    }
}