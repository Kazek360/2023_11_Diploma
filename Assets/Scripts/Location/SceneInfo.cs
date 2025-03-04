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
        
        public void SaveData(ref GameData gameData)
        {
            gameData.scene.name = currentScene;
            gameData.scene.displayedName = displayedSceneName;
        }
    }
}