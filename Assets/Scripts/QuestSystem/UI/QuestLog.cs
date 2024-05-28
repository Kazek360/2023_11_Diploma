using System.Linq;
using InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem.UI
{
    public class QuestLog : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private GameObject contentParent;
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
            InputEvents.OnQuestLogToggle += ToggleQuestLog;
            InputEvents.OnInterfaceClosed += CloseQuestLog;

            QuestEvents.OnQuestStateChange += QuestStateChange;
        }

        private void OnDisable()
        {
            InputEvents.OnQuestLogToggle -= ToggleQuestLog;
            InputEvents.OnInterfaceClosed -= CloseQuestLog;

            QuestEvents.OnQuestStateChange -= QuestStateChange;
        }

        private void Start()
        {
            contentParent.SetActive(false);
        }

        private void ToggleQuestLog()
        {
            if (contentParent.activeSelf)
            {
                contentParent.SetActive(false);
                GameConfig.QuestTracker.EnableText();
            }
            else
            {
                contentParent.SetActive(true);
                if (_defaultSelectedButton is not null)
                {
                    _defaultSelectedButton.Select();
                    GameConfig.QuestTracker.DisableText();
                }
            }
        }

        private void CloseQuestLog()
        {
            contentParent.SetActive(false);
        }

        private void QuestStateChange(Quest quest)
        {
            if (quest.State == QuestState.InProgress || quest.State == QuestState.CanFinish)
            {
                var questLogButton = scrollingList.GetQuestLogButton(quest, () => { SetQuestInfo(quest); });

                _defaultSelectedButton = questLogButton.button;
            }
            else if (quest.State == QuestState.Finished)
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
            
            displayNameText.text = quest.Info.displayName;
            descriptionText.text = quest.Info.description;
            currentStepText.text = quest.stepsDescriptions.Last();
            if (quest.stepsDescriptions.Count > 1)
            {
                stepsText.text = quest.stepsDescriptions[0];
                for (int i = 1; i < quest.stepsDescriptions.Count - 1; i++)
                {
                    stepsText.text = $"{quest.stepsDescriptions[i]}\n{stepsText.text}";
                }
            }
            else
            {
                stepsText.text = "";
            }

            if (quest.State == QuestState.Finished)
            {
                trackQuestButton.gameObject.SetActive(false);
                currentStepText.color = Color.gray;
            }
            else
            {
                trackQuestButton.gameObject.SetActive(true);
                currentStepText.color = Color.white;
            }

            trackQuestButtonText.text = quest.isTracked ?  "Przestań śledzić" : "Śledź";
        }

        public void SetTrackedQuest()
        {
            if (_selectedQuest.isTracked)
            {
                _selectedQuest.isTracked = false;
                GameConfig.QuestTracker.trackedQuest = null;
                trackQuestButtonText.text = "Śledź";
            }
            else
            {
                GameConfig.QuestTracker.trackedQuest = _selectedQuest;
                _selectedQuest.isTracked = true;
                trackQuestButtonText.text = "Przestań śledzić";
                GameConfig.QuestTracker.UpdateText();
            }
        }
    }
}
