using System.Linq;
using InputSystem;
using TMPro;
using UI;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Quests.UI
{
    public class QuestLog : UserInterface
    {
        public override GameState State => GameState.Quests;
        
        [Header("Components")] 
        [SerializeField] private QuestLogScrollingList scrollingList;
        [SerializeField] private TextMeshProUGUI displayNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI currentStepText;
        [SerializeField] private TextMeshProUGUI stepsText;
        [SerializeField] private Button trackQuestButton;
        [SerializeField] private TextMeshProUGUI trackQuestButtonText;

        private Quest _selectedQuest;
        private Button _defaultSelectedButton;

        private void OnEnable()
        {
            QuestEvents.OnQuestStateChange += QuestStateChange;
        }

        private void OnDisable()
        {
            QuestEvents.OnQuestStateChange -= QuestStateChange;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0;
            InputManager.Instance.Controlls.Player.Disable();
            if (_defaultSelectedButton is not null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                _defaultSelectedButton.Select();
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            InputManager.Instance.Controlls.Player.Enable();
        }
        
        private void QuestStateChange(Quest quest)
        {
            if (quest.state == QuestState.InProgress)
            {
                var questLogButton = scrollingList.GetQuestLogButton(quest, () => { SetQuestInfo(quest); });

                _defaultSelectedButton = questLogButton.button;
            }
            else if (quest.state == QuestState.Finished)
            {
                var questLogButton = scrollingList.GetQuestLogButton(quest, () => { SetQuestInfo(quest); });
                questLogButton =
                    scrollingList.MoveQuestLogButtonToFinishedCategory(quest, () => { SetQuestInfo(quest); });
                _defaultSelectedButton = questLogButton.button;
            }
        }

        private void SetQuestInfo(Quest quest)
        {
            _selectedQuest = quest;
            
            displayNameText.text = quest.info.displayName;
            descriptionText.text = quest.info.description;
            currentStepText.text = quest.stepsDescriptions.Last();
            if (quest.stepsDescriptions.Count > 1)
            {
                stepsText.text =  $"{quest.stepsDescriptions[0]}";
                for (int i = 1; i < quest.stepsDescriptions.Count - 1; i++)
                {
                    stepsText.text = $"{quest.stepsDescriptions[i]}\n{stepsText.text}";
                }
            }
            else
            {
                stepsText.text = "";
            }

            if (quest.state == QuestState.Finished)
            {
                trackQuestButton.gameObject.SetActive(false);
                currentStepText.color = Color.gray;
            }
            else
            {
                trackQuestButton.gameObject.SetActive(true);
                currentStepText.color = Color.white;
            }

            trackQuestButtonText.text = _selectedQuest.Equals(QuestManager.Instance.TrackedQuest) ? "Przestań śledzić" : "Śledź";
        }

        public void SetTrackedQuest()
        {
            if (_selectedQuest.Equals(QuestManager.Instance.TrackedQuest))
            {
                QuestManager.Instance.TrackedQuest = null;
                trackQuestButtonText.text = "Śledź";
            }
            else
            {
                QuestManager.Instance.TrackedQuest = _selectedQuest;
                trackQuestButtonText.text = "Przestań śledzić";
            }
        }
    }
}
