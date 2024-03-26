using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private GameObject contentParent;
    [SerializeField] private QuestLogScrollingList scrollingList;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI currentStepText;
    [SerializeField] private TextMeshProUGUI stepsText;

    private Button _defaultSelectedButton;
    private void Start()
    {
        contentParent.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
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
    }

    private void OnEnable()
    {
        QuestEvents.OnQuestStateChange += QuestStateChange;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestStateChange -= QuestStateChange;
    }
    
    private void QuestStateChange(Quest quest)
    {
        if (quest.State == QuestState.InProgress)
        {
            QuestLogButton questLogButton = scrollingList.GetQuestLogButton(quest, () =>
            {
                SetQuestInfo(quest);
            });

            _defaultSelectedButton = questLogButton.button;
        }
        else if (quest.State == QuestState.Finished)
        {
            QuestLogButton questLogButton = scrollingList.MoveQuestLogButtonToFinishedCategory(quest, () =>
            {
                SetQuestInfo(quest);
            });
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
