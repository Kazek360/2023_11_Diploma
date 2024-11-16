using System.Collections.Generic;
using DialogueSystem.Events;
using Quests;
using Quests.ScriptableObjects;
using UnityEngine;

namespace DialogueSystem.Managers
{
    //class Storing information about all dialogues in game
    public class DialogueManager : MonoBehaviour
    {
        private Dictionary<string, DialogueEntity> _dialogueEntityDictionary;
        
        private void Awake()
        {
            _dialogueEntityDictionary = CreateDialogueEntityDictionary();
        }
        
        private void OnEnable()
        {
            DialogueEvents.OnDialogueTriggered += StartDialogue;
            DialogueEvents.OnDialogueUnlocked += UnlockDialogue;
        }
        
        private void OnDisable()
        {
            DialogueEvents.OnDialogueTriggered -= StartDialogue;
            DialogueEvents.OnDialogueUnlocked -= UnlockDialogue;
        }
        
        private void Start()
        {
            foreach (var entity in _dialogueEntityDictionary.Values)
            {
                foreach (var dialogue in entity.dialogues)
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
            var currentEntity = _dialogueEntityDictionary[triggerName];
            
            DialogueEvents.StartDialogue(currentEntity.currentDialogue.info);
            
            if (currentEntity.currentDialogue.info.dialogueType != DialogueInfo.Type.Repeatable)
            {
                currentEntity.currentDialogue.state = Dialogue.State.Finished;
                currentEntity.ChangeCurrentDialogue();
            }
        }
        
        private void ChangeAllCurrentDialogues()
        {
            foreach (var entity in _dialogueEntityDictionary.Values)
            {
                entity.ChangeCurrentDialogue();
            }
        }
        
        private bool CheckIfDialogueCanTrigger(Dialogue dialogue)
        {
            if (dialogue.info.dialogueType != DialogueInfo.Type.Repeatable)
            {
                if (dialogue.info.dialogueType == DialogueInfo.Type.QuestStarter)
                {
                    if (QuestManager.Instance.GetQuestById(dialogue.info.questToStart.name).state != QuestState.CanStart)
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
        
        private void UnlockDialogue(string triggerName, string dialogueName)
        {
            var entity = _dialogueEntityDictionary[triggerName];
            var dialogue = entity.dialogues.Find((match) => match.info.name.Equals(dialogueName));

            if (dialogue != null)
            {
                dialogue.state = Dialogue.State.CanTrigger;
                entity.ChangeCurrentDialogue();
            }
        }
        
        private Dictionary<string, DialogueEntity> CreateDialogueEntityDictionary()
        {
            var allDialogues = Resources.LoadAll<DialogueInfo>("Dialogues");
            Dictionary<string, DialogueEntity> dictionary = new Dictionary<string, DialogueEntity>();
            foreach (var dialogueInfo in allDialogues)
            {
                if (!dictionary.ContainsKey(dialogueInfo.triggerName))
                {
                    dictionary.Add(dialogueInfo.triggerName, new DialogueEntity());
                }
                dictionary[dialogueInfo.triggerName].dialogues.Add(new Dialogue(dialogueInfo));
            }

            return dictionary;
        }
    }
}
