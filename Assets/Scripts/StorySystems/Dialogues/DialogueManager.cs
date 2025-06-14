using System.Collections.Generic;
using Quests;
using Shared;
using StorySystems.Dialogues.Trigger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StorySystems.Dialogues
{
    /// <summary>
    /// class Storing information about all dialogues in game
    /// </summary>
    public class DialogueManager : Singleton<DialogueManager>
    {
        private Dictionary<string, DialogueEntity> _dialogueEntityDictionary = new();
        private DialogueInfo[] _allDialoguesOnScene;
        public static bool isInitialized;
        
        protected override void Awake()
        {
            base.Awake();
            _allDialoguesOnScene = LoadDialogues();
            isInitialized = false;
        }
        
        private void OnEnable()
        {
            DialogueEvent.OnEntitySubscribed += InitializeEntity;
            DialogueEvent.OnDialogueUnlocked += UnlockDialogue;
        }
        
        private void OnDisable()
        {
            DialogueEvent.OnDialogueUnlocked -= UnlockDialogue;
            DialogueEvent.OnEntitySubscribed -= InitializeEntity;
        }

        private void Start()
        {
            DialogueEvent.Ping();
            isInitialized = true;
        }

        /// <summary>
        /// Metoda sprawdzająca, czy dialog może zostać uruchomiony
        /// </summary>
        /// <param name="dialogue">Sprawdzany dialog</param>
        /// <returns>Zwraca boola mówiącego, czy dialog może zostać uruchomiony</returns>
        private bool CheckIfDialogueCanTrigger(Dialogue dialogue)
        {
            switch (dialogue.info.dialogueType)
            {
                case DialogueInfo.Type.Repeatable or DialogueInfo.Type.Unrepeatable:
                    return true;
                case DialogueInfo.Type.QuestStarter:
                    return QuestManager.Instance.GetQuestById(dialogue.info.questToStart.name).state ==
                           QuestState.CanStart;
                case DialogueInfo.Type.SideQuest or DialogueInfo.Type.MainQuest:
                    return false;
            }
            return false;
        }
        
        /// <summary>
        /// Metoda ustawiająca stan dialogu dla określonego triggera na CanTrigger
        /// </summary>
        /// <param name="triggerName">nazwa triggera, dla DialogueEntity </param>
        /// <param name="dialogueName">nazwa dialogu, którego stan chcemy zmienić</param>
        private void UnlockDialogue(string triggerName, string dialogueName)
        {
            var entity = _dialogueEntityDictionary[triggerName];
            var dialogue = entity.dialogues.Find(match => match.info.name.Equals(dialogueName));

            if (dialogue != null)
            {
                dialogue.state = Dialogue.State.CanTrigger;
                entity.ChangeCurrentDialogue();
            }
        }
        
        /// <summary>
        /// Metoda pozwalająca uruchomić dialog o określonej nazwie triggera
        /// </summary>
        /// <param name="triggerName">nazwa triggera, dla DialogueEntity </param>
        public void TriggerDialogue(string triggerName)
        {
            _dialogueEntityDictionary[triggerName].StartDialogue();
        }

        /// <summary>
        /// Metoda inicjalizująca obiekt EntityDialogue
        /// </summary>
        /// <param name="triggerName">nazwa triggera, dla DialogueEntity </param>
        /// <param name="entity">Obiekt do zinicjalizowania</param>
        private void InitializeEntity(string triggerName, DialogueEntity entity)
        {
            if (_dialogueEntityDictionary.ContainsKey(triggerName))
            {
                return;
            }
            _dialogueEntityDictionary.Add(triggerName, entity);
            
            foreach (var dialogue in _allDialoguesOnScene)
            {
                if (dialogue.triggerName == triggerName)
                {
                    _dialogueEntityDictionary[triggerName].dialogues.Add(new Dialogue(dialogue));
                }
            }
            
            foreach (var dialogue in _dialogueEntityDictionary[triggerName].dialogues)
            {
                if (dialogue.state == Dialogue.State.CannotTrigger && CheckIfDialogueCanTrigger(dialogue))
                {
                    dialogue.state = Dialogue.State.CanTrigger;
                }
            }
            _dialogueEntityDictionary[triggerName].ChangeCurrentDialogue();
        }
        
        /// <summary>
        /// Metoda ładująca wszystkie DialogueInfo dostępne w folderze o nazwie sceny
        /// </summary>
        /// <returns>Zwraca tablicę DialogueInfo</returns>
        private DialogueInfo[] LoadDialogues()
        {
            return Resources.LoadAll<DialogueInfo>($"Dialogues/{SceneManager.GetActiveScene().name}");
        }
    }
}
