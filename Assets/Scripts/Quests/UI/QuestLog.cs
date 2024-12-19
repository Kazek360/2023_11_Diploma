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
        public override UserInterfaceState State => UserInterfaceState.Quests;
        
        [Header("Components")] 
        [SerializeField] private QuestLogScrollingList scrollingList;
        [SerializeField] private TextMeshProUGUI displayNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI currentStepText;
        [SerializeField] private TextMeshProUGUI stepsText;
        [SerializeField] private Button trackQuestButton;
        [SerializeField] private RectTransform trackQuestButtonTransform;
        
        [Header("Additional")] 
        [SerializeField] private Sprite trackImage;
        [SerializeField] private Sprite stopTrackingImage;
        
        private Quest _selectedQuest;
        private Button _defaultSelectedButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            QuestEvents.OnQuestStateChange += QuestStateChange;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
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
                currentStepText.color = new Color(239f/255, 186f/255, 65f/255);
            }

            trackQuestButton.image.sprite = _selectedQuest.Equals(QuestManager.Instance.TrackedQuest) 
                ? stopTrackingImage : trackImage;
            trackQuestButtonTransform.sizeDelta = _selectedQuest.Equals(QuestManager.Instance.TrackedQuest) 
                ? new Vector2(350, trackQuestButtonTransform.sizeDelta.y) : 
                new Vector2(250, trackQuestButtonTransform.sizeDelta.y);;
        }

        public void SetTrackedQuest()
        {
            if (_selectedQuest.Equals(QuestManager.Instance.TrackedQuest))
            {
                QuestManager.Instance.TrackedQuest = null;
                trackQuestButton.image.sprite = trackImage;
                trackQuestButtonTransform.sizeDelta = new Vector2(250, trackQuestButtonTransform.sizeDelta.y);
            }
            else
            {
                QuestManager.Instance.TrackedQuest = _selectedQuest;
                trackQuestButton.image.sprite = stopTrackingImage;
                trackQuestButtonTransform.sizeDelta = new Vector2(350, trackQuestButtonTransform.sizeDelta.y);
            }
        }
    }
}
