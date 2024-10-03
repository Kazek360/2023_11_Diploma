using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuestSystem
{
    public class Quest
    {
        [NonSerialized] public QuestSO Info;
        public QuestState State;
        [NonSerialized] public List<string> stepsDescriptions = new List<string>();
        
        [NonSerialized] private BaseQuestStep _currentStep;
        [JsonProperty] private int _currentStepIndex;
        [JsonProperty] private int _currentStepProgress;
        public Quest(QuestSO info)
        {
            Info = info;
            State = QuestState.RequirementsNotMet;
            _currentStepIndex = 0;
        }

        private bool CurrentStepExists()
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

        private void AddDescription()
        {
            if (_currentStep.Amount > 1)
            {
                stepsDescriptions.Add($"{_currentStep.description} ({_currentStep.Progress}/{_currentStep.Amount})");
            }
            else
            {
                stepsDescriptions.Add($"{_currentStep.description}");
            }
        }
        
        public void AddDescriptionsOnLoad()
        {
            for (var i = 0; i < Info.steps.Count; i++)
            {
                var step = Info.steps[i].GetComponent<BaseQuestStep>();
                
                if (step.Amount > 1)
                {
                    stepsDescriptions.Add($"{step.description} ({step.Amount}/{step.Amount})");
                }
                else
                {
                    stepsDescriptions.Add($"{step.description}");
                }
            }
        }
        
        public void UpdateDescription()
        {
            if (_currentStep.Amount > 1)
            {
                stepsDescriptions[_currentStepIndex] = $"{_currentStep.description} ({_currentStep.Progress}/{_currentStep.Amount})";
            }
            else
            {
                stepsDescriptions[_currentStepIndex] = $"{_currentStep.description}";
            }
        }

        public void UpdateProgress()
        {
            _currentStepProgress = _currentStep.Progress;
        }
        
        public void InstantiateCurrentStep(Transform parentTransform)
        {
            _currentStep = Object.Instantiate(Info.steps[_currentStepIndex], parentTransform).GetComponent<BaseQuestStep>();
            _currentStep.InitializeQuestStep(Info.id);
            _currentStepProgress = 0;
            AddDescription();
        }
        public void InstantiateCurrentStepFromSaveFile(Transform parentTransform)
        {
            _currentStep = Object.Instantiate(Info.steps[_currentStepIndex], parentTransform).GetComponent<BaseQuestStep>();
            _currentStep.InitializeQuestStep(Info.id);
            _currentStep.Progress = _currentStepProgress;
            AddDescriptionsOnLoad();
        }
    }

}
