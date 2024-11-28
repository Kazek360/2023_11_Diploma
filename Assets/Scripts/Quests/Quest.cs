using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Quests.ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Quests
{
    public class Quest
    {
        [NonSerialized] public QuestInfo info;
        public QuestState state;
        [NonSerialized] public List<string> stepsDescriptions = new List<string>();
        
        [NonSerialized] private QuestStepInfo _currentStepInfo;
        [NonSerialized] private BaseQuestStep _currentStep;
        
        [JsonProperty] private int _currentStepIndex;
        [JsonProperty] public int currentStepProgress { get; private set; }
        public Quest(QuestInfo info)
        {
            this.info = info;
            state = QuestState.RequirementsNotMet;
            _currentStepIndex = 0;
        }
    
        public bool NextStepExists()
        {
            return _currentStepInfo.nextSteps.Count > 0;
        }

        private void AddDescription()
        {
            stepsDescriptions.Add($"{_currentStepInfo.description}");
        }
        
        private void AddDescriptionWithProgress()
        {
            stepsDescriptions.Add($"{_currentStepInfo.description} ({_currentStep.Progress}/{_currentStepInfo.repeatAmount})");
        }
        
        private void AddCompletedDescription()
        {
            if (_currentStepInfo.repeatAmount > 1)
            {
                stepsDescriptions.Add($"{_currentStepInfo.description} " +
                                      $"({_currentStepInfo.repeatAmount}/{_currentStepInfo.repeatAmount})");
            }
            else
            {
                AddDescription();
            }
        }
        
        public void UpdateStepProgress()
        {
            stepsDescriptions[_currentStepIndex - 1] = $"{_currentStepInfo.description} ({_currentStep.Progress}" +
                                                       $"/{_currentStepInfo.repeatAmount})";
            currentStepProgress = _currentStep.Progress;
        }
        
        public void AddCompletedQuestDescriptions()
        {
            _currentStepInfo = info.firstStep;
            AddCompletedDescription();
            for (var i = 1; i < _currentStepIndex; i++)
            {
                foreach (var nextStepInfo in _currentStepInfo.nextSteps)
                {
                    if (QuestManager.Instance.CheckForRequiredFlagsState(nextStepInfo.requiredFlags))
                    {
                        _currentStepInfo = nextStepInfo;
                        break;
                    }
                }
                AddCompletedDescription();
            }
        }
        
        public void InstantiateFirstStep(Transform parentTransform)
        {
            _currentStepInfo = info.firstStep;
            _currentStep = Object.Instantiate(_currentStepInfo.prefab, parentTransform).GetComponent<BaseQuestStep>();
            _currentStep.InitializeQuestStep(this, _currentStepInfo);
            if (_currentStepInfo.repeatAmount > 1)
            {
                currentStepProgress = 0;
                AddDescriptionWithProgress();
            }
            else
            {
                AddDescription();
            }
        }
        
        public void InstantiateNextStep(Transform parentTransform, int progress = 0)
        {
            foreach (var nextStepInfo in _currentStepInfo.nextSteps)
            {
                if (QuestManager.Instance.CheckForRequiredFlagsState(nextStepInfo.requiredFlags))
                {
                    _currentStepInfo = nextStepInfo;
                    break;
                }
            }
            
            _currentStep = Object.Instantiate(_currentStepInfo.prefab, parentTransform).GetComponent<BaseQuestStep>();
            _currentStep.InitializeQuestStep(this, _currentStepInfo);
            if (_currentStepInfo.repeatAmount > 1)
            {
                currentStepProgress = progress;
                AddDescriptionWithProgress();
            }
            else
            {
                AddDescription();
            }

            _currentStepIndex++;
        }
        public void InstantiateCurrentStep(Transform parentTransform)
        {
            if (_currentStepIndex == 0)
            {
                InstantiateFirstStep(parentTransform);
            }
            else
            {
                _currentStepInfo = info.firstStep;
                AddCompletedDescription();
                for (var i = 1; i < _currentStepIndex - 1; i++)
                {
                    foreach (var nextStepInfo in _currentStepInfo.nextSteps)
                    {
                        if (QuestManager.Instance.CheckForRequiredFlagsState(nextStepInfo.requiredFlags))
                        {
                            _currentStepInfo = nextStepInfo;
                            break;
                        }
                    }
                    AddCompletedDescription();
                }
                InstantiateNextStep(parentTransform, currentStepProgress);
            }
        }
    }
}
