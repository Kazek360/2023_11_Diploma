using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Quests.QuestSteps;
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
    
        private void InstantiateStep(Transform parentTransform, int progress = 0)
        {
            _currentStep = Object.Instantiate(_currentStepInfo.prefab, parentTransform).GetComponent<BaseQuestStep>();
            _currentStep.InitializeQuestStep(this, _currentStepInfo);
            if (_currentStepInfo.repeatAmount > 0)
            {
                currentStepProgress = progress;
                AddDescriptionWithProgress();
            }
            else
            {
                AddDescription();
            }
        }

        public void InstantiateFirstStep(Transform parentTransform)
        {
            ChooseFirstStep();
            InstantiateStep(parentTransform);
        }
        
        public void InstantiateNextStep(Transform parentTransform)
        {
            ChooseNextStep();
            InstantiateStep(parentTransform);
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
                PassThroughCompletedSteps();
                ChooseNextStep();
                InstantiateStep(parentTransform, currentStepProgress);
            }
        }
        
        public bool NextStepExists()
        {
            return _currentStepInfo.transitions.Count > 0;
        }

        private void AddDescription()
        {
            if (_currentStepInfo.isHidden)
            {
                return;
            }
            stepsDescriptions.Add($"{_currentStepInfo.description}");
        }
        
        private void AddDescriptionWithProgress()
        {
            if (_currentStepInfo.isHidden)
            {
                return;
            }
            stepsDescriptions.Add($"{_currentStepInfo.description} ({_currentStep.Progress}/{_currentStepInfo.repeatAmount})");
        }
        
        private void AddCompletedDescription()
        {
            if (_currentStepInfo.isHidden)
            {
                return;
            }
            if (_currentStepInfo.repeatAmount > 0)
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
            if (!_currentStepInfo.isHidden)
            {
                stepsDescriptions[_currentStepIndex - 1] = $"{_currentStepInfo.description} ({_currentStep.Progress}" +
                                                           $"/{_currentStepInfo.repeatAmount})";
            }
            currentStepProgress = _currentStep.Progress;
        }
        
        private void ChooseFirstStep()
        {
            foreach (var transition in info.firstStepTransitions)
            {
                if (QuestManager.Instance.CheckForRequiredFlagsState(transition.requiredFlags))
                {
                    _currentStepInfo = transition.nextStep;
                    break;
                }
            }
        }
        
        private void ChooseNextStep()
        {
            foreach (var transition in _currentStepInfo.transitions)
            {
                if (QuestManager.Instance.CheckForRequiredFlagsState(transition.requiredFlags))
                {
                    _currentStepInfo = transition.nextStep;
                    break;
                }
            }
        }
        
        public void PassThroughCompletedSteps()
        {
            ChooseFirstStep();
            AddCompletedDescription();
            for (var i = 1; i < _currentStepIndex; i++)
            {
                ChooseNextStep();
                AddCompletedDescription();
            }
        }
    }
}
