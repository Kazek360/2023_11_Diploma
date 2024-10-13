using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Quests
{
    public class Quest
    {
        [NonSerialized] public QuestInfo info;
        public QuestState state;
        [NonSerialized] public List<string> stepsDescriptions = new List<string>();
        
        [NonSerialized] private BaseQuestStep _currentStep;
        [JsonProperty] private int _currentStepIndex;
        [JsonProperty] private int _currentStepProgress;
        public Quest(QuestInfo info)
        {
            this.info = info;
            state = QuestState.RequirementsNotMet;
            _currentStepIndex = 0;
        }
    
        public bool NextStepExists()
        {
            return _currentStepIndex + 1 < info.steps.Count;
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
        
        public void AddCompletedDescriptions()
        {
            for (var i = 0; i < info.steps.Count; i++)
            {
                var step = info.steps[i].GetComponent<BaseQuestStep>();
                
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
        
        private void AddDescriptionsToQuestInProgress()
        {
            for (var i = 0; i < info.steps.Count - 1; i++)
            {
                var step = info.steps[i].GetComponent<BaseQuestStep>();
                
                if (step.Amount > 1)
                {
                    stepsDescriptions.Add($"{step.description} ({step.Amount}/{step.Amount})");
                }
                else
                {
                    stepsDescriptions.Add($"{step.description}");
                }
            }
            AddDescription();
        }
        
        
        public void UpdateProgress()
        {
            _currentStepProgress = _currentStep.Progress;
        }
        
        public void InstantiateCurrentStep(Transform parentTransform)
        {
            _currentStep = Object.Instantiate(info.steps[_currentStepIndex], parentTransform).GetComponent<BaseQuestStep>();
            _currentStep.InitializeQuestStep(info.name);
            _currentStepProgress = 0;
            AddDescription();
        }
        public void InstantiateCurrentStepFromSaveFile(Transform parentTransform)
        {
            _currentStep = Object.Instantiate(info.steps[_currentStepIndex], parentTransform).GetComponent<BaseQuestStep>();
            _currentStep.InitializeQuestStep(info.name);
            _currentStep.Progress = _currentStepProgress;
            AddDescriptionsToQuestInProgress();
        }
    }

}
