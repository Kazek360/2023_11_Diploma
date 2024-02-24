using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
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
            DialogueEvents.StartDialogue(_currentDialogue);
            
            switch (_currentDialogue.Info.questProgress)
            {
                case DialogueQuestProgress.Start:
                    if (_currentQuestState == QuestState.CanStart)
                    {
                        QuestEvents.StartQuest(_currentDialogue.Info.quest.id);
                        _dialogueDictionary[_currentDialogue.Info.id].State = DialogueState.Finished;
                        ChangeCurrentDialogue();
                    }
                    break;
                case DialogueQuestProgress.Progress:
                    if (_currentQuestState == QuestState.InProgress)
                    {
                        QuestEvents.ProgressQuest(_currentDialogue.Info.quest.id);
                        _dialogueDictionary[_currentDialogue.Info.id].State = DialogueState.Finished;
                        ChangeCurrentDialogue();
                    }
                    break;
                case DialogueQuestProgress.Finish:
                    if (_currentQuestState == QuestState.CanFinish)
                    {
                        QuestEvents.FinishQuest(_currentDialogue.Info.quest.id);
                        _dialogueDictionary[_currentDialogue.Info.id].State = DialogueState.Finished;
                        ChangeCurrentDialogue();
                    }
                    break;
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

            if (dialogue.State == DialogueState.CanTrigger)
            {
                if (dialogue.Info.priority > _currentDialogue.Info.priority ||
                    _currentDialogue.State == DialogueState.Finished)
                {
                    _currentDialogue = dialogue;
                }
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
            Debug.LogError($"There is no quest with ID: {dialogue.Info}");
        }
        
        return dialogue;
    }
    
    
    
}
