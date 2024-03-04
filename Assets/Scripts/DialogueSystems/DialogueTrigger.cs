using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string locationName;
    [SerializeField] private string triggerName;
    
    private Dictionary<string, Dialogue> _dialogueDictionary;
    private Dialogue _currentDialogue;
    
    private bool _playerInRange;
    
    private void Awake()
    {
        _dialogueDictionary = CreateDialogueDictionary();
        ChangeCurrentDialogue();
    }
    private void Start()
    {
        foreach (var dialogue in _dialogueDictionary.Values)
        {
            if (dialogue.State == DialogueState.NotUnlocked && CheckIfDialogueCanTrigger(dialogue))
            {
                dialogue.State = DialogueState.CanTrigger;
            }
        }
        ChangeCurrentDialogue();
    }

    private void OnEnable()
    {
        QuestEvents.OnQuestStateChange += ChangeDialogueState;
        DialogueEvents.OnDialogueUnlocked += UnlockDialogue;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestStateChange -= ChangeDialogueState;
        DialogueEvents.OnDialogueUnlocked -= UnlockDialogue;
    }

    public void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueEvents.StartDialogue(_currentDialogue);
            
            switch (_currentDialogue.Info.questProgress)
            {
                case DialogueQuestProgress.Start:
                    QuestEvents.StartQuest(_currentDialogue.Info.quest.id);
                    _dialogueDictionary[_currentDialogue.Info.id].State = DialogueState.Finished;
                    ChangeCurrentDialogue();
                    break;
                case DialogueQuestProgress.Progress:
                    _dialogueDictionary[_currentDialogue.Info.id].State = DialogueState.Finished;
                    ChangeCurrentDialogue();
                    break;
                case DialogueQuestProgress.Finish:
                    QuestEvents.FinishQuest(_currentDialogue.Info.quest.id);
                    _dialogueDictionary[_currentDialogue.Info.id].State = DialogueState.Finished;
                    ChangeCurrentDialogue();
                    break;
            }
        }
    }
    
    private void ChangeCurrentDialogue()
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
                    _currentDialogue.State == DialogueState.NotUnlocked ||
                    _currentDialogue.State == DialogueState.Finished)
                {
                    _currentDialogue = dialogue;
                }
            }
        } 
    }
    
    private void ChangeDialogueState(Quest quest)
    {
        foreach (var dialogue in _dialogueDictionary.Values)
        {
            if (dialogue.Info.quest is not null)
            {
                if (quest.Info.id == dialogue.Info.quest.id)
                {
                    if (dialogue.State == DialogueState.NotUnlocked && CheckIfDialogueCanTrigger(dialogue))
                    {
                        dialogue.State = DialogueState.CanTrigger;
                    }
                }  
            }
        }
        ChangeCurrentDialogue();
    }
    
    private bool CheckIfDialogueCanTrigger(Dialogue dialogue)
    {
        bool canTrigger = true;
        foreach (var questRequirement in dialogue.Info.requirements)
        {
            if (QuestManager.GetQuestById(questRequirement.id).State != QuestState.Finished)
            {
                canTrigger = false;
            }
        }

        if (dialogue.Info.quest is not null)
        {
            if (dialogue.Info.questProgress == DialogueQuestProgress.Start)
            {
                if (QuestManager.GetQuestById(dialogue.Info.quest.id).State != QuestState.CanStart)
                {
                    canTrigger = false;
                }
            }
            else if (dialogue.Info.questProgress == DialogueQuestProgress.Progress)
            {
                canTrigger = false;
            }
            else if (dialogue.Info.questProgress == DialogueQuestProgress.Finish)
            {
                if (QuestManager.GetQuestById(dialogue.Info.quest.id).State != QuestState.CanFinish)
                {
                    canTrigger = false;
                }
            }
        }

        return canTrigger;
    }

    private void UnlockDialogue(string id)
    {
        if (_dialogueDictionary.ContainsKey(id))
        {
            _dialogueDictionary[id].State = DialogueState.CanTrigger;
            ChangeCurrentDialogue(); 
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
        DialogueSO[] allDialogues = Resources.LoadAll<DialogueSO>($"Dialogues/{locationName}/{triggerName}");
        
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
}
