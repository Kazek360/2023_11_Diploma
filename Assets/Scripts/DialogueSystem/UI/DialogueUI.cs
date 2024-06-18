using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem.Managers;
using DialogueSystem.ScriptableObjects;
using Ink.Runtime;
using InputSystem;
using SaveSystem;
using SaveSystem.Data;

namespace DialogueSystem.UI
{
    //Class managing the currently triggered dialogue.
    public class DialogueUI : MonoBehaviour, IDataPersistence
    {
        [Header("Params")] 
        [SerializeField] private float typingSpeed;

        [Header("Load Globals JSON")] 
        [SerializeField] private TextAsset loadGlobalsJSON;
        
        [Header("Components")] 
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Animator portraitAnimator;
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        
        [Header("Choice buttons")]
        [SerializeField] private List<GameObject> choices;
        private List<TextMeshProUGUI> _choicesText;

        private DialogueSO _currentDialogue;
        private Story _currentStory;
        private Coroutine _displayLineCoroutine;
        
        private bool _canContinueToNextLine;
        private bool _submitButtonPressedThisLine;
        
        private const string SPEAKER_TAG = "speaker";
        private const string PORTRAIT_TAG = "portrait";

        private static DialogueVariables _dialogueVariables;
        
        private void Awake()
        {
            GameConfig.DialogueUI = this;  
            _dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        }
        
        private void OnEnable()
        {
            DialogueEvents.OnDialogueStarted += StartDialogue;
            InputEvents.OnDialogueContinueInput += ContinueDialogue;
        }

        private void OnDisable()
        {
            DialogueEvents.OnDialogueStarted -= StartDialogue;
            InputEvents.OnDialogueContinueInput -= ContinueDialogue;
        }
        
        private void Start()
        {
            _choicesText = new List<TextMeshProUGUI>();
            _canContinueToNextLine = false;
            _submitButtonPressedThisLine = false;
            dialoguePanel.SetActive(false);
            foreach (var choice in choices)
            {
                _choicesText.Add(choice.GetComponentInChildren<TextMeshProUGUI>());
            }
        }
        
        private void StartDialogue(DialogueSO dialogueInfo)
        {
            GameConfig.QuestTracker.DisableText();
            _currentDialogue = dialogueInfo;
            _currentStory = new Story(_currentDialogue.dialogueText.text);
            _canContinueToNextLine = true;
            _dialogueVariables.StartListening(_currentStory);
            
            dialoguePanel.SetActive(true);
            speakerName.text = "???";
            portraitAnimator.Play("none");
            ContinueDialogue();
        }

        private void ContinueDialogue()
        {
            if (!_canContinueToNextLine || _currentStory.currentChoices.Count > 0)
            {
                if (!_submitButtonPressedThisLine)
                {
                    _submitButtonPressedThisLine = true;
                }
                return;
            }
            if (_currentStory.canContinue)
            {
                if (_displayLineCoroutine is not null)
                {
                    StopCoroutine(_displayLineCoroutine);
                }
                var nextLine = _currentStory.Continue();
                _submitButtonPressedThisLine = false;
                if (nextLine.Equals("") && !_currentStory.canContinue)
                {
                    EndDialogue();
                }
                else
                {
                    HandleTags(_currentStory.currentTags);
                    _displayLineCoroutine = StartCoroutine(DisplayLine(nextLine)); 
                }
            }
            else
            {
                EndDialogue();
            }
        }
        
        private void EndDialogue()
        {
            GameConfig.QuestTracker.EnableText();
            _dialogueVariables.StopListening(_currentStory);
            dialoguePanel.SetActive(false);
            DialogueEvents.FinishDialogue(_currentDialogue);
            dialogueText.text = "";
        }
        
        private IEnumerator DisplayLine(string line)
        {
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;
            HideChoices();
            _canContinueToNextLine = false;
            var addingRichTextTag = false;
            
            foreach (var letter in line.ToCharArray())
            {
                if (_submitButtonPressedThisLine)
                {
                    _submitButtonPressedThisLine = false;
                    dialogueText.maxVisibleCharacters = line.Length;
                    break;
                }
                if (letter == '<' || addingRichTextTag)
                {
                    addingRichTextTag = true;
                    if (letter == '>')
                    {
                        addingRichTextTag = false;
                    }
                }
                else
                {
                    dialogueText.maxVisibleCharacters++;
                    yield return new WaitForSeconds(typingSpeed);
                }
            }
            DisplayChoices();
            _canContinueToNextLine = true;
        }
        
        private void DisplayChoices()
        {
            List<Choice> currentChoices = _currentStory.currentChoices;
            if (currentChoices.Count > choices.Count)
            {
                Debug.LogError("More choices were given than UI can support");
            }
            
            var index = 0;
            foreach (var choice in currentChoices)
            {
                choices[index].SetActive(true);
                _choicesText[index].text = choice.text;
                index++;
            }
            
            for (var i = index; i < choices.Count; i++)
            {
                choices[i].SetActive(false);
            }
        }

        private void HideChoices()
        {
            foreach (var choice in choices)
            {
                choice.SetActive(false);
            }
        }
        
        public void MakeChoice(int choiceIndex)
        {
            if (_canContinueToNextLine)
            {
                _currentStory.ChooseChoiceIndex(choiceIndex);
                ContinueDialogue(); 
            }
        }
        
        private void HandleTags(List<string> currentTags)
        {
            foreach (var currentTag in currentTags)
            {
                string[] splitTag = currentTag.Split(":");
                if (splitTag.Length != 2)
                {
                    Debug.LogError($"Tag could not be parsed: {currentTag}");
                }
                var tagKey = splitTag[0].Trim();
                var tagValue = splitTag[1].Trim();

                switch (tagKey)
                {
                    case SPEAKER_TAG:
                        speakerName.text = tagValue;
                        break;
                    case PORTRAIT_TAG:
                        portraitAnimator.Play(tagValue);
                        break;
                }
            }
        }

        public Ink.Runtime.Object GetVariableState(string variableName)
        {
            Ink.Runtime.Object variableValue;
            _dialogueVariables._variablesDictionary.TryGetValue(variableName, out variableValue);
            if (variableValue == null)
            {
                Debug.LogWarning($"Ink Variable was not found: {variableName}");
            }

            return variableValue;
        }
        
        public void SetVariableState(string variableName, Ink.Runtime.Object variableValue)
        {
            if (_dialogueVariables._variablesDictionary.ContainsKey(variableName))
            {
                var globalStory = new Story(loadGlobalsJSON.text);
                _dialogueVariables.StartListening(globalStory);
                globalStory.variablesState.SetGlobal(variableName, variableValue);
                _dialogueVariables.StopListening(globalStory);
            }
        }

        public void LoadData(GameData gameData)
        {
            if (gameData.inkVariables is null)
            {
                return;
            }
        
            _dialogueVariables.LoadVariables(loadGlobalsJSON, gameData.inkVariables);
        }
        
        public void SaveData(ref GameData gameData)
        {
            gameData.inkVariables = _dialogueVariables.SaveVariables();
        }
    }
}
