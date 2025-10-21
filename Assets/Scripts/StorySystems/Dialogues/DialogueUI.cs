using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using InputSystem;
using Quests;
using UI;
using UI.Interfaces;

namespace StorySystems.Dialogues
{
    //Class managing the currently triggered dialogue.
    public class DialogueUI : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.Dialogue;
        
        [Header("Params")] 
        [SerializeField] private float typingDelay;
        
        [Header("Components")] 
        [SerializeField] private Animator portraitAnimator;
        [SerializeField] private Animator backgroundAnimator;
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        
        [Header("Choice buttons")]
        [SerializeField] private List<GameObject> choices;
        private List<TextMeshProUGUI> _choicesText;

        
        private TextAsset _currentDialogue;
        private string _currentDialogueTrigger;
        
        private Story _currentStory;
        private Coroutine _displayLineCoroutine;
        
        private bool _canContinueToNextLine;
        private bool _submitButtonPressedThisLine;

        private InkExternalFunctions _inkExternalFunctions;
        
        private const string SPEAKER_TAG = "speaker";
        private const string PORTRAIT_TAG = "portrait";
        private const string BACKGROUND_TAG = "background";
        
        protected override void OnEnable()
        {
            base.OnEnable();
            DialogueEvent.OnDialogueStarted += StartDialogue;
            DialogueEvent.OnDialogueContinued += ContinueDialogue;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            DialogueEvent.OnDialogueStarted -= StartDialogue;
            DialogueEvent.OnDialogueContinued -= ContinueDialogue;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            InputManager.Instance.DisableInputs();
            InputManager.Instance.Controlls.UI.Disable();
            InputManager.Instance.Controlls.Story.Enable();   
        }
        
        public override void OnExit()
        {
            base.OnExit();
            InputManager.Instance.EnableInputs();
            InputManager.Instance.Controlls.UI.Enable();
            InputManager.Instance.Controlls.Story.Disable();   
        }
        
        private void Start()
        {
            _inkExternalFunctions = new InkExternalFunctions();
            _choicesText = new List<TextMeshProUGUI>();
            _canContinueToNextLine = false;
            _submitButtonPressedThisLine = false;
            foreach (var choice in choices)
            {
                _choicesText.Add(choice.GetComponentInChildren<TextMeshProUGUI>());
            }
        }
        
        private void StartDialogue(string triggerName, TextAsset dialogue)
        {
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Dialogue);

            _currentDialogueTrigger = triggerName;
            _currentDialogue = dialogue;
            _currentStory = new Story(dialogue.text);
            
            _canContinueToNextLine = true;
            QuestManager.Instance.FlagManager.StartListening(_currentStory);
            _inkExternalFunctions.Bind(_currentStory);
            speakerName.text = "???";
            portraitAnimator.Play("none");
            backgroundAnimator.Play("none");
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
            _inkExternalFunctions.Unbind(_currentStory);
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
            QuestManager.Instance.FlagManager.StopListening(_currentStory);
            
            DialogueEvent.FinishDialogue();
            DialogueEvent.FinishQuestDialogue(_currentDialogueTrigger);
            
            dialogueText.text = "";
            portraitAnimator.Play("none");
            backgroundAnimator.Play("none");
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
                    yield return new WaitForSeconds(typingDelay);
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
                    case BACKGROUND_TAG:
                        backgroundAnimator.Play(tagValue);
                        break;
                }
            }
        }
    }
}
