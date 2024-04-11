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
        [Header("Components")] 
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Animator portraitAnimator;
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        
        [Header("Choice buttons")]
        [SerializeField] private List<GameObject> choices;
        private List<TextMeshProUGUI> _choicesText;

        //private DialogueSO _currentDialogue;
        //private SentenceSO _currentSentence;

        private const string SPEAKER_TAG = "speaker";
        private const string PORTRAIT_TAG = "portrait";
        
        private Story _currentStory;
        private bool _isDialoguePlaying;
        private bool _choicesAreActive;
        
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
            _isDialoguePlaying = false;
            _choicesAreActive = false;
            dialoguePanel.SetActive(false);
            foreach (var choice in choices)
            {
                _choicesText.Add(choice.GetComponentInChildren<TextMeshProUGUI>());
            }
        }
        public void Update()
        {
            if (_isDialoguePlaying && Input.GetButtonDown("Fire1") && !_choicesAreActive)
            {
                ContinueDialogue();
            }
        }
        
        private void StartDialogue(TextAsset inkJson)
        {
            _currentStory = new Story(inkJson.text);
            _isDialoguePlaying = true;

            speakerName.text = "???";
            portraitAnimator.Play("none");
            
            dialoguePanel.SetActive(true);
            InterfaceEvents.OpenInterface(gameObject.name);
            ContinueDialogue();
        }

        private void ContinueDialogue()
        {
            if (_currentStory.canContinue)
            {
                dialogueText.text = _currentStory.Continue();
                DisplayChoices();
                HandleTags(_currentStory.currentTags);
            }
            else
            {
                EndDialogue();
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
        
        private void EndDialogue()
        {
            _isDialoguePlaying = false;
            dialoguePanel.SetActive(false);
            InterfaceEvents.CloseInterface();
            dialogueText.text = "";
        }
        
        private void DisplayChoices()
        {
            List<Choice> currentChoices = _currentStory.currentChoices;
            _choicesAreActive = currentChoices.Count > 0;
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
        
        public void MakeChoice(int choiceIndex)
        {
            _currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueDialogue();
        }
        
        
        // private void StartDialogue(Dialogue dialogue)
        // {
        //     dialoguePanel.SetActive(true);
        //     InterfaceEvents.OpenInterface(gameObject.name);
        //     _currentDialogue = dialogue.Info;
        //     SetSentence(_currentDialogue.firstSentence);
        // }
        
        // public void Update()
        // {
        //     if (dialoguePanel.activeSelf && Input.GetButtonDown("Fire1") && _currentSentence.nextSentences.Count < 2)
        //     {
        //         if (_currentSentence.nextSentences.Count == 0)
        //         {
        //             if (_currentDialogue.questProgress != DialogueQuestProgress.None)
        //             {
        //                 if (_currentDialogue.questProgress == DialogueQuestProgress.Start)
        //                 {
        //                     QuestEvents.StartQuest(_currentDialogue.relatedQuest.id);
        //                 }
        //                 else if (_currentDialogue.questProgress == DialogueQuestProgress.Finish)
        //                 {
        //                     QuestEvents.FinishQuest(_currentDialogue.relatedQuest.id);
        //                 }
        //             }
        //     
        //             dialoguePanel.SetActive(false);
        //             InterfaceEvents.CloseInterface();
        //             DialogueEvents.FinishDialogue(_currentDialogue);
        //         }
        //         else
        //         {
        //             SetSentence(_currentSentence.nextSentences[0]);
        //         }
        //     }
        // }

        // public void SetSentence(SentenceSO sentence)
        // {
        //     _currentSentence = sentence;
        //     DisplayChoices();
        //     speakerPortrait.sprite = _currentSentence.portrait;
        //     speakerName.text = _currentSentence.speakerName;
        //     dialogueText.text = _currentSentence.text;
        //
        //     if (_currentSentence.flagToTrigger is not null)
        //     {
        //         DialogueEvents.TriggerDialogueMarker(_currentSentence.flagToTrigger.id);
        //     }
        // }

        // private void DisplayChoices()
        // {
        //     int index = 0;
        //     foreach (var answer in _currentSentence.answers)
        //     {
        //         choices[index].SetActive(true);
        //         _choicesText[index].text = answer;
        //         index++;
        //     }
        //
        //     for (int i = index; i < choices.Count; i++)
        //     {
        //         choices[i].SetActive(false);
        //     }
        // }

        // public void MakeChoice(int choiceIndex)
        // {
        //     SetSentence(_currentSentence.nextSentences[choiceIndex]);
        // }
    }
}
