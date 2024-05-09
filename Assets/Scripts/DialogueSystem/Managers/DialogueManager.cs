using System.Collections.Generic;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;
using QuestSystem;
using UnityEngine;

namespace DialogueSystem.Managers
{
    //class Storing information about all dialogues in game
    public class DialogueManager : MonoBehaviour
    {
        private Dictionary<string, List<Dialogue>> _dialoguesDictionary;
        private Dictionary<string, Dialogue> _currentDialogues;
        
        private void Awake()
        {
            _dialoguesDictionary = CreateDialogueDictionary();
            ChangeAllCurrentDialogues();
        }
        
        private void OnEnable()
        {
            QuestEvents.OnQuestStateChange += ChangeDialoguesState;
            DialogueEvents.OnDialogueUnlocked += UnlockDialogue;
            DialogueEvents.OnDialogueTriggered += StartDialogue;
        }
        
        private void OnDisable()
        {
            QuestEvents.OnQuestStateChange -= ChangeDialoguesState;
            DialogueEvents.OnDialogueUnlocked -= UnlockDialogue;
            DialogueEvents.OnDialogueTriggered -= StartDialogue;
        }
        
        private void Start()
        {
            foreach (var dialogues in _dialoguesDictionary.Values)
            {
                foreach (var dialogue in dialogues)
                {
                    if (dialogue.state == Dialogue.State.CannotTrigger && CheckIfDialogueCanTrigger(dialogue))
                    {
                        dialogue.state = Dialogue.State.CanTrigger;
                    }
                }
            }
            ChangeAllCurrentDialogues();
        }
        
        private void StartDialogue(string triggerName)
        {
            DialogueEvents.StartDialogue(_currentDialogues[triggerName].info);
            if (_currentDialogues[triggerName].info.relatedQuest is not null)
            {
                _dialoguesDictionary[triggerName].Find(e => e == _currentDialogues[triggerName]).state = 
                    Dialogue.State.Finished;
                ChangeCurrentDialogue(triggerName);
            }
        }
        
        private void ChangeCurrentDialogue(string triggerName)
        {
            foreach (var dialogue in _dialoguesDictionary[triggerName])
            {
                if (_currentDialogues[triggerName] is null)
                {
                    _currentDialogues[triggerName] = dialogue;
                    continue;
                }
        
                if (dialogue.state == Dialogue.State.CanTrigger)
                {
                    if (dialogue.currentPriority > _currentDialogues[triggerName].currentPriority ||
                        _currentDialogues[triggerName].state == Dialogue.State.CannotTrigger ||
                        _currentDialogues[triggerName].state == Dialogue.State.Finished)
                    {
                        _currentDialogues[triggerName] = dialogue;
                    }
                }
            }
        }
        
        private void ChangeAllCurrentDialogues()
        {
            foreach (var triggerName in _dialoguesDictionary.Keys)
            {
                ChangeCurrentDialogue(triggerName);
            }
        }
        
        private bool CheckIfDialogueCanTrigger(Dialogue dialogue)
        {
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
        
        private void ChangeDialoguesState(Quest quest)
        {
            foreach (var dialogues in _dialoguesDictionary.Values)
            {
                foreach (var dialogue in dialogues)
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
            }
            ChangeAllCurrentDialogues();
        }
        
        private void UnlockDialogue(string id)
        {
            var foundDialogue = false;
            foreach (var pair in _dialoguesDictionary)
            {
                foreach (var dialogue in pair.Value)
                {
                    if (dialogue.id.Equals(id))
                    {
                        foundDialogue = true;
                        ChangeCurrentDialogue(pair.Key);
                        break;
                    }
                }
                if (foundDialogue)
                {
                    break;
                }
            }
        }
        
        private Dictionary<string, List<Dialogue>> CreateDialogueDictionary()
        {
            DialogueSO[] allDialogues = Resources.LoadAll<DialogueSO>($"Dialogues");
            Dictionary<string, List<Dialogue>> dialoguesDictionary = new Dictionary<string, List<Dialogue>>();
            
            foreach (var dialogueInfo in allDialogues)
            {
                if (!dialoguesDictionary.ContainsKey(dialogueInfo.triggerName))
                {
                    dialoguesDictionary.Add(dialogueInfo.triggerName, new List<Dialogue>());
                }
                dialoguesDictionary[dialogueInfo.triggerName].Add(new Dialogue(dialogueInfo));
            }
            return dialoguesDictionary;
        }
    }
}
