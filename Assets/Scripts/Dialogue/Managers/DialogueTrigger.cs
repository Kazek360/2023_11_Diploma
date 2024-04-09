using System.Collections.Generic;
using UnityEngine;

//Class storing NPC dialogues and allowing for their execution in a dialogue window.
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string locationName;
    [SerializeField] private string triggerName;
    
    private Dictionary<string, Dialogue> _dialogueDictionary;
    private Dialogue _currentDialogue;
    private bool _playerInRange;
    
    private void OnEnable()
    {
        QuestEvents.OnQuestStateChange += ChangeDialogueState;
        DialogueEvents.OnDialogueMarkerStateChange += ChangeDialogueState;
        DialogueEvents.OnDialogueUnlocked += UnlockDialogue;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestStateChange -= ChangeDialogueState;
        DialogueEvents.OnDialogueMarkerStateChange -= ChangeDialogueState;
        DialogueEvents.OnDialogueUnlocked -= UnlockDialogue;
    }
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

    public void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueEvents.StartDialogue(_currentDialogue);
            
            if (_currentDialogue.Info.questProgress != DialogueQuestProgress.None)
            {
                _dialogueDictionary[_currentDialogue.Info.id].State = DialogueState.Finished;
                ChangeCurrentDialogue();
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
                if (dialogue.currentPriority > _currentDialogue.currentPriority ||
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
            if (dialogue.Info.relatedQuest is not null)
            {
                if (quest.Info.id == dialogue.Info.relatedQuest.id)
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
    
    private void ChangeDialogueState(string id)
    {
        foreach (var dialogue in _dialogueDictionary.Values)
        {
            if (dialogue.Info.requiredFlags.Count > 0)
            {
                if (dialogue.State == DialogueState.NotUnlocked && CheckIfDialogueCanTrigger(dialogue))
                {
                    dialogue.State = DialogueState.CanTrigger;
                }
            }
        }
        ChangeCurrentDialogue();
    }
    
    private bool CheckIfDialogueCanTrigger(Dialogue dialogue)
    {
        foreach (var questRequirement in dialogue.Info.requiredQuests)
        {
            if (QuestManager.GetQuestById(questRequirement.id).State != QuestState.Finished)
            {
                return false;
            }
        }
        foreach (var flagRequirement in dialogue.Info.requiredFlags)
        {
            if (!DialogueMarkerManager.GetMarkerById(flagRequirement.id)._isTriggered)
            {
                return false;
            }
        }
        
        if (dialogue.Info.relatedQuest is not null)
        {
            if (dialogue.Info.questProgress == DialogueQuestProgress.Progress)
            {
                return false;
            }
            if (dialogue.Info.questProgress == DialogueQuestProgress.Start)
            {
                if (QuestManager.GetQuestById(dialogue.Info.relatedQuest.id).State != QuestState.CanStart)
                {
                    return false;
                }
            }
            else if (dialogue.Info.questProgress == DialogueQuestProgress.Finish)
            {
                if (QuestManager.GetQuestById(dialogue.Info.relatedQuest.id).State != QuestState.CanFinish)
                {
                    return false;
                }
            }
        }
        return true;
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
