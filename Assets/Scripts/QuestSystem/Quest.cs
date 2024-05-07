using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuestSystem
{
    public class Quest
    {
        [NonSerialized] public QuestSO Info;
        public QuestState State;
        public List<string> stepsDescriptions = new List<string>();
        [JsonProperty] private int _currentStepIndex;

        public Quest(QuestSO info)
        {
            Info = info;
            State = QuestState.RequirementsNotMet;
            _currentStepIndex = 0;
        }

        public bool CurrentStepExists()
        {
            return _currentStepIndex < Info.steps.Count;
        }
    
        public bool NextStepExists()
        {
            return _currentStepIndex + 1 < Info.steps.Count;
        }
    
        public void MoveToNextStep()
        {
            _currentStepIndex++;
        }

        public void InstantiateCurrentQuestStep(Transform parentTransform)
        {
            if (CurrentStepExists())
            {
                var questStep = Object.Instantiate(Info.steps[_currentStepIndex], parentTransform).GetComponent<BaseQuestStep>();
                questStep.InitializeQuestStep(Info.id);

                if (stepsDescriptions.Count > 0)
                {
                    if (!stepsDescriptions[stepsDescriptions.Count - 1].Equals(questStep.description))
                    {
                        stepsDescriptions.Add(questStep.description);
                    }
                }
                else
                {
                    stepsDescriptions.Add(questStep.description);
                }
            }
            else
            {
                Debug.LogWarning($"Quest with id: {Info.id} does not have step: {_currentStepIndex}");
            }
        }
    }

}
