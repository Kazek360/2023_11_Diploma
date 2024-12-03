using System.Collections;
using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using Shared;
using UnityEngine;

namespace Tutorials
{
    public class TutorialManager : Singleton<TutorialManager>, IDataPersistence
    {
        public Dictionary<string, Tutorial> tutorials;

        protected override void Awake()
        {
            base.Awake();
            tutorials = CreateTutorialDictionary();
        }
        
        
        public void TriggerTutorial()
        {
            
        }
        
        private Dictionary<string, Tutorial> CreateTutorialDictionary()
        {
            TutorialInfo[] allTutorials = Resources.LoadAll<TutorialInfo>("Tutorials");
            Dictionary<string, Tutorial> idToDictionary = new Dictionary<string, Tutorial>();
            foreach (var tutorialInfo in allTutorials)
            {
                if (idToDictionary.ContainsKey(tutorialInfo.name))
                {
                    Debug.LogWarning($"Found Duplicate ID: {tutorialInfo.name}");
                }

                idToDictionary.Add(tutorialInfo.name, new Tutorial(tutorialInfo));
            }
            return idToDictionary;
        }

        public void LoadData(GameData gameData)
        {
            var loadedTutorials = gameData.tutorials;
            foreach (var pair in loadedTutorials)
            {
                tutorials[pair.Key].hasTriggered = loadedTutorials[pair.Key].hasTriggered;
            }
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.tutorials = tutorials;
        }
    }
}
