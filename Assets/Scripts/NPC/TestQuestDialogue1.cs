using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestDialogue1 : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private DialogueChooserScriptableObject dialogueChooser;
    [SerializeField] private List<string> flags;   //TODO: Replace with some real flag system
    private bool _playerInRange;

    [SerializeField] private QuestSO questInfo;
    private QuestState _currentQuestState;
    
    public void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.E) && _currentQuestState == QuestState.CanStart)
        {
            QuestEvents.StartQuest("EpicQuest");
            StartCoroutine(StartDialogue());
        }
    }

    private void OnEnable()
    {
        QuestEvents.OnQuestStateChange += ChangeQuestState;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestStateChange -= ChangeQuestState;
    }

    public void ChangeQuestState(Quest quest)
    {
        if (quest.Info.id == questInfo.id)
        {
            _currentQuestState = quest.State;
        }
    }
    
    
    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(1f);
        DialogueScriptableObject dialogue = dialogueChooser.ChooseDialogue(flags);
        dialogueWindow.GetComponent<DialogueWindowScript>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _playerInRange = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _playerInRange = false;
    }
}
