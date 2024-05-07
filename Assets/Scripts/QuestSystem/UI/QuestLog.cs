using System.Linq;
using InputSystem;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem.UI
{
    public class QuestLog : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private GameObject contentParent;

        [SerializeField] private QuestLogScrollingList scrollingList;
        [SerializeField] private TextMeshProUGUI displayNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI currentStepText;
        [SerializeField] private TextMeshProUGUI stepsText;

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
            }
            else
            {
                contentParent.SetActive(true);
                if (_defaultSelectedButton is not null)
                {
                    _defaultSelectedButton.Select();
                }
            }
        }

        private void CloseQuestLog()
        {
            contentParent.SetActive(false);
        }

        private void QuestStateChange(Quest quest)
        {
            if (quest.State == QuestState.InProgress)
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

            currentStepText.color = quest.State == QuestState.Finished ? Color.gray : Color.white;

        }
    }
}
