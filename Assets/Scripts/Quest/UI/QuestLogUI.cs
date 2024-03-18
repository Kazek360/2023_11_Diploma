using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private GameObject contentParent;
    [SerializeField] private QuestLogScrollingList scrollingList;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private Button firstSelectedButton;
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
        QuestLogButton questLogButton = scrollingList.CreateButtonIfNotExists(quest, () =>
        {
            SetQuestLogInfo(quest);
        });

        if (firstSelectedButton is null)
        {
            firstSelectedButton = questLogButton.button;
            firstSelectedButton.Select();
        }
    }
    private void SetQuestLogInfo(Quest quest)
    {
        displayNameText.text = quest.Info.displayName;
        descriptionText.text = quest.Info.description;
    }
}
