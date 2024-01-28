using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestDialogue2 : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private DialogueChooserScriptableObject dialogueChooser;
    [SerializeField] private List<string> flags;   //TODO: Replace with some real flag system
    private bool _playerInRange;

    [SerializeField] private QuestSO questInfo;
    private QuestState _currentQuestState;
    
    public void Update()
    {
        //Debug.Log($"Mona in range: {_playerInRange}");
        if (_playerInRange && Input.GetKeyDown(KeyCode.E) && _currentQuestState == QuestState.InProgress)
        {
            QuestEvents.ProgressQuest("EpicQuest");
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
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
}
