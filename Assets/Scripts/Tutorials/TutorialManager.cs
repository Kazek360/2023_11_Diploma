using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using SaveSystem.Interfaces;
using Shared;
using UI;
using UnityEngine;

namespace Tutorials
{
    public class TutorialManager : Singleton<TutorialManager>, ISavable
    {
        public Dictionary<string, Tutorial> tutorials;

        protected override void Awake()
        {
            base.Awake();
            tutorials = CreateTutorialDictionary();
        }
        
        private void OnEnable()
        {
            SaveSystemEvents.OnFromFileLoad += LoadData;
            SaveSystemEvents.OnBetweenScenesLoad += LoadData;
            SaveSystemEvents.OnSave += SaveData;
        }

        private void OnDisable()
        {
            SaveSystemEvents.OnFromFileLoad -= LoadData;
            SaveSystemEvents.OnBetweenScenesLoad -= LoadData;
            SaveSystemEvents.OnSave -= SaveData;
        }
        
        public void TriggerTutorial(TutorialInfo info)
        {
            if (tutorials[info.name].hasTriggered)
            {
                return;
            }
            tutorials[info.name].hasTriggered = true;
            TutorialUI.InvokeOnTutorialTrigger(info);
            if (UserInterfaceManager.Instance.CurrentStateType == UserInterfaceState.Exploration)
            {
                UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Tutorial);
            }
            else
            {
                UserInterfaceManager.Instance.ChangeStateWithoutVisibilityChange(UserInterfaceState.Tutorial);
            }
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
        
        private void LoadData(GameData gameData)
        {
            var loadedTutorials = gameData.tutorials;
            foreach (var pair in loadedTutorials)
            {
                if (!tutorials.ContainsKey(pair.Key))
                {
                    continue;
                }
                
                tutorials[pair.Key].hasTriggered = loadedTutorials[pair.Key].hasTriggered;
            }
        }

        public void SaveData(GameData gameData)
        {
            gameData.tutorials = tutorials;
        }
    }
}
