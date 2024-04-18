using System.Collections.Generic;
using UnityEngine;
using DialogueSystem.Events;
using InputSystem;

namespace DialogueSystem.Managers
{
    //Class storing NPC dialogues and allowing for their execution in a dialogue window.
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Dialogues Location")]
        [SerializeField] private string locationName;
        [SerializeField] private string triggerName;
        
        [Header("Interaction")]
        [SerializeField] private GameObject interactionKeyImage;
        
        [Header("Auto")]
        [SerializeField] private bool autoTrigger;
        
        private Dictionary<string, Dialogue> _dialogueDictionary;
        private Dialogue _currentDialogue;
        
        private bool _playerInRange;
        private bool _disabled;

        private void OnEnable()
        {
            InputEvents.OnInteractionInput += TriggerDialogueByInteraction;
            AreaEvent.OnCorrectAreaEnter += TriggerDialogueByAreaEnter;
            QuestEvents.OnQuestStateChange += ChangeDialogueState;
            DialogueEvents.OnDialogueUnlocked += UnlockDialogue;
            PathEvents.OnMovementStarted += DisableTrigger;
            PathEvents.OnMovementEnded += EnableTrigger;
            
        }
        
        private void OnDisable()
        {
            InputEvents.OnInteractionInput -= TriggerDialogueByInteraction;
            AreaEvent.OnCorrectAreaEnter -= TriggerDialogueByAreaEnter;
            QuestEvents.OnQuestStateChange -= ChangeDialogueState;
            DialogueEvents.OnDialogueUnlocked -= UnlockDialogue;
            PathEvents.OnMovementStarted -= DisableTrigger;
            PathEvents.OnMovementEnded -= EnableTrigger;
            
        }

        private void Awake()
        {
            if (!autoTrigger)
            {
                interactionKeyImage.SetActive(false); 
            }
            _playerInRange = false;
            _disabled = false;
            _dialogueDictionary = CreateDialogueDictionary();
            ChangeCurrentDialogue();
        }

        private void Start()
        {
            foreach (var dialogue in _dialogueDictionary.Values)
            {
                if (dialogue.state == Dialogue.State.CannotTrigger && CheckIfDialogueCanTrigger(dialogue))
                {
                    dialogue.state = Dialogue.State.CanTrigger;
                }
            }
            ChangeCurrentDialogue();
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
        
                if (dialogue.state == Dialogue.State.CanTrigger)
                {
                    if (dialogue.currentPriority > _currentDialogue.currentPriority ||
                        _currentDialogue.state == Dialogue.State.CannotTrigger ||
                        _currentDialogue.state == Dialogue.State.Finished)
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
                if (dialogue.info.relatedQuest is not null)
                {
                    if (quest.Info.id == dialogue.info.relatedQuest.id)
                    {
                        if (dialogue.state == Dialogue.State.CannotTrigger && CheckIfDialogueCanTrigger(dialogue))
                        {
                            dialogue.state = Dialogue.State.CanTrigger;
                        }
                    }
                }
            }
        
            ChangeCurrentDialogue();
        }
        
        private bool CheckIfDialogueCanTrigger(Dialogue dialogue)
        {
            foreach (var questRequirement in dialogue.info.requiredQuests)
            {
                if (QuestManager.GetQuestById(questRequirement.id).State != QuestState.Finished)
                {
                    return false;
                }
            }
            
            if (dialogue.info.relatedQuest is not null)
            {
                if (dialogue.info.startsQuest)
                {
                    if (QuestManager.GetQuestById(dialogue.info.relatedQuest.id).State != QuestState.CanStart)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        
        private void TriggerDialogue()
        {
            DialogueEvents.StartDialogue(_currentDialogue.info);
            if (_currentDialogue.info.relatedQuest is not null)
            {
                _dialogueDictionary[_currentDialogue.info.name].state = Dialogue.State.Finished;
                ChangeCurrentDialogue();
            }
        }
        private void TriggerDialogueByInteraction()
        {
            if (autoTrigger || _disabled || !_playerInRange)
            {
                return;
            }
            TriggerDialogue();
        }
        
        private void TriggerDialogueByAreaEnter(string areaName)
        {
            if (!areaName.Equals(gameObject.name))
            {
                return;
            }
            TriggerDialogue();
        }
        
        private void UnlockDialogue(string id)
        {
            if (_dialogueDictionary.ContainsKey(id))
            {
                _dialogueDictionary[id].state = Dialogue.State.CanTrigger;
                ChangeCurrentDialogue();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            if (autoTrigger)
            {
                AreaEvent.EnterArea(gameObject.name, other.name);
            }
            else
            {
                interactionKeyImage.SetActive(true);
                _playerInRange = true;  
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (autoTrigger || !other.CompareTag("Player"))
            {
                return;
            }
            interactionKeyImage.SetActive(false);
            _playerInRange = false;
        }

        private Dictionary<string, Dialogue> CreateDialogueDictionary()
        {
            DialogueSO[] allTriggerDialogues = Resources.LoadAll<DialogueSO>($"Dialogues/{locationName}/{triggerName}");
            Dictionary<string, Dialogue> idToDictionary = new Dictionary<string, Dialogue>();
            foreach (var dialogueInfo in allTriggerDialogues)
            {
                if (idToDictionary.ContainsKey(dialogueInfo.name))
                {
                    Debug.LogWarning($"Found Duplicate ID: {dialogueInfo.name}");
                }
                idToDictionary.Add(dialogueInfo.name, new Dialogue(dialogueInfo));
            }
            return idToDictionary;
        }

        private void DisableTrigger()
        {
            _disabled = true;
        }
        private void EnableTrigger()
        {
            _disabled = false;
        }
    }
}

