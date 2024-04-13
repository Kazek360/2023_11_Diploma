using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem.Events;
using Ink.Runtime;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace DialogueSystem.Ink_Rework
{
    //Class managing the currently triggered dialogue.
    public class DialogueUI : MonoBehaviour
    {
        [Header("Params")] 
        [SerializeField] private float typingSpeed;
        
        [Header("Components")] 
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Animator portraitAnimator;
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        
        [Header("Choice buttons")]
        [SerializeField] private List<GameObject> choices;
        private List<TextMeshProUGUI> _choicesText;

        //private DialogueSO _currentDialogue;
        private Story _currentStory;
        private Coroutine _displayLineCoroutine;
        
        private bool _dialoguePlaying;
        private bool _canContinueToNextLine;
        private bool _submitButtonPressedThisFrame;
        
        private const string SPEAKER_TAG = "speaker";
        private const string PORTRAIT_TAG = "portrait";
        
        private void OnEnable()
        {
            DialogueEvents.OnDialogueStarted += StartDialogue;
        }

        private void OnDisable()
        {
            DialogueEvents.OnDialogueStarted -= StartDialogue;
        }
        private void Start()
        {
            _choicesText = new List<TextMeshProUGUI>();
            _dialoguePlaying = false;
            _canContinueToNextLine = false;
            _submitButtonPressedThisFrame = false;
            dialoguePanel.SetActive(false);
            foreach (var choice in choices)
            {
                _choicesText.Add(choice.GetComponentInChildren<TextMeshProUGUI>());
            }
        }
        public void Update()
        { 
            if (Input.GetButtonDown("Fire1"))
            {
                _submitButtonPressedThisFrame = true;
            }
            if (!_dialoguePlaying)
            {
                return;
            }
            if (_canContinueToNextLine && _currentStory.currentChoices.Count == 0 && _submitButtonPressedThisFrame)
            {
                _submitButtonPressedThisFrame = false;
                ContinueDialogue();
            }
        }
        
        private void StartDialogue(TextAsset inkJson)
        {
            _currentStory = new Story(inkJson.text);
            _dialoguePlaying = true;
            
            dialoguePanel.SetActive(true);
            speakerName.text = "???";
            portraitAnimator.Play("none");
            
            InterfaceEvents.OpenInterface(gameObject.name);
            ContinueDialogue();
        }

        private void ContinueDialogue()
        {
            if (_currentStory.canContinue)
            {
                if (_displayLineCoroutine is not null)
                {
                    StopCoroutine(_displayLineCoroutine);
                }
                _displayLineCoroutine = StartCoroutine(DisplayLine(_currentStory.Continue()));
                HandleTags(_currentStory.currentTags);
            }
            else
            {
                EndDialogue();
            }
        }
        
        private void EndDialogue()
        {
            _dialoguePlaying = false;
            dialoguePanel.SetActive(false);
            InterfaceEvents.CloseInterface();
            dialogueText.text = "";
        }
        
        private IEnumerator DisplayLine(string line)
        {
            dialogueText.text = "";
            HideChoices();
            _canContinueToNextLine = false;

            var addingRichTextTag = false;
            
            foreach (var letter in line.ToCharArray())
            {
                if (_submitButtonPressedThisFrame)
                {
                    _submitButtonPressedThisFrame = false;
                    dialogueText.text = line;
                    break;
                }

                if (letter == '<' || addingRichTextTag)
                {
                    addingRichTextTag = true;
                    dialogueText.text += letter;
                    if (letter == '>')
                    {
                        addingRichTextTag = false;
                    }
                }
                else
                {
                    dialogueText.text += letter;
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
                    default:
                        Debug.LogWarning($"Tag Cannot be handled: {currentTag}");
                        break;
                }
            }
        }
    }
}
