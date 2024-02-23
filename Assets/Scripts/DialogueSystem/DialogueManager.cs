using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private string locationName;
    [SerializeField] private string characterName;
    
    private Dictionary<string, Dialogue> _dialogueDictionary;
    private Dialogue _currentDialogue;
    
    private QuestState _currentQuestState;
    private bool _playerInRange;
    
    private void Awake()
    {
        _dialogueDictionary = CreateDialogueDictionary();
        ChangeCurrentDialogue();
    }
    
    private void OnEnable()
    {
        QuestEvents.OnQuestStateChange += ChangeQuestState;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestStateChange -= ChangeQuestState;
    }

    public void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (_currentDialogue.Info.questProgress == DialogueQuestProgress.Start)
            {
                if (_currentQuestState == QuestState.CanStart)
                {
                    QuestEvents.StartQuest("EpicQuest");
                    dialogueWindow.GetComponent<DialogueWindow>().StartDialogue(_currentDialogue.Info);
                }
            }
            else if (_currentDialogue.Info.questProgress == DialogueQuestProgress.Progress)
            {
                if (_currentQuestState == QuestState.InProgress)
                {
                    QuestEvents.ProgressQuest("EpicQuest");
                    dialogueWindow.GetComponent<DialogueWindow>().StartDialogue(_currentDialogue.Info);
                }
            }
            else if (_currentDialogue.Info.questProgress == DialogueQuestProgress.Finish)
            {
                if (_currentQuestState == QuestState.CanFinish)
                {
                    QuestEvents.FinishQuest("EpicQuest");
                    dialogueWindow.GetComponent<DialogueWindow>().StartDialogue(_currentDialogue.Info);
                }
            }
        }
    }
    
    public void ChangeCurrentDialogue()
    {
        foreach (var dialogue in _dialogueDictionary.Values)
        {
            if (_currentDialogue is null)
            {
                _currentDialogue = dialogue;
                continue;
            }

            if (_currentDialogue.Info.priority < dialogue.Info.priority)
            {
                _currentDialogue = dialogue;
            }
        }
    }
    
    public void ChangeQuestState(Quest quest)
    {
        if (_currentDialogue.Info.quest is not null)
        {
            if (quest.Info.id == _currentDialogue.Info.quest.id)
            {
                _currentQuestState = quest.State;
            }  
        }
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
    
    private Dictionary<string, Dialogue> CreateDialogueDictionary()
    {
        DialogueSO[] allDialogues = Resources.LoadAll<DialogueSO>($"Dialogues/{locationName}/{characterName}");
        
        Dictionary<string, Dialogue> idToMap = new Dictionary<string, Dialogue>();
        
        foreach (var dialogueInfo in allDialogues)
        {
            if (idToMap.ContainsKey(dialogueInfo.id))
            {
                Debug.LogWarning($"Found Duplicate ID: {dialogueInfo.id}");
            }
            idToMap.Add(dialogueInfo.id, new Dialogue(dialogueInfo));
        }

        return idToMap;
    }

    private Dialogue GetDialogueById(string id)
    {
        Dialogue dialogue = _dialogueDictionary[id];
        if (dialogue == null)
        {
            //Debug.LogError($"There is no quest with ID: {dialogue.Info}");
        }
        
        return dialogue;
    }
    
    
    
}
